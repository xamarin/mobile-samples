using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruityFalls.Scenes
{
    public class TitleScene : CCScene
    {
        CCLayer layer;

        public TitleScene(CCGameView gameView) : base(gameView)
        {
            layer = new CCLayer();
            this.AddChild(layer);

            CreateText();

            CreateTouchListener();

        }

        private void CreateText()
        {
            var label = new CCLabel("Tap to begin", "Arial", 30, CCLabelFormat.SystemFont);
            label.PositionX = layer.ContentSize.Width / 2.0f;
            label.PositionY = layer.ContentSize.Height / 2.0f;

            layer.AddChild(label);
        }

        private void CreateTouchListener()
        {
            var touchListener = new CCEventListenerTouchAllAtOnce();
			touchListener.OnTouchesBegan = HandleTouchesBegan;
            layer.AddEventListener(touchListener);
        }

        private void HandleTouchesBegan(List<CCTouch> arg1, CCEvent arg2)
        {
            var newScene = new GameScene(GameController.GameView);
            GameController.GoToScene(newScene);
        }
    }
}
