using System;
using System.Windows;

namespace ObjectTypes
{
    public partial class LinePort : Port
    {
        protected const double END_OF_LINE_PORT = 0.9999;

        public LinePort()
        {
            InitializeComponent();
        }

        public override double GetDistanceToPosition(Point position)
        {
            double a = GeometryHelper.GetLineLength(TransformedWidth, TransformedHeight);
            double b = position.GetDistanceToPoint(this.Position);
            double c = position.GetDistanceToPoint(this.EndPoint);

            double nearestPointOfLinePort = GetNearestPointOfPort(position);
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
                return new Point(this.Position.X + TransformedWidth, this.Position.Y + TransformedHeight);
            }
        }

        private Rect GetTransformedBoundingRect()
        {
            FrameworkElement parent = this.Parent as FrameworkElement;
            double widthOriginal = this.ActualWidth;
            if (widthOriginal == 0)
	        {
                switch (this.HorizontalAlignment)
	            {
		            case HorizontalAlignment.Center:
                    case HorizontalAlignment.Left:
                    case HorizontalAlignment.Right:
                        widthOriginal = this.Width;
                        break;
                    case HorizontalAlignment.Stretch:
                        widthOriginal = parent.ActualWidth - this.Margin.Left - this.Margin.Right;
                        break;
	            }
            }
            Rect boundingRectOriginal = new Rect(this.Margin.Left, this.Margin.Top, widthOriginal, this.Height);
            return this.RenderTransform.TransformBounds(boundingRectOriginal);
        }

        public override double TransformedWidth
        {
            get
            {
                return GetTransformedBoundingRect().Width;
            }
        }

        public override double TransformedHeight
        {
            get
            {
                return GetTransformedBoundingRect().Height;
            }
        }

        public override double GetNearestPointOfPort(Point position)
        {
            double nearestPointOfLinePort;
            if (TransformedWidth == this.Height)
            {
                nearestPointOfLinePort = (position.Y - this.Position.Y) / (this.EndPoint.Y - this.Position.Y);
            }
            else if (TransformedHeight == this.Height)
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
            double k = GetNearestPointOfPort(position);
            if (k < 0)
            {
                k = 0;    
            }
            else if (k > END_OF_LINE_PORT)
            {
                k = END_OF_LINE_PORT;
            }
            result.X = this.Position.X + k * TransformedWidth;
            result.Y = this.Position.Y + k * TransformedHeight;
            return result;
        }
    }
}
