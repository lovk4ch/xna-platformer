using AnimatedApp_15.Servants;
using AnimatedApp_15.Servants.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace AnimatedApp_15.Components.Applied_Interface
{
    class Cursor : ParticleController
    {
        public void EngineRocket(Vector2 position) // функция, которая будет генерировать частицы
        {
            for (int a = 0; a < 1; a++)
            {
                Vector2 velocity = AngleToV2((float)(-1 + Math.PI * .1d * random.NextDouble()), 0);
                float angle = 0;
                float angleVel = 0;
                Vector4 color = new Vector4(1f, 1f, 1f, 1f);
                float size = .4f;
                int ttl = 0;
                float sizeVel = 0;
                float alphaVel = 0;

                GenerateNewParticle(particleTexture, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel, Particle.Type.Effect, BlendState.Additive);
            }
        }
        public void LoadContent(ContentManager Content)
        {
            particleTexture = Content.Load<Texture2D>("Textures/cursor");
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (TestGame.gameState == TestGame.GameState.Game)
                EngineRocket(InputManager.GetMousePosition() + new Vector2(Hero.ScrollX, 0));
            else EngineRocket(InputManager.GetMousePosition() + new Vector2(particleTexture.Width * .2f + Hero.ScrollX, particleTexture.Height * .2f));
        }
    }
}