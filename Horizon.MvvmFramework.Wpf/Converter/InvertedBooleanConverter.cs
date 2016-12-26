using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Horizon.MvvmFramework.Wpf.Converter
{
    /// <summary>
    /// Converter which inverts a boolean value.
    /// </summary>
    public class InvertedBooleanConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var booleanValue = value as bool?;

            if (!booleanValue.HasValue)
            {
                return DependencyProperty.UnsetValue;
            }
            return !booleanValue.Value;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}