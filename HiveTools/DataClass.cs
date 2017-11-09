using HiveTools.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace HiveTools
{
    [Serializable]
    public class DataClass // Hold values for selected period for all 15min points
    {
        public Data15MinClass[] data;
        private int numOf15MinValues;
        public DateTime startDate;
        public DateTime endDate;
        public int deviceID = -1; // Device ID
        FloatValues lastFw = new FloatValues("666", "666", "666", "666", "666", "666", "0"); // If this number matches the reading... it will generate 1 false positive AND DEVIL WILL BE SPAWNED
        public int rowIndexInAllData = -1;

        // Summary and analysis variables
        public DataIntegritySummaryClass summary; // This will be null until generate summary is run (i want to throw error if someone uses this earlier)

        public DataClass(int numOfDays, int rowNum, DateTime _dateStart, DateTime _dateEnd)
        {
            startDate = _dateStart;
            endDate = _dateEnd;
            // numOfDays can be calculated, but it is precalculated anyway
            numOf15MinValues = numOfDays * 96; 
            data = new Data15MinClass[numOf15MinValues];
            rowIndexInAllData = rowNum;
        }

        public void ReplaceData(Data15MinClass d)
        {

        }

        /// <summary>
        /// Decode data from database and fill AllData[currentRow] with results
        /// </summary>
        /// <param name="dataString"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="id"></param>
        /// <param name="delim"></param>
        public void FillDataRow(string dataString, int id, char delim)
        {
            string validFlag;
            string timestamp;
            DateTime currentDate, readDate;
            TimeSpan ts;
            int current15MinValue = 0;
            
            string[] rows = dataString.Split(delim); // split monolithic data to single rows
            currentDate = startDate;

            // Fill everything with data missing
            ts = endDate.Subtract(currentDate);
            while (ts.TotalMinutes != 0)
            {
                data[current15MinValue] = new Data15MinClass(currentDate); // Fill with "missing" data
                currentDate = currentDate.AddMinutes(15); // Update current date
                current15MinValue++;
                ts = endDate.Subtract(currentDate);
            }


            // Sometimes there wont be any data
            if (rows.Length == 1 && dataString.Equals(""))
            {// This is when no results came from db
                deviceID = id;
                return;
            }
            else
            { // Everything is ok (we have some data), loop thru every row and parse it
                
                for (int i = 0; i < rows.Length; i++)
                {
                    #warning I experimented with null values

                    // Read values
                    string[] temp = rows[i].Split(';'); // Split row from db to separated searched values
                    // t[0]=valid, t[1]=ia, t[2]=ib, t[3]=ic, t[4]=vab, t[5]=vbc, t[6]=vca, t[7]=read_time
                    validFlag = temp[0];
                    timestamp = temp[7];
                    readDate = HelperClass.ConvertOracleTimeToDateTime(timestamp);
                    FloatValues fv = new FloatValues(temp[1], temp[2], temp[3], temp[4], temp[5], temp[6], temp[8]);
                    if (!fv.successfullyParsed)
                    {
                        // This is null value, replace with default values for null
                        fv.ReplaceWithNullValue();
                    }

                    // Get number of minutes since start
                    ts = readDate.Subtract(startDate);

                    // Calculate 15min sector and update it
                    current15MinValue = (int)ts.TotalMinutes / 15;
                    data[current15MinValue].UpdateData15min(Convert.ToInt32(validFlag), fv);

                }
            }
            deviceID = id;
        }


        /// <summary>
        /// Analyze all 15min data points and rise internal flags based on values
        /// </summary>
        public void ProcessData()
        {
            lastFw = new FloatValues(); // Initialize with devil data
            foreach (var d in data)
            {
                if (d.valid == 1)
                {
                    // Data is valid
                    if (d.numberOfPoints != 0)
                    {
                        // First check if there is some data (null values)
                        if (d.Values == null)
                        {
                            d.flags.nullFlag = true;
                            continue;
                        }

                        // Check is it zero
                        d.flags.zeroFlag = FloatHelper.CheckIfZero(d.Values);

                        if (!d.flags.zeroFlag)
                        {
                            // Check if data is repeating, ignoring zero values
                            d.flags.dataRepeatFlag = FloatHelper.CompareDuplicateValues(lastFw, d.Values); // This duplicate is based on last storred value
                            lastFw = d.Values; // make sure next 15min point compares with this value
                        }

                        // Check if null
                        if (!d.Values.successfullyParsed) // Probably redundant
                        {
                            d.flags.nullFlag = true;
                        }

                        if (d.numberOfPoints > 1) // Is it duplicate (more data points arrived for this timestamp)
                        {
                            d.flags.duplicateFlag = true;
                        }

                    }
                    else
                    { // numberOfPoints = 0
                      // Data is missing
                        d.flags.missingFlag = true;
                    }

                }
                else
                { // Data is not valid
                    d.flags.invalidFlag = true;
                }

            } // foreach


        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(1000);
            DateTime date = startDate;

            foreach (var item in data)
            {
                //sb.Append(date.ToString("dd/MM/yyyy HH:mm") + ";" + item.numberOfPoints.ToString() + "|");
                //date = date.AddMinutes(15);
                sb.Append(item.date.ToString("dd/MM/yyyy HH:mm") + ";" + item.numberOfPoints.ToString() + "|");
            }
            sb.Remove(sb.Length - 1, 1); // Remove last "|"

            return sb.ToString();
        }

        /// <summary>
        /// This will generate short summary of valid/invalid data and occured flags
        /// </summary>
        /// <returns></returns>
        internal string GenerateSummary()
        {
            // Result is : Valid:xxx|Duplicate:xxx OLD
            // Generate data desc for other flags
            string zFlag = ""; // zero flag
            string nFlag = ""; // null flag
            string dFlag = ""; // duplicate flag
            string rFlag = ""; // data repeat flag
            string iFlag = ""; // invalid flag

            if (summary.numOfZeroes > 0)
            {
                zFlag = "Z ";
            }
            if (summary.numOfNullValues > 0)
            {
                nFlag = "N ";
            }
            if (summary.numOfDuplicate > 0)
            {
                dFlag = "D ";
            }
            if (summary.numOfDataRepeat > 0)
            {
                rFlag = "R ";
            }
            if (summary.numOfInvalid > 0)
            {
                iFlag = "I ";
            }
            string flagStatus = zFlag + nFlag + dFlag + rFlag + iFlag;
            string res = ((float)((summary.numOfValid + summary.numOfDataRepeat + summary.numOfZeroes + summary.numOfDuplicate) * 100)/ summary.totalNumber).ToString("0.000") + "|" + flagStatus;
            return res;
        }

        // TODO: this can be private method !!!
        /// <summary>
        /// This will generate stats based on all 15min values
        /// </summary>
        public void RunAnalysis()
        {
            summary = new DataIntegritySummaryClass();
            
            foreach (var item in data)
            {
                summary.totalNumber++;

                // First check validity flag
                if (item.flags.invalidFlag)
                {
                    summary.numOfInvalid++;
                    continue; // I dont want to check for further flags if data is invalid
                }

                // Check for missing data
                if (item.flags.missingFlag)
                {
                    summary.numOfMissing++;
                    continue; // I dont need to check for other flags if data is missing
                }

                // At this point we received valid data for sure

                // Data can have multiple flags, this is quick fix :(
                int multiFLag = -1;

                // Check for null value
                if (item.flags.nullFlag)
                {
                    summary.numOfNullValues++;
                    multiFLag++;
                }

                if (item.flags.zeroFlag)
                {
                    summary.numOfZeroes++;
                    multiFLag++;
                }

                if (item.flags.dataRepeatFlag)
                {
                    summary.numOfDataRepeat++;
                    multiFLag++;
                }

                if (item.flags.duplicateFlag)
                {
                    summary.numOfDuplicate++;
                    multiFLag++;
                }

                if (!(item.flags.duplicateFlag || item.flags.dataRepeatFlag || item.flags.zeroFlag || item.flags.nullFlag))
                {
                    // If not any error or warning flag it is valid flag
                    summary.numOfValid++;
                    multiFLag++;
                }

                summary.totalNumber += multiFLag;
                
            }
            
            // Now generate percentages
            // First calculate total number of flags
            //int totalFlags = summary.numOfDataRepeat + summary.numOfDuplicate + summary.numOfInvalid + summary.numOfMissing + summary.numOfNullValues +
            //        +summary.numOfValid + summary.numOfZeroes;
            // Then calculate percentages
            //summary.percentageDataRepeat = (float)summary.numOfDataRepeat / ((float)totalFlags) * 100;
            //summary.percentageDuplicate = (float)summary.numOfDuplicate / ((float)totalFlags) * 100;
            //summary.percentageInvalid = (float)summary.numOfInvalid / ((float)totalFlags) * 100;
            //summary.percentageMissing = (float)summary.numOfMissing / ((float)totalFlags) * 100;
            //summary.percentageNull = (float)summary.numOfNullValues / ((float)totalFlags) * 100;
            //summary.percentageValid = (float)summary.numOfValid / ((float)totalFlags) * 100;
            //summary.percentageZeroes = (float)summary.numOfZeroes / ((float)totalFlags) * 100;

        }

        /// <summary>
        /// Not used right now but might have ussage later
        /// </summary>
        /// <returns></returns>
        public DataClass Clone()
        {
            // Create a copy of this object
            MemoryStream stream = SerializeHelperClass.SerializeToStream(this);
            DataClass clone = (DataClass)SerializeHelperClass.DeserializeFromStream(stream);
            return clone;
        }

    } // End of DataClass


    #region DATA HELPER CLASSES

    [Serializable]
    public class FloatValues
    {
        public bool successfullyParsed;
        public float vab, vbc, vca;
        public float ia, ib, ic;
        public string sIa, sIb, sIc, sVab, sVbc, sVca;
        public double uniqueID; // This is ID that is unique to this measurement

        /// <summary>
        /// Initialize with data that is almost imposible to cause a false positive
        /// </summary>
        public FloatValues() : this("666", "666", "666", "666", "666", "666", "0")// Constructor with wicked data
        {
        }

        /// <summary>
        /// Replace float values with default null values, while keeping uniqueID number and "successfullyParsed" flag
        /// </summary>
        public void ReplaceWithNullValue()
        {
            vab = ConfigClass.DEFAULT_VALUE_FOR_NULL_FLOAT;
            vbc = ConfigClass.DEFAULT_VALUE_FOR_NULL_FLOAT;
            vca = ConfigClass.DEFAULT_VALUE_FOR_NULL_FLOAT;

            ia = ConfigClass.DEFAULT_VALUE_FOR_NULL_FLOAT;
            ib = ConfigClass.DEFAULT_VALUE_FOR_NULL_FLOAT;
            ic = ConfigClass.DEFAULT_VALUE_FOR_NULL_FLOAT;
        }

        public FloatValues(string fIa, string fIb, string fIc, string fVab, string fVbc, string fVca, string _uniqueID)
        {
            successfullyParsed = true;

            // Copy all strings and replace fucking ',' to '.'
            sIa = fIa.Replace(",", ".");
            sIb = fIb.Replace(",", ".");
            sIc = fIc.Replace(",", ".");
            sVab = fVab.Replace(",", ".");
            sVbc = fVbc.Replace(",", ".");
            sVca = fVca.Replace(",", ".");

            double.TryParse(_uniqueID, out this.uniqueID);

            if (CheckIfNull())
            {
                successfullyParsed = false;
                return;
            }
            


            // Now parse values
            try
            {
                vab = float.Parse(sVab, CultureInfo.InvariantCulture);
                vbc = float.Parse(sVbc, CultureInfo.InvariantCulture);
                vca = float.Parse(sVca, CultureInfo.InvariantCulture);

                ia = float.Parse(sIa, CultureInfo.InvariantCulture);
                ib = float.Parse(sIb, CultureInfo.InvariantCulture);
                ic = float.Parse(sIc, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                successfullyParsed = false;
            }
        }

        /// <summary>
        /// Returns true if any of value is empty
        /// </summary>
        /// <returns></returns>
        public bool CheckIfNull()
        {
            if (sIa.Equals(""))
            {
                return true;
            }
            else if(sIb.Equals(""))
            {
                return true;
            }
            else if (sIc.Equals(""))
            {
                return true;
            }
            else if (sVab.Equals(""))
            {
                return true;
            }
            else if (sVbc.Equals(""))
            {
                return true;
            }
            else if (sVca.Equals(""))
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return sIa + "" + sIb + "" + sIc + "" + sVab + "" + sVbc + "" + sVca;
        }

    } // Class FloatValues
    [Serializable]
    public class Flags
    {
        public bool zeroFlag, nullFlag, dataRepeatFlag, invalidFlag, missingFlag, duplicateFlag; // Status flags
    }

    #endregion
}
