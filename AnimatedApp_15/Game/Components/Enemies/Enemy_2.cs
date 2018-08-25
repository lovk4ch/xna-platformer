using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace AnimatedApp_15.Components.Enemies
{
    public class Enemy_2 : AnimatedApp_15.Components.AnimatedSprite
    {
        Random random = new Random();
        int timeForJump;
        bool isWaiting;
        public Enemy_2(int number, Rectangle rect, Texture2D idle, Texture2D run, Texture2D jump, int x, int y, Level level)
        {
            this.timeForJump = 1500 + random.Next(3500);
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
            //if (gameState == TestGame.GameState.Preview) goto Watch;

            if (isRunning || isWaiting)
            {
                float dx = xSpeed * (float)gameTime.ElapsedGameTime.Ticks / 100000;
                if (!isRunningRight) dx = -dx;

                Rectangle nextPosition = new Rectangle((int)(x + dx), rect.Y, rect.Width, rect.Height);
                Rectangle boundingRect = GetBoundingRect(nextPosition);

                isWaiting = false;
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
                            goto Watch;
                        }
                    }
                }

                if (boundingRect.Left < 0 || boundingRect.Right >= Level.levelLength)
                    isRunningRight = !isRunningRight;
                else if (CollidesWithLevel(boundingRect) || (WillFallDown(boundingRect, isRunningRight)))
                {
                    isRunningRight = !isRunningRight;
                    goto Watch;
                }
                else
                {
                    rect = nextPosition;
                    x += dx;
                }

                if (Math.Abs(level.hero.rect.Left - rect.Left) < dx)
                {
                    rect = new Rectangle(level.hero.rect.Left, rect.Top, rect.Width, rect.Height);
                    Stop();
                }
            }

            /*if (game.hero.rect.Left > rect.Left)
                Run(false);
            if (game.hero.rect.Left < rect.Left)
                Run(true);*/

        Watch:

            timeForJump -= gameTime.ElapsedGameTime.Milliseconds;
            if (timeForJump < 0)
            {
                Jump();
                timeForJump = 1500 + random.Next(3500);
            }

            ApplyGravity(gameTime);
            base.Update(gameTime);
        }
    }
}