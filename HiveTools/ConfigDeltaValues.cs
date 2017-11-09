using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiveTools
{
    [Serializable]
    public class ConfigDeltaValues
    {
        public const string fileName = "DeltaValuesConfig.xml";

        // Default delta values
        public float deltaCurrent = 0.01f;
        public float deltaVoltage = 0.1f;
        public float zeroDeltaCurrent = 0.2f;
        public float zeroDeltaVoltage = 1;
    }
}
