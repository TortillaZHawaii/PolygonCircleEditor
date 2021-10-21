using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PolygonCircleEditor.Figures
{
    public struct PointInt
    {
        public int X { get; set; }
        public int Y { get; set; }

        public PointInt(int x=0, int y=0)
        {
            X = x;
            Y = y;
        }

        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }
    }
}
