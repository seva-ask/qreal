﻿using System;
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
using QReal.Ria.Types;
using QReal.Web.Database;
using QReal.Ria.Database;
using System.Diagnostics;
using QReal.Types;

namespace QReal.Controls
{
    public class CanvasDDTarget : ItemsControlDragDropTarget<CanvasItemsControl, ObjectType>
    {
        public CanvasDDTarget()
        {
            this.MouseMove += new MouseEventHandler(CanvasDDTarget_MouseMove);
            this.MouseMove += new MouseEventHandler(CanvasDDTarget_MouseMoveElement);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(CanvasDDTarget_MouseLeftButtonDown);
        }

        private void CanvasDDTarget_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UIManager.Instance.SelectedGraphicInstance = null;
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
                    NodeInstance nodeInstance = (this.Content as CanvasItemsControl).Items.Single(item => item as GraphicInstance == objectType.DataContext) as NodeInstance;
                    if (nodeInstance != null)
                    {
                        foreach (var instanceChild in nodeInstance.Children)
                        {
                            instanceChild.X += deltaX;
                            instanceChild.Y += deltaY;
                        }
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
            Type type = (rawObject.Data as System.Collections.ObjectModel.SelectionCollection).First().Item as Type;
            LogicalInstance logicalInstance = new LogicalInstance();
            logicalInstance.Name = "anonymous " + (Activator.CreateInstance(type) as ObjectType).TypeName;
            logicalInstance.Type = type.FullName;
            InstancesManager.Instance.InstancesContext.LogicalInstances.Add(logicalInstance);
            GraphicVisualizedInstance graphicVisualizedInstance = null;
            if (type.IsSubclassOf(typeof(NodeType)))
            {
                graphicVisualizedInstance = new NodeInstance();
            }
            else
            {
                graphicVisualizedInstance = new EdgeInstance();
            }
            graphicVisualizedInstance.LogicalInstance = logicalInstance;
            Point position = args.GetPosition(this);
            graphicVisualizedInstance.Parent = FindParent(position, graphicVisualizedInstance);
            graphicVisualizedInstance.X = position.X;
            graphicVisualizedInstance.Y = position.Y;
            graphicVisualizedInstance.Width = 200;
            graphicVisualizedInstance.Height = 200;
            InstancesManager.Instance.InstancesContext.GraphicInstances.Add(graphicVisualizedInstance);
        }


        private NodeInstance FindParent(Point position, GraphicInstance instance)
        {
            foreach (var item in (this.Content as CanvasItemsControl).Items)
            {
                NodeInstance nodeInstance = item as NodeInstance;
                if ((nodeInstance == null) || (nodeInstance == instance))
                {
                    continue;
                }
                Rect itemBoundingRect = new Rect(nodeInstance.X, nodeInstance.Y, nodeInstance.Width, nodeInstance.Height);
                if (itemBoundingRect.Contains(position))
                {
                    return nodeInstance;
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
                GraphicVisualizedInstance graphicVisualizedInstance = item as GraphicVisualizedInstance;
                rightBound = Math.Max(rightBound, graphicVisualizedInstance.X + graphicVisualizedInstance.Width + 10);
                leftBound = Math.Min(leftBound, graphicVisualizedInstance.X - 10);
                topBound = Math.Min(topBound, graphicVisualizedInstance.Y - 10);
                bottomBound = Math.Max(bottomBound, graphicVisualizedInstance.Y + graphicVisualizedInstance.Height + 10);
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
                    GraphicVisualizedInstance graphicVisualizedInstance = item as GraphicVisualizedInstance;
                    graphicVisualizedInstance.X -= leftBound;
                }
            }
            if (topBound < 0)
            {
                foreach (var item in (this.Content as CanvasItemsControl).Items)
                {
                    GraphicVisualizedInstance graphicVisualizedInstance = item as GraphicVisualizedInstance;
                    graphicVisualizedInstance.Y -= topBound;
                }
            }
        }
    }
}