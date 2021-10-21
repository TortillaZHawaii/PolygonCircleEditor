using PolygonCircleEditor.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Figures2d
{
    public class LockablePolygon : Polygon
    {
        protected List<bool> _lockedEdges;

        public LockablePolygon(IEnumerable<PointInt> points) : base(points)
        {
            _lockedEdges = new List<bool>(new bool[_points.Count]);
        }

        public override void RemoveVertex(int vertexNumber)
        {
            // unlock neighbors
            (int before, int after) = GetNeighborEdgesIndexes(vertexNumber);
            _lockedEdges[before] = false;
            // remove after unlocking neighbors to avoid problems with indexes
            _lockedEdges.RemoveAt(after);

            base.RemoveVertex(vertexNumber);
        }

        public override void SplitEdge(int edgeNumber)
        {
            // unlocked split edge
            _lockedEdges[edgeNumber] = false;
            _lockedEdges.Insert(edgeNumber, false);

            base.SplitEdge(edgeNumber);
        }

        public virtual void LockEdge(int edgeNumber) => _lockedEdges[edgeNumber] = true;
        public virtual void UnlockEdge(int edgeNumber) => _lockedEdges[edgeNumber] = false;

        public override void MoveVertex(int vertexNumber, int dx, int dy)
        {
            (int before, int after) = GetNeighborEdgesIndexes(vertexNumber);

            // find first not locked edge to the left (decreasing indexes) and to the right (increasing)
            while(_lockedEdges[before] && before != after)
            {
                before = (before + _points.Count - 1) % _points.Count;
            }

            while(_lockedEdges[after] && after != before)
            {
                after = (after + 1) % _points.Count;
            }

            // TODO: move all locked together edges

        }
    }
}
