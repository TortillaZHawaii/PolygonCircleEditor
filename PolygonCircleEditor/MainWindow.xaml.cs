using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Figures2d;
using PolygonCircleEditor.Figures;

namespace PolygonCircleEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WriteableBitmap _writeableBitmap;
        Image _image => DisplayImage;
        PointInt _leftPoint = new PointInt();
        PointInt _rightPoint = new PointInt();

        public MainWindow()
        {
            InitializeComponent();

            PresentationSource source = PresentationSource.FromVisual(this);

            //double dpiX, dpiY;
            //if (source != null)
            //{
            //    dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
            //    dpiY = 96.0 * source.CompositionTarget.TransformToDevice.M22;
            //}
            //else
            //{
            //    throw new Exception();
            //}

            _writeableBitmap = new WriteableBitmap(
                (int)512,
                (int)512,
                96,
                96,
                PixelFormats.Bgr32,
                null);

            //_image = new Image()
            //{
            //    Source = _writeableBitmap,
            //    Stretch = Stretch.None,
            //    HorizontalAlignment = HorizontalAlignment.Left,
            //    VerticalAlignment = VerticalAlignment.Top,
            //};

            DisplayImage.MouseLeftButtonDown += new MouseButtonEventHandler(SetLeft);
            DisplayImage.MouseRightButtonDown += new MouseButtonEventHandler(SetRightAndDraw);

            DisplayImage.Source = _writeableBitmap;

            var square = new Polygon(new PointInt[]
            {
                new PointInt(100, 100),
                new PointInt(100, 200),
                new PointInt(200, 200),
                new PointInt(200, 100),
            });

            var triangle = new Polygon(new PointInt[]
            {
                new PointInt(300, 300),
                new PointInt(350, 350),
                new PointInt(250, 350),
            });

            var rng = new Random();

            var points = new List<PointInt>();

            for(int i = 0; i < rng.Next(3, 20); ++i)
            {
                points.Add(new PointInt(rng.Next(512), rng.Next(512)));
            }

            var poly = new Polygon(points);

            square.SplitEdge(0);
            square.MoveVertex(1, 20, 0);
            square.SplitEdge(3);
            square.MoveVertex(4, 20, 0);
            DrawPolygon(square);
            //DrawPolygon(poly);

            //DrawLine(new PointInt(511, 511), new PointInt(256, 256));
        }

        void DrawLine(PointInt a, PointInt b)
        {
            foreach (var point in PointExtensions.Breline(a, b))
            {
                DrawPixel(point.X, point.Y);
            }
        }

        void DrawPolygon(Figures.Polygon polygon)
        {
            for(int i = 0; i < polygon.Points.Count; ++i)
            {
                DrawLine(polygon.Points[i], polygon.Points[(i + 1) % polygon.Points.Count]);
            }
        }

        void DrawPixelMouse(object sender, MouseButtonEventArgs e)
        {
            int x = (int)e.GetPosition(_image).X;
            int y = (int)e.GetPosition(_image).Y;

            DrawPixel(x, y);
        }

        void SetLeft(object sender, MouseButtonEventArgs e)
        {
            int x = (int)e.GetPosition(_image).X;
            int y = (int)e.GetPosition(_image).Y;

            _leftPoint.X = x * 2;
            _leftPoint.Y = y * 2;
        }

        void SetRightAndDraw(object sender, MouseButtonEventArgs e)
        {
            int x = (int)e.GetPosition(_image).X;
            int y = (int)e.GetPosition(_image).Y;

            _rightPoint.X = x * 2;
            _rightPoint.Y = y * 2;

            DrawLine(_leftPoint, _rightPoint);
        }

        // https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.imaging.writeablebitmap?redirectedfrom=MSDN&view=windowsdesktop-5.0
        void DrawPixel(int x, int y)
        {
            int column = x;
            int row = y;

            try
            {
                // Reserve the back buffer for updates.
                _writeableBitmap.Lock();

                unsafe
                {
                    // Get a pointer to the back buffer.
                    IntPtr pBackBuffer = _writeableBitmap.BackBuffer;

                    // Find the address of the pixel to draw.
                    pBackBuffer += row * _writeableBitmap.BackBufferStride;
                    pBackBuffer += column * 4;

                    // Compute the pixel's color.
                    int color_data = 255 << 16; // R
                    color_data |= 128 << 8;   // G
                    color_data |= 255 << 0;   // B

                    // Assign the color data to the pixel.
                    *((int*)pBackBuffer) = color_data;
                }

                // Specify the area of the bitmap that changed.
                _writeableBitmap.AddDirtyRect(new Int32Rect(column, row, 1, 1));
            }
            finally
            {
                // Release the back buffer and make it available for display.
                _writeableBitmap.Unlock();
            }
        }
    }
}
