using AnimatedApp_15.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AnimatedApp_15.Servants
{
    class distortion
    {
        RenderTarget2D renderTarget;
        bool isActive;
        Effect effect;
        float timer;
        float drand;
        Random random;
        GraphicsDevice device;
        public distortion(GraphicsDevice device, RenderTarget2D renderTarget)
        {
            this.random = new Random();
            this.timer = 1;
            this.device = device;
            this.renderTarget = renderTarget;
        }
        public void Update(GameTime gameTime, float a, float b)
        {
            if (a / b < 0.25f)
            {
                effect.Parameters["initialization"].SetValue(-0.005f + (float)random.NextDouble() / 100);
                effect.Parameters["random1"].SetValue((float)random.NextDouble() * (1f - a / b * 4));
                effect.Parameters["force"].SetValue(1f - a / b * 4);
                effect.Parameters["timer"].SetValue(timer);
                effect.Parameters["random2"].SetValue((float)random.NextDouble() * (1f - a / b * 4));
                effect.Parameters["desaturation_float"].SetValue((float)(timer * random.NextDouble()
                    * (1f - a / b * 4) / drand));
                timer += (float)gameTime.ElapsedGameTime.Ticks / 100000;
                if (timer > drand)
                {
                    if (isActive = !isActive) drand = 50 + random.Next(150);
                    else drand = 200 + random.Next(600);
                    timer = 0;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.None,
                RasterizerState.CullCounterClockwise, effect);
            spriteBatch.Draw(renderTarget, new Rectangle(0, 0, TestGame.Width, TestGame.Height), Color.White);
            spriteBatch.End();
        }
        public void LoadContent(ContentManager Content)
        {
            effect = Content.Load<Effect>("Effects/distortion");
        }
    }
}