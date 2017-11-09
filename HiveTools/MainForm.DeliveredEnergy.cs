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

namespace HiveTools
{
    partial class MainForm
    {
        #region DELIVERED ENERGY

        //private const int DAYS_TO_CHECK = 6; // This is number of days to check + 1
        private const string VALUE_MISSING = "VALUE_MISSING";
        private const string VALUE_MISSING_FAST = "VALUE_MISSING_FAST";



        #region TOOL STRIP ITEMS

        private void deliveredEnergyToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // If there are no rows, ignore this request
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel File|*.csv";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = sfd.FileName;
                StreamWriter sw = new StreamWriter(File.Create(path));
                sw.Write(GetDeliveredEnergyRowValues());
                sw.Dispose();
            }
        }

        #endregion


        #region BUTTON HANDLERS
        private void buttonGetLocations_Click(object sender, EventArgs e)
        {
            string res = ""; // it will hold data from database
            // "SELECT ID,NAME,MAC FROM device"
            // SELECT READ_TIME FROM DATA_HISTORY WHERE LOCATION_ID=30 AND ROWNUM <= 50;
            // SELECT IPAL FROM DATA_HISTORY WHERE LOCATION_ID=65 AND ROWNUM <= 50 AND READ_TIME >= TO_DATE('2015/11/01 8:15', 'YYYY/MM/DD HH:MI') AND READ_TIME <= TO_DATE('2015/11/01 8:16', 'YYYY/MM/DD HH:MI');
            // SELECT EPTC,EQTC FROM DATA_HISTORY WHERE LOCATION_ID=37 AND READ_TIME >= TO_DATE('2017/05/1 0:0', 'YYYY/MM/DD HH24:MI') AND READ_TIME <= TO_DATE('2017/05/1 0:1', 'YYYY/MM/DD HH24:MI') AND ROWNUM <= 1
            // SELECT DEVICE_ID, DESCRIPTION FROM LOCATION ORDER BY DEVICE_ID ASC;
            char delim = '|';
            try
            {
                res = DatabaseControllerClass.FetchValuesFromDatabase(DatabaseSettingsClass.ReadConnectionString(), "SELECT DEVICE_ID, NAME, DESCRIPTION FROM LOCATION ORDER BY NAME ASC", delim);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            dataGridView1.Rows.Clear();
            // Filter res first from bad values (new lines)
            res = res.Replace("\r\n", "");
            // Split monolithic string to data lines
            //string[] dataLines = res.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            string[] dataLines = res.Split(delim);
            // We need to filter dataLines from bad inputs... APARENTLY
            StringBuilder sb = new StringBuilder();
            foreach (var item in dataLines)
            {
                if (item.Equals(""))
                {
                    continue;
                }
                sb.Append(item + "|");
            }
            dataLines = sb.ToString().Split('|'); // dataLines should be filtered now from empty lines

            string[] rowInfo;
            // Now fill data grid based on results
            for (int i = 0; i < dataLines.Length - 1; i++)
            {
                rowInfo = dataLines[i].Split(';');
                dataGridView1.Rows.Add(false, rowInfo[0], rowInfo[1], rowInfo[2], null);
            }
        }


        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                item.Cells[0].Value = true;
            }
        }

        private void buttonDeselectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                item.Cells[0].Value = false;
            }
        }

        private void buttonProcessSelected_Click(object sender, EventArgs e)
        {
            DisableUIControl();
            ICancelableForm fw = new FormWait(this, GetDeliveredEnergyActiveRowNumber());
            Action action = () =>
            {
                TaskSlow(fw);
            };

            HelperInterruptibleWorker worker = new HelperInterruptibleWorker(this, fw);
            worker.ExecuteTask(action);
        }


        private void buttonProcessSelectedFast_Click(object sender, EventArgs e)
        {
            DisableUIControl();
            ICancelableForm fw = new FormWait(this, GetDeliveredEnergyActiveRowNumber());
            Action action = () =>
            {
                TaskFast(fw);
            };

            HelperInterruptibleWorker worker = new HelperInterruptibleWorker(this, fw);
            worker.ExecuteTask(action);
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            DisableUIControl();
            ICancelableForm fw = new FormWait(this, GetDeliveredEnergyActiveRowNumber());
            Action action = () =>
            {
                TaskUpdate(fw);
            };

            HelperInterruptibleWorker worker = new HelperInterruptibleWorker(this, fw);
            worker.ExecuteTask(action);
        }
        
        
        #endregion BUTTON HANDLERS

        private int GetDeliveredEnergyActiveRowNumber() // Get number of selected rows
        {
            int x = 0;
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if ((bool)item.Cells[0].Value == true)
                {
                    x++;
                }
            }
            return x;
        }

        private void TaskFast(ICancelableForm fw) // Task to fetch fast from database
        {
            GetValuesFromDBFast(fw);
        }

        private void TaskSlow(ICancelableForm fw) // Task to fetch slow from database
        {
            GetValuesFromDB(fw);
        }

        private void TaskUpdate(ICancelableForm fw) // Task when update button is pressed
        {
            UpdateValuesFromDB(fw);
        }
        
        #region DATETIMEPICKER GET METHODS
        private string GetStartYear()
        {
            return dateTimePickerStart.Value.ToString("yyyy");
        }
        private string GetEndYear()
        {
            DateTime temp = dateTimePickerEnd.Value;

            return temp.AddMonths(1).ToString("yyyy");
        }
        private string GetStartMonth()
        {
            return dateTimePickerStart.Value.ToString("MM");
        }
        private string GetEndMonth()
        {
            DateTime temp = dateTimePickerEnd.Value;
            return temp.AddMonths(1).ToString("MM");
        }
        #endregion

        // Format energy values from db 
        private string FormatEnergyValues(string start, string end, out bool warning)
        {
            warning = false; // This warns if active energy is negative
            //resStart = "787895872,5632;226675232,3328";
            //resEnd = "829181056,112640;242267360,2816";
            string activeEnergyStart;
            string reactiveEnergyStart;
            string activeEnergyEnd;
            string reactiveEnergyEnd;

            float activeEnergyStartFloat;
            float reactiveEnergyStartFloat;
            float activeEnergyEndFloat;
            float reactiveEnergyEndFloat;

            int index = start.IndexOf(';');
            activeEnergyStart = start.Substring(0, index++);
            reactiveEnergyStart = start.Substring(index, start.Length - index);

            index = end.IndexOf(';');
            activeEnergyEnd = end.Substring(0, index++);
            reactiveEnergyEnd = end.Substring(index, end.Length - index);

            // Change , to . so parser knows what to do
            activeEnergyStart = activeEnergyStart.Replace(',', '.');
            reactiveEnergyStart = reactiveEnergyStart.Replace(',', '.');
            activeEnergyEnd = activeEnergyEnd.Replace(',', '.');
            reactiveEnergyEnd = reactiveEnergyEnd.Replace(',', '.');

            // Now convert to float
            try
            {
                // Will throw exception if something is wrong
                activeEnergyStartFloat = float.Parse(activeEnergyStart, CultureInfo.InvariantCulture);
                reactiveEnergyStartFloat = float.Parse(reactiveEnergyStart, CultureInfo.InvariantCulture);

                activeEnergyEndFloat = float.Parse(activeEnergyEnd, CultureInfo.InvariantCulture);
                reactiveEnergyEndFloat = float.Parse(reactiveEnergyEnd, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                warning = true;
                return "";
            }

            // Check for negative or zero active power
            if (activeEnergyStartFloat <= 0f || activeEnergyEndFloat <= 0f)
            {
                warning = true;
            }

            // Force to kW
            float tempResultActive = (activeEnergyEndFloat - activeEnergyStartFloat) / 1000;
            // Check if active energy difference was negative or zero this month
            if (tempResultActive <= 0f)
            {
                warning = true;
            }

            float tempResultReactive = (reactiveEnergyEndFloat - reactiveEnergyStartFloat) / 1000;

            // Check for zero difference 
            if (tempResultActive > -0.0001f && tempResultActive < 0.0001f)
            {
                warning = true;
            }

            if (tempResultReactive > -0.0001f && tempResultReactive < 0.0001f)
            {
                warning = true;
            }

            //return "Wa=" + tempResultActive.ToString("0") + " [kW]" + "/" + "Wr=" + tempResultReactive.ToString("0") + " [kVAr]";
            return tempResultActive.ToString("0") + "/" + tempResultReactive.ToString("0");
        }
        
        // Convert selected rows to string file which will be converted to csv
        private string GetDeliveredEnergyRowValues()
        {
            string delimiter = ",";
            StringBuilder sb = new StringBuilder();

            // If row is enabled, process it
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(item.Cells[0].Value) == true) // if checkbox is enabled
                {
                    if (item.Cells[4].Value == null) // Ignore if there is no data
                    {
                        continue;
                    }
                    
                    // First check if value is missing
                    if (item.Cells[4].Value.ToString().Equals(VALUE_MISSING_FAST) || item.Cells[4].Value.ToString().Equals(VALUE_MISSING))
                    {
                        sb.Append(item.Cells[2].Value + delimiter + item.Cells[3].Value + delimiter + item.Cells[4].Value + delimiter + item.Cells[5].Value + delimiter
                      + item.Cells[6].Value + delimiter + item.Cells[7].Value + "\r\n");
                    }
                    else
                    {
                        // Value is not missing
                        // Wa=7806 [kW];Wr=2871 [kVAr]; PRIMER
                        string active = item.Cells[4].Value.ToString();
                        //active = active.Substring(active.IndexOf('=') + 1, active.IndexOf('[') - 2 - active.IndexOf('='));

                        string reactive = item.Cells[5].Value.ToString();
                        //reactive = reactive.Substring(reactive.IndexOf('=') + 1, reactive.IndexOf('[') - 2 - reactive.IndexOf('='));

                        sb.Append(item.Cells[2].Value + delimiter + item.Cells[3].Value + delimiter + active + delimiter + reactive + delimiter
                           + item.Cells[6].Value + delimiter + item.Cells[7].Value + "\r\n");
                    }


                }
            }

            sb.Remove(sb.Length - 2, 2); // Remove last new line

            return sb.ToString();
        }

        #region DATABASE PROCESS BUTTONS
        // Get values from DB slow way (search for a given number of days)
        private void GetValuesFromDB(ICancelableForm fw)
        {
            string oracleDateStart;
            string oracleDateStartOffset;
            string oracleDateEnd;
            string oracleDateEndOffset;


            string dateStringTimestamp = "";
            string querryStart;
            string querryEnd;
            string resStart = "";
            string resEnd = "";

            bool startValid = false;
            bool endValid = false;

            // If row is enabled, process it
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(item.Cells[0].Value) == true)
                {
                    // Selected element is enabled
                    // First intialize time
                    TimeFormatterClass tf = new TimeFormatterClass();
                    do
                    {
                        // Form querry string for time
                        try
                        {
                            dateStringTimestamp = GetStartYear() + "/" + GetStartMonth() + "/" + tf.GetDay() + " " + tf.GetHour() + ":" + tf.GetMin() + "-" + GetEndYear() + "/" + GetEndMonth() + "/" + tf.GetDay() + " " + tf.GetHour() + ":" + tf.GetMin();
                            oracleDateStart = HelperClass.ConvertToOracleTime(GetStartYear(), GetStartMonth(), tf.GetDay(), tf.GetHour(), tf.GetMin());
                            oracleDateStartOffset = HelperClass.ConvertToOracleTime(GetStartYear(), GetStartMonth(), tf.GetDay(), tf.GetHour(), (Convert.ToInt32(tf.GetMin()) + 1).ToString());

                            oracleDateEnd = HelperClass.ConvertToOracleTime(GetEndYear(), GetEndMonth(), tf.GetDay(), tf.GetHour(), tf.GetMin());
                            oracleDateEndOffset = HelperClass.ConvertToOracleTime(GetEndYear(), GetEndMonth(), tf.GetDay(), tf.GetHour(), (Convert.ToInt32(tf.GetMin()) + 1).ToString());
                        }
                        catch (Exception)
                        {
                            break; // This means all time periods are tried, abort this
                        }
                        
                        querryStart = "SELECT EPTC,EQTC FROM DATA_HISTORY WHERE DEVICE_ID=" + item.Cells[1].Value + " AND READ_TIME >= " + oracleDateStart + " AND READ_TIME <= " + oracleDateStartOffset + "AND ROWNUM <= 1";
                        querryEnd = "SELECT EPTC,EQTC FROM DATA_HISTORY WHERE DEVICE_ID=" + item.Cells[1].Value + " AND READ_TIME >= " + oracleDateEnd + " AND READ_TIME <= " + oracleDateEndOffset + "AND ROWNUM <= 1";
                        char delim = '|';
                        try
                        {
                            resStart = DatabaseControllerClass.FetchValuesFromDatabase(DatabaseSettingsClass.ReadConnectionString(), querryStart, delim);
                            resEnd = DatabaseControllerClass.FetchValuesFromDatabase(DatabaseSettingsClass.ReadConnectionString(), querryEnd, delim);
                        }
                        catch (Exception)
                        {
                            Task.Run(() => MessageBox.Show("Timeout for device ID:" + item.Cells[1].Value));
                            return;
                        }
                        // Now we should have a results, but we must check for validity
                        startValid = HelperClass.CheckDataValidity(resStart);
                        endValid = HelperClass.CheckDataValidity(resEnd);

                        if (!(startValid && endValid)) // If one of each is  not valid calculate next time
                        {
                            tf.CalculateNextValues();
                            if (Convert.ToInt32(tf.GetDay()) == ConfigClass.other.daysToCheck) // If we reached maximum days
                            {
                                break;
                            }
                        }
                    } while (!(startValid && endValid)); // Will run until both values are valid or unitl end is reached

                    if (fw.AlreadyClosed)
                    {
                        return;
                    }
                    fw.ProgresStep(); // Update progress bar
                    

                    // Result have active and reactive component
                    if (startValid && endValid == true)
                    {
                        // We have both values, lets process them
                        bool warning; // will warn if negative active power
                        string valueRes = FormatEnergyValues(resStart, resEnd, out warning);
                        string[] temp = valueRes.Split('/');
                        item.Cells[4].Value = temp[0].Trim();
                        item.Cells[5].Value = temp[1].Trim();
                        string[] tempTime = dateStringTimestamp.Split('-');
                        item.Cells[6].Value = tempTime[0].Trim();
                        item.Cells[7].Value = tempTime[1].Trim();
                        if (warning == true)
                        {
                            item.Cells[4].Style.BackColor = Color.Yellow;
                            item.Cells[5].Style.BackColor = Color.Yellow;
                            item.Cells[6].Style.BackColor = Color.Yellow;
                            item.Cells[7].Style.BackColor = Color.Yellow;
                        }
                        else
                        {
                            item.Cells[4].Style.BackColor = Color.White;
                            item.Cells[5].Style.BackColor = Color.White;
                            item.Cells[6].Style.BackColor = Color.White;
                            item.Cells[7].Style.BackColor = Color.White;
                        }

                    }
                    else
                    {
                        item.Cells[4].Value = VALUE_MISSING;
                        item.Cells[4].Style.BackColor = Color.Red;
                        item.Cells[5].Value = VALUE_MISSING;
                        item.Cells[5].Style.BackColor = Color.Red;
                        item.Cells[6].Value = VALUE_MISSING;
                        item.Cells[6].Style.BackColor = Color.Red;
                        item.Cells[7].Value = VALUE_MISSING;
                        item.Cells[7].Style.BackColor = Color.Red;
                    }

                }
                else // Cell is not selected, erase value
                {
                    item.Cells[4].Value = null;
                    item.Cells[4].Style.BackColor = Color.White;

                    item.Cells[5].Value = null;
                    item.Cells[5].Style.BackColor = Color.White;

                    item.Cells[6].Value = null;
                    item.Cells[6].Style.BackColor = Color.White;

                    item.Cells[7].Value = null;
                    item.Cells[7].Style.BackColor = Color.White;
                }

            }
        }

        // Get values from DB fast way, will search only first day at 0:0
        private void GetValuesFromDBFast(ICancelableForm fw)
        {
            string oracleDateStart;
            string oracleDateStartOffset;
            string oracleDateEnd;
            string oracleDateEndOffset;


            string dateStringTimestamp = "";
            string querryStart;
            string querryEnd;
            string resStart = "";
            string resEnd = "";

            bool startValid = false;
            bool endValid = false;


            // Show loading screen in new thread
            //this.BeginInvoke((Action)delegate {

            //    var form = new FormWait();
            //    form.Show();
            //});

            // If row is enabled, process it
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(item.Cells[0].Value) == true)
                {
                    // Selected element is enabled
                    // First intialize time
                    TimeFormatterClass tf = new TimeFormatterClass();
                    do
                    {

                        // Form querry string for time
                        try
                        {
                            dateStringTimestamp = GetStartYear() + "/" + GetStartMonth() + "/" + tf.GetDay() + " " + tf.GetHour() + ":" + tf.GetMin() + "-" + GetEndYear() + "/" + GetEndMonth() + "/" + tf.GetDay() + " " + tf.GetHour() + ":" + tf.GetMin();
                            //dateStringTimestamp = GetStartYear() + "/" + GetStartMonth() + "/" + tf.GetDay() + " - " + tf.GetHour() + ":" + tf.GetMin() + "-" + GetEndYear() + "/" + GetEndMonth() + "/" + tf.GetDay() + " " + tf.GetHour() + ":" + tf.GetMin();
                            oracleDateStart = HelperClass.ConvertToOracleTime(GetStartYear(), GetStartMonth(), tf.GetDay(), tf.GetHour(), tf.GetMin());
                            oracleDateStartOffset = HelperClass.ConvertToOracleTime(GetStartYear(), GetStartMonth(), tf.GetDay(), tf.GetHour(), (Convert.ToInt32(tf.GetMin()) + 1).ToString());

                            oracleDateEnd = HelperClass.ConvertToOracleTime(GetEndYear(), GetEndMonth(), tf.GetDay(), tf.GetHour(), tf.GetMin());
                            oracleDateEndOffset = HelperClass.ConvertToOracleTime(GetEndYear(), GetEndMonth(), tf.GetDay(), tf.GetHour(), (Convert.ToInt32(tf.GetMin()) + 1).ToString());
                        }
                        catch (Exception)
                        {
                            break; // This means all time periods are tried, abort this
                        }


                        querryStart = "SELECT EPTC,EQTC FROM DATA_HISTORY WHERE DEVICE_ID=" + item.Cells[1].Value + " AND READ_TIME >= " + oracleDateStart + " AND READ_TIME <= " + oracleDateStartOffset + "AND ROWNUM <= 1";
                        querryEnd = "SELECT EPTC,EQTC FROM DATA_HISTORY WHERE DEVICE_ID=" + item.Cells[1].Value + " AND READ_TIME >= " + oracleDateEnd + " AND READ_TIME <= " + oracleDateEndOffset + "AND ROWNUM <= 1";
                        char delim = '|';
                        try
                        {
                            resStart = DatabaseControllerClass.FetchValuesFromDatabase(DatabaseSettingsClass.ReadConnectionString(), querryStart, delim);
                            resEnd = DatabaseControllerClass.FetchValuesFromDatabase(DatabaseSettingsClass.ReadConnectionString(), querryEnd, delim);
                        }
                        catch (Exception)
                        {
                            Task.Run(() => MessageBox.Show("Timeout for device ID:" + item.Cells[1].Value));
                            return;
                        }
                        // Now we should have a results, but we must check for validity
                        startValid = HelperClass.CheckDataValidity(resStart);
                        endValid = HelperClass.CheckDataValidity(resEnd);

                        if (!(startValid && endValid)) // If one of each is  not valid abort
                        {
                            break;
                        }

                    } while (!(startValid && endValid)); // Will run until both values are valid or unitl end is reached

                    if (fw.AlreadyClosed)
                    {
                        return;
                    }
                    fw.ProgresStep(); // Update progress bar
                    
                    
                    // Result have active and reactive component
                    if (startValid && endValid == true)
                    {
                        // We have both values, lets process them
                        // We have both values, lets process them
                        bool warning; // will warn if negative active power
                        string valueRes = FormatEnergyValues(resStart, resEnd, out warning);
                        string[] temp = valueRes.Split('/');
                        item.Cells[4].Value = temp[0].Trim();
                        item.Cells[5].Value = temp[1].Trim();
                        string[] tempTime = dateStringTimestamp.Split('-');
                        item.Cells[6].Value = tempTime[0].Trim();
                        item.Cells[7].Value = tempTime[1].Trim();
                        if (warning == true)
                        {
                            item.Cells[4].Style.BackColor = Color.Yellow;
                            item.Cells[5].Style.BackColor = Color.Yellow;
                            item.Cells[6].Style.BackColor = Color.Yellow;
                            item.Cells[7].Style.BackColor = Color.Yellow;
                        }
                        else
                        {
                            item.Cells[4].Style.BackColor = Color.White;
                            item.Cells[5].Style.BackColor = Color.White;
                            item.Cells[6].Style.BackColor = Color.White;
                            item.Cells[7].Style.BackColor = Color.White;
                        }

                    }
                    else
                    {
                        item.Cells[4].Value = VALUE_MISSING_FAST;
                        item.Cells[4].Style.BackColor = Color.Orange;
                        item.Cells[5].Value = VALUE_MISSING_FAST;
                        item.Cells[5].Style.BackColor = Color.Orange;
                        item.Cells[6].Value = VALUE_MISSING_FAST;
                        item.Cells[6].Style.BackColor = Color.Orange;
                        item.Cells[7].Value = VALUE_MISSING_FAST;
                        item.Cells[7].Style.BackColor = Color.Orange;
                    }

                }
                else // Cell is not selected, erase value
                {
                    item.Cells[4].Value = null;
                    item.Cells[4].Style.BackColor = Color.White;

                    item.Cells[5].Value = null;
                    item.Cells[5].Style.BackColor = Color.White;

                    item.Cells[6].Value = null;
                    item.Cells[6].Style.BackColor = Color.White;

                    item.Cells[7].Value = null;
                    item.Cells[7].Style.BackColor = Color.White;
                }

            }
        }

        // Update selected rows in slow way while keeping results on other rows
        private void UpdateValuesFromDB(ICancelableForm fw)
        {
            string oracleDateStart;
            string oracleDateStartOffset;
            string oracleDateEnd;
            string oracleDateEndOffset;


            string dateStringTimestamp = "";
            string querryStart;
            string querryEnd;
            string resStart = "";
            string resEnd = "";

            bool startValid = false;
            bool endValid = false;

            // If row is enabled, process it
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(item.Cells[0].Value) == true)
                {
                    // Selected element is enabled
                    // First intialize time
                    TimeFormatterClass tf = new TimeFormatterClass();
                    do
                    {

                        // Form querry string for time
                        try
                        {
                            dateStringTimestamp = GetStartYear() + "/" + GetStartMonth() + "/" + tf.GetDay() + " " + tf.GetHour() + ":" + tf.GetMin() + "-" + GetEndYear() + "/" + GetEndMonth() + "/" + tf.GetDay() + " " + tf.GetHour() + ":" + tf.GetMin();
                            oracleDateStart = HelperClass.ConvertToOracleTime(GetStartYear(), GetStartMonth(), tf.GetDay(), tf.GetHour(), tf.GetMin());
                            oracleDateStartOffset = HelperClass.ConvertToOracleTime(GetStartYear(), GetStartMonth(), tf.GetDay(), tf.GetHour(), (Convert.ToInt32(tf.GetMin()) + 1).ToString());

                            oracleDateEnd = HelperClass.ConvertToOracleTime(GetEndYear(), GetEndMonth(), tf.GetDay(), tf.GetHour(), tf.GetMin());
                            oracleDateEndOffset = HelperClass.ConvertToOracleTime(GetEndYear(), GetEndMonth(), tf.GetDay(), tf.GetHour(), (Convert.ToInt32(tf.GetMin()) + 1).ToString());
                        }
                        catch (Exception)
                        {
                            break; // This means all time periods are tried, abort this
                        }


                        querryStart = "SELECT EPTC,EQTC FROM DATA_HISTORY WHERE DEVICE_ID=" + item.Cells[1].Value + " AND READ_TIME >= " + oracleDateStart + " AND READ_TIME <= " + oracleDateStartOffset + "AND ROWNUM <= 1";
                        querryEnd = "SELECT EPTC,EQTC FROM DATA_HISTORY WHERE DEVICE_ID=" + item.Cells[1].Value + " AND READ_TIME >= " + oracleDateEnd + " AND READ_TIME <= " + oracleDateEndOffset + "AND ROWNUM <= 1";
                        char delim = '|';
                        try
                        {
                            resStart = DatabaseControllerClass.FetchValuesFromDatabase(DatabaseSettingsClass.ReadConnectionString(), querryStart, delim);
                            resEnd = DatabaseControllerClass.FetchValuesFromDatabase(DatabaseSettingsClass.ReadConnectionString(), querryEnd, delim);
                        }
                        catch (Exception)
                        {
                            Task.Run(() => MessageBox.Show("Timeout for device ID:" + item.Cells[1].Value));
                            return;
                        }
                        // Now we should have a results, but we must check for validity
                        startValid = HelperClass.CheckDataValidity(resStart);
                        endValid = HelperClass.CheckDataValidity(resEnd);

                        if (!(startValid && endValid)) // If one of each is  not valid calculate next time
                        {
                            tf.CalculateNextValues();
                            if (Convert.ToInt32(tf.GetDay()) == ConfigClass.other.daysToCheck) // If we reached maximum days
                            {
                                break;
                            }
                        }
                    } while (!(startValid && endValid)); // Will run until both values are valid or unitl end is reached

                    // Update percentage bar and check if aborted
                    if (fw.AlreadyClosed)
                    {
                        return;
                    }
                    fw.ProgresStep(); // Update progress bar
                    

                    // Result have active and reactive component
                    if (startValid && endValid == true)
                    {
                        // We have both values, lets process them
                        bool warning; // will warn if negative active power
                        string valueRes = FormatEnergyValues(resStart, resEnd, out warning);
                        string[] temp = valueRes.Split('/');
                        item.Cells[4].Value = temp[0].Trim();
                        item.Cells[5].Value = temp[1].Trim();
                        string[] tempTime = dateStringTimestamp.Split('-');
                        item.Cells[6].Value = tempTime[0].Trim();
                        item.Cells[7].Value = tempTime[1].Trim();
                        if (warning == true)
                        {
                            item.Cells[4].Style.BackColor = Color.Yellow;
                            item.Cells[5].Style.BackColor = Color.Yellow;
                            item.Cells[6].Style.BackColor = Color.Yellow;
                            item.Cells[7].Style.BackColor = Color.Yellow;
                        }
                        else
                        {
                            item.Cells[4].Style.BackColor = Color.White;
                            item.Cells[5].Style.BackColor = Color.White;
                            item.Cells[6].Style.BackColor = Color.White;
                            item.Cells[7].Style.BackColor = Color.White;
                        }

                    }
                    else
                    {
                        item.Cells[4].Value = VALUE_MISSING;
                        item.Cells[4].Style.BackColor = Color.Red;
                        item.Cells[5].Value = VALUE_MISSING;
                        item.Cells[5].Style.BackColor = Color.Red;
                        item.Cells[6].Value = VALUE_MISSING;
                        item.Cells[6].Style.BackColor = Color.Red;
                        item.Cells[7].Value = VALUE_MISSING;
                        item.Cells[7].Style.BackColor = Color.Red;
                    }

                }
                else // Cell is not selected, erase value
                {
                    // Nothing i just want to update
                }

            }
        }


        #endregion

        #endregion DELIVERED ENERGY
    }
}
