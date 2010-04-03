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

namespace ObjectTypes
{
    public abstract class NodeType : ObjectType
    {
        public NodeType()
        {
            this.Loaded += new RoutedEventHandler(NodeType_Loaded);
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
    }
}
