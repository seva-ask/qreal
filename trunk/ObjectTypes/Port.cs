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
using System.Windows.Browser;

namespace ObjectTypes
{
    public class Port : UserControl
    {
        protected const double HEIGHT = 7.0;

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
            ObjectType parent = GetParent();
            if (parent != null)
            {
                parent.MouseEnter += new MouseEventHandler(parent_MouseEnter);
                parent.MouseLeave += new MouseEventHandler(parent_MouseLeave);
            }
        }

        private void parent_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void parent_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Visibility = Visibility.Visible;
        }

        private ObjectType GetParent()
        {
            FrameworkElement parent = (this.Parent as FrameworkElement);
            while ((parent != null) && (!(parent is ObjectType)))
            {
                parent = parent.Parent as FrameworkElement;
            }
            return parent as ObjectType;
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
                ObjectType objectType = FindObjectTypeParent();
                Point result = new Point();
                switch (this.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                    case HorizontalAlignment.Stretch:
                        result.X = this.Margin.Left;
                        break;
                    case HorizontalAlignment.Right:
                        result.X = objectType.Width - this.Margin.Right;
                        break;
                    default:
                        throw new ArgumentException("Не стоит располагать порт как придется, лучше слева или справа!");
                }
                switch (this.VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        result.Y = objectType.Height - this.Margin.Bottom;
                        break;
                    case VerticalAlignment.Top:
                    case VerticalAlignment.Stretch:
                        result.Y = this.Margin.Top;
                        break;
                    default:
                        throw new ArgumentException("Не стоит располагать порт как придется, лучше сверху или снизу!");
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
    }
}
