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
using System.Linq;
using ObjectTypes;
using QReal.Types;
using QReal.Web.Database;
using QReal.Database;
using System.Diagnostics;

namespace QReal.Controls
{
    public class CanvasDDTarget : ItemsControlDragDropTarget<CanvasItemsControl, ObjectType>
    {
        public CanvasDDTarget()
        {
            this.MouseMove += new MouseEventHandler(CanvasDDTarget_MouseMove);
            this.MouseMove += new MouseEventHandler(CanvasDDTarget_MouseMoveElement);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(CanvasDDTarget_MouseLeftButtonDown);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(CanvasDDTarget_MouseLeftButtonUp);
        }

        private void CanvasDDTarget_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ObjectType objectType = FindObjectType(e);
            SetZIndex(objectType, 0);
        }

        private void CanvasDDTarget_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ObjectType objectType = FindObjectType(e);
            SetZIndex(objectType, 1);
            int selectedId = -1;
            if (objectType != null)
            {
                selectedId = objectType.Id;
            }
            UIManager.Instance.SelectedGraphicInstanceId = selectedId;
        }

        private static void SetZIndex(ObjectType objectType, int zIndex)
        {
            if (objectType != null)
            {
                ContentPresenter contentPresenter = VisualTreeHelper.GetParent(objectType.Parent) as ContentPresenter;
                Canvas.SetZIndex(contentPresenter, zIndex);
            }
        }

        private void CanvasDDTarget_MouseMoveElement(object sender, MouseEventArgs e)
        {
            ObjectType objectType = FindObjectType(e);
            if (objectType != null)
            {
                if (objectType.IsMouseCaptured)
                {
                    double deltaY;
                    double deltaX;
                    objectType.GetDeltaMouseMove(e, out deltaY, out deltaX);
                    GraphicInstance graphicInstance = (this.Content as CanvasItemsControl).Items.Single(item => (item as GraphicInstance).Id == objectType.Id) as GraphicInstance;
                    foreach (var instanceChild in graphicInstance.Children)
                    {
                        instanceChild.X += deltaX;
                        instanceChild.Y += deltaY;
                    }
                }
            }
        }

        private ObjectType FindObjectType(MouseEventArgs e)
        {
            FrameworkElement parent = (e.OriginalSource as FrameworkElement);
            while ((parent != null) && (!(parent is ObjectType)))
            {
                parent = parent.Parent as FrameworkElement;
            }
            return parent as ObjectType;
        }

        protected override void OnDropOverride(Microsoft.Windows.DragEventArgs args)
        {
            ItemDragEventArgs rawObject = args.Data.GetData(args.Data.GetFormats()[0]) as ItemDragEventArgs;
            string typeName = (rawObject.Data as System.Collections.ObjectModel.SelectionCollection).First().Item as string;
            LogicalInstance logicalInstance = new LogicalInstance();
            logicalInstance.Name = "anonymous " + typeName;
            logicalInstance.Type = typeName;
            InstancesManager.Instance.InstancesContext.LogicalInstances.Add(logicalInstance);
            GraphicInstance graphicInstance = new GraphicInstance();
            graphicInstance.LogicalInstance = logicalInstance;
            Point position = args.GetPosition(this);
            graphicInstance.Parent = FindParent(position, graphicInstance);
            graphicInstance.X = position.X;
            graphicInstance.Y = position.Y;
            graphicInstance.Width = 200;
            graphicInstance.Height = 200;
            InstancesManager.Instance.InstancesContext.GraphicInstances.Add(graphicInstance);
            InstancesManager.Instance.UpdateProperties();
            InstancesManager.Instance.InstancesContext.SubmitChanges(); // to get id for new instance
        }

        private GraphicInstance FindParent(Point position, GraphicInstance instance)
        {
            foreach (var item in (this.Content as CanvasItemsControl).Items)
            {
                GraphicInstance graphicInstance = item as GraphicInstance;
                if (graphicInstance == instance)
                {
                    continue;
                }
                Rect itemBoundingRect = new Rect(graphicInstance.X, graphicInstance.Y, graphicInstance.Width, graphicInstance.Height);
                if (itemBoundingRect.Contains(position))
                {
                    return graphicInstance;
                }
            }
            return null;
        }

        private void CanvasDDTarget_MouseMove(object sender, MouseEventArgs e)
        {
            double rightBound = 0;
            double leftBound = double.PositiveInfinity;
            double bottomBound = 0;
            double topBound = double.PositiveInfinity;
            foreach (var item in (this.Content as CanvasItemsControl).Items)
            {
                GraphicInstance graphicInstance = item as GraphicInstance;
                rightBound = Math.Max(rightBound, graphicInstance.X + graphicInstance.Width + 10);
                leftBound = Math.Min(leftBound, graphicInstance.X - 10);
                topBound = Math.Min(topBound, graphicInstance.Y - 10);
                bottomBound = Math.Max(bottomBound, graphicInstance.Y + graphicInstance.Height + 10);
            }
            if (rightBound > this.Width)
            {
                this.Width = rightBound;
            }
            if (bottomBound > this.Height)
            {
                this.Height = bottomBound;
            }
            if (leftBound < 0)
            {
                foreach (var item in (this.Content as CanvasItemsControl).Items)
                {
                    GraphicInstance graphicInstance = item as GraphicInstance;
                    graphicInstance.X -= leftBound;
                }
            }
            if (topBound < 0)
            {
                foreach (var item in (this.Content as CanvasItemsControl).Items)
                {
                    GraphicInstance graphicInstance = item as GraphicInstance;
                    graphicInstance.Y -= topBound;
                }
            }
        }
    }
}
