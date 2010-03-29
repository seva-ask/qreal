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

namespace ObjectTypes
{
    public class ObjectType : UserControl
    {
        public virtual string TypeName 
        { 
            get 
            { 
                return "You shouldn't see it. It is not abstract only because"
                 + "Expression Blend can't parse classes, inherited from abstract!";
            }
        }

        public ObjectType()
        {
            this.MouseLeftButtonDown += new MouseButtonEventHandler(ObjectType_MouseLeftButtonDown);
            this.MouseMove += new MouseEventHandler(ObjectType_MouseMove);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(ObjectType_MouseLeftButtonUp);
        }

        private double mouseX = -1;
        private double mouseY = -1;
        private bool isMouseCaptured = false;

        private void ObjectType_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mouseY = e.GetPosition(null).Y;
            mouseX = e.GetPosition(null).X;
            isMouseCaptured = true;
            this.CaptureMouse();
        }

        private void ObjectType_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseCaptured)
            {
                double deltaY = e.GetPosition(null).Y - mouseY;
                double deltaX = e.GetPosition(null).X - mouseX;

                double newTop = deltaY + (double)this.GetValue(Canvas.TopProperty);
                double newLeft = deltaX + (double)this.GetValue(Canvas.LeftProperty);

                this.SetValue(Canvas.TopProperty, newTop);
                this.SetValue(Canvas.LeftProperty, newLeft);

                mouseY = e.GetPosition(null).Y;
                mouseX = e.GetPosition(null).X;
            }
        }

        private void ObjectType_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isMouseCaptured = false;
            this.ReleaseMouseCapture();
            mouseY = -1;
            mouseX = -1;
        }
    }
}
