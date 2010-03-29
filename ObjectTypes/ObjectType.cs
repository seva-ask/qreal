using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace ObjectTypes
{
    public abstract class ObjectType : UserControl
    {
        public abstract string TypeName { get; }

        public ObjectType()
        {
            IsMouseCaptured = false;
            this.MouseLeftButtonDown += new MouseButtonEventHandler(ObjectType_MouseLeftButtonDown);
            this.MouseMove += new MouseEventHandler(ObjectType_MouseMove);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(ObjectType_MouseLeftButtonUp);
            this.Loaded += new RoutedEventHandler(ObjectType_Loaded);
            this.MinHeight = 100;
            this.MinWidth = 100;
        }

        private void ObjectType_Loaded(object sender, RoutedEventArgs e)
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

        private double mouseX = -1;
        private double mouseY = -1;

        public bool IsMouseCaptured { get; set; }

        private void ObjectType_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mouseY = e.GetPosition(null).Y;
            mouseX = e.GetPosition(null).X;
            IsMouseCaptured = true;
            this.CaptureMouse();
            Canvas.SetZIndex(this, 2);
            (this.Parent as Canvas).UpdateLayout();
        }

        private void ObjectType_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseCaptured)
            {
                double deltaY;
                double deltaX;
                GetDeltaMouseMove(e, out deltaY, out deltaX);

                double newTop = deltaY + (double)this.GetValue(Canvas.TopProperty);
                double newLeft = deltaX + (double)this.GetValue(Canvas.LeftProperty);

                this.SetValue(Canvas.TopProperty, newTop);
                this.SetValue(Canvas.LeftProperty, newLeft);

                previousMouseX = mouseX != -1 ? mouseX : e.GetPosition(null).X;
                previousMouseY = mouseY != -1 ? mouseY : e.GetPosition(null).Y;

                mouseY = e.GetPosition(null).Y;
                mouseX = e.GetPosition(null).X;
            }
        }

        private double previousMouseX = -1;
        private double previousMouseY = -1;

        public void GetDeltaMouseMove(MouseEventArgs e, out double deltaY, out double deltaX)
        {
            deltaY = e.GetPosition(null).Y - mouseY;
            deltaX = e.GetPosition(null).X - mouseX;
            if ((deltaX == 0) && (deltaY == 0))
            {
                deltaY = e.GetPosition(null).Y - previousMouseY;
                deltaX = e.GetPosition(null).X - previousMouseX;
            }
        }

        private void ObjectType_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsMouseCaptured = false;
            this.ReleaseMouseCapture();
            mouseY = -1;
            mouseX = -1;
        }

        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Id.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(ObjectType), null);
    }
}
