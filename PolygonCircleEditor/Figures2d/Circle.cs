using PolygonCircleEditor.Figures;

namespace Figures2d
{
    public class Circle : IMoveableShape
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

        public PointInt GetMidPoint() => Point;
    }
}
