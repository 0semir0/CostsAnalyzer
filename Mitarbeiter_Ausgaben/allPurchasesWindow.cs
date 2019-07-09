using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mitarbeiter_Ausgaben
{
    public partial class AllPurchasesWindow : Form
    {
        public AllPurchasesWindow()
        {
            InitializeComponent();
            SetTextboxContent();
        }

        private void SetTextboxContent()
        {
            FinalWindow login = new FinalWindow();
            login.getmID();

            string dsCount = login.getCMD($@"SELECT COUNT(*) 
                                             FROM ausgaben 
                                             WHERE mitarbeiter_id = {login.getmID()};").ExecuteScalar().ToString(); //counts datasets of logged in user
            int datasetCount = Int32.Parse(dsCount);

            string[] purchases = new string[datasetCount];
            for(int i = 0; i < datasetCount; i++)
            {
                purchases[i] = "----------------------------------------------------------------\r\n";
                purchases[i] += login.getCMD($@"SELECT CONCAT(datum, ' | ', gericht, ': ', preis, '€') 
                                                FROM ausgaben 
                                                WHERE mitarbeiter_id = {login.getmID()} limit 1 offset {i};").ExecuteScalar().ToString();
            }

            string allDatasets = string.Join(Environment.NewLine, purchases);
            String sum = login.getCMD($@"SELECT SUM(preis) 
                                         FROM ausgaben 
                                         WHERE mitarbeiter_id = {login.getmID()};").ExecuteScalar().ToString();

            textBox1.Text += dsCount + " Eintragungen:\r\n\r\n" + allDatasets + "\r\n\r\nSumme der Ausgaben: " + sum;
        }
    }
}