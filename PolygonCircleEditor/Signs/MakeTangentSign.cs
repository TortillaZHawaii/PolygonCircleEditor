using Figures2d;
using PolygonCircleEditor.Icons;

namespace PolygonCircleEditor.Signs
{
    public class MakeCircleTangentOnCircleSign : MidCircleSign
    {
        public MakeCircleTangentOnCircleSign(Circle circle) : base(circle, IconsRepository.RedTangentCircle)
        {
        }
    }
}