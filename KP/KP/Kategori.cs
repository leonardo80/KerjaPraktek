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
    public partial class Kategori : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;
        public string nama = "";
        DataGridViewColumn col;

        public Kategori()
        {
            InitializeComponent();
        }

        private void Kategori_Load(object sender, EventArgs e)
        {
            this.ActiveControl = textBox1;
            Koneksi.openConn();
            try
            {
                da = new MySqlDataAdapter("select nama_kategori as \"Nama Kategori\" from kategori", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                col = dataGridView1.Columns[0]; col.Width = 150;
            }
            catch (Exception x)
            { MessageBox.Show(x.Message);}            
            Koneksi.conn.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Koneksi.openConn();
            sql = "select nama_kategori from kategori where nama_kategori='" + textBox1.Text + "'";
            cmd = new MySqlCommand(sql, Koneksi.conn);
            try
            {
                string nama = cmd.ExecuteScalar().ToString();
                lbStatus.Text = "Unavailable";
                button1.Enabled = false;

            }
            catch (Exception)
            {
                lbStatus.Text = "Available";
                button1.Enabled = true;
            }
            Koneksi.conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Koneksi.openConn();
                sql = "select count(*) from kategori";
                cmd = new MySqlCommand(sql, Koneksi.conn);
                string temp = cmd.ExecuteScalar().ToString();
                int urutan = Convert.ToInt32(temp) + 1;
                string kodekategori = urutan.ToString().PadLeft(4, '0');
                cmd = new MySqlCommand("insert into kategori values('"+kodekategori+"','" + textBox1.Text + "')", Koneksi.conn);
                cmd.ExecuteNonQuery();
                da = new MySqlDataAdapter("select nama_kategori as \"Nama Kategori\" from kategori", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                col = dataGridView1.Columns[0]; col.Width = 150;
                textBox1.Text = "";
                this.ActiveControl = textBox1;
                Koneksi.conn.Close();
            }
            catch (Exception x)
            {}
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            button2.Enabled = true;
            nama = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            textBox1.Text = nama;
            button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Koneksi.openConn();
            cmd = new MySqlCommand("update kategori set nama_kategori='"+textBox1.Text+"' where nama_kategori='"+nama+"'",Koneksi.conn);
            cmd.ExecuteNonQuery();
            button2.Enabled = false;
            button1.Enabled = true;
            da = new MySqlDataAdapter("select nama_kategori as \"Nama Kategori\" from kategori", Koneksi.conn);
            ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            col = dataGridView1.Columns[0]; col.Width = 150;
            Koneksi.conn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
