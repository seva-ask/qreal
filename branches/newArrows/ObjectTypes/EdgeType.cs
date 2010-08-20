using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using ObjectTypes.Arrows;
using ObjectTypes.Ports;
using QReal.Web.Database;
using System.Linq;
using System.Collections.Generic;

namespace ObjectTypes
{
    public abstract class EdgeType : ObjectType
    {
        protected EdgeType()
        {
            Canvas canvas = new Canvas { Name = "LayoutRoot" };
            this.Content = canvas;
            this.Loaded += new RoutedEventHandler(EdgeType_Loaded);
        }

        protected readonly Line MainLine = new Line {StrokeThickness = 5};
        private Arrow myStartArrow;
        private Arrow myEndArrow;

        private void EdgeType_Loaded(object sender, RoutedEventArgs e)
        {
            Binding bindingColor = new Binding
            {
                Source = this,
                Path = new PropertyPath("LineBrush"),
                Mode = BindingMode.TwoWay
            };
            MainLine.SetBinding(Line.StrokeProperty, bindingColor);

            (this.Content as Panel).Children.Add(MainLine);

            //myStartArrow.VerticalAlignment = VerticalAlignment.Top;
            //myStartArrow.HorizontalAlignment = HorizontalAlignment.Left;
            //myStartArrow.Margin = new Thickness(0, -(Arrow.HEIGHT - 7) / 2, 0, 0);
            //(this.Content as Panel).Children.Add(myStartArrow);

            //myEndArrow.VerticalAlignment = VerticalAlignment.Top;
            //myEndArrow.HorizontalAlignment = HorizontalAlignment.Left;
            //(this.Content as Panel).Children.Add(myEndArrow);

            LinkBoundaryPointPort endPort = new LinkBoundaryPointPort
                                                {
                                                    Width = 7,
                                                    Height = 7
                                                };
            Binding bindingEndPortX = new Binding
            {
                Source = this,
                Path = new PropertyPath("X2"),
                Mode = BindingMode.TwoWay
            };
            endPort.SetBinding(Canvas.LeftProperty, bindingEndPortX);
            Binding bindingEndPortY = new Binding
            {
                Source = this,
                Path = new PropertyPath("Y2"),
                Mode = BindingMode.TwoWay
            };
            endPort.SetBinding(Canvas.TopProperty, bindingEndPortY);
            endPort.DragStarted += new DragStartedEventHandler(BoundaryPortDragStarted);
            endPort.DragDelta += new DragDeltaEventHandler(EndPortDragDelta);
            endPort.DragCompleted += new DragCompletedEventHandler(EndPortDragCompleted);
            (this.Content as Panel).Children.Add(endPort);

            LinkBoundaryPointPort startPort = new LinkBoundaryPointPort
                                                  {
                                                      Width = 7,
                                                      Height = 7
                                                  };
            startPort.DragStarted += new DragStartedEventHandler(BoundaryPortDragStarted);
            startPort.DragDelta += new DragDeltaEventHandler(StartPortDragDelta);
            startPort.DragCompleted += new DragCompletedEventHandler(StartPortDragCompleted);
            (this.Content as Panel).Children.Add(startPort);
        }

        private void EndPortDragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.MouseRelease();
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
                PortTo = GetPortNumber(nearestPort, nodeTo) + nearestPort.GetNearestPointOfPort(positionInNode);
            }
            else
            {
                NodeTo = null;
            }
        }

        private void StartPortDragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.MouseRelease();
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
                PortFrom = GetPortNumber(nearestPort, nodeFrom) + nearestPort.GetNearestPointOfPort(positionInNode);
            }
            else
            {
                NodeFrom = null;
            }
        }

        private static Port GetNearestPort(Point position, NodeType nodeType)
        {
            IEnumerable<UIElement> ports = (nodeType.Content as Panel).Children.Where(item => item is Port);
            return ports.AsQueryable().OrderBy(port => (port as Port), new PortComparer(position)).First() as Port;
        }

        private static int GetPortNumber(Port port, NodeType nodeType)
        {
            IEnumerable<UIElement> ports = (nodeType.Content as Panel).Children.Where(item => item is Port);
            int number = 0;
            foreach (var item in ports)
            {
                if (item == port)
                {
                    return number;
                }
                number++;
            }
            return -1;
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

        private void StartPortDragDelta(object sender, DragDeltaEventArgs e)
        {
            X += e.HorizontalChange;
            Y += e.VerticalChange;
            X2 -= e.HorizontalChange;
            Y2 -= e.VerticalChange;
        }

        private void BoundaryPortDragStarted(object sender, DragStartedEventArgs e)
        {
            this.MousePress();
        }

        private void EndPortDragDelta(object sender, DragDeltaEventArgs e)
        {
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
            //Transform generalTransform = null;
            //if ((edgeType.X2 < 0) && (edgeType.Y2 > 0))
            //{
            //    double angle = 2 * Math.Atan(Math.Abs(edgeType.X2 / edgeType.Y2)) * 180 / Math.PI;
            //    generalTransform = GetTransform(edgeType, angle, true, false);
            //}
            //else if ((edgeType.X2 > 0) && (edgeType.Y2 < 0))
            //{
            //    double angle = - 2 * Math.Atan(Math.Abs(edgeType.Y2 / edgeType.X2)) * 180 / Math.PI;
            //    generalTransform = GetTransform(edgeType, angle, false, true);
            //}
            //else if ((edgeType.X2 < 0) && (edgeType.Y2 < 0))
            //{
            //    const double angle = 180;
            //    generalTransform = GetTransform(edgeType, angle, true, true);
            //}
            //edgeType.RenderTransform = generalTransform;

            //RotateTransform rotate = new RotateTransform
            //                             {
            //                                 Angle = Math.Atan(Math.Abs(edgeType.Y2/edgeType.X2))*180/Math.PI,
            //                                 CenterY = Arrow.HEIGHT/2
            //                             };
            //if (edgeType.myStartArrow == null)
            //{
            //    edgeType.myStartArrow = edgeType.GetStartArrow();
            //    edgeType.myEndArrow = edgeType.GetEndArrow();
            //}
            //edgeType.myStartArrow.RenderTransform = rotate;
            //Point start = rotate.Transform(new Point(Arrow.WIDTH, 7 / 2));
            edgeType.MainLine.X1 = 0;
            edgeType.MainLine.Y1 = 0;

            edgeType.MainLine.X2 = edgeType.X2;
            edgeType.MainLine.Y2 = edgeType.Y2;
            //edgeType.myEndArrow.RenderTransform = rotate;
            //edgeType.myEndArrow.Margin = new Thickness(edgeType.MainLine.X2 - Arrow.WIDTH / 2, edgeType.MainLine.Y2 - Arrow.HEIGHT / 2, 0, 0);
        }

        protected virtual Arrow GetStartArrow()
        {
            return new NoArrow();
        }

        protected virtual Arrow GetEndArrow()
        {
            return new NoArrow();
        }

        protected override void Select()
        {
            LineBrush = new SolidColorBrush(Colors.Blue);
        }

        protected override void UnSelect()
        {
            LineBrush = new SolidColorBrush(Colors.Black);
        }

        public Brush LineBrush
        {
            get { return (Brush) GetValue(LineBrushProperty); }
            set { SetValue(LineBrushProperty, value); }
        }

        public static readonly DependencyProperty LineBrushProperty =
            DependencyProperty.Register("LineBrush", typeof (Brush), typeof (EdgeType), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public double PortFrom
        {
            get
            {
                return (double) (this.DataContext as EdgeInstance).PortFrom;
            }
            set
            {
                (this.DataContext as EdgeInstance).PortFrom = value;
            }
        }

        public double PortTo
        {
            get
            {
                return (double)(this.DataContext as EdgeInstance).PortTo;
            }
            set
            {
                (this.DataContext as EdgeInstance).PortTo = value;
            }
        }

        public NodeInstance NodeFrom
        {
            get
            {
                return (this.DataContext as EdgeInstance).NodeFrom;
            }
            set
            {
                (this.DataContext as EdgeInstance).NodeFrom = value;
            }
        }

        public NodeInstance NodeTo
        {
            get
            {
                return (this.DataContext as EdgeInstance).NodeTo;
            }
            set
            {
                (this.DataContext as EdgeInstance).NodeTo = value;
            }
        }

        public double X
        {
            get
            {
                return (this.DataContext as EdgeInstance).X;
            }
            set
            {
                (this.DataContext as EdgeInstance).X = value;
            }
        }

        public double Y
        {
            get
            {
                return (this.DataContext as EdgeInstance).Y;
            }
            set
            {
                (this.DataContext as EdgeInstance).Y = value;
            }
        }

        protected override bool CanMove()
        {
            return ((NodeFrom == null) && (NodeTo == null));
        }
    }
}
