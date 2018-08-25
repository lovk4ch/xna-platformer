using AnimatedApp_15.Components;
using AnimatedApp_15.Servants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AnimatedApp_15
{
    public class Gem
    {
        public Rectangle rect;
        Texture2D texture;
        Level level;
        int dy;
        public Gem(Rectangle rect, Texture2D texture, Level level)
        {
            this.rect = rect;
            this.level = level;
            this.texture = texture;
        }
        public void Update(GameTime gameTime)
        {
            float t = (float)gameTime.TotalGameTime.TotalSeconds * 3 + rect.X * 10;
            dy = (int)(Math.Sin(t) * 10);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            Rectangle r = new Rectangle(rect.X, rect.Y + dy, rect.Width, rect.Height);
            Rectangle screenRect = Hero.GetScreenRect(r);
            spriteBatch.Draw(texture, screenRect, Color.White);
            spriteBatch.End();
        }
    }
}