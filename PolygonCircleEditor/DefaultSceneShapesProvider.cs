using PolygonCircleEditor.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCircleEditor
{
    public static class DefaultSceneShapesProvider
    {
        private static PointInt[] MPoints = new PointInt[]
        {
            new(0,0), new (20, 0), new (80, 200), new (140, 0), 
            new (160, 0),
            new(160, 250), new (140, 250), new (140, 40), new (80, 250), 
            new (20, 40), new (20, 250), new(0, 250)
        };

        private static PointInt[] NPoints = new PointInt[]
        {
            new(0,0), new(20,0), new(80, 200), new(80, 0),
            new(100,0), new(100, 250), new(80, 250),
            new(20, 50), new(20, 250), new(0, 250)
        };

        private static PointInt[] IPoints = new PointInt[]
        {
            new(0,0),new(20, 0), new(20, 250), new(0, 250)
        };

        public static Polygon GenerateLetterM(int dx=0, int dy=0)
        {
            return GenerateFromPoints(MPoints, dx, dy);
        }

        public static Polygon GenerateLetterN(int dx = 0, int dy = 0)
        {
            return GenerateFromPoints(NPoints, dx, dy);
        }

        public static Polygon GenerateLetterI(int dx=0, int dy=0)
        {
            return GenerateFromPoints(IPoints, dx, dy);
        }

        private static Polygon GenerateFromPoints(PointInt[] points, int dx, int dy)
        {
            var shape = new Polygon(points);
            shape.MoveWhole(dx, dy);
            return shape;
        }
    }
}
