using PolygonCircleEditor.Figures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Figures2d
{
    public static class PointExtensions
    {
        public static PointInt Midpoint(params PointInt[] points)
        {
            int count = points.Length;
            int x = points.Sum(k => k.X) / count;
            int y = points.Sum(k => k.Y) / count;

            return new PointInt(x, y);
        }

        public static uint Distance(PointInt a, PointInt b)
        {
            int dx = a.X - b.X;
            int dy = a.Y - b.Y;
            return (uint)Math.Sqrt(dx * dx + dy * dy);
        }

        public static PointInt Prolong(PointInt a, PointInt b, uint newSize)
        {
            int dx = b.X - a.X;
            int dy = b.Y - a.Y;
            var vect = new Vector2(dx, dy);
            vect *= ((float)newSize) / vect.Length();
            return new PointInt(a.X + (int)vect.X, a.Y + (int)vect.Y);
        }

        public static (int dx, int dy) ProlongDelta(PointInt a, PointInt b, uint newSize)
        {
            var newB = Prolong(a, b, newSize);
            
            int dx = newB.X - b.X;
            int dy = newB.Y - b.Y;

            return (dx, dy);
        }
    }
}
