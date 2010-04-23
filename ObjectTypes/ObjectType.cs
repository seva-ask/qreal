using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ObjectTypes
{
    public delegate void ZIndexChangedHandler(ObjectType objectType, int newZIndex);

    public abstract class ObjectType : UserControl
    {
        public abstract string TypeName { get; }

        protected ObjectType()
        {
            IsMouseCaptured = false;
            this.MouseLeftButtonDown += new MouseButtonEventHandler(ObjectType_MouseLeftButtonDown);
            this.MouseMove += new MouseEventHandler(ObjectType_MouseMove);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(ObjectType_MouseLeftButtonUp);
        }

        public void SetZIndex(int zIndex)
        {
            ContentPresenter contentPresenter = VisualTreeHelper.GetParent(this.Parent) as ContentPresenter;
            Canvas.SetZIndex(contentPresenter, zIndex);
            if (ZIndexChanged != null)
            {
                ZIndexChanged(this, zIndex);
            }
        }

        public event ZIndexChangedHandler ZIndexChanged;

        private double myMouseX = -1;
        private double myMouseY = -1;

        public bool IsMouseCaptured { get; set; }

        private void ObjectType_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CanMove())
            {
                myMouseY = e.GetPosition(null).Y;
                myMouseX = e.GetPosition(null).X;
                IsMouseCaptured = true;
                this.CaptureMouse();
                SetZIndex(1);
            }
        }

        protected virtual bool CanMove()
        {
            return true;
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

                myPreviousMouseX = myMouseX != -1 ? myMouseX : e.GetPosition(null).X;
                myPreviousMouseY = myMouseY != -1 ? myMouseY : e.GetPosition(null).Y;

                myMouseY = e.GetPosition(null).Y;
                myMouseX = e.GetPosition(null).X;
                OnMoving(deltaX, deltaY);
            }
        }

        protected virtual void OnMoving(double deltaX, double deltaY)
        {
        }

        private double myPreviousMouseX = -1;
        private double myPreviousMouseY = -1;

        public void GetDeltaMouseMove(MouseEventArgs e, out double deltaY, out double deltaX)
        {
            deltaY = e.GetPosition(null).Y - myMouseY;
            deltaX = e.GetPosition(null).X - myMouseX;
            if ((deltaX == 0) && (deltaY == 0))
            {
                deltaY = e.GetPosition(null).Y - myPreviousMouseY;
                deltaX = e.GetPosition(null).X - myPreviousMouseX;
            }
        }

        private void ObjectType_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsMouseCaptured = false;
            this.ReleaseMouseCapture();
            myMouseY = -1;
            myMouseX = -1;
            SetZIndex(0);
        }

        public bool Selected
        {
            get { return (bool)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(bool), typeof(ObjectType), new PropertyMetadata(OnSelectedPropertyChanged));

        private static void OnSelectedPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ObjectType objectType = obj as ObjectType;
            if (objectType.Selected)
            {
                objectType.Select();
            }
            else
            {
                objectType.UnSelect();
            }
        }

        protected virtual void Select()
        {
        }

        protected virtual void UnSelect()
        {
        }
    }
}
