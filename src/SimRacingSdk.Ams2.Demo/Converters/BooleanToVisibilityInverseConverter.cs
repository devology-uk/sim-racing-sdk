﻿using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SimRacingSdk.Ams2.Demo.Converters;

public class BooleanToVisibilityInverseConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value is bool booleanValue)
        {
            return booleanValue? Visibility.Collapsed: Visibility.Visible;
        }

        return Visibility.Visible;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}