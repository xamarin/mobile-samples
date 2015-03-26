//
// ElementBadge.cs: defines the Badge Element.
//
// Author:
//   Miguel de Icaza (miguel@gnome.org)
//
// Copyright 2010, Novell, Inc.
//
// Code licensed under the MIT X11 license
//
using System;
using System.Collections;
using System.Collections.Generic;
using UIKit;
using CoreGraphics;
using System.Drawing;
using Foundation;

namespace MWC.iOS.UI.CustomElements {
	/// <summary>
	/// Lifted this code from MT.D source, so it could be customized
	/// </summary>
	public class CustomBadgeElement {
		public CustomBadgeElement ()
		{
		}
		public static UIImage MakeCalendarBadge (UIImage template, string smallText, string bigText)
		{
			using (var cs = CGColorSpace.CreateDeviceRGB ()){
				using (var context = new CGBitmapContext (IntPtr.Zero, 59, 58, 8, 59*4, cs, CGImageAlphaInfo.PremultipliedLast)){
					//context.ScaleCTM (0.5f, -1);
					context.TranslateCTM (0, 0);
					context.DrawImage (new CGRect (0, 0, 59, 58), template.CGImage);
					context.SetFillColor (0, 0, 0, 1);
					
					// The _small_ string
					context.SelectFont ("Helvetica-Bold", 14f, CGTextEncoding.MacRoman);
					
					// Pretty lame way of measuring strings, as documented:
					var start = context.TextPosition.X;					
					context.SetTextDrawingMode (CGTextDrawingMode.Invisible);
					context.ShowText (smallText);
					var width = context.TextPosition.X - start;
					
					context.SetTextDrawingMode (CGTextDrawingMode.Fill);
					context.ShowTextAtPoint ((59-width)/2, 10, smallText); // was 46
					
					// The BIG string
					context.SelectFont ("Helvetica-Bold", 32, CGTextEncoding.MacRoman);					
					start = context.TextPosition.X;
					context.SetTextDrawingMode (CGTextDrawingMode.Invisible);
					context.ShowText (bigText);
					width = context.TextPosition.X - start;
					
					context.SetFillColor (0, 0, 0, 1);
					context.SetTextDrawingMode (CGTextDrawingMode.Fill);
					context.ShowTextAtPoint ((59-width)/2, 25, bigText);	// was 9
					
					context.StrokePath ();
				
					return UIImage.FromImage (context.ToImage ());
				}
			}
		}
		public static UIImage MakeCalendarBadgeSmall (UIImage template, string smallText, string bigText)
		{
			int imageWidth=30, imageHeight=29;
			int smallTextY=5, bigTextY=12;
			float smallTextSize=7f, bigTextSize=16f;

			using (var cs = CGColorSpace.CreateDeviceRGB ()) {
				using (var context = new CGBitmapContext (IntPtr.Zero
								, imageWidth, imageHeight, 8, imageWidth*4, cs
								, CGImageAlphaInfo.PremultipliedLast)) {
					
					//context.ScaleCTM (0.5f, -1);
					context.TranslateCTM (0, 0);
					context.DrawImage (new CGRect (0, 0, imageWidth, imageHeight), template.CGImage);
					context.SetFillColor (0, 0, 0, 1);
					
					// The _small_ string
					context.SelectFont ("Helvetica-Bold", smallTextSize, CGTextEncoding.MacRoman);
					
					// Pretty lame way of measuring strings, as documented:
					var start = context.TextPosition.X;					
					context.SetTextDrawingMode (CGTextDrawingMode.Invisible);
					context.ShowText (smallText);
					var width = context.TextPosition.X - start;
					
					context.SetTextDrawingMode (CGTextDrawingMode.Fill);
					context.ShowTextAtPoint ((imageWidth-width)/2, smallTextY, smallText);
					
					// The BIG string
					context.SelectFont ("Helvetica-Bold", bigTextSize, CGTextEncoding.MacRoman);					
					start = context.TextPosition.X;
					context.SetTextDrawingMode (CGTextDrawingMode.Invisible);
					context.ShowText (bigText);
					width = context.TextPosition.X - start;
					
					context.SetFillColor (0, 0, 0, 1);
					context.SetTextDrawingMode (CGTextDrawingMode.Fill);
					context.ShowTextAtPoint ((imageWidth-width)/2, bigTextY, bigText);
					
					context.StrokePath ();
				
					return UIImage.FromImage (context.ToImage ());
				}
			}
		}
	}
}

