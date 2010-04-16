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
    public partial class LinePort : Port
    {
        protected const double WIDTH = 7.0;
        protected const double END_OF_LINE_PORT = 0.9999;

        public LinePort()
        {
            InitializeComponent();
        }

        public override double GetDistanceToPosition(Point position)
        {
            double a = GeometryHelper.GetLineLength(this.RenderSize.Width, this.RenderSize.Height);
            double b = position.GetDistanceToPoint(this.Position);
            double c = position.GetDistanceToPoint(this.EndPoint);

            double nearestPointOfLinePort = GetNearestPointOfLinePort(position);
            if ((nearestPointOfLinePort < 0) || (nearestPointOfLinePort > END_OF_LINE_PORT))
            {
                return Math.Min(b, c);
            }
            else
            {
                double p = (a + b + c) / 2;
                double triangleSquare = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
                double minDistance = 2 * triangleSquare / a;
                return minDistance;
            }
        }

        private Point EndPoint
        {
            get
            {
                return new Point(this.Position.X + this.RenderSize.Width, this.Position.Y + this.RenderSize.Height);
            }
        }

        private double GetNearestPointOfLinePort(Point position)
        {
            double nearestPointOfLinePort = 0;
            if (this.RenderSize.Width == WIDTH)
            {
                nearestPointOfLinePort = (position.Y - this.Position.Y) / (this.EndPoint.Y - this.Position.Y);
            }
            else if (this.RenderSize.Height == HEIGHT)
            {
                nearestPointOfLinePort = (position.X - this.Position.X) / (this.EndPoint.X - this.Position.X);
            }
            else
            {
                double k = (this.EndPoint.Y - this.Position.Y) / (this.EndPoint.X - this.Position.X);
                double b2 = position.Y + 1 / k * position.X;
                double b = this.Position.Y - k * this.Position.X;
                double x3 = k / (1 + k * k) * (b2 - b);
                nearestPointOfLinePort = (x3 - this.Position.X) / (this.EndPoint.X - this.Position.X);
            }
            return nearestPointOfLinePort;
        }

        public override Point GetNearestPointToPosition(Point position)
        {
            Point result = new Point();
            double k = GetNearestPointOfLinePort(position);
            if (k < 0)
            {
                k = 0;    
            }
            else if (k > END_OF_LINE_PORT)
            {
                k = END_OF_LINE_PORT;
            }
            result.X = this.Position.X + k * this.RenderSize.Width;
            result.Y = this.Position.Y + k * this.RenderSize.Height;
            return result;
        }
    }
}
