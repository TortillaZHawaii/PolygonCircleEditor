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
    }
}
