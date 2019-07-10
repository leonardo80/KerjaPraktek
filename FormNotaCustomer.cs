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
    public partial class FormNotaCustomer : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;

        public FormNotaCustomer()
        {
            InitializeComponent();
        }

        private void FormNotaCustomer_Load(object sender, EventArgs e)
        {
            Koneksi.openConn();
            da = new MySqlDataAdapter("select kode as Kode, id_jual as \"Nomer Nota\", tanggal as Tanggal, format(totaljual,0,'de_DE') as Total from jual where id_customer='"+FormCustomer.idcust+"' order by id_jual desc", Koneksi.conn);
            ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            Koneksi.conn.Close();
        }
    }
}
