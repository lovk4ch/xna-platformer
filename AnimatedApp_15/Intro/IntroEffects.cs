using AnimatedApp_15.Servants.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AnimatedApp_15.Intro
{
    public class IntroEffects : ParticleController
    {
        public struct coordinates
        {
            public coordinates(float x0, float x1, float dx, float y0, float y1, float dy)
            {
                this.xImBoard = x0;
                this.xBoard = x1;
                this.dx = dx;
                this.isRight = false;

                this.yImBoard = y0;
                this.yBoard = y1;
                this.dy = dy;
                this.isHigh = false;

                freshSpeed = 0.016f;
                sizeSpeed = 0.007f;
                this.isFlight = true;
                
            }
            public void Update(float time)
            {
                if (!isRight)
                {
                    if (dx > xBoard)
                        dx -= (dx - xImBoard) * time / 4 * freshSpeed;
                    else
                        isRight = !isRight;
                }
                else dx += (dx - xImBoard) * time / 5 * freshSpeed;

                if (!isHigh)
                {
                    if (dy < yBoard)
                        dy += (yImBoard - dy) * time / 4 * freshSpeed;
                    else
                        isHigh = !isHigh;
                }
                else
                {
                    if (isFlight && dy > 0)
                        dy -= (yImBoard - dy) * time / 3 * freshSpeed;
                    else
                        isFlight = false;

                }
                sizeSpeed += time / 50000;
            }

            public float xBoard;
            float xImBoard;
            bool isRight;
            public float dx;

            public float yBoard;
            float yImBoard;
            bool isHigh;
            public float dy;

            float freshSpeed;
            public bool isFlight;
            public float sizeSpeed;
        }
        public coordinates crd;
        int type;
        public IntroEffects(Texture2D sparkTexture, Random random)
        {
            this.particleTexture = sparkTexture;
            this.type = random.Next(3);
            crd = new coordinates(
                TestGame.Width * -0.5f,
                TestGame.Width * -0.2f,
                TestGame.Width * 2 / 3,
                TestGame.Height * 1.4f,
                TestGame.Height * 1.2f, 0);
        }
        public override void Update(GameTime gameTime)
        {
            if (crd.isFlight)
            {
                float time = (float)gameTime.ElapsedGameTime.Ticks / 100000;
                EngineSpark(time);
                crd.Update(time);
            }
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update(gameTime);
                if (particles[particle].Size <= 0 || particles[particle].TTL <= 0
                    || particles[particle].Position.X < crd.xBoard
                    || particles[particle].Position.Y > crd.yBoard)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }
        public Vector2 getVelocity(float time)
        {
            switch (type)
            {
                case 1:
                    float hyp = (float)Math.Sqrt(Math.Pow(crd.dx - crd.xBoard, 2) + Math.Pow(crd.dy - crd.yBoard, 2));
                    float route = (float)Math.Asin((crd.dx - crd.xBoard) / hyp);
                    int t = -1 + random.Next(3);
                    int s = random.Next(20);
                    if (s == 0) s = 1;
                    else s = 0;
                    return AngleToV2((float)(route + random.NextDouble() / 10 * time + Math.PI * t / 3), (float)(time * (1 + 0.3 * random.NextDouble())));
                case 2:
                    hyp = (float)Math.Sqrt(Math.Pow(crd.dx - crd.xBoard, 2) + Math.Pow(crd.dy - crd.yBoard, 2));
                    route = (float)Math.Asin((crd.dx - crd.xBoard) / hyp);
                    t = -2 + random.Next(5);
                    s = random.Next(20);
                    if (s == 0) s = 1;
                    else s = 0;
                    return AngleToV2((float)(route + random.NextDouble() / 10 * time + Math.PI * t / 5), (float)(time * (2 + 1.5 * random.NextDouble())));
                default:
                    return AngleToV2((float)(Math.PI * 2d * random.NextDouble()), time * 0.7f);
            }
        }
        public void EngineSpark(float time)
        {
            for (int a = 0; a < 10 * time; a++)
            {
                float size = (float)TestGame.Width / (float)particleTexture.Width * crd.sizeSpeed;
                float ttl = 200 / time * (float)random.NextDouble();
                Vector2 velocity = getVelocity(time);
                float sizeVel = 0;
                float alphaVel = 0;
                float angle = (float)random.NextDouble();
                Vector4 color = new Vector4(255, 255, 255, 0);
                Vector2 position = new Vector2(crd.dx, crd.dy);
                float angleVel = (float)random.NextDouble() * time / 10;

                GenerateNewParticle(particleTexture, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel, Particle.Type.Effect, BlendState.AlphaBlend);
            }
        }
    }
}