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
        public override Polygon Polygon { get; init; }
        public override int EdgeNumber { get; set; }
        public uint Size { get; init; }

        public EqualEdgeSize(Polygon polygon, int edgeNumber, uint size)
        {
            Polygon = polygon;
            EdgeNumber = edgeNumber;
            Size = size;
        }

        public override void Adjust()
        {
            Polygon.SetEdgeLength(EdgeNumber, Size);
        }

        public override void CleanUp()
        {
            if(PairRelation != null)
            {
                CleanUp(PairRelation);
            }
            CleanUp(this);
        }

        private void CleanUp(EqualEdgeSize relation)
        {
            relation.PairRelation = null;
            relation.Polygon.NullifyRelation(relation.EdgeNumber);

        }

    }
}
