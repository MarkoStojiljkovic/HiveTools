using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiveTools
{
    [Serializable]
    /// <summary>
    /// This class will hold all information for 15min point in time
    /// </summary>
    public class Data15MinClass
    {
        // 0 : missing, 1 : valid, >1 : duplicate, -1 : corrupted (not valid, null value, duplicate)
        public int numberOfPoints = 0; // Number of database results for this 15min point
        public double uniqueID = 0; // This is unique row ID that is fetched from database

        private List<FloatValues> internalValues;

        public int valid = -1; // Validity flag read from database, if this is other than 1 i will ignore everything
        public FloatValues Values
        {
            get
            {
                if (internalValues.Count == 0)
                {
                    return null;
                }
                return internalValues.Last();
            }


            set
            {
                internalValues.Add(value);
            }
        } // Values read from database


        
        public Flags flags;
        public DateTime date;


        public Data15MinClass(DateTime _date) // Base constructor
        {
            flags = new Flags();
            valid = 1;
            numberOfPoints = 0;
            internalValues = new List<FloatValues>();
            date = _date;
        }

        /// <summary>
        /// This method is called when we found data point
        /// </summary>
        /// <param name="data"></param>
        public void UpdateData15min(int val, FloatValues data) // This should be called when readTime < currentTime, probably duplicate
        {
            //double.TryParse(uniqueID, out this.uniqueID);

            if (val != 1) // if this data read from database is corrupted abort
            {
                return;
            }
            // Database readings are valid, continue
            numberOfPoints++;
            Values = data; // Add newer data (this is special setter method not just assignment)
            //if (data.successfullyParsed)
            //{
            //    Values = data; // Add newer data if data is consistent (this is special setter method not just assignment)
            //}
        }

        public List<FloatValues> GetAllValues()
        {
            return internalValues;
        }

        public void RemoveValue(FloatValues f)
        {
            internalValues.Remove(f);
            numberOfPoints--;
        }

        public void ReplaceValues(List<FloatValues> values)
        {
            internalValues = values;
            numberOfPoints = values.Count;
        }

    } // end class Data15min
}
