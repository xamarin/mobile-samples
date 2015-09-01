using System;
using CocosSharp;

namespace ActionProject.Common
{
	public class LineWidthAction : CCFiniteTimeAction
	{
		public float EndWidth { get; private set; }

		public LineWidthAction (float duration, float width) : base(duration)
		{
			EndWidth = width;
		}

		public override CCFiniteTimeAction Reverse ()
		{
			throw new NotImplementedException ();
		}

		protected override CCActionState StartAction (CCNode target)
		{
			return new LineWidthState (this, target);
		}
	}

	public class LineWidthState : CCFiniteTimeActionState
	{
		float deltaWidth;
		float startWidth;
		float endWidth;



		public LineWidthState(LineWidthAction action, CCNode target) : base(action, target)
		{
			var asDrawNode = target as LineNode;

			if (asDrawNode == null)
			{
				throw new InvalidOperationException ("The argument target must be a LineNode");
			}

			startWidth = asDrawNode.Width;
			endWidth = action.EndWidth;
			deltaWidth = endWidth - startWidth;
		}

		public override void Update (float time)
		{
			var asLine = Target as LineNode;
			if (asLine != null)
			{
				asLine.Width = startWidth + deltaWidth * time;
			}
		}
	}
}

