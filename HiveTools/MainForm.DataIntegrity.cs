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
using HiveToolsFlagsHelper;

namespace HiveTools
{
    partial class MainForm
    {
        #region DATA INTEGRITY

        DataClass[] allData;
        public static DataClass[] lastAllDataPtr;

        #region TOOLSTRIP ITEMS

        private void dataIntegrityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count == 0) // If no rows are populated, ignore request
            {
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel File|*.csv";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = sfd.FileName;
                StreamWriter sw = new StreamWriter(File.Create(path));
                sw.Write(GetValuesForCSVDataIntegrity());
                sw.Dispose();
            }
        }

        /// <summary>
        /// Generate CSV report for all flags in selected fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allToolStripMenuItem_Click(object sender, EventArgs e) // Make report for all flags
        {
            if (dataGridView2.Rows.Count == 0) // If no rows are populated, ignore request
            {
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel File|*.csv";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = sfd.FileName;
                using (StreamWriter sw = new StreamWriter(File.Create(path)))
                {
                    try
                    {
                        sw.Write(GetValuesForCSVDataIntegrityAllFlags());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    
                }
            }
        }

        /// <summary>
        /// Generate CSV report for missing flags in selected fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void missingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlagsToolStripMenuHelper(ReportFlag.Missing);
        }

        /// <summary>
        /// Generate CSV report for duplicate flags in selected fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void duplicateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlagsToolStripMenuHelper(ReportFlag.Duplicate);
        }

        /// <summary>
        /// Generate CSV report for zero flags in selected fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zeroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlagsToolStripMenuHelper(ReportFlag.Zero);
        }

        /// <summary>
        /// Generate CSV report for null flags in selected fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nullToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlagsToolStripMenuHelper(ReportFlag.Null);
        }

        /// <summary>
        /// Generate CSV report for repeat flags in selected fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repeatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlagsToolStripMenuHelper(ReportFlag.Repeat);
        }
        
        /// <summary>
        /// Helper method which is used by single flag type CSV StripMenuItemClick
        /// </summary>
        /// <param name="flagType"></param>
        private void FlagsToolStripMenuHelper(ReportFlag flagType)
        {
            if (dataGridView2.Rows.Count == 0) // If no rows are populated, ignore request
            {
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel File|*.csv";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = sfd.FileName;
                using (StreamWriter sw = new StreamWriter(File.Create(path)))
                {
                    try
                    {
                        sw.Write(GetValuesForCSVDataIntegrityFlags(flagType));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }


        /// <summary>
        /// Show active flags for selected field in FormGUIStats representation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showFlagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentCell == null)
            {
                return;
            }

            // Get selected row index
            int rowIndex = dataGridView2.CurrentCell.RowIndex;
            int valueID = Convert.ToInt32(dataGridView2.Rows[rowIndex].Cells[1].Value.ToString());
            
            var tuple =  GetInstanceFromAllDataByDeviceID(valueID);

            if (tuple != null)
            {
                DataClass item = tuple.Item1;
                // Open new form with complete flag description
                FormGUIStats fg = new FormGUIStats(item);
                fg.Show();
            }
        }

        #endregion toolstrip items



        #region BUTTON HANDLERS DATA INTEGRITY

        private void buttonGetLocationsDataIntegrity_Click(object sender, EventArgs e)
        {
            char delim = '|';
            string res = ""; // it will hold data from database
            try
            {
                res = DatabaseControllerClass.FetchValuesFromDatabase(DatabaseSettingsClass.ReadConnectionString(), "SELECT DEVICE_ID, NAME, DESCRIPTION FROM LOCATION ORDER BY NAME ASC", delim);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            dataGridView2.Rows.Clear();
            // Split monolithic string to data lines
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
            allData = new DataClass[dataLines.Length]; // Every row will have data class for storing data from database
            string[] rowInfo;
            // Now fill data grid based on results
            for (int i = 0; i < dataLines.Length - 1; i++)
            {
                rowInfo = dataLines[i].Split(';');
                dataGridView2.Rows.Add(false, rowInfo[0], rowInfo[1], rowInfo[2], null);
            }

        }


        private void buttonProcess_Click(object sender, EventArgs e)
        {
            DisableUIControl();
            ICancelableForm fw = new FormWait(this, GetDataIntegrityActiveRowNumber());
            Action action = () =>
            {
                TaskDataIntegrityProcess(fw);
            };

            HelperInterruptibleWorker worker = new HelperInterruptibleWorker(this, fw);
            worker.ExecuteTask(action);
        }


        private void buttonSelectAllDataIntegrity_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView2.Rows)
            {
                item.Cells[0].Value = true;
            }
        }

        private void buttonDeselectAllDataIntegrity_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView2.Rows)
            {
                item.Cells[0].Value = false;
            }
        }

        #endregion button handlers data integrity

        private void TaskDataIntegrityProcess(ICancelableForm fw) // Task when process button is pressed
        {
            Thread.Sleep(100);
            try
            {
                ProcessDataIntegrity(fw);
            }
            catch (Exception e)
            {
                FormCustomConsole.Write("Exception at process data integrity \r\n" + e.ToString() + "\r\n\r\n");
            }
        }

        private void ProcessDataIntegrity(ICancelableForm fw)
        {
            string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            string sysUIFormat = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern;

            FormCustomConsole.WriteLine("CurrentCulture: " + sysFormat);
            FormCustomConsole.WriteLine("CurrentUICulture: " + sysUIFormat);

            DateTime dateStart = GetDateFiltered(dateTimePickerStartDataIntegrity);
            DateTime dateEnd = GetDateFiltered(dateTimePickerEndDataIntegrity);

            // Check if day selection is valid
            TimeSpan dayDiff = dateEnd.Subtract(dateStart); // It will hold days difference
            if (dayDiff.Days <= 0)
            {
                MessageBox.Show("Select valid time!");
                return;
            }

            string oracleDateStart;
            string oracleDateEnd;

            string querryCommand;
            string result = "";
            int currentRow = -1;

            // If row is enabled, process it
            foreach (DataGridViewRow item in dataGridView2.Rows)
            {
                currentRow++; // Start from -1 so it is 0 at first iteration

                if (Convert.ToBoolean(item.Cells[0].Value) == true)
                {
                    // Selected element is enabled

                    // Create oracle time selection string
                    oracleDateStart = HelperClass.ConvertToOracleTime(dateStart);
                    oracleDateEnd = HelperClass.ConvertToOracleTime(dateEnd);

                    // Create command
                    querryCommand = "SELECT VALID,IPAA,IPBA,IPCA,VABA,VBCA,VCAA, READ_TIME, ID FROM DATA_HISTORY WHERE DEVICE_ID=" + item.Cells[1].Value + " AND READ_TIME >= " + oracleDateStart + " AND READ_TIME < " + oracleDateEnd + "AND ROWNUM <= " + ConfigClass.MAX_ROWS_GET.ToString();
                    char delim = '|';
                    try
                    {
                        result = DatabaseControllerClass.FetchValuesFromDatabase(DatabaseSettingsClass.ReadConnectionString(), querryCommand, delim);
                    }
                    catch (Exception)
                    {
                        Task.Run(() => MessageBox.Show("Timeout for device ID:" + item.Cells[1].Value));
                        return;
                    }
                    // Instantiate dataclass for current row
                    allData[currentRow] = new DataClass(dayDiff.Days, currentRow, dateStart, dateEnd);
                    // Fill it with values from database
                    allData[currentRow].FillDataRow(result, Convert.ToInt32(item.Cells[1].Value), delim);
                    // Process data row for: null values, zero values, duplicate values and generaly some way of corrupted data
                    allData[currentRow].ProcessData();

                    allData[currentRow].RunAnalysis(); // This will calculate all needed info
                    string tempSummary = allData[currentRow].GenerateSummary();
                    string[] summary = tempSummary.Split(delim);
                    // Fill cells
                    item.Cells[4].Value = summary[0]; // valid
                    item.Cells[5].Value = summary[1]; // flag status
                    item.Cells[6].Value = dateStart.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    item.Cells[7].Value = dateEnd.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    item.Cells[8].Value = "Ready";
                    item.Cells[8].Style.BackColor = Color.Lavender;

                    // Update percentage bar and check if aborted
                    fw.ProgresStep(); // Update progress bar
                    if (fw.AlreadyClosed)
                    {
                        return;
                    }

                }
                else // Cell is not selected
                {

                }
            }

        }

        private int GetDataIntegrityActiveRowNumber() // Get number of selected rows
        {
            int x = 0;
            foreach (DataGridViewRow item in dataGridView2.Rows)
            {
                if ((bool)item.Cells[0].Value == true)
                {
                    x++;
                }
            }
            return x;
        }

        /// <summary>
        /// Get instance from allData with matching ID, and position in array, if not found returns null
        /// </summary>
        /// <param name="devId"></param>
        /// <returns></returns>
        private Tuple<DataClass, int> GetInstanceFromAllDataByDeviceID(int devId)
        {
            DataClass itemData;
            for (int i = 0; i < allData.Length; i++)
            {
                itemData = allData[i];
                if (itemData == null)
                {
                    continue;
                }
                if (itemData.deviceID == devId)
                {
                    // DataClass object found, 
                    return new Tuple<DataClass, int>(itemData, i);
                }
            }

            return null;
        }

        /// <summary>
        /// Replace DataCLass object within allData with matching deviceID
        /// </summary>
        /// <param name="data"></param>
        private void ReplaceDataPoint(DataClass data)
        {
            for (int i = 0; i < allData.Length; i++)
            {
                if (allData[i] == null)
                {
                    continue;
                }
                if (data.deviceID == allData[i].deviceID)
                {
                    //Replace data point
                    allData[i] = data;
                    return;
                }
            }
            throw new Exception("DataClass object with that ID doesnt exist in allData array");
        }

        private string GetValuesForCSVDataIntegrity()
        {
            string delimiter = ";";
            StringBuilder sb = new StringBuilder();

            // If row is enabled, process it
            foreach (DataGridViewRow item in dataGridView2.Rows)
            {
                if (Convert.ToBoolean(item.Cells[0].Value) == true) // if checkbox is enabled
                {

                    if (item.Cells[8].Value == null) // Ignore if there is no data
                    {
                        continue;
                    }

                    sb.Append(item.Cells[1].Value + delimiter + item.Cells[2].Value + delimiter + item.Cells[3].Value + delimiter + item.Cells[4].Value
                        + delimiter + item.Cells[5].Value + delimiter + item.Cells[6].Value + delimiter + item.Cells[7].Value + delimiter + "\r\n");
                }
            }

            sb.Remove(sb.Length - 2, 2); // Remove last new line

            return sb.ToString();
        }


        #region FLAG GETTERS FOR REPORT
        
        private string GetValuesForCSVDataIntegrityAllFlags()
        {
            string delimiter = ",";
            StringBuilder sb = new StringBuilder();

            // If row is enabled, process it
            foreach (DataGridViewRow item in dataGridView2.Rows)
            {
                if (Convert.ToBoolean(item.Cells[0].Value) == true) // if checkbox is enabled
                {

                    if (item.Cells[8].Value == null) // Ignore if there is no data in rows
                    {
                        continue;
                    }

                    int valueID = Convert.ToInt32(item.Cells[1].Value); // Get id from row

                    List<FlagOccurrenceClass> flagList;
                    // Now find DataClass object binded with this valueID
                    //foreach (DataClass itemData in allData)
                    //{
                    //    if (itemData == null)
                    //    {
                    //        continue;
                    //    }
                    //    if (itemData.cellID == valueID)
                    //    {
                    //        // DataClass object found, now get all flags and convert them in string (CSV) representation
                    //        flagList = HelperGUIStats.GenerateReportForAllFlags(itemData);
                    //        string res = HelperGUIStats.ConvertListToCSV(flagList, delimiter, itemData.cellID);
                    //        sb.Append(res);
                    //    }
                    //}
                    var tuple = GetInstanceFromAllDataByDeviceID(valueID);
                    
                    if (tuple != null)
                    {
                        DataClass itemData = tuple.Item1;
                        // DataClass object found, now get all flags and convert them in string (CSV) representation
                        flagList = HelperGUIStats.GenerateReportForAllFlags(itemData);
                        string res = HelperGUIStats.ConvertListToCSV(flagList, delimiter, itemData.deviceID);
                        sb.Append(res);
                    }

                }
            }

            if (sb.Length == 0)
            {
                throw new Exception("Couldn't get any data from Data Integrity");
            }
            sb.Remove(sb.Length - 2, 2); // Remove last new line

            return sb.ToString();
        }

        private string GetValuesForCSVDataIntegrityFlags(ReportFlag flagType)
        {
            string delimiter = ",";
            StringBuilder sb = new StringBuilder();

            // If row is enabled, process it
            foreach (DataGridViewRow item in dataGridView2.Rows)
            {
                if (Convert.ToBoolean(item.Cells[0].Value) == true) // if checkbox is enabled
                {

                    if (item.Cells[8].Value == null) // Ignore if there is no data in rows
                    {
                        continue;
                    }

                    int valueID = Convert.ToInt32(item.Cells[1].Value); // Get id from row

                    List<FlagOccurrenceClass> flagList;

                    var tuple = GetInstanceFromAllDataByDeviceID(valueID);
                    

                    if (tuple != null)
                    {
                        DataClass itemData = tuple.Item1;
                        // DataClass object found, now get all flags and convert them in string (CSV) representation
                        flagList = HelperGUIStats.GenerateReport(itemData, flagType);
                        string res = HelperGUIStats.ConvertListToCSV(flagList, delimiter, itemData.deviceID);
                        sb.Append(res);
                    }

                }
            }

            if (sb.Length == 0)
            {
                throw new Exception("Couldn't get any data from Data Integrity");
            }
            sb.Remove(sb.Length - 2, 2); // Remove last new line

            return sb.ToString();
        }

        #endregion flag getters for report

        #region DATA INTEGRITY EVENTS

        // Those 2 are resposible for displaying pie chart on hover
        private void dataGridView2_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return; // Table is not populated yet
            }

            //dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Blue;
            int valueID = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[1].Value); // Get id from selected row row

            var tuple = GetInstanceFromAllDataByDeviceID(valueID);
            if (tuple == null)
            {
                return;
            }
            DataClass itemData = tuple.Item1;
            
            gf.Show();

            int wid = gf.Size.Width;
            int heig = gf.Size.Height;
            // put new form in bottom right corner
            gf.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - wid, Screen.PrimaryScreen.WorkingArea.Height - heig);

            gf.SetChartData(itemData);
            
            
        }

        private void dataGridView2_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            gf.FlushChart();
            gf.Hide();
        }



        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Get clicked device ID
            int valueID = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString());
            foreach (DataClass item in allData)
            {
                if (item == null)
                {
                    continue;
                }
                if (item.deviceID == valueID)
                {
                    DataIntegrityGUIForm tf = new DataIntegrityGUIForm(item);
                    tf.Show();

                    break;
                }
            }
        }

        private void dataGridView2_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Ignore if a column or row header is clicked
            //if (e.RowIndex != -1 && e.ColumnIndex != -1)
            //{
            //    if (e.Button == MouseButtons.Right)
            //    {
            //        DataGridViewCell clickedCell = (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex];

            //        // Here you can do whatever you want with the cell
            //        dataGridView2.CurrentCell = clickedCell;  // Select the clicked cell, for instance

            //        // Get mouse position relative to the vehicles grid
            //        var relativeMousePosition = dataGridView2.PointToClient(Cursor.HotSpot);
            //        // Show the context menu
            //        contextMenuStripDataIntegrity.Show(dataGridView2, relativeMousePosition);
            //    }
            //}
        }

        #endregion

        #region DATETIMEPICKER GET METHODS
        // year
        private string GetStartYearDataIntegrity()
        {
            return dateTimePickerStartDataIntegrity.Value.ToString("yyyy");
        }
        private string GetEndYearDataIntegrity()
        {
            return dateTimePickerEndDataIntegrity.Value.ToString("yyyy");
        }

        // Month
        private string GetStartMonthDataIntegrity()
        {
            return dateTimePickerStartDataIntegrity.Value.ToString("MM");
        }
        private string GetEndMonthDataIntegrity()
        {
            return dateTimePickerEndDataIntegrity.Value.ToString("MM");
        }

        // Day
        private string GetStartDayDataIntegrity()
        {
            return dateTimePickerStartDataIntegrity.Value.ToString("dd");
        }
        private string GetEndDayDataIntegrity()
        {
            return dateTimePickerEndDataIntegrity.Value.ToString("dd");
        }

        private DateTime GetDateFiltered(DateTimePicker dt)
        {
            return new DateTime(dt.Value.Year, dt.Value.Month, dt.Value.Day);
        }


        #endregion

        // TODO: Make sorting disabled while updating
        public void ReplaceDataPoint(int deviceID)
        {

            DataGridViewRow dataGridItem = null;
            int dataGridSelectedIndex = -1;
            // Find row with matching ID
            foreach (DataGridViewRow item in dataGridView2.Rows) 
            {
                dataGridSelectedIndex = Convert.ToInt32(item.Cells[1].Value);
                if (dataGridSelectedIndex == deviceID)
                {
                    dataGridItem = item;
                    break;
                }
            }

            if (dataGridItem == null)
            {
                throw new Exception("Item in DataGrid with that deviceID doesn't exist");
            }

            // Find matching ID in allData
            var tuple = GetInstanceFromAllDataByDeviceID(deviceID);
            if (tuple == null)
            {
                throw new Exception("Data item in allData with that deviceID doesn't exist");
            }
            // Extract information from data object
            DataClass data = tuple.Item1;
            
            DateTime dateStart = data.startDate;
            DateTime dateEnd = data.endDate;

            string oracleDateStart = HelperClass.ConvertToOracleTime(data.startDate);
            string oracleDateEnd = HelperClass.ConvertToOracleTime(data.endDate);
            // Generate oracle time selection string
            string result = "";
            //int currentRow = _index;

            string querryCommand = "SELECT VALID,IPAA,IPBA,IPCA,VABA,VBCA,VCAA, READ_TIME, ID FROM DATA_HISTORY WHERE DEVICE_ID=" + dataGridSelectedIndex + " AND READ_TIME >= " + oracleDateStart + " AND READ_TIME < " + oracleDateEnd + "AND ROWNUM <= " + ConfigClass.MAX_ROWS_GET.ToString();
            char delim = '|';
            try
            {
                result = DatabaseControllerClass.FetchValuesFromDatabase(DatabaseSettingsClass.ReadConnectionString(), querryCommand, delim);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            finally
            {
                //fw.Close();
            }
            // Instantiate dataclass for current row
            //data = new DataClass(dateEnd.Subtract(dateStart).Days, dataGridItem.Index, dateStart, dateEnd);
            var newData = new DataClass(dateEnd.Subtract(dateStart).Days, tuple.Item2, dateStart, dateEnd);
            // Fill it with values from database
            newData.FillDataRow(result, dataGridSelectedIndex, delim);
            // Process data row for: null values, zero values, duplicate values and generaly some way of corrupted data
            newData.ProcessData();
            newData.RunAnalysis(); // This will calculate all needed info
            string tempSummary = newData.GenerateSummary();
            string[] summary = tempSummary.Split(delim);
            // Fill cells
            dataGridItem.Cells[4].Value = summary[0]; // valid
            dataGridItem.Cells[5].Value = summary[1]; // flag status
            dataGridItem.Cells[6].Value = dateStart.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            dataGridItem.Cells[7].Value = dateEnd.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            dataGridItem.Cells[8].Value = "Ready";
            dataGridItem.Cells[8].Style.BackColor = Color.Lavender;

            // Finaly replace data instance in allData array
            ReplaceDataPoint(newData);
        }


        #endregion DATA INTEGRITY

    }
}
