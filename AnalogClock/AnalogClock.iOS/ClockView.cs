using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace AnalogClock.iOS
{
	public class ClockView : UIView
	{
		CGPath tickMarks;
		CGPath hourHand;
		CGPath minuteHand;
		CGPath secondHand;

		float hourAngle, minuteAngle, secondAngle;

		public ClockView ()
		{
			// Set background to pink.
			this.BackgroundColor = UIColor.FromRGB (1.0f, 0.8f, 0.8f);

			// All paths are based on 100-unit clock radius
			//		centered at (0, 0)

			// Define circle for tick marks.
			tickMarks = new CGPath ();
			tickMarks.AddEllipseInRect(new CGRect(-90, -90, 180, 180));

			// Hour, minute, second hands defined to point straight up.

			// Define hour hand.
			hourHand = new CGPath ();
			hourHand.MoveToPoint (0, -60);
			hourHand.AddCurveToPoint (0, -30, 20, -30, 5, - 20);
			hourHand.AddLineToPoint (5, 0);
			hourHand.AddCurveToPoint (5, 7.5f, -5, 7.5f, -5, 0);
			hourHand.AddLineToPoint (-5, -20);
			hourHand.AddCurveToPoint (-20, -30, 0, -30, 0, -60);
			hourHand.CloseSubpath ();

			// Define minute hand.
			minuteHand = new CGPath ();
			minuteHand.MoveToPoint (0, -80);
			minuteHand.AddCurveToPoint (0, -75, 0, -70, 2.5f, -60);
			minuteHand.AddLineToPoint (2.5f, 0);
			minuteHand.AddCurveToPoint (2.5f, 5, -2.5f, 5, -2.5f, 0);
			minuteHand.AddLineToPoint (-2.5f, -60);
			minuteHand.AddCurveToPoint (0, -70, 0, -75, 0, -80);
			minuteHand.CloseSubpath ();

			// Define second hand.
			secondHand = new CGPath ();
			secondHand.MoveToPoint (0, 10);
			secondHand.AddLineToPoint(0, -80);
		}

		// Called from ClockViewController.
		public void SetClockHandAngles(float hourAngle, float minuteAngle, float secondAngle)
		{
			this.hourAngle = hourAngle;
			this.minuteAngle = minuteAngle;
			this.secondAngle = secondAngle;

			this.SetNeedsDisplay();
		}

		public override void Draw (CGRect rect)
		{
			base.Draw (rect);

			// Get current graphics context.
			using (CGContext g = UIGraphics.GetCurrentContext ()) {

				// Overall transforms to shift (0, 0) to center and scale.
				g.TranslateCTM (rect.GetMidX (), rect.GetMidY ());
				nfloat scale = (nfloat)Math.Min(rect.Width, rect.Height) / 2 / 100;
				g.ScaleCTM (scale, scale);

				// Attributes for tick marks
				g.SetStrokeColor (new CGColor (0, 0, 0));
				g.SetLineCap (CGLineCap.Round);

				// Set line dash to draw tick marks for every minute.
				g.SetLineWidth (3);
				g.SetLineDash (0, new nfloat[] { 0, 3 * (nfloat)Math.PI });
				g.AddPath (tickMarks);
				g.DrawPath (CGPathDrawingMode.Stroke);

				// Set line dash to draw tick marks for every hour.
				g.SetLineWidth (6);
				g.SetLineDash(0, new nfloat[] { 0, 15 * (nfloat)Math.PI });
				g.AddPath (tickMarks);
				g.DrawPath (CGPathDrawingMode.Stroke);

				// Set common attributes for clock hands.
				g.SetStrokeColor (new CGColor (0, 0, 0));
				g.SetFillColor(new CGColor(0, 0, 1));
				g.SetLineWidth (2);
				g.SetLineDash (0, null);
				g.SetLineJoin (CGLineJoin.Round);

				// Draw hour hand.
				g.SaveState ();
				g.RotateCTM (hourAngle); // 2 * (float)Math.PI * (dt.Hour + dt.Minute / 60.0f) / 12);
				g.AddPath (hourHand);
				g.DrawPath (CGPathDrawingMode.FillStroke);
				g.RestoreState ();

				// Draw minute hand.
				g.SaveState ();
				g.RotateCTM (minuteAngle); // 2 * (float)Math.PI * (dt.Minute + dt.Second / 60.0f) / 60);
				g.AddPath (minuteHand);
				g.DrawPath (CGPathDrawingMode.FillStroke);
				g.RestoreState ();

				// Draw second hand.
				g.SaveState ();
				g.RotateCTM (secondAngle); // 2 * (float)Math.PI * dt.Second / 60);
				g.AddPath (secondHand);
				g.DrawPath (CGPathDrawingMode.Stroke);
				g.RestoreState ();
			}
		}
	}
}

