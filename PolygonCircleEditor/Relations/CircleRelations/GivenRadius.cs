using Figures2d;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCircleEditor.Relations
{
    public class GivenRadius : CircleRelation
    {
        public uint Radius { get; }
        public override Circle Circle { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }

        public GivenRadius(Circle circle, uint radius)
        {
            Radius = radius;
            Circle = circle;
        }

        public override void Adjust()
        {
            Circle.Radius = Radius;
        }

        public override void CleanUp()
        {
            Circle.Relation = null;
        }
    }
}
