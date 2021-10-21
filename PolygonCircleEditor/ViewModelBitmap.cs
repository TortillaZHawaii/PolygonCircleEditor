using PolygonCircleEditor.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PolygonCircleEditor
{
    public class ViewModelBitmap
    {
        public WriteableBitmap WriteableBitmap { get; private set; }
        private static readonly double _dpi = 96;

        public ViewModelBitmap(int width, int height)
        {
            WriteableBitmap = new WriteableBitmap(width, height, _dpi, _dpi, PixelFormats.Bgr32, null);
        }

        public void DrawPixel(int x, int y, Color? color = null)
        {
            var colors = new List<Color>();
            if (color != null)
                colors.Add(color.Value);
            else
                colors.Add(Colors.Black);
            DrawPixels(new[] { new PointInt(x, y) }, colors);
        }

        /// <summary>
        /// Clear that needs to happen when bitmap is locked
        /// </summary>
        private Int32Rect _UnsafeClear(Color? color = null)
        {
            var actualColor = color ?? Colors.Gray;

            for (int i = 0; i < WriteableBitmap.PixelWidth; ++i)
            {
                for (int j = 0; j < WriteableBitmap.PixelHeight; ++j)
                {
                    unsafe
                    {
                        IntPtr pBackBuffer = WriteableBitmap.BackBuffer;

                        pBackBuffer += j * WriteableBitmap.BackBufferStride + i * 4;

                        int colorData = actualColor.R << 16;
                        colorData |= actualColor.G << 8;
                        colorData |= actualColor.B << 0;

                        *(int*)pBackBuffer = colorData;
                    }
                }
            }

            return new Int32Rect(0, 0, WriteableBitmap.PixelWidth, WriteableBitmap.PixelHeight);
        }

        private Int32Rect _UnsafeDraw(IEnumerable<PointInt> pixels, List<Color> colors)
        {
            int maxX = 0,
                maxY = 0,
                minX = WriteableBitmap.PixelWidth,
                minY = WriteableBitmap.PixelHeight;
            int i = 0;

            foreach (var pixel in pixels)
            {
                // discard pixels outside of bitmap
                if (pixel.X < 0 || pixel.Y < 0 || pixel.Y >= WriteableBitmap.PixelHeight || pixel.X >= WriteableBitmap.PixelWidth)
                    continue;

                unsafe
                {
                    IntPtr pBackBuffer = WriteableBitmap.BackBuffer;

                    pBackBuffer += pixel.Y * WriteableBitmap.BackBufferStride + pixel.X * 4;

                    Color color = colors[i];

                    int colorData = color.R << 16;
                    colorData |= color.G << 8;
                    colorData |= color.B << 0;

                    *(int*)pBackBuffer = colorData;
                }

                maxX = Math.Max(maxX, pixel.X);
                minX = Math.Min(minX, pixel.X);
                maxY = Math.Max(maxY, pixel.Y);
                minY = Math.Min(minY, pixel.Y);
            }

            // this should be return parameter
            return new Int32Rect(minX, minY, maxX - minX + 1, maxY - minY + 1);
        }

        public void DrawPixels(IEnumerable<PointInt> pixels, List<Color> colors)
        {
            try
            {
                WriteableBitmap.Lock();
                var dirtyRect = _UnsafeDraw(pixels, colors);
                WriteableBitmap.AddDirtyRect(dirtyRect);
            }
            finally
            {
                WriteableBitmap.Unlock();
            }
        }

        public void RedrawPixels(IEnumerable<PointInt> pixels, List<Color> colors)
        {
            try
            {
                WriteableBitmap.Lock();
                var dirtyRect = _UnsafeClear();
                _UnsafeDraw(pixels, colors);
                WriteableBitmap.AddDirtyRect(dirtyRect);
            }
            finally
            {
                WriteableBitmap.Unlock();
            }
        }

        public void Clear(Color color)
        {
            try
            {
                WriteableBitmap.Lock();
                var dirtyRect = _UnsafeClear(color);
                WriteableBitmap.AddDirtyRect(dirtyRect);
            }
            finally
            {
                WriteableBitmap.Unlock();
            }
        }
    }
}
