using AnimatedApp_15.Components;
using AnimatedApp_15.Game.Change;
using AnimatedApp_15.MenuSystem;
using AnimatedApp_15.Servants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace AnimatedApp_15.Game.Conditions
{
    public class Completed : DrawableGameComponent
    {
        enum CompletedState { Complete, Total }
        void Score(int a, int b, float dt)
        {
            if (timeToEnd < a)
            {
                if (a == 3) items[b].indent += items[b].d1 * dt;
                else Score(a - 1, b - 1, dt);
            }
            else
            {
                items[b].imTotal += items[b].total * dt;
                if (a != 6)
                    items[b + 1].indent += items[b + 1].d1 * dt;
            }
        }
        void CreateItems()
        {
            items = new List<TotalItem>();
            timeToEnd = 0;
            items.Add(new TotalItem("Собрано изумрудов:", level.gems.Capacity - 1,
                totalFont, font, text));
            items.Add(new TotalItem("Врагов уничтожено:", level.Score / 100,
                totalFont, font, text));
            items.Add(new TotalItem("Бонус за шкалу жизни:", level.hero.power,
                totalFont, font, text));
            int total = 0;
            foreach (TotalItem item in items) total += (int)item.total;
            items.Add(new TotalItem("Результат:", total, totalFont, font, text));
            state = CompletedState.Total;
        }
        class TotalItem
        {
            public TotalItem(string l1, int total, SpriteFont font1, SpriteFont font2, string text)
            {
                float measure = (TestGame.Width - font2.MeasureString(text).X) / 2;
                this.d1 = font1.MeasureString(l1).X + measure;
                this.d2 = font1.MeasureString("0").X;
                this.l1 = l1;
                this.total = total;
                this.dk = d1 / (d2 + measure);
            }

            public float indent;
            public float dk;
            public float d1;
            public float d2;
            public string l1;
            public float total;
            public float imTotal;
        }

        Texture2D blackoutTexture;
        Texture2D idleTexture;

        CompletedState state;
        double timeToEnd;
        Level level;
        string text;
        float symbols;
        float textHigh;
        List<TotalItem> items;

        SoundEffect totalSound;
        SoundEffect beepSound;
        SpriteFont font;
        SpriteFont totalFont;
        SpriteBatch spriteBatch;
        public Completed(Microsoft.Xna.Framework.Game game, Level level) : base(game)
        {
            this.text = "Уровень пройден!";
            this.DrawOrder = 1;
            this.level = level;
            Level.levelState = Level.LevelState.Overview;
        }
        protected override void LoadContent()
        {
            blackoutTexture = Game.Content.Load<Texture2D>("Textures/void");
            idleTexture = Game.Content.Load<Texture2D>("Textures/idle");
            font = Game.Content.Load<SpriteFont>("Fonts/levelFont");
            totalFont = Game.Content.Load<SpriteFont>("Fonts/introFont");

            totalSound = Game.Content.Load<SoundEffect>("Sound/total");
            beepSound = Game.Content.Load<SoundEffect>("Sound/beep");

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.Ticks / 10000000;
            switch (state)
            {
                case CompletedState.Complete:
                    if (symbols < text.Length)
                    {
                        timeToEnd += dt;
                        if (timeToEnd > 4)
                        {
                            int t = (int)symbols;
                            symbols += dt * 10;
                            if ((int)symbols > t && text[t] != ' ') beepSound.Play();
                        }
                    }
                    else CreateItems();
                    break;

                case CompletedState.Total:
                    timeToEnd += (double)gameTime.ElapsedGameTime.
                        Ticks / 10000000;
                    if (timeToEnd < 2)
                    {
                        textHigh += dt * TestGame.Height / 6;
                    }
                    else
                    {
                        if (timeToEnd < 7)
                        {
                            if (timeToEnd < 6)
                            {
                                if ((int)(timeToEnd + dt) > (int)timeToEnd)
                                    totalSound.Play();
                            }
                            Score(6, items.Count - 1, dt);
                        }
                        else
                        {
                            if (InputManager.isAnyKeyPress())
                            {
                                Game.Components.Add(new Transition(Game, new List<IGameComponent> { this, level },
                                    new List<IGameComponent> { new Menu(Game) }));
                                TestGame.gameState = TestGame.GameState.Menu;
                            }
                        }
                    }
                    break;
            }
        }
        public override void Draw(GameTime gameTime)
        {
            switch (state)
            {
                case CompletedState.Complete:
                    Rectangle screenRect = Hero.GetScreenRect(level.hero.rect);
                    if (timeToEnd < 2)
                    {
                        spriteBatch.Begin();
                        spriteBatch.Draw(idleTexture, screenRect, new Color(new Vector4(255, 255, 255, 1 - (float)timeToEnd / 2)));
                    }
                    else
                    {
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
                        spriteBatch.Draw(idleTexture, screenRect, new Color(new Vector4(255, 255, 255, (3 - (float)timeToEnd))));
                    }
                    break;

                case CompletedState.Total:
                    spriteBatch.Begin();
                    if (timeToEnd < 2)
                    {
                        spriteBatch.Draw(blackoutTexture, new Rectangle(0, 0, TestGame.Width, TestGame.Height),
                            new Color(new Vector4(0, 0, 0, (float)timeToEnd / 2 - .15f)));
                    }
                    else
                    {
                        spriteBatch.Draw(blackoutTexture, new Rectangle(0, 0, TestGame.Width, TestGame.Height),
                            new Color(new Vector4(0, 0, 0, 0.85f)));

                        for (int i = 0; i < items.Count; i++)
                        {
                            spriteBatch.DrawString(totalFont, items[i].l1, new Vector2(items[i].indent - totalFont.MeasureString(items[i].l1).X,
                                (int)(TestGame.Height * 0.4f + totalFont.LineSpacing * (i + (1 + Math.Sign(i - items.Count + 1))))), Color.White);
                            string score = ((int)items[i].imTotal).ToString();
                            spriteBatch.DrawString(totalFont, score,
                                new Vector2((TestGame.Width - items[i].indent / items[i].dk) + items[i].d2 - totalFont.MeasureString(score).X,
                                (int)(TestGame.Height * 0.4f + totalFont.LineSpacing * (i + (1 + Math.Sign(i - items.Count + 1))))), Color.White);
                        }
                        if (timeToEnd > 7)
                        {
                            Help.DrawCentered("Нажмите любую клавишу...", null, 0, TestGame.Width, (int)(TestGame.Height * 0.8f),
                                new Color(new Vector4(Math.Sign(Math.Cos(timeToEnd * 3)))), 1, level.font, spriteBatch);
                        }
                    }
                    break;
            }
            Help.DrawCentered(text, text.Substring(0, (int)symbols), 0, TestGame.Width, (TestGame.Height
                - font.LineSpacing) / 2 - (int)textHigh, Color.White, 1, font, spriteBatch);
            spriteBatch.End();
        }
    }
}