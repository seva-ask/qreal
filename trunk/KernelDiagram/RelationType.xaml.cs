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
    public partial class RelationType : ObjectType
    {
        public RelationType()
        {
            InitializeComponent();
        }

        public override string TypeName
        {
            get { return "Relation"; }
        }
    }
}
