using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ObjectTypes
{
    public delegate void ZIndexChangedHandler(ObjectType objectType, int newZIndex);

    public delegate void ClickHandler(ObjectType sender);

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
            this.MousePress();
            if (CanMove())
            {
                myMouseY = e.GetPosition(null).Y;
                myMouseX = e.GetPosition(null).X;
                IsMouseCaptured = true;
                this.CaptureMouse();
            }
            e.Handled = true;
        }

        protected void MousePress()
        {
            SetZIndex(1);
            if (Clicked != null)
            {
                Clicked(this);
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
                Point mousePosition = e.GetPosition(null);
                double deltaX = mousePosition.X - myMouseX;
                double deltaY = mousePosition.Y - myMouseY;
                Move(deltaX, deltaY);
                myMouseX = mousePosition.X;
                myMouseY = mousePosition.Y;
            }
        }

        protected virtual void Move(double deltaX, double deltaY)
        {
            double newTop = deltaY + (double)this.GetValue(Canvas.TopProperty);
            double newLeft = deltaX + (double)this.GetValue(Canvas.LeftProperty);
            this.SetValue(Canvas.TopProperty, newTop);
            this.SetValue(Canvas.LeftProperty, newLeft);
        }

        private void ObjectType_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MouseRelease();
            IsMouseCaptured = false;
            this.ReleaseMouseCapture();
            myMouseY = -1;
            myMouseX = -1;
        }

        protected void MouseRelease()
        {
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

        public virtual bool CanBeRootItem
        {
            get
            {
                return false;
            }
        }

        public event ClickHandler Clicked;
    }
}
