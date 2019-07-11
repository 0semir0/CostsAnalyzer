using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Mitarbeiter_Ausgaben
{
    public partial class PWChangeWindow : Form
    {
        public PWChangeWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pwOld = textBox1.Text;
            string pwNew = textBox2.Text;
            string pwRepeat = textBox3.Text;

            //make hashes ready to compare down below with saved hashes in db
            string pwOldHash = GetHashString(pwOld); 
            string pwNewHash = GetHashString(pwNew);
            
            try
            {
                FinalWindow f1 = new FinalWindow();

                if (pwNew != pwRepeat) MessageBox.Show("Entries not correct. \nRepeat!");
                if (pwOldHash != f1.getPw()) MessageBox.Show("Entries not correct. \nRepeat!");

                else if (pwNew == pwRepeat)
                {
                    pwNewHash = "'" + pwNewHash + "'"; //in Anführungszeichen -> mysql Syntax
                    f1.getCMD($@"UPDATE users 
                                 SET passwdHash = {pwNewHash} 
                                 WHERE user_id= {f1.getmID()};").ExecuteNonQuery();

                    MessageBox.Show("Password Changed!");

                    this.Close(); //close window after successful PW-Change
                }
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        
        //Hash functions
        public byte[] GetHash(string inputString)
        {
            HashAlgorithm algo = SHA256.Create();
            return algo.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString)) sb.Append(b.ToString("X2"));
            
            return sb.ToString();
        }
    }
}
