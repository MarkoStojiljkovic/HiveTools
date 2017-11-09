using HiveTools.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.Diagnostics;
using System.Xml.Linq;

namespace HiveTools
{
    public partial class DataIntegrityGUIForm : Form, ICancelableAsync
    {
        private DataClass dataPtr; // Reference to currently selected DataClass row in allData[]
        static string username = "";
        static string password = "";

        public DataIntegrityGUIForm()
        {
            InitializeComponent();
        }


        public DataIntegrityGUIForm(DataClass dPtr)
        {
            dataPtr = dPtr;
            //Console.WriteLine("DataIntegrityGUIForm ptr = allDataPtr: {0}", Object.ReferenceEquals(ptr.data[4], MainForm.allDataPtr[9].data[4]));
            InitializeComponent();
            DateTime startTime = dataPtr.startDate;
            // Name this form based on ID
            this.Text = "ID = " + dataPtr.deviceID.ToString();
            // Fill pointer to DataClass
            // Data will take numOfData number for every 15min value
            string[] data = ProcessData(dataPtr); // This returns number of reading for every time stamp (15min)

            // Calculate days of data
            int numOfRows = data.Length / 96 - 1;
            if (numOfRows < 0)
            {
                MessageBox.Show("Not enough data");
                return;
            }
            int tempIncrement = 0;
            while (numOfRows >= 0) // Add rows based on data
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[tempIncrement++].Cells[0].Value = startTime.ToString("dd/MM/yyyy");
                startTime = startTime.AddDays(1);
                numOfRows--;
            }

            int currentIndex = 0;
            int currentColumn = 1;
            int currentRow = 0;

            // Fill all cells
            while (currentIndex < data.Length)
            {
                if (currentColumn == 97)
                {
                    currentColumn = 1;
                    currentRow++;
                }

                string value = data[currentIndex];
                // This is what will be shown in cell (number of data arrived for that 15min point)
                dataGridView1.Rows[currentRow].Cells[currentColumn].Value = value;
                if ((currentColumn - 1) % 4 == 0) // Change font size of every 4th column
                {
                    DataGridViewCellStyle norStyle = new DataGridViewCellStyle();
                    norStyle.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold); // 8.25F
                    dataGridView1.Rows[currentRow].Cells[currentColumn].Style = norStyle;
                }
                // Color will be dependent of data point flags
                dataGridView1.Rows[currentRow].Cells[currentColumn].Style.BackColor = GetColorFromValue(dataPtr.data[currentIndex++]);
                currentColumn++;

            }

            // Autoload last typed username and password in aplication session
            textBoxUsername.Text = username;
            textBoxPassword.Text = password;

        }


        private Color GetColorFromValue(Data15MinClass s)
        {

            // Data repeat flag is orange
            if (s.flags.dataRepeatFlag)
            {
                return ConfigClass.colors.colorRepeat;
            }

            // Data duplicate flag is Olive, but CornflowerBlue if zero and Orchid if null
            if (s.flags.duplicateFlag)
            {
                if (s.Values.successfullyParsed)
                {
                    // This means its valid or zero

                    if (FloatHelper.CheckIfZero(s.Values)) // If zero color it with zero color
                    {
                        return ConfigClass.colors.colorZero;
                    }
                    return ConfigClass.colors.colorDuplicate; ; // Otherwise color it with olive
                }
                else
                {
                    // This means its null
                    return ConfigClass.colors.colorNull;
                }

            }

            // Missing flag is OrangeRed
            if (s.flags.missingFlag)
            {
                return ConfigClass.colors.colorMissing;
            }

            // Null flag is Orchid
            if (s.flags.nullFlag)
            {
                return ConfigClass.colors.colorNull;
            }

            // Zero flag is CornflowerBlue
            if (s.flags.zeroFlag)
            {
                return ConfigClass.colors.colorZero;
            }

            // invalid flag is Red
            if (s.flags.invalidFlag)
            {
                return ConfigClass.colors.colorInvalid;
            }

            // If no flags are active that means data is just valid
            return ConfigClass.colors.colorValid;
        }

        private void GenerateLabel(int pos, DateTime startTime)
        {
            int rowHeight = dataGridView1.RowTemplate.Height;
            Point labelLoc = new Point(15, 32);

            int offset = pos * rowHeight;
            labelLoc.Offset(0, offset);

            Label timeLabel = new Label();
            timeLabel.Location = labelLoc;
            startTime = startTime.AddDays(pos);
            timeLabel.Text = startTime.ToString("dd/MM/yyyy");
            this.Controls.Add(timeLabel);
        }

        private string[] ProcessData(DataClass s)
        {
            StringBuilder sb = new StringBuilder();


            foreach (Data15MinClass item in s.data)
            {
                sb.Append(item.numberOfPoints + "|");
            }

            sb.Remove(sb.Length - 1, 1); // Remove last "|"
            string res = sb.ToString();


            return res.Split('|');
        }

        #region STRIPMENU ITEMS
        private void showStatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGUIStats sf = new FormGUIStats(dataPtr);
            sf.Show();
        }

        /// <summary>
        /// Remove duplicates for all 15min periods, duplicates are removed if all measurment values are same
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeDuplicatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region OLD WAY
            //List<string> commands = new List<string>(); // List of database commands
            //// Loop thru every 15min point, and generate remove database queries for true duplicate data 

            //foreach (Data15MinClass d15 in dataPtr.data)
            //{
            //    if (d15.numberOfPoints < 2) 
            //    {
            //        continue; // If point have less than 2 values ignore
            //    }

            //    var uniqueValues = new List<FloatValues>(); // Create new list which will hold unique values
            //    var all15minValues = d15.GetAllValues(); // Some 15min point can have multiple values, duplicates

            //    // First value is always unique
            //    uniqueValues.Add(all15minValues[0]);

            //    // Generate remove command for each duplicated value, leave values that are different in float values
            //    for (int i = 1; i < d15.GetAllValues().Count; i++) // Skip first
            //    {
            //        bool equalFlag = false; // This will be set to true if any of data point is true duplicate
            //        foreach (var item2 in uniqueValues)
            //        {
            //            if (FloatHelper.CompareDuplicateValues(item2, all15minValues[i])) // Check if they are equal
            //            {
            //                equalFlag = true;
            //                // Generate command for row removal from database and add it to list of commands
            //                commands.Add(GenerateDatabaseRemoveCommand(all15minValues[i]));
            //            }

            //        }

            //        if (!equalFlag)
            //        {
            //            // This garantiues unique value
            //            uniqueValues.Add(all15minValues[i]);
            //        }
            //        equalFlag = false;
            //    }
            //}
            #endregion old way
            List<string> commands = CreateDBRemoveCommands();
            // Make dialog box, for connfirming deletion
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete: " + commands.Count + " items from database?", "Warning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                return;
            }
            // Yes was selected

            Action action = () =>
            {
                //Console.WriteLine("Starting request");
                foreach (var item in commands)
                {
                    FormCustomConsole.WriteLine("Sending command: " + item);

                    try
                    {
#warning Deleteing items from database can be configured here
                        DatabaseControllerClass.SendCommandToDatabase(DatabaseSettingsClass.ReadConnectionString(), item);
                        FormCustomConsole.WriteLine("Successfully executed command:" + item);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            };

            ICancelableForm cf = new FormWaitMarquee(this);
            HelperInterruptibleWorker worker = new HelperInterruptibleWorker(this, cf);
            worker.ExecuteTask(action);

        }

        /// <summary>
        /// Create and return commands for deleting duplicate items from database 
        /// </summary>
        /// <returns></returns>
        private List<string> CreateDBRemoveCommands()
        {
            List<string> commands = new List<string>(); // List of database commands

            foreach (Data15MinClass d15 in dataPtr.data)
            {
                if (d15.numberOfPoints < 2)
                {
                    continue; // If point have less than 2 values ignore
                }

                var uniqueValues = new List<FloatValues>(); // Create new list which will hold unique values
                var all15minValues = d15.GetAllValues(); // Some 15min point can have multiple values, duplicates

                // First value is always unique
                uniqueValues.Add(all15minValues[0]);

                // Generate remove command for each duplicated value, leave values that are different in float values
                for (int i = 1; i < d15.GetAllValues().Count; i++) // Skip first
                {
                    bool equalFlag = false; // This will be set to true if any of data point is true duplicate
                    foreach (var item2 in uniqueValues)
                    {
                        if (FloatHelper.CompareDuplicateValues(item2, all15minValues[i])) // Check if they are equal
                        {
                            equalFlag = true;
                            // Generate command for row removal from database and add it to list of commands
                            commands.Add(GenerateDatabaseRemoveCommand(all15minValues[i]));
                        }

                    }

                    if (!equalFlag)
                    {
                        // This garantiues unique value
                        uniqueValues.Add(all15minValues[i]);
                    }
                    equalFlag = false;
                }
            }

            return commands;
        }

        async private void downloadCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cells = dataGridView1.SelectedCells;
            Console.WriteLine("Number of cells: " + dataGridView1.SelectedCells.Count);

            List<int> indexes = new List<int>();

            if (dataGridView1.SelectedCells.Count < 0) // Check if nothing is selected
            {
                return;
            }
            // ------------- FIND FIRST AND LAST (SELECTED) INDEX IN DATAGRID
            foreach (DataGridViewTextBoxCell item in cells)
            {
                int temp = item.RowIndex * 96 + item.ColumnIndex - 1; // Convert to data array index
                if (temp < 0) // Check if header is selected
                {
                    return;
                }
                indexes.Add(temp);
            }

            // Find min and max indexes
            int indexMin = indexes.First();
            int indexMax = indexes.First();

            foreach (var item in indexes)
            {
                if (indexMin > item)
                {
                    indexMin = item;
                }
            }

            foreach (var item in indexes)
            {
                if (indexMax < item)
                {
                    indexMax = item;
                }
            }

            // ---------- INDEXES FOUND

            //DEBUG
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            // debug

            // Now select time period based on min-max
            var startDate = dataPtr.data[indexMin].date;
            var endDate = dataPtr.data[indexMax].date;

            // Get username and password from textboxes
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;


            // Create array of date pairs if xml data needs to be fragmented
            List<CustomXmlDatePair> datePeriodsList = CustomXmlDatePair.FragmentDate(startDate, endDate);
            
            // ----- Instantiate ntpm object
            string ip;
            try
            {
                // Get IP and Port from database
                ip = GetIPAndPort();
            }
            catch (Exception)
            {
                MessageBox.Show("Couldn't get IP and Port from database");
                return; // abort
            }

            // ----------------------------------- Get and parse report.xml
            string reportRes;
            XDocument reportDoc;

            try
            {
                reportRes = await NTPMControllerClass.GetReport(ip); // Get xml report from NTPM
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

            FormCustomConsole.WriteLine("StartTime: " + headerStartTime);
            FormCustomConsole.WriteLine("EndTime: " + headerEndTime);


            // now we only need to fill values with each row

            // --------------------------------Get and parse measurement.xml

            List<XDocument> measurementDoc = new List<XDocument>();
            List<string> measurementRows = new List<string>();

            foreach (var datePeriod in datePeriodsList)
            {
                Console.WriteLine("Parsing period: " + datePeriod.startHeader + "---" + datePeriod.endHeader);
                await HelperRetryClass.DoWithRetryAsync(async () =>
                {
                    FormCustomConsole.WriteLine("Loading: " + datePeriod.startHeader + "---" + datePeriod.endHeader);
                    List<KeyValuePair<string, string>> pairs = NTPMControllerClass.GetPairsForCustomXmlPostMethod(datePeriod.startHeader, datePeriod.endHeader, username, password);
                    string measurementRes = await HTTPClientClass.PostRequest("http://" + ip + "/custom.xml", pairs);
                    XDocument tempMeasure = XDocument.Parse(measurementRes);
                    NTPMControllerClass.CheckErrorTag(tempMeasure);
                    string measureRow = NTPMControllerClass.CreateCSVMeasurementRow(tempMeasure);
                    // At this point, download and parsing is ok
                    measurementRows.Add(measureRow);
                    measurementDoc.Add(tempMeasure);
                },
                TimeSpan.FromSeconds(1));
            }

            // now form whole string (HEADER + TAGS + MEASUREMENT ROWS)
            StringBuilder sFinal = new StringBuilder();
            sFinal.Append(csvHeader);
            foreach (var item in measurementRows)
            {
                sFinal.Append(item);
                sFinal.Append("\r\n");
            }
            sFinal.Remove(sFinal.Length - 2, 2); // Remove last new line


            // Write elapsed time
            stopWatch.Stop();
            FormCustomConsole.WriteLine("XML fetching operation finished in: " + (stopWatch.ElapsedMilliseconds / 1000).ToString() + "seconds");


            // --------------------------------------- Finaly save file
            string fileName = XmlHelper.FindFirstDescendant("hostname", reportDoc).Trim();
            string fileMac = XmlHelper.FindFirstDescendant("mac", reportDoc).Replace(':', '-');
            string fileType = "by_15min";

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel File|*.csv";
            // Create file name (based on downloaded data)
            sfd.FileName = fileName + "_" + fileMac + "_" + headerStartTime + "_" + headerEndTime + "_" + fileType;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = sfd.FileName;
                StreamWriter sw = new StreamWriter(File.Create(path));
                sw.Write(sFinal.ToString());
                sw.Dispose();
            }

        }

        private void downloadCSVAutoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #endregion STRIPMENU ITEMS

        private string GenerateDatabaseRemoveCommand(FloatValues point)
        {
            return "DELETE FROM DATA_HISTORY WHERE ID = " + point.uniqueID.ToString();
        }

        private string GetIPAndPort()
        {
            string res;

            if (checkBoxDefPort.Checked)
            {
                // Default port 80 will be used
                res = NTPMControllerClass.FindIPAndPortFromId(dataPtr, '|').Split(';')[0];
            }
            else
            {
                // Infer port from database
                res = NTPMControllerClass.FindIPAndPortFromId(dataPtr, '|');
                res = res.Replace(';', ':');
            }
            return res;
        }

        #region Callback from ICancelableAsync intefrace
        public void ErrorCallback()
        {
            // Nothing will happen for now, thread will continue to execute all commands :(
            Console.WriteLine("ErrorCallback from DataIntegrityForm");
            return;
        }

        public void SuccessCallback()
        {
            // This method should be executed by owner thread only
            if (this.InvokeRequired)
            {
                Action d = new Action(SuccessCallback);
                this.Invoke(d);
            }
            else
            {
                Console.WriteLine("Replace dataPoint and close DataintegrityGUIForm");
                // So far database rows are deleted, now update current form view and update DataClass with new modified values
                MainForm.mainFormRef.ReplaceDataPoint(dataPtr.deviceID);
                this.Close();
            }
            return;
        }

        #endregion

        #region MOUSE HOVER EVENTS

        /// <summary>
        /// Read data from selected NTPM device (NOT USED RIGHT NOW)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Equals("0"))
            //{
            //    int positionInDataGrid = e.RowIndex * 96 + e.ColumnIndex;
            //    Console.WriteLine(positionInDataGrid);
            //}

            int positionInArray = e.RowIndex * 96 + (e.ColumnIndex - 1);
            //Console.WriteLine(positionInDataGrid);

            //NTPMControllerClass ntpm = new NTPMControllerClass(ptr, pos);

        }



        //private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        //{
        //    Console.WriteLine(e.RowIndex + " cell entered");
        //    if (e.RowIndex == -1)
        //    {
        //        return; // Table is not populated yet
        //    }

        //}

        //private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        //{
        //    Console.WriteLine(e.RowIndex + " cell left");
        //}
        #endregion

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string resI, resU;
            if (e.ColumnIndex == 0)
            {
                labelValues.Text = "";
                return;
            }
            int offset = e.RowIndex * 96 + e.ColumnIndex - 1;
            // Check if there are any values
            if (dataPtr.data[offset].flags.missingFlag)
            {
                return;
            }
            if (dataPtr.data[offset].Values.successfullyParsed == false) // Check if null values
            {
                resI = "Ia=NULL Ib=NULL ic=NULL" + "    ";
                resU = "Vab=NULL Vbc=NULL Vca=NULL";
            }
            else
            {
                // Data is valid
                FloatValues f = dataPtr.data[offset].Values;
                resI = "Ia=" + f.ia.ToString("0.000") + "  " + "Ib=" + f.ib.ToString("0.000") + "  " + "Ic=" + f.ic.ToString("0.000") + "    ";
                resU = "Vab=" + f.vab.ToString("0.000") + "  " + "Vbc=" + f.vbc.ToString("0.000") + "  " + "Vca=" + f.vca.ToString("0.000");
            }

            labelValues.Text = resI + resU;

        }

        /// <summary>
        /// Save username and password when form is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataIntegrityGUIForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            username = textBoxUsername.Text;
            password = textBoxPassword.Text;
        }


        private class CustomXmlDatePair // Helper class for holding values when sending CustomXml requests
        {
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
                TimeSpan timeOffset = ConfigClass.stepPeriod;

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
                // so farm tempStart and tempEnd are pointing to same time
                CustomXmlDatePair finalXdate = new CustomXmlDatePair(tempStart, tempStart.Add(ts));
                list.Add(finalXdate);

                return list;
            }
        }

    }
}
