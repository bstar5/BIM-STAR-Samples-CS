using System;
using System.Globalization;
using System.Windows.Data;

namespace PostilApp.Converters
{
    public class IsEnabledToOpacityCvt : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 0.7 : 0.25;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}