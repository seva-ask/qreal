using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace ObjectTypes
{
    public abstract class EdgeType : ObjectType
    {
        public EdgeType()
        {
            Grid grid = new Grid();
            grid.Name = "LayoutRoot";
            this.Content = grid;
            this.Loaded += new RoutedEventHandler(EdgeType_Loaded);
        }

        private Line mainLine;

        private void EdgeType_Loaded(object sender, RoutedEventArgs e)
        {
            mainLine = new Line();
            mainLine.StrokeThickness = 5;
            mainLine.Stroke = new SolidColorBrush(Colors.Black);

            Binding bindingX2 = new Binding();
            bindingX2.Source = this;
            bindingX2.Path = new PropertyPath("X2");
            bindingX2.Mode = BindingMode.TwoWay;
            bindingX2.Converter = new AbsConverter();
            mainLine.SetBinding(Line.X2Property, bindingX2);

            Binding bindingY2 = new Binding();
            bindingY2.Source = this;
            bindingY2.Path = new PropertyPath("Y2");
            bindingY2.Mode = BindingMode.TwoWay;
            bindingY2.Converter = new AbsConverter();
            mainLine.SetBinding(Line.Y2Property, bindingY2);

            (this.Content as Panel).Children.Add(mainLine);

            Thumb thumb = new Thumb();
            thumb.Width = 7;
            thumb.Height = 7;
            thumb.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            thumb.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            thumb.DragStarted += new DragStartedEventHandler(thumb_DragStarted);
            thumb.DragDelta += new DragDeltaEventHandler(thumb_DragDelta);
            (this.Content as Panel).Children.Add(thumb);
        }

        private void thumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            mouseTransform = this.RenderTransform;
        }

        private GeneralTransform mouseTransform;

        private void thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Point deltaTempPoint = new Point(e.HorizontalChange, e.VerticalChange);
            if (mouseTransform != null)
            {
                Point deltaTempPointTransformed = mouseTransform.Transform(deltaTempPoint);
                X2 += deltaTempPointTransformed.X;
                Y2 += deltaTempPointTransformed.Y;
                return;
            }
            X2 += e.HorizontalChange;
            Y2 += e.VerticalChange;
        }

        public double X2
        {
            get { return (double)GetValue(X2Property); }
            set { SetValue(X2Property, value); }
        }

        public static readonly DependencyProperty X2Property =
            DependencyProperty.Register("X2", typeof(double), typeof(EdgeType), new PropertyMetadata(OnSizePropertyChanged));

        public double Y2
        {
            get { return (double)GetValue(Y2Property); }
            set { SetValue(Y2Property, value); }
        }

        public static readonly DependencyProperty Y2Property =
            DependencyProperty.Register("Y2", typeof(double), typeof(EdgeType), new PropertyMetadata(OnSizePropertyChanged));

        private static void OnSizePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            EdgeType edgeType = obj as EdgeType;
            Transform generalTransform = null;
            if ((edgeType.X2 < 0) && (edgeType.Y2 > 0))
            {
                double angle = 2 * Math.Atan(Math.Abs(edgeType.X2 / edgeType.Y2)) * 180 / Math.PI;
                generalTransform = GetTransform(edgeType, angle, true, false);
            }
            else if ((edgeType.X2 > 0) && (edgeType.Y2 < 0))
            {
                double angle = - 2 * Math.Atan(Math.Abs(edgeType.Y2 / edgeType.X2)) * 180 / Math.PI;
                generalTransform = GetTransform(edgeType, angle, false, true);
            }
            else if ((edgeType.X2 < 0) && (edgeType.Y2 < 0))
            {
                double angle = 180;
                generalTransform = GetTransform(edgeType, angle, true, true);
            }
            edgeType.RenderTransform = generalTransform;
        }

        private static Transform GetTransform(EdgeType edgeType, double angle, bool xTranslateNeeded, bool yTranformNeeded)
        {
            TransformGroup transformGroup = new TransformGroup();

            RotateTransform rotateTransform = new RotateTransform();
            rotateTransform.Angle = angle;
            rotateTransform.CenterX = Math.Abs(edgeType.X2) / 2;
            rotateTransform.CenterY = Math.Abs(edgeType.Y2) / 2;
            transformGroup.Children.Add(rotateTransform);

            TranslateTransform translateTransform = new TranslateTransform();
            if (xTranslateNeeded)
            {
                translateTransform.X = - Math.Abs(edgeType.X2);
            }
            if (yTranformNeeded)
            {
                translateTransform.Y = - Math.Abs(edgeType.Y2);
            }

            transformGroup.Children.Add(translateTransform);
            return transformGroup;
        }

        public override void Select()
        {
            mainLine.Stroke = new SolidColorBrush(Colors.Blue);
        }

        public override void UnSelect()
        {
            mainLine.Stroke = new SolidColorBrush(Colors.Black);
        }
    }
}
