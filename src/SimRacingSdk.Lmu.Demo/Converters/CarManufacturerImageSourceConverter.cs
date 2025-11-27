using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SimRacingSdk.Lmu.Demo.Converters;

public class CarManufacturerImageSourceConverter: IValueConverter
{
    public object Convert(object value,
                          Type targetType,
                          object parameter,
                          CultureInfo culture)
    {
        var stringValue = (string)value;
        if(string.IsNullOrWhiteSpace(stringValue))
        {
            return new
                BitmapImage(new
                                Uri("pack://application:,,,/SimRacingSdk.Lmu.Demo;component/Images/delete.png",
                                    UriKind.Absolute));
        }

        stringValue = stringValue.Replace(' ', '-')
                                 .ToLowerInvariant();
        return new
            BitmapImage(new
                            Uri($@"pack://application:,,,/SimRacingSdk.Lmu.Demo;component/Images/Manufacturers/{stringValue}.png",
                                UriKind.Absolute));
    }

    public object ConvertBack(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}