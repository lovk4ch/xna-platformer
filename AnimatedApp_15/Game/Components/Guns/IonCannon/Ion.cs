using AnimatedApp_15.Components;
using AnimatedApp_15.Servants.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AnimatedApp_15.Game.Components.Guns.IonCannon
{
    public class Ion : ParticleController
    {
        public Rectangle rect;
        Texture2D blastTexture;
        Level level;
        float R;
        float G;
        float B;
        bool blast;
        float course;
        public float energy;
        public Ion(Rectangle rect, float course, float energy, Texture2D shot, Texture2D blast, Level level, GameTime gameTime)
        {
            this.particleTexture = shot;
            this.course = course;
            this.level = level;
            this.R = 255;
            this.G = 255;
            this.B = 255;
            this.rect = rect;
            this.energy = energy;
            this.blastTexture = blast;
            EngineRocket(new Vector2(rect.Left + rect.Width / 2, rect.Y + rect.Height / 3),
                (float)gameTime.ElapsedGameTime.Ticks / 100000);
        }
        public void EngineBlast(Vector2 position, float time, int power)
        {
            for (int a = 0; a < power; a++)
            {
                float size = (float)rect.Width / (float)blastTexture.Width / 50 * energy;
                float target = (float)(Math.PI * (2d * a / energy));
                Vector4 color = new Vector4(255, 255, 255, 0);
                float alphaVel = 0;
                float sizeVel = 0;
                float ttl = 70 / time * (float)random.NextDouble();
                float angle = -target; //+ (float)Math.PI / 4;
                float angleVel = (float)random.NextDouble() * time / 10;
                Vector2 velocity = AngleToV2(target, 2 * time * (float)random.NextDouble());

                GenerateNewParticle(blastTexture, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel, Particle.Type.Subdamage, BlendState.AlphaBlend);
            }
        }
        public void EngineRocket(Vector2 position, float time)
        {
            for (int a = 0; a < energy; a++)
            {
                Vector4 color = new Vector4(R, G, B, 0);
                float angle = course;
                float size = 0.005f;
                float angleVel = 0;
                float ttl = ((2 * energy) - a) / time * 5 / 3;
                float sizeVel = 3 * a / energy * time / 500;
                float alphaVel = 0;
                Vector2 velocity = AngleToV2(course, time * (7 + 4 * a / energy));

                GenerateNewParticle(particleTexture, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel, Particle.Type.Damage, BlendState.AlphaBlend);
            }
        }
        public override void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.Ticks / 100000;
            R = (float)random.NextDouble();
            G = (float)random.NextDouble();
            //B = (float)random.NextDouble();

            for (int particle = 0; particle < particles.Count; particle++)
            {
                Point p = new Point((int)particles[particle].Position.X, (int)particles[particle].Position.Y);
                particles[particle].Color = new Vector4(R, G, B, 0);
                for (int i = 0; i < level.enemies.Count; i++)
                {
                    if (level.enemies[i].GetBoundingRect(level.enemies[i].rect).Contains(p))
                    {
                        particles.RemoveAt(particle);
                        if (particle > 0) particle--;
                        level.enemies[i].power--;
                    }
                }
                for (int i = 0; i < level.blocks.Count; i++)
                {
                    if (level.blocks[i].rect.Contains(p)
                        && (particles[particle].pType == Particle.Type.Damage))
                    {
                        particles.RemoveAt(particle);
                        if (particle > 0) particle--;
                        if (!blast && particles.Count > 0)
                        {
                            EngineBlast(particles[particle].Position, time, particles.Count);
                            blast = true;
                        }
                    }
                }
            }
            base.Update(gameTime);
        }
    }
}