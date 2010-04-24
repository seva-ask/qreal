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
using System.Collections.Generic;

namespace ObjectTypes
{
    public class PortComparer : IComparer<Port>
    {
        private Point position;

        public PortComparer(Point position)
        {
            this.position = position;
        }

        public int Compare(Port first, Port second)
        {
            double lengthDiff = first.GetDistanceToPosition(position) - second.GetDistanceToPosition(position);
            if (lengthDiff > 0)
	        {
                return 1;
	        }
            else if (lengthDiff < 0)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
