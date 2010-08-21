using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
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

        protected readonly Line MainLine = new Line();

        private Arrow myStartArrow;
        private Arrow myEndArrow;

        private void EdgeType_Loaded(object sender, RoutedEventArgs e)
        {
            CreateMainLine();

            CreateArrows();

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

        private void CreateMainLine()
        {
            Binding bindingColor = new Binding
                                       {
                                           Source = this,
                                           Path = new PropertyPath("LineBrush"),
                                           Mode = BindingMode.TwoWay
                                       };
            MainLine.SetBinding(Line.StrokeProperty, bindingColor);

            Binding bindingStartX = new Binding
            {
                Source = this,
                Path = new PropertyPath("MainLineStartX"),
                Mode = BindingMode.TwoWay
            };
            MainLine.SetBinding(Line.X1Property, bindingStartX);

            Binding bindingStartY = new Binding
            {
                Source = this,
                Path = new PropertyPath("MainLineStartY"),
                Mode = BindingMode.TwoWay
            };
            MainLine.SetBinding(Line.Y1Property, bindingStartY);

            Binding bindingEndX = new Binding
            {
                Source = this,
                Path = new PropertyPath("MainLineEndX"),
                Mode = BindingMode.TwoWay
            };
            MainLine.SetBinding(Line.X2Property, bindingEndX);

            Binding bindingEndY = new Binding
            {
                Source = this,
                Path = new PropertyPath("MainLineEndY"),
                Mode = BindingMode.TwoWay
            };
            MainLine.SetBinding(Line.Y2Property, bindingEndY);

            (this.Content as Panel).Children.Add(MainLine);
        }

        private void CreateArrows()
        {
            myStartArrow = GetStartArrow();
            if (myStartArrow != null)
            {
                (this.Content as Panel).Children.Add(myStartArrow);
            }

            myEndArrow = GetEndArrow();
            if (myEndArrow != null)
            {
                Binding bindingX = new Binding
                {
                    Source = this,
                    Path = new PropertyPath("X2"),
                    Mode = BindingMode.TwoWay
                };
                myEndArrow.SetBinding(Canvas.LeftProperty, bindingX);

                Binding bindingY = new Binding
                {
                    Source = this,
                    Path = new PropertyPath("Y2"),
                    Mode = BindingMode.TwoWay
                };
                myEndArrow.SetBinding(Canvas.TopProperty, bindingY);
                (this.Content as Panel).Children.Add(myEndArrow);
            }
            AdjustArrowsAndMainLine();
        }

        private void EndPortDragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.MouseRelease();
            double y = Y + Y2;
            double x = X + X2;
            Point position = new Point(x, y);
            NodeType nodeTo = FindNodeUnderPosition(position);
            if (nodeTo != null)
            {
                Point positionInNode = new Point(x - nodeTo.X, y - nodeTo.Y);
                Port nearestPort = GetNearestPort(positionInNode, nodeTo);
                Y2 = nodeTo.Y + nearestPort.GetNearestPointToPosition(positionInNode).Y - Y;
                X2 = nodeTo.X + nearestPort.GetNearestPointToPosition(positionInNode).X - X;
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
            Point position = new Point(X, Y);
            NodeType nodeFrom = FindNodeUnderPosition(position);
            if (nodeFrom != null)
            {
                Point positionInNode = new Point(X - nodeFrom.X, Y - nodeFrom.Y);
                Port nearestPort = GetNearestPort(positionInNode, nodeFrom);
                double oldY = Y;
                double oldX = X;
                Y = nodeFrom.Y + nearestPort.GetNearestPointToPosition(positionInNode).Y;
                X = nodeFrom.X + nearestPort.GetNearestPointToPosition(positionInNode).X;
                Y2 += oldY - Y;
                X2 += oldX - X;
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
                    Rect itemBoundingRect = new Rect(nodeType.X, nodeType.Y, nodeType.Width, nodeType.Height);
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
            MoveStartPort(e.HorizontalChange, e.VerticalChange);
        }

        public void MoveStartPort(double deltaX, double deltaY)
        {
            X += deltaX;
            Y += deltaY;
            X2 -= deltaX;
            Y2 -= deltaY;
        }

        public void SetStartPortPosition(double x, double y)
        {
            double deltaX = x - X;
            double deltaY = y - Y;
            X = x;
            Y = y;
            X2 -= deltaX;
            Y2 -= deltaY;
        }

        private void BoundaryPortDragStarted(object sender, DragStartedEventArgs e)
        {
            this.MousePress();
        }

        private void EndPortDragDelta(object sender, DragDeltaEventArgs e)
        {
            MoveEndPort(e.HorizontalChange, e.VerticalChange);
        }

        public void MoveEndPort(double deltaX, double deltaY)
        {
            X2 += deltaX;
            Y2 += deltaY;
        }

        public void SetEndPortPosition(double x, double y)
        {
            X2 = x;
            Y2 = y;
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
            edgeType.AdjustArrowsAndMainLine();
        }

        protected override void OnPositionChanged()
        {
            base.OnPositionChanged();
            AdjustArrowsAndMainLine();
        }

        public double MainLineStartX
        {
            get { return (double) GetValue(MainLineStartXProperty); }
            set { SetValue(MainLineStartXProperty, value); }
        }

        public static readonly DependencyProperty MainLineStartXProperty =
            DependencyProperty.Register("MainLineStartX", typeof (double), typeof (EdgeType), null);

        public double MainLineStartY
        {
            get { return (double) GetValue(MainLineStartYProperty); }
            set { SetValue(MainLineStartYProperty, value); }
        }

        public static readonly DependencyProperty MainLineStartYProperty =
            DependencyProperty.Register("MainLineStartY", typeof (double), typeof (EdgeType), null);

        public double MainLineEndX
        {
            get { return (double) GetValue(MainLineEndXProperty); }
            set { SetValue(MainLineEndXProperty, value); }
        }

        public static readonly DependencyProperty MainLineEndXProperty =
            DependencyProperty.Register("MainLineEndX", typeof (double), typeof (EdgeType), null);

        public double MainLineEndY
        {
            get { return (double) GetValue(MainLineEndYProperty); }
            set { SetValue(MainLineEndYProperty, value); }
        }

        public static readonly DependencyProperty MainLineEndYProperty =
            DependencyProperty.Register("MainLineEndY", typeof (double), typeof (EdgeType), null);

        private void AdjustArrowsAndMainLine()
        {
            double angle = Math.Atan(Y2 / X2) + (X2 < 0 ? Math.PI : 0);
            if (myStartArrow != null)
            {
                RotateTransform rotateStartArrow = new RotateTransform
                {
                    Angle = angle * 180 / Math.PI 
                };
                myStartArrow.RenderTransform = rotateStartArrow;
            }
            if (myStartArrow != null && !myStartArrow.IsMainLineVisibleUnderArrow)
            {
                double deltaY = Math.Sin(angle) * myStartArrow.MinWidth;
                MainLineStartY = deltaY;
                double deltaX = Math.Cos(angle) * myStartArrow.MinWidth;
                MainLineStartX = deltaX;
            }
            else
            {
                MainLineStartX = 0;
                MainLineStartY = 0;
            }
            if (myEndArrow != null)
            {
                RotateTransform rotateEndArrow = new RotateTransform
                {
                    Angle = angle * 180 / Math.PI + 180
                };
                myEndArrow.RenderTransform = rotateEndArrow;
            }
            if (myEndArrow != null && !myEndArrow.IsMainLineVisibleUnderArrow)
            {
                double deltaY = Math.Sin(angle) * myEndArrow.MinWidth;
                MainLineEndY = Y2 - deltaY;
                double deltaX = Math.Cos(angle) * myEndArrow.MinWidth;
                MainLineEndX = X2 - deltaX;
            }
            else
            {
                MainLineEndX = X2;
                MainLineEndY = Y2;
            }
         }

        protected virtual Arrow GetStartArrow()
        {
            return null;
        }

        protected virtual Arrow GetEndArrow()
        {
            return null;
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

        protected override bool CanMove()
        {
            return ((NodeFrom == null) && (NodeTo == null));
        }
    }
}
