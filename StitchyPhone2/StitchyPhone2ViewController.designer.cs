// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace StitchyPhone2
{
	[Register ("StitchyPhone2ViewController")]
	partial class StitchyPhone2ViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIImageView imgImage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblLabel { get; set; }

		[Action ("actnDoStuff:")]
		partial void actnDoStuff (MonoTouch.Foundation.NSObject sender);

		[Action ("actnManageThreads:")]
		partial void actnManageThreads (MonoTouch.Foundation.NSObject sender);

		[Action ("actnOpenClick:")]
		partial void actnOpenClick (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (imgImage != null) {
				imgImage.Dispose ();
				imgImage = null;
			}

			if (lblLabel != null) {
				lblLabel.Dispose ();
				lblLabel = null;
			}
		}
	}
}
