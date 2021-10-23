using Figures2d;
using PolygonCircleEditor.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCircleEditor.Relations
{
    public abstract class EdgeRelation : Relation
    {
        public abstract Polygon Polygon { get; init; }
        public abstract int EdgeNumber { get; set; }

        public Line GetEdgeLine()
        {
            var (a, b) = Polygon.GetNeighborVertices(EdgeNumber);
            var line = new Line(a, b);

            return line;
        }
    }
}
