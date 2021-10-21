using PolygonCircleEditor.Figures;
using PolygonCircleEditor.Icons;
using System.Windows.Input;

namespace PolygonCircleEditor.Signs
{
    public class DeleteVertexSign : VertexSign
    {
        public DeleteVertexSign(Polygon poly, int vertexNumber) :
            base(poly, vertexNumber, IconsRepository.RedCross)
        {
            MouseLeftButtonDown += DeleteVertex;
        }

        private void DeleteVertex(object sender, MouseButtonEventArgs e)
        {
            if(Polygon.Points.Count > 3)
            {
                Polygon.RemoveVertex(VertexNumber);
            }
        }
    }
}
