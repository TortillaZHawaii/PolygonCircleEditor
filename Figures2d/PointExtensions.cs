using PolygonCircleEditor.Figures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Figures2d
{
    public static class PointExtensions
    {
        public static List<PointInt> BresenhamAlgorithm(PointInt a, PointInt b)
        {
            var points = new List<PointInt>();

            int dx = Math.Abs(a.X - b.X);
            int sx = a.X < b.X ? 1 : -1;
            int dy = -Math.Abs(a.Y - b.Y);
            int sy = a.Y < b.Y ? 1 : -1;

            int err = dx + dy;

            // start at a
            int x = a.X, y = a.X;

            while(true)
            {
                points.Add(new PointInt() { X = x, Y = y });
                
                // if we reached the destination
                if(x == b.X && y == b.Y)
                {
                    break;
                }

                int e2 = 2 * err;
                
                if(e2 >= dy)
                {
                    err += dy;
                    x += sx;
                }

                if(e2 < dx)
                {
                    err += dx;
                    y += sy;
                }
            }

            return points;
        }

        public static List<PointInt> Breline(PointInt a, PointInt b)
        {
            return Breline(a.X, a.Y, b.X, b.Y);
        }

        public static List<PointInt> Breline(int x, int y, int x2, int y2)
        {
            var list = new List<PointInt>();

            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                list.Add(new PointInt(x, y));
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }

            return list;
        }
    }
}
