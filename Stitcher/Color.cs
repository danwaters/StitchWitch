using System;
using System.Collections.Generic;
using System.Globalization;

namespace Stitcher
{
	public class Color
	{
        public byte Red { get; private set; }
        public byte Green { get; private set; }
        public byte Blue { get; private set; }

        public string HexCode { get; private set; }

        public bool IsTransparent { get; private set; }

        public static Color Transparent = new Color(0, 255, 255, 255) { IsTransparent = true };

		public Color (byte a, byte r, byte g, byte b)
		{
		    Red = r;
		    Green = g;
		    Blue = b;

		    if (a < 255)
		    {
		        IsTransparent = true;
		        HexCode = "FFFFFF";
		    }
		    else
		    {
		        IsTransparent = false;
		        HexCode = FormatHex();
		    }
		}

        public Color(string hex)
        {
            hex = hex.Trim().Replace("#", "");

            if (hex.Length != 6)
            {
                throw new Exception("The hex string " + hex + " was invalid.");
            }

            var redString = hex.Substring(0, 2);
            var greenString = hex.Substring(2, 2);
            var blueString = hex.Substring(4, 2);

            var red = byte.Parse(redString, NumberStyles.HexNumber);
            var green = byte.Parse(greenString, NumberStyles.HexNumber);
            var blue = byte.Parse(blueString, NumberStyles.HexNumber);

            Red = red;
            Green = green;
            Blue = blue;

            HexCode = FormatHex();
        }

        private string FormatHex()
        {
            return string.Format("{0:X02}{1:X02}{2:X02}", Red, Green, Blue);
        }

        public Color GetClosestPaletteColor(List<Color> palette)
        {
            if (IsTransparent) return Transparent;
            var currentClosestDistance = int.MaxValue;
            Color closestColor = null;
            foreach (var color in palette)
            {
                var distance = GetColorDistance(this, color);
                if (distance >= currentClosestDistance) continue;

                currentClosestDistance = distance;
                closestColor = color;

                if (distance == 0)
                    break;
            }

            return closestColor;
        }

        private int GetColorDistance(Color a, Color b)
        {
            var dr = a.Red - b.Red;
            var dg = a.Green - b.Green;
            var db = a.Blue - b.Blue;

            return dr * dr + dg * dg + db * db;
        }
	}
}

