using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using DarklandsBusinessObjects.Objects;

namespace DarklandsUiCommon.Converters
{
    public class RgbConverter : IMultiValueConverter
    {
        private const float Scale = (float) 255/Rgb.RgbMax;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Any(v => v == DependencyProperty.UnsetValue)
                || !values.All(v => v is int ))
            {
                return DependencyProperty.UnsetValue;
            }

            var red = (byte)((int)values[0] * Scale);
            var green = (byte)((int)values[1] * Scale);
            var blue = (byte)((int)values[2] * Scale);

            var brush = new SolidColorBrush(Color.FromRgb(red, green, blue));

            return brush;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
