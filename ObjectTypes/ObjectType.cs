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
    }
}
