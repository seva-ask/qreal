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
using System.Collections.Generic;
using QReal.Web.Database;
using System.Linq;
using System.Windows.Data;
using QReal.Ria.Database;

namespace ObjectTypes
{
    public abstract class NodeType : ObjectType
    {
        public NodeType()
        {
            this.Loaded += new RoutedEventHandler(NodeType_Loaded);
            this.SizeChanged += new SizeChangedEventHandler(NodeType_SizeChanged);
            this.MinHeight = 100;
            this.MinWidth = 100;
        }

        private void NodeType_Loaded(object sender, RoutedEventArgs e)
        {
            Thumb thumb = new Thumb();
            thumb.Width = 7;
            thumb.Height = 7;
            thumb.Cursor = Cursors.SizeNWSE;
            thumb.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            thumb.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            thumb.DragDelta += new DragDeltaEventHandler(thumb_DragDelta);

            Binding bindingThumbVisibility = new Binding();
            bindingThumbVisibility.Source = this;
            bindingThumbVisibility.Path = new PropertyPath("Selected");
            bindingThumbVisibility.Mode = BindingMode.TwoWay;
            bindingThumbVisibility.Converter = new VisibilityConverter();
            thumb.SetBinding(Thumb.VisibilityProperty, bindingThumbVisibility);

            (this.Content as Panel).Children.Add(thumb);
        }

        private void thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double newWidth = this.Width + e.HorizontalChange;
            if (newWidth > 0)
            {
                this.Width = newWidth;
            }
            double newHeight = this.Height + e.VerticalChange;
            if (newHeight > 0)
            {
                this.Height = newHeight;
            }
        }

        protected override void OnMoving(double deltaX, double deltaY)
        {
            base.OnMoving(deltaX, deltaY);
            AdjustSelectRectanglesPositions();
            Canvas canvas = VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(this.Parent)) as Canvas;
            UIElementCollection children = canvas.Children;
            foreach (var linkInstanceFrom in LinksFrom)
            {
                EdgeType linkFrom = GetEdgeType(children.Single(item =>
                    {
                        EdgeType edgeType = GetEdgeType(item);
                        return edgeType == null ? false : edgeType.DataContext == linkInstanceFrom;
                    }));
                double oldY = (double)linkFrom.GetValue(Canvas.TopProperty);
                double oldX = (double)linkFrom.GetValue(Canvas.LeftProperty);
                linkFrom.SetValue(Canvas.TopProperty, (double)linkFrom.GetValue(Canvas.TopProperty) + deltaY);
                linkFrom.SetValue(Canvas.LeftProperty, (double)linkFrom.GetValue(Canvas.LeftProperty) + deltaX);
                linkFrom.Y2 += oldY - (double)linkFrom.GetValue(Canvas.TopProperty);
                linkFrom.X2 += oldX - (double)linkFrom.GetValue(Canvas.LeftProperty);
            }           
            foreach (var linkInstanceTo in LinksTo)
            {
                EdgeType linkTo = GetEdgeType(children.Single(item =>
                    {
                        EdgeType edgeType = GetEdgeType(item);
                        return edgeType == null ? false : edgeType.DataContext == linkInstanceTo;
                    }));
                linkTo.Y2 += deltaY;
                linkTo.X2 += deltaX;
            }
        }

        private EdgeType GetEdgeType(object item)
        {
            return (VisualTreeHelper.GetChild((item as ContentPresenter), 0) as Canvas).Children[0] as EdgeType;
        }

        private void NodeType_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AdjustSelectRectanglesPositions();

            Canvas canvas = VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(this.Parent)) as Canvas;
            UIElementCollection children = canvas.Children;
            foreach (var linkInstanceFrom in LinksFrom)
            {
                EdgeType linkFrom = GetEdgeType(children.Single(item =>
                {
                    EdgeType edgeType = GetEdgeType(item);
                    return edgeType == null ? false : edgeType.DataContext == linkInstanceFrom;
                }));

                IEnumerable<UIElement> ports = (this.Content as Panel).Children.Where(item => item is Port);
                Port portFrom = ports.ElementAt((int)Math.Floor(linkFrom.PortFrom)) as Port;
                double portNearestPosition = linkFrom.PortFrom - Math.Floor(linkFrom.PortFrom);

                double oldY = (double)linkFrom.GetValue(Canvas.TopProperty);
                double oldX = (double)linkFrom.GetValue(Canvas.LeftProperty);
                linkFrom.SetValue(Canvas.TopProperty, (double)this.GetValue(Canvas.TopProperty)
                    + portFrom.Position.Y +
                    portFrom.TransformedHeight * portNearestPosition);
                linkFrom.SetValue(Canvas.LeftProperty, (double)this.GetValue(Canvas.LeftProperty)
                    + portFrom.Position.X +
                    portFrom.TransformedWidth * portNearestPosition);
                linkFrom.Y2 += oldY - (double)linkFrom.GetValue(Canvas.TopProperty);
                linkFrom.X2 += oldX - (double)linkFrom.GetValue(Canvas.LeftProperty);
            }
            foreach (var linkInstanceTo in LinksTo)
            {
                EdgeType linkTo = GetEdgeType(children.Single(item =>
                {
                    EdgeType edgeType = GetEdgeType(item);
                    return edgeType == null ? false : edgeType.DataContext == linkInstanceTo;
                }));

                IEnumerable<UIElement> ports = (this.Content as Panel).Children.Where(item => item is Port);
                Port portTo = ports.ElementAt((int)Math.Floor(linkTo.PortFrom)) as Port;
                double portNearestPosition = linkTo.PortFrom - Math.Floor(linkTo.PortFrom);

                linkTo.Y2 = (double)this.GetValue(Canvas.TopProperty) + portTo.Position.Y +
                    portTo.TransformedHeight * portNearestPosition - (double)linkTo.GetValue(Canvas.TopProperty);

                linkTo.X2 = (double)this.GetValue(Canvas.LeftProperty) + portTo.Position.X +
                    portTo.TransformedWidth * portNearestPosition - (double)linkTo.GetValue(Canvas.LeftProperty);
            }
        }

        public IEnumerable<EdgeInstance> LinksFrom
        {
            get
            {
                return GetLinks(edge => edge.NodeFrom == this.DataContext);
            }
        }

        private IEnumerable<EdgeInstance> GetLinks(Func<EdgeInstance, bool> predicate)
        {
            return InstancesManager.Instance.InstancesContext.GraphicInstances.OfType<EdgeInstance>().Where(predicate);
        }

        public IEnumerable<EdgeInstance> LinksTo
        {
            get
            {
                return GetLinks(edge => edge.NodeTo == this.DataContext);
            }
        }

        private Rectangle rectTopLeft;
        private Rectangle rectTopRight;
        private Rectangle rectBottomLeft;
        private Rectangle rectBottomRight;

        private void CreateSelectRectangles()
        {
            CreateSelectRectangle(ref rectTopLeft);
            CreateSelectRectangle(ref rectTopRight);
            CreateSelectRectangle(ref rectBottomLeft);
            CreateSelectRectangle(ref rectBottomRight);
        }

        private void CreateSelectRectangle(ref Rectangle rect)
        {
            Panel parent = this.Parent as Panel;
            rect = new Rectangle();
            rect.Width = 5;
            rect.Height = 5;
            rect.Fill = new SolidColorBrush(Colors.Blue);

            Binding bindingRectVisibility = new Binding();
            bindingRectVisibility.Source = this;
            bindingRectVisibility.Path = new PropertyPath("Selected");
            bindingRectVisibility.Mode = BindingMode.TwoWay;
            bindingRectVisibility.Converter = new VisibilityConverter();
            rect.SetBinding(Thumb.VisibilityProperty, bindingRectVisibility);

            parent.Children.Add(rect);
        }

        private void AdjustSelectRectanglesPositions()
        {
            if (rectTopLeft == null)
            {
                CreateSelectRectangles();
            }

            rectTopLeft.SetValue(Canvas.LeftProperty, (double)this.GetValue(Canvas.LeftProperty));
            rectTopLeft.SetValue(Canvas.TopProperty, (double)this.GetValue(Canvas.TopProperty));

            rectTopRight.SetValue(Canvas.LeftProperty, (double)this.GetValue(Canvas.LeftProperty) + this.ActualWidth - rectTopRight.Width);
            rectTopRight.SetValue(Canvas.TopProperty, (double)this.GetValue(Canvas.TopProperty));

            rectBottomLeft.SetValue(Canvas.LeftProperty, (double)this.GetValue(Canvas.LeftProperty));
            rectBottomLeft.SetValue(Canvas.TopProperty, (double)this.GetValue(Canvas.TopProperty) + this.ActualHeight - rectTopRight.Height);

            rectBottomRight.SetValue(Canvas.LeftProperty, (double)this.GetValue(Canvas.LeftProperty) + this.ActualWidth - rectTopRight.Width);
            rectBottomRight.SetValue(Canvas.TopProperty, (double)this.GetValue(Canvas.TopProperty) + this.ActualHeight - rectTopRight.Height);
        }
    }
}
