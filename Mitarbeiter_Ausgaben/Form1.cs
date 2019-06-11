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
    public partial class Form1 : Form
    {
        public static String box5Content;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {            
            dbMaList();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            int mID = Int32.Parse(textBox1.Text);
            String gericht = textBox2.Text;
            
            decimal preis = Convert.ToDecimal(textBox3.Text.Replace(".", ","));

            DbConnect dc = new DbConnect();
            dc.dbcon(gericht, preis, mID);
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) //Wenn auf Zelle in Tabelle geklickt wird, dann...
        {
            //Zellinhalt in String speichern
            String cellContent= dataGridView1.SelectedCells[0].Value.ToString();
            label4.Text = cellContent;
            cellContent = "'" + cellContent + "'";

            try
            { 
                MySqlDataReader dr = getCMD("select mitarbeiter_id from mitarbeiter where concat(n_name, '.', v_name) = " + cellContent).ExecuteReader(); //db-verb. herstellen und select ausführen und output auslesen
                fillTbox(dr, textBox1); //textbox1 mit output des selects füllen
                dataGridView1.Visible = false; //nach klick auf eine Zelle, soll Tabelle verschwinden
            }catch(Exception ex) { MessageBox.Show(ex.Message); }

            try
            {
                MySqlDataReader dr = getCMD("select concat(n_name, '.', v_name) from mitarbeiter where concat(n_name, '.', v_name) = " + cellContent).ExecuteReader();
                fillTbox(dr, textBox5);
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        
        private void button1_Click_1(object sender, EventArgs e) //Wenn "bestätigen" button geklickt, dann
        {
            box5Content = "'" + textBox5.Text + "'";
            //box5Content = "'" + box5Content + "'";
            try
            {
                String realPassword = getCMD("select kennwort from mitarbeiter where concat(n_name, '.', v_name) = " + box5Content).ExecuteScalar().ToString();
                String box4Input = textBox4.Text;

                if(box4Input == realPassword) panel1.Visible = false;
                else MessageBox.Show("Passwort inkorrekt.");
                
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void passwortÄndernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 editPWwindow = new Form2();
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

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 editPWindow = new Form3();
            editPWindow.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String dsCount = getCMD("select count(*) from ausgaben where mitarbeiter_id =" + getmID() + ";").ExecuteScalar().ToString();
            int datasetCount = Int32.Parse(dsCount);

            String[] allpurch = new String[datasetCount];
            for (int i = 0; i < datasetCount; i++)
            {
                allpurch[i] = getCMD("select concat(datum, ' | ', gericht, ': ', preis, '€') from ausgaben where mitarbeiter_id =" + getmID() + " limit 1 offset " + i + ";").ExecuteScalar().ToString();
            }

            String sum = getCMD("select sum(preis) from ausgaben where mitarbeiter_id=" + getmID() + ";").ExecuteScalar().ToString();
            String toDisplay = string.Join(Environment.NewLine, allpurch); //allDates in einen String zusammenfassen um in einer MsgBox ausgeben zu können
            
            MessageBox.Show(dsCount + " Eintragungen:\n\n" + toDisplay + "\n\nSumme der Ausgaben: " + sum);
        }
    }
}