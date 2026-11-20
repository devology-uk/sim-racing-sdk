using System.Speech.Synthesis;

namespace SimRacingSdk.R3e.DataManager.SetupMaps.Voice;

public sealed class WindowsVoiceSpeaker : IVoiceSpeaker, IDisposable
{
    private readonly SpeechSynthesizer synthesizer = new();
    private DateTime lastFinishedAt = DateTime.MinValue;

    public WindowsVoiceSpeaker()
    {
        this.synthesizer.SetOutputToDefaultAudioDevice();
        this.synthesizer.SpeakCompleted += this.OnSpeakCompleted;
        this.ChooseBritishVoice();
    }

    public void Say(string text)
    {
        this.synthesizer.SpeakAsyncCancelAll();
        this.synthesizer.SpeakAsync(text);
    }

    public bool WasSpeakingSince(DateTime moment)
    {
        return this.synthesizer.State == SynthesizerState.Speaking || this.lastFinishedAt > moment;
    }

    public void Dispose()
    {
        this.synthesizer.SpeakCompleted -= this.OnSpeakCompleted;
        this.synthesizer.Dispose();
    }

    private void ChooseBritishVoice()
    {
        var britishVoice = this.synthesizer.GetInstalledVoices()
            .FirstOrDefault(voice => voice.Enabled && voice.VoiceInfo.Culture.Name == "en-GB");
        if(britishVoice is not null)
        {
            this.synthesizer.SelectVoice(britishVoice.VoiceInfo.Name);
        }
    }

    private void OnSpeakCompleted(object? sender, SpeakCompletedEventArgs e)
    {
        this.lastFinishedAt = DateTime.Now;
    }
}
