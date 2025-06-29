using System.Xml.Linq;
using SimRacingSdk.Lmu.Core.Abstractions;

namespace SimRacingSdk.Lmu.Core.Models;

public class LmuDrivingSession
{
    public int DateTimeValue { get; set; }
    public int FormationAndStart { get; set; }
    public int Laps { get; set; }
    public int Minutes { get; set; }
    public int MostLapsCompleted { get; set; }
    public string? SessionName { get; set; }
    public string? SessionType { get; set; }
    public string? TimeString { get; set; }

    public IList<LmuStreamEvent> Stream { get; } = new List<LmuStreamEvent>();
    public IList<LmuDriver> Drivers { get; } = new List<LmuDriver>();
}