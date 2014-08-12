using System;
using System.Collections.Generic;
using CocosDenshion;
using CocosSharp;
using System.Linq;

using Box2D.Common;
using Box2D.Dynamics;
using Box2D.Collision.Shapes;

namespace GoneBananas
{
    public class GameLayer : CCLayerColor
    {
        const float MONKEY_SPEED = 350.0f;
        const float GAME_DURATION = 60.0f; // game ends after 60 seconds or when the monkey hits a ball, whichever comes first

        // point to meter ratio for physics
        const int PTM_RATIO = 32;

        float elapsedTime;
        CCSprite monkey;
        List<CCSprite> visibleBananas;
        List<CCSprite> hitBananas;

        // monkey walking animation
        CCAnimation walkAnim;
        CCRepeatForever walkRepeat;
        CCCallFuncN walkAnimStop = new CCCallFuncN (node => node.StopAllActions ());

        // background sprite
        CCSprite grass;

        // particles
        CCParticleSun sun;

        // circle layered behind sun
        CCDrawNode circleNode;

        // parallax node for clouds
        CCParallaxNode parallaxClouds;
            
        // define our banana rotation action
        CCRotateBy rotateBanana = new CCRotateBy (0.8f, 360);

        // define our completion action to remove the banana once it hits the bottom of the screen
        CCCallFuncN moveBananaComplete = new CCCallFuncN (node => node.RemoveFromParent ());

        // physics world
        b2World world;
        
        // balls sprite batch
        CCSpriteBatchNode ballsBatch;
        CCTexture2D ballTexture;

        public GameLayer ()
        {
            var touchListener = new CCEventListenerTouchAllAtOnce ();
            touchListener.OnTouchesEnded = OnTouchesEnded;

            AddEventListener (touchListener, this);
            Color = new CCColor3B (CCColor4B.White);
            Opacity = 255;

            visibleBananas = new List<CCSprite> ();
            hitBananas = new List<CCSprite> ();

            // batch node for physics balls
            ballsBatch = new CCSpriteBatchNode ("balls", 100);
            ballTexture = ballsBatch.Texture;
            AddChild (ballsBatch, 1, 1);
	
            AddGrass ();
            AddSun ();
            AddMonkey ();

            Schedule (t => {
                visibleBananas.Add (AddBanana ());
                elapsedTime += t;
                if (ShouldEndGame ()) {
                    EndGame ();
                }
                AddBall ();
            }, 1.0f);

            Schedule (t => CheckCollision ());

            Schedule (t => {
                world.Step (t, 8, 1);

                foreach (CCPhysicsSprite sprite in ballsBatch.Children) {
                    if (sprite.Visible && sprite.PhysicsBody.Position.x < 0f || sprite.PhysicsBody.Position.x * PTM_RATIO > ContentSize.Width) { //or should it be Layer.VisibleBoundsWorldspace.Size.Width
                        world.DestroyBody (sprite.PhysicsBody);
                        sprite.Visible = false;
                        sprite.RemoveFromParent ();
                    } else {
                        sprite.UpdateTransformedSpriteTextureQuads ();
                    }
                }
            });
        }

        void AddGrass ()
        {
            grass = new CCSprite ("grass");
            AddChild (grass);
        }

        void AddSun ()
        {
            circleNode = new CCDrawNode ();
            circleNode.DrawSolidCircle (CCPoint.Zero, 30.0f, CCColor4B.Yellow);
            AddChild (circleNode);

            sun = new CCParticleSun (CCPoint.Zero);
            sun.StartColor = new CCColor4F (CCColor3B.Red);
            sun.EndColor = new CCColor4F (CCColor3B.Yellow);
            AddChild (sun);
        }

        void AddMonkey ()
        {
            var spriteSheet = new CCSpriteSheet ("animations/monkey.plist");
            var animationFrames = spriteSheet.Frames.FindAll ((x) => x.TextureFilename.StartsWith ("frame"));

            walkAnim = new CCAnimation (animationFrames, 0.1f);
            walkRepeat = new CCRepeatForever (new CCAnimate (walkAnim));
            monkey = new CCSprite (animationFrames.First ()) { Name = "Monkey" };
            monkey.Scale = 0.25f;

            AddChild (monkey);
        }

        CCSprite AddBanana ()
        {
            var spriteSheet = new CCSpriteSheet ("animations/monkey.plist");
            var banana = new CCSprite (spriteSheet.Frames.Find ((x) => x.TextureFilename.StartsWith ("Banana")));

            var p = GetRandomPosition (banana.ContentSize);
            banana.Position = p;
            banana.Scale = 0.5f;

            AddChild (banana);

            var moveBanana = new CCMoveTo (5.0f, new CCPoint (banana.Position.X, 0));
            banana.RunActions (moveBanana, moveBananaComplete);
            banana.RepeatForever (rotateBanana);

            return banana;
        }

        CCPoint GetRandomPosition (CCSize spriteSize)
        {
            double rnd = CCRandom.NextDouble ();
            double randomX = (rnd > 0) 
                ? rnd * VisibleBoundsWorldspace.Size.Width - spriteSize.Width / 2 
                : spriteSize.Width / 2;

            return new CCPoint ((float)randomX, VisibleBoundsWorldspace.Size.Height - spriteSize.Height / 2);
        }

        void AddClouds ()
        {
            float h = VisibleBoundsWorldspace.Size.Height;

            parallaxClouds = new CCParallaxNode {
                Position = new CCPoint (0, h)
            };
             
            AddChild (parallaxClouds);

            var cloud1 = new CCSprite ("cloud");
            var cloud2 = new CCSprite ("cloud");
            var cloud3 = new CCSprite ("cloud");

            float yRatio1 = 1.0f;
            float yRatio2 = 0.15f;
            float yRatio3 = 0.5f;

            parallaxClouds.AddChild (cloud1, 0, new CCPoint (1.0f, yRatio1), new CCPoint (100, -100 + h - (h * yRatio1)));
            parallaxClouds.AddChild (cloud2, 0, new CCPoint (1.0f, yRatio2), new CCPoint (250, -200 + h - (h * yRatio2)));
            parallaxClouds.AddChild (cloud3, 0, new CCPoint (1.0f, yRatio3), new CCPoint (400, -150 + h - (h * yRatio3)));
        }

        void MoveClouds (float dy)
        {
            parallaxClouds.StopAllActions ();
            var moveClouds = new CCMoveBy (1.0f, new CCPoint (0, dy * 0.1f));
            parallaxClouds.RunAction (moveClouds);
        }

        void CheckCollision ()
        {
            visibleBananas.ForEach (banana => {
                bool hit = banana.BoundingBoxTransformedToParent.IntersectsRect (monkey.BoundingBoxTransformedToParent);
                if (hit) {
                    hitBananas.Add (banana);
                    CCSimpleAudioEngine.SharedEngine.PlayEffect ("Sounds/tap");
                    Explode (banana.Position);
                    banana.RemoveFromParent ();
                }
            });

            hitBananas.ForEach (banana => visibleBananas.Remove (banana));

            int ballHitCount = ballsBatch.Children.Count (ball => ball.BoundingBoxTransformedToParent.IntersectsRect (monkey.BoundingBoxTransformedToParent));

            if (ballHitCount > 0) {
                EndGame ();
            }
        }

        void EndGame ()
        {
            var gameOverScene = GameOverLayer.SceneWithScore (Window, hitBananas.Count);
            var transitionToGameOver = new CCTransitionMoveInR (0.3f, gameOverScene);
            Director.ReplaceScene (transitionToGameOver);
        }

        void Explode (CCPoint pt)
        {
            var explosion = new CCParticleExplosion (pt); //TODO: manage "better" for performance when "many" particles
            explosion.TotalParticles = 10;
            explosion.AutoRemoveOnFinish = true;
            AddChild (explosion);
        }

        bool ShouldEndGame ()
        {
            return elapsedTime > GAME_DURATION;
        }

        void OnTouchesEnded (List<CCTouch> touches, CCEvent touchEvent)
        {
            monkey.StopAllActions ();

            var location = touches [0].LocationOnScreen;
            location = WorldToScreenspace (location);  //Layer.WorldToScreenspace(location); 
            float ds = CCPoint.Distance (monkey.Position, location);

            var dt = ds / MONKEY_SPEED;

            var moveMonkey = new CCMoveTo (dt, location);

            //BUG: calling walkRepeat separately as it doesn't run when called in RunActions or CCSpawn
            monkey.RunAction (walkRepeat);
            monkey.RunActions (moveMonkey, walkAnimStop);

            // move the clouds relative to the monkey's movement
            MoveClouds (location.Y - monkey.Position.Y);
        }

        protected override void AddedToScene ()
        {
            base.AddedToScene ();

            Scene.SceneResolutionPolicy = CCSceneResolutionPolicy.NoBorder;

            grass.Position = VisibleBoundsWorldspace.Center;
            monkey.Position = VisibleBoundsWorldspace.Center;

            var b = VisibleBoundsWorldspace;
            sun.Position = b.UpperRight.Offset (-100, -100); //BUG: doesn't appear in visible area on Nexus 7 device

            circleNode.Position = sun.Position;

            AddClouds ();
        }

        void InitPhysics ()
        {
            CCSize s = Layer.VisibleBoundsWorldspace.Size;

            var gravity = new b2Vec2 (0.0f, -10.0f);
            world = new b2World (gravity);

            world.SetAllowSleeping (true);
            world.SetContinuousPhysics (true);

            var def = new b2BodyDef ();
            def.allowSleep = true;
            def.position = b2Vec2.Zero;
            def.type = b2BodyType.b2_staticBody;
            b2Body groundBody = world.CreateBody (def);
            groundBody.SetActive (true);

            b2EdgeShape groundBox = new b2EdgeShape ();
            groundBox.Set (b2Vec2.Zero, new b2Vec2 (s.Width / PTM_RATIO, 0));
            b2FixtureDef fd = new b2FixtureDef ();
            fd.shape = groundBox;
            groundBody.CreateFixture (fd);
        }

        void AddBall ()
        {
            int idx = (CCRandom.Float_0_1 () > .5 ? 0 : 1);
            int idy = (CCRandom.Float_0_1 () > .5 ? 0 : 1);
            var sprite = new CCPhysicsSprite (ballTexture, new CCRect (32 * idx, 32 * idy, 32, 32), PTM_RATIO);

            ballsBatch.AddChild (sprite);

            CCPoint p = GetRandomPosition (sprite.ContentSize);

            sprite.Position = new CCPoint (p.X, p.Y);

            var def = new b2BodyDef ();
            def.position = new b2Vec2 (p.X / PTM_RATIO, p.Y / PTM_RATIO);
            def.type = b2BodyType.b2_dynamicBody;
            b2Body body = world.CreateBody (def);

            var circle = new b2CircleShape ();
            circle.Radius = 0.5f;

            var fd = new b2FixtureDef ();
            fd.shape = circle;
            fd.density = 1f;
            fd.restitution = 0.85f;
            fd.friction = 0.3f;
            body.CreateFixture (fd);

            sprite.PhysicsBody = body;

            Console.WriteLine ("sprite batch node count = {0}", ballsBatch.ChildrenCount);
        }

        public override void OnEnter ()
        {
            base.OnEnter ();

            InitPhysics ();
        }

        public static CCScene GameScene (CCWindow mainWindow)
        {
            var scene = new CCScene (mainWindow);
            var layer = new GameLayer ();
			
            scene.AddChild (layer);

            return scene;
        }
    }
}