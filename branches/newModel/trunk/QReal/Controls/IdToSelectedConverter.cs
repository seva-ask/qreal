using System;
using System.Windows.Data;
using ObjectTypes;

namespace QReal.Controls
{
    public class IdToSelectedConverter : IValueConverter
    {
        private readonly ObjectType myObjectType;

        public IdToSelectedConverter(ObjectType objectType)
        {
            this.myObjectType = objectType;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == myObjectType.DataContext;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
