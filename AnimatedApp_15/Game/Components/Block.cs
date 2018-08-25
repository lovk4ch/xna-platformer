using AnimatedApp_15.Servants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimatedApp_15.Components
{
    public class Block
    {
        protected Texture2D texture;
        protected Level level;
        public Rectangle rect;
        public Block(Rectangle rect, Texture2D texture, Level level)
        {
            this.texture = texture;
            this.level = level;
            this.rect = rect;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            Rectangle screenRect = Hero.GetScreenRect(rect);
            spriteBatch.Draw(texture, screenRect, Color.White);
            spriteBatch.End();
        }
    }
}