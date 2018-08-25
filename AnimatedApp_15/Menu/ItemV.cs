using AnimatedApp_15.Servants.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimatedApp_15.MenuSystem
{
    class ItemV : ParticleController
    {
        Texture2D sparkTexture;
        float allTime;
        public void Update(GameTime gameTime, string name, SpriteFont font, float scale, float Y)
        {
            float time = (float)gameTime.ElapsedGameTime.Ticks / 100000;
            Vector2 map = font.MeasureString(name) * scale;

            EngineItem(time, map, Y);
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update(gameTime);
                if (particles[particle].Size <= 0
                    || particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }
        public ItemV(Texture2D sparkTexture)
        {
            this.sparkTexture = sparkTexture;
        }
        public void EngineItem(float time, Vector2 map, float y)
        {
            int t = (int)(allTime / 10);
            allTime += time;
            if ((int)(allTime / 10) > t)
            for (int a = 0; a < 7; a++)
            {
                Vector2 velocity = AngleToV2((float)(Math.PI * 2 * random.NextDouble()), 0);
                float size = (float)TestGame.Width / (float)sparkTexture.Width / 50;
                Vector4 color = new Vector4(1f, 1f, 1f, 1f);
                float ttl = 50f / time;
                float sizeVel = 0;
                float alphaVel = time / 30;
                float angleVel = time / 20;
                float k = (float)TestGame.Width / 1600 * map.X;
                Vector2 position = new Vector2((float)((TestGame.Width - k) / 2 + random.NextDouble() * k),
                    y + map.Y * (float)random.NextDouble() * 0.75f);

                GenerateNewParticle(sparkTexture, position, velocity, 0, angleVel, color, size, ttl, sizeVel, alphaVel, Particle.Type.Effect, BlendState.Additive);
            }
        }
    }
}