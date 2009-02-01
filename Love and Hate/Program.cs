using System;

namespace Love_and_Hate
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        static Game1 mGame;

        static void Main(string[] args)
        {
            using (mGame = new Game1())
            {
                mGame.Run();
            }
        }

        public static Game1 Instance
        {
            get { return mGame; }
        }

    }
}

