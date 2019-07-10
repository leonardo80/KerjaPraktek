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
    public partial class FormNotaSupplier : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;

        public FormNotaSupplier()
        {
            InitializeComponent();
        }

        private void FormNotaSupplier_Load(object sender, EventArgs e)
        {
            Koneksi.openConn();
            da = new MySqlDataAdapter("select kode as Kode, id_beli as \"Nomer Nota\", tanggal as Tanggal, format(totalbeli,0,'de_DE') as Total from beli where id_supplier='" + FormSupplier.idsupp + "' order by id_beli desc", Koneksi.conn);
            ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            Koneksi.conn.Close();
        }
    }
}
