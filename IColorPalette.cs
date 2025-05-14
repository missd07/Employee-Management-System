using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpMS
{
	internal interface IColorPalette
	{
		Color Primary { get; }          // IndianRed
		Color Secondary { get; }        // Salmon
		Color Accent { get; }           // Gold
		Color Background { get; }       // BurlyWood
		Color Highlight { get; }        // Firebrick
	}
	public class SpiritedAwayBathhousePalette : IColorPalette
	{
		public Color Primary => Color.IndianRed;
		public Color Secondary => Color.Salmon;
		public Color Accent => Color.Gold;
		public Color Background => Color.BurlyWood;
		public Color Highlight => Color.Firebrick;
	}
}
