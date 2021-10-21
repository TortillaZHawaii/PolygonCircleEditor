using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PolygonCircleEditor
{
    internal static class PointIntFactory
    {
        public static Figures.PointInt FromDoublePoint(this Point point)
        {
            return new Figures.PointInt((int)point.X, (int)point.Y);
        }
    }
}
