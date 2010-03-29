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
    public partial class DiagramType : ObjectType
    {
        public DiagramType()
        {
            InitializeComponent();
            Random a = new Random((int)DateTime.Now.Ticks);
            LayoutRoot.Background = new SolidColorBrush(Color.FromArgb(255, (byte)a.Next(255), (byte)a.Next(255), (byte)a.Next(255)));
        }

        public override string TypeName
        {
            get { return "Diagram"; }
        }
    }
}
