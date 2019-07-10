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
    public partial class FormDetailPembayaran : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;

        public FormDetailPembayaran()
        {
            InitializeComponent();
        }

        private void FormDetailPembayaran_Load(object sender, EventArgs e)
        {
            try
            {
                Koneksi.openConn();
                da = new MySqlDataAdapter("select kode as Kode, id_jual as \"Nomer Nota\",tanggal as Tanggal, format(dibayarkan,0,'de_DE') as Jumlah from dpiutang where kode='"+FormBayarPiutang.kodenota+"' and id_jual ='"+FormBayarPiutang.nonota+"' order by tanggal desc", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];                
                dataGridView1.Columns["Jumlah"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Koneksi.conn.Close();
            }
            catch (Exception x)
            {}
        }
    }
}
