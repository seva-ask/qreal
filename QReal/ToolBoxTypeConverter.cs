using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using ObjectTypes;

namespace QReal
{
    public class ToolBoxTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter.ToString() == "Name")
            {
                Type type = value as Type;
                return (Activator.CreateInstance(type) as ObjectType).TypeName;
            }
            else
            {
                BitmapImage img = new BitmapImage(new Uri("../demo.png", UriKind.Relative));
                return img;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new InvalidOperationException("Should not be called");
        }
    }
}
