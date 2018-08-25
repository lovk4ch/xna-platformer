using AnimatedApp_15.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace AnimatedApp_15.Servants.Particles
{
    public class ParticleController
    {
        protected Particle GenerateNewParticle(Texture2D texture, Vector2 position, Vector2 velocity, float angle,
            float angularVelocity, Vector4 color, float size, float ttl, float sizeVel, float alphaVel, Particle.Type type, BlendState state) // генерация новой частицы
        {
            Particle particle = new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl, sizeVel, alphaVel, type);
            particles.Add(particle);
            this.state = state;
            return particle;
        }

        protected Texture2D particleTexture; // текстура
        BlendState state;
        public List<Particle> particles;
        protected Random random;
        public ParticleController()
        {
            this.particles = new List<Particle>();
            random = new Random();
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, state); // ставим режим смешивания Addictive

            for (int index = 0; index < particles.Count; index++) // рисуем все частицы
            {
                particles[index].Draw(spriteBatch);
            }

            spriteBatch.End();
        }
        public Vector2 AngleToV2(float angle, float length)
        {
            Vector2 direction = Vector2.Zero;
            direction.X = (float)Math.Cos(angle) * length;
            direction.Y = -(float)Math.Sin(angle) * length;
            return direction;
        }
        public virtual void Update(GameTime gameTime)
        {
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update(gameTime);
                if (particles[particle].Size <= 0 || particles[particle].TTL <= 0
                    || particles[particle].Position.X > Level.levelLength
                    || particles[particle].Position.Y > TestGame.Height
                    || particles[particle].Position.X < 0
                    || particles[particle].Position.Y < 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }
    }
}