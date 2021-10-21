using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Figures2d;

namespace PolygonCircleEditor.Signs
{
    public class MidCircleSign : Sign
    {
        public Circle Circle { get; init; } 

        public MidCircleSign(Circle circle, ImageSource img)
            : base(circle.Point.X, circle.Point.Y,
                  img)
        {
            Circle = circle;
        }

    }
}
