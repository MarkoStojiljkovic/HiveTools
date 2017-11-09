using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace HiveTools
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                // Generate log file if application crashes
                CrashLogClass.GenerateLog(ex);
                throw;
            }
        }
    }
}
