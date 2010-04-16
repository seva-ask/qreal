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

namespace ObjectTypes
{
    public static class GeometryHelper
    {
        public static double GetDistanceToPoint(this Point first, Point second)
        {
            return GetLineLength(first.X - second.X, first.Y - second.Y);
        }

        public static double GetLineLength(double width, double height)
        {
            return Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2));
        }
    }
}
