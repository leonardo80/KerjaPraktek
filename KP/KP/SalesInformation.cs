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
    public partial class SalesInformation : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;

        public SalesInformation()
        {
            InitializeComponent();
        }

        private void SalesInformation_Load(object sender, EventArgs e)
        {
            sql = "select j.tanggal as Tanggal, j.kode as Kode, j.id_jual as \"Nomer Nota\", c.nama_customer as Customer, dj.jumlah_barang as Jumlah, dj.diskon1 as Disc1, dj.diskon2 as Disc2, dj.totalharga as Total from jual j, djual dj, customer c where j.kode=dj.kode and j.id_jual=dj.id_jual and j.id_customer=c.id_customer and dj.nama_barang='"+FormBarang.namabarang+"' order by j.tanggal desc;";
            da = new MySqlDataAdapter(sql, Koneksi.conn);
            ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }
    }
}
