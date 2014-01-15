using System;
using System.Drawing;
using MonoTouch.CoreImage;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Runtime.InteropServices;

namespace StitchyPhone2
{
	public class ImageHelper
	{
		protected CGBitmapContext CreateARGBBitmapContext(CGImage inImage)
		{
			var pixelsWide = inImage.Width;
			var pixelsHigh = inImage.Height;
			var bitmapBytesPerRow = pixelsWide * 4;
			var bitmapByteCount = bitmapBytesPerRow * pixelsHigh;
			//Note implicit colorSpace.Dispose() 
			using(var colorSpace = CGColorSpace.CreateDeviceRGB())
			{
				//Allocate the bitmap and create context
				var bitmapData = Marshal.AllocHGlobal(bitmapByteCount);
				//I think this is unnecessary, as I believe Marshal.AllocHGlobal will throw OutOfMemoryException
				if(bitmapData == IntPtr.Zero)
				{
					throw new Exception("Memory not allocated.");
				}

				var context = new CGBitmapContext(bitmapData, pixelsWide, pixelsHigh, 8,
				                                  bitmapBytesPerRow, colorSpace, CGImageAlphaInfo.PremultipliedFirst);
				if(context == null)
				{
					throw new Exception("Context not created");
				}
				return context;
			}
		}

		protected IntPtr RequestImagePixelData(UIImage inImage)
		{
			var imageSize = inImage.Size;
			CGBitmapContext ctxt = CreateARGBBitmapContext(inImage.CGImage);
			var rect = new RectangleF(0.0f, 0.0f, imageSize.Width, imageSize.Height);
			ctxt.DrawImage(rect, inImage.CGImage);
			var data = ctxt.Data;
			return data;
		}

		unsafe byte GetByte(int offset, IntPtr buffer)
		{
			byte* bufferAsBytes = (byte*) buffer;
			return bufferAsBytes[offset];
		}
	}
}

