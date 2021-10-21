using PolygonCircleEditor.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Figures2d
{
    public static class LineExtension
    {
        public static bool IsParallel(this Line a, Line b)
        {
            return AreDoublesEqual(a.A, b.A, a.Epsilon);
        }

        private static bool AreDoublesEqual(double a, double b, double epsilon)
        {
            return Math.Abs(a - b) < epsilon;
        }

        private static int CompareDouble(double a, double b, double epsilon)
        {
            if (AreDoublesEqual(a, b, epsilon))
                return 0;
            else
                return a - b > 0 ? -1 : 1;
        }

        public static bool IsPerpendicular(this Line a, Line b)
        {
            if (AreDoublesEqual(b.A, 0d, a.Epsilon))
            {
                return Double.IsInfinity(a.A);
            }
            else if (AreDoublesEqual(a.A, 0d, a.Epsilon))
            {
                return Double.IsInfinity(b.A);
            }

            return AreDoublesEqual(a.A, -1 / b.A, a.Epsilon);
        }

        public static double DistanceToPoint(this Line line, PointInt point)
        {
            if(double.IsInfinity(line.A))
            {
                return Math.Abs(point.X - line.B);
            }

            double up = Math.Abs(line.A * point.X - point.Y + line.B);
            double down = Math.Sqrt(line.A * line.A + 1);
            return up / down;
        }

        public static Line PerpendicularLineThroughPoint(this Line line, PointInt point)
        {
            if(Double.IsInfinity(line.A))
            {
                return new Line(0, point.Y);
            }
            else if(AreDoublesEqual(0, line.A, line.Epsilon))
            {
                return new Line(double.PositiveInfinity, point.X);
            }

            var newA = -1 / line.A;
            var newB = point.Y - point.X * newA;

            return new Line(newA, newB);
        }

        // https://www.wolframalpha.com/input/?i=solve+for+x%2C+y+%7B%28x-i%29%5E2+%2B+%28y-j%29%5E2+%3D+r+%5E+2%3B+y+%3D+ax+%2B+b%7D&assumption=%22i%22+-%3E+%22Variable%22
        public static (PointInt first, PointInt second) GetIntersectionOfColinearCircleAndLine(this Line line, Circle circle)
        {
            double a = line.A;
            double b = line.B;
            double i = circle.Point.X;
            double j = circle.Point.Y;
            double r = circle.Radius;

            if(Double.IsInfinity(a))
            {
                // colinear is neccessary here
                if (circle.Point.X != line.B)
                    throw new Exception("Not colinear");

                int y1 = circle.Point.Y + (int)circle.Radius;
                int y2 = circle.Point.Y - (int)circle.Radius;

                var p1 = new PointInt(circle.Point.X, y1);
                var p2 = new PointInt(circle.Point.X, y2);

                return (p1, p2);
            }
            else
            {
                // colinear guarantees two solutions
                var sqrt = SqrtUp(a, b, i, j, r);

                var x1 = (-sqrt - a*b + a*j + i) / (a*a + 1);
                var x2 = (sqrt - a*b + a*j + i) / (a*a + 1);
                var y1 = (-a*sqrt - a*a*j + a*i + b) / (a*a + 1);
                var y2 = (a*sqrt - a*a*j + a*i + b) / (a*a + 1);

                var p1 = new PointInt((int)x1, (int)y1);
                var p2 = new PointInt((int)x2, (int)y2);

                return (p1, p2);
            }

        }

        private static double SqrtUp(double a, double b, double i, double j, double r)
        {
            return Math.Sqrt(-a*a*i*i + a*a*r*r - 2*a*b*i + 2*a*i*j - b*b + 2*b*j - j*j + r*r);
        }
    }
}
