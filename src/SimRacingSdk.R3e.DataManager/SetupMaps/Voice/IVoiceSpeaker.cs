namespace SimRacingSdk.R3e.DataManager.SetupMaps.Voice;

public interface IVoiceSpeaker
{
    void Say(string text);

    // True when a reply was playing at any point since the moment, so the recogniser hearing it can be ignored.
    bool WasSpeakingSince(DateTime moment);
}
