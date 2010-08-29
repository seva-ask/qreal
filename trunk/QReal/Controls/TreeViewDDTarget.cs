using System;
using System.Linq;
using System.Windows.Controls;
using ObjectTypes;
using QReal.Ria.Database;
using QReal.Web.Database;

namespace QReal.Controls
{
    public class TreeViewDDTarget : TreeViewDragDropTarget
    {
        protected override void OnDropOverride(Microsoft.Windows.DragEventArgs args)
        {
            ItemDragEventArgs rawObject = args.Data.GetData(args.Data.GetFormats()[0]) as ItemDragEventArgs;
            Type type = (rawObject.Data as System.Collections.ObjectModel.SelectionCollection).First().Item as Type;
            if (CanAddItem(null, type))
            {
                LogicalInstance logicalInstance = new LogicalInstance
                {
                    Name = "anonymous " + (Activator.CreateInstance(type) as ObjectType).TypeName,
                    Type = type.FullName
                };
                InstancesManager.Instance.InstancesContext.LogicalInstances.Add(logicalInstance);
                GraphicInstance graphicInstance = new RootInstance { LogicalInstance = logicalInstance };
                InstancesManager.Instance.InstancesContext.GraphicInstances.Add(graphicInstance);
                InstancesManager.Instance.AddInstance(graphicInstance);
                InstancesManager.Instance.InstancesContext.SubmitChanges();
            }
        }

        protected override bool CanAddItem(ItemsControl itemsControl, object data)
        {
            if (data is Type)
            {
                return (Activator.CreateInstance(data as Type) as ObjectType).CanBeRootItem;
            }
            return false;
        }
    }
}
