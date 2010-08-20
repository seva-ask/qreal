using System;
using System.Windows.Data;

namespace ObjectTypes
{
    public class ArithmeticConverter : IValueConverter
    {
        /// <summary>
        /// Возвращает измененное значения.
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="targetType">double</param>
        /// <param name="parameter">Разница (может быть отрицательной)</param>
        /// <param name="culture">not used</param>
        /// <returns>Значение + разница</returns>
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)value + double.Parse(parameter.ToString());
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new InvalidOperationException("Should not be called");
        }
    }
}
