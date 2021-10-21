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
    public class EqualEdgeEmptySign : MidEdgeSign
    {
        public EqualEdgeEmptySign(Polygon poly, int edgeNumber) : base(poly, edgeNumber, IconsRepository.BlueCircle)
        {
        }

        public uint GetEdgeLength()
        {
            return Polygon.GetEdgeLength(EdgeNumber);
        }
    }
}
