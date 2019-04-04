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
    public partial class FormFindSupplier : Form
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

        public static void eraselistsupplier()
        {
            listid = new List<string>();
            listnama = new List<string>();
            listalamat = new List<string>();
            listkota = new List<string>();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            for (int i = 0; i < listid.Count(); i++)
            {
                if (listid[i] == id)
                {
                    index = i;
                }
            }
            this.Close();
            FormSupplier f = new FormSupplier();
            f.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string search = textBox1.Text;
            loadlistsupplier();
            if (comboBox1.Text == "ID")
            {
                da = new MySqlDataAdapter("select id_supplier as ID, nama_supplier as Nama, alamat_supplier as Alamat, kota_supplier as Kota from supplier where id_supplier like '%" + search + "%' ", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

            }
            if (comboBox1.Text == "NAMA")
            {
                da = new MySqlDataAdapter("select id_supplier as ID, nama_supplier as Nama, alamat_supplier as Alamat, kota_supplier as Kota from supplier where nama_supplier like '%" + search + "%' ", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
            if (comboBox1.Text == "ALAMAT")
            {
                da = new MySqlDataAdapter("select id_supplier as ID, nama_supplier as Nama, alamat_supplier as Alamat, kota_supplier as Kota from supplier where alamat_supplier like '%" + search + "%' ", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

            }
            if (comboBox1.Text == "KOTA")
            {
                da = new MySqlDataAdapter("select id_supplier as ID, nama_supplier as Nama, alamat_supplier as Alamat, kota_supplier as Kota from supplier where kota_supplier like '%" + search + "%' ", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
        }

        public static void loadlistsupplier()
        {
            string stm = "select id_supplier as ID, nama_supplier as Nama, alamat_supplier as Alamat, kota_supplier as Kota from supplier";
            Koneksi.openConn();
            MySqlCommand cmd = new MySqlCommand(stm, Koneksi.conn);
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                listid.Add(dr.GetString(0));
            }
            Koneksi.conn.Close();
        }

        public FormFindSupplier()
        {
            InitializeComponent();
        }

        private void FormFindSupplier_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            da = new MySqlDataAdapter("select id_supplier as ID, nama_supplier as Nama, alamat_supplier as Alamat, kota_supplier as Kota from supplier", Koneksi.conn);
            ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            loadlistsupplier();
            this.ActiveControl = comboBox1;
        }

    }
}
