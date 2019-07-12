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
                int uID = Int32.Parse(textBox1.Text);
                string item = textBox2.Text;

                decimal price = Convert.ToDecimal(textBox3.Text.Replace(".", ","));

                DbConnect dc = new DbConnect();
                dc.dbcon(item, price, uID);

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

        //DB-CONNECTION AND SQL-COMMAND FUNCTION
        public MySqlCommand getCMD(string command) 
        {
            String conString = "Server=canbolat.eu;Port=3306;Database=d02f011e;Uid=d02f011e;password=ptTA2RtMakMWoQ26;";
            MySqlConnection con = new MySqlConnection(conString);
            con.Open();

            MySqlCommand cmd = new MySqlCommand(command, con);
            return cmd;
        }

        //DB-CONNECTION AND SQL-UPDATE/DELETE FUNCITON
        public void getDBmanipulation(string command) 
        {
            string conString = "Server=canbolat.eu;Port=3306;Database=d02f011e;Uid=d02f011e;password=ptTA2RtMakMWoQ26;";

            MySqlConnection con = new MySqlConnection(conString);
            MySqlCommand cmd = con.CreateCommand();

            cmd.CommandText = command;

            con.Open();

            cmd.ExecuteNonQuery();
        }
        
        //FÜLLT TEXTBOX MIT OUTPUT EINES SELECTS
        public void fillTbox(MySqlDataReader reader, TextBox box2fill) //FILL TEXTBOX FUNCTION
        {
            if (reader.Read()) box2fill.Text = reader.GetValue(0).ToString(); //fills given textbox with select statement
        }
        
        //GIBT MITARBEITER ID AUS DB ZURÜCK
        public string getmID()
        {
            string mID = getCMD($@"SELECT user_id 
                                   FROM users
                                   WHERE CONCAT(lastName, '.', firstName) = {box5Content};").ExecuteScalar().ToString();
            return mID;
        }

        //USED ONCE -> LOGINWINDOW
        public void GiveUsername(string uname) => box5Content = uname;

        //GIBT PASSWORT AUS DB ZURÜCK, -> HASH
        public string getPw()
        {
            string pw = getCMD($@"SELECT passwdHash 
                                  FROM users 
                                  WHERE CONCAT(lastName, '.', firstName) = {box5Content};").ExecuteScalar().ToString();
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