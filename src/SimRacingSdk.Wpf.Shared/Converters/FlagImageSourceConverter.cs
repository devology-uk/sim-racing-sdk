using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SimRacingSdk.Wpf.Shared.Converters;

public class FlagImageSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var stringValue = (string)value;
        if(string.IsNullOrWhiteSpace(stringValue))
        {
            return new BitmapImage(new Uri(
                "pack://application:,,,/SimRacingSdk.Wpf.Shared;component/Images/delete.png",
                UriKind.Absolute));
        }

        return new BitmapImage(new Uri(
            $@"pack://application:,,,/SimRacingSdk.Wpf.Shared;component/Images/Flags/{stringValue}.png",
            UriKind.Absolute));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
