using Figures2d;
using PolygonCircleEditor.Figures;
using PolygonCircleEditor.Icons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PolygonCircleEditor.Signs
{
    public class SplitSign : MidEdgeSign
    {
        public SplitSign(Polygon poly, int edgeNumber)
            : base(poly, edgeNumber,
                  IconsRepository.CrossedRectangle)
        {
            MouseLeftButtonDown += SplitEdge;
        }

        private void SplitEdge(object sender, MouseButtonEventArgs e)
        {
            Polygon.SplitEdge(EdgeNumber);
        }
    }
}
