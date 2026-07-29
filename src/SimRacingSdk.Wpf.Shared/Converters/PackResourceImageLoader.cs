using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SimRacingSdk.Wpf.Shared.Converters;

internal static class PackResourceImageLoader
{
    public static ImageSource LoadOrPlaceholder(string relativePath)
    {
        try
        {
            return new BitmapImage(new Uri(
                $"pack://application:,,,/SimRacingSdk.Wpf.Shared;component/{relativePath}",
                UriKind.Absolute));
        }
        catch(IOException)
        {
            return MissingImagePlaceholder.Source;
        }
    }
}
