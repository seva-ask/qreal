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
        }

        private void SetZIndex(int zIndex)
        {
            ContentPresenter contentPresenter = VisualTreeHelper.GetParent(this.Parent) as ContentPresenter;
            Canvas.SetZIndex(contentPresenter, zIndex);
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
            SetZIndex(1);
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
            SetZIndex(0);
        }

        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Id.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(ObjectType), null);

        public string ElementName
        {
            get { return (string)GetValue(ElementNameProperty); }
            set { SetValue(ElementNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ElementName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElementNameProperty =
            DependencyProperty.Register("ElementName", typeof(string), typeof(ObjectType), null);

        public virtual void Select()
        {
            (this.Content as Panel).Background = new SolidColorBrush(Colors.Blue);
        }

        public virtual void UnSelect()
        {
            (this.Content as Panel).Background = null;
        }
    }
}
