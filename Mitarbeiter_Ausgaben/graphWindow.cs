using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Mitarbeiter_Ausgaben
{
    public partial class GraphWindow : Form
    {
        public GraphWindow()
        {

            InitializeComponent();

            chart1.Titles.Add("Statistik");

            FinalWindow f1 = new FinalWindow();
            
            for (int i = 1; i <= 31; i++) //fill spline
            {
                chart1.Series["Ausgaben"].Points.AddXY(i, 12);
            }
            
            chart1.Series["Ausgaben"].BorderWidth = 4;

            String dsCount = f1.getCMD("select count(*) from ausgaben where mitarbeiter_id =" + f1.getmID() + ";").ExecuteScalar().ToString();
            int datasetCount = Int32.Parse(dsCount);

            String[] storage = new String[datasetCount];

            for (int i = 0; i <= datasetCount; i++)
            {
                break;
                storage[i] = f1.getCMD("select substring(datum, 6, 6) from ausgaben where mitarbeiter_id = " + f1.getmID() + " limit 1 offset " + i + ";").ExecuteScalar().ToString();
            }

            String toDisplay = string.Join(Environment.NewLine, storage); //allDates in einen String zusammenfassen um in einer MsgBox ausgeben zu können
            //MessageBox.Show(toDisplay);

            var chart = chart1.ChartAreas[0];
            
            //chart axis config.
            chart.AxisX.Interval = 1;
            chart.AxisX.Minimum = 1;
            chart.AxisX.Maximum = 31;

            chart.AxisY.Interval = 0.5;
            chart.AxisY.Minimum = 1;
            chart.AxisY.Maximum = 15;
        }

    private void Chart1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Click works!");
        }
    }
}
