using Figures2d;
using PolygonCircleEditor.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCircleEditor.Relations
{
    public class PerpendicularEdge : EdgeRelation
    {
        public override Polygon Polygon { get; init; }
        public override int EdgeNumber { get; set; }

        public PerpendicularEdge? SecondEdgeRelation { get; set; }

        public PerpendicularEdge(Polygon polygon, int edgeNumber, PerpendicularEdge? secondEdgeRelation=null)
        {
            Polygon = polygon;
            EdgeNumber = edgeNumber;
            SecondEdgeRelation = secondEdgeRelation;
        }

        public override void Adjust()
        {
            if(SecondEdgeRelation != null)
            {
                var line = SecondEdgeRelation.GetEdgeLine();
                MakeEdgePerpendicular(line);
            }
        }

        public void MakeEdgePerpendicular(Line line)
        {
            Polygon.MakeEdgePerpendicular(EdgeNumber, line);
        }

        public override void CleanUp()
        {
            throw new NotImplementedException();
        }
    }
}
