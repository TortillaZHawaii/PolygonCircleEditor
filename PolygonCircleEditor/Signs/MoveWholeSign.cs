using Figures2d;
using PolygonCircleEditor.Figures;
using PolygonCircleEditor.Icons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCircleEditor.Signs
{
    public class MoveWholeSign : Sign, IMoveableSign
    {
        // ref to moveable
        public IMoveableShape moveableShape{ get; init; }
        // vertex number

        public MoveWholeSign(IMoveableShape shape) :
            base(shape.GetMidPoint().X,
                shape.GetMidPoint().Y,
                IconsRepository.PinkStar)
        {
            moveableShape = shape;
        }

        public void ChangePositionTo(PointInt point)
        {
            var midPoint = moveableShape.GetMidPoint();
            int dx = point.X - midPoint.X;
            int dy = point.Y - midPoint.Y;
            moveableShape.MoveWhole(dx, dy);
        }
    }
}
