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
    public class Port : UserControl
    {
        public Port()
        {
            this.Visibility = Visibility.Collapsed;
            this.Loaded += new RoutedEventHandler(Port_Loaded);
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
    }
}
