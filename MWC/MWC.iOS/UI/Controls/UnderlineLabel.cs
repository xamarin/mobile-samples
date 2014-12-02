using System;
using System.Drawing;
using UIKit;
using CoreGraphics;
using Foundation;

namespace MWC.iOS {
	/// <summary>
	/// Custom label that appears to be a 'url link' (blue, underlined)
	/// </summary>
	public class UnderlineLabel : UILabel {
		public UnderlineLabel ()
		{
		}
		public override void Draw (CGRect rect)
		{
			base.Draw (rect);
			var st = new NSString(Text);
			var sz = st.StringSize (Font);

			CGContext context = UIGraphics.GetCurrentContext();
			context.SetFillColor (AppDelegate.ColorTextLink.CGColor); 
			context.SetLineWidth(0.5f);
			context.MoveTo(0,sz.Height+2);
			context.AddLineToPoint(sz.Width,sz.Height+2);
			context.StrokePath();  
		}
	}
}

