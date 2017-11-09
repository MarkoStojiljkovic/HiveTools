using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiveTools
{
    class CrashLogClass
    {
        private const string fileName = "CrashLog.xml";
        
        public static void GenerateLog(Exception e)
        {
            DateTime time = DateTime.Now;
            string message = e.Message;
            string stackTrace = e.StackTrace;
            string path = AppDomain.CurrentDomain.BaseDirectory + fileName;
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("-----------------------------------------------------------------");
                    sw.WriteLine(time.ToString("dd.MM.yyyy  HH:mm:ss", CultureInfo.InvariantCulture));
                    sw.WriteLine(message + "\r\n");
                    sw.WriteLine(stackTrace);
                    sw.WriteLine("-----------------------------------------------------------------\r\n");
                }
                return;
            }

            // If file exist, just append
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine("-----------------------------------------------------------------");
                sw.WriteLine(time.ToString("dd.MM.yyyy  HH:mm:ss", CultureInfo.InvariantCulture));
                sw.WriteLine(message + "\r\n");
                sw.WriteLine(stackTrace);
                sw.WriteLine("-----------------------------------------------------------------\r\n");
            }


        }
    }
}
