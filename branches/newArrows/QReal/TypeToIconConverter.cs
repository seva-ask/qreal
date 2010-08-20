using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace QReal
{
    public class TypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BitmapImage img = new BitmapImage(new Uri("../demo.png", UriKind.Relative));
            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new InvalidOperationException("Should not be called");
        }
    }
}
