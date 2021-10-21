using PolygonCircleEditor.Figures;
using PolygonCircleEditor.Icons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCircleEditor.Signs
{
    public class EqualEdgeFilledSign : MidEdgeSign
    {
        readonly EqualEdgeEmptySign _firstEdgeSign;

        public EqualEdgeFilledSign(Polygon poly, int edgeNumber, EqualEdgeEmptySign firstEdgeSign) 
            : base(poly, edgeNumber, IconsRepository.BlueFilledCircle)
        {
            _firstEdgeSign = firstEdgeSign;
        }

        public void SetThisEdgeToEqualFirst()
        {
            uint size = _firstEdgeSign.GetEdgeLength();

            Polygon.SetEdgeLength(EdgeNumber, size);
        }
    }
}
