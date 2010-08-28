using System;
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
                type = TypesHelper.GetType(value.ToString());
            }
            else
            {
                type = value as Type;
            }
            const int iconSize = 35;
            Canvas canvas = new Canvas();
            ObjectType objectType = (Activator.CreateInstance(type)) as ObjectType;
            if (objectType is NodeType)
            {
                double scaleCoefficient = iconSize/Math.Max(objectType.Width, objectType.Height);
                objectType.RenderTransform = new ScaleTransform {ScaleX = scaleCoefficient, ScaleY = scaleCoefficient};
                objectType.SetValue(Canvas.TopProperty, (iconSize - objectType.Height*scaleCoefficient)/2);
                objectType.SetValue(Canvas.LeftProperty, (iconSize - objectType.Width*scaleCoefficient)/2);
            }
            else
            {
                (objectType as EdgeType).X2 = iconSize;
                (objectType as EdgeType).Y2 = iconSize;
            }
            objectType.IsHitTestVisible = false;
            canvas.Children.Add(objectType);
            return canvas;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new InvalidOperationException("Should not be called");
        }
    }
}
