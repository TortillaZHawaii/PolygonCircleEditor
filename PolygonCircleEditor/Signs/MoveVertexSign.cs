using PolygonCircleEditor.Figures;
using PolygonCircleEditor.Icons;

namespace PolygonCircleEditor.Signs
{
    public class MoveVertexSign : VertexSign, IMoveableSign
    {
        public MoveVertexSign(Polygon poly, int vertexNumber) :
            base(poly, vertexNumber, 
                IconsRepository.PinkDiamond)
        {
        }

        public void ChangePositionTo(PointInt point)
        {
            int dx = point.X - Polygon.Points[VertexNumber].X;
            int dy = point.Y - Polygon.Points[VertexNumber].Y;
            Polygon.MoveVertex(VertexNumber, dx, dy);
        }
    }
}
