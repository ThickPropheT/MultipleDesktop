using System;
using uApplication = System.Windows.Forms.Application;

namespace MultipleDesktop.Windows.Forms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            uApplication.EnableVisualStyles();
            uApplication.SetCompatibleTextRenderingDefault(false);
            uApplication.Run(CompositionRoot.Compose());
        }
    }
}
