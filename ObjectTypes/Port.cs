using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Browser;

namespace ObjectTypes
{
    public class Port : UserControl
    {
        public Port()
        {
            if (IsNotDesigner())
            {
                this.Visibility = Visibility.Collapsed;
            }
            this.Loaded += new RoutedEventHandler(Port_Loaded);
        }

        private static bool IsNotDesigner()
        {
            return HtmlPage.IsEnabled;
        }

        private void Port_Loaded(object sender, RoutedEventArgs e)
        {
            ObjectType parent = FindObjectTypeParent();
            if (parent != null)
            {
                parent.MouseEnter += new MouseEventHandler(ParentMouseEnter);
                parent.MouseLeave += new MouseEventHandler(ParentMouseLeave);
                parent.SelectedChanged += new SelectedChangedHandler(ParentSelectedChanged);
            }
        }

        private bool myIsParentSelected = false;

        private bool myIsMouseInsideParent = false;

        void ParentSelectedChanged(bool newState)
        {
            myIsParentSelected = newState;
            SetVibility();
        }

        private void SetVibility()
        {
            bool mustBeVisible = myIsParentSelected || myIsMouseInsideParent;
            if (mustBeVisible)
            {
                this.Visibility = Visibility.Visible;
            }
            else
            {
                this.Visibility = Visibility.Collapsed;
            }
        }

        private void ParentMouseLeave(object sender, MouseEventArgs e)
        {
            myIsMouseInsideParent = false;
            SetVibility();
        }

        private void ParentMouseEnter(object sender, MouseEventArgs e)
        {
            myIsMouseInsideParent = true;
            SetVibility();
        }

        private ObjectType FindObjectTypeParent()
        {
            FrameworkElement parent = this.Parent as FrameworkElement;
            while ((parent != null) && (!(parent is ObjectType)))
            {
                parent = parent.Parent as FrameworkElement;
            }
            return parent as ObjectType;
        }

        public Point Position
        {
            get
            {
                if (this.Parent is Canvas)
                {
                    return new Point((double)this.GetValue(Canvas.LeftProperty), (double) this.GetValue(Canvas.TopProperty));
                }
                ObjectType objectType = FindObjectTypeParent();
                Point result = new Point();
                switch (this.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        result.X = this.Margin.Left;
                        break;
                    case HorizontalAlignment.Right:
                        result.X = objectType.Width - this.Margin.Right;
                        break;
                    case HorizontalAlignment.Stretch:
                    case HorizontalAlignment.Center:
                        result.X = (objectType.Width - this.TransformedWidth) / 2;
                        break;
                }
                switch (this.VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        result.Y = objectType.Height - this.Margin.Bottom;
                        break;
                    case VerticalAlignment.Top:
                        result.Y = this.Margin.Top;
                        break;
                    case VerticalAlignment.Stretch:
                    case VerticalAlignment.Center:
                        result.Y = (objectType.Height - this.TransformedHeight) / 2;
                        break;
                }
                return result;
            }
        }

        public virtual double GetDistanceToPosition(Point position)
        {
            return this.Position.GetDistanceToPoint(position);
        }

        public virtual Point GetNearestPointToPosition(Point position)
        {
            return this.Position;
        }

        public virtual double TransformedWidth
        {
            get
            {
                return this.Width;
            }
        }

        public virtual double TransformedHeight
        {
            get
            {
                return this.Height;
            }
        }

        public virtual double GetNearestPointOfPort(Point position)
        {
            return 0;
        }
    }
}
