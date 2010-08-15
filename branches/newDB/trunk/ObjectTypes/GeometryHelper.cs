using System;
using System.Windows;

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
