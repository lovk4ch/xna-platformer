using System;

namespace AnimatedApp_15
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            using (TestGame game = new TestGame())
            {
                game.Run();
            }
        }
    }
#endif
}