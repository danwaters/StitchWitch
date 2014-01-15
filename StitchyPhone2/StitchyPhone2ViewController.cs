using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using MonoTouch.CoreImage;

namespace StitchyPhone2
{
	public partial class StitchyPhone2ViewController : UIViewController
	{
		private UIImagePickerController imagePicker;

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

			imagePicker = new UIImagePickerController();
			imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
			imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);

			imagePicker.Canceled += HandleCanceled;
			imagePicker.FinishedPickingImage += HandleFinishedPickingImage;
			imagePicker.FinishedPickingMedia += HandleFinishedPickingMedia;

			NavigationController.PresentViewController(imagePicker, false, null);
		}

		void HandleCanceled (object sender, EventArgs e)
		{
			imagePicker.DismissViewController (false, null);
			this.lblLabel.Text = "That was canceled.";
		}

		void HandleFinishedPickingMedia (object sender, UIImagePickerMediaPickedEventArgs e)
		{
			var image = e.Info [UIImagePickerController.OriginalImage] as UIImage;
			var height = Chart.FromCIImage (image.CGImage);
			lblLabel.Text = height;
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

		void DoStuff()
		{
			SizeF bitmapSize = new SizeF (imgImage.Image.Size);
			RectangleF rect = new RectangleF (0, 0, bitmapSize.Width, bitmapSize.Height);

			var mono = new CIColorMonochrome {
				Color = CIColor.FromRgb (1, 1, 1),
				Intensity = 1.0f,
				Image = CIImage.FromCGImage (imgImage.Image.CGImage)
			};

			var cgContext = CIContext.FromOptions (null);
			var output = mono.OutputImage;
			var renderedImage = cgContext.CreateCGImage (output, output.Extent);
			imgImage.Image = UIImage.FromImage (renderedImage);

			var cgImage = renderedImage;
			using (CGBitmapContext context = new CGBitmapContext (
				System.IntPtr.Zero, (int)bitmapSize.Width, (int)bitmapSize.Height, 8, (int)(4 * bitmapSize.Width), 
				CGColorSpace.CreateDeviceRGB (), CGImageAlphaInfo.PremultipliedFirst))
			{
				//cgImage.
			}


			//using (CGBitmapContext context = new CGBitmapContext(System.IntPtr.Zero, bitmapSize.Width, 

			// manipulate stuff here
			/*
			using (CGBitmapContext context = new CGBitmapContext (System.IntPtr.Zero, (int)bitmapSize.Width, (int)bitmapSize.Height, 8, (int)(4 * bitmapSize.Width), CGColorSpace.CreateDeviceRGB (), CGImageAlphaInfo.PremultipliedFirst)) {

				context.DrawImage (rect, renderedImage);
				imgImage.Image = UIImage.FromImage (context.ToImage ());

				lblLabel.Text = "I wrote new stuff";
			}*/
		}
	}
}

