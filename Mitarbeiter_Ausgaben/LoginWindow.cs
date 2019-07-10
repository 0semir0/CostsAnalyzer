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
    public partial class LoginWindow : Form
    {
        public string box1Content;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PWChangeWindow hasher = new PWChangeWindow(); //New Object
            FinalWindow fw = new FinalWindow();
            box1Content = "'" + textBox1.Text + "'";
            try
            {
                string realPasswordHash = fw.getCMD($@"SELECT kennwort 
                                                       FROM mitarbeiter 
                                                       WHERE concat(n_name, '.', v_name) = {box1Content};").ExecuteScalar().ToString();

                fw.GiveUsername("'" + textBox1.Text + "'");
                string mitarbeiterID = fw.getmID();
                
                string box2Input = textBox2.Text;
                string box2InputHash = hasher.GetHashString(box2Input); //Make Hash String out of plaintext pw

                if (box2InputHash == realPasswordHash)
                {
                    fw.getDBmanipulation($@"UPDATE mitarbeiter 
                                            SET anmeldungen = anmeldungen + 1
                                            WHERE mitarbeiter_id = {fw.getmID()};");  //if login is success, logincntr plus one

                    fw.textBox1.Text = mitarbeiterID;
                    fw.label4.Text = textBox1.Text; //Text unten links -> FinalWindow

                    fw.ShowDialog();

                    DialogResult = DialogResult.No; //close loginscreen after closing finalwindow
                }
                else
                {
                    fw.getDBmanipulation($@"UPDATE mitarbeiter
                                            SET fehlanmeldungen = fehlanmeldungen + 1
                                            WHERE mitarbeiter_id = {fw.getmID()}");

                    MessageBox.Show("Passwort inkorrekt.");
                }
                    
                fw.textBox2.Select(); //Cursor in die TextBox platzieren -> FinalWindow

            } catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
