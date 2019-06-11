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

namespace Mitarbeiter_Ausgaben
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String pwOld = textBox1.Text;
            String pwNew = textBox2.Text;
            String pwRepeat = textBox3.Text;
            
            try
            {
                Form1 f1 = new Form1();

                if (pwNew != pwRepeat) MessageBox.Show("Eingaben falsch. \nKorrigieren!");
                if (pwOld != f1.getPw()) MessageBox.Show("Eingaben falsch. \nKorrigieren!");

                else if (pwNew == pwRepeat)
                {
                    pwNew = "'" + pwNew + "'"; //in Anführungszeichen -> mysql Syntax
                    f1.getCMD("update mitarbeiter set kennwort=" + pwNew + "where mitarbeiter_id=" + f1.getmID() + ";").ExecuteNonQuery();

                    MessageBox.Show("Passwort geändert!");
                }
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
