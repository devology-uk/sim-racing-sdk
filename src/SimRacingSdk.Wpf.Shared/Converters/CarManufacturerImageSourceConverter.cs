using System.Globalization;
using System.Windows.Data;

namespace SimRacingSdk.Wpf.Shared.Converters;

public class CarManufacturerImageSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var stringValue = (string)value;
        if(string.IsNullOrWhiteSpace(stringValue))
        {
            return MissingImagePlaceholder.Source;
        }

        stringValue = stringValue.Replace(' ', '-')
                                 .ToLowerInvariant();
        return PackResourceImageLoader.LoadOrPlaceholder($"Images/Manufacturers/{stringValue}.png");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
