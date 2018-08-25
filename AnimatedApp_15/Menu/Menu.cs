using AnimatedApp_15.Components;
using AnimatedApp_15.Game.Change;
using AnimatedApp_15.Servants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace AnimatedApp_15.MenuSystem
{
    class Menu : DrawableGameComponent
    {
        public Menu(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            this.oldState = Keyboard.GetState();
            this.Items = new List<MenuItem>();
        }

        //bool route; // эффект масштаба
        //float itemScale;
        public List<MenuItem> Items { get; set; }
        KeyboardState oldState;
        int currentItem;
        ItemV itemV;
        SpriteFont font;
        SpriteBatch spriteBatch;
        Texture2D sparkTexture;
        Texture2D menuBackground;
        public override void Update(GameTime gameTime)
        {
            if (InputManager.IsKeyPress(Keys.Enter))
                Items[currentItem].OnClick();

            int delta = 0;
            if (InputManager.IsKeyPress(Keys.Up))
                delta = -1;
            if (InputManager.IsKeyPress(Keys.Down))
                delta = 1;
            currentItem += delta;

            bool ok = false;
            while (!ok)
            {
                if (currentItem > Items.Count - 1)
                    currentItem = 0;
                else if (currentItem < 0)
                    currentItem = Items.Count - 1;
                else if (!Items[currentItem].Active)
                    currentItem += delta;
                else ok = true;
            }

            // Эффект увеличения

            float time = (float)gameTime.ElapsedGameTime.Ticks / 15000000;
            //itemScale += time * Help.Sign(route);
            //if (route)
            //{
            //    if (itemScale >= 0.95f)
            //        route = !route;
            //}
            //else
            //{
            //    if (itemScale <= 0.75f)
            //        route = !route;
            //}
            itemV.Update(gameTime, Items[currentItem].Name, font, 1, TestGame.Height / 2 + currentItem * font.LineSpacing);
        }
        protected override void LoadContent()
        {
            menuBackground = Game.Content.Load<Texture2D>("Textures/Menu");
            font = Game.Content.Load<SpriteFont>("Fonts/MenuFont");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sparkTexture = Game.Content.Load<Texture2D>("Textures/star_spark");
        }
        public override void Initialize()
        {
            MenuItem newGame = new MenuItem("Начать новую игру");
            MenuItem resume = new MenuItem("Продолжить");
            MenuItem settings = new MenuItem("Настройки");
            MenuItem editor = new MenuItem("Редактор");
            MenuItem exit = new MenuItem("Выйти из игры");

            newGame.Click += newGame_Click;
            resume.Click += resume_Click;
            exit.Click += exit_Click;
            editor.Click += editor_Click;
            settings.Click += settings_Click;

            Items.Add(newGame);
            Items.Add(resume);
            Items.Add(settings);
            Items.Add(editor);
            Items.Add(exit);
            base.Initialize();

            //route = true;
            //itemScale = 1;

            switch (TestGame.gameState)
            {
                case TestGame.GameState.Game:
                    currentItem = 1;
                    break;
                case TestGame.GameState.Edit:
                    resume.Active = false;
                    currentItem = 2;
                    break;
                default:
                    resume.Active = false;
                    break;
            }

            itemV = new ItemV(sparkTexture);
        }
        void newGame_Click(object sender, EventArgs e)
        {
            foreach (GameComponent component in Game.Components)
                if (component.GetType() == typeof(Level))
                {
                    Game.Components.Remove(component);
                    break;
                }
            TestGame.gameState = TestGame.GameState.Game;
            Game.Components.Add(new Transition(Game, this, new Level(Game)));
        }
        void resume_Click(object sender, EventArgs e)
        {
            TestGame.gameState = TestGame.GameState.Game;
            Level.levelState = Level.oldLevelState;
            Game.Components.Add(new Transition(Game, this, null));
        }
        void exit_Click(object sender, EventArgs e)
        {
            Game.Exit();
        }
        void editor_Click(object sender, EventArgs e)
        {
            TestGame.gameState = TestGame.GameState.Edit;
            Game.Components.Add(new Transition(Game, this, new Editor(Game)));
        }
        void settings_Click(object sender, EventArgs e)
        {
            Game.Components.Add(new Transition(Game, this, new Settings(Game)));
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(menuBackground, new Rectangle(0, 0, TestGame.Width, TestGame.Height), Color.White);
            int y = TestGame.Height / 2;
            for (int i = 0; i < Items.Count; i++)
            {
                Color color = Color.Cyan;
                if (Items[i].Active == false)
                    color = Color.Black;
                if (i == currentItem)
                    color = Color.Red;
                Help.DrawCentered(Items[i].Name, null, 0, TestGame.Width, y, color, 1, font, spriteBatch);
                y += font.LineSpacing;

            }
            spriteBatch.End();
            itemV.Draw(spriteBatch);
        }
    }
}