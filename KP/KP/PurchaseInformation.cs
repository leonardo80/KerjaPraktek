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
    public partial class PurchaseInformation : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;

        public PurchaseInformation()
        {
            InitializeComponent();
        }

        private void PurchaseInformation_Load(object sender, EventArgs e)
        {
            sql = "select b.tanggal as Tanggal,b.kode as Kode, b.id_beli, s.nama_supplier, db.jumlah_barang, db.diskon1,db.diskon2,db.totalharga from beli b,supplier s,dbeli db where b.kode=db.kode and b.id_beli=db.id_beli and b.id_supplier=s.id_supplier and db.nama_barang='" + FormBarang.namabarang + "' order by Tanggal desc";
            da = new MySqlDataAdapter(sql, Koneksi.conn);
            ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }
    }
}
