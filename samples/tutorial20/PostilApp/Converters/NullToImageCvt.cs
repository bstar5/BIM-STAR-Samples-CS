using System;
using System.Globalization;
using System.Windows.Data;
using Mango;

namespace PostilApp.Converters
{
    public class NullToImageCvt : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return this.GetAppResPath("Assets/暂无图片.png");
            var url = value.ToString();
            if (string.IsNullOrWhiteSpace(url))
                return this.GetAppResPath("Assets/暂无图片.png");
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}