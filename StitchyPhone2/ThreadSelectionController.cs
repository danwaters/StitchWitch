using System;
using System.Drawing;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using StitchWitch.Data;

namespace StitchyPhone2
{
	public partial class ThreadSelectionController : UIViewController
	{
		public ThreadSelectionController () : base ("ThreadSelectionController", null)
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
		    LoadThreads();
		}

        private void LoadThreads()
        {
            TableView.Source = new ThreadTableSource(ThreadRepository.Instance().Threads.Select(t => t.Name).ToArray());
        }
	}
}

