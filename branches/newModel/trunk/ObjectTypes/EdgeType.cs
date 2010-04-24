using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using QReal.Web.Database;
using System.Linq;
using System.Collections.Generic;

namespace ObjectTypes
{
    public abstract class EdgeType : ObjectType
    {
        protected EdgeType()
        {
            Grid grid = new Grid {Name = "LayoutRoot"};
            this.Content = grid;
            this.Loaded += new RoutedEventHandler(EdgeType_Loaded);
        }

        private Line myMainLine;

        private void EdgeType_Loaded(object sender, RoutedEventArgs e)
        {
            myMainLine = new Line {StrokeThickness = 5};

            Binding bindingColor = new Binding
            {
                Source = this,
                Path = new PropertyPath("LineBrush"),
                Mode = BindingMode.TwoWay
            };
            myMainLine.SetBinding(Line.StrokeProperty, bindingColor);

            Binding bindingX2 = new Binding
                                    {
                                        Source = this,
                                        Path = new PropertyPath("X2"),
                                        Mode = BindingMode.TwoWay,
                                        Converter = new AbsConverter()
                                    };
            myMainLine.SetBinding(Line.X2Property, bindingX2);

            Binding bindingY2 = new Binding
                                    {
                                        Source = this,
                                        Path = new PropertyPath("Y2"),
                                        Mode = BindingMode.TwoWay,
                                        Converter = new AbsConverter()
                                    };
            myMainLine.SetBinding(Line.Y2Property, bindingY2);

            (this.Content as Panel).Children.Add(myMainLine);

            LinkBoundaryPointPort endPort = new LinkBoundaryPointPort
                                                {
                                                    Width = 7,
                                                    Height = 7,
                                                    VerticalAlignment = VerticalAlignment.Bottom,
                                                    HorizontalAlignment = HorizontalAlignment.Right
                                                };
            endPort.DragStarted += new DragStartedEventHandler(BoundaryPortDragStarted);
            endPort.DragDelta += new DragDeltaEventHandler(EndPortDragDelta);
            endPort.DragCompleted += new DragCompletedEventHandler(EndPortDragCompleted);
            (this.Content as Panel).Children.Add(endPort);

            LinkBoundaryPointPort startPort = new LinkBoundaryPointPort
                                                  {
                                                      Width = 7,
                                                      Height = 7,
                                                      VerticalAlignment = VerticalAlignment.Top,
                                                      HorizontalAlignment = HorizontalAlignment.Left
                                                  };
            startPort.DragStarted += new DragStartedEventHandler(BoundaryPortDragStarted);
            startPort.DragDelta += new DragDeltaEventHandler(StartPortDragDelta);
            startPort.DragCompleted += new DragCompletedEventHandler(StartPortDragCompleted);
            (this.Content as Panel).Children.Add(startPort);
        }

        private void EndPortDragCompleted(object sender, DragCompletedEventArgs e)
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
                PortTo = GetPortNumber(nearestPort, nodeTo) + nearestPort.GetNearestPointOfPort(positionInNode);
            }
            else
            {
                NodeTo = null;
            }
        }

        private void StartPortDragCompleted(object sender, DragCompletedEventArgs e)
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
            Point deltaTempPoint = new Point(e.HorizontalChange, e.VerticalChange);
            if (myMouseTransform != null)
            {
                Point deltaTempPointTransformed = myMouseTransform.Transform(deltaTempPoint);
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

        private void BoundaryPortDragStarted(object sender, DragStartedEventArgs e)
        {
            myMouseTransform = this.RenderTransform;
        }

        private GeneralTransform myMouseTransform;

        private void EndPortDragDelta(object sender, DragDeltaEventArgs e)
        {
            Point deltaTempPoint = new Point(e.HorizontalChange, e.VerticalChange);
            if (myMouseTransform != null)
            {
                Point deltaTempPointTransformed = myMouseTransform.Transform(deltaTempPoint);
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
                const double angle = 180;
                generalTransform = GetTransform(edgeType, angle, true, true);
            }
            edgeType.RenderTransform = generalTransform;
        }

        private static Transform GetTransform(EdgeType edgeType, double angle, bool xTranslateNeeded, bool yTranformNeeded)
        {
            TransformGroup transformGroup = new TransformGroup();

            RotateTransform rotateTransform = new RotateTransform
                                                  {
                                                      Angle = angle,
                                                      CenterX = Math.Abs(edgeType.X2)/2,
                                                      CenterY = Math.Abs(edgeType.Y2)/2
                                                  };
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
