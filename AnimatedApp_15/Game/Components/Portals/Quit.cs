using AnimatedApp_15.Game.Conditions;
using AnimatedApp_15.Servants.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AnimatedApp_15.Components.Portals
{
    public class Quit : ParticleController
    {
        public Rectangle rect;
        enum PortalState { Closed, Prepared, Opened, Entrance };
        PortalState state;
        Level level;
        float dy;
        float R;
        float G;
        float additive;
        Texture2D sparkTexture;
        public SoundEffect quitSound;
        public SoundEffect quitPortalSound;
        public void EngineRocket(Vector2 position)
        {
            for (int a = 0; a < 1; a++)
            {
                Vector2 velocity = AngleToV2((float)(-1 + Math.PI * .1d * random.NextDouble()), 0);
                float angle = dy;
                float angleVel = -dy * 0.75f;
                Vector4 color = new Vector4(R, G, 0, 0.61f);
                float size = (float)rect.Width / (float)particleTexture.Width / 1.26f;
                int ttl = 3;
                float sizeVel = .255f;
                float alphaVel = .2f;

                GenerateNewParticle(particleTexture, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel, Particle.Type.Effect, BlendState.AlphaBlend);
            }
        } // функция, которая будет генерировать частицы
        public void EngineSpark(float rotate)
        {
            for (int a = 0; a < 5 * rotate; a++)
            {
                Vector2 velocity = AngleToV2((float)(dy / rotate + Math.PI * a / 5f), (float)((random.Next(35) + 15)) * .1f * rotate);
                float angle = (float)random.NextDouble();
                float angleVel = (float)(Math.PI / 100);
                Vector4 color = new Vector4(R, G, 0, 0f);
                float size = (float)rect.Width / (float)sparkTexture.Width / 10;
                float ttl = 700 / rotate;
                float sizeVel = 0;
                float alphaVel = 0;
                Vector2 position = new Vector2(rect.X + rect.Width / 2, rect.Y);

                GenerateNewParticle(sparkTexture, position, velocity, angle, angleVel, color, size, ttl, sizeVel, alphaVel, Particle.Type.Effect, BlendState.AlphaBlend);
            }
        } // доп. эффект открытия портала
        public Quit(Rectangle rect, Level level)
        {
            this.state = PortalState.Closed;
            this.G = 0;
            this.level = level;
            this.rect = rect;
        }  // установка портала
        public void OpenPortal()
        {
            state = PortalState.Prepared;
            R = 1;
        } // открытие портала после сбора
        public void LoadContent(ContentManager Content)
        {
            particleTexture = Content.Load<Texture2D>("Textures/quit");
            sparkTexture = Content.Load<Texture2D>("Textures/spark");
            quitSound = Content.Load<SoundEffect>("Sound/quit");
            quitPortalSound = Content.Load<SoundEffect>("Sound/quit_portal");
        }
        public override void Update(GameTime gameTime)
        {
            switch (state)
            {
                case PortalState.Opened:
                    if (rect.Intersects(level.hero.rect))
                    {
                        int distance = rect.X + rect.Width / 2 - level.hero.rect.X - level.hero.rect.Width / 2;
                        if (distance == 0 && rect.Bottom - level.hero.rect.Bottom - 1 == 100)
                        {
                            level.Game.Components.Add(new Completed(level.Game, level));
                            this.state = PortalState.Entrance;
                        }
                    }
                    additive = (float)random.Next(51) / 100;
                    G = 1 - additive;
                    break;
                case PortalState.Prepared:
                    if (R > 0)
                    {
                        float K = (float)gameTime.ElapsedGameTime.Ticks / 40000000;
                        R -= K;
                        G += K;
                        EngineSpark(500 * K);
                    }
                    else
                    {
                        state = PortalState.Opened;
                        R = 0;
                    }
                    break;
                case PortalState.Closed:
                    additive = (float)random.Next(51) / 100;
                    R = 1 - additive;
                    break;
                case PortalState.Entrance:
                    additive = (float)random.Next(51) / 100;
                    G = 1 - additive;
                    break;
            }
            dy += (float)gameTime.ElapsedGameTime.Ticks / 3000000;
            base.Update(gameTime);
            EngineRocket(new Vector2(rect.X + rect.Width / 2, rect.Y));
        }
    }
}