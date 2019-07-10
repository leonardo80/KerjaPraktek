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
    public partial class DeletedPromo : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;

        public DeletedPromo()
        {
            InitializeComponent();
        }

        private void DeletedPromo_Load(object sender, EventArgs e)
        {
            Koneksi.openConn();
            da = new MySqlDataAdapter("select id_promo as ID, nama as Nama,start as Start,end as End, tahun as Tahun from promo where status='1'", Koneksi.conn);
            ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = ds.Tables[0];
            Koneksi.conn.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Koneksi.openConn();
            cmd = new MySqlCommand("update promo set status='0' where id_promo='"+dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()+"'",Koneksi.conn);
            cmd.ExecuteNonQuery();
            Koneksi.conn.Close();
            Koneksi.openConn();
            da = new MySqlDataAdapter("select id_promo as ID, nama as Nama,start as Start,end as End, tahun as Tahun from promo where status='1'", Koneksi.conn);
            ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = ds.Tables[0];
            Koneksi.conn.Close();
        }
    }
}
