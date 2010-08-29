using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using ObjectTypes;
using QReal.Web.Database;
using QReal.Ria.Database;
using DragEventArgs = Microsoft.Windows.DragEventArgs;
using QReal.Types;

namespace QReal.Controls
{
    public class CanvasDDTarget : ItemsControlDragDropTarget<CanvasItemsControl, ObjectType>
    {
        public CanvasDDTarget()
        {
            this.MouseMove += new MouseEventHandler(CanvasDDTargetMouseMove);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(CanvasDDTargetMouseLeftButtonDown);
        }

        private static void CanvasDDTargetMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UIManager.Instance.SelectedGraphicInstance = null;
        }

        protected override bool CanAddItem(CanvasItemsControl itemsControl, object data)
        {
            if (InstancesManager.Instance.CanvasRootElement != null)
            {
                return true;
            }
            if (data is GraphicInstance)
            {
                return true;
            }
            if (data is Type)
            {
                return (Activator.CreateInstance(data as Type) as ObjectType).CanBeRootItem;
            }
            return false;
        }

        protected override void OnDropOverride(Microsoft.Windows.DragEventArgs args)
        {
            ItemDragEventArgs rawObject = args.Data.GetData(args.Data.GetFormats()[0]) as ItemDragEventArgs;
            object droppedItem = (rawObject.Data as System.Collections.ObjectModel.SelectionCollection).First().Item;
            LogicalInstance logicalInstance = null;
            Type type = null;
            bool shouldCreateItem = false;
            if (droppedItem is Type)
            {
                type = droppedItem as Type;
                if (CanAddItem(null, type))
                {
                    logicalInstance = new LogicalInstance
                        {
                            Name = "anonymous " + (Activator.CreateInstance(type) as ObjectType).TypeName,
                            Type = type.FullName
                        };
                    InstancesManager.Instance.InstancesContext.LogicalInstances.Add(logicalInstance);
                    shouldCreateItem = true;
                }
            }
            else if (droppedItem is GraphicInstance)
            {
                logicalInstance = (droppedItem as GraphicInstance).LogicalInstance;
                type = TypesManager.Instance.GetType(logicalInstance.Type);
                shouldCreateItem = true;
            }
            if (shouldCreateItem)
            {
                CreateItem(args, type, logicalInstance);
            }
        }

        private void CreateItem(DragEventArgs args, Type type, LogicalInstance logicalInstance)
        {
            GraphicInstance graphicInstance = null;
            if (InstancesManager.Instance.CanvasRootElement == null)
            {
                graphicInstance = new RootInstance { LogicalInstance = logicalInstance };
            }
            else
            {
                graphicInstance = CreateGraphicVisualizedInstance(type, logicalInstance, args);
            }
            InstancesManager.Instance.InstancesContext.GraphicInstances.Add(graphicInstance);
            InstancesManager.Instance.AddInstance(graphicInstance);
            InstancesManager.Instance.InstancesContext.SubmitChanges();
        }

        private GraphicInstance CreateGraphicVisualizedInstance(Type type, LogicalInstance logicalInstance,DragEventArgs args)
        {
            Point position = args.GetPosition(this);
            double width = (double) type.GetProperty("Width").GetValue(Activator.CreateInstance(type),null);
            double height = (double)type.GetProperty("Height").GetValue(Activator.CreateInstance(type), null);
            GraphicInstance graphicInstance = null;
            if (type.IsSubclassOf(typeof(NodeType)))
            {
                ParentableInstance parent = (this.Content as CanvasItemsControl).FindParent(position, graphicInstance);
                graphicInstance = new NodeInstance
                                                {
                                                    X = position.X,
                                                    Y = position.Y,
                                                    Width = (width != 0 && !double.IsNaN(width)) ? width : 200,
                                                    Height = (height != 0 && !double.IsNaN(height)) ? height : 200,
                                                    LogicalInstance = logicalInstance,
                                                    Parent = parent
                                                };
            }
            else
            {
                graphicInstance = new EdgeInstance
                                                {
                                                    X = position.X,
                                                    Y = position.Y,
                                                    Width = (width != 0 && !double.IsNaN(width)) ? width : 200,
                                                    Height = (height != 0 && !double.IsNaN(height)) ? height : 200,
                                                    LogicalInstance = logicalInstance,
                                                    Parent = InstancesManager.Instance.CanvasRootElement
                                                };
            }
            return graphicInstance;
        }

        private void CanvasDDTargetMouseMove(object sender, MouseEventArgs e)
        {
            double rightBound = 0;
            double leftBound = double.PositiveInfinity;
            double bottomBound = 0;
            double topBound = double.PositiveInfinity;
            foreach (var item in (this.Content as CanvasItemsControl).Items)
            {
                if (item is NodeInstance)
                {
                    NodeInstance nodeInstance = item as NodeInstance;
                    rightBound = Math.Max(rightBound, nodeInstance.X + nodeInstance.Width + 10);
                    leftBound = Math.Min(leftBound, nodeInstance.X - 10);
                    topBound = Math.Min(topBound, nodeInstance.Y - 10);
                    bottomBound = Math.Max(bottomBound, nodeInstance.Y + nodeInstance.Height + 10);
                }
                else if (item is EdgeInstance)
                {
                    EdgeInstance edgeInstance = item as EdgeInstance;
                    rightBound = Math.Max(rightBound, edgeInstance.X + edgeInstance.Width + 10);
                    leftBound = Math.Min(leftBound, edgeInstance.X - 10);
                    topBound = Math.Min(topBound, edgeInstance.Y - 10);
                    bottomBound = Math.Max(bottomBound, edgeInstance.Y + edgeInstance.Height + 10);
                }
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
                    if (item is NodeInstance)
                    {
                        (item as NodeInstance).X -= leftBound;
                    }
                    else if (item is EdgeInstance)
                    {
                        (item as EdgeInstance).X -= leftBound;
                    }
                }
            }
            if (topBound < 0)
            {
                foreach (var item in (this.Content as CanvasItemsControl).Items)
                {
                    if (item is NodeInstance)
                    {
                        (item as NodeInstance).Y -= topBound;
                    }
                    else if (item is EdgeInstance)
                    {
                        (item as EdgeInstance).Y -= topBound;
                    }
                }
            }
        }
    }
}
