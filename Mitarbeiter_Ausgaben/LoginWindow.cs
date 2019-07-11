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
                string realPasswordHash = fw.getCMD($@"SELECT passwdHash 
                                                       FROM users 
                                                       WHERE concat(lastName, '.', firstName) = {box1Content};").ExecuteScalar().ToString();

                fw.GiveUsername("'" + textBox1.Text + "'");
                string userID = fw.getmID();
                
                string box2Input = textBox2.Text;
                string box2InputHash = hasher.GetHashString(box2Input); //Make Hash String out of plaintext pw

                if (box2InputHash == realPasswordHash)
                {
                    fw.getDBmanipulation($@"UPDATE users 
                                            SET logins = logins + 1
                                            WHERE user_id = {fw.getmID()};");  //if login is success, logincntr plus one

                    fw.textBox1.Text = userID;
                    fw.label4.Text = textBox1.Text; //Text unten links -> FinalWindow

                    fw.ShowDialog();

                    DialogResult = DialogResult.No; //close loginscreen after closing finalwindow
                }
                else
                {
                    fw.getDBmanipulation($@"UPDATE users
                                            SET failedLogins = failedLogins + 1
                                            WHERE user_id = {fw.getmID()}");  //if login is no success, errorcntr plus one

                    MessageBox.Show("Password incorrect.");
                }
                    
                fw.textBox2.Select(); //Cursor in die TextBox platzieren -> FinalWindow

            } catch (Exception ex) { MessageBox.Show(ex.Message); }
        }


        //SHOW SIGN UP WINDOW
        private void button2_Click(object sender, EventArgs e)
        {
            SignUpWindow SignUp = new SignUpWindow();
            SignUp.ShowDialog();
        }
    }
}
