using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Stitcher
{
	public class StitchImage
	{
		public List<ColorRow> ColorGrid { get; private set; }
        public List<Thread> RequiredThreads { get; private set; } 
		public StitchImage ()
		{
			ColorGrid = new List<ColorRow> ();
            RequiredThreads = new List<Thread>();
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

	    public void CalculateNeededThreads(List<Thread> availableThreads)
	    {
            RequiredThreads = new List<Thread>();
	        var availableColors = availableThreads.Select(t => new Color(t.HexCode)).ToList();

            foreach (var row in ColorGrid)
            {
                foreach (var color in row.Colors)
                {
                    var nearestAvailableColor = color.GetClosestPaletteColor(availableColors);
                    if (nearestAvailableColor.IsTransparent) continue;
                    
                    var thread = availableThreads.First(t => t.HexCode.Trim() == nearestAvailableColor.HexCode);

                    if (!RequiredThreads.Any(t => t.HexCode.Trim() == thread.HexCode.Trim()))
                        RequiredThreads.Add(thread);
                }
            }

	        RequiredThreads = RequiredThreads.OrderBy(t => t.DMC).ToList();

            for (var i = 0; i < RequiredThreads.Count; i ++)
            {
                RequiredThreads[i].GlyphCode = Glyphs.AvailableGlyphs[i];
            }
	    }

        public string GenerateHtml()
        {
            var sb = new StringBuilder();

            sb.Append("<html><head><title>StitchWitch</title><style>" +
                      ".glyph {font-size: 20px; font-weight: bold; }table td { width: 15px; height: 15px; min-width: 15px; min-height: 15px; text-align: center; border:1px solid #efefef; display:inline-block; overflow:hidden; font-family: Arial; font-size: 12px; } table tr {height: 15px; }table { empty-cells: show; border: 2px solid black; } td:nth-of-type(10n) {border-right: 2px solid black;} tr:nth-of-type(10n) td {border-bottom: 2px solid black;}</style></head>");
            sb.Append("<body>");
            sb.Append("<h2>Threads Needed</h2>");
            sb.Append("<ul>");
            foreach (var thread in RequiredThreads)
            {
                sb.Append("<li>");
                sb.Append(string.Format("<span class=\"glyph\">{0}</span> {1} {2}", thread.GlyphCode, thread.DMC, thread.Name));
                sb.Append(
                    string.Format(
                        "<div style=\"width: 100px; background-color: #{0}; border: 1px solid black\">&nbsp;</div>",
                    thread.HexCode.Trim()));
                sb.Append("</li>");
            }
            sb.Append("</ul>");
            sb.Append(GetTableHtml());
            sb.Append("</body></html>");
            return sb.ToString();
        }

        private string GetTableHtml()
        {
            var sb = new StringBuilder();
            sb.Append("<table cellpadding=\"0\" cellspacing=\"0\">");
            foreach (var row in ColorGrid)
            {
                sb.Append("<tr>");
                foreach (var color in row.Colors)
                {
                    var nearestColor = color.GetClosestPaletteColor(RequiredThreads.Select(t => new Color(t.HexCode)).ToList());
                    sb.Append("<td>");
                    if (color.IsTransparent == false)
                        sb.Append(RequiredThreads.Single(t => t.HexCode.Trim() == nearestColor.HexCode.Trim()).GlyphCode);
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }
	}
}

