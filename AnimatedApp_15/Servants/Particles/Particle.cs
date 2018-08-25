using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimatedApp_15.Servants.Particles
{
    public class Particle
    {
        public Texture2D Texture
            { get; set; } // Текстура нашей частички
        public Vector2 Position
            { get; set; } // Позиция частички
        public Vector2 Velocity
            { get; set; } // Скорость частички
        public float Angle
            { get; set; } // Угол поворота частички
        public float AngularVelocity
            { get; set; } // Угловая скорость
        public Vector4 Color
            { get; set; } // Цвет частички
        public float Size
            { get; set; } // Размеры
        public float SizeVel
            { get; set; } // Скорость уменьшения размера
        public float AlphaVel
            { get; set; } // Скорость уменьшения альфы
        public float TTL
            { get; set; } // Время жизни частички
        public enum Type { Effect, Damage, Subdamage };
        public Type pType
            { get; set; } // Тип частички
        public Particle(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Vector4 color, float size, float ttl, float sizeVel, float alphaVel, Type type) // конструктор
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            Color = color;
            AngularVelocity = angularVelocity;
            Size = size;
            SizeVel = sizeVel;
            AlphaVel = alphaVel;
            TTL = ttl;
            pType = type;
        }
        public void Update(GameTime gameTime) // цикл обновления
        {
            TTL--; // уменьшаем время жизни

            // Меняем параметры в соответствии с скоростями
            Angle += AngularVelocity;
            Position += Velocity;
            Size += SizeVel;
            Color = new Vector4(Color.X, Color.Y, Color.Z, Color.W - AlphaVel);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height); // область из текстуры: вся
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2); // центр
            Vector2 vScroll = new Vector2(Hero.ScrollX, 0);

            spriteBatch.Draw(Texture, Position - vScroll, sourceRectangle, new Color(Color),
                Angle, origin, Size, SpriteEffects.None, 0); // акт прорисовки
        }
    }
}