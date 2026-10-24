namespace SimRacingSdk.Lmu.Rest.Models;

public record LmuRestWeatherNode
{
    public int Duration { get; init; }
    public int Humidity { get; init; }
    public int RainChance { get; init; }
    public int Sky { get; init; }
    public int StartTime { get; init; }
    public int Temperature { get; init; }
    public int WindDirection { get; init; }
    public int WindSpeed { get; init; }
}
