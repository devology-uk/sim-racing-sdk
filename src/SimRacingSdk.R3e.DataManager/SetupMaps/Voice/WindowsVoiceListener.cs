using System.Globalization;
using System.Speech.Recognition;

namespace SimRacingSdk.R3e.DataManager.SetupMaps.Voice;

// An in-process recogniser rather than the shared Windows one, so it keeps listening while the game has focus.
public sealed class WindowsVoiceListener : IVoiceListener, IDisposable
{
    private static readonly string[] PreferredCultures = ["en-GB", "en-US"];

    private SpeechRecognitionEngine? engine;
    private SynchronizationContext? uiContext;

    public event EventHandler<HeardPhrase>? Heard;

    public void Start(Func<CultureInfo, Grammar> buildGrammar)
    {
        if(this.engine is not null)
        {
            return;
        }

        var recogniser = ChooseRecogniser()
                         ?? throw new VoiceUnavailableException("No English speech recogniser is installed. Add English (United Kingdom) speech in Windows' Language settings.");
        this.uiContext = SynchronizationContext.Current;
        this.engine = this.StartEngine(recogniser, buildGrammar);
    }

    public void Stop()
    {
        if(this.engine is null)
        {
            return;
        }

        this.engine.SpeechRecognized -= this.OnSpeechRecognized;
        this.engine.RecognizeAsyncCancel();
        this.engine.Dispose();
        this.engine = null;
    }

    public void Dispose()
    {
        this.Stop();
    }

    private static RecognizerInfo? ChooseRecogniser()
    {
        var installed = SpeechRecognitionEngine.InstalledRecognizers();
        return PreferredCultures
                   .Select(culture => installed.FirstOrDefault(recogniser => recogniser.Culture.Name == culture))
                   .FirstOrDefault(recogniser => recogniser is not null)
               ?? installed.FirstOrDefault(recogniser => recogniser.Culture.TwoLetterISOLanguageName == "en");
    }

    private SpeechRecognitionEngine StartEngine(RecognizerInfo recogniser, Func<CultureInfo, Grammar> buildGrammar)
    {
        var newEngine = new SpeechRecognitionEngine(recogniser);
        try
        {
            newEngine.LoadGrammar(buildGrammar(recogniser.Culture));
            newEngine.SetInputToDefaultAudioDevice();
            newEngine.SpeechRecognized += this.OnSpeechRecognized;
            newEngine.RecognizeAsync(RecognizeMode.Multiple);
            return newEngine;
        }
        catch(Exception exception) when(exception is InvalidOperationException or ArgumentException or FormatException)
        {
            newEngine.Dispose();
            throw new VoiceUnavailableException($"Voice entry couldn't start: {exception.Message} If it's about the audio device, check a microphone is connected and set as the default recording device.", exception);
        }
    }

    private static HeardPhrase ToHeardPhrase(RecognitionResult result)
    {
        var words = result.Words.Select(word => word.Text).ToList();
        var carName = result.Semantics.ContainsKey(VoiceWords.CarKey) ? result.Semantics[VoiceWords.CarKey].Value?.ToString() : null;
        var startedAt = DateTime.Now - (result.Audio?.Duration ?? TimeSpan.Zero);
        return new HeardPhrase(words, result.Confidence, carName, startedAt);
    }

    private void OnSpeechRecognized(object? sender, SpeechRecognizedEventArgs e)
    {
        var heard = ToHeardPhrase(e.Result);
        if(this.uiContext is null)
        {
            this.Heard?.Invoke(this, heard);
            return;
        }

        this.uiContext.Post(state => this.Heard?.Invoke(this, heard), null);
    }
}
