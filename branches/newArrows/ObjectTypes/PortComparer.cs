using System.Windows;
using System.Collections.Generic;

namespace ObjectTypes
{
    public class PortComparer : IComparer<Port>
    {
        private readonly Point myPosition;

        public PortComparer(Point position)
        {
            this.myPosition = position;
        }

        public int Compare(Port first, Port second)
        {
            double lengthDiff = first.GetDistanceToPosition(myPosition) - second.GetDistanceToPosition(myPosition);
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
