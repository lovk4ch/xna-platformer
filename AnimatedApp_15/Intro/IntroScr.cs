using AnimatedApp_15.Game.Change;
using AnimatedApp_15.MenuSystem;
using AnimatedApp_15.Servants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace AnimatedApp_15.Intro
{
    public class IntroScr : DrawableGameComponent
    {
        public enum IntroState { Text }

        Texture2D sparkTexture;
        Texture2D university;
        Texture2D lovkach;

        SpriteBatch spriteBatch;
        SpriteFont font;
        SpriteFont pressFont;
        SoundEffect beepSound;

        float presentsLetters;
        float timeToEnd;
        string presents;
        string text;
        Logo logo;
        float textLetters;
        Random random;
        List<IntroEffects> effects;
        public IntroScr(TestGame game)
            : base(game)
        {
            this.text = "Самарский Государственный Аэрокосмический\nУниверситет им. С.П.Королёва";
            this.presents = "Представляют:";
            TestGame.gameState = TestGame.GameState.Intro;
            effects = new List<IntroEffects>();
            this.random = new Random();
        }
        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.Ticks / 10000000;
            timeToEnd += dt;
            if (timeToEnd > 1)
            {
                if (textLetters < text.Length)
                {
                    int t = (int)textLetters;
                    textLetters += dt * 10;
                    if ((int)textLetters > t && text[t] != ' ') beepSound.Play();
                }
                if (timeToEnd > 9)
                {
                    logo.Update(gameTime);
                }
                if (timeToEnd > 12)
                {
                    if (presentsLetters < presents.Length)
                    {
                        int t = (int)presentsLetters;
                        presentsLetters += dt * 10;
                        if ((int)presentsLetters > t) beepSound.Play();
                    }
                    else if (InputManager.isAnyKeyPress())
                    {
                        TestGame.gameState = TestGame.GameState.Menu;
                        Game.Components.Add(new Transition(Game, this, new Menu(Game)));
                    }
                }
            }

            if (InputManager.IsKeyPress(Keys.Enter))
            {
                TestGame.gameState = TestGame.GameState.Menu;
                Game.Components.Add(new Transition(Game, this, new Menu(Game)));
            }
            for (int i = 0; i < effects.Count; i++)
            {
                effects[i].Update(gameTime);
                if (!effects[i].crd.isFlight && effects[i].particles.Count == 0)
                {
                    effects.RemoveAt(i);
                    effects.Add(new IntroEffects(sparkTexture, random));
                }
            }
        }
        protected override void LoadContent()
        {
            sparkTexture = Game.Content.Load<Texture2D>("Textures/star_spark");
            university = Game.Content.Load<Texture2D>("Textures/university");
            lovkach = Game.Content.Load<Texture2D>("Textures/lovkach");

            beepSound = Game.Content.Load<SoundEffect>("Sound/beep");

            font = Game.Content.Load<SpriteFont>("Fonts/introFont");
            pressFont = Game.Content.Load<SpriteFont>("Fonts/gameFont");

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        public override void Initialize()
        {
            base.Initialize();
            this.logo = new Logo(lovkach, sparkTexture);
            for (int i = 0; i < 1; i++) effects.Add(new IntroEffects(sparkTexture, random));
        }
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);

            if (timeToEnd > 4)
            {
                if (timeToEnd < 9)
                {
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    spriteBatch.Draw(university, new Rectangle(0, 0, TestGame.Width, TestGame.Height), new Color(0, 0, (timeToEnd - 4) / 9, 0));
                    spriteBatch.End();
                }
                else
                {
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    spriteBatch.Draw(university, new Rectangle(0, 0, TestGame.Width, TestGame.Height), new Color(0, 0, 0.5f, 0));
                    spriteBatch.End();
                    if (timeToEnd > 14)
                    {
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
                        Help.DrawCentered("Нажмите любую клавишу...", null, 0, TestGame.Width, (int)((TestGame.Height * 0.94f)),
                            new Color(1f, 1f, 1f, (float)Math.Sign(Math.Sin(timeToEnd * 3))), 1, pressFont, spriteBatch);
                        spriteBatch.End();
                    }
                    logo.Draw(spriteBatch);
                }
            }

            spriteBatch.Begin();
            Help.DrawCentered(text, text.Substring(0, (int)textLetters), 0, TestGame.Width, (int)((TestGame.Height
                - font.LineSpacing) * 0.83f), Color.White, 1, font, spriteBatch);
            Help.DrawCentered(presents, presents.Substring(0, (int)presentsLetters), 0, TestGame.Width,
                (int)((TestGame.Height * 0.36f)), Color.White, 1, font, spriteBatch);
            spriteBatch.End();
            for (int i = 0; i < effects.Count; i++)
                effects[i].Draw(spriteBatch);
        }
    }
}