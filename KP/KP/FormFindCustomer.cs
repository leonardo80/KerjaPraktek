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
    public partial class FormFindCustomer : Form
    {

        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;
        public static string id, nama, alamat, kota;
        public static int index = -1;
        public static List<string> listid = new List<string>();
        public static List<string> listnama = new List<string>();
        public static List<string> listalamat = new List<string>();
        public static List<string> listkota = new List<string>();
        public static List<string> liststatus = new List<string>();

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            nama = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            alamat= dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            kota = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            for (int i = 0; i < listid.Count(); i++)
            {
                if (listid[i] == id)
                {
                    index = i;
                }
            }

            this.Close();
            FormCustomer f = new FormCustomer();
            f.Show();
        }

        public static void eraselistcustomer()
        {
            listid = new List<string>();
            listnama = new List<string>();
            listalamat = new List<string>();
            listkota = new List<string>();
            liststatus = new List<string>();
        }

        private void FormFindCustomer_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            this.ActiveControl = comboBox1;
        }

        public static void loadlistcustomer()
        {
            string stm = "select * from customer";
            Koneksi.openConn();
            MySqlCommand cmd = new MySqlCommand(stm, Koneksi.conn);
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                listid.Add(dr.GetString(0));
                listnama.Add(dr.GetString(1));
                listalamat.Add(dr.GetString(2));
                listkota.Add(dr.GetString(3));
                liststatus.Add(dr.GetString(4));
            }
            Koneksi.conn.Close();
        }

        public FormFindCustomer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string search = textBox1.Text;
            loadlistcustomer();
            if (comboBox1.Text == "ID")
            {
                da = new MySqlDataAdapter("select * from customer where id_customer like '%" + search + "%' ", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

            }
            if (comboBox1.Text == "NAMA")
            {
                da = new MySqlDataAdapter("select * from customer where nama_customer like '%" + search + "%' ", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
            if (comboBox1.Text == "ALAMAT")
            {
                da = new MySqlDataAdapter("select * from customer where alamat_customer like '%" + search + "%' ", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

            }
            if (comboBox1.Text == "KOTA")
            {
                da = new MySqlDataAdapter("select * from customer where kota_customer like '%" + search + "%' ", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
            if (comboBox1.Text == "STATUS")
            {
                da = new MySqlDataAdapter("select * from customer where status like '%" + search + "%' ", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
        }
    }
}
