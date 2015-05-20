using System;
using CocosSharp;

namespace CoinTimeGame.ContentRuntime.Animations
{
	public class AnimationFrame
	{
		public CCRect SourceRectangle { get; set; }
		public TimeSpan Duration { get; set; }
		public bool FlipHorizontal { get; set;}
	}
}

