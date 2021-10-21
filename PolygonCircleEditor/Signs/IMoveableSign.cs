using PolygonCircleEditor.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCircleEditor.Signs
{
    internal interface IMoveableSign
    {
        public void ChangePositionTo(PointInt point);
    }
}
