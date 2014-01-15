using System;
using System.Collections.Generic;

namespace Stitcher
{
	public class StitchImage
	{
		public List<ColorRow> ColorGrid { get; private set; }
		public StitchImage ()
		{
			ColorGrid = new List<ColorRow> ();
		}

		public void AddRow(ColorRow row)
		{
			ColorGrid.Add (row);
		}

		public int Width
		{
			get { return ColorGrid.Count <= 0 ? 0 : ColorGrid[0].Colors.Count; }
		}

		public int Height
		{
			get {
				return ColorGrid.Count;
			}
		}
	}
}

