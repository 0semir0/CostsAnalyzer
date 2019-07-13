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
    public partial class SignUpWindow : Form
    {
        private string userName;
        private string firstName;
        private string lastName;
        private string mailAdress;
        private string password;
        private string repeatPassword;

        public SignUpWindow()
        {
            InitializeComponent();
            
        }

        private void setInputs()
        {
            userName = textBox1.Text;
            firstName = textBox2.Text;
            lastName = textBox3.Text;
            mailAdress = textBox4.Text;
            password = textBox5.Text;
            repeatPassword = textBox6.Text;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            setInputs();
            userName = "'" + userName + "'";

            FinalWindow fw = new FinalWindow();

            string unameCheck = fw.getCMD($@"SELECT COUNT(*)
                                             FROM users
                                             WHERE userName LIKE {userName};").ExecuteScalar().ToString();

            int unameLineCounter = Int32.Parse(unameCheck);

            if(unameLineCounter > 0)
            {
                MessageBox.Show("Username is taken, try another one.");
            }

            else if(unameLineCounter == 0)
            {

            }

        }
    }
}
