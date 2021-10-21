using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Figures2d
{
    public interface IMoveableShape
    {
        public void MoveWhole(int dx, int dy);
        public PolygonCircleEditor.Figures.PointInt GetMidPoint();
    }
}
