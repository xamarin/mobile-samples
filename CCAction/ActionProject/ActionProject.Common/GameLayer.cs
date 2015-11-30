using System;
using System.Collections.Generic;
using CocosSharp;
using ActionProject.Common;

namespace ActionProject
{
	public class GameLayer : CCLayer
	{

		List<string> VariableOptions = new List<string> {
			"Position", 
			"Scale",
			"Rotation",
			"LineWidth"
		};

		List<string> EasingOptions = new List<string> {
			"<None>",
			"CCEaseBack",
			"CCEaseBounce",
			"CCEaseElastic",
			"CCEaseExponential",
			"CCEaseSine"
		};

		List<string> OutInOptions = new List<string> {

			"Out",
			"In",
			"Both"
		};

		int currentVariableIndex = 0;
		int currentEasingIndex = 0;
		int currentInOutIndex = 0;

		CCNode drawNodeRoot;
		LineNode lineNode;

		CCLabel variableLabel;
		CCLabel easingLabel;
		CCLabel inOutLabel;

		const float DefaultCircleRadius = 40;

		public GameLayer ()
		{

			drawNodeRoot = new CCNode ();
			drawNodeRoot.PositionX = 500;
			drawNodeRoot.PositionY = 350;
			this.AddChild (drawNodeRoot);

			CCDrawNode circle;
			circle = new CCDrawNode ();
			circle.DrawSolidCircle (CCPoint.Zero, DefaultCircleRadius, CCColor4B.Red);
			drawNodeRoot.AddChild (circle);

			lineNode = new LineNode ();
			drawNodeRoot.AddChild (lineNode);

			variableLabel = new CCLabel ("Hello", "Arial", 46, CCLabelFormat.SystemFont);
			variableLabel.HorizontalAlignment = CCTextAlignment.Left;
			variableLabel.AnchorPoint = new CCPoint (0, 0);
			variableLabel.PositionX = 48;
			variableLabel.PositionY = 390;
			this.AddChild (variableLabel);


			easingLabel = new CCLabel ("Hello", "Arial", 46, CCLabelFormat.SystemFont);
			easingLabel.HorizontalAlignment = CCTextAlignment.Left;
			easingLabel.AnchorPoint = new CCPoint (0, 0);
			easingLabel.PositionX = 48;
			easingLabel.PositionY = 330;
			this.AddChild (easingLabel);


			inOutLabel = new CCLabel ("Hello", "Arial", 46, CCLabelFormat.SystemFont);
			inOutLabel.HorizontalAlignment = CCTextAlignment.Left;
			inOutLabel.AnchorPoint = new CCPoint (0, 0);
			inOutLabel.PositionX = 48;
			inOutLabel.PositionY = 270;
			this.AddChild (inOutLabel);



			UpdateLabels ();
		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			var touchListener = new CCEventListenerTouchAllAtOnce ();
			touchListener.OnTouchesEnded = OnTouchesEnded;
			AddEventListener (touchListener, this);


		}

		void OnTouchesEnded (List<CCTouch> touches, CCEvent touchEvent)
		{
			if (touches.Count > 0)
			{
				var touch = touches [0];

				if (easingLabel.BoundingBoxTransformedToWorld.ContainsPoint (touch.Location))
				{
					HandleEasingLabelTouched ();
				}
				else if (inOutLabel.BoundingBoxTransformedToWorld.ContainsPoint (touch.Location))
				{
					HandleInOutLabelTouched ();
				}
				else if (variableLabel.BoundingBoxTransformedToWorld.ContainsPoint (touch.Location))
				{
					HandleVariableLabelTouched ();
				}
				else
				{
					HandleMoveCircle (touch);
				}
			}
		}

		private void HandleVariableLabelTouched ()
		{
			currentVariableIndex++;
			currentVariableIndex = currentVariableIndex % VariableOptions.Count;

			UpdateLabels ();
		}

		private void HandleEasingLabelTouched()
		{
			currentEasingIndex++;
			currentEasingIndex = currentEasingIndex % EasingOptions.Count;

			UpdateLabels ();
		}

		private void HandleInOutLabelTouched()
		{
			currentInOutIndex++;
			currentInOutIndex = currentInOutIndex % OutInOptions.Count;

			UpdateLabels ();

		}

		private void UpdateLabels()
		{
			variableLabel.Text = VariableOptions [currentVariableIndex];
			easingLabel.Text = EasingOptions [currentEasingIndex];
			inOutLabel.Text = OutInOptions [currentInOutIndex];
		}

		private void HandleMoveCircle(CCTouch touch)
		{
			const float timeToTake = 1.5f; // in seconds
			CCFiniteTimeAction coreAction = null;

			CCNode nodeToAddTo = drawNodeRoot;

			switch (VariableOptions [currentVariableIndex])
			{
			case "Position":
				coreAction = new CCMoveTo(timeToTake, touch.Location);

					break;
			case "Scale":
					var distance = CCPoint.Distance (touch.Location, drawNodeRoot.Position);
					var desiredScale = distance / DefaultCircleRadius;
					coreAction = new CCScaleTo(timeToTake, desiredScale);

					break;
			case "Rotation":
					float differenceY = touch.Location.Y - drawNodeRoot.PositionY;
					float differenceX = touch.Location.X - drawNodeRoot.PositionX;

					float angleInDegrees = -1 * CCMathHelper.ToDegrees(
						(float)System.Math.Atan2(differenceY, differenceX));

					coreAction = new CCRotateTo (timeToTake, angleInDegrees);

					break;
				case "LineWidth":
					coreAction = new LineWidthAction (timeToTake, touch.Location.X / 40.0f);
					nodeToAddTo = lineNode;
					break;
			}

			CCAction easing = null;
			switch (EasingOptions [currentEasingIndex])
			{
				case "<None>":
					// no easing, do nothing, it will be handled below
					break;
				case "CCEaseBack":
					if (currentInOutIndex == 0)
						easing = new CCEaseBackOut (coreAction);
					else if (currentInOutIndex == 1)
						easing = new CCEaseBackIn (coreAction);
					else
						easing = new CCEaseBackInOut (coreAction);

					break;
				case "CCEaseBounce":
					if (currentInOutIndex == 0)
						easing = new CCEaseBounceOut (coreAction);
					else if (currentInOutIndex == 1)
						easing = new CCEaseBounceIn (coreAction);
					else
						easing = new CCEaseBounceInOut (coreAction);

					break;
				case "CCEaseElastic":
					if (currentInOutIndex == 0)
						easing = new CCEaseElasticOut (coreAction);
					else if (currentInOutIndex == 1)
						easing = new CCEaseElasticIn (coreAction);
					else
						easing = new CCEaseElasticInOut (coreAction);

					break;
				case "CCEaseExponential":
					if (currentInOutIndex == 0)
						easing = new CCEaseExponentialOut (coreAction);
					else if (currentInOutIndex == 1)
						easing = new CCEaseExponentialIn (coreAction);
					else
						easing = new CCEaseExponentialInOut (coreAction);

					break;
				case "CCEaseSine":

					if (currentInOutIndex == 0)
						easing = new CCEaseSineOut (coreAction);
					else if (currentInOutIndex == 1)
						easing = new CCEaseSineIn (coreAction);
					else
						easing = new CCEaseSineInOut (coreAction);

					break;
			}

			if (easing != null)
			{
				nodeToAddTo.AddAction (easing);
			}
			else
			{
				nodeToAddTo.AddAction (coreAction);
			}

		}
	}
}
