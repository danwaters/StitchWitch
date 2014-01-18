using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using MonoTouch.CoreImage;
using StitchWitch.Data;
using Stitcher;

namespace StitchyPhone2
{
	public partial class StitchyPhone2ViewController : UIViewController
	{
		private UIImagePickerController imagePicker;
	    private UIWebView webView;
	    private UIImage pickedImage;

	    private StitchImage stitchedImage;

		public StitchyPhone2ViewController () : base ("StitchyPhone2ViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
		}

		[Obsolete]
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}

		partial void actnOpenClick (NSObject sender)
		{
			this.lblLabel.Text = "Button " + ((UIButton)sender).CurrentTitle + " clicked.";

			imagePicker = new UIImagePickerController
			    {
			        SourceType = UIImagePickerControllerSourceType.PhotoLibrary,
			        MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary)
			    };

		    imagePicker.Canceled += HandleCanceled;
			imagePicker.FinishedPickingImage += HandleFinishedPickingImage;
			imagePicker.FinishedPickingMedia += HandleFinishedPickingMedia;

			NavigationController.PresentViewController(imagePicker, false, null);
		}

		partial void actnManageThreads(NSObject sender)
		{
			var threadSelectionController = new ThreadSelectionController();
			NavigationController.PresentViewController(threadSelectionController, false, null);
		}

		void HandleCanceled (object sender, EventArgs e)
		{
			imagePicker.DismissViewController (false, null);
			this.lblLabel.Text = "That was canceled.";
		}

		void HandleFinishedPickingMedia (object sender, UIImagePickerMediaPickedEventArgs e)
		{
			var image = e.Info [UIImagePickerController.OriginalImage] as UIImage;
		    pickedImage = image;
            imagePicker.DismissViewController (false, null);
		}

		void HandleFinishedPickingImage (object sender, UIImagePickerImagePickedEventArgs e)
		{
			this.imgImage.Image = e.Image;

			imagePicker.DismissViewController (false, null);


		}

		partial void actnDoStuff (NSObject sender)
		{
			DoStuff();
		}

	    private CGBitmapContext CreateARGBBitmapContext(CGImage cgImage)
	    {
            var pixelsWide = cgImage.Width;
            var pixelsHigh = cgImage.Height;
            var bitmapBytesPerRow = pixelsWide * 4;
            var bitmapByteCount = bitmapBytesPerRow * pixelsHigh;

            using (var colorSpace = CGColorSpace.CreateDeviceRGB())
            {
                var bitmapData = Marshal.AllocHGlobal(bitmapByteCount);
                if (bitmapData == IntPtr.Zero)
                {
                    throw new Exception("Could not allocate memory for the image.");
                }

                var context = new CGBitmapContext(bitmapData, pixelsWide, pixelsHigh, 8, bitmapBytesPerRow, colorSpace,
                                                  CGImageAlphaInfo.PremultipliedFirst);

                return context;
            }
	    }

        private IntPtr GetPixelData(UIImage image)
        {
            var imageSize = image.Size;
            var context = CreateARGBBitmapContext(image.CGImage);
            var rectangle = new RectangleF(0.0f, 0.0f, imageSize.Width, imageSize.Height);
            context.DrawImage(rectangle, image.CGImage);
            var data = context.Data;
            return data;
        }

        private unsafe byte GetByte(int offset, IntPtr buffer)
        {
            var bufferAsBytes = (byte*) buffer;
            return bufferAsBytes[offset];
        }

        private unsafe Color GetPixel(int x, int y, IntPtr buffer, int imageWidth)
        {
            var bufferAsBytes = (byte*) buffer;
            var offset = (int)((y * imageWidth + x) * 4);

            return new Color(bufferAsBytes[offset], bufferAsBytes[offset+1], bufferAsBytes[offset+2], bufferAsBytes[offset+3]);
        }

        private unsafe void SetPixel(int x, int y, IntPtr buffer, int imageWidth, Color color)
        {
            var offset = (int) ((y*imageWidth + x)*4);
            SetColor(offset, buffer, color);
        }

        private unsafe void SetColor(int offset, IntPtr buffer, Color color)
        {
            var bufferAsBytes = (byte*) buffer;
            bufferAsBytes[offset] = color.IsTransparent ? (byte)0 : (byte)255;
            bufferAsBytes[offset + 1] = color.Red;
            bufferAsBytes[offset + 2] = color.Green;
            bufferAsBytes[offset + 3] = color.Blue;
        }

        private IntPtr Pixelate(UIImage image, int size)
        {
            var imageSize = image.Size;
            var data = GetPixelData(image);

            for (var x = 0; x < imageSize.Width; x += size)
            {
                for (var y = 0; y < imageSize.Height; y += size)
                {
                    var offsetX = size / 2;
                    var offsetY = size / 2;

                    while (x + offsetX >= imageSize.Width) offsetX--;
                    while (y + offsetY >= imageSize.Height) offsetY--;

                    var color = GetPixel(x + offsetX, y + offsetX, data, (int)imageSize.Width);
                    for (var newx = x; newx < x + size && x < imageSize.Width; newx++)
                    {
                        for (var newy = y; newy < y + size && y < imageSize.Height; newy++)
                        {
                            SetPixel(newx, newy, data, (int)imageSize.Width, color);
                        }
                    }
                }
            }

            return data;
        }

		private void DoStuff()
		{
		    //var data = Pixelate(pickedImage, 4);
		    var data = GetPixelData(pickedImage);
            var pixelHeight = pickedImage.CGImage.Height;
            var pixelWidth = pickedImage.CGImage.Width;

            stitchedImage = new StitchImage();

            for (var row = 0; row < pixelHeight; row ++)
            {
                var colorRow = new ColorRow();
                for (var column = 0; column < pixelWidth; column ++)
                {
                    var startByte = (int) ((row*pixelWidth + column)*4);
                    var alpha = GetByte(startByte, data);
                    var red = GetByte(startByte + 1, data);
                    var blue = GetByte(startByte + 2, data);
                    var green = GetByte(startByte + 3, data);

                    colorRow.Colors.Add(alpha < 255 ? Color.Transparent : new Color(255, red, green, blue));
                }
                stitchedImage.AddRow(colorRow);
            }

		    stitchedImage.CalculateNeededThreads(ThreadRepository.Instance().Threads);
		    var html = stitchedImage.GenerateHtml();
            
		    webView = new UIWebView();
            webView.LoadHtmlString(html, null);

            NavigationController.View = webView;
		}
	}
}

