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
    public class MakeEdgesPerpendicularStage2Sign : MidEdgeSign
    {
        private readonly MakeEdgesPerpendicularStage1Sign _signStage1;

        public MakeEdgesPerpendicularStage2Sign(Polygon poly, int edgeNumber, MakeEdgesPerpendicularStage1Sign firstChosenSign)
            : base(poly, edgeNumber, IconsRepository.RedTangentCircle)
        {
            _signStage1 = firstChosenSign;
        }

        public void MakeEdgePerpendicular()
        {
            var fixedLine = _signStage1.GetLineGoingThroughEdge();
            Polygon.MakeEdgePerpendicular(EdgeNumber, fixedLine);
        }
    }
}
