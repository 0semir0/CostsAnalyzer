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
        public void dbcon(String item, decimal price, int uID)
        {
            try {
                //neue DB-Verbindung
                String conString = "Server=canbolat.eu;Port=3306;Database=d02f011e;Uid=d02f011e;password=ptTA2RtMakMWoQ26;";

                MySqlConnection con = new MySqlConnection(conString);
                MySqlCommand command = con.CreateCommand();

                item = "'" + item + "'"; //ready for sql query
                //neuer Befehl
                command.CommandText = $@"INSERT INTO allPurchases(item, price, user_id) 
                                         VALUES({item}, {price.ToString(CultureInfo.InvariantCulture)}, {uID});";
                
                con.Open();

                //Befehl ausführen
                command.ExecuteNonQuery();
                MessageBox.Show("Daten wurden in die Datenbank geschrieben.");
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }        
    }
}
