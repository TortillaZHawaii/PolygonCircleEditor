using Figures2d;
using PolygonCircleEditor.Figures;
using PolygonCircleEditor.Icons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PolygonCircleEditor.Signs
{
    public class MakeTangentCircleOnEdgeSign : MidEdgeSign
    {
        private readonly Circle _circle;

        public MakeTangentCircleOnEdgeSign(Polygon poly, int edgeNumber, Circle circle)
            : base(poly, edgeNumber, IconsRepository.RedTangent)
        {
            _circle = circle;
        }

        public void AlignCircleToBeTangent()
        {
            var (a, b) = RetrieveEdgeNeighborVertecies();
            var line = new Line(a, b);

            var distance = line.DistanceToPoint(_circle.Point);
            _circle.Radius = (uint)distance;
        }

        public (PointInt a, PointInt b) RetrieveEdgeNeighborVertecies()
        {
            var (before, after) = Polygon.GetNeighborVerticesIndexes(EdgeNumber);
            return (Polygon.Points[before], Polygon.Points[after]);
        }
    }
}
