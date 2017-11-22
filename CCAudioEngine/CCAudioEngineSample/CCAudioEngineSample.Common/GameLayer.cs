using System;
using System.Collections.Generic;
using CocosSharp;

namespace CCAudioEngineSample
{
	public class GameLayer : CCLayerColor
	{
		CCLabel musicLabel;
		CCDrawNode musicPlayButton;

		CCLabel sfxLabel;
		CCDrawNode sfxPlayButton;

		CCLabel sfxVolumeLabel;
		CCLabel musicVolumeLabel;

		CCDrawNode sfxVolumeUp;
		CCDrawNode sfxVolumeDown;

		CCDrawNode musicVolumeUp;
		CCDrawNode musicVolumeDown;

		public GameLayer () : base (new CCColor4B(20, 20, 20))
		{
			// Load and instantate your assets here

			// Make any renderable node objects (e.g. sprites) children of this layer
		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			// Use the bounds to layout the positioning of our drawable assets
			CCRect bounds = VisibleBoundsWorldspace;

			// Register for touch events
			var touchListener = new CCEventListenerTouchAllAtOnce ();
			touchListener.OnTouchesEnded = OnTouchesEnded;
			AddEventListener (touchListener, this);


			CreateMusicLabel();
			CreateMusicPlayButton();

			CreateSfxLabel();
			CreateSfxPlayButton();

			CreateMusicVolumeControls();
			CreateSfxVolumeControls();

		}
		private void CreateMusicLabel()
		{
			musicLabel = new CCLabel("Music", "Arial", 24, CCLabelFormat.SystemFont);
			musicLabel.PositionX = 80;
			musicLabel.PositionY = this.ContentSize.Height - 40;

			this.AddChild(musicLabel);
		}

		private void CreateMusicPlayButton()
		{
			musicPlayButton = new CCDrawNode();

			MakePlayGraphic(musicPlayButton);

			musicPlayButton.PositionX = musicLabel.PositionX + musicLabel.BoundingBox.Size.Width;
			musicPlayButton.PositionY = musicLabel.PositionY - 18;

			this.AddChild(musicPlayButton);
		}

		private void CreateSfxLabel()
		{
			sfxLabel = new CCLabel("Sound", "Arial", 24, CCLabelFormat.SystemFont);
			sfxLabel.PositionX = 500;
			sfxLabel.PositionY = this.ContentSize.Height - 40;

			this.AddChild(sfxLabel);
		}


		private void CreateSfxPlayButton()
		{
			sfxPlayButton = new CCDrawNode();

			MakePlayGraphic(sfxPlayButton);

			sfxPlayButton.PositionX = sfxLabel.PositionX + sfxLabel.BoundingBox.Size.Width;
			sfxPlayButton.PositionY = sfxLabel.PositionY - 18;

			this.AddChild(sfxPlayButton);
		}


		private void CreateMusicVolumeControls()
		{
			const float x = 100;

			float y = this.ContentSize.Height - 120;

			musicVolumeUp = new CCDrawNode();
			MakeUpArrowGraphic(musicVolumeUp);
			musicVolumeUp.PositionX = x;
			musicVolumeUp.PositionY = y;
			this.AddChild(musicVolumeUp);

			y -= 30;

			musicVolumeLabel = new CCLabel("", "Arial", 24, CCLabelFormat.SystemFont);
			musicVolumeLabel.HorizontalAlignment = CCTextAlignment.Center;
			musicVolumeLabel.PositionX = x;
			musicVolumeLabel.PositionY = y;
			UpdateMusicVolumeDisplay();
			this.AddChild(musicVolumeLabel);

			y -= 60;

			musicVolumeDown = new CCDrawNode();
			MakeDownArrowGraphic(musicVolumeDown);
			musicVolumeDown.PositionX = x;
			musicVolumeDown.PositionY = y;
			this.AddChild(musicVolumeDown);

		}

		private void CreateSfxVolumeControls()
		{
			const float x = 500;

			float y = this.ContentSize.Height - 120;

			sfxVolumeUp = new CCDrawNode();
			MakeUpArrowGraphic(sfxVolumeUp);
			sfxVolumeUp.PositionX = x;
			sfxVolumeUp.PositionY = y;
			this.AddChild(sfxVolumeUp);

			y -= 30;

			sfxVolumeLabel = new CCLabel("", "Arial", 24, CCLabelFormat.SystemFont);
			sfxVolumeLabel.HorizontalAlignment = CCTextAlignment.Center;
			sfxVolumeLabel.PositionX = x;
			sfxVolumeLabel.PositionY = y;
			UpdateSfxVolumeDisplay();
			this.AddChild(sfxVolumeLabel);

			y -= 60;

			sfxVolumeDown = new CCDrawNode();
			MakeDownArrowGraphic(sfxVolumeDown);
			sfxVolumeDown.PositionX = x;
			sfxVolumeDown.PositionY = y;
			this.AddChild(sfxVolumeDown);
		}

		private void UpdateSfxVolumeDisplay()
		{
			this.sfxVolumeLabel.Text = $"Volume: {CCAudioEngine.SharedEngine.EffectsVolume}";
		}

		void OnTouchesEnded (List<CCTouch> touches, CCEvent touchEvent)
		{
			if (touches.Count > 0) 
			{
				var firstTouch = touches[0];

				if(musicPlayButton.BoundingBoxTransformedToWorld.ContainsPoint(firstTouch.Location))
				{
					string fileName = "FruityFalls";

					CCAudioEngine.SharedEngine.PlayBackgroundMusic (
						fileName, loop:false);

				}
				else if (sfxPlayButton.BoundingBoxTransformedToWorld.ContainsPoint(firstTouch.Location))
				{
					string fileName = "Electricity";

					CCAudioEngine.SharedEngine.PlayEffect (fileName);
				}
				else if(musicVolumeUp.BoundingBoxTransformedToWorld.ContainsPoint(firstTouch.Location))
				{
					CCAudioEngine.SharedEngine.BackgroundMusicVolume += .25f;
					UpdateMusicVolumeDisplay();
				}
				else if (musicVolumeDown.BoundingBoxTransformedToWorld.ContainsPoint(firstTouch.Location))
				{
					CCAudioEngine.SharedEngine.BackgroundMusicVolume -= .25f;
					UpdateMusicVolumeDisplay();
				}
				else if (sfxVolumeUp.BoundingBoxTransformedToWorld.ContainsPoint(firstTouch.Location))
				{
					CCAudioEngine.SharedEngine.EffectsVolume += .25f;
					UpdateSfxVolumeDisplay();
				}
				else if (sfxVolumeDown.BoundingBoxTransformedToWorld.ContainsPoint(firstTouch.Location))
				{
					CCAudioEngine.SharedEngine.EffectsVolume -= .25f;
					UpdateSfxVolumeDisplay();
				}
			}
		}

		private void UpdateMusicVolumeDisplay()
		{
			this.musicVolumeLabel.Text = $"Volume: {CCAudioEngine.SharedEngine.BackgroundMusicVolume}";
		}

		public void MakePlayGraphic(CCDrawNode drawNode)
		{
			drawNode.Clear();

			const float playButtonHeight = 40;
			var verts = new CCPoint[]
			{
				new CCPoint(0,0),
				new CCPoint(0, playButtonHeight),
				new CCPoint(22, playButtonHeight/2.0f)
			};

			drawNode.DrawPolygon(verts, 3, CCColor4B.Green, 1, CCColor4B.White);
		}



		void MakeUpArrowGraphic(CCDrawNode drawNode)
		{
			var verts = new CCPoint[]
			{
				new CCPoint(0,0),
				new CCPoint(20, 22),
				new CCPoint(40, 0)
			};

			drawNode.DrawPolygon(verts, 3, CCColor4B.Gray, 1, CCColor4B.White);

		}

		void MakeDownArrowGraphic(CCDrawNode drawNode)
		{
			var verts = new CCPoint[]
			{
				new CCPoint(0,22),
				new CCPoint(20, 0),
				new CCPoint(40, 22)
			};

			drawNode.DrawPolygon(verts, 3, CCColor4B.Gray, 1, CCColor4B.White);
		}
	}
}
