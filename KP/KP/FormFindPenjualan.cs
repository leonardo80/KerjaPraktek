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
    public partial class FormFindPenjualan : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;

        public static string kode = "", cust = "";
        public static int index = -1;
        public static List<string> listidcust = new List<string>();
        public static List<string> listnonota = new List<string>();

        public FormFindPenjualan()
        {
            InitializeComponent();
        }

        public void loadnota(string kodenota)
        {
            kode = kodenota;
            string stm = "select * from jual where kode='" + kode + "'";  
            Koneksi.openConn();
            MySqlCommand cmd = new MySqlCommand(stm, Koneksi.conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                try
                {
                    listnonota.Add(dr.GetString(1));
                    listidcust.Add(dr.GetString(2));

                }
                catch (Exception x)
                { MessageBox.Show(x.Message);}
            }
            Koneksi.conn.Close();
        }

        private void FormFindPenjualan_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0; comboBox2.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string search = textBox1.Text;
            loadnota(comboBox2.Text);
            if (comboBox1.Text == "KODE")
            {
                da = new MySqlDataAdapter("select * from jual where kode='"+comboBox2.Text+"' and id_jual like '%" + search + "%' order by id_jual desc ", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

            }
            if (comboBox1.Text == "CUSTOMER")
            {
                da = new MySqlDataAdapter("select * from jual where kode='"+comboBox2.Text+"' and id_customer like '%" + search + "%' order by id_jual desc ", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
            if (comboBox1.Text == "TANGGAL")
            {
                da = new MySqlDataAdapter("select * from jual where kode='" + comboBox2.Text + "' and tanggal like '%" + search + "%' order by id_jual desc", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string id = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            cust = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            for (int i = 0; i < listnonota.Count(); i++)
            {
                if (listnonota[i] == id)
                {
                    index = i;
                }
            }
            this.Close();
            FormPenjualan f=new FormPenjualan();
            f.Show();
        }
    }
}
