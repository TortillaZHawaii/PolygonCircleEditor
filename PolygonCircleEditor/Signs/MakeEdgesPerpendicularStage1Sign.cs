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
    public class MakeEdgesPerpendicularStage1Sign : MidEdgeSign
    {
        public MakeEdgesPerpendicularStage1Sign(Polygon poly, int edgeNumber)
            : base(poly, edgeNumber, IconsRepository.RedTangent)
        {
        }

        public Figures2d.Line GetLineGoingThroughEdge()
        {
            var (before, after) = Polygon.GetNeighborVertices(EdgeNumber);
            return new Figures2d.Line(before, after);
        }
    }
}
