using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonCircleEditor.Figures
{
    public class Polygon
    {
        protected List<PointInt> _points;

        public Polygon(IEnumerable<PointInt> points)
        {
            _points = new List<PointInt>(points);
            
            if(_points.Count <= 2)
            {
                throw new ArgumentException("Not enough points to create a polygon.");
            }
        }

        public IReadOnlyList<PointInt> Points => _points;

        public virtual void MoveVertex(int vertexNumber, int dx, int dy)
        {
            var p = _points[vertexNumber];
            _points[vertexNumber] = new PointInt(p.X + dx, p.Y + dy);
        }

        public virtual (int edgeBefore, int edgeAfter) NeighborEdges(int vertexNumber)
        {
            int edgeBefore = (vertexNumber + _points.Count - 1) % _points.Count;
            int edgeAfter = vertexNumber;

            return (edgeBefore, edgeAfter);
        }

        public virtual (int vertexBefore, int vertexAfter) NeighborVertices(int edgeNumber)
        {
            int vertexBefore = edgeNumber;
            int vertexAfter = (edgeNumber + 1) % _points.Count;

            return (vertexBefore, vertexAfter);
        }

        public virtual void RemoveVertex(int vertexNumber)
        {
            _points.RemoveAt(vertexNumber);
        }
        
        public virtual void SplitEdge(int edgeNumber)
        {
            var p1 = _points[edgeNumber];
            var p2 = _points[(edgeNumber + 1) % _points.Count];

            int x = (p1.X + p2.X) / 2;
            int y = (p1.Y + p2.Y) / 2;

            _points.Insert(edgeNumber + 1, new PointInt(x, y));
        }

        public virtual void MoveEdge(int edgeNumber, int dx, int dy)
        {
            int p1 = edgeNumber;
            int p2 = (edgeNumber + 1) % _points.Count;

            _points[p1] = new PointInt(_points[p1].X + dx, _points[p1].Y + dy);
            _points[p2] = new PointInt(_points[p2].X + dx, _points[p2].Y + dy);
        }

        public virtual void MoveWhole(int dx, int dy)
        {
            for(int i = 0; i < _points.Count; ++i)
            {
                _points[i] = new PointInt(_points[i].X + dx, _points[i].Y + dy);
            }
        }
    }
}
