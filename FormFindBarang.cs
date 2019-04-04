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
    public partial class FormFindBarang : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;
        public static string id, nama, satuan, harga;
        public static int index = -1;
        public static List<string> listid = new List<string>();
        public static List<string> listnama = new List<string>();
        public static List<string> listsatuan = new List<string>();
        public static List<string> listharga = new List<string>();

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            for (int i = 0; i < listid.Count(); i++)
            {
                if (listid[i]==id)
                {
                    index = i;
                }
            }
            this.Close();
            FormBarang g = new FormBarang();
            g.Show();
        }

        public void settingdatagrid()
        {
            DataGridViewColumn col;
            col = dataGridView1.Columns[0]; col.Width = 65;
            col = dataGridView1.Columns[1]; col.Width = 200;
            col = dataGridView1.Columns[2]; col.Width = 50;
            col = dataGridView1.Columns[3]; col.Width = 70;
            col = dataGridView1.Columns[4]; col.Width = 80;
            col = dataGridView1.Columns[5]; col.Width = 120;
            col = dataGridView1.Columns[6]; col.Width = 120;
            col = dataGridView1.Columns[7]; col.Width = 50;
        }

        public static void eraselistbarang()
        {
            listid = new List<string>();
            listnama = new List<string>();
            listsatuan = new List<string>();
            listharga = new List<string>();
        }

        private void FormFindBarang_Load(object sender, EventArgs e)
        {
            Koneksi.openConn();
            comboBox1.SelectedIndex = 0;

            da = new MySqlDataAdapter("select b.id_barang as \"ID Barang\", b.nama_barang as \"Nama Barang\", s.stok as Stok, b.satuan_barang as Satuan, b.harga_barang as Harga, b.nama_kategori as \"Nama Kategori\", b.deskripsi as Deskripsi, b.status as Status from barang b, stok s where s.id_barang=b.id_barang and status='Aktif' order by b.id_barang", Koneksi.conn);
            ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            Koneksi.conn.Close();
            loadlistbarang();
            settingdatagrid();
            this.ActiveControl = comboBox1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DeletedList d = new DeletedList();
            d.Show();
            this.Close();
        }

        public static void loadlistbarang()
        {
            string stm = "select id_barang as \"ID Barang\", nama_barang as \"Nama Barang\", satuan_barang as Satuan, harga_barang as Harga, nama_kategori as \"Nama Kategori\", deskripsi as Deskripsi, status as Status from barang where status='Aktif' order by id_barang";
            Koneksi.openConn();
            MySqlCommand cmd = new MySqlCommand(stm, Koneksi.conn);
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                listid.Add(dr.GetString(0));
            }
            Koneksi.conn.Close();
        }

        public FormFindBarang()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string search = textBox1.Text;
            loadlistbarang();
            Koneksi.openConn();
            if (comboBox1.Text=="ID")
            {
                da = new MySqlDataAdapter("select b.id_barang as \"ID Barang\", b.nama_barang as \"Nama Barang\", s.stok as Stok, b.satuan_barang as Satuan, b.harga_barang as Harga, b.nama_kategori as \"Nama Kategori\", b.deskripsi as Deskripsi, b.status as Status from barang b, stok s where s.id_barang=b.id_barang and b.id_barang like '%" + search + "%' and b.status='Aktif'", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
            else if (comboBox1.Text=="NAMA")
            {
                da = new MySqlDataAdapter("select b.id_barang as \"ID Barang\", b.nama_barang as \"Nama Barang\", s.stok as Stok, b.satuan_barang as Satuan, b.harga_barang as Harga, b.nama_kategori as \"Nama Kategori\", b.deskripsi as Deskripsi, b.status as Status from barang b, stok s where s.id_barang=b.id_barang and b.nama_barang like '%" + search + "%' and b.status='Aktif' ", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
            else if (comboBox1.Text == "KATEGORI")
            {
                try
                {
                    da = new MySqlDataAdapter("select b.id_barang as \"ID Barang\", b.nama_barang as \"Nama Barang\", s.stok as Stok, b.satuan_barang as Satuan, b.harga_barang as Harga, b.nama_kategori as \"Nama Kategori\", b.deskripsi as Deskripsi, b.status as Status from barang b, stok s where s.id_barang=b.id_barang and b.nama_kategori='" + textBox1.Text + "' and b.status='Aktif' ", Koneksi.conn);
                    ds = new DataSet();
                    da.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];
                }
                catch (Exception x)
                {}

            }
            settingdatagrid();
            Koneksi.conn.Close();
        }
    }
}
