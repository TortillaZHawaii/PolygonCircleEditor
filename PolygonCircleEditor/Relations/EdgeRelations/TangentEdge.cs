using Figures2d;
using PolygonCircleEditor.Figures;
using PolygonCircleEditor.Relations.CircleRelations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCircleEditor.Relations.EdgeRelations
{
    public class TangentEdge : EdgeRelation
    {
        public CircleRelations.TangentCircle? TangentCircle { get; set; }
        public override Polygon Polygon { get; init; }
        public override int EdgeNumber { get; set; }

        public TangentEdge(Polygon polygon, int edgeNumber, TangentCircle? tangentCircle)
        {
            Polygon = polygon;
            EdgeNumber = edgeNumber;
            TangentCircle = tangentCircle;
        }

        public override void Adjust()
        {
        }

        public override void CleanUp()
        {
            CleanUpWithoutSisterRelation();
            TangentCircle?.CleanUpWithoutSisterRelation();
        }
        
        public void CleanUpWithoutSisterRelation()
        {
            Polygon.NullifyRelation(EdgeNumber);
            TangentCircle = null;
        }
    }
}
