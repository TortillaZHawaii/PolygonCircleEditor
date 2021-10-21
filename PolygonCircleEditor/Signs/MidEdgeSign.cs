using PolygonCircleEditor.Figures;
using PolygonCircleEditor.Icons;
using System.Windows.Media;

namespace PolygonCircleEditor.Signs
{
    public class MidEdgeSign : Sign
    {
        public Polygon Polygon { get; init; }
        public int EdgeNumber { get; init; }

        public MidEdgeSign(Polygon poly, int edgeNumber, ImageSource img)
           : base(poly.GetMidEdgePoint(edgeNumber).X,
                 poly.GetMidEdgePoint(edgeNumber).Y,
                 img)
        {
            Polygon = poly;
            EdgeNumber = edgeNumber;
        }
    }
}
