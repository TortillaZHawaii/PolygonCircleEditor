using PolygonCircleEditor.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Figures2d
{
    public class Line
    {
        public double A { get; set; } = 0.0d;
        public double B { get; set; } = 0.0d;

        public double Epsilon { get; set; } = 0.0001d;

        public Line(double a=0.0, double b=0.0)
        {
            A = a;
            B = b;
        }

        public Line(PointInt a, PointInt b)
        {
            if (a.X == b.X)
            {
                A = double.PositiveInfinity;
                B = a.X;
            }
            else
            {
                // we need extra precision to avoid numerical errors
                // double division is neccessary
                A = (double)(a.Y - b.Y) / (double)(a.X - b.X);
                B = (a.Y - A * a.X);
            }
        }
    }
}
