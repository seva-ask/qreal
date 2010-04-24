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
using ObjectTypes;

namespace KernelDiagram
{
    public partial class DiagramType : NodeType
    {
        public DiagramType()
        {
            InitializeComponent();
  //          Random a = new Random((int)DateTime.Now.Ticks);
 //           LayoutRoot.Background = new SolidColorBrush(Color.FromArgb(255, (byte)a.Next(255), (byte)a.Next(255), (byte)a.Next(255)));
        }

        public override string TypeName
        {
            get { return "Diagram"; }
        }

        public string MyProperty
        {
            get { return (string)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(string), typeof(ObjectType), null);

        public override bool CanBeRootElement
        {
            get
            {
                return true;
            }
        }
    }
}
