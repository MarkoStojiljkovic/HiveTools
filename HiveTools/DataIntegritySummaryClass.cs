using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiveTools
{
    [Serializable]
    public class DataIntegritySummaryClass
    {
        public int numOfValid = 0;
        public int numOfDuplicate = 0;
        public int numOfInvalid = 0;
        public int numOfMissing = 0;

        public int numOfZeroes = 0;
        public int numOfNullValues = 0;
        public int numOfDataRepeat = 0;
        public int totalNumber = 0;

        //public float percentageValid = 0;
        //public float percentageDuplicate = 0;
        //public float percentageInvalid = 0;
        //public float percentageMissing = 0;
        //public float percentageZeroes = 0;
        //public float percentageNull = 0;
        //public float percentageDataRepeat = 0;
    }
}
