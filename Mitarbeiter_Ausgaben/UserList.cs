using MySql.Data.MySqlClient;
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
    public partial class UserList : Form
    {
        public UserList() => InitializeComponent();

        private void UserList_Load(object sender, EventArgs e) => dbMaList();


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
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            FinalWindow fw = new FinalWindow();
            LoginWindow login = new LoginWindow();

            String cellContent = dataGridView1.SelectedCells[0].Value.ToString();
            
            cellContent = "'" + cellContent + "'";
            
            try
            {
                MySqlDataReader dr = fw.getCMD("select mitarbeiter_id from mitarbeiter where concat(n_name, '.', v_name) = " + cellContent).ExecuteReader(); //db-verb. herstellen und select ausführen und output auslesen
                fw.fillTbox(dr, fw.textBox1); //textbox1 mit output des selects füllen
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            try
            {
                MySqlDataReader dr = fw.getCMD("select concat(n_name, '.', v_name) from mitarbeiter where concat(n_name, '.', v_name) = " + cellContent).ExecuteReader();
                fw.fillTbox(dr, login.textBox1);

                login.ShowDialog();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
