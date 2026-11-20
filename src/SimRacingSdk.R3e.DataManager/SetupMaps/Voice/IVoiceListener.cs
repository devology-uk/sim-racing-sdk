using System.Globalization;
using System.Speech.Recognition;

namespace SimRacingSdk.R3e.DataManager.SetupMaps.Voice;

public interface IVoiceListener
{
    event EventHandler<HeardPhrase>? Heard;

    // Throws VoiceUnavailableException when there's no recogniser or microphone.
    void Start(Func<CultureInfo, Grammar> buildGrammar);

    void Stop();
}
