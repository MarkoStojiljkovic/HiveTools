using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiveTools
{
    interface ICancelableForm
    {
        bool ReadyToDie { get; set; }
        bool AlreadyClosed { get; set; }
        void CloseFromOtherThread();
        void ProgresStep();
        void ShowForm();
    }
}
