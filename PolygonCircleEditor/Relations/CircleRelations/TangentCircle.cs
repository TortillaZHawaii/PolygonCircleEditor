using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Figures2d;
using PolygonCircleEditor.Relations.EdgeRelations;

namespace PolygonCircleEditor.Relations.CircleRelations
{
    public class TangentCircle : CircleRelation
    {
        public override Circle Circle { get; init; }

        public TangentEdge? TangentEdge { get; set; }

        public TangentCircle(Circle circle, TangentEdge? tangentEdge=null)
        {
            Circle = circle;
            TangentEdge = tangentEdge;
        }

        public override void Adjust()
        {
            if(TangentEdge != null)
            {
                AlignCircleToBeTangent(TangentEdge);
            }
        }
        public void AlignCircleToBeTangent(TangentEdge edge)
        {
            var line = edge.GetEdgeLine();
            var distance = line.DistanceToPoint(Circle.Point);
            Circle.Radius = (uint)distance;
        }

        public override void CleanUp()
        {
            CleanUpWithoutSisterRelation();
            TangentEdge?.CleanUpWithoutSisterRelation();
        }

        public void CleanUpWithoutSisterRelation()
        {
            Circle.Relation = null;
            TangentEdge = null;
        }
    }
}
