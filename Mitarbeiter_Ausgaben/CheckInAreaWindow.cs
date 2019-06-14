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
    public partial class CheckInAreaWindow : Form
    {
        public static String box5Content;
        
        public CheckInAreaWindow()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {            
            dbMaList();
        }
        
        //AUSGABEN-FELD
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

        public void dbMaList() //Tabelle befüllen
        {
            try
            {
                //neue DB-Verbindung
                String conString = "datasource=192.168.0.118;Port=3306;Database=mittagessen_ausgaben;Uid=remoteUser0;password=usbw;";
                MySqlConnection con = new MySqlConnection(conString);
                con.Open();

                //MySQL Befehl
                MySqlDataAdapter adapter = new MySqlDataAdapter("select concat(n_name,'.', v_name) as Benutzer from mitarbeiter;", con);

                //Output des Befehls in die Tabelle schreiben
                DataSet ds = new DataSet();
                adapter.Fill(ds, "mitarbeiter");
                dataGridView1.DataSource = ds.Tables["mitarbeiter"];

            }catch(Exception e) { MessageBox.Show(e.Message); }
        }

        //BENUTZERKONTEN AUSWAHLTABELLE
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) //Wenn auf Zelle in Tabelle geklickt wird, dann...
        {
            String cellContent= dataGridView1.SelectedCells[0].Value.ToString();
            label4.Text = cellContent;
            cellContent = "'" + cellContent + "'";

            try
            { 
                MySqlDataReader dr = getCMD("select mitarbeiter_id from mitarbeiter where concat(n_name, '.', v_name) = " + cellContent).ExecuteReader(); //db-verb. herstellen und select ausführen und output auslesen
                fillTbox(dr, textBox1); //textbox1 mit output des selects füllen
                dataGridView1.Visible = false; //nach klick auf eine Zelle, soll Tabelle verschwinden
                textBox4.Focus(); //Cursor in die TextBox platzieren

            }catch(Exception ex) { MessageBox.Show(ex.Message); }

            try
            {
                MySqlDataReader dr = getCMD("select concat(n_name, '.', v_name) from mitarbeiter where concat(n_name, '.', v_name) = " + cellContent).ExecuteReader();
                fillTbox(dr, textBox5);
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        
        //ANMELDUNG
        public void button1_Click_1(object sender, EventArgs e)
        {
            PWChangeWindow hasher = new PWChangeWindow(); //New Object
            box5Content = "'" + textBox5.Text + "'";
            try
            {
                String realPasswordHash = getCMD("select kennwort from mitarbeiter where concat(n_name, '.', v_name) = " + box5Content).ExecuteScalar().ToString();
                
                String box4Input = textBox4.Text;

                string box4InputHash = hasher.GetHashString(box4Input); //Make Hash String out of plaintext pw

                if(box4InputHash == realPasswordHash) panel1.Visible = false;
                else MessageBox.Show("Passwort inkorrekt.");
                textBox2.Focus(); //Cursor in die TextBox platzieren
                
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
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
        
        private void fillTbox(MySqlDataReader reader, TextBox box2fill) //FILL TEXTBOX FUNCTION
        {
            if (reader.Read())
            {
                box2fill.Text = reader.GetValue(0).ToString();
            }
        }
        
        public String getmID()
        {
            String mID = getCMD("select mitarbeiter_id from mitarbeiter where concat(n_name, '.', v_name) = " + box5Content).ExecuteScalar().ToString();
            return mID;
        }

        public String getPw()
        {
            String pw = getCMD("select kennwort from mitarbeiter where concat(n_name, '.', v_name) = " + box5Content).ExecuteScalar().ToString();
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