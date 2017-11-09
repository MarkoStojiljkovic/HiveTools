using System;
using System.Collections.Generic;
using System.Drawing;
using HiveTools;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiveToolsFlagsHelper
{
    public enum ReportFlag { Empty, Valid, Duplicate, Missing, Null, Repeat, Zero, Invalid };
    public class HelperGUIStats
    {
        enum ReportSM { STATE_ENTRY, STATE_FLAGS_DIFFER, STATE_FLAGS_MATCH }

        /// <summary>
        /// Generate list of selected flags, separe days too
        /// </summary>
        /// <param name="item"> DataClass object </param>
        /// <param name="flag"> Selected flag for report generation </param>
        /// <returns></returns>
        public static List<FlagOccurrenceClass> GenerateReportSeparateDays(DataClass item, ReportFlag flag)
        {
            List<FlagOccurrenceClass> lis = new List<FlagOccurrenceClass>();
            //  Get tuple based on flag
            var resFromFlag = GetDelegateBasedOnFlag(flag);

            // Assign tuple values returned by method
            Func<Flags, bool> calcuFunc = resFromFlag.Item1;
            string flagDesc = resFromFlag.Item2;
            Color currentCol = resFromFlag.Item3;

            bool res; // result from comparing flags
            DateTime startDate = item.startDate; // Date to display start of active flags
            bool endFlag = false; // This will signal end of array
            ReportSM state = ReportSM.STATE_ENTRY; // Inital state of SM
            
            bool newDay = false;
            for (int i = 0; i < item.data.Length; i++) // Loop thru every 15min value
            {
                if (i + 1 == item.data.Length)
                {
                    endFlag = true;
                }

                Data15MinClass dataItem = item.data[i]; // Get data
                res = calcuFunc(dataItem.flags); // Get result from flags compare
                newDay = dataItem.date.Day != startDate.Day; // Get new day indicating flag
                switch (state)
                {
                    case ReportSM.STATE_ENTRY: // Entry state sets startDate and chooses next state
                        startDate = dataItem.date;
                        if (res)
                        {
                            state = ReportSM.STATE_FLAGS_MATCH;
                        }
                        else
                        {
                            state = ReportSM.STATE_FLAGS_DIFFER;
                        }
                        break;
                    case ReportSM.STATE_FLAGS_DIFFER: // Update startDate if new day and wait for matching flags
                        if (newDay)
                        {
                            startDate = dataItem.date;
                        }
                        if (res) // Check if flags are matching
                        {
                            state = ReportSM.STATE_FLAGS_MATCH; // Goto state for matching flags
                            startDate = dataItem.date; // Save starting date
                        }
                        break;
                    case ReportSM.STATE_FLAGS_MATCH: // Wait for flag difference and write result
                        if (endFlag) // Its end of data and we are still in flags match state, end current "array"
                        {
                            DateTime temp = dataItem.date.AddMinutes(15); // Add minutes so temp points to new day and "00"
                            //lis.Add(new RowDataClass(flagDesc, currentCol, startDate.ToString("dd/MM/yyyy"), startDate.ToString("HH:mm"), temp.ToString("HH:mm"), "end")); // original
                            lis.Add(new FlagOccurrenceClass(flagDesc, currentCol, startDate, temp));
                            break;
                        }
                        if (newDay) // Check if new day, new day restarts cycle
                        {
                            // It is new day, save result and start over
                            //lis.Add(new RowDataClass(flagDesc, currentCol, startDate.ToString("dd/MM/yyyy"), startDate.ToString("HH:mm"), dataItem.date.ToString("HH:mm"), "end")); // original
                            lis.Add(new FlagOccurrenceClass(flagDesc, currentCol, startDate, dataItem.date));
                            startDate = dataItem.date;
                            break;
                        }
                        // Compare result
                        if (!res)
                        {
                            state = ReportSM.STATE_FLAGS_DIFFER;
                            //lis.Add(new RowDataClass(flagDesc, currentCol, startDate.ToString("dd/MM/yyyy"), startDate.ToString("HH:mm"), dataItem.date.ToString("HH:mm"), "end"));
                            lis.Add(new FlagOccurrenceClass(flagDesc, currentCol, startDate, dataItem.date));
                        }
                        break;
                    default:
                        throw new Exception("Corrupted state in ReportSM");
                }
            }
            return lis;
        }
        
        /// <summary>
        /// Generate list of selected flags
        /// </summary>
        /// <param name="item"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static List<FlagOccurrenceClass> GenerateReport(DataClass item, ReportFlag flag)
        {
            List<FlagOccurrenceClass> lis = new List<FlagOccurrenceClass>();

            //  Get tuple based on flag
            var resFromFlag = GetDelegateBasedOnFlag(flag);

            // Assign tuple values returned by method
            Func<Flags, bool>  calcuFunc = resFromFlag.Item1;
            string flagDesc = resFromFlag.Item2;
            Color currentCol = resFromFlag.Item3;
            
            bool res; // result from comparing flags
            DateTime startDate = item.startDate; // Date to display start of active flags
            bool endFlag = false; // This will signal end of array
            ReportSM state = ReportSM.STATE_ENTRY; // Inital state of SM

            
            for (int i = 0; i < item.data.Length; i++) // Loop thru every 15min value
            {
                if (i + 1 == item.data.Length)
                {
                    endFlag = true;
                }

                Data15MinClass dataItem = item.data[i]; // Get data
                res = calcuFunc(dataItem.flags); // Get result from flags compare (does it match selected flag)
                switch (state)
                {
                    case ReportSM.STATE_ENTRY: // Entry state sets startDate and chooses next state
                        startDate = dataItem.date;
                        if (res)
                        {
                            state = ReportSM.STATE_FLAGS_MATCH;
                        }
                        else
                        {
                            state = ReportSM.STATE_FLAGS_DIFFER;
                        }
                        break;
                    case ReportSM.STATE_FLAGS_DIFFER: // Update startDate if new day and wait for matching flags

                        if (res) // Check if flags are matching
                        {
                            state = ReportSM.STATE_FLAGS_MATCH; // Goto state for matching flags
                            startDate = dataItem.date; // Save starting date
                        }
                        break;
                    case ReportSM.STATE_FLAGS_MATCH: // Wait for flag difference and write result
                        if (endFlag) // Its end of data and we are still in flags match state, end current "array"
                        {
                            DateTime temp = dataItem.date.AddMinutes(15); // Add minutes so temp points to new day and "00"
                            //lis.Add(new RowDataClass(flagDesc, currentCol, startDate.ToString("dd/MM/yyyy"), startDate.ToString("HH:mm"), temp.ToString("HH:mm"), "end")); // original
                            lis.Add(new FlagOccurrenceClass(flagDesc, currentCol, startDate, temp));
                            break;
                        }
                        // Compare result
                        if (!res)
                        {
                            state = ReportSM.STATE_FLAGS_DIFFER;
                            //lis.Add(new RowDataClass(flagDesc, currentCol, startDate.ToString("dd/MM/yyyy"), startDate.ToString("HH:mm"), dataItem.date.ToString("HH:mm"), "end"));
                            lis.Add(new FlagOccurrenceClass(flagDesc, currentCol, startDate, dataItem.date));
                        }
                        break;
                    default:
                        throw new Exception("Corrupted state in ReportSM");
                }
            }
            return lis;
        }

        /// <summary>
        /// Get method implementation, description and color based on given flag
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        private static Tuple<Func<Flags, bool>,string, Color> GetDelegateBasedOnFlag(ReportFlag flag)
        {
            Func<Flags, bool> calcuFunc;
            string flagDesc = "";
            Color currentCol;

            switch (flag) // Select fucntion and color based on flag
            {
                case ReportFlag.Empty:
                case ReportFlag.Valid:
                case ReportFlag.Duplicate:
                    flagDesc = "Duplicate";
                    calcuFunc = (f) => f.duplicateFlag == true;
                    currentCol = ConfigClass.colors.colorDuplicate;
                    break;
                case ReportFlag.Missing:
                    flagDesc = "Missing";
                    calcuFunc = (f) => f.missingFlag == true;
                    currentCol = ConfigClass.colors.colorMissing;
                    break;
                case ReportFlag.Null:
                    flagDesc = "Null values";
                    calcuFunc = (f) => f.nullFlag == true;
                    currentCol = ConfigClass.colors.colorNull;
                    break;
                case ReportFlag.Repeat:
                    flagDesc = "Data repeat";
                    calcuFunc = (f) => f.dataRepeatFlag == true;
                    currentCol = ConfigClass.colors.colorRepeat;
                    break;
                case ReportFlag.Zero:
                    flagDesc = "Zero values";
                    calcuFunc = (f) => f.zeroFlag == true;
                    currentCol = ConfigClass.colors.colorZero;
                    break;
                case ReportFlag.Invalid:
                    flagDesc = "Invalid";
                    calcuFunc = (f) => f.invalidFlag == true;
                    currentCol = ConfigClass.colors.colorInvalid;
                    break;
                default:
                    throw new Exception("Flag not recognized");
            }

            return new Tuple<Func<Flags, bool>, string, Color>(calcuFunc, flagDesc, currentCol);
        }

        /// <summary>
        /// List off all flags represented in occurance periods
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static List<FlagOccurrenceClass> GenerateReportForAllFlags(DataClass item)
        {
            List<FlagOccurrenceClass> lis = new List<FlagOccurrenceClass>();

            lis.AddRange(GenerateReport(item,ReportFlag.Missing));
            lis.AddRange(GenerateReport(item, ReportFlag.Duplicate));
            lis.AddRange(GenerateReport(item, ReportFlag.Zero));
            lis.AddRange(GenerateReport(item, ReportFlag.Null));
            lis.AddRange(GenerateReport(item, ReportFlag.Repeat));
            return lis;
        }


        public static string ConvertListToCSV(List<FlagOccurrenceClass> flagList, string delim, int ID)
        {
            StringBuilder sb = new StringBuilder(); // It will quite often be a large list
            foreach (var item in flagList)
            {
                sb.Append(ID.ToString() + delim + item.flag + delim + item.from + delim + item.to + "\r\n");
            }
            return sb.ToString();
        }
        
    }


    /// <summary>
    /// Holds information of flag description and its occurrence in time period (start and end date)
    /// </summary>
    public class FlagOccurrenceClass
    {
        public string flag;
        public Color col;
        public string from;
        public string to;

        public DateTime startDate;
        public DateTime endDate;

        public FlagOccurrenceClass(string flag_, Color col_, DateTime startDate_, DateTime endDate_)
        {
            startDate = startDate_;
            endDate = endDate_;

            flag = flag_;
            col = col_;
            from = startDate_.ToString("dd/MM/yyyy  HH:mm");
            to = endDate_.ToString("dd/MM/yyyy  HH:mm");
        }

    }


}
