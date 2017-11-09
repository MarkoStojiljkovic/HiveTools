using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HiveTools
{
    public partial class FormWaitMarquee : Form, ICancelableForm
    {

        ICancelableAsync client;

        public bool ReadyToDie { get; set; }
        public bool AlreadyClosed { get; set; }

        public FormWaitMarquee(ICancelableAsync _client)
        {
            client = _client;

            InitializeComponent();
            // set MarqueeAnimationSpeed
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.MarqueeAnimationSpeed = 30;
            progressBar1.Visible = true;
        }
        
        private void FormWaitMarquee_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!AlreadyClosed)
            {
                // Put closing in invoke queue to baypass race condition
                e.Cancel = true;
                this.BeginInvoke(new Action(() => { this.Close(); }));
                AlreadyClosed = true;
                return;
            }

            if (ReadyToDie)
            {
                client.SuccessCallback();
            }
            else
            {
                client.ErrorCallback(); // I want to notify client that user clicked "X" button
            }

        }

        public void CloseFromOtherThread()
        {
            if (AlreadyClosed)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                Action d = new Action(CloseFromOtherThread);
                this.Invoke(d);
            }
            else
            {
                this.Close();
            }
        }

        public void ProgresStep()
        {
            // We dont need step for this form
        }

        public void ShowForm()
        {
            this.Show();
        }
    }
}
