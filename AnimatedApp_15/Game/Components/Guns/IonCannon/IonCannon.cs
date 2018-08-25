using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AnimatedApp_15.Game.Components.Guns.IonCannon
{
    public class IonCannon : Gun
    {
        public IonCannon(Hero hero, float reload, int power)
        {
            this.ions = new List<Ion>();
            this.reload = reload;
            this.hero = hero;
            this.mouse = false;
            this.power = power;
        }
        SoundEffect ionSound;
        List<Ion> ions;
        int power;
        Texture2D ionTexture;
        Texture2D blastTexture;
        public override void LoadContent(ContentManager Content)
        {
            ionSound = Content.Load<SoundEffect>("Sound/izomatic");
            ionTexture = Content.Load<Texture2D>("Textures/ion");
            blastTexture = Content.Load<Texture2D>("Textures/star_spark");
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Ion ion in ions) ion.Draw(spriteBatch);
        }
        public override void Update(GameTime gameTime)
        {
            if (mouse)
            {
                if (base.UpdateMouse(gameTime))
                {
                    Ion ion = new Ion(hero.GetBoundingRect(hero.rect), base.GetAngle(), power,
                        ionTexture, blastTexture, hero.level, gameTime);
                    ions.Add(ion);
                    shotTime = 0;
                    ionSound.Play();
                }
            }
            else
            {
                if (base.UpdateKeyboard(gameTime))
                {
                    Ion ion = new Ion(hero.GetBoundingRect(hero.rect), angle, power,
                        ionTexture, blastTexture, hero.level, gameTime);
                    ions.Add(ion);
                    shotTime = 0;
                    ionSound.Play();
                }
            }
            for (int i = 0; i < ions.Count; i++)
            {
                ions[i].Update(gameTime);
                if (ions[i].particles.Count < 1)
                {
                    ions.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}