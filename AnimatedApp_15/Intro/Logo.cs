using AnimatedApp_15.Servants.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AnimatedApp_15.Intro
{
    class Logo : ParticleController
    {
        enum LogoState { One, Two, Three}
        LogoState state;
        float allTime;
        Texture2D sparkTexture;
        public Logo(Texture2D texture, Texture2D sparkTexture)
        {
            this.sparkTexture = sparkTexture;
            this.particleTexture = texture;
            this.state = LogoState.One;
        }
        public override void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.Ticks / 100000;
            switch (state)
            {
                case LogoState.One:
                    if (allTime > 100) state = LogoState.Two;
                    break;
            }
            EngineLogo(time);
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
        public void EngineLogo(float time)
        {
            for (int a = 0; a < 1; a++)
            {
                Vector4 color = new Vector4(1f, 1f, (float)random.NextDouble(), 1);
                float size = (float)TestGame.Width / 1600;
                Vector2 velocity = Vector2.Zero;
                float angleVel = 0;
                float angle = 0;
                int ttl = 2;
                float sizeVel = 0;
                float alphaVel = 0;
                if (allTime < 100)
                    size = (float)Math.Pow(allTime / 100, 2) * TestGame.Width / 1600;
                Vector2 position = new Vector2(TestGame.Width / 2, TestGame.Height / 5);

                GenerateNewParticle(particleTexture, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel, Particle.Type.Effect, BlendState.Additive);
            }
            switch (state)
            {
                case LogoState.One:
                    allTime += time;
                    break;
                case LogoState.Two:
                {
                    state = LogoState.Three;
                    for (int a = 1; a < 6; a++)
                    {
                        float size = (float)TestGame.Width / 1600;
                        Vector2 velocity = Vector2.Zero;
                        float ttl = 500f / time;
                        float sizeVel = time / 300 * a;
                        float alphaVel = time / 300 * (7 - a);
                        Vector4 color = new Vector4(1f, 1f, 1f, 1f);
                        Vector2 position = new Vector2(TestGame.Width / 2, TestGame.Height / 5);

                        GenerateNewParticle(particleTexture, position, velocity, 0, 0, color, size, ttl, sizeVel, alphaVel, Particle.Type.Effect, BlendState.Additive);
                    }
                    allTime += time;
                    break;
                }
                case LogoState.Three:
                {
                    int t = (int)(allTime / 10);
                    allTime += time;
                    if ((int)(allTime / 10) > t)
                    for (int a = 0; a < 5; a++)
                    {
                        Vector2 velocity = AngleToV2((float)-Math.PI / 2, (float)random.NextDouble() * 9 * time);
                        float size = (float)TestGame.Width / (float)sparkTexture.Width / 100;
                        Vector4 color = new Vector4(1f, 1f, 1f, 1f);
                        float ttl = 50f / time;
                        float sizeVel = 0;
                        float alphaVel = 0;
                        float k = (float)TestGame.Width / 1600 * particleTexture.Width * 0.95f;
                        Vector2 position = new Vector2((float)((TestGame.Width - k) / 2 + random.NextDouble() * k), TestGame.Height * 0.2f);

                        GenerateNewParticle(sparkTexture, position, velocity, 0, 0, color, size, ttl, sizeVel, alphaVel, Particle.Type.Effect, BlendState.Additive);
                    }
                    break;
                }
            }
        }
    }
}