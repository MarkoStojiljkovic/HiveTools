using HiveTools.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace HiveTools
{
    class ConfigClass
    {
        static string path = AppDomain.CurrentDomain.BaseDirectory; // Path to current directory
        // Default color palete
        public static ColorPalete colors = new ColorPalete();
        // Default delta values
        public static ConfigDeltaValues delta = new ConfigDeltaValues();
        public static ConfigOther other = new ConfigOther();

        public const int MAX_ROWS_GET = 100000; // Max rows that will be read from database

        public const float DEFAULT_VALUE_FOR_NULL_FLOAT = -666;

        //public static TimeSpan maxTimePeriod = new TimeSpan(23, 45, 0); // 24h , i dont want next day 00:00 sample
        public static TimeSpan maxTimePeriod = new TimeSpan(0, 45, 0); // 2h , i dont want next hour xx:00 sample
        public static TimeSpan stepPeriod = new TimeSpan(0, 15, 0);

        public static void InitializeConfiguration()
        {
            // Color init
            InitDataColors();
            // Float values init
            InitDeltaValues();
            // Database init
            // Other values init
            InitOther();
        }
        
        #region FILE MENAGMENT

        /// <summary>
        /// Create file, if file already exists ignore
        /// </summary>
        /// <param name="filename">Name of a file with extention</param>
        /// <returns>Returns true if file is created</returns>
        public static bool CreateFile(string filename)
        {
            string fullPath = path + filename;
            if (!File.Exists(fullPath))
            {
                using (File.Create(fullPath))
                {
                    return true;
                }
            }
            return false;
        }

        public static void WriteToFile(string filename, string data)
        {
            string fullPath = path + filename;
            CreateFile(filename); // This create file if it doesnt exist and ignore if it exist
            using (StreamWriter outputFile = new StreamWriter(fullPath, true))
            {
                outputFile.WriteLine(data);
            }
        }

        public static string ReadFromFile(string filename)
        {
            string fullPath = path + filename;
            string data;
            CreateFile(filename); // This create file if it doesnt exist and ignore if it exist

            using (StreamReader outputFile = new StreamReader(fullPath, true))
            {
                data = outputFile.ReadLine(); // Always read first line
            }
            return data;
        }
        #endregion FILE MENAGMENT


        #region PARTIAL INIT REGION
        private static void InitDataColors()
        {
            LoadColors();
        }
        private static void InitDeltaValues()
        {
            LoadDeltaValues();
        }
        private static void InitDatabase()
        { }

        private static void InitOther()
        {
            LoadOther();
        }

        
        #endregion PARTIAL INIT REGION

        #region COLOR IMPORT/EXPORT
        /// <summary>
        /// Save current set colors to xml config file
        /// </summary>
        public static void SaveColors()
        {
            using (var stream = new FileStream(ConfigColor.fileName, FileMode.Create))
            {
                XmlSerializer xml = new XmlSerializer(typeof(ConfigColor));
                xml.Serialize(stream, colors);
            }
        }

        /// <summary>
        /// Reset to default colors, and overwrite config file
        /// </summary>
        public static void ResetColors()
        {
            ConfigColor col = new ConfigColor();
            // Copy default colors
            colors.colorDuplicate = col.ColorDuplicate;
            colors.colorInvalid = col.ColorInvalid;
            colors.colorMissing = col.ColorMissing;
            colors.colorNull = col.ColorNull;
            colors.colorRepeat = col.ColorRepeat;
            colors.colorValid = col.ColorValid;
            colors.colorZero = col.ColorZero;

            using (var stream = new FileStream(ConfigColor.fileName, FileMode.Create))
            {
                XmlSerializer xml = new XmlSerializer(typeof(ConfigColor));
                xml.Serialize(stream, col);
            }
        }

        /// <summary>
        /// Load colors from xml config file, if it doesnt exist, create default
        /// </summary>
        public static void LoadColors() 
        {
            ConfigColor col;

            try
            {
                using (var stream = new FileStream(ConfigColor.fileName, FileMode.Open))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(ConfigColor));
                    col = (ConfigColor)xml.Deserialize(stream);

                }
                colors.colorDuplicate = col.ColorDuplicate;
                colors.colorInvalid = col.ColorInvalid;
                colors.colorMissing = col.ColorMissing;
                colors.colorNull = col.ColorNull;
                colors.colorRepeat = col.ColorRepeat;
                colors.colorValid = col.ColorValid;
                colors.colorZero = col.ColorZero;
            }
            catch (Exception)
            {
                Console.WriteLine("COLORS ARE RESET");
                ResetColors(); // This will overwrite corupted config file and return default colors
            }
            
        }
        #endregion COLOR IMPORT/EXPORT

        #region DELTA VALUES IMPORT/EXPORT

        public static void LoadDeltaValues()
        {
            ConfigDeltaValues conf;
            try
            {
                using (var stream = new FileStream(ConfigDeltaValues.fileName, FileMode.Open))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(ConfigDeltaValues));
                    conf = (ConfigDeltaValues)xml.Deserialize(stream);

                }
                delta.deltaCurrent = conf.deltaCurrent;
                delta.deltaVoltage = conf.deltaVoltage;
                delta.zeroDeltaCurrent = conf.zeroDeltaCurrent;
                delta.zeroDeltaVoltage = conf.zeroDeltaVoltage;
            }
            catch (Exception)
            {
                Console.WriteLine("DELTA VALUES ARE RESET");
                ResetDeltaValues(); // This will overwrite corupted config file and return default values
            }
        }

        /// <summary>
        /// Reset delta values and overwrite configuration file
        /// </summary>
        public static void ResetDeltaValues()
        {
            ConfigDeltaValues conf = new ConfigDeltaValues();
            // Copy default delta values
            delta.deltaCurrent = conf.deltaCurrent;
            delta.deltaVoltage = conf.deltaVoltage;
            delta.zeroDeltaCurrent = conf.zeroDeltaCurrent;
            delta.zeroDeltaVoltage = conf.zeroDeltaVoltage;

            // And write them in configuration file
            using (var stream = new FileStream(ConfigDeltaValues.fileName, FileMode.Create))
            {
                XmlSerializer xml = new XmlSerializer(typeof(ConfigDeltaValues));
                xml.Serialize(stream, conf);
            }
        }

        public static void SaveDeltaValues()
        {
            using (var stream = new FileStream(ConfigDeltaValues.fileName, FileMode.Create))
            {
                XmlSerializer xml = new XmlSerializer(typeof(ConfigDeltaValues));
                xml.Serialize(stream, delta);
            }
        }

        #endregion DELTA VALUES IMPORT/EXPORT

        #region OTHER VALUES IMPORT/EXPORT
        public static void SaveOther()
        {
            using (var stream = new FileStream(ConfigOther.fileName, FileMode.Create))
            {
                XmlSerializer xml = new XmlSerializer(typeof(ConfigOther));
                xml.Serialize(stream, other);
            }
        }

        public static void LoadOther()
        {
            ConfigOther conf;
            try
            {
                using (var stream = new FileStream(ConfigOther.fileName, FileMode.Open))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(ConfigOther));
                    conf = (ConfigOther)xml.Deserialize(stream);

                }
                other.daysToCheck = conf.daysToCheck + 1; // days are not zero based, thats why +1
                other.dbWaitTimeout = conf.dbWaitTimeout;
            }
            catch (Exception)
            {
                Console.WriteLine("OTHER VALUES ARE RESET");
                ResetOtherValues(); // This will overwrite corupted config file and return default values
            }
        }

        public static void ResetOtherValues()
        {
            ConfigOther conf = new ConfigOther(); // This will create default values
            // Copy default other values
            other.daysToCheck = conf.daysToCheck + 1;
            other.dbWaitTimeout = conf.dbWaitTimeout;

            // And write them in configuration file
            using (var stream = new FileStream(ConfigOther.fileName, FileMode.Create))
            {
                XmlSerializer xml = new XmlSerializer(typeof(ConfigOther));
                xml.Serialize(stream, conf);
            }
        }

        #endregion
    }
}
