using AnimatedApp_15.Servants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AnimatedApp_15.Game.Change
{
    public class Transition : DrawableGameComponent
    {
        enum TransitionState { Old, New };
        TransitionState state;
        List<IGameComponent> a;
        List<IGameComponent> b;

        Texture2D texture;
        float timeToEnd;
        SpriteBatch spriteBatch;
        public Transition(Microsoft.Xna.Framework.Game game, List<IGameComponent> a, List<IGameComponent> b)
            : base(game)
        {
            this.a = new List<IGameComponent>();
            foreach (GameComponent c in a) this.a.Add(c);
            this.b = new List<IGameComponent>();
            foreach (GameComponent c in b) this.b.Add(c);

            this.DrawOrder = 1;
            InputManager.isActive = false;
        }
        public Transition(Microsoft.Xna.Framework.Game game, IGameComponent a, IGameComponent b)
            : base(game)
        {
            this.a = new List<IGameComponent>();
            this.a.Add(a);
            this.b = new List<IGameComponent>();
            this.b.Add(b);

            this.DrawOrder = 1;
            InputManager.isActive = false;
        }
        protected override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("Textures/void");
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        public override void Update(GameTime gameTime)
        {
            timeToEnd += (float)gameTime.ElapsedGameTime.Ticks / 10000000;
            if (state == TransitionState.Old)
            {
                if (timeToEnd > 0.5f)
                {
                    foreach (GameComponent c in a)
                        Game.Components.Remove(c);
                    foreach (GameComponent c in b)
                        if (c != null)
                            Game.Components.Add(c);
                    state = TransitionState.New;
                }
            }
            else
            {
                if (timeToEnd > 1)
                {
                    InputManager.isActive = true;
                    Game.Components.Remove(this);
                }
            }
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            float alpha;
            if (timeToEnd < 0.5f) alpha = timeToEnd * 2;
            else
            {
                if (timeToEnd > 0.5f) alpha = 1 - (timeToEnd - 0.5f) * 2;
                else alpha = 1;
            }
            spriteBatch.Draw(texture, new Rectangle(0, 0, TestGame.Width, TestGame.Height), new Color(new Vector4(alpha)));
            spriteBatch.End();
        }
    }
}