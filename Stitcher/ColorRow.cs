using System.Collections.Generic;

namespace Stitcher
{
	public class ColorRow
	{
        public List<Color> Colors { get; private set; }
		public ColorRow ()
		{
            Colors = new List<Color>();
		}
	}
}

