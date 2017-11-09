using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace HiveTools
{
    class DatabaseSettingsClass
    {
        //static string currentTimeStamp = DateTime.Now.ToString("ddMMyyyy");
        static string currentTimeStamp = DateTime.Now.ToString("yyyy.MM.dd", CultureInfo.InvariantCulture);
        static string connectionString; // Last updated connection string value
        static string connectionStringPath; // Connection string path
        
        public static void ConnectionStringInit() // Init connection string (recreate if deleted and fill wiht default values), do nothing if exist
        {
            //string connectionString;
            bool IsFileCreated = false;
            string path = AppDomain.CurrentDomain.BaseDirectory;

            //path += @"\";   "DATA SOURCE=192.168.2.98:1521/M2CDatabase;PERSIST SECURITY INFO=True;USER ID=NTPM;PASSWORD=NeticoPassword123;";
            
                connectionStringPath = path + @"DBSettings.txt";
                if (!File.Exists(connectionStringPath))
                {
                    using (File.Create(connectionStringPath))
                    {
                        Console.WriteLine("New DBSettings file created");
                        IsFileCreated = true;
                    }
                }
                
                if (IsFileCreated)
                {
                    using (StreamWriter outputFile = new StreamWriter(connectionStringPath, true))
                    {
                        outputFile.WriteLine("DATA SOURCE=192.168.1.1:1521/M2CDatabase;PERSIST SECURITY INFO=True;USER ID=Username;PASSWORD=TypePasswordHere;");
                        outputFile.WriteLine("Only first line will be read as connection string...");
                    }
                }
        }

        public static string ReadConnectionString() // Get connection string from file
        {
            using (StreamReader outputFile = new StreamReader(connectionStringPath, true))
            {
                connectionString = outputFile.ReadLine(); // Always read first line
            }
            
            return connectionString;
        }

        public static string GetConnectionStringPath() // Get path to connection string
        {
            return connectionStringPath;
        }

        public static void UpdateConnectionString(string s) // Manualy update connection string (whole)
        {
            string wholeFIle;
            string subFIle = "";

            if (!File.Exists(connectionStringPath)) // If file doesnt exist, create it
            {
                ConnectionStringInit();
            }

            using (StreamReader outputFile = new StreamReader(connectionStringPath, true))
            {
                wholeFIle = outputFile.ReadToEnd();
            }
            File.WriteAllText(connectionStringPath, String.Empty);
            if (!wholeFIle.Equals(""))
            {
                subFIle = wholeFIle.Substring(wholeFIle.IndexOf(Environment.NewLine) + 2);
            }
            using (StreamWriter outputFile = new StreamWriter(connectionStringPath, true))
            {
                outputFile.Write(s + Environment.NewLine + subFIle);
            }
        }
    }

    
}
