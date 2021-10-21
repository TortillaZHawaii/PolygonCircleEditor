using PolygonCircleEditor.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCircleEditor.Relations
{
    public class EqualEdgeSize : EdgeRelation
    {
        public EqualEdgeSize? PairRelation { get; set; }
        public Polygon OwnedPolygon { get; set; }
        public int CorrespondingEdgeNumber { get; set; }

        public EqualEdgeSize(Polygon polygon, int edgeNumber)
        {
            OwnedPolygon = polygon;
            CorrespondingEdgeNumber = edgeNumber;
        }
    }
}
