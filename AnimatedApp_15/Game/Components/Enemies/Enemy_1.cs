using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimatedApp_15.Components.Enemies
{
    public class Enemy_1 : AnimatedApp_15.Components.AnimatedSprite
    {
        public Enemy_1(int number, Rectangle rect, Texture2D idle, Texture2D run, Texture2D jump, int x, int y, Level level)
        {
            this.idleTexture = idle;
            this.number = number;
            this.maxYSpeed = y;
            this.level = level;
            this.xSpeed = x;
            this.x = rect.X;
            this.y = rect.Y;
            this.rect = rect;
            this.runTexture = run;
            this.jumpTexture = jump;

            frameWidth = frameHeight = run.Height;
        }
        public bool WillFallDown(Rectangle rect, bool state)
        {
            if (state) rect.Offset(rect.Width, 1);
            else rect.Offset(-rect.Width, 1);
            if (CollidesWithLevel(rect))
                return false;
            return true;
        }
        public override void Update(GameTime gameTime)
        {
            if (isRunning)
            {
                float dx = xSpeed * (float)gameTime.ElapsedGameTime.Ticks / 100000;
                if (!isRunningRight) dx = -dx;

                Rectangle nextPosition = new Rectangle((int)(x + dx), rect.Y, rect.Width, rect.Height);
                Rectangle boundingRect = GetBoundingRect(nextPosition);

                foreach (AnimatedSprite enemy in level.enemies)
                {
                    Rectangle enemyBoundingRect = enemy.GetBoundingRect(enemy.rect);
                    if (enemyBoundingRect.Intersects(boundingRect) && enemy.number != number)
                    {
                        if (enemy.power < power)
                        {
                            level.enemies.Remove(enemy);
                            power += enemy.power;
                        }
                        else
                        {
                            level.enemies.Remove(this);
                            enemy.power += power;
                            Stop();
                        }
                    }
                }

                if (boundingRect.Left < 0 || boundingRect.Right >= Level.levelLength)
                    isRunningRight = !isRunningRight;
                else if (CollidesWithLevel(boundingRect) || (WillFallDown(boundingRect, isRunningRight)))
                {
                    isRunningRight = !isRunningRight;
                }
                else
                {
                    rect = nextPosition;
                    x += dx;
                }
            }
            base.Update(gameTime);
        }
    }
}