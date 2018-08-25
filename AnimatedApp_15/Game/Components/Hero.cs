using AnimatedApp_15.Components;
using AnimatedApp_15.Game.Components.Guns;
using AnimatedApp_15.Servants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace AnimatedApp_15
{
    public class Hero : AnimatedSprite
    {
        float sight; // смещение позиции главного героя относительно центра
        public static float ScrollX;
        Gun gun;
        public float lives;
        public float health;
        public Hero(Rectangle rect, float x, float y, float lives, Level level)
        {
            this.health = this.lives = lives;
            this.maxYSpeed = y;
            this.xSpeed = x;
            this.rect = rect;
            this.sight = 0;
            this.x = rect.X;
            this.y = rect.Y;
            this.level = level;
            gun = new AnimatedApp_15.Game.Components.Guns.IonCannon.IonCannon(this, 0.2f, 40);
        }
        public static void Scroll(float dx)
        {
            if (ScrollX + dx < Level.levelLength - TestGame.Width)
            {
                if (ScrollX + dx > 0) ScrollX += dx;
                else ScrollX = 0;
            }
            else ScrollX = Level.levelLength - TestGame.Width;
        }
        public override void Update(GameTime gameTime)
        {
            /// Keys Event Handler For Hero
            /// Управление персонажем
            /// 
            if (InputManager.IsKeyDown(Keys.Left)) Run(true);
            else if (InputManager.IsKeyDown(Keys.Right))
                Run(false);
            else Stop();
            if (InputManager.IsKeyDown(Keys.Up)) Jump();

            float dx = xSpeed * gameTime.ElapsedGameTime.Ticks / 100000;
            Rectangle heroScreenRect = GetScreenRect(rect);
            int center = heroScreenRect.Left + rect.Width / 2;
            int ds = (int)(TestGame.Width / 2 * (1 - sight));
            if (center != ds) Scroll(dx * Help.Sign(center > ds));

            if (isRunning)
            {
                if (!isRunningRight)
                    dx = -dx;
            }
            else dx = 0;

            Rectangle nextPosition = new Rectangle((int)(x + dx), heroScreenRect.Y, heroScreenRect.Width, heroScreenRect.Height);
            Rectangle boundingRect = GetBoundingRect(nextPosition);
            Rectangle screenRect = GetScreenRect(boundingRect);

            if (screenRect.Right < TestGame.Width && screenRect.Left > 0 && !CollidesWithLevel(boundingRect))
            {
                rect = nextPosition;
                x += dx;
            }
            ApplyGravity(gameTime);
            base.Update(gameTime);
            gun.Update(gameTime);
        }
        public void LoadContent(ContentManager Content)
        {
            idleTexture = Content.Load<Texture2D>("Textures/idle");
            runTexture = Content.Load<Texture2D>("Textures/run");
            jumpTexture = Content.Load<Texture2D>("Textures/jump");
            gun.LoadContent(Content);

            frameWidth = frameHeight = runTexture.Height;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            gun.Draw(spriteBatch);
        }
        public static Rectangle GetScreenRect(Rectangle rect)
        {
            rect.Offset((int)Math.Round(-ScrollX), 0);
            return rect;
        }
    }
}