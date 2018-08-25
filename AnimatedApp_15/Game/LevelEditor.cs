using AnimatedApp_15.Components.Enemies;
using AnimatedApp_15.Servants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;
using System.Collections.Generic;
using AnimatedApp_15.Components.Portals;
using AnimatedApp_15.Game.Components;
using AnimatedApp_15.MenuSystem;
using AnimatedApp_15.Game.Change;

namespace AnimatedApp_15.Components
{
    class Editor : Level
    {
        public override void Initialize()
        {
            base.Initialize();
        }
        List<List<char>> level;
        int blockNum;

        Texture2D enterTexture;
        Texture2D quitTexture;
        Texture2D saveTexture;
        Texture2D loadTexture;
        Texture2D arrowTexture;

        SpriteBatch spriteBatch;
        Rectangle panel;
        Rectangle save;
        Rectangle load;
        Rectangle upArrow;
        Rectangle downArrow;
        public Editor(Microsoft.Xna.Framework.Game game) : base(game)
        {
            level = new List<List<char>>();
            for (int i = 0; i < TestGame.Height / 50; i++)
            {
                level.Add(new List<char>());
                for (int j = 0; j < TestGame.Width / 50; j++)
                {
                    level[i].Add('0');
                }
            }

            enemies = new List<AnimatedSprite>();
            blocks = new List<Block>();
            gems = new List<Gem>();
            this.blockNum = 1;
            levelLength = TestGame.Width;
            this.background = new Background(this);

            panel = new Rectangle(TestGame.Width - 100, (TestGame.Height - 340) / 2, 100, 340);
            downArrow = new Rectangle(panel.X + 5, panel.Y + 125, 90, 20);
            upArrow = new Rectangle(panel.X + 5, panel.Y + 5, 90, 20);
            save = new Rectangle(panel.X + 5, panel.Y + 150, 90, 90);
            load = new Rectangle(panel.X + 5, panel.Y + 245, 90, 90);
        }
        public override void Update(GameTime gameTime)
        {
            if (TestGame.gameState == TestGame.GameState.Edit)
            {
                /// Keys Event Handler
                if (InputManager.IsKeyDown(Keys.Left))
                {
                    if (levelLength > TestGame.Width + gameTime.ElapsedGameTime.Milliseconds)
                        levelLength -= gameTime.ElapsedGameTime.Milliseconds;
                    Hero.Scroll(-gameTime.ElapsedGameTime.Milliseconds);
                }
                if (InputManager.IsKeyDown(Keys.Right))
                {
                    if (levelLength < 25000)
                        levelLength += gameTime.ElapsedGameTime.Milliseconds;
                    Hero.Scroll(gameTime.ElapsedGameTime.Milliseconds);
                }

                if (InputManager.IsKeyDown(Keys.Escape))
                    Game.Components.Add(new Transition(Game, this, new Menu(Game)));

                if (Mouse.GetState().X > 0 && Mouse.GetState().X < TestGame.Width && Mouse.GetState().Y > 0 && Mouse.GetState().Y < TestGame.Height)
                {
                    Point target = new Point((Mouse.GetState().X + (int)Hero.ScrollX) / 50 * 50, Mouse.GetState().Y / 50 * 50);
                    if (InputManager.IsMouseLeftDown()) setObject(target);
                    else
                        if (InputManager.IsMouseRightDown()) removeObject(target);
                }

                foreach (Gem gem in gems) gem.Update(gameTime);
                if (enter != null) enter.Update(gameTime);
                if (quit != null) quit.Update(gameTime);
            }
        }
        private void removeObject(Point target)
        {
            if (!panel.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                if (target.X / 50 - level[0].Count < 0 && level[target.Y / 50][target.X / 50] != 0)
                {
                    switch (level[target.Y / 50][target.X / 50])
                    {
                        case 'X':
                            foreach (Block block in blocks)
                                if (block.rect.X == target.X
                                    && block.rect.Y == target.Y)
                                {
                                    blocks.Remove(block);
                                    break;
                                }
                            level[target.Y / 50][target.X / 50] = '0';
                            break;
                        case 'Y':
                            foreach (Block block in blocks)
                                if (block.rect.X == target.X
                                    && block.rect.Y == target.Y)
                                {
                                    blocks.Remove(block);
                                    break;
                                }
                            level[target.Y / 50][target.X / 50] = '0';
                            break;
                        case 'G':
                            foreach (Gem gem in gems)
                                if (gem.rect.X - 10 == target.X
                                    && gem.rect.Y - 10 == target.Y)
                                {
                                    gems.Remove(gem);
                                    break;
                                }
                            level[target.Y / 50][target.X / 50] = '0';
                            break;
                        case 'P':
                            enter = null;
                            level[target.Y / 50][target.X / 50] = '0';
                            break;
                        case 'Q':
                            quit = null;
                            level[target.Y / 50][target.X / 50] = '0';
                            break;
                        case '★':
                            foreach (AnimatedSprite enemy in enemies)
                                if (enemy.rect.X == target.X - 20
                                    && enemy.rect.Y + 41 == target.Y)
                                {
                                    enemies.Remove(enemy);
                                    break;
                                }
                            level[target.Y / 50][target.X / 50] = '0';
                            break;
                    }
                }
            }
        }
        private void setObject(Point target)
        {
            Rectangle rect = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1);
            if (rect.Intersects(panel))
            {
                if (InputManager.IsMouseLeftClick())
                {
                    if (rect.Intersects(upArrow))
                    {
                        if (blockNum-- == 1)
                            blockNum = 6;
                    }
                    else
                    if (rect.Intersects(downArrow))
                    {
                        if (blockNum++ == 6)
                            blockNum = 1;
                    }
                    else
                    if (rect.Intersects(save))
                    {
                        saveTheCurrentLevel();
                    }
                    else
                    if (rect.Intersects(load))
                    {
                        loadTheCurrentLevel();
                    }
                }
            }
            else
            {
                int shift = target.X / 50 - level[0].Count;
                while (shift-- > -1)
                    foreach (List<char> list in level)
                        list.Add('0');

                if (level[target.Y / 50][target.X / 50] == '0')
                {
                    Rectangle block;
                    switch (blockNum)
                    {
                        case 1:
                            block = new Rectangle(target.X, target.Y, 50, 50);
                            Block brick = new Block(block, blockTexture, this);
                            blocks.Add(brick);
                            level[target.Y / 50][target.X / 50] = 'X';
                            break;
                        case 2:
                            block = new Rectangle(target.X, target.Y, 50, 50);
                            brick = new Block(block, blockTexture2, this);
                            blocks.Add(brick);
                            level[target.Y / 50][target.X / 50] = 'Y';
                            break;
                        case 3:
                            block = new Rectangle(target.X + 10, target.Y + 10, 30, 30);
                            Gem gem = new Gem(block, gemTexture, this);
                            gems.Add(gem);
                            level[target.Y / 50][target.X / 50] = 'G';
                            break;
                        case 4:
                            if (enter == null)
                            {
                                block = new Rectangle(target.X - 50, target.Y, 150, 150);
                                enter = new Enter(block, this);
                                enter.LoadContent(Game.Content);
                                level[target.Y / 50][target.X / 50] = 'P';
                            }
                            break;
                        case 5:
                            if (quit == null)
                            {
                                block = new Rectangle(target.X - 50, target.Y, 150, 150);
                                quit = new Quit(block, this);
                                quit.LoadContent(Game.Content);
                                level[target.Y / 50][target.X / 50] = 'Q';
                            }
                            break;
                        case 6:
                            block = new Rectangle(target.X - 20, target.Y - 41, 90, 90);
                            AnimatedSprite enemy = new Enemy_1(enemies.Count,
                                block,
                                enemyIdleTexture,
                                enemyRunTexture,
                                enemyRunTexture, 2, 9, this);
                            enemies.Add(enemy);
                            level[target.Y / 50][target.X / 50] = '★';
                            break;
                    }
                }
            }
        }
        private void saveTheCurrentLevel()
        {
            bool empty = true;
            while (empty)
            {
                if (level[0].Count > 32)
                {
                    for (int i = 0; i < level.Count; i++)
                    {
                        if (level[i][level[i].Count - 1] != '0')
                        {
                            empty = false;
                            break;
                        }
                    }
                    if (empty)
                        for (int i = 0; i < level.Count; i++)
                            level[i].RemoveAt(level[i].Count - 1);
                }
                else empty = false;
            }
            System.Windows.Forms.SaveFileDialog save = new System.Windows.Forms.SaveFileDialog();
            save.Filter = "Level Files (*.lvl)|*.lvl";
            save.InitialDirectory = Environment.CurrentDirectory + "\\Content\\Levels";
            if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamWriter f = new StreamWriter(save.FileName);
                for (int i = 0; i < level.Count; i++)
                {
                    for (int j = 0; j < level[i].Count; j++)
                    {
                        f.Write(level[i][j]);
                    }
                    f.WriteLine();
                }
                f.Close();
            }
        }
        private void loadTheCurrentLevel()
        {
            System.Windows.Forms.OpenFileDialog open = new System.Windows.Forms.OpenFileDialog();
            open.Filter = "Level Files (*.lvl)|*.lvl";
            open.InitialDirectory = Environment.CurrentDirectory + "\\Content\\Levels";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] structure = File.ReadAllLines(open.FileName);
                enemies.Clear();
                blocks.Clear();
                gems.Clear();
                Hero.ScrollX = 0;
                for (int i = 0; i < structure.Length; i++)
                {
                    for (int j = 0; j < structure[i].Length; j++)
                    {
                        if (j > level[i].Count - 1) level[i].Add(structure[i][j]);
                        else level[i][j] = structure[i][j];
                        Rectangle block;
                        switch (level[i][j])
                        {
                            case 'X':
                                {
                                    block = new Rectangle(j * 50, i * 50, 50, 50);
                                    Block brick = new Block(block, blockTexture, this);
                                    blocks.Add(brick);
                                }
                                break;
                            case 'Y':
                                {
                                    block = new Rectangle(j * 50, i * 50, 50, 50);
                                    Block brick = new Block(block, blockTexture2, this);
                                    blocks.Add(brick);
                                }
                                break;
                            case 'G':
                                {
                                    block = new Rectangle(j * 50 + 10, i * 50 + 10, 30, 30);
                                    Gem gem = new Gem(block, gemTexture, this);
                                    gems.Add(gem);
                                }
                                break;
                            case 'P':
                                {
                                    if (enter == null)
                                    {
                                        block = new Rectangle(j * 50 - 50, i * 50, 150, 150);
                                        enter = new Enter(block, this);
                                        enter.LoadContent(Game.Content);
                                    }
                                }
                                break;
                            case 'Q':
                                {
                                    if (quit == null)
                                    {
                                        block = new Rectangle(j * 50 - 50, i * 50, 150, 150);
                                        quit = new Quit(block, this);
                                        quit.LoadContent(Game.Content);
                                    }
                                }
                                break;
                            case '★':
                                {
                                    block = new Rectangle(j * 50 - 20, i * 50 - 41, 90, 90);
                                    AnimatedSprite enemy = new Enemy_1(enemies.Count,
                                        block,
                                        enemyIdleTexture,
                                        enemyRunTexture,
                                        enemyRunTexture, 2, 9, this);
                                    enemies.Add(enemy);
                                }
                                break;
                        }
                    }
                }
            }
        }
        protected override void LoadContent()
        {
            base.LoadContent();
            arrowTexture = Game.Content.Load<Texture2D>("Textures/arrow");
            enterTexture = Game.Content.Load<Texture2D>("Textures/quit");
            quitTexture = Game.Content.Load<Texture2D>("Textures/enter");
            saveTexture = Game.Content.Load<Texture2D>("Textures/save");
            loadTexture = Game.Content.Load<Texture2D>("Textures/load");
            background.LoadContent(Game.Content);
            hero = new Hero(Rectangle.Empty, 0, 0, 1, this);
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        public override void Draw(GameTime gameTime)
        {
            if (TestGame.gameState == TestGame.GameState.Edit)
            {
                background.Draw(spriteBatch);
                if (enter != null)
                    enter.Draw(spriteBatch);
                if (quit != null)
                    quit.Draw(spriteBatch);
                foreach (AnimatedSprite enemy in enemies) enemy.Draw(spriteBatch);
                foreach (Block block in blocks) block.Draw(spriteBatch);
                foreach (Gem gem in gems) gem.Draw(spriteBatch);

                spriteBatch.Begin();
                switch (blockNum)
                {
                    case 1:
                        spriteBatch.Draw(blockTexture, new Rectangle(panel.X + 5, panel.Y + 30, 90, 90), Color.White);
                        break;
                    case 2:
                        spriteBatch.Draw(blockTexture2, new Rectangle(panel.X + 5, panel.Y + 30, 90, 90), Color.White);
                        break;
                    case 3:
                        spriteBatch.Draw(gemTexture, new Rectangle(panel.X + 5, panel.Y + 30, 90, 90), Color.White);
                        break;
                    case 4:
                        spriteBatch.Draw(enterTexture, new Rectangle(panel.X + 5, panel.Y + 30, 90, 90), Color.White);
                        break;
                    case 5:
                        spriteBatch.Draw(quitTexture, new Rectangle(panel.X + 5, panel.Y + 30, 90, 90), Color.White);
                        break;
                    case 6:
                        spriteBatch.Draw(enemyIdleTexture, new Rectangle(panel.X + 5, panel.Y + 30, 90, 90), Color.White);
                        break;
                }
                spriteBatch.Draw(arrowTexture, upArrow, Color.White);
                spriteBatch.Draw(saveTexture, save, Color.White);
                spriteBatch.Draw(loadTexture, load, Color.White);
                spriteBatch.Draw(arrowTexture, downArrow, null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipVertically, 0);
                spriteBatch.End();
            }
        }
    }
}