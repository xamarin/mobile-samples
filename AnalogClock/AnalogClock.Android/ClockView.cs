using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace AnalogClock.Android
{
	public class ClockView : View
	{
		Path tickMarks;
		Path hourHand;
		Path minuteHand;
		Path secondHand;

		Paint paint = new Paint();
		DashPathEffect minuteTickDashEffect = 
			new DashPathEffect (new float[] { 0.1f, 3 * (float)Math.PI - 0.1f}, 0);

		DashPathEffect hourTickDashEffect =
			new DashPathEffect (new float[] { 0.1f, 15 * (float)Math.PI - 0.1f}, 0);
			
		float hourAngle, minuteAngle, secondAngle;

		public ClockView (Context context) :
			base (context)
		{
			Initialize ();
		}

		public ClockView (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			Initialize ();
		}

		public ClockView (Context context, IAttributeSet attrs, int defStyle) :
			base (context, attrs, defStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{
			// Create Paint for all drawing
			paint = new Paint ();

			// All paths are based on 100-unit clock radius
			//		centered at (0, 0).

			// Define circle for tick marks.
			tickMarks = new Path ();
			tickMarks.AddCircle (0, 0, 90, Path.Direction.Cw);

			// Hour, minute, second hands defined to point straight up.

			// Define hour hand.
			hourHand = new Path ();
			hourHand.MoveTo (0, -60);
			hourHand.CubicTo (0, -30, 20, -30, 5, -20);
			hourHand.LineTo (5, 0);
			hourHand.CubicTo (5, 7.5f, -5, 7.5f, -5, 0);
			hourHand.LineTo (-5, -20);
			hourHand.CubicTo (-20, -30, 0, -30, 0, -60);
			hourHand.Close ();

			// Define minute hand.
			minuteHand = new Path ();
			minuteHand.MoveTo (0, -80);
			minuteHand.CubicTo (0, -75, 0, -70, 2.5f, -60);
			minuteHand.LineTo (2.5f, 0);
			minuteHand.CubicTo (2.5f, 5, -2.5f, 5, -2.5f, 0);
			minuteHand.LineTo (-2.5f, -60);
			minuteHand.CubicTo (0, -70, 0, -75, 0, -80);
			minuteHand.Close ();

			// Define second hand.
			secondHand = new Path ();
			secondHand.MoveTo (0, 10);
			secondHand.LineTo (0, -80);
		}

		// Called from MainActivity.
		public void SetClockHandAngles(float hourAngle, float minuteAngle, float secondAngle)
		{
			this.hourAngle = hourAngle;
			this.minuteAngle = minuteAngle;
			this.secondAngle = secondAngle;

			this.Invalidate ();
		}

		protected override void OnDraw (Canvas canvas)
		{
			base.OnDraw (canvas);

			// Clear screen to pink.
			paint.Color = new Color (255, 204, 204);
			canvas.DrawPaint (paint);

			// Overall transforms to shift (0, 0) to center and scale.
			canvas.Translate (this.Width / 2, this.Height / 2);
			float scale = Math.Min (this.Width, this.Height) / 2.0f / 100;
			canvas.Scale (scale, scale);

			// Attributes for tick marks.
			paint.Color = Color.Black;
			paint.StrokeCap = Paint.Cap.Round;
			paint.SetStyle (Paint.Style.Stroke);

			// Set line dash to draw tick marks for every minute.
			paint.StrokeWidth = 3;
			paint.SetPathEffect (minuteTickDashEffect);
			canvas.DrawPath (tickMarks, paint);

			// Set line dash to draw tick marks for every hour.
			paint.StrokeWidth = 6;
			paint.SetPathEffect (hourTickDashEffect);
			canvas.DrawPath (tickMarks, paint);

			// Set attributes common to all clock hands.
			Color strokeColor = Color.Black;
			Color fillColor = Color.Blue;
			paint.StrokeWidth = 2;
			paint.SetPathEffect (null);

			// Draw hour hand.
			canvas.Save ();
			canvas.Rotate (this.hourAngle);
			paint.Color = fillColor;
			paint.SetStyle (Paint.Style.Fill);
			canvas.DrawPath (hourHand, paint);
			paint.Color = strokeColor;
			paint.SetStyle (Paint.Style.Stroke);
			canvas.DrawPath (hourHand, paint);
			canvas.Restore ();

			// Draw minute hand.
			canvas.Save ();
			canvas.Rotate (this.minuteAngle);
			paint.Color = fillColor;
			paint.SetStyle (Paint.Style.Fill);
			canvas.DrawPath (minuteHand, paint);
			paint.Color = strokeColor;
			paint.SetStyle (Paint.Style.Stroke);
			canvas.DrawPath (minuteHand, paint);
			canvas.Restore ();

			// Draw second hand.
			canvas.Save ();
			canvas.Rotate (this.secondAngle);
			paint.Color = strokeColor;
			paint.SetStyle (Paint.Style.Stroke);
			canvas.DrawPath (secondHand, paint);
			canvas.Restore ();
		}
	}
}

