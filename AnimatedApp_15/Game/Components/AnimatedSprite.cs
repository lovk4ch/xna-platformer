using AnimatedApp_15.Servants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimatedApp_15.Components
{
    public abstract class AnimatedSprite
    {
        //---------------Текстуры---------------//
        #region
        public bool CollidesWithLevel(Rectangle rect)
        {
            int minx = rect.Left / 50;
            int miny = rect.Top / 50;
            int maxx = rect.Right / 50;
            int maxy = rect.Bottom / 50;
            for (int i = minx; i <= maxx; i++)
                for (int j = miny; j <= maxy; j++)
                {
                    if (j < level.levelMap.GetLength(1))
                    {
                        if (level.levelMap[i, j] == 1)
                            return true;
                    }
                }
            return false;
        }

        protected Texture2D idleTexture;
        protected Texture2D runTexture;
        protected Texture2D jumpTexture;
        protected int FrameCount
        {
            get { return runTexture.Width / frameWidth; }
        }

        protected float timeElapsed;
        public Rectangle rect;
        public int power = 5;
        public int number;
        public Level level;

        protected int frameWidth;
        protected int frameHeight;
        protected int currentFrame;
        protected int timeForFrame = 100;
        public virtual void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime.Ticks / 10000;
            if (timeElapsed > timeForFrame)
            {
                timeElapsed = 0;
                currentFrame = (currentFrame + 1) % FrameCount;
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            Rectangle sourceRect = new Rectangle(frameWidth * currentFrame, 0, frameWidth, frameHeight);
            Rectangle screenRect = Hero.GetScreenRect(rect);

            SpriteEffects effect = SpriteEffects.None;
            if (!isRunningRight)
                effect = SpriteEffects.FlipHorizontally;

            if (isJumping)
            {
                spriteBatch.Draw(jumpTexture, screenRect, sourceRect, Color.White, 0, Vector2.Zero, effect, 0);
            }
            else
                if (isRunning)
                {
                    spriteBatch.Draw(runTexture, screenRect, sourceRect, Color.White, 0, Vector2.Zero, effect, 0);
                }
                else
                {
                    spriteBatch.Draw(idleTexture, screenRect, Color.White);
                }
            Help.DrawCentered(power.ToString(), null, screenRect.Left, rect.Width, rect.Top - level.font.LineSpacing,
                Color.White, 1, level.font, spriteBatch);
            spriteBatch.End();
        }
        public Rectangle GetBoundingRect(Rectangle rectangle)
        {
            int width = (int)(rectangle.Width * 0.4f);
            int dx = (int)(rectangle.Width * 0.3f);

            return new Rectangle(rectangle.X + dx, rectangle.Y, width, rectangle.Height);
        }

        #endregion

        //---------------Движение---------------//
        #region

        public void Run(bool right)
        {
            if (!isRunning)
            {
                isRunning = true;
                currentFrame = 0;
                timeElapsed = 0;
            }
            isRunningRight = !right;
        }
        public void Stop()
        {
            isRunning = false;
            currentFrame = 0;
            timeElapsed = 0;
        }

        protected bool isRunning;
        public bool isRunningRight;

        #endregion

        //--------------Гравитация--------------//
        #region
        protected void ApplyGravity(GameTime gameTime)
        {
            ySpeed = ySpeed - g * gameTime.ElapsedGameTime.Ticks / 100000;
            float dy = ySpeed * gameTime.ElapsedGameTime.Ticks / 100000;

            Rectangle nextPosition = new Rectangle(rect.X, (int)(y - dy),
                rect.Width, rect.Height);
            Rectangle boundingRect = GetBoundingRect(nextPosition);

            if (nextPosition.Top > 0)
            {
                if (CollidesWithLevel(boundingRect))
                {
                    if (ySpeed < 0)
                    {
                        isJumping = false;
                        ySpeed = 0;
                    }
                }
                else
                {
                    if (nextPosition.Top < TestGame.Height)
                    {
                        rect = nextPosition;
                        y -= dy;
                    }
                    else level.CreateLevel(0);
                }
            }
        }
        public void Jump()
        {
            if (!isJumping && ySpeed == 0)
            {
                ySpeed = maxYSpeed;
                isJumping = true;
                currentFrame = 0;
                timeElapsed = 0;
            }
        }

        protected float maxYSpeed;
        protected bool isJumping;
        public float xSpeed;
        public float x;
        public float y;
        public float ySpeed;
        protected float g = 0.206f;

        #endregion
    }
}