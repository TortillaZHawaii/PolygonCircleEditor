using Figures2d;
using PolygonCircleEditor.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PolygonCircleEditor.Rasterizers
{
    public class BresenhamRasterizer : IRasterizer
    {
        // adapted from lectures
        public List<Pixel> DrawCircle(Circle circle, Color color)
        {
            var pixels = new List<Pixel>();

            int cX = circle.Point.X;
            int cY = circle.Point.Y;

            int r = (int)circle.Radius;
            int deltaE = 3;
            int deltaSE = 5 - 2 * r;
            int d = 1 - r;
            int x = 0;
            int y = r;

            pixels.Add(new(new PointInt(x + cX, y + cY), color));
            pixels.Add(new(new PointInt(y + cX, -x + cY), color));
            pixels.Add(new(new PointInt(-x + cX, -y + cY), color));
            pixels.Add(new(new PointInt(-y + cX, x + cY), color));
            pixels.Add(new(new PointInt(y + cX, x + cY), color));
            pixels.Add(new(new PointInt(x + cX, -y + cY), color));
            pixels.Add(new(new PointInt(-y + cX, -x + cY), color));
            pixels.Add(new(new PointInt(-x + cX, y + cY), color));

            while (y > x)
            {
                if(d < 0)
                {
                    d += deltaE;
                    deltaE += 2;
                    deltaSE += 2;
                }
                else
                {
                    d += deltaSE;
                    deltaE += 2;
                    deltaSE += 4;
                    y--;
                }
                x++;

                pixels.Add(new(new PointInt(x + cX, y + cY), color));
                pixels.Add(new(new PointInt(y + cX, -x + cY), color));
                pixels.Add(new(new PointInt(-x + cX, -y + cY), color));
                pixels.Add(new(new PointInt(-y + cX, x + cY), color));
                pixels.Add(new(new PointInt(y + cX, x + cY), color));
                pixels.Add(new(new PointInt(x + cX, -y + cY), color));
                pixels.Add(new(new PointInt(-y + cX, -x + cY), color));
                pixels.Add(new(new PointInt(-x + cX, y + cY), color));
            }

            return pixels;
        }

        // adapted from stackoverflow
        public List<Pixel> DrawLine(PointInt start, PointInt end, Color color)
        {
            var pixels = new List<Pixel>();

            int x = start.X;
            int y = start.Y;
            int x2 = end.X;
            int y2 = end.Y;

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
                pixels.Add(new(x, y, color));

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

            return pixels;
        }

        public List<Pixel> DrawPoly(Polygon poly, Color color)
        {
            var completePixels = new List<Pixel>();

            for(int i = 0; i < poly.Points.Count; ++i)
            {
                var start = poly.Points[i];
                var end = poly.Points[(i + 1) % poly.Points.Count];
                var pixels = DrawLine(start, end, color);
                completePixels.AddRange(pixels);
            }

            return completePixels;
        }
    }
}
