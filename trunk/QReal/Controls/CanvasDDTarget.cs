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

namespace QReal.Controls
{
    public class CanvasDDTarget : ItemsControlDragDropTarget<CanvasItemsControl, ObjectType>
    {
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

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
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
