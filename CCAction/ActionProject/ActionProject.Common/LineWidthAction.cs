using System;
using CocosSharp;

namespace ActionProject.Common
{
	public class LineWidthAction : CCFiniteTimeAction
	{
		readonly float endWidth;

		public LineWidthAction (float duration, float width) : base (duration)
		{
			endWidth = width;
		}

		public override CCFiniteTimeAction Reverse ()
		{
			throw new NotImplementedException ();
		}

		protected override CCActionState StartAction (CCNode target)
		{
			return new LineWidthState (this, target, endWidth);
		}
	}

	public class LineWidthState : CCFiniteTimeActionState
	{
		readonly float deltaWidth;
		readonly float startWidth;
		readonly LineNode castedTarget;

		public LineWidthState (LineWidthAction action, CCNode target, float endWidth) : base (action, target)
		{
			castedTarget = target as LineNode;

			if (castedTarget == null) {
				throw new InvalidOperationException ("The argument target must be a LineNode");
			}

			startWidth = castedTarget.Width;
			deltaWidth = endWidth - startWidth;
		}

		public override void Update (float time)
		{
			castedTarget.Width = startWidth + deltaWidth * time;
		}
	}
}

