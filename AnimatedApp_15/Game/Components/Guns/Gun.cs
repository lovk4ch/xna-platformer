using AnimatedApp_15.Servants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace AnimatedApp_15.Game.Components.Guns
{
    public abstract class Gun
    {
        protected float shotTime;
        protected bool mouse;
        protected bool fix;
        protected Hero hero;
        protected float angle;
        protected float reload;
        public abstract void LoadContent(ContentManager Content);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
        protected float GetAngle()
        {
            float cat1 = Mouse.GetState().X + Hero.ScrollX - hero.rect.Left - hero.rect.Width / 2;
            float cat2 = hero.rect.Top + hero.rect.Height / 2 - Mouse.GetState().Y;
            float hyp = (float)Math.Sqrt(Math.Pow(cat1, 2) + Math.Pow(cat2, 2));
            return (float)Math.Acos(cat1 / hyp) * Math.Sign(cat2); // угол прицела
        } // направление прицела
        protected bool UpdateMouse(GameTime gameTime)
        {
            /// Mouse Event Handler For Gun
            /// Стрельба
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (shotTime < reload) shotTime += (float)gameTime.ElapsedGameTime.Ticks / 10000000;
                else
                {
                    return true;
                }
            }
            else shotTime = reload;
            return false;
        }
        protected bool UpdateKeyboard(GameTime gameTime)
        {
            /// Keys Event Handler For Gun
            /// Стрельба
            if (InputManager.IsKeyPress(Keys.Z))
                fix = !fix;

            if (InputManager.IsKeyDown(Keys.LeftControl))
            {
                if (!InputManager.IsKeyPress(Keys.LeftControl))
                {
                    if (shotTime < reload) shotTime += (float)gameTime.ElapsedGameTime.Ticks / 10000000;
                    else
                    {
                        if (!fix)
                        {
                            if (InputManager.IsKeyDown(Keys.Up))
                            {
                                if (InputManager.IsKeyDown(Keys.Left))
                                    angle = (float)Math.PI * 3 / 4;
                                else
                                    if (InputManager.IsKeyDown(Keys.Right))
                                        angle = (float)Math.PI / 4;
                                    else angle = (float)Math.PI / 2;
                            }
                            else if (InputManager.IsKeyDown(Keys.Down))
                            {
                                if (InputManager.IsKeyDown(Keys.Left))
                                    angle = (float)-Math.PI * 3 / 4;
                                else
                                    if (InputManager.IsKeyDown(Keys.Right))
                                        angle = (float)-Math.PI / 4;
                                    else angle = (float)-Math.PI / 2;
                            }
                            else if (InputManager.IsKeyDown(Keys.Right))
                                angle = 0;
                            else if (InputManager.IsKeyDown(Keys.Left))
                                angle = (float)Math.PI;
                        }
                        return true;
                    }
                }
                else shotTime = reload;
            }
            return false;
        }
    }
}