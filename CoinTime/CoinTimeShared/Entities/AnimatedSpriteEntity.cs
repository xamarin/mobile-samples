using System;
using System.Linq;
using CocosSharp;
using System.Collections.Generic;
using CoinTimeGame.ContentRuntime.Animations;
using CoinTimeGame.ContentLoading.Animations;
using CoinTimeGame.ContentLoading;

namespace CoinTimeGame.Entities
{
	public class AnimatedSpriteEntity : CCNode
	{
		TimeSpan timeIntoAnimation;

		CCSprite sprite;

		static Dictionary<string, List<Animation>> animationCache = 
			new Dictionary<string, List<Animation>>();

		protected List<Animation> animations;

		Animation currentAnimation;

		public bool IsLoopingAnimation
		{
			get;
			set;
		}

		public CCRect BoundingBoxWorld
		{
			get
			{
				return this.sprite.BoundingBoxTransformedToWorld;
			}
		}

		protected Animation CurrentAnimation
		{
			get
			{
				return currentAnimation;
			}
			set
			{ 
				if (currentAnimation != value)
				{
					currentAnimation = value;
					// restart the animation:
					timeIntoAnimation = TimeSpan.Zero;
					// Update the sprite immediately:
					PerformSpriteAnimation (0);
				}
			}
		}

		public AnimatedSpriteEntity ()
		{
			CreateSprite ();

			Schedule (PerformSpriteAnimation);
		}

		void CreateSprite()
		{
			IsLoopingAnimation = true;

			// The entire game will use mastersheet.png:
			sprite = new CCSprite ("mastersheet.png");

			sprite.IsAntialiased = false;

			this.AddChild (sprite);
			sprite.TextureRectInPixels = new CCRect (1024, 0, 100, 100);
			sprite.ContentSize = new CCSize (100, 100);

		}

		protected void LoadAnimations(string fileName)
		{
			if (animationCache.ContainsKey (fileName))
			{
				animations = animationCache [fileName];
			}
			else
			{
				animations = new List<Animation> ();
				AnimationChainListSave acls = XmlDeserializer.Self.XmlDeserialize<AnimationChainListSave> (fileName);

				foreach (var animationSave in acls.AnimationChains)
				{
					animations.Add (Animation.FromAnimationSave (animationSave));
				}


				animationCache.Add (fileName, animations);
			}

			// This prevents the sprite from temporarily showing
			// the entire PNG for a split second.
			if (animations != null && animations.Count > 0)
			{
				CurrentAnimation = animations [0];
			}

		}

		void PerformSpriteAnimation(float time)
		{
			if (CurrentAnimation != null)
			{
				double secondsIntoAnimation = 
					timeIntoAnimation.TotalSeconds + time;
				double remainder = secondsIntoAnimation % CurrentAnimation.Duration.TotalSeconds;
				bool passedEnd = remainder < secondsIntoAnimation;

				if (passedEnd && !IsLoopingAnimation)
				{
					// we're not looping so set the time to the duration minus half of the last frame
					// to minimize rounding errors.
					// This is somewhat inefficient, but we're probably dealing with a small number of entities
					// so we'll just swallow the inefficiency to keep the code simpler. The alternative is to store
					// off that this entity is no longer animating until either the IsLoopingAnimation is set to true
					// or the CurrentAnimation is changed.
					remainder = CurrentAnimation.Duration.TotalSeconds - CurrentAnimation.Frames.Last().Duration.TotalSeconds/2.0;
				}

				timeIntoAnimation = TimeSpan.FromSeconds (remainder);

				var rectangle = CurrentAnimation.GetRectangleAtTime (timeIntoAnimation);
				bool flipHorizontally = CurrentAnimation.GetIfFlippedHorizontallyAtTime (timeIntoAnimation);

				sprite.FlipX = flipHorizontally;

				sprite.TextureRectInPixels = rectangle;
				sprite.ContentSize = rectangle.Size;
			}
		}

		public bool Intersects(AnimatedSpriteEntity other)
		{
			return this.sprite.BoundingBoxTransformedToWorld.IntersectsRect (other.sprite.BoundingBoxTransformedToWorld);
		}
	}
}

