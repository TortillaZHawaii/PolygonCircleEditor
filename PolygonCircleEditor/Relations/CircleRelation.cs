using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCircleEditor.Relations
{
    public abstract class CircleRelation : Relation
    {
        public abstract Figures2d.Circle Circle { get; init; }
    }
}
