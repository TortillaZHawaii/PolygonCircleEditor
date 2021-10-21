using Figures2d;
using PolygonCircleEditor.Figures;
using PolygonCircleEditor.Icons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace PolygonCircleEditor.Signs
{
    public class ChangeEdgeLengthSign : MidEdgeSign
    {
        public ChangeEdgeLengthSign(Polygon poly, int edgeNumber)
            : base(poly, edgeNumber,
                  IconsRepository.BlackCircle)
        {
        }

        public uint GetEdgeLength()
        {
            return Polygon.GetEdgeLength(EdgeNumber);
        }
    }
}
