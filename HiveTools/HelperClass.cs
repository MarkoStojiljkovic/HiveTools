using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace HiveTools.Helpers
{
    // Empty is dummy value, for non initialized state
    enum StatusCode {Empty, Valid, Duplicate, Missing, ErrorNull, ErrorDuplicate, ErrorZero, ErrorValid};

    class HelperClass
    {
        // Convert datetime object to oracle querry string
        public static string ConvertToOracleTime(string y, string m, string d, string h, string min)
        {
            // TO_DATE('2015/11/01 8:15', 'YYYY/MM/DD HH:MI')
            StringBuilder sb = new StringBuilder();
            sb.Append("TO_DATE('" +  y + '/' + m + '/' + d + ' ' + h + ':' + min + "', 'YYYY/MM/DD HH24:MI')");
            return sb.ToString();
        }
        public static string ConvertToOracleTime(DateTime dt)
        {
            string time = "TO_DATE('" + dt.Year.ToString() + '/' + dt.Month.ToString() + '/' + dt.Day.ToString() + ' ' + dt.Hour.ToString() + ':' + dt.Minute.ToString() + "', 'YYYY/MM/DD HH24:MI')";
            return time;
        }
        
        /// <summary>
        /// Convert DateTime object to timestamp string which Custom.xml uses
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ConvertToNtpmCustomXmlTime(DateTime dt)
        {
            string time = dt.Year + "-" + dt.Month.ToString("00") + "-" + dt.Day.ToString("00") + "-" + dt.Hour.ToString("00") + "-" + dt.Minute.ToString("00") + "-" + dt.Second.ToString("00");
            return time;
        }

        // Check if data is not null or empty
        public static bool CheckDataValidity(string res)
        {
            string[] temp = res.Split(';');
            if (temp.Length == 2)
            {
                if (temp[0].Equals("") || temp[1].Equals(""))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public static DateTime ConvertOracleTimeToDateTime(string s)
        {
            // Those are fucking 2 ways of representing date data which db can return
            // 1.5.2017. 10:45:01
            // 7/1/2017 12:00:01 AM

            int day, month, year, min, hour;

            int current = 0; // Current index
            int next;

            // For now look for first date delimiter and parse accordingly to that, its not the best solution but it should work
            next = s.IndexOf('.');

            if (next != -1)
            {
                // Format is probably like 1.5.2017. 10:45:01 
                day = Convert.ToInt32(s.Substring(current, next));

                current = next + 1;
                next = s.IndexOf('.', current);
                month = Convert.ToInt32(s.Substring(current, next - current));

                current = next + 1;
                next = s.IndexOf('.', current);
                year = Convert.ToInt32(s.Substring(current, next - current));

                current = next + 2;
                next = s.IndexOf(':', current);
                hour = Convert.ToInt32(s.Substring(current, next - current));

                current = next + 1;
                next = s.IndexOf(':', current);
                min = Convert.ToInt32(s.Substring(current, next - current));

                return new DateTime(year, month, day, hour, min, 0);
            }
            else
            {
                // For now i want to throw exception if this parsing isnt good too, i will assume that format is like 7/1/2017 12:00:01 AM , NOTE: DAY AND MONTH ARE SWAPPED
                
                DateTime time = DateTime.ParseExact(s, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                // I want to filter minutes from datetime
                string filteredTime = time.ToString("MM/dd/yyyy hh:mm tt");
                time = DateTime.ParseExact(filteredTime, "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);

                return time;
                
            }
        }
    }

    class FloatHelper
    {
        public static bool CheckIfZero(FloatValues f1)
        {
            bool currentARes = EqualWithDelta(f1.ia, 0f, ConfigClass.delta.zeroDeltaCurrent);
            bool currentBRes = EqualWithDelta(f1.ib, 0f, ConfigClass.delta.zeroDeltaCurrent);
            bool currentCRes = EqualWithDelta(f1.ic, 0f, ConfigClass.delta.zeroDeltaCurrent);

            bool vabRes = EqualWithDelta(f1.vab, 0f, ConfigClass.delta.zeroDeltaVoltage);
            bool vbcRes = EqualWithDelta(f1.vbc, 0f, ConfigClass.delta.zeroDeltaVoltage);
            bool vcaRes = EqualWithDelta(f1.vca, 0f, ConfigClass.delta.zeroDeltaVoltage);

            return currentARes && currentBRes && currentCRes && vabRes && vbcRes && vcaRes; // If all are the same, its zero value

        }
        public static bool CompareDuplicateValues(FloatValues f1, FloatValues f2)
        {
            bool currentARes = EqualWithDelta(f1.ia, f2.ia, ConfigClass.delta.deltaCurrent);
            bool currentBRes = EqualWithDelta(f1.ib, f2.ib, ConfigClass.delta.deltaCurrent);
            bool currentCRes = EqualWithDelta(f1.ic, f2.ic, ConfigClass.delta.deltaCurrent);

            bool vabRes = EqualWithDelta(f1.vab, f2.vab, ConfigClass.delta.deltaVoltage);
            bool vbcRes = EqualWithDelta(f1.vbc, f2.vbc, ConfigClass.delta.deltaVoltage);
            bool vcaRes = EqualWithDelta(f1.vca, f2.vca, ConfigClass.delta.deltaVoltage);

            //if (currentARes && currentBRes && currentCRes && vabRes && vbcRes && vcaRes)
            //{
            //    Console.WriteLine("Debug Line");
            //}
            return currentARes && currentBRes && currentCRes && vabRes && vbcRes && vcaRes; // If all are the same, its duplicate value
        }
        public static bool EqualWithDelta(float f1, float f2, float delta)
        {
            if (f1 > 0 && f2 > 0) // Are they both positive nubers
            {

                float temp = Math.Abs(f1 - f2);

                if (temp > delta)
                {
                    return false;
                }
                return true;
            }
            else if (f1 < 0 && f2 < 0) // Are they both negative numbers
            {
                float temp = Math.Abs(f1 + f2);

                if (temp > delta)
                {
                    return false;
                }
                return true;
            }
            else
            {
                // 1 is positive and 1 is negative number
                if (f1 < 0) // lets asume f1 is negative
                {
                    if (f1 + delta > f2)
                    {
                        return true;
                    }
                    return false;
                }
                else
                {
                    // We assumed wrong, f2 is negative
                    if (f2 + delta > f1)
                    {
                        return true;
                    }
                    return false;
                }
            }
        }
    }

    class XmlHelper
    {

        public static string FindFirstDescendant(string tag, XDocument doc)
        {
            return doc.Descendants(tag).First().Value;
        }
    }
}
