using PolygonCircleEditor.Figures;

namespace Figures2d
{
    public class Circle
    {
        public PointInt Point { get; set; }
        public uint Radius { get; set; }

        public Circle(PointInt point, uint radius)
        {
            Point = point;
            Radius = radius;
        }

        public void MoveWhole(int dx, int dy)
        {
            Point = new PointInt(Point.X + dx, Point.Y + dy);
        }
    }
}
