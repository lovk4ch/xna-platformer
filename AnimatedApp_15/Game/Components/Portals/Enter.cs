using AnimatedApp_15.Servants.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AnimatedApp_15.Components.Portals
{
    public class Enter : ParticleController
    {
        public Rectangle rect;
        Level level;
        float dy;
        public void EngineRocket(Vector2 position) // функция, которая будет генерировать частицы
        {
            for (int a = 0; a < 1; a++)
            {
                Vector2 velocity = AngleToV2((float)(-1 + Math.PI * .1d * random.NextDouble()), 0);
                float angle = dy;
                float angleVel = -dy * 0.75f;
                Vector4 color = new Vector4(0f, 0.5f + (float)random.Next(51) / 100, 1f, 0.61f);
                float size = (float)rect.Width / (float)particleTexture.Width / 1.26f;
                int ttl = 3;
                float sizeVel = .255f;
                float alphaVel = .2f;

                GenerateNewParticle(particleTexture, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel, Particle.Type.Effect, BlendState.AlphaBlend);
            }
        }
        public Enter(Rectangle rect, Level level)
        {
            this.level = level;
            this.rect = rect;
        } // установка портала
        public void LoadContent(ContentManager Content)
        {
            particleTexture = Content.Load<Texture2D>("Textures/enter");
        }
        public override void Update(GameTime gameTime)
        {
            dy += (float)gameTime.ElapsedGameTime.Ticks / 3000000;
            base.Update(gameTime);
            EngineRocket(new Vector2(rect.X + rect.Width / 2, rect.Y));
        }
    }
}