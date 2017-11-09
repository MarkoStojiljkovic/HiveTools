using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;

namespace HiveTools
{
    class HelperInterruptibleWorker
    {
        ICancelableAsync client;
        //private CancellationTokenSource cts; // i dont use this for now
        //private CancellationToken ctoken; // i dont use this for now
        //FormWaitMarquee fw;
        Thread cancelableThread;
        ICancelableForm f;


        public HelperInterruptibleWorker(ICancelableAsync _client, ICancelableForm _f)
        {
            client = _client;
            f = _f;
        }

        public void ExecuteTask(Action action)
        {
            //fw = new FormWaitMarquee(this);
            f.ShowForm();

            cancelableThread = new Thread(() =>
            {
                action();
                CloseForm();
            });
            cancelableThread.Start();
        }

        //public void ErrorCallback()
        //{
        //    Console.WriteLine("ErrorCallback called from form closing");
        //    client.ErrorCallback();
        //    cancelableThread.Abort();
        //}

        //public void SuccessCallback() // This will be called when form is closed
        //{
        //    Console.WriteLine("Callback from form");
        //}

        private void CloseForm()
        {
            f.ReadyToDie = true;
            f.CloseFromOtherThread(); // Close form wait
        }
    }
}
