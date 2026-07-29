using System.Globalization;
using System.Windows.Data;

namespace SimRacingSdk.Wpf.Shared.Converters;

public class FlagImageSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var stringValue = (string)value;
        if(string.IsNullOrWhiteSpace(stringValue))
        {
            return MissingImagePlaceholder.Source;
        }

        return PackResourceImageLoader.LoadOrPlaceholder($"Images/Flags/{stringValue}.png");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
