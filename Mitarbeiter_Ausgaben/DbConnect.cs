using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data;
using System.Globalization;


namespace Mitarbeiter_Ausgaben
{
    class DbConnect
    {
        public void dbcon(String gericht, decimal preis, int mID)
        {
            try {
                //neue DB-Verbindung
                String conString = "Server=192.168.0.118;Port=3306;Database=mittagessen_ausgaben;Uid=remoteUser0;password=usbw;";

                MySqlConnection con = new MySqlConnection(conString);
                MySqlCommand command = con.CreateCommand();

                gericht = "'" + gericht + "'"; //ready for sql query
                //neuer Befehl
                command.CommandText = $"insert into ausgaben(gericht, preis, mitarbeiter_id) values({gericht}, {preis.ToString(CultureInfo.InvariantCulture)}, {mID});";



                con.Open();

                //Befehl ausführen
                command.ExecuteNonQuery();
                MessageBox.Show("Daten wurden in die Datenbank geschrieben.");
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }        
    }
}
