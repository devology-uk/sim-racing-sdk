using SimRacingSdk.Lmu.Rest.Abstractions;
using SimRacingSdk.Lmu.Rest.Models;

namespace SimRacingSdk.Lmu.Rest;

// Curated from docs/sdk/lmu-rest-swagger.json (191 operations). Left out on purpose: anything that writes
// setups, options, save files, profile or livery data, joins or leaves multiplayer, or starts or ends a session,
// plus binary downloads and the auth ticket. Regex routes are rewritten as named route values.
public class LmuRestEndpointCatalog : ILmuRestEndpointCatalog
{
    private const string LiveSession = "Live session";
    private const string Strategy = "Strategy and fuel";
    private const string CarState = "Car state";
    private const string Setups = "Setups";
    private const string ReferenceData = "Reference data";
    private const string CameraAndReplay = "Camera and replay";
    private const string Hud = "HUD";
    private const string GameAndProfile = "Game and profile";

    public IReadOnlyList<LmuRestEndpoint> All { get; } =
    [
        Read(LiveSession, "/rest/watch/sessionInfo"),
        Read(LiveSession, "/rest/watch/standings"),
        Read(LiveSession, "/rest/watch/standings/history"),
        Read(LiveSession, "/rest/sessions/GetGameState"),
        Read(LiveSession, "/rest/sessions/GetSessionsInfoForEvent"),
        Read(LiveSession, "/rest/sessions/"),
        Read(LiveSession, "/rest/sessions/amount"),
        Read(LiveSession, "/rest/sessions/weather"),
        Read(LiveSession, "/rest/sessions/opponents"),
        Read(LiveSession, "/rest/sessions/opponents/all"),
        Read(LiveSession, "/rest/sessions/restartStintAvailable"),
        Read(LiveSession, "/rest/race/getAllowedToStartRacing"),
        Read(LiveSession, "/rest/chat/"),

        Read(Strategy, "/rest/strategy/usage"),
        Read(Strategy, "/rest/strategy/overall"),
        Read(Strategy, "/rest/strategy/pitstop-estimate"),

        Read(CarState, "/rest/garage/tireinfo"),
        Read(CarState, "/rest/garage/brakeinfo"),
        RealtimeRead(CarState, LmuRestPaths.VehicleCondition),
        Read(CarState, "/rest/garage/getPlayerGarageData"),
        Read(CarState, "/rest/garage/summary"),
        Read(CarState, "/rest/garage/isRefreshInProgress"),
        Read(CarState, "/rest/garage/PitMenu/receivePitMenu"),
        Read(CarState, "/rest/garage/UIScreen/CarSetupOverview"),
        Read(CarState, "/rest/garage/UIScreen/CoopOverview"),
        Read(CarState, "/rest/garage/UIScreen/DriverHandOffStintEnd"),
        Read(CarState, "/rest/garage/UIScreen/DriverHandOffStintStart"),
        Read(CarState, "/rest/garage/UIScreen/PitCarReview"),
        Read(CarState, "/rest/garage/UIScreen/RepairAndRefuel"),
        Read(CarState, "/rest/garage/UIScreen/SessionSetup"),
        Read(CarState, "/rest/garage/UIScreen/TireManagement"),

        Read(Setups, "/rest/garage/setup"),
        Read(Setups, "/rest/garage/showOnlyRelevantSetups"),
        ParameterisedRead(Setups, "/rest/garage/setup/notes/{setup}"),

        Read(ReferenceData, "/rest/race/car"),
        Read(ReferenceData, "/rest/race/track"),
        Read(ReferenceData, "/rest/watch/trackmap"),
        Read(ReferenceData, "/rest/sessions/getAllVehicles"),
        Read(ReferenceData, "/rest/sessions/getTracksAll"),
        Read(ReferenceData, "/rest/sessions/getTracksInSeries"),
        ParameterisedRead(ReferenceData, "/rest/race/track/{id}/trackmap"),

        Read(CameraAndReplay, "/rest/watch/focus"),
        Read(CameraAndReplay, "/rest/replay/CameraController/getCameraInfo"),
        Read(CameraAndReplay, "/rest/replay/isActive"),
        Read(CameraAndReplay, "/rest/watch/getBookmarkedTimestamps"),
        Read(CameraAndReplay, "/rest/watch/replays"),
        Read(CameraAndReplay, "/rest/watch/replay/getReplayFolder"),
        ParameterisedRead(CameraAndReplay, "/rest/watch/getIncidentsList/{minTimeBetweenContacts}"),
        Action(CameraAndReplay, HttpMethod.Put, "/rest/watch/focus/{slotid}"),
        Action(CameraAndReplay, HttpMethod.Put, "/rest/watch/focusForward"),
        Action(CameraAndReplay, HttpMethod.Put, "/rest/watch/focusBackward"),
        Action(CameraAndReplay, HttpMethod.Put, "/rest/watch/focus/{cameraType}/{trackSideGroup}/{shouldAdvance}"),
        Action(CameraAndReplay, HttpMethod.Post, "/rest/replay/CameraController/setCameraByName/{cameraName}"),
        Action(CameraAndReplay, HttpMethod.Post, "/rest/replay/CameraController/setCamera"),
        Action(CameraAndReplay, HttpMethod.Post, "/rest/replay/toggleactive"),
        Action(CameraAndReplay, HttpMethod.Put, "/rest/watch/replaytime/{time}"),
        Action(CameraAndReplay, HttpMethod.Put, "/rest/watch/replayCommand/{command}"),
        Action(CameraAndReplay, HttpMethod.Post, "/rest/watch/replay/setReplayUIVisible"),
        Action(CameraAndReplay, HttpMethod.Get, "/rest/watch/play/{id}"),
        Action(CameraAndReplay, HttpMethod.Post, "/rest/sessions/returnToMonitor"),

        Read(Hud, "/rest/hud"),
        Action(Hud, HttpMethod.Post, "/rest/hud/toggle/{component}"),
        Action(Hud, HttpMethod.Post, "/rest/hud/toggleAllComponents/{visible}"),
        Action(Hud, HttpMethod.Post, "/rest/sessions/setHudOnWatchScreen"),

        Read(GameAndProfile, "/navigation/state"),
        Read(GameAndProfile, "/navigation/GetLoadingScreen"),
        Read(GameAndProfile, "/rest/multiplayer/join/state"),
        Read(GameAndProfile, "/rest/multiplayer/steam/status"),
        Read(GameAndProfile, "/rest/multiplayer/teams"),
        Read(GameAndProfile, "/rest/profile/eacActive"),
        Read(GameAndProfile, "/rest/profile/inDevMode")
    ];

    public IEnumerable<LmuRestEndpoint> RealtimeReads =>
        this.All.Where(endpoint => endpoint.Kind == LmuRestEndpointKind.RealtimeRead);

    public IEnumerable<LmuRestEndpoint> SafeReads => this.All.Where(endpoint => endpoint.Kind == LmuRestEndpointKind.SafeRead);

    private static LmuRestEndpoint Read(string group, string path)
    {
        return new LmuRestEndpoint(group, HttpMethod.Get, path, LmuRestEndpointKind.SafeRead);
    }

    private static LmuRestEndpoint RealtimeRead(string group, string path)
    {
        return new LmuRestEndpoint(group, HttpMethod.Get, path, LmuRestEndpointKind.RealtimeRead);
    }

    private static LmuRestEndpoint ParameterisedRead(string group, string path)
    {
        return new LmuRestEndpoint(group, HttpMethod.Get, path, LmuRestEndpointKind.ParameterisedRead);
    }

    private static LmuRestEndpoint Action(string group, HttpMethod method, string path)
    {
        return new LmuRestEndpoint(group, method, path, LmuRestEndpointKind.Action);
    }
}
