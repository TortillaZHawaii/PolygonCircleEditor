using Figures2d;
using PolygonCircleEditor.Icons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace PolygonCircleEditor.Signs
{
    public class ChangeRadiusSign : MidCircleSign
    {
        public ChangeRadiusSign(Circle circle) 
            : base(circle, IconsRepository.BlackCircle)
        {
        }
    }
}
