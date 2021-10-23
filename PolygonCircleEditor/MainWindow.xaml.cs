//#define RELATIONS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Figures2d;
using PolygonCircleEditor.Figures;
using PolygonCircleEditor.Rasterizers;
using PolygonCircleEditor.Relations;
using PolygonCircleEditor.Relations.CircleRelations;
using PolygonCircleEditor.Relations.EdgeRelations;
using PolygonCircleEditor.Signs;

namespace PolygonCircleEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public enum CurrentUserModes
    {
        Viewing,
        AddingPoly,
        AddingCircle,
        Moving,
        Removing,
        Spliting,
        Other,
    }

    public partial class MainWindow : Window
    {
        readonly List<Figures.Polygon> _polygons = new();
        readonly List<Circle> _circles = new();
        readonly List<PointInt> _addingPoints = new();
        Ellipse? _firstAddedPoint;
        readonly ViewModelBitmap _bitmap;
        readonly IRasterizer _rasterizer;
        CurrentUserModes _currentUserMode = CurrentUserModes.Viewing;
        Color _backgroundColor;

        private CurrentUserModes _CurrentUserMode
        {
            get => _currentUserMode;
            set
            {
                ClearCanvasAndRedraw();
                RedrawBitmap();
                _currentUserMode = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            _rasterizer = new BresenhamRasterizer();
            _bitmap = new ViewModelBitmap((int)MainCanvas.Width, (int)MainCanvas.Height);
            MainCanvas.Background = new ImageBrush()
            {
                ImageSource = _bitmap.WriteableBitmap,
                Stretch = Stretch.None
            };
            _backgroundColor = Colors.Gainsboro;
            AddDefaultShapesToLists();
            AdjustRelations();
            RedrawBitmap();
        }

        private void AddPoly_Click(object sender, RoutedEventArgs e)
        {
            _CurrentUserMode = CurrentUserModes.AddingPoly;
        }

        private void AddCircle_Click(object sender, RoutedEventArgs e)
        {
            _CurrentUserMode = CurrentUserModes.AddingCircle;
        }

        private void RemovePoints_Click(object sender, RoutedEventArgs e)
        {
            _CurrentUserMode = CurrentUserModes.Removing;
            EnterDeleteMode();
        }

        private void SetTangentCircle_Click(object sender, RoutedEventArgs e)
        {
            _CurrentUserMode = CurrentUserModes.Other;
            EnterTangentCircleSelectCircleMode();
        }

        private void SplitPoints_Click(object sender, RoutedEventArgs e)
        {
            _CurrentUserMode = CurrentUserModes.Spliting;
            EnterSplitMode();
        }

        private void SetEdgeSize_Click(object sender, RoutedEventArgs e)
        {
            _CurrentUserMode = CurrentUserModes.Other;
            EnterSetEdgeLengthMode();
        }

        private void SetRadiusButton_Click(object sender, RoutedEventArgs e)
        {
            _CurrentUserMode = CurrentUserModes.Other;
            EnterSetRadiusSizeMode();
        }

        private void EqualEdgeButton_Click(object sender, RoutedEventArgs e)
        {
            _CurrentUserMode = CurrentUserModes.Other;
            EnterEqualSideStage1();
        }

        private void PerpendicularEdgeButton_Click(object sender, RoutedEventArgs e)
        {
            _CurrentUserMode = CurrentUserModes.Other;
            EnterPerpendicularEdgeStage1Mode();
        }

        private void MainCanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            switch(_CurrentUserMode)
            {
                case CurrentUserModes.AddingCircle:
                    {
                        if (e.LeftButton == MouseButtonState.Pressed)
                            CreateCirclePoint(sender, e);
                        else if (e.RightButton == MouseButtonState.Pressed)
                            CancelCreating();

                        break;
                    }
                case CurrentUserModes.AddingPoly:
                    {
                        if (e.LeftButton == MouseButtonState.Pressed)
                            CreateFigureAddPoint(sender, e);
                        else if (e.RightButton == MouseButtonState.Pressed)
                            CancelCreating();
                        break;
                    }
                case CurrentUserModes.Removing:
                case CurrentUserModes.Viewing:
                default:
                    {
                        if (e.RightButton == MouseButtonState.Pressed)
                        {
                            ClearCanvasAndRedraw();
                        }
                        break;
                    }
            }
        }


        private void CreateCirclePoint(object sender, MouseButtonEventArgs e)
        {
            Point relativeToMainCanvas = e.GetPosition(MainCanvas);

            if(_addingPoints.Count == 0)
            {
                int dotRadius = 5;
                _firstAddedPoint = new Ellipse()
                {
                    Width = dotRadius * 2,
                    Height = dotRadius * 2,
                    Fill = new SolidColorBrush(Colors.Yellow),
                };

                _addingPoints.Add(new PointInt((int)relativeToMainCanvas.X, (int)relativeToMainCanvas.Y));

                // put dot in front
                Panel.SetZIndex(_firstAddedPoint, 100);

                // add and place dot at center of a click
                MainCanvas.Children.Add(_firstAddedPoint);
                Canvas.SetLeft(_firstAddedPoint, relativeToMainCanvas.X - dotRadius);
                Canvas.SetTop(_firstAddedPoint, relativeToMainCanvas.Y - dotRadius);
            }
            else
            {
                // Pythagoras theorem
                int dx = (int)relativeToMainCanvas.X - _addingPoints[0].X;
                int dy = (int)relativeToMainCanvas.Y - _addingPoints[0].Y;

                uint radius = (uint)Math.Sqrt(dx * dx + dy * dy);
                var createdCircle = new Circle(_addingPoints[0], radius);
                _circles.Add(createdCircle);

                var (points, colors) = _rasterizer.DrawCircle(createdCircle, Colors.Black);
                _bitmap.DrawPixels(points, colors);

                CancelCreating();
            }

        }

        private void CreateFigureAddPoint(object sender, MouseButtonEventArgs e)
        {
            Point relativeToMainCanvas = e.GetPosition(MainCanvas);
            
            // first add
            if (_addingPoints.Count == 0)
            {
                int dotRadius = 5;
                _firstAddedPoint = new Ellipse()
                {
                    Width = dotRadius * 2,
                    Height = dotRadius * 2,
                    Fill = new SolidColorBrush(Colors.Yellow),
                };

                // set finishing action
                _firstAddedPoint.MouseLeftButtonDown += OnFirstPoint_Click;

                // put dot in front
                Panel.SetZIndex(_firstAddedPoint, 100);

                // add and place dot at center of a click
                MainCanvas.Children.Add(_firstAddedPoint);
                Canvas.SetLeft(_firstAddedPoint, relativeToMainCanvas.X - dotRadius);
                Canvas.SetTop(_firstAddedPoint, relativeToMainCanvas.Y - dotRadius);
            }
            else
            {
                var line = new System.Windows.Shapes.Line()
                {
                    X1 = _addingPoints.Last().X,
                    X2 = relativeToMainCanvas.X,
                    Y1 = _addingPoints.Last().Y,
                    Y2 = relativeToMainCanvas.Y,
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 3
                };

                MainCanvas.Children.Add(line);
            }

            // every add
            _addingPoints.Add(new PointInt(
                (int)relativeToMainCanvas.X, (int)relativeToMainCanvas.Y));
        }

        private void CancelAddingPoints(object sender, MouseButtonEventArgs e)
        {
            CancelCreating();
        }

        private void CancelCreating()
        {
            if(_firstAddedPoint != null)
            {
                MainCanvas.Children.Clear();
                _firstAddedPoint = null;
                _addingPoints.Clear();
            }
            _CurrentUserMode = CurrentUserModes.Viewing;
        }

        private void OnMovePoint_Click(object sender, RoutedEventArgs e)
        {
            _CurrentUserMode = CurrentUserModes.Moving;
            EnterMoveMode();
        }

        private void EnterSetRadiusSizeMode()
        {
            foreach (var circle in _circles)
            {
                var sign = new ChangeRadiusSign(circle);
                AddSignToForegroundWithLeftButtonAction(sign, SetRadiusClicked);
            }
        }

        private void EnterSetEdgeLengthMode()
        {
            foreach (var poly in _polygons)
            {
                for(int i = 0; i < poly.Points.Count; ++i)
                {
                    var sign = new ChangeEdgeLengthSign(poly, i);
                    AddSignToForegroundWithLeftButtonAction(sign, SetEdgeLengthClicked);
                }
            }
        }

        private void EnterPerpendicularEdgeStage1Mode()
        {
            foreach (var poly in _polygons)
            {
                for (int i = 0; i < poly.Points.Count; ++i)
                {
                    var sign = new MakeEdgesPerpendicularStage1Sign(poly, i);
                    AddSignToForegroundWithLeftButtonAction(sign, PerpendicularStage1Clicked);
                }
            }
        }

        private void EnterPerpendicularEdgeStage2Mode(MakeEdgesPerpendicularStage1Sign chosenSign)
        {
            foreach (var poly in _polygons)
            {
                for (int i = 0; i < poly.Points.Count; ++i)
                {
                    var sign = new MakeEdgesPerpendicularStage2Sign(poly, i, chosenSign);
                    AddSignToForegroundWithLeftButtonAction(sign, PerpendicularStage2Clicked);
                }
            }
        }

        

        private void EnterTangentCircleSelectCircleMode()
        {
            foreach(var circle in _circles)
            {
                var sign = new MakeCircleTangentOnCircleSign(circle);
                AddSignToForegroundWithLeftButtonAction(sign, SelectCircleToTangentClicked);
            }
        }

        private void EnterTangentCircleSelectEdgeMode(Circle circle)
        {
            foreach(var poly in _polygons)
            {
                for(int i = 0; i < poly.Points.Count; ++i)
                {
                    var sign = new MakeTangentCircleOnEdgeSign(poly, i, circle);
                    AddSignToForegroundWithLeftButtonAction(sign, SelectEdgeToTangentCircleClicked);
                }
            }
        }

        private void EnterDeleteMode()
        {
            EnterDeleteVerticesMode();
            EnterDeleteShapesMode();
        }

        private void EnterDeleteVerticesMode()
        {
            foreach (var poly in _polygons)
            {
                if(poly.Points.Count > 3)
                {
                    for (int i = 0; i < poly.Points.Count; ++i)
                    {
                        var deleteSign = new DeleteVertexSign(poly, i);
                        AddSignToForegroundWithLeftButtonAction(deleteSign, DeleteVertexClicked);
                    }
                }
            }
        }

        private void EnterDeleteShapesMode()
        {
            EnterDeletePolyMode();
            EnterDeleteCirclesMode();
        }

        private void EnterDeleteCirclesMode()
        {
            foreach (var circle in _circles)
            {
                var sign = new DeleteShapeSign(circle);
                AddSignToForegroundWithLeftButtonAction(sign, DeleteShapeClicked);
            }
        }

        private void EnterDeletePolyMode()
        {
            foreach(var poly in _polygons)
            {
                var sign = new DeleteShapeSign(poly);
                AddSignToForegroundWithLeftButtonAction(sign, DeleteShapeClicked);
            }
        }

        private void EnterSplitMode()
        {
            foreach (var poly in _polygons)
            {
                for(int i = 0; i < poly.Points.Count; ++i)
                {
                    var splitSign = new SplitSign(poly, i);
                    AddSignToForegroundWithLeftButtonAction(splitSign, SplitSignClicked);
                }
            }
        }

        private void EnterEqualSideStage1()
        {
            foreach (var poly in _polygons)
            {
                for(int i = 0; i < poly.Points.Count; ++i)
                {
                    var equalEdgeSign = new EqualEdgeEmptySign(poly, i);
                    AddSignToForegroundWithLeftButtonAction(equalEdgeSign, EqualEdgeStage1Clicked);
                }
            }
        }

        private void EnterEqualSideStage2(EqualEdgeEmptySign chosenEdge)
        {
            foreach (var poly in _polygons)
            {
                for(int i = 0; i < poly.Points.Count; ++i)
                {
                    if (poly == chosenEdge.Polygon && i == chosenEdge.EdgeNumber)
                        continue;

                    var equalEdgeSign = new EqualEdgeFilledSign(poly, i, chosenEdge);
                    AddSignToForegroundWithLeftButtonAction(equalEdgeSign, EqualEdgeStage2Clicked);
                }
            }
        }

        private void EqualEdgeStage1Clicked(object sender, MouseButtonEventArgs e)
        {
            if (sender is not EqualEdgeEmptySign sign)
                return;

            ClearCanvasAndRedraw();
            EnterEqualSideStage2(sign);
        }

        private void EqualEdgeStage2Clicked(object sender, MouseButtonEventArgs e)
        {
            if (sender is not EqualEdgeFilledSign sign)
                return;

            sign.SetThisEdgeToEqualFirst();
            ClearCanvasAndRedraw();
            EnterEqualSideStage1();
        }

        private void AddSignToForegroundWithLeftButtonAction(Sign sign, MouseButtonEventHandler handler)
        {
            sign.MouseLeftButtonDown += handler;
            MainCanvas.Children.Add(sign);
        }

        private void SelectCircleToTangentClicked(object sender, MouseButtonEventArgs e)
        {
            if (sender is not MakeCircleTangentOnCircleSign sign)
                return;

            var circle = sign.Circle;

            ClearCanvasAndRedraw();
            EnterTangentCircleSelectEdgeMode(circle);
        }

        private void SelectEdgeToTangentCircleClicked(object sender, MouseButtonEventArgs e)
        {
            if (sender is not MakeTangentCircleOnEdgeSign sign)
                return;

            sign.AlignCircleToBeTangent();

            ClearCanvasAndRedraw();
            EnterTangentCircleSelectCircleMode();
        }

        private void SplitSignClicked(object sender, MouseButtonEventArgs e)
        {
            ClearCanvasAndRedraw();
            EnterSplitMode();
        }

        private void DeleteVertexClicked(object sender, MouseButtonEventArgs e)
        {
            ClearCanvasAndRedraw();
            EnterDeleteMode();
        }

        private void DeleteShapeClicked(object sender, MouseButtonEventArgs e)
        {
            if (sender is not DeleteShapeSign sign)
                return;
            
            FindAndDeleteShape(sign.Shape);
            ClearCanvasAndRedraw();
            EnterDeleteMode();
        }

        private void FindAndDeleteShape(IMoveableShape shape)
        {
            foreach(var poly in _polygons)
            {
                if(Object.ReferenceEquals(poly, shape))
                {
                    _polygons.Remove(poly);
                    return;
                }
            }

            foreach(var circle in _circles)
            {
                if(Object.ReferenceEquals(circle, shape))
                {
                    _circles.Remove(circle);
                    return;
                }
            }
        }

        private void SetRadiusClicked(object sender, MouseButtonEventArgs e)
        {
            if (sender is not ChangeRadiusSign sign)
                return;

            var dialog = new PickSizeDialog(sign.Circle.Radius)
            {
                Owner = this
            };

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                sign.Circle.Radius = dialog.Size;
                ClearCanvasAndRedraw();
                EnterSetRadiusSizeMode();
            }
        }

        private void PerpendicularStage1Clicked(object sender, MouseButtonEventArgs e)
        {
            if (sender is not MakeEdgesPerpendicularStage1Sign sign)
                return;

            ClearCanvasAndRedraw();
            EnterPerpendicularEdgeStage2Mode(sign);
        }

        private void PerpendicularStage2Clicked(object sender, MouseButtonEventArgs e)
        {
            if (sender is not MakeEdgesPerpendicularStage2Sign sign)
                return;

            sign.MakeEdgePerpendicular();
            ClearCanvasAndRedraw();
            EnterPerpendicularEdgeStage1Mode();
        }

        private void SetEdgeLengthClicked(object sender, MouseButtonEventArgs e)
        {
            if (sender is not ChangeEdgeLengthSign sign)
                return;

            var length = sign.GetEdgeLength();
            var dialog = new PickSizeDialog(length)
            {
                Owner = this
            };

            bool? result = dialog.ShowDialog();
            if(result == true)
            {
                sign.Polygon.SetEdgeLength(sign.EdgeNumber, dialog.Size);
                ClearCanvasAndRedraw();
                EnterSetEdgeLengthMode();
            }
        }

        private void ClearCanvasAndRedraw()
        {
            AdjustRelations();
            MainCanvas.Children.Clear();
            RedrawBitmap();
        }

        private void OnFirstPoint_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            // if we have enough points we finish adding figure
            if(_addingPoints.Count > 2)
            {
                var createdPoly = new Figures.Polygon(_addingPoints);
                _polygons.Add(createdPoly);

                var (points, colors) = _rasterizer.DrawPoly(createdPoly, Colors.Black);
                _bitmap.DrawPixels(points, colors);

                CancelCreating();
            }
            // throw some error
            else
            {

            }
        }

        private void EnterMoveMode()
        {
            AdjustRelations();
            EnterMoveWholeMode();
            EnterMoveVertexMode();
            EnterMoveEdgesMode();
        }

        private void EnterMoveVertexMode()
        {
            for (int i = 0; i < _polygons.Count; i++)
            {
                for (int j = 0; j < _polygons[i].Points.Count; j++)
                {
                    var moveSign = new MoveVertexSign(_polygons[i], j);
                    AddMoveSignToForeground(moveSign);
                }
            }
        }

        private void EnterMoveWholeMode()
        {
            EnterMoveWholePolyMode();
            EnterMoveWholeCircleMode();
        }

        private void EnterMoveWholeCircleMode()
        {
            foreach (var circle in _circles)
            {
                var moveSign = new MoveWholeSign(circle);
                AddMoveSignToForeground(moveSign);
            }
        }

        private void EnterMoveWholePolyMode()
        {
            foreach (var poly in _polygons)
            {
                var moveSign = new MoveWholeSign(poly);
                AddMoveSignToForeground(moveSign);
            }
        }

        private void EnterMoveEdgesMode()
        {
            foreach(var poly in _polygons)
            {
                for(int i = 0; i < poly.Points.Count; i++)
                {
                    var moveSign = new MoveEdgeSign(poly, i);
                    AddMoveSignToForeground(moveSign);
                }
            }
        }

        private void AddMoveSignToForeground(Sign sign)
        {
            sign.MouseLeftButtonDown += SelectSignToMove;
            sign.MouseLeftButtonUp += DeselectSignToMove;

            MainCanvas.Children.Add(sign);
        }

        private Sign? _selectedSign;

        private void DeselectSignToMove(object sender, MouseButtonEventArgs e)
        {
            _selectedSign = null;
        }

        private void SelectSignToMove(object sender, MouseButtonEventArgs e)
        {
            _selectedSign = sender as Sign;
        }

        private void ClearForeground()
        {
            MainCanvas.Children.Clear();
        }

        private void PositionChanged(object sender, MouseEventArgs e)
        {
            if (sender is not IMoveableSign moveable)
                return;

            Point curPos = e.GetPosition(MainCanvas);
            moveable.ChangePositionTo(new PointInt((int)curPos.X, (int)curPos.Y));
            
            ClearCanvasAndRedraw();
            EnterMoveMode();
        }

        private void RedrawBitmap()
        {
            RedrawBitmapBackground();
            RedrawPolies();
            RedrawCircles();
        }

        private void RedrawBitmapBackground()
        {
            FillBitmapWithColor(_backgroundColor);
        }

        private void FillBitmapWithColor(Color color)
        {
            _bitmap.Clear(color);
        }

        private void RedrawPolies()
        {
            foreach (var poly in _polygons)
            {
                var (points, colors) = _rasterizer.DrawPoly(poly, Colors.Black);
                _bitmap.DrawPixels(points, colors);
            }
        }

        private void RedrawCircles()
        {
            foreach (var circle in _circles)
            {
                var (points, colors) = _rasterizer.DrawCircle(circle, Colors.Black);
                _bitmap.DrawPixels(points, colors);
            }
        }

        private void MainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(_selectedSign != null)
            {
                Point currentPosition = e.GetPosition(MainCanvas);
                if (_selectedSign is IMoveableSign moveable)
                {
                    var currentPositionInt = PointIntFactory.FromDoublePoint(currentPosition);
                    moveable.ChangePositionTo(currentPositionInt);
                    ClearForeground();
                    RedrawBitmap();
                    EnterMoveMode();
                }
            }
        }

        private void AddDefaultShapesToLists()
        {
            var letterM = DefaultSceneShapesProvider.GenerateLetterM(120, 180);
            var letterI1 = DefaultSceneShapesProvider.GenerateLetterI(360, 200);
            var letterN = DefaultSceneShapesProvider.GenerateLetterN(460, 260);
            var letterI2 = DefaultSceneShapesProvider.GenerateLetterI(660, 200);

            _polygons.Add(letterM);
            _polygons.Add(letterI1);
            _polygons.Add(letterN);
            _polygons.Add(letterI2);


            var circle1 = new Circle(new(370, 150), 80);
            var circle2 = new Circle(new(670, 140), 30);

            _circles.Add(circle1);
            _circles.Add(circle2);
#if RELATIONS
            AddTangentRelation(letterI1, 0, circle1);
            AddMGivenEdgeLength(letterM);
            AddIPerpendicularEdges(letterI2);
#endif
        }

        // experimental
#if RELATIONS
        private void AddMGivenEdgeLength(Figures.Polygon letterM)
        {
            for (int i = 0; i < letterM.Points.Count; i++)
            {
                letterM.SetRelation(i, new GivenEdgeLength(letterM, i, letterM.GetEdgeLength(i)));
            }
        }

        private void AddTangentRelation(Figures.Polygon poly, int edgeNumber, Circle circle)
        {
            poly.Relations[edgeNumber]?.CleanUp();
            circle.Relation?.CleanUp();
            var t1 = new TangentCircle(circle);
            var t2 = new TangentEdge(poly, edgeNumber, t1);
            t1.TangentEdge = t2;

            poly.SetRelation(edgeNumber, t2);
            circle.Relation = t1;
        }

        private void AddIPerpendicularEdges(Figures.Polygon letterI)
        {
            var r1 = new PerpendicularEdge(letterI, 0);
            var r2 = new PerpendicularEdge(letterI, 1, r1);
            r2.SecondEdgeRelation = r1;

            letterI.SetRelation(0, r1);
            letterI.SetRelation(1, r2);
        }
#endif

        private void AdjustRelations()
        {
#if RELATIONS
            for (int i = 0; i < 1; i++)
            {
                foreach (var poly in _polygons)
                {
                    for (int j = 0; j < poly.Points.Count; ++j)
                    {
                        poly.Relations[j]?.Adjust();
                    }
                }

                foreach (var circle in _circles)
                {
                    circle.Relation?.Adjust();
                }
            }
#endif
        }
    }
}

