using AnimatedApp_15.Components;
using AnimatedApp_15.Servants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AnimatedApp_15.Game.Conditions
{
    public class Preview : DrawableGameComponent
    {
        SoundEffect startSound;
        Texture2D idleTexture;
        float timeToStart;
        bool isPlay;
        Level level;
        SpriteFont font;
        SpriteBatch spriteBatch;
        public Preview(Microsoft.Xna.Framework.Game game, Level level) : base(game)
        {
            Hero.ScrollX = Level.levelLength - TestGame.Width;
            this.DrawOrder = 1;
            this.level = level;
        }
        protected override void LoadContent()
        {
            idleTexture = Game.Content.Load<Texture2D>("Textures/idle");
            startSound = Game.Content.Load<SoundEffect>("Sound/start");
            font = Game.Content.Load<SpriteFont>("Fonts/previewFont");

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        public override void Update(GameTime gameTime)
        {
            if (Hero.ScrollX > 0)
            {
                Hero.ScrollX -= Level.levelLength * (float)gameTime.ElapsedGameTime.Ticks / 100000000;
                level.background.Update(gameTime);
            }
            else
            {
                if (timeToStart < 7)
                {
                    timeToStart += (float)gameTime.ElapsedGameTime.Ticks / 10000000;
                    if (timeToStart > 3)
                    {
                        if (!isPlay)
                        {
                            startSound.Play();
                            isPlay = true;
                        }
                        else
                        {
                            SoundEffectInstance instance = startSound.CreateInstance();
                            if (instance.State == SoundState.Paused)
                                instance.Play();
                        }
                    }
                }
                else
                {
                    Level.levelState = Level.LevelState.Active;
                    Game.Components.Remove(this);
                    timeToStart = 0;
                    isPlay = false;
                }
            }

            if (InputManager.IsKeyDown(Keys.Escape))
                startSound.CreateInstance().Pause();
        }
        public override void Draw(GameTime gameTime)
        {
            Rectangle screenRect = Hero.GetScreenRect(level.hero.rect);
            BlendState state = BlendState.AlphaBlend;
            float alpha = (timeToStart - 1) / 2;
            if (timeToStart < 1)
            {
                state = BlendState.Additive;
                alpha = timeToStart;
            }
            string text = "FIGHT!";
            spriteBatch.Begin(SpriteSortMode.Deferred, state);

            if (timeToStart > 3)
            {
                if (timeToStart < 6)
                    text = ((int)(7 - timeToStart)).ToString();

                Help.DrawCentered(text, null, 0, TestGame.Width, (TestGame.Height - font.LineSpacing) / 2, Color.White,
                    1, font, spriteBatch);
            }
            spriteBatch.Draw(idleTexture, screenRect, new Color(new Vector4(255, 255, 255, alpha)));
            Help.DrawCentered(level.hero.power.ToString(), null,
                screenRect.Left,
                level.hero.rect.Width,
                level.hero.rect.Top - level.font.LineSpacing,
                new Color(new Vector4(255, 255, 255, alpha)), 1, level.font, spriteBatch);

            spriteBatch.End();
        }
    }
}