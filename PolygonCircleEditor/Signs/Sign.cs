using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace PolygonCircleEditor.Signs
{
    public class Sign : Image
    {
        public Sign(double x, double y, ImageSource img) : base()
        {
            base.Source = img;
            Canvas.SetLeft(this, x - Source.Width / 2);
            Canvas.SetTop(this, y - Source.Height / 2);
        }
    }
}
