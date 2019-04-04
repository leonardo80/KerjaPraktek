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

namespace KP
{
    public partial class Login : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;

        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Koneksi.openConn();
            string user = textBox1.Text;
            string pass = textBox2.Text;
            string passdb = "";
            sql = "select password_user from user where nama_user='" + user + "'";
            try
            {
                cmd = new MySqlCommand(sql, Koneksi.conn);
                passdb = cmd.ExecuteScalar().ToString();
                if (passdb==pass)
                {
                    sql="select pangkat from user where nama_user='"+user+"'";
                    cmd = new MySqlCommand(sql, Koneksi.conn);
                    string pangkat = cmd.ExecuteScalar().ToString();
                    MdiParent f = new MdiParent();
                    f.Show();
                }
                else
                {
                    MessageBox.Show("Password Salah");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("User Salah");
            }
           
            Koneksi.conn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            this.ActiveControl = textBox1;
        }
    }
}
