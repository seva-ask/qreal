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
using System.IO;
using QReal.Web.Database;
using System.Linq;
using System.Collections.Generic;

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

            LinkBoundaryPointPort endPort = new LinkBoundaryPointPort();
            endPort.Width = 7;
            endPort.Height = 7;
            endPort.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            endPort.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            endPort.DragStarted += new DragStartedEventHandler(boundaryPort_DragStarted);
            endPort.DragDelta += new DragDeltaEventHandler(endPort_DragDelta);
            endPort.DragCompleted += new DragCompletedEventHandler(endPort_DragCompleted);
            (this.Content as Panel).Children.Add(endPort);

            LinkBoundaryPointPort startPort = new LinkBoundaryPointPort();
            startPort.Width = 7;
            startPort.Height = 7;
            startPort.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            startPort.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            startPort.DragStarted += new DragStartedEventHandler(boundaryPort_DragStarted);
            startPort.DragDelta += new DragDeltaEventHandler(startPort_DragDelta);
            startPort.DragCompleted += new DragCompletedEventHandler(startPort_DragCompleted);
            (this.Content as Panel).Children.Add(startPort);
        }

        private void endPort_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            double y = (double)this.GetValue(Canvas.TopProperty) + Y2;
            double x = (double)this.GetValue(Canvas.LeftProperty) + X2;
            Point position = new Point(x, y);
            NodeType nodeTo = FindNodeUnderPosition(position);
            if (nodeTo != null)
            {
                Point positionInNode = new Point(x - (double)nodeTo.GetValue(Canvas.LeftProperty), y - (double)nodeTo.GetValue(Canvas.TopProperty));
                Port nearestPort = GetNearestPort(positionInNode, nodeTo);
                Y2 = (double)nodeTo.GetValue(Canvas.TopProperty) + nearestPort.GetNearestPointToPosition(positionInNode).Y - (double)this.GetValue(Canvas.TopProperty);
                X2 = (double)nodeTo.GetValue(Canvas.LeftProperty) + nearestPort.GetNearestPointToPosition(positionInNode).X - (double)this.GetValue(Canvas.LeftProperty);
                NodeTo = nodeTo.DataContext as NodeInstance;
            }
            else
            {
                NodeTo = null;
            }
        }

        private void startPort_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            double y = (double)this.GetValue(Canvas.TopProperty);
            double x = (double)this.GetValue(Canvas.LeftProperty);
            Point position = new Point(x, y);
            NodeType nodeFrom = FindNodeUnderPosition(position);
            if (nodeFrom != null)
            {
                Point positionInNode = new Point(x - (double)nodeFrom.GetValue(Canvas.LeftProperty), y - (double)nodeFrom.GetValue(Canvas.TopProperty));
                Port nearestPort = GetNearestPort(positionInNode, nodeFrom);
                double oldY = (double)this.GetValue(Canvas.TopProperty);
                double oldX = (double)this.GetValue(Canvas.LeftProperty);
                this.SetValue(Canvas.TopProperty, (double)nodeFrom.GetValue(Canvas.TopProperty) + nearestPort.GetNearestPointToPosition(positionInNode).Y);
                this.SetValue(Canvas.LeftProperty, (double)nodeFrom.GetValue(Canvas.LeftProperty) + nearestPort.GetNearestPointToPosition(positionInNode).X);
                Y2 += oldY - (double)this.GetValue(Canvas.TopProperty);
                X2 += oldX - (double)this.GetValue(Canvas.LeftProperty);
                NodeFrom = nodeFrom.DataContext as NodeInstance;
            }
            else
            {
                NodeFrom = null;
            }
        }

        private Port GetNearestPort(Point position, NodeType nodeType)
        {
            IEnumerable<UIElement> ports = (nodeType.Content as Panel).Children.Where(item => item is Port);
            return ports.AsQueryable<UIElement>().OrderBy(port => (port as Port), new PortComparer(position)).First() as Port;
        }

        private NodeType FindNodeUnderPosition(Point position)
        {
            Canvas canvas = VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(this.Parent)) as Canvas;
            UIElementCollection children = canvas.Children;
            foreach (var item in children)
            {
                NodeType nodeType = (VisualTreeHelper.GetChild((item as ContentPresenter), 0) as Canvas).Children[0] as NodeType;
                if (nodeType != null)
                {
                    double nodeX = (double)nodeType.GetValue(Canvas.LeftProperty);
                    double nodeY = (double)nodeType.GetValue(Canvas.TopProperty);
                    Rect itemBoundingRect = new Rect(nodeX, nodeY, nodeType.Width, nodeType.Height);
                    if (itemBoundingRect.Contains(position))
                    {
                        return nodeType;
                    }
                }
            }
            return null;
        }

        private void startPort_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Point deltaTempPoint = new Point(e.HorizontalChange, e.VerticalChange);
            if (mouseTransform != null)
            {
                Point deltaTempPointTransformed = mouseTransform.Transform(deltaTempPoint);
                X2 -= deltaTempPointTransformed.X;
                Y2 -= deltaTempPointTransformed.Y;
                double newTop = deltaTempPointTransformed.Y + (double)this.GetValue(Canvas.TopProperty);
                double newLeft = deltaTempPointTransformed.X + (double)this.GetValue(Canvas.LeftProperty);

                this.SetValue(Canvas.TopProperty, newTop);
                this.SetValue(Canvas.LeftProperty, newLeft);
                return;
            }
            X2 -= e.HorizontalChange;
            Y2 -= e.VerticalChange;
            double newTop2 = e.VerticalChange + (double)this.GetValue(Canvas.TopProperty);
            double newLeft2 = e.HorizontalChange + (double)this.GetValue(Canvas.LeftProperty);

            this.SetValue(Canvas.TopProperty, newTop2);
            this.SetValue(Canvas.LeftProperty, newLeft2);
        }

        private void boundaryPort_DragStarted(object sender, DragStartedEventArgs e)
        {
            mouseTransform = this.RenderTransform;
        }

        private GeneralTransform mouseTransform;

        private void endPort_DragDelta(object sender, DragDeltaEventArgs e)
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

        protected override void Select()
        {
            mainLine.Stroke = new SolidColorBrush(Colors.Blue);
        }

        protected override void UnSelect()
        {
            mainLine.Stroke = new SolidColorBrush(Colors.Black);
        }

        public double PortFrom
        {
            get { return (double)GetValue(PortFromProperty); }
            set { SetValue(PortFromProperty, value); }
        }

        public static readonly DependencyProperty PortFromProperty =
            DependencyProperty.Register("PortFrom", typeof(double), typeof(EdgeType), null);

        public double PortTo
        {
            get { return (double)GetValue(PortToProperty); }
            set { SetValue(PortToProperty, value); }
        }

        public static readonly DependencyProperty PortToProperty =
            DependencyProperty.Register("PortTo", typeof(double), typeof(EdgeType), null);

        public NodeInstance NodeFrom
        {
            get { return (NodeInstance)GetValue(NodeFromProperty); }
            set { SetValue(NodeFromProperty, value); }
        }

        public static readonly DependencyProperty NodeFromProperty =
            DependencyProperty.Register("NodeFrom", typeof(NodeInstance), typeof(EdgeType), null);

        public NodeInstance NodeTo
        {
            get { return (NodeInstance)GetValue(NodeToProperty); }
            set { SetValue(NodeToProperty, value); }
        }

        public static readonly DependencyProperty NodeToProperty =
            DependencyProperty.Register("NodeTo", typeof(NodeInstance), typeof(EdgeType), null);
    }
}
