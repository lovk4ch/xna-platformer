using AnimatedApp_15.Components.Enemies;
using AnimatedApp_15.Components.Portals;
using AnimatedApp_15.Game.Change;
using AnimatedApp_15.Game.Components;
using AnimatedApp_15.Game.Conditions;
using AnimatedApp_15.MenuSystem;
using AnimatedApp_15.Servants;
using AnimatedApp_15.Servants.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace AnimatedApp_15.Components
{
    public class Level : DrawableGameComponent
    {
        public enum LevelState { Active, Paused, Overview };
        public List<AnimatedSprite> enemies;
        public List<Block> blocks;
        public List<Gem> gems;
        public Level(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            enemies = new List<AnimatedSprite>();
            blocks = new List<Block>();
            gems = new List<Gem>();
        }
        protected override void LoadContent()
        {
            blockTexture2 = Game.Content.Load<Texture2D>("Textures/block_2");
            blockTexture = Game.Content.Load<Texture2D>("Textures/block");
            gemTexture = Game.Content.Load<Texture2D>("Textures/gem");

            shader = new ShaderController(Game.GraphicsDevice, this);
            shader.Initialize();

            sound = Game.Content.Load<SoundEffect>("Sound/gem");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background.LoadContent(Game.Content);
            font = Game.Content.Load<SpriteFont>("Fonts/gameFont");

            healthTexture = Game.Content.Load<Texture2D>("Textures/health");
            enemyRunTexture = Game.Content.Load<Texture2D>("Enemy/run");
            enemyIdleTexture = Game.Content.Load<Texture2D>("Enemy/idle");
        }
        public void CreateLevel(byte state)
        {
            LevelGenerator generator = new LevelGenerator(this);
            enemies.Clear();
            blocks.Clear();
            gems.Clear();
            background.xBack = 0;

            switch (state)
            {
                case 1:
                    currentLevel++;
                    level = generator.Generate();
                    break;
                default:
                    Score = 0;
                    break;
                case 2:
                    level = generator.Generate();
                    break;
            }

            levelMap = new byte[level[0].Length, level.Length];
            levelLength = 50 * level[0].Length;

            int x = 0;
            int y = 0;

            int i = 0;
            int j = 0;

            foreach (string str in level)
            {
                foreach (char c in str)
                {
                    if (c != '0') switch (c)
                    {
                        case 'G':
                            {
                                Rectangle gemRect = new Rectangle(x + 10, y + 10, 30, 30);
                                Gem gem = new Gem(gemRect, gemTexture, this);
                                gems.Add(gem);
                            }
                            break;
                        case 'X':
                            {
                                Rectangle rect = new Rectangle(x, y, 50, 50);
                                Block block = new Block(rect, blockTexture, this);
                                blocks.Add(block);
                                levelMap[i, j] = 1;
                            }
                            break;
                        case 'Y':
                            {
                                Rectangle rect = new Rectangle(x, y, 50, 50);
                                Block block = new Block(rect, blockTexture2, this);
                                blocks.Add(block);
                                levelMap[i, j] = 1;
                            }
                            break;
                        default:
                            {
                                Rectangle enemyRect = new Rectangle(x - 20, y - 41, 90, 90);
                                AnimatedSprite enemy = null;
                                switch (c)
                                {
                                    case '★':
                                        enemy = new Enemy_1(enemies.Count,
                                            enemyRect,
                                            enemyIdleTexture,
                                            enemyRunTexture,
                                            enemyRunTexture, 2, 9, this);
                                        break;
                                }
                                enemies.Add(enemy);
                                enemy.Run(true);
                            }
                            break;
                        case 'P':
                            {
                                hero = new Hero(new Rectangle(x - 20, y - 41, 90, 90), 3, 8.2f, 100, this);
                                Rectangle enterRect = new Rectangle(x - 50, y, 150, 150);
                                enter = new Enter(enterRect, this);
                                enter.LoadContent(Game.Content);
                                hero.LoadContent(Game.Content);
                            }
                            break;
                        case 'Q':
                            {
                                Rectangle quitRect = new Rectangle(x - 50, y, 150, 150);
                                quit = new Quit(quitRect, this);
                                quit.LoadContent(Game.Content);
                            }
                            break;
                    }
                    x += 50;
                    i++;
                }
                x = 0;
                i = 0;
                j++;
                y += 50;
            }
            //Game.Components.Add(new Preview(Game, this));
            levelState = LevelState.Active;
        }
        public override void Initialize()
        {
            this.background = new Background(this);
            base.Initialize();
            Hero.ScrollX = 0;
            if (TestGame.gameState == TestGame.GameState.Game)
            {
                levelState = LevelState.Active;
                CreateLevel(1);
            }
        }

        public static LevelState oldLevelState;
        public static LevelState levelState;

        public static int levelLength;
        public int currentLevel;
        string[] level;
        public int Score;
        public byte[,] levelMap;

        public Enter enter;
        public Quit quit;
        public Hero hero;

        public Background background;
        SoundEffect sound;
        public SpriteFont font;
        SpriteBatch spriteBatch;
        public ShaderController shader;

        protected Texture2D blockTexture2;
        protected Texture2D blockTexture;
        protected Texture2D gemTexture;
        protected Texture2D healthTexture;

        protected Texture2D gameBackground;
        protected Texture2D enemyIdleTexture;
        protected Texture2D enemyRunTexture;
        public void UpdateActiveUnits(GameTime gameTime)
        {
            if (levelState == LevelState.Paused)
            {
                if (InputManager.IsKeyPress(Keys.P))
                    levelState = LevelState.Active;
            }
            else
            {
                foreach (AnimatedSprite enemy in enemies)
                    enemy.Update(gameTime);
                enter.Update(gameTime);
                quit.Update(gameTime);
            }
            foreach (Gem gem in gems) gem.Update(gameTime);
        }
        public override void Update(GameTime gameTime)
        {
            if (levelState == LevelState.Active)
            {
                foreach (Gem gem in gems) gem.Update(gameTime);
                int i = 0;
                Rectangle boundingRect = hero.GetBoundingRect(hero.rect);

                while (i < gems.Count)
                {
                    if (gems[i].rect.Intersects(boundingRect))
                    {
                        gems.RemoveAt(i);
                        Score += 100;
                        sound.Play();
                        if (gems.Count == 0)
                        {
                            quit.quitPortalSound.Play();
                            quit.quitSound.Play();
                            quit.OpenPortal();
                        }
                    }
                    else i++;
                }
                enter.Update(gameTime);
                hero.Update(gameTime);
                quit.Update(gameTime);
                shader.Update(gameTime);
                background.Update(gameTime);

                try
                {
                    foreach (AnimatedSprite enemy in enemies)
                    {
                        Rectangle enemyBoundingRect = enemy.GetBoundingRect(enemy.rect);
                        if (enemyBoundingRect.Intersects(boundingRect))
                        {
                            hero.health -= enemy.power * (float)gameTime.ElapsedGameTime.Ticks / 1000000;
                            if (hero.health <= 0)
                            {
                                Score = 0;
                                CreateLevel(0);
                            }
                        }
                        enemy.Update(gameTime);
                    }
                }
                catch (InvalidOperationException) { }

                /// Keys Event Handler For Level
                /// Новый уровень
                if (InputManager.IsKeyPress(Keys.Space))
                    CreateLevel(2);

                /// Пауза
                if (InputManager.IsKeyPress(Keys.P))
                {
                    oldLevelState = levelState;
                    levelState = LevelState.Paused;
                }

                /// Выход в главное меню
                if (InputManager.IsKeyDown(Keys.Escape))
                {
                    oldLevelState = levelState;
                    levelState = LevelState.Paused;
                    Game.Components.Add(new Transition(Game, null, new Menu(Game)));
                }
            }
            else UpdateActiveUnits(gameTime);
        }
        public void DrawText(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(healthTexture, new Rectangle(10, 10, (int)(TestGame.Width / 5 * hero.health / hero.lives), TestGame.Height / 50),
                new Rectangle(0, 0, (int)(healthTexture.Width * hero.health / hero.lives), healthTexture.Height),
                new Color(1f - 1f * hero.health / hero.lives, 1f * hero.health / hero.lives, 0, 1f));
            spriteBatch.DrawString(font, "Your Score: " + Score, new Vector2(10, font.LineSpacing + 10), Color.White);
            spriteBatch.DrawString(font, Game.Window.Title, new Vector2(10, font.LineSpacing * 2 + 10), Color.White);
            spriteBatch.DrawString(font, "Level: " + currentLevel + " " + Help.t, new Vector2(10, font.LineSpacing * 3 + 10), Color.White);
            spriteBatch.End();
        }
        public void DrawLevel(SpriteBatch spriteBatch)
        {
            enter.Draw(spriteBatch);
            quit.Draw(spriteBatch);

            foreach (AnimatedSprite enemy in enemies)
                enemy.Draw(spriteBatch);
            foreach (Block block in blocks)
                block.Draw(spriteBatch);
            foreach (Gem gem in gems)
                gem.Draw(spriteBatch);
            if (levelState != LevelState.Overview)
                hero.Draw(spriteBatch);
        }
        public override void Draw(GameTime gameTime)
        {
            if (TestGame.gameState == TestGame.GameState.Game)
            {
                if (levelState == LevelState.Active) shader.Render();
                background.Draw(spriteBatch);
                DrawLevel(spriteBatch);
                DrawText(spriteBatch);
                if (levelState == LevelState.Active) shader.Draw(spriteBatch);
            }
        }
    }
}