using System;
using System.Collections.Generic;
using CocosSharp;

namespace BouncingGame
{
	public class GameLayer : CCLayer
	{
		int score;
		float ballXVelocity;
		float ballYVelocity;
		// How much to modify the ball's y velocity per second:
		const float gravity = 140;

		CCSprite paddleSprite;
		CCSprite ballSprite;
		CCLabel scoreLabel;

		public GameLayer ()
		{
			// "paddle" refers to the paddle.png image
			paddleSprite = new CCSprite ("paddle");
			paddleSprite.PositionX = 100;
			paddleSprite.PositionY = 100;
			AddChild (paddleSprite);

			ballSprite = new CCSprite ("ball");
			ballSprite.PositionX = 320;
			ballSprite.PositionY = 600;
			AddChild (ballSprite);

				scoreLabel = new CCLabel ("Score: 0", "arial", 22, CCLabelFormat.SpriteFont);
			scoreLabel.PositionX = 50;
			scoreLabel.PositionY = 1000;
			scoreLabel.AnchorPoint = CCPoint.AnchorUpperLeft;
			AddChild (scoreLabel);


			// New code:
			Schedule (RunGameLogic);
		}

		void RunGameLogic(float frameTimeInSeconds)
		{
			// This is a linear approximation, so not 100% accurate
			ballYVelocity += frameTimeInSeconds * -gravity;

			ballSprite.PositionX += ballXVelocity * frameTimeInSeconds;
			ballSprite.PositionY += ballYVelocity * frameTimeInSeconds;

			// New Code:

			// Check if the two CCSprites overlap...
			bool doesBallOverlapPaddle = ballSprite.BoundingBoxTransformedToParent.IntersectsRect(
				paddleSprite.BoundingBoxTransformedToParent);
			// ... and if the ball is moving downward.
			bool isMovingDownward = ballYVelocity < 0;
			if (doesBallOverlapPaddle && isMovingDownward)
			{
				// First let's invert the velocity:
				ballYVelocity *= -1;
				// Then let's assign a random to the ball's x velocity:
				const float minXVelocity = -300;
				const float maxXVelocity = 300;
				ballXVelocity = CCRandom.GetRandomFloat (minXVelocity, maxXVelocity);

				// New code:
				score++;
				scoreLabel.Text = "Score: " + score;
			}

			// First let’s get the ball position:   
			float ballRight = ballSprite.BoundingBoxTransformedToParent.MaxX;
			float ballLeft = ballSprite.BoundingBoxTransformedToParent.MinX;

			// Then let’s get the screen edges
			float screenRight = VisibleBoundsWorldspace.MaxX;
			float screenLeft = VisibleBoundsWorldspace.MinX;

			// Check if the ball is either too far to the right or left:    
			bool shouldReflectXVelocity = 
				(ballRight > screenRight && ballXVelocity > 0) ||
				(ballLeft < screenLeft && ballXVelocity < 0);

			if (shouldReflectXVelocity)
			{
				ballXVelocity *= -1;
			}
		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			// Use the bounds to layout the positioning of our drawable assets
			CCRect bounds = VisibleBoundsWorldspace;

			// Register for touch events
			var touchListener = new CCEventListenerTouchAllAtOnce ();
			touchListener.OnTouchesEnded = OnTouchesEnded;
			// new code:
			touchListener.OnTouchesMoved = HandleTouchesMoved;
			AddEventListener (touchListener, this);
		}
		void HandleTouchesMoved (System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
		{
			// we only care about the first touch:
			var locationOnScreen = touches [0].Location;
			paddleSprite.PositionX = locationOnScreen.X;

			// Added for the sake of debugging:
			scoreLabel.Text = locationOnScreen.X.ToString();

		}

		void OnTouchesEnded (List<CCTouch> touches, CCEvent touchEvent)
		{
			if (touches.Count > 0)
			{
				// Perform touch handling here
			}
		}
	}
}
