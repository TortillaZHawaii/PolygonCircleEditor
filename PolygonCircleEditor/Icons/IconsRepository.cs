using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PolygonCircleEditor.Icons
{
    public static class IconsRepository
    {
        public static BitmapImage BlackCircle { get; } = CreateFromShortName("blackcircle");

        public static BitmapImage BlueArrows { get; } = CreateFromShortName("bluearrows");
        public static BitmapImage BlueCircle { get; } = CreateFromShortName("bluecircle");
        public static BitmapImage BlueFilledCircle { get; } = CreateFromShortName("bluefilledcircle");

        public static BitmapImage RedCross { get; } = CreateFromShortName("cross");
        public static BitmapImage CrossedRectangle { get; } = CreateFromShortName("crossedrectangle");
        public static BitmapImage WhiteDiamond { get; } = CreateFromShortName("diamond");
        
        public static BitmapImage PinkCircle { get; } = CreateFromShortName("pinkcircle");
        public static BitmapImage PinkDiamond { get; } = CreateFromShortName("pinkdiamond");
        public static BitmapImage PinkStar { get; } = CreateFromShortName("pinkstar");

        public static BitmapImage RedFatCross { get; } = CreateFromShortName("redfatcross");
        public static BitmapImage RedLock { get; } = CreateFromShortName("redlock");
        public static BitmapImage RedTangent { get; } = CreateFromShortName("redtangent");
        public static BitmapImage RedTangentCircle { get; } = CreateFromShortName("redtangentcircle");

        public static BitmapImage WhiteCircledCross { get; } = CreateFromShortName("whitecircledcross");
        public static BitmapImage WhiteStar { get; } = CreateFromShortName("whitestar");

        private static BitmapImage CreateFromShortName(string name)
        {
            var fullname = LongNameFromShort(name);
            var path = $"Icons/{fullname}";
            var uri = new Uri(path, UriKind.Relative);
            return new BitmapImage(uri);
        }

        private static string LongNameFromShort(string name)
        {
            return $"{name}32x32.png";
        }

    }
}
