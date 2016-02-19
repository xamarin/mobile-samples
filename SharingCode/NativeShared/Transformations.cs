using System;
using System.Drawing;

#if __UNIFIED__
using UIKit;

// Mappings Unified CoreGraphic classes to MonoTouch classes
using RectangleF = global::CoreGraphics.CGRect;
using SizeF = global::CoreGraphics.CGSize;
using PointF = global::CoreGraphics.CGPoint;
#else
// Mappings Unified types to MonoTouch types
using nfloat = global::System.Single;
using nint = global::System.Int32;
using nuint = global::System.UInt32;
#endif

namespace NativeShared
{

	public class Transformations
	{
		#region Constructors
		public Transformations ()
		{
		}
		#endregion

		#region Public Methods
		public static nfloat CalculateArea(RectangleF rect) {

			// Calculate area...
			return (rect.Width * rect.Height);

		}
		#endregion
	}
}

