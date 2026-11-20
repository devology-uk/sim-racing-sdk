namespace SimRacingSdk.R3e.DataManager.SetupMaps.Voice;

// CarName is filled when the phrase named a car, since car names are matched by the recogniser, not parsed.
public record HeardPhrase(IReadOnlyList<string> Words, float Confidence, string? CarName, DateTime StartedAt);
