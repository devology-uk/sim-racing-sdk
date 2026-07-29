using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace SimRacingSdk.Wpf.Shared.Converters;

public static class MissingImagePlaceholder
{
    public static ImageSource Source { get; } = Create();

    static ImageSource Create()
    {
        var bounds = new Rect(0, 0, 48, 48);
        var glyph = new FormattedText(
            "?",
            CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight,
            new Typeface("Segoe UI"),
            28,
            Brushes.DimGray,
            96);

        var drawingGroup = new DrawingGroup();
        using(var context = drawingGroup.Open())
        {
            context.DrawRectangle(Brushes.WhiteSmoke, new Pen(Brushes.Silver, 1), bounds);
            context.DrawText(glyph, new Point(
                (bounds.Width - glyph.Width) / 2,
                (bounds.Height - glyph.Height) / 2));
        }

        var image = new DrawingImage(drawingGroup);
        image.Freeze();
        return image;
    }
}
