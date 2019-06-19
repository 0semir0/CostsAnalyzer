using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MySql.Data.MySqlClient;

namespace Mitarbeiter_Ausgaben
{
    public partial class GraphWindow : Form
    {

        private bool buttonClickOnceChecker = false;
        public GraphWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FinalWindow f1 = new FinalWindow();

            var cmd = f1.getCMD($"select concat(month(datum), '.', year(datum)) as Datum, preis from ausgaben where mitarbeiter_id = {f1.getmID()};");
            MySqlDataReader reader;

            try
            {
                //chart config
                var chart = chart1.ChartAreas[0];

                chart.AxisY.Interval = 0.5;
                chart.AxisY.Minimum = 0.25;
                chart.AxisY.Maximum = 15;

                chart.AxisX.Interval = 1;
                chart.AxisX.Minimum = 1;

                if(buttonClickOnceChecker == true)
                {
                    MessageBox.Show("Daten wurden bereits geladen!");
                }

                else
                {
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        this.chart1.Series["Ausgaben"].Points.AddXY(reader.GetString("Datum"), reader.GetInt32("preis"));
                    }

                    buttonClickOnceChecker = true;
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
