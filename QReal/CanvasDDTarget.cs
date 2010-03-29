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

namespace QReal
{
    public class CanvasDDTarget : ItemsControlDragDropTarget<ItemsControl, ObjectType>
    {
        protected override void OnDropOverride(Microsoft.Windows.DragEventArgs args)
        {
            Point position = args.GetPosition(this);
            ItemDragEventArgs rawObject = args.Data.GetData(args.Data.GetFormats()[0]) as ItemDragEventArgs;
            string typeName = (rawObject.Data as System.Collections.ObjectModel.SelectionCollection).First().Item as string;
            ObjectType type = TypeManager.Instance.Objects["Kernel Diagram"][typeName];
            type.Margin = new System.Windows.Thickness(position.X - this.ActualWidth / 2, position.Y - this.ActualHeight / 2, 0, 0);
            (this.Content as ItemsControl).Items.Add(type);
        }
    }
}
