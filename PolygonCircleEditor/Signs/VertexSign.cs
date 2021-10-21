using PolygonCircleEditor.Figures;
using PolygonCircleEditor.Icons;
using System.Windows.Media;

namespace PolygonCircleEditor.Signs
{
    public class VertexSign : Sign
    {
        public Polygon Polygon { get; init; }
        public int VertexNumber { get; init; }
        public VertexSign(Polygon poly, int vertexNumber, ImageSource img) :
            base(poly.Points[vertexNumber].X,
                poly.Points[vertexNumber].Y, 
                img)
        {
            Polygon = poly;
            VertexNumber = vertexNumber;
        }
    }
}
