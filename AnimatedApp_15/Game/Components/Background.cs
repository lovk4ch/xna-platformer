using AnimatedApp_15.Components;
using AnimatedApp_15.Servants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AnimatedApp_15.Game.Components
{
    public class Background
    {
        public float kBack;
        public int xBack;
        Level level;
        Texture2D texture;
        public Background(Level level)
        {
            this.level = level;
            this.xBack = 0;
            this.kBack = 1;
        }
        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Textures/Forest");
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, new Rectangle(-xBack, 0, (int)(TestGame.Width * kBack), TestGame.Height), Color.White);
            spriteBatch.End();
        }
        public void Update(GameTime gameTime)
        {
            if (Level.levelLength == TestGame.Width) xBack = 0;
            else xBack = (int)(Hero.ScrollX / (Level.levelLength - TestGame.Width) * (kBack - 1) * TestGame.Width);
        }
    }
}