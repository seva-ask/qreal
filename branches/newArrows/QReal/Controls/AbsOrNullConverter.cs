using System;
using System.Windows.Data;

namespace QReal.Controls
{
    public class AbsOrNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double source = (double)value;
            return source < 0 ? Math.Abs(source) : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new InvalidOperationException("Should not be called");
        }
    }
}
