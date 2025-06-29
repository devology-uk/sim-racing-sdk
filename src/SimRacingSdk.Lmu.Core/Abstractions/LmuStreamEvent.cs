namespace SimRacingSdk.Lmu.Core.Abstractions
{
    public abstract record LmuStreamEvent(double EventTiming, string Message) { }
}