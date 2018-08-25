using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace AnimatedApp_15.Servants
{
    class Help
    {
        static System.Diagnostics.Stopwatch stop = new System.Diagnostics.Stopwatch();
        static float ticks;
        public static string s;
        public static int t = 0;
        public static void Start()
        {
            stop.Restart();
        }
        public static string End(GameTime gameTime)
        {
            ticks += (float)gameTime.ElapsedGameTime.Ticks / 2000000;
            stop.Stop();
            if (ticks > 1)
            {
                s = stop.ElapsedTicks.ToString();
                ticks = 0;
            }
            return s;
        }
        public static void DrawCentered(String text, String part, int x, int width, int y, Color color, float scale,
            SpriteFont font, SpriteBatch spriteBatch)
        {
            Vector2 textSize = font.MeasureString(text) * scale;
            int centerX = x + ((width - (int)textSize.X) / 2);
            if (part != null) text = part;

            // Draw the centered text.
            spriteBatch.DrawString(font, text, new Vector2(centerX, y), color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
        public static sbyte Sign(bool value)
        {
            if (value) return 1;
            else return -1;
        }
    }
}