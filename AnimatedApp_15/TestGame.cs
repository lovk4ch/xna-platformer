﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using AnimatedApp_15.Servants;
using AnimatedApp_15.Components.Applied_Interface;
using AnimatedApp_15.Intro;

namespace AnimatedApp_15
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TestGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static GameState gameState;
        public static int Height;
        public static int Width;

        SoundEffect music;
        float fps;
        Cursor cursor;
        SoundEffectInstance musicInstance;
        public enum GameState { Intro, Game, Menu, Edit };

        public TestGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Width = graphics.PreferredBackBufferWidth = 1600;
            Height = graphics.PreferredBackBufferHeight = 900;

            this.Components.Add(new FPSCounter(this));
            this.Components.Add(new IntroScr(this));
            graphics.IsFullScreen = false;
            IsFixedTimeStep = true;
            Content.RootDirectory = "Content";
            graphics.SynchronizeWithVerticalRetrace = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            cursor = new Cursor();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            music = Content.Load<SoundEffect>("Sound/music");
            musicInstance = music.CreateInstance();
            musicInstance.IsLooped = true;
            musicInstance.Play();
            cursor.LoadContent(Content);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            float fraps = (float)gameTime.ElapsedGameTime.Ticks / 10000000;
            fps += fraps;
            //if (fps >= 1f / 60)
            {
                cursor.Update(gameTime);
                base.Update(gameTime);
                fps -= 1f / 60;
                InputManager.Update();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here

            base.Draw(gameTime);
            cursor.Draw(spriteBatch);
        }
    }
}