using System;
using System.ServiceModel.DomainServices.Client;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using ObjectTypes;
using QReal.Web.Database;
using QReal.Ria.Database;
using DragEventArgs = Microsoft.Windows.DragEventArgs;

namespace QReal.Controls
{
    public class CanvasDDTarget : ItemsControlDragDropTarget<CanvasItemsControl, ObjectType>
    {
        public CanvasDDTarget()
        {
            this.MouseMove += new MouseEventHandler(CanvasDDTarget_MouseMove);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(CanvasDDTarget_MouseLeftButtonDown);
        }

        private static void CanvasDDTarget_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UIManager.Instance.SelectedGraphicInstance = null;
        }

        protected override bool CanAddItem(CanvasItemsControl itemsControl, object data)
        {
            Type type = data as Type;
            return (InstancesManager.Instance.CanvasRootElement != null) || ((Activator.CreateInstance(type) as ObjectType).CanBeRootItem);
        }

        protected override void OnDropOverride(Microsoft.Windows.DragEventArgs args)
        {
            ItemDragEventArgs rawObject = args.Data.GetData(args.Data.GetFormats()[0]) as ItemDragEventArgs;
            Type type = (rawObject.Data as System.Collections.ObjectModel.SelectionCollection).First().Item as Type;

            if (CanAddItem(null, type))
            {
                LogicalInstance logicalInstance = new LogicalInstance
                {
                    Name =
                        "anonymous " +
                        (Activator.CreateInstance(type) as ObjectType).TypeName,
                    Type = type.FullName
                };
                InstancesManager.Instance.InstancesContext.LogicalInstances.Add(logicalInstance);
                GraphicInstance graphicInstance = null;
                if (InstancesManager.Instance.CanvasRootElement == null)
                {
                    graphicInstance = new RootInstance { LogicalInstance = logicalInstance };
                    InstancesManager.Instance.CanvasRootElement = graphicInstance as RootInstance;
                }
                else
                {
                    graphicInstance = CreateGraphicVisualizedInstance(type, logicalInstance, args);
                }
                InstancesManager.Instance.InstancesContext.GraphicInstances.Add(graphicInstance);
                InstancesManager.Instance.AddInstance(graphicInstance);
       //         InstancesManager.Instance.InstancesContext.SubmitChanges();
            }
        }

        private GraphicInstance CreateGraphicVisualizedInstance(Type type, LogicalInstance logicalInstance,DragEventArgs args)
        {
            Point position = args.GetPosition(this);
            double width = (double) type.GetProperty("Width").GetValue(Activator.CreateInstance(type),null);
            double height = (double)type.GetProperty("Height").GetValue(Activator.CreateInstance(type), null);
            GeometryInformation geometryInformation = new GeometryInformation
                                                          {
                                                              X = position.X,
                                                              Y = position.Y,
                                                              Width = (width != 0 && !double.IsNaN(width)) ? width : 200,
                                                              Height = (height != 0 && !double.IsNaN(height)) ? height : 200
                                                          };
            InstancesManager.Instance.InstancesContext.GeometryInformations.Add(geometryInformation);
            GraphicInstance graphicInstance = null;
            if (type.IsSubclassOf(typeof(NodeType)))
            {
                ParentableInstance parent = FindParent(position, graphicInstance);
                graphicInstance = new NodeInstance
                                                {
                                                    GeometryInformation = geometryInformation,
                                                    LogicalInstance = logicalInstance,
                                                    Parent = parent
                                                };
            }
            else
            {
                graphicInstance = new EdgeInstance
                                                {
                                                    GeometryInformation = geometryInformation,
                                                    LogicalInstance = logicalInstance,
                                                    Parent = InstancesManager.Instance.CanvasRootElement
                                                };
            }
            return graphicInstance;
        }

        private ParentableInstance FindParent(Point position, GraphicInstance instance)
        {
            foreach (var item in (this.Content as CanvasItemsControl).Items)
            {
                NodeInstance nodeInstance = item as NodeInstance;
                if ((nodeInstance == null) || (nodeInstance == instance))
                {
                    continue;
                }
                GeometryInformation geom = nodeInstance.GeometryInformation;
                Rect itemBoundingRect = new Rect(geom.X, geom.Y, geom.Width, geom.Height);
                if (itemBoundingRect.Contains(position))
                {
                    return nodeInstance;
                }
            }
            return InstancesManager.Instance.CanvasRootElement;
        }

        private void CanvasDDTarget_MouseMove(object sender, MouseEventArgs e)
        {
            double rightBound = 0;
            double leftBound = double.PositiveInfinity;
            double bottomBound = 0;
            double topBound = double.PositiveInfinity;
            foreach (var item in (this.Content as CanvasItemsControl).Items)
            {
                GeometryInformation geom = (item as GraphicInstance).GeometryInformation;
                rightBound = Math.Max(rightBound, geom.X + geom.Width + 10);
                leftBound = Math.Min(leftBound, geom.X - 10);
                topBound = Math.Min(topBound, geom.Y - 10);
                bottomBound = Math.Max(bottomBound, geom.Y + geom.Height + 10);
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
                    GeometryInformation geom = (item as GraphicInstance).GeometryInformation;
                    geom.X -= leftBound;
                }
            }
            if (topBound < 0)
            {
                foreach (var item in (this.Content as CanvasItemsControl).Items)
                {
                    GeometryInformation geom = (item as GraphicInstance).GeometryInformation;
                    geom.Y -= topBound;
                }
            }
        }
    }
}
