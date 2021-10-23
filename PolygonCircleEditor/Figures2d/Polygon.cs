using Figures2d;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolygonCircleEditor.Relations;

namespace PolygonCircleEditor.Figures
{
    public class Polygon : IMoveableShape
    {
        protected List<PointInt> _points;
        protected List<EdgeRelation?> _relations;
        public IReadOnlyList<PointInt> Points => _points;
        public IReadOnlyList<EdgeRelation?> Relations => _relations;

        public Polygon(IEnumerable<PointInt> points)
        {
            _points = new List<PointInt>(points);

            var prefilledRelations = new EdgeRelation?[_points.Count];
            _relations = new List<EdgeRelation?>(prefilledRelations);

            if (_points.Count <= 2)
            {
                throw new ArgumentException("Not enough points to create a polygon.");
            }
        }



        public PointInt GetMidEdgePoint(int edgeNumber)
        {
            var (before, after) = GetNeighborVerticesIndexes(edgeNumber);
            var edgeMidPoint = PointExtensions.Midpoint(Points[before], Points[after]);

            return edgeMidPoint;
        }

        public PointInt GetMidPoint() => GetMidWholePoint();

        public PointInt GetMidWholePoint()
        {
            return PointExtensions.Midpoint(Points.ToArray());
        }

        public virtual void MoveVertex(int vertexNumber, int dx, int dy)
        {
            var p = _points[vertexNumber];
            _points[vertexNumber] = new PointInt(p.X + dx, p.Y + dy);
        }

        public virtual (int edgeBefore, int edgeAfter) GetNeighborEdgesIndexes(int vertexNumber)
        {
            int edgeBefore = (vertexNumber + _points.Count - 1) % _points.Count;
            int edgeAfter = vertexNumber;

            return (edgeBefore, edgeAfter);
        }

        public virtual (int vertexBefore, int vertexAfter) GetNeighborVerticesIndexes(int edgeNumber)
        {
            int vertexBefore = edgeNumber;
            int vertexAfter = (edgeNumber + 1) % _points.Count;

            return (vertexBefore, vertexAfter);
        }

        public virtual (PointInt vertexBefore, PointInt vertexAfter) GetNeighborVertices(int edgeNumber)
        {
            var (indexBefore, indexAfter) = GetNeighborVerticesIndexes(edgeNumber);
            
            var pointBefore = Points[indexBefore];
            var pointAfter = Points[indexAfter];

            return (pointBefore, pointAfter);
        }

        public virtual void RemoveVertex(int vertexNumber)
        {
            var (before, after) = GetNeighborEdgesIndexes(vertexNumber);
            _relations[before]?.CleanUp();
            _relations[after]?.CleanUp();
            
            _relations.RemoveAt(vertexNumber);
            _points.RemoveAt(vertexNumber);
        }

        public virtual void SplitEdge(int edgeNumber)
        {
            var p1 = _points[edgeNumber];
            var p2 = _points[(edgeNumber + 1) % _points.Count];

            int x = (p1.X + p2.X) / 2;
            int y = (p1.Y + p2.Y) / 2;

            RemoveRelation(edgeNumber);
            _relations.Insert(edgeNumber + 1, null);
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
            for (int i = 0; i < _points.Count; ++i)
            {
                _points[i] = new PointInt(_points[i].X + dx, _points[i].Y + dy);
            }
        }

        public virtual void MoveEdgesBy(int dx, int dy, params int[] edgeNumbers)
        {
            var verticesToMove = FindCommonVerticesFromEdges(edgeNumbers);

            foreach(int vertex in verticesToMove)
            {
                MoveVertex(vertex, dx, dy);
            }
        }

        public virtual void SetEdgeLengthFromMid(int edgeNumber, uint size)
        {
            // todo: clean up
            var (before, after) = GetNeighborVertices(edgeNumber);
            //var (dx, dy) = PointExtensions.ProlongDelta(before, after, size);

            var midPoint = GetMidEdgePoint(edgeNumber);
            var line = new Line(before, after);

            var circle = new Circle(midPoint, size / 2);
            var (p1, p2) = LineExtension.GetIntersectionOfColinearCircleAndLine(line, circle);

            var (beforeIndex, afterIndex) = GetNeighborVerticesIndexes(edgeNumber);

            var distance1 = PointExtensions.Distance(before, p1);
            var distance2 = PointExtensions.Distance(before, p2);

            if(distance1 <= distance2)
            {
                _points[beforeIndex] = p1;
                _points[afterIndex] = p2;
            }
            else
            {
                _points[beforeIndex] = p2;
                _points[afterIndex] = p1;
            }
        }

        public virtual void SetEdgeLength(int edgeNumber, uint size, int startingVertex=-1)
        {
            // todo: clean up
            var (before, after) = GetNeighborVerticesIndexes(edgeNumber);

            bool isAfterStartingVertex = startingVertex == after;

            int vertexIndexToStay = !isAfterStartingVertex ? before : after;
            int vertexIndexToMove = !isAfterStartingVertex ? after : before;

            var vertexToStay = _points[vertexIndexToStay];
            var vertexToMove = _points[vertexIndexToMove];

            var (dx, dy) = PointExtensions.ProlongDelta(vertexToStay, vertexToMove, size);
            MoveVertex(vertexIndexToMove, dx, dy);
        }

        public virtual uint GetEdgeLength(int edgeNumber)
        {
            var (before, after) = GetNeighborVerticesIndexes(edgeNumber);
            
            var beforePoint = _points[before];
            var afterPoint = _points[after];

            return PointExtensions.Distance(beforePoint, afterPoint);
        }

        private IEnumerable<int> FindCommonVerticesFromEdges(params int[] edgeNumbers)
        {
            var verticesToMove = new HashSet<int>();

            foreach (int edgeNumber in edgeNumbers)
            {
                var (before, after) = GetNeighborVerticesIndexes(edgeNumber);
                verticesToMove.Add(before);
                verticesToMove.Add(after);
            }

            return verticesToMove;
        }

        public void MakeEdgePerpendicular(int edgeNumber, Line line)
        {
            var (before, _) = GetNeighborVertices(edgeNumber);
            var (_, afterIndex) = GetNeighborVerticesIndexes(edgeNumber);

            var perpendicularLine = LineExtension.PerpendicularLineThroughPoint(line, before);
            var edgeLength = GetEdgeLength(edgeNumber);

            var circle = new Circle(before, edgeLength);
            var (_, afterPoint) = LineExtension.GetIntersectionOfColinearCircleAndLine(perpendicularLine, circle);

            _points[afterIndex] = afterPoint;
        }

        public void SetRelation(int edgeNumber, EdgeRelation relation)
        {
            RemoveRelation(edgeNumber);
            _relations[edgeNumber] = relation;
        }

        public void RemoveRelation(int edgeNumber)
        {
            _relations[edgeNumber]?.CleanUp();
        }

        public void NullifyRelation(int edgeNumber)
        {
            _relations[edgeNumber] = null;
        }
    }
}
