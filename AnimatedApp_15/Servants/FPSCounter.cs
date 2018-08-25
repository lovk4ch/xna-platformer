using Microsoft.Xna.Framework;

namespace AnimatedApp_15
{
    class FPSCounter : DrawableGameComponent
    {
        public int FPS;
        int frames;
        double seconds;
        public FPSCounter(TestGame game) : base(game)
        {

        }
        public override void Draw(GameTime gameTime)
        {
            frames++;
            base.Draw(gameTime);
        }
        public override void Update(GameTime gameTime)
        {
            seconds += gameTime.ElapsedGameTime.TotalSeconds;
            if (seconds >= 1)
            {
                FPS = frames;
                seconds = 0;
                frames = 0;
                Game.Window.Title = "fps: " + FPS.ToString();
            }
            base.Update(gameTime);
        }
    }
}