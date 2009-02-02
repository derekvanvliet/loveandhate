using System;
using System.Windows;  

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
            try
            {
                using (mGame = new Game1())
                {
                    System.Windows.Forms.MessageBox.Show("hi2");

                    mGame.Run();
                }
            }
            catch (Exception e)
            {
                ReportException(e);
            }
        }

        public static Game1 Instance
        {
            get { return mGame; }
        }

        static void ReportException(Exception e)
        {
            System.Windows.Forms.MessageBox.Show(
                "Message: " + e.Message + "\n\n"
                + "Source: " + e.Source + "\n\n"
                + "Stack Trace: " + e.StackTrace + "\n\n"
                + "Help Link: " + e.HelpLink + "\n\n"
                + "Target Site Name: " + e.TargetSite.Name + "\n\n"
                + "Exception: " + e.ToString()
                );

            if (e.InnerException != null)
            {
                ReportException(e.InnerException);
            }
        }

    }
}

