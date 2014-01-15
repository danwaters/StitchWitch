using System;
using MonoTouch.CoreImage;
using MonoTouch.CoreGraphics;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;

namespace StitchyPhone2
{
	public class Chart
	{
		public Chart ()
		{

		}

		public static string FromCIImage(CGImage image)
		{
			var width = image.Width;
			var height = image.Height;

			var rawData = new byte[image.Width * 4 * image.Height];
			var colorSpace = CGColorSpace.CreateDeviceRGB();

			var context = new CGBitmapContext(rawData, image.Width, image.Height, 8, image.BytesPerRow, colorSpace, CGImageAlphaInfo.PremultipliedLast);
			context.DrawImage(new RectangleF(0, 0, image.Width, image.Height), image);

			for (var i = 0; i < rawData.Length; i+=4) {
				var r = rawData[i];
				var g = rawData[i + 1];
				var b = rawData[i + 2];

				if (r != 0) {
					return ToHexString (r, g, b, 0);
				}
			}

			return "Bullshit";
		}

		public static string ToHexString(int r, int g, int b, byte alpha)
		{
			const string format = "0x{0:x2}{1:x2}{2:x2}";
			return string.Format (format, r, g, b);
		}
	}
}

