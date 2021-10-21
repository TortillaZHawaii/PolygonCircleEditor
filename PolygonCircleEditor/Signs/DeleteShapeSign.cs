using Figures2d;
using PolygonCircleEditor.Icons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCircleEditor.Signs
{
    public class DeleteShapeSign : Sign
    {
        public IMoveableShape Shape { get; init; }

        public DeleteShapeSign(IMoveableShape shape) :
            base(shape.GetMidPoint().X,
                shape.GetMidPoint().Y,
                IconsRepository.RedFatCross)
        {
            Shape = shape;
        }
    }
}
