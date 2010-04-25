﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Collections.Generic;
using QReal.Ria.Database;
using QReal.Web.Database;
using System.Linq;
using System.Windows.Data;

namespace ObjectTypes
{
    public abstract class NodeType : ObjectType
    {
        protected NodeType()
        {
            this.Loaded += new RoutedEventHandler(NodeType_Loaded);
            this.SizeChanged += new SizeChangedEventHandler(NodeType_SizeChanged);
            this.MinHeight = 100;
            this.MinWidth = 100;
        }

        private void NodeType_Loaded(object sender, RoutedEventArgs e)
        {
            Thumb thumb = new Thumb
                              {
                                  Width = 7,
                                  Height = 7,
                                  Cursor = Cursors.SizeNWSE,
                                  VerticalAlignment = VerticalAlignment.Bottom,
                                  HorizontalAlignment = HorizontalAlignment.Right
                              };
            thumb.DragDelta += new DragDeltaEventHandler(ThumbDragDelta);

            Binding bindingThumbVisibility = new Binding
                                                 {
                                                     Source = this,
                                                     Path = new PropertyPath("Selected"),
                                                     Mode = BindingMode.TwoWay,
                                                     Converter = new VisibilityConverter()
                                                 };
            thumb.SetBinding(VisibilityProperty, bindingThumbVisibility);

            (this.Content as Panel).Children.Add(thumb);
        }

        private void ThumbDragDelta(object sender, DragDeltaEventArgs e)
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

        protected override void Move(double deltaX, double deltaY)
        {
            base.Move(deltaX, deltaY);
            AdjustSelectRectanglesPositions();
            MoveChildren(deltaX, deltaY);
            Canvas canvas = VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(this.Parent)) as Canvas;
            UIElementCollection children = canvas.Children;
            foreach (var linkInstanceFrom in LinksFrom)
            {
                EdgeType linkFrom = GetObjectType<EdgeType>(children.Single(item =>
                    {
                        EdgeType edgeType = GetObjectType<EdgeType>(item);
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
                EdgeType linkTo = GetObjectType<EdgeType>(children.Single(item =>
                    {
                        EdgeType edgeType = GetObjectType<EdgeType>(item);
                        return edgeType == null ? false : edgeType.DataContext == linkInstanceTo;
                    }));
                linkTo.Y2 += deltaY;
                linkTo.X2 += deltaX;
            }
        }

        private void MoveChildren(double deltaX, double deltaY)
        {
            Canvas canvas = VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(this.Parent)) as Canvas;
            UIElementCollection children = canvas.Children;
            foreach (var nodeInstance in (this.DataContext as NodeInstance).GetParent<ParentableInstance>().NodeChildren)
            {
                NodeType node = GetObjectType<NodeType>(children.Single(item =>
                {
                    NodeType nodeType = GetObjectType<NodeType>(item);
                    return nodeType == null ? false : nodeType.DataContext == nodeInstance;
                }));
                node.Move(deltaX, deltaY);
            }
        }

        private static TObjectType GetObjectType<TObjectType>(object item) where TObjectType:ObjectType
        {
            return (VisualTreeHelper.GetChild((item as ContentPresenter), 0) as Canvas).Children[0] as TObjectType;
        }

        private void NodeType_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AdjustSelectRectanglesPositions();

            Canvas canvas = VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(this.Parent)) as Canvas;
            UIElementCollection children = canvas.Children;
            foreach (var linkInstanceFrom in LinksFrom)
            {
                EdgeType linkFrom = GetObjectType<EdgeType>(children.Single(item =>
                {
                    EdgeType edgeType = GetObjectType<EdgeType>(item);
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
                EdgeType linkTo = GetObjectType<EdgeType>(children.Single(item =>
                {
                    EdgeType edgeType = GetObjectType<EdgeType>(item);
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
                return (this.DataContext as NodeInstance).EdgesFrom;
            }
        }

        public IEnumerable<EdgeInstance> LinksTo
        {
            get
            {
                return (this.DataContext as NodeInstance).EdgesTo;
            }
        }

        private Rectangle myRectTopLeft;
        private Rectangle myRectTopRight;
        private Rectangle myRectBottomLeft;
        private Rectangle myRectBottomRight;

        private void CreateSelectRectangles()
        {
            CreateSelectRectangle(ref myRectTopLeft);
            CreateSelectRectangle(ref myRectTopRight);
            CreateSelectRectangle(ref myRectBottomLeft);
            CreateSelectRectangle(ref myRectBottomRight);
        }

        private void CreateSelectRectangle(ref Rectangle rect)
        {
            Panel parent = this.Parent as Panel;
            rect = new Rectangle {Width = 5, Height = 5, Fill = new SolidColorBrush(Colors.Blue)};

            Binding bindingRectVisibility = new Binding
                                                {
                                                    Source = this,
                                                    Path = new PropertyPath("Selected"),
                                                    Mode = BindingMode.TwoWay,
                                                    Converter = new VisibilityConverter()
                                                };
            rect.SetBinding(VisibilityProperty, bindingRectVisibility);

            parent.Children.Add(rect);
        }

        private void AdjustSelectRectanglesPositions()
        {
            if (myRectTopLeft == null)
            {
                CreateSelectRectangles();
            }

            myRectTopLeft.SetValue(Canvas.LeftProperty, (double)this.GetValue(Canvas.LeftProperty));
            myRectTopLeft.SetValue(Canvas.TopProperty, (double)this.GetValue(Canvas.TopProperty));

            myRectTopRight.SetValue(Canvas.LeftProperty, (double)this.GetValue(Canvas.LeftProperty) + this.ActualWidth - myRectTopRight.Width);
            myRectTopRight.SetValue(Canvas.TopProperty, (double)this.GetValue(Canvas.TopProperty));

            myRectBottomLeft.SetValue(Canvas.LeftProperty, (double)this.GetValue(Canvas.LeftProperty));
            myRectBottomLeft.SetValue(Canvas.TopProperty, (double)this.GetValue(Canvas.TopProperty) + this.ActualHeight - myRectTopRight.Height);

            myRectBottomRight.SetValue(Canvas.LeftProperty, (double)this.GetValue(Canvas.LeftProperty) + this.ActualWidth - myRectTopRight.Width);
            myRectBottomRight.SetValue(Canvas.TopProperty, (double)this.GetValue(Canvas.TopProperty) + this.ActualHeight - myRectTopRight.Height);
        }
    }
}
