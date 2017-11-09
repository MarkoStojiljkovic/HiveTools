using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HiveTools
{
    /// <summary>
    /// Used for creating cancelable async methods
    /// </summary>
    public interface ICancelableAsync
    {
        //CancellationTokenSource Cts { get; set; }
        //CancellationToken Ctoken { get; set; }
        void ErrorCallback();
        void SuccessCallback();
    }
}
