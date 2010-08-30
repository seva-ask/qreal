using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ObjectTypes;
using QReal.Types;
using System.Windows.Shapes;

namespace QReal
{
    public class TypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Type type;
            if (parameter.ToString() == "TypeName")
            {
                type = TypesManager.Instance.GetType(value.ToString());
            }
            else
            {
                type = value as Type;
            }
            return TypesManager.Instance.GetIcon(type);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new InvalidOperationException("Should not be called");
        }
    }
}
