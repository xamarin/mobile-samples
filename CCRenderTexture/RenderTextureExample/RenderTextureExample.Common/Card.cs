using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderTextureExample
{
    class Card : CCNode
    {
        bool usesRenderTexture;

        List<CCNode> visualComponents = new List<CCNode>();

        CCSprite background;
        CCSprite colorIcon;
        CCSprite monsterSprite;
        CCLabel monsterNameDisplay;
        CCLabel hpDisplay;
        CCLabel descriptionDisplay;

        CCRenderTexture renderTexture;

        public string MonsterName
        {
            get
            {
                return monsterNameDisplay.Text;
            }
            set
            {
                monsterNameDisplay.Text = value;
            }
        }

        public string HpText
        {
            get
            {
                return hpDisplay.Text;
            }
            set
            {
                hpDisplay.Text = value;
            }
        }

        public string DescriptionText
        {
            get
            {
                return descriptionDisplay.Text;
            }
            set
            {
                descriptionDisplay.Text = value;
            }
        }

        public CCTexture2D ColorIconTexture
        {
            get
            {
                return colorIcon.Texture;
            }
            set
            {
                colorIcon.Texture = value;
                colorIcon.IsAntialiased = false;

            }
        }

        public CCTexture2D MonsterTexture
        {
            get
            {
                return monsterSprite.Texture;
            }
            set
            {
                monsterSprite.Texture = value;
                ReactToMonsterSpriteTextureSet();
            }
        }

        public bool UsesRenderTexture
        {
            get
            {
                return usesRenderTexture;
            }
            set
            {
                usesRenderTexture = value;
                if (usesRenderTexture)
                {
                    SwitchToRenderTexture();
                }
                else
                {
                    SwitchToRenderingComponents();
                }
            }
        }

        public override byte Opacity
        {
            get
            {
                return base.Opacity;
            }

            set
            {
                base.Opacity = value;

                if (usesRenderTexture)
                {
                    this.renderTexture.Sprite.Opacity = value;
                }
                else
                {
                    foreach (var component in visualComponents)
                    {
                        component.Opacity = value;
                    }
                }
            }
        }

        public Card()
        {
            CreateBackground();

            CreateColorIcon();

            CreateMonsterSprite();

            CreateMonsterNameDisplay();

            CreateHpDisplay();

            CreateDescriptionDisplay();

            AddComponentsToList();
        }

        private void CreateBackground()
        {
            background = new CCSprite("CardBackground.png");
            background.IsAntialiased = false;
            // The background serves as the largest sprite so it essentially defines the
            // card size and anchor point. Which is bottom left.
            background.AnchorPoint = CCPoint.Zero;
        }

        private void CreateColorIcon()
        {
            colorIcon = new CCSprite("GreenIcon.png");

            colorIcon.AnchorPoint = CCPoint.Zero;
            colorIcon.PositionX = 97;
            colorIcon.PositionY = 72;
            colorIcon.IsAntialiased = false;
        }

        private void CreateMonsterSprite()
        {
            monsterSprite = new CCSprite("GreenGuy.png");
            monsterSprite.PositionX = background.ContentSize.Center.X;
            monsterSprite.PositionY = 135;
            ReactToMonsterSpriteTextureSet();
        }

        private void ReactToMonsterSpriteTextureSet()
        {
            monsterSprite.IsAntialiased = false;
            // The TextureRectInPixels gets automatically set when the CCSprite is first
            // created, but the value does not adjust automatically when the texture is changed:
            monsterSprite.TextureRectInPixels = new CCRect(0, 0,
                monsterSprite.Texture.PixelsWide, monsterSprite.Texture.PixelsHigh);
            monsterSprite.Scale = 5;
        }

        private void CreateMonsterNameDisplay()
        {
            monsterNameDisplay = CreateLabel("Monster Name");
            monsterNameDisplay.HorizontalAlignment = CCTextAlignment.Center;
            monsterNameDisplay.PositionX = background.ContentSize.Center.X;
            monsterNameDisplay.PositionY = background.ContentSize.Height - 10;
        }

        private void CreateHpDisplay()
        {
            hpDisplay = CreateLabel("HP: 10/10");
            hpDisplay.AnchorPoint = new CCPoint(0, .5f);
            hpDisplay.PositionY = 80;
            hpDisplay.PositionX = 10;
        }

        private void CreateDescriptionDisplay()
        {
            descriptionDisplay = CreateLabel("Monster Description that wraps to multiple lines", 22);
            descriptionDisplay.Color = CCColor3B.Black;
            descriptionDisplay.PositionX = 15;
            descriptionDisplay.PositionY = 70;
            descriptionDisplay.LineHeight = 75;
            descriptionDisplay.Dimensions = new CCSize(180, 400);
            descriptionDisplay.LineBreak = CCLabelLineBreak.Word;
            descriptionDisplay.AnchorPoint = new CCPoint(0, 1);
        }

        private void AddComponentsToList()
        {
            visualComponents.Add(background);
            visualComponents.Add(colorIcon);
            visualComponents.Add(monsterSprite);
            visualComponents.Add(monsterNameDisplay);
            visualComponents.Add(hpDisplay);
            visualComponents.Add(descriptionDisplay);
        }

        private void SwitchToRenderingComponents()
        {
            if (renderTexture != null && this.Children.Contains(renderTexture.Sprite))
            {
                this.RemoveChild(renderTexture.Sprite);
            }

            bool areVisualComponentsAlreadyAdded = this.Children != null && this.Children.Contains(visualComponents[0]);

            if (!areVisualComponentsAlreadyAdded)
            {
                foreach (var component in visualComponents)
                {
                    this.AddChild(component);
                }
            }

        }

        private void SwitchToRenderTexture()
        {
            // The card needs to be moved to the origin (0,0) so it's rendered on the render target. 
            // After it's rendered to the CCRenderTexture, it will be moved back to its old position
            var oldPosition = this.Position;

            // Make sure visuals are part of the card so they get rendered
            bool areVisualComponentsAlreadyAdded = this.Children != null && this.Children.Contains(visualComponents[0]);
            if (!areVisualComponentsAlreadyAdded)
            {
                // Temporarily add them so we can render the object:
                foreach (var component in visualComponents)
                {
                    this.AddChild(component);
                }
            }

            // Create the render texture if it hasn't yet been made:
            if (renderTexture == null)
            {
                // Even though the game is zoomed in to create a pixellated look, we are using
                // high-resolution textures. Therefore, we want to have our canvas be 2x as big as 
                // the background so fonts don't appear pixellated
                var unitResolution = background.ContentSize;
                var pixelResolution = background.ContentSize * 2;
                renderTexture = new CCRenderTexture(unitResolution, pixelResolution);
            }

            // We don't want the render target to be a child of the card 
            // when we call Visit
            if (this.Children != null && this.Children.Contains(renderTexture.Sprite))
            {
                this.RemoveChild(renderTexture.Sprite);
            }

            // Move this instance back to the origin so it is rendered inside the render target:
            this.Position = CCPoint.Zero;

            // Clears the CCRenderTexture
            renderTexture.BeginWithClear(CCColor4B.Transparent);

            // Visit renders this object and all of its children
            this.Visit();

            // Ends the rendering, which means the CCRenderTexture's Sprite can be used
            renderTexture.End();

            // We no longer want the individual components to be drawn, so remove them:
            foreach (var component in visualComponents)
            {
                this.RemoveChild(component);
            }

            // add the render target sprite to this:
            this.AddChild(renderTexture.Sprite);

            renderTexture.Sprite.AnchorPoint = CCPoint.Zero;

            // Move this back to its original position:
            this.Position = oldPosition;

        }

        private CCLabel CreateLabel(string text, int fontSize = 28)
        {
            var toReturn = new CCLabel(text, "Arial", fontSize, CCLabelFormat.SystemFont);
            toReturn.Scale = .5f;

            return toReturn;
        }
    }
}
