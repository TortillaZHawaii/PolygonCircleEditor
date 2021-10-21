using Figures2d;
using PolygonCircleEditor.Figures;
using PolygonCircleEditor.Icons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCircleEditor.Signs
{
    public class MoveEdgeSign : MidEdgeSign, IMoveableSign
    {
        public MoveEdgeSign(Polygon poly, int edgeNumber) :
            base(poly, edgeNumber,
                IconsRepository.PinkCircle)
        {
        }

        public void ChangePositionTo(PointInt point)
        {
            var midEdge = Polygon.GetMidEdgePoint(EdgeNumber);
            int dx = point.X - midEdge.X;
            int dy = point.Y - midEdge.Y;
            Polygon.MoveEdge(EdgeNumber, dx, dy);
        }
    }
}
