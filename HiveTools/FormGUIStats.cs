using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HiveToolsFlagsHelper;

namespace HiveTools
{
    public partial class FormGUIStats : Form
    {
        public FormGUIStats()
        {
            InitializeComponent();
        }

        // I want this data to be persitant while this form is active
        List<FlagOccurrenceClass> missingRows;
        List<FlagOccurrenceClass> duplicateRows;
        List<FlagOccurrenceClass> zeroRows;
        List<FlagOccurrenceClass> nullRows;
        List<FlagOccurrenceClass> repeatRows;

        public FormGUIStats(DataClass item) : this()
        {
            int valueID = item.deviceID;
            this.Text = "ID = " + valueID.ToString();
            // Generate missing values report
            int cnt = 0;
            missingRows = HelperGUIStats.GenerateReport(item, ReportFlag.Missing);
            FillRows(valueID, missingRows, ref cnt);

            duplicateRows = HelperGUIStats.GenerateReport(item, ReportFlag.Duplicate);
            FillRows(valueID, duplicateRows, ref cnt);

            zeroRows = HelperGUIStats.GenerateReport(item, ReportFlag.Zero);
            FillRows(valueID, zeroRows, ref cnt);

            nullRows = HelperGUIStats.GenerateReport(item, ReportFlag.Null);
            FillRows(valueID, nullRows, ref cnt);

            repeatRows = HelperGUIStats.GenerateReport(item, ReportFlag.Repeat);
            FillRows(valueID, repeatRows, ref cnt);

        }

        private void FillRows(int ID, List<FlagOccurrenceClass> rows, ref int cnt)
        {
            
            foreach (var rowValues in rows)
            {
                dataGridViewStats.Rows.Add();
                dataGridViewStats.Rows[cnt].Cells[0].Value = ID.ToString() ;
                dataGridViewStats.Rows[cnt].Cells[1].Value = rowValues.flag;
                dataGridViewStats.Rows[cnt].Cells[2].Style.BackColor = rowValues.col;
                dataGridViewStats.Rows[cnt].Cells[3].Value = rowValues.from;
                dataGridViewStats.Rows[cnt].Cells[4].Value = rowValues.to;
                cnt++;
            }
        }
        
        
    }
}
