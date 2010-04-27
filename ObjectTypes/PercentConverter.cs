using System;
using System.Windows.Data;

namespace ObjectTypes
{
    public class PercentConverter : IValueConverter
    {
        /// <summary>
        /// Возвращает заданный процент от значения.
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="targetType">double</param>
        /// <param name="parameter">Процент</param>
        /// <param name="culture">not used</param>
        /// <returns>Процент от значения</returns>
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double) value * double.Parse(parameter.ToString()) / 100;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new InvalidOperationException("Should not be called");
        }
    }
}
