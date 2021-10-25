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
    // dekorator
    public class ThickRasterizer : IRasterizer
    {
        private IRasterizer _rasterizer;
        private int _thickness;

        public ThickRasterizer(IRasterizer rasterizer, int thickness)
        {
            _rasterizer = rasterizer;
            _thickness = thickness;
        }

        public List<Pixel> DrawCircle(Circle circle, Color color)
        {
            var pixels = _rasterizer.DrawCircle(circle, color);
            return ThickenPixels(pixels);
        }

        public List<Pixel> DrawLine(PointInt start, PointInt end, Color color)
        {
            var line = _rasterizer.DrawLine(start, end, color);
            return ThickenPixels(line);
        }

        public List<Pixel> DrawPoly(Polygon poly, Color color)
        {
            var pixels = _rasterizer.DrawPoly(poly, color);
            return ThickenPixels(pixels);
        }

        private List<Pixel> ThickenPixels(List<Pixel> startingPixels)
        {
            var thickenedPixels = new List<Pixel>();
            
            foreach(var pixel in startingPixels)
            {
                var square = ThickenPixelToSquare(pixel);
                thickenedPixels.AddRange(square);
            }

            return thickenedPixels;
        }

        private List<Pixel> ThickenPixelToSquare(Pixel pixel)
        {
            int halfThickness = (_thickness + 1) / 2;
            int start = -halfThickness;
            int end = halfThickness;

            var pixels = new List<Pixel>();

            for(int i = start; i <= end; i++)
                for(int j = start; j <= end; j++)
                    pixels.Add(new Pixel(i + pixel.X, j + pixel.Y, pixel.Color));

            return pixels;
        }
    }
}
