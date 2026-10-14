using System.Text;
using SimRacingSdk.Ace.SharedMemory.Models;

namespace SimRacingSdk.Ace.Demo.CameraProbe;

public static class CarTableFormatter
{
    public static string Format(AceGraphicsData graphicsData)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"Car table: {graphicsData.ActiveCars} active cars");
        for(var index = 0; index < graphicsData.ActiveCars; index++)
        {
            builder.AppendLine(FormatRow(graphicsData, index));
        }

        return builder.ToString();
    }

    public static int FindCarIndex(AceGraphicsData graphicsData, AceCarIdPair carId)
    {
        return Array.IndexOf(graphicsData.CarIds, carId);
    }

    private static string FormatRow(AceGraphicsData graphicsData, int index)
    {
        var carId = graphicsData.CarIds[index];
        var coordinate = graphicsData.CarCoordinates[index];
        return $"  [{index}] Id {carId.A}/{carId.B}  X {coordinate.X:F1}  Y {coordinate.Y:F1}  Z {coordinate.Z:F1}"
            + FormatMarkers(graphicsData, carId);
    }

    private static string FormatMarkers(AceGraphicsData graphicsData, AceCarIdPair carId)
    {
        var markers = string.Empty;
        if(carId == graphicsData.FocusedCarId)
        {
            markers += "  <- focused";
        }

        if(carId == graphicsData.PlayerCarId)
        {
            markers += "  <- player";
        }

        return markers;
    }
}
