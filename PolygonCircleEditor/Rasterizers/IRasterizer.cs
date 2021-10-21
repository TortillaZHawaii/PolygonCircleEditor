using Figures2d;
using PolygonCircleEditor.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PolygonCircleEditor.Rasterizers
{
    public interface IRasterizer
    {
        public (List<PointInt> points, List<Color> colors) DrawLine(PointInt start, PointInt end, Color color);
        public (List<PointInt> points, List<Color> colors) DrawPoly(Polygon poly, Color color);
        public (List<PointInt> points, List<Color> colors) DrawCircle(Circle circle, Color color);
    }
}
