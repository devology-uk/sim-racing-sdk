#nullable disable

namespace SimRacingSdk.Lmu.Core.Models
{
    public record LmuTrackInfo
    {
        public string Country { get; init; }
        public string CountryCode { get; init; }
        public double Latitude { get; init; }
        public IList<LmuTrackLayoutInfo> Layouts { get; init; }
        public double Longitude { get; init; }
        public string Name { get; init; }
        public string ShortName { get; init; }
    }
}