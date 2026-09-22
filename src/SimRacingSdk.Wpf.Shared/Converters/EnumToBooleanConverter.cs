using System.Globalization;
using System.Windows.Data;

namespace SimRacingSdk.Wpf.Shared.Converters;

// Binds a RadioButton's IsChecked to one member of an enum-typed property - Convert compares the
// bound value against ConverterParameter by string, ConvertBack parses the enum back from
// targetType (the source property's own type), so one converter instance works for any enum.
public class EnumToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value?.ToString() == parameter?.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is true ? Enum.Parse(targetType, parameter!.ToString()!) : Binding.DoNothing;
    }
}
