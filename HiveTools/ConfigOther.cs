using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiveTools
{
    [Serializable]
    public class ConfigOther
    {

        public const string fileName = "OtherConfig.xml";

        
        public int daysToCheck = 5;
        public int dbWaitTimeout = 180; // In seconds

    }
}
