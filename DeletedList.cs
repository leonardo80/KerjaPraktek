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
    public partial class DeletedList : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;
        public static string id, nama, satuan, harga;
        public static int index = -1;

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            Koneksi.openConn();
            cmd = new MySqlCommand("update barang set status='Aktif' where id_barang='"+id+"'",Koneksi.conn);
            cmd.ExecuteNonQuery();
            Koneksi.conn.Close();
            loaddata();
        }

        public void loaddata()
        {
            Koneksi.openConn();
            da = new MySqlDataAdapter("select id_barang as \"ID Barang\", nama_barang as \"Nama Barang\", satuan_barang as Satuan, harga_barang as Harga, nama_kategori as \"Nama Kategori\", deskripsi as Deskripsi, status as Status from barang where status='Tidak Aktif' order by id_barang", Koneksi.conn);
            ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = ds.Tables[0];
            Koneksi.conn.Close();
        }

        public DeletedList()
        {
            InitializeComponent();
        }

        public void settingdatagrid()
        {
            DataGridViewColumn col;
            col = dataGridView1.Columns[0]; col.Width = 65;
            col = dataGridView1.Columns[1]; col.Width = 200;
            col = dataGridView1.Columns[2]; col.Width = 60;
            col = dataGridView1.Columns[3]; col.Width = 70;
            col = dataGridView1.Columns[4]; col.Width = 100;
            col = dataGridView1.Columns[5]; col.Width = 120;
            col = dataGridView1.Columns[6]; col.Width = 80;
        }

        private void DeletedList_Load(object sender, EventArgs e)
        {
            loaddata();
            settingdatagrid();
        }
    }
}
