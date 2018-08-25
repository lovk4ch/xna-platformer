using AnimatedApp_15.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AnimatedApp_15.Servants.Effects
{
    public class ShaderController
    {
        RenderTarget2D renderTarget;
        distortion distortion;
        Level level;
        GraphicsDevice device;
        public List<string> effects;
        public ShaderController(GraphicsDevice device, Level level)
        {
            renderTarget = new RenderTarget2D(device, TestGame.Width, TestGame.Height);
            effects = new List<string>();
            this.device = device;
            this.level = level;
        }
        public void Initialize()
        {
            distortion = new distortion(device, renderTarget);
            distortion.LoadContent(level.Game.Content);
            effects.Add("low_health");
        }
        public void Render()
        {
            if (effects.Count > 0)
            {
                for (int i = 0; i < effects.Count; i++)
                {
                    switch (effects[i])
                    {
                        case "low_health":
                            if (level.hero.health / level.hero.lives < 0.25f)
                                device.SetRenderTarget(renderTarget); // рисуем в renderTarget
                            break;
                    }
                }
            }
        }
        public void Unrender()
        {
            effects.Remove("low_health");
        }
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                switch (effects[i])
                {
                    case "low_health":
                        if (level.hero.health / level.hero.lives < 0.25f)
                            distortion.Update(gameTime, level.hero.health, level.hero.lives);
                        break;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (effects.Count > 0)
            {
                device.SetRenderTarget(null); // рисуем в сцену

                for (int i = 0; i < effects.Count; i++)
                {
                    switch (effects[i])
                    {
                        case "low_health":
                            if (level.hero.health / level.hero.lives < 0.25f)
                                distortion.Draw(spriteBatch);
                            break;
                    }
                }
            }
        }
    }
}