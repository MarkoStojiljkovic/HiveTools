using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HiveTools
{
    public partial class GraphForm : Form
    {
        public GraphForm()
        {
            InitializeComponent();
            PieChart.Series["Validity"]["PieLabelStyle"] = "Disabled";
        }

        /// <summary>
        /// Populate chart based on DataClass argument
        /// </summary>
        /// <param name="data"></param>
        public void SetChartData(DataClass data)
        {
            // So many things can go wrong
            try
            {
                PieChart.Series["Validity"].Points.AddXY("Valid", (double)data.summary.numOfValid/ data.summary.totalNumber);
                PieChart.Series["Validity"].Points[0].Color = ConfigClass.colors.colorValid;
                PieChart.Series["Validity"].Points.AddXY("Duplicate", (double)data.summary.numOfDuplicate / data.summary.totalNumber);
                PieChart.Series["Validity"].Points[1].Color = ConfigClass.colors.colorDuplicate;
                PieChart.Series["Validity"].Points.AddXY("Missing", (double)data.summary.numOfMissing / data.summary.totalNumber);
                PieChart.Series["Validity"].Points[2].Color = ConfigClass.colors.colorMissing;
                PieChart.Series["Validity"].Points.AddXY("Zero", (double)data.summary.numOfZeroes / data.summary.totalNumber);
                PieChart.Series["Validity"].Points[3].Color = ConfigClass.colors.colorZero;
                PieChart.Series["Validity"].Points.AddXY("Null", (double)data.summary.numOfNullValues / data.summary.totalNumber);
                PieChart.Series["Validity"].Points[4].Color = ConfigClass.colors.colorNull;
                PieChart.Series["Validity"].Points.AddXY("Data repeat", (double)data.summary.numOfDataRepeat / data.summary.totalNumber);
                PieChart.Series["Validity"].Points[5].Color = ConfigClass.colors.colorRepeat;
                PieChart.Series["Validity"].Points.AddXY("Invalid", (double)data.summary.numOfInvalid / data.summary.totalNumber);
                PieChart.Series["Validity"].Points[6].Color = ConfigClass.colors.colorInvalid;
                
                
            }
            catch (Exception e)
            {
                FormCustomConsole.WriteLine(e.Message);
            }
            
        }

        public void FlushChart()
        {
            PieChart.Series["Validity"].Points.Clear();
        }

    }
}
