using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;

namespace HiveTools
{
    public partial class FormWait : Form, ICancelableForm
    {
        private float step;
        private float currentValueF;
        private int ProgresBarValue;
        //System.Timers.Timer aTimer;
        ICancelableAsync client;
        public bool ReadyToDie { get; set; } // This controls what callback (success or error) should be called
        public bool AlreadyClosed { get; set; } // Indicates if form is already set to be closed

        public FormWait(ICancelableAsync _client, int numOfRows)
        {
            client = _client;

            InitializeComponent();
            progressBar1.Value = 0;
            currentValueF = 0;
            ProgresBarValue = 0;
            if (numOfRows != 0)
            {
                step = 100 / (float)numOfRows;
            }
        }
        

        public void ProgresStep()
        {
            if (AlreadyClosed) 
            {
                return;
            }
            
            if (InvokeRequired)
            {
                Invoke((Action) ProgresStep);
            }
            else
            {
                currentValueF += step;

                if (currentValueF > 100)
                {
                    ProgresBarValue = 100;
                }
                else
                {
                    ProgresBarValue = (int)currentValueF;
                }

                progressBar1.Value = ProgresBarValue;
                labelPercentage.Text = ProgresBarValue.ToString() + "%";

            }
            
        }
        

        private void FormWait_FormClosing(object sender, FormClosingEventArgs e)
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
            if (AlreadyClosed) // skip closing if form is closed manualy
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

        public void ShowForm()
        {
            this.Show();
        }
    }
}
