using PolygonCircleEditor.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCircleEditor.Relations
{
    public class GivenEdgeLength : EdgeRelation
    {
        public override Polygon Polygon { get; init; }
        public override int EdgeNumber { get; set; }
        public uint Size { get; init; }

        public GivenEdgeLength(Polygon polygon, int edgeNumber, uint size)
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
            Polygon.NullifyRelation(EdgeNumber);
        }
    }
}
