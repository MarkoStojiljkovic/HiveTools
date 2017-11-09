using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiveTools
{
    class TimeFormatterClass
    {

        private int min;
        private int Min
        {
            get { return this.min;}
            set
            {
                this.min = value;
                if (this.min == 60)
                {
                    Hour++;
                    this.min = 0;
                }
            }
        }

        private int hour;
        private int Hour
        {
            get { return this.hour; }
            set
            {
                this.hour = value;
                if (this.hour == 24)
                {
                    Day++;
                    this.hour = 0;
                }
            }
        }

        private int day;
        private int Day
        {
            get { return this.day; }
            set
            {
                this.day = value;
            }
        }

        public TimeFormatterClass()
        {
            Min = 0;
            Hour = 0;
            Day = 1;
        }

        //public TimeFormatterClass(int day, int hour, int min)
        //{
        //    Day = day;
        //    Hour = hour;
        //    Min = min;
        //}

        public void CalculateNextValues()
        {
            Min += 15; // This will cause chain of events in this object
        }
        #region GET METHODS

        public string GetMin()
        {
            return Min.ToString();
        }

        public string GetHour()
        {
            return Hour.ToString();
        }

        public string GetDay()
        {
            return Day.ToString();
        }

        #endregion


        public bool Compare(TimeFormatterClass tf)
        {
            if (this.day != tf.Day)
            {
                return false;
            }

            if (this.hour != tf.Hour)
            {
                return false;
            }

            if (this.min != tf.Min)
            {
                return false;
            }

            return true;
        }
    }
}
