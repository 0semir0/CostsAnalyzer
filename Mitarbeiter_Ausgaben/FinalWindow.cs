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
using System.Globalization;
using System.Runtime.InteropServices;

namespace Mitarbeiter_Ausgaben
{
    public partial class FinalWindow : Form
    {
        public static string box5Content;
        
        public FinalWindow()
        {
            InitializeComponent();
        }
        
        //EINKÄUFE EINGABEFELD
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int mID = Int32.Parse(textBox1.Text);
                String gericht = textBox2.Text;

                decimal preis = Convert.ToDecimal(textBox3.Text.Replace(".", ","));

                DbConnect dc = new DbConnect();
                dc.dbcon(gericht, preis, mID);

                textBox2.Clear();
                textBox2.Focus();
                textBox3.Clear();
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        
        //PASSWORT ÄNDERN FENSTER ÖFFNEN
        private void passwortÄndernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PWChangeWindow editPWwindow = new PWChangeWindow();
            editPWwindow.ShowDialog();
        }

        public MySqlCommand getCMD(String command) //DB-CONNECTION AND SQL-COMMAND FUNCTION
        {
            String conString = "datasource=192.168.0.118;Port=3306;Database=mittagessen_ausgaben;Uid=remoteUser0;password=usbw;";
            MySqlConnection con = new MySqlConnection(conString);
            con.Open();

            MySqlCommand cmd = new MySqlCommand(command, con);
            return cmd;
        }
        
        //FÜLLT TEXTBOX MIT OUTPUT EINES SELECTS
        public void fillTbox(MySqlDataReader reader, TextBox box2fill) //FILL TEXTBOX FUNCTION
        {
            if (reader.Read())
            {
                box2fill.Text = reader.GetValue(0).ToString(); //fills given textbox with select statement
            }
        }
        
        //GIBT MITARBEITER ID AUS DB ZURÜCK
        public string getmID()
        {
            string mID = getCMD("select mitarbeiter_id from mitarbeiter where concat(n_name, '.', v_name) = " + box5Content).ExecuteScalar().ToString();
            return mID;
        }

        //USED ONCE -> LOGINWINDOW
        public void GiveUsername(string uname)
        {
            box5Content = uname;
        }

        //GIBT PASSWORT AUS DB ZURÜCK, -> HASH
        public string getPw()
        {
            string pw = getCMD("select kennwort from mitarbeiter where concat(n_name, '.', v_name) = " + box5Content).ExecuteScalar().ToString();
            return pw;
        }

        //PASSWORT ÄNDERN FENSTER ÖFFNEN -> Form2
        private void button2_Click(object sender, EventArgs e)
        {
            GraphWindow editPWindow = new GraphWindow();
            editPWindow.ShowDialog();
        }

        //ALLE AUSGABEN FENSTER ÖFFNEN -> Form4
        private void button3_Click(object sender, EventArgs e) //ALLE AUSGABEN ANZEIGEN
        {
            AllPurchasesWindow allDatasetsWindow = new AllPurchasesWindow();
            allDatasetsWindow.ShowDialog();
        }
    }
}