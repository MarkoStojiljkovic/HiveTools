using HiveTools.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;

namespace HiveTools
{
    public partial class MainForm : Form, ICancelableAsync
    {
        //FormWait fw; // Form wait
        GraphForm gf; // Form that will be hidden until someone hovers across some cell
        private bool asyncWorkDone { get; set; }


        // Global static reference to MainForm
        public static MainForm mainFormRef = null;

        public MainForm()
        {

            CultureInfo newCultureDefinition = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            ////make percentages n% instead of n % .ToString("p") adds a space by default english culture in asp.net
            //newCultureDefinition.CurrentUICulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            //Thread.CurrentThread.CurrentUICulture = newCultureDefinition;

            InitializeComponent();
            DatabaseSettingsClass.ConnectionStringInit();
            dateTimePickerStartDataIntegrity.Value = DateTime.Now;
            dateTimePickerEndDataIntegrity.Value = DateTime.Now;
            dateTimePickerStart.Value = DateTime.Now;
            dateTimePickerEnd.Value = DateTime.Now;

            string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            string sysUIFormat = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern;

            gf = new GraphForm();
            gf.StartPosition = FormStartPosition.CenterScreen;
            ConfigClass.InitializeConfiguration();
            #region Test for post request
            //List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>
            //{
            //    new KeyValuePair<string, string>("start", "2017-06-01-00-00-00"),
            //    new KeyValuePair<string, string>("stop", "2017-06-01-00-15-00"),
            //    new KeyValuePair<string, string>("type", "current"),
            //    new KeyValuePair<string, string>("tags", "TemA"),
            //    new KeyValuePair<string, string>("user", "admin"),
            //    new KeyValuePair<string, string>("pass", "admin"),
            //};
            //HTTPClientClass test = new HTTPClientClass();  
            //test.PostRequest("http://172.20.160.5:81/custom.xml", pairs);
            #endregion
            mainFormRef = this;
            //lastAllDataPtr = allData; // Always update reference
            //FormWaitMarquee test = new FormWaitMarquee();
            //test.Show();
            //DEBUGFUNC();
        }


        async void DEBUGFUNC()
        {
            try
            {
#warning CHANGE DATE FORMAT
                DateTime startDate = new DateTime(2017, 10, 10, 0, 0, 0);
                DateTime endDate = new DateTime(2017, 10, 10, 23, 45, 0);

                // Create array of date pairs if xml data needs to be fragmented
                List<CustomXmlDatePair> datePeriodsList = CustomXmlDatePair.FragmentDate(startDate, endDate);
                
                string ip = "192.168.2.21";

                string reportRes;
                XDocument reportDoc;

                try
                {
                    reportRes = await NTPMControllerClass.GetReport(ip);
                    reportDoc = XDocument.Parse(reportRes);
                }
                catch (Exception)
                {
                    MessageBox.Show("Couldn't get or parse report.xml");
                    return; // abort
                }

                // Create header dates
                string headerStartTime = HelperClass.ConvertToNtpmCustomXmlTime(startDate);
                string headerEndTime = HelperClass.ConvertToNtpmCustomXmlTime(endDate);
                string csvHeader = NTPMControllerClass.CreateCSVHeader(headerStartTime, headerEndTime, reportDoc);

                // now we only need to fill values with each row

                // ---- Get and parse measurement.xml

                List<XDocument> measurementDoc = new List<XDocument>();
                List<string> measurementRows = new List<string>();

                foreach (var datePeriod in datePeriodsList)
                {
                    Console.WriteLine("Parsing period: " + datePeriod.startHeader + "---" + datePeriod.endHeader);
                    await HelperRetryClass.DoWithRetryAsync(async () =>
                    {
                        // Generate the random number between 1-2
                        Random rnd = new Random();
                        var result = rnd.Next(1, 10);
                        if (result == 2)
                        {
                            throw new Exception("Random throw for debug");
                        }

                        List<KeyValuePair<string, string>> pairs = NTPMControllerClass.GetPairsForCustomXmlPostMethod(datePeriod.startHeader, datePeriod.endHeader, "neticoadmin", "neticoadmin");
                        string measurementRes = await HTTPClientClass.PostRequest("http://" + ip + "/custom.xml", pairs);
                        XDocument tempMeasure = XDocument.Parse(measurementRes);
                        //FormCustomConsole.WriteLine(tempMeasure.ToString());
                        NTPMControllerClass.CheckErrorTag(tempMeasure);
                        string measureRow = NTPMControllerClass.CreateCSVMeasurementRow(tempMeasure);
                        // At this point, download and parsing is ok
                        measurementRows.Add(measureRow);
                        measurementDoc.Add(tempMeasure);
                    },
                    TimeSpan.FromSeconds(2));
                }

                StringBuilder sFinal = new StringBuilder();
                sFinal.Append(csvHeader);
                foreach (var item in measurementRows)
                {
                    sFinal.Append(item);
                    sFinal.Append("\r\n");
                }
                sFinal.Remove(sFinal.Length - 2, 2); // Remove last new line

                Console.WriteLine("After task create CSV");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        #region U/I CONTROL ENABLE/DISABLE
        // While reading from db disable all buttons
        private void DisableUIControl()
        {
            // Tab energy
            buttonGetLocations.Enabled = false;
            buttonProcessSelected.Enabled = false;
            buttonProcessSelectedFast.Enabled = false;
            buttonUpdate.Enabled = false;
            buttonSelectAll.Enabled = false;
            buttonDeselectAll.Enabled = false;
            dateTimePickerStart.Enabled = false;
            dateTimePickerEnd.Enabled = false;

            // Tab data integrity
            buttonSelectAllDataIntegrity.Enabled = false;
            buttonDeselectAllDataIntegrity.Enabled = false;
            buttonGetLocationsDataIntegrity.Enabled = false;
            dateTimePickerStartDataIntegrity.Enabled = false;
            dateTimePickerEndDataIntegrity.Enabled = false;
            buttonProcess.Enabled = false;

        }
        // After db read, enable all buttons
        private void EnableUIControl()
        {
            buttonGetLocations.Enabled = true;
            buttonProcessSelected.Enabled = true;
            buttonProcessSelectedFast.Enabled = true;
            buttonUpdate.Enabled = true;
            buttonSelectAll.Enabled = true;
            buttonDeselectAll.Enabled = true;
            dateTimePickerStart.Enabled = true;
            dateTimePickerEnd.Enabled = true;

            // Tab data integrity
            buttonSelectAllDataIntegrity.Enabled = true;
            buttonDeselectAllDataIntegrity.Enabled = true;
            buttonGetLocationsDataIntegrity.Enabled = true;
            dateTimePickerStartDataIntegrity.Enabled = true;
            dateTimePickerEndDataIntegrity.Enabled = true;
            buttonProcess.Enabled = true;
        }

        public void ErrorCallback()
        {
            Console.WriteLine("Error callback MainForm");
            EnableUIControl();
        }

        public void SuccessCallback()
        {
            Console.WriteLine("Succsess callback MainForm");
            EnableUIControl();
        }

        #endregion

        #region CONTEXT MENU STRIP (right click)
        private void consoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormCustomConsole.isActive == true) // Only open form if it's not active
            {
                return;
            }
            FormCustomConsole fc = new FormCustomConsole();
            fc.Show();
        }




        #endregion


        private class CustomXmlDatePair
        {
#warning REMOVE AFTER TESTING
            public DateTime start;
            public DateTime end;

            public string startHeader;
            public string endHeader;

            public CustomXmlDatePair(DateTime start, DateTime end)
            {
                this.start = start;
                this.end = end;

                this.startHeader = HelperClass.ConvertToNtpmCustomXmlTime(this.start);
                this.endHeader = HelperClass.ConvertToNtpmCustomXmlTime(this.end);
            }

            /// <summary>
            /// Splits date if period is too long, to smaller date fragments which Custom.xml request can handle
            /// </summary>
            /// <param name="start"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            public static List<CustomXmlDatePair> FragmentDate(DateTime start, DateTime end)
            {
#warning REMOVE AFTER TESTING
                var list = new List<CustomXmlDatePair>();
                TimeSpan ts = end.Subtract(start);
                if (ts <= ConfigClass.maxTimePeriod)
                {
                    // If period is not too big, dont fragment it
                    CustomXmlDatePair xdate = new CustomXmlDatePair(start, end);
                    list.Add(xdate);
                    return list;
                }

                // It needs fragmention :(

                DateTime tempEnd;
                DateTime tempStart = start;
                TimeSpan timeOffset = new TimeSpan(0, 0, 15, 0, 0); // 15min

                while (ts >= ConfigClass.maxTimePeriod)
                {
                    tempEnd = tempStart.Add(ConfigClass.maxTimePeriod);
                    CustomXmlDatePair xdate = new CustomXmlDatePair(tempStart, tempEnd);
                    list.Add(xdate);
                    tempStart = tempEnd.AddMinutes(timeOffset.TotalMinutes); // Update tempStart
                    ts = ts.Subtract(ConfigClass.maxTimePeriod.Add(timeOffset)); // Update total timespan
                }

                if (ts <= TimeSpan.Zero)
                {
                    // If nothing left return current list
                    return list;
                }

                // There is still more :(
                // so farm tempStart and tempEnd are pointing to save time
                CustomXmlDatePair finalXdate = new CustomXmlDatePair(tempStart, tempStart.Add(ts));
                list.Add(finalXdate);

                return list;
            }
        }

    }
}
