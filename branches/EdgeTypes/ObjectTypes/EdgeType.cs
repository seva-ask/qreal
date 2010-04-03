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
            grid.Background = new SolidColorBrush(Colors.Transparent);
            this.Content = grid;
            this.Loaded += new RoutedEventHandler(EdgeType_Loaded);
        }

        private void EdgeType_Loaded(object sender, RoutedEventArgs e)
        {
            Binding bindingTransform = new Binding();
            bindingTransform.Source = this;
            bindingTransform.Path = new PropertyPath("Transformation");
            bindingTransform.Mode = BindingMode.TwoWay;
            this.SetBinding(UserControl.RenderTransformProperty, bindingTransform);

            Line line = new Line();
            line.X1 = 0;
            line.Y1 = 0;
            line.StrokeThickness = 5;
            line.Stroke = new SolidColorBrush(Colors.Black);
            Binding bindingX2 = new Binding();
            bindingX2.Source = this;
            bindingX2.Path = new PropertyPath("X2");
            bindingX2.Mode = BindingMode.TwoWay;
            bindingX2.Converter = new AbsConverter();
            line.SetBinding(Line.X2Property, bindingX2);
            Binding bindingY2 = new Binding();
            bindingY2.Source = this;
            bindingY2.Path = new PropertyPath("Y2");
            bindingY2.Mode = BindingMode.TwoWay;
      //      bindingY2.Converter = new AbsConverter();
            line.SetBinding(Line.Y2Property, bindingY2);
            (this.Content as Panel).Children.Add(line);
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
            mouseTransform = this.Transformation;
        }

        private GeneralTransform mouseTransform;

        private void thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Point p = new Point(e.HorizontalChange, e.VerticalChange);
            if (mouseTransform != null)
            {
                Point p2 = mouseTransform.Transform(p);
                X2 += p2.X;
                Y2 += p2.Y;
                return;
            }
            X2 += e.HorizontalChange;
            Y2 += e.VerticalChange;
        }

        public GeneralTransform Transformation
        {
            get { return (GeneralTransform)GetValue(TransformationProperty); }
            set { SetValue(TransformationProperty, value); }
        }

        public static readonly DependencyProperty TransformationProperty =
            DependencyProperty.Register("Transformation", typeof(GeneralTransform), typeof(EdgeType), null);

        public double X2
        {
            get { return (double)GetValue(X2Property); }
            set { SetValue(X2Property, value); }
        }

        public static readonly DependencyProperty X2Property =
            DependencyProperty.Register("X2", typeof(double), typeof(EdgeType), new PropertyMetadata(OnX2PropertyChanged));

        private static void OnX2PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            EdgeType edgeType = obj as EdgeType;
            if ((edgeType.X2 < 0) && (edgeType.Y2 > 0))
            {
                TransformGroup transformGroup = new TransformGroup();

                RotateTransform rotateTransform = new RotateTransform();
                rotateTransform.Angle = 2 * Math.Atan(Math.Abs(edgeType.X2 / edgeType.Y2)) * 180 / Math.PI;
                rotateTransform.CenterX = Math.Abs(edgeType.X2) / 2;
                rotateTransform.CenterY = Math.Abs(edgeType.Y2) / 2;
                transformGroup.Children.Add(rotateTransform);

                TranslateTransform translateTransform = new TranslateTransform();
                translateTransform.X = - Math.Abs(edgeType.X2);
                transformGroup.Children.Add(translateTransform);

                edgeType.Transformation = transformGroup;
            }
            else
            {
                edgeType.Transformation = null;
            }
        }

        public double Y2
        {
            get { return (double)GetValue(Y2Property); }
            set { SetValue(Y2Property, value); }
        }

        public static readonly DependencyProperty Y2Property =
            DependencyProperty.Register("Y2", typeof(double), typeof(EdgeType), null);
    }
}
