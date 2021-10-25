using PolygonCircleEditor.Figures;
using System.Windows.Media;

namespace PolygonCircleEditor.Rasterizers
{
    public struct Pixel
    {
        public PointInt Point { get; set; }
        public Color Color { get; set; }
        public int X => Point.X;
        public int Y => Point.Y;

        public Pixel(PointInt point, Color color)
        {
            Point = point;
            Color = color;
        }

        public Pixel(int x, int y, Color color)
        {
            Point = new PointInt(x, y);
            Color = color;
        }
    }
}
