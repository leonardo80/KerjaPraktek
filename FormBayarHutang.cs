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
    public partial class FormBayarHutang : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;

        public static string kodenota = "";
        public static string nonota = "";
        public static string status = "";

        public FormBayarHutang()
        {
            InitializeComponent();
        }

        public void count()
        {
            if (checkBox1.Checked == true)
            {
                tbTotal.Text = "0";
            }
            else
            {
                Koneksi.openConn();
                int total = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    string tagihan = dataGridView1.Rows[i].Cells[3].Value.ToString();
                    tagihan = tagihan.Replace(".", "");
                    total += Convert.ToInt32(tagihan);
                }
                tbTotal.Text = total + "";
                tbTotal.Text = Function.separator(tbTotal.Text);
                Koneksi.conn.Close();
            }
        }

        private void FormBayarHutang_Load(object sender, EventArgs e)
        {
            Koneksi.openConn();
            da = new MySqlDataAdapter("select h.id_beli as Nomer, b.tanggaljatuhtempo as \"Jatuh Tempo\", s.nama_supplier as Supplier ,format(h.total,0,'de_DE') as Total from hutang h, beli b, supplier s where h.id_beli = b.id_beli and h.status='"+status+ "' and h.id_supplier = s.id_supplier order by b.tanggaljatuhtempo asc,Nomer desc", Koneksi.conn);
            ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            DataGridViewColumn col;
            col = dataGridView1.Columns[0]; col.Width = 80;
            col = dataGridView1.Columns[1]; col.Width = 75;
            col = dataGridView1.Columns[2]; col.Width = 200;
            col = dataGridView1.Columns[3]; col.Width = 80; dataGridView1.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            Koneksi.conn.Close();
            this.ActiveControl = tbSearch;
            count();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                status = "1";
            }
            else
            {
                status = "0";
            }
            Koneksi.openConn();
            da = new MySqlDataAdapter("select h.id_beli as Nomer, b.tanggaljatuhtempo as \"Jatuh Tempo\", s.nama_supplier as Supplier ,format(h.total,0,'de_DE') as Total from hutang h, beli b, supplier s where h.id_beli = b.id_beli and h.status='" + status + "' and h.id_supplier = s.id_supplier order by b.tanggaljatuhtempo asc,Nomer desc", Koneksi.conn);
            ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            DataGridViewColumn col;
            col = dataGridView1.Columns[0]; col.Width = 80;
            col = dataGridView1.Columns[1]; col.Width = 75;
            col = dataGridView1.Columns[2]; col.Width = 200;
            col = dataGridView1.Columns[3]; col.Width = 120;
            col = dataGridView1.Columns[3]; col.Width = 100; dataGridView1.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            Koneksi.conn.Close();
            count();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Koneksi.openConn();
            da = new MySqlDataAdapter("select h.id_beli as Nomer, b.tanggaljatuhtempo as \"Jatuh Tempo\", s.nama_supplier as Supplier ,format(h.total,0,'de_DE') as Total from hutang h, beli b, supplier s where h.id_beli = b.id_beli and h.status='" + status + "' and h.id_supplier = s.id_supplier and s.nama_supplier like '%"+tbSearch.Text+ "%' order by b.tanggaljatuhtempo asc,Nomer desc", Koneksi.conn);
            ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            DataGridViewColumn col;
            col = dataGridView1.Columns[0]; col.Width = 80;
            col = dataGridView1.Columns[1]; col.Width = 75;
            col = dataGridView1.Columns[2]; col.Width = 200;
            col = dataGridView1.Columns[3]; col.Width = 80; dataGridView1.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            Koneksi.conn.Close();
            count();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string idbeli = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            Koneksi.openConn();
            cmd = new MySqlCommand("update hutang set status='1' where id_beli='"+idbeli+"' ",Koneksi.conn);
            cmd.ExecuteNonQuery();
            Koneksi.conn.Close();
            Koneksi.openConn();
            da = new MySqlDataAdapter("select h.id_beli as Nomer, b.tanggaljatuhtempo as \"Jatuh Tempo\", s.nama_supplier as Supplier ,format(h.total,0,'de_DE') as Total from hutang h, beli b, supplier s where h.id_beli = b.id_beli and h.status='" + status + "' and h.id_supplier = s.id_supplier order by b.tanggaljatuhtempo asc,Nomer desc", Koneksi.conn);
            ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            DataGridViewColumn col;
            col = dataGridView1.Columns[0]; col.Width = 80;
            col = dataGridView1.Columns[1]; col.Width = 75;
            col = dataGridView1.Columns[2]; col.Width = 200;
            col = dataGridView1.Columns[3]; col.Width = 80; dataGridView1.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            Koneksi.conn.Close();
            count();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Koneksi.openConn();
            da = new MySqlDataAdapter("select h.id_beli as Nomer, b.tanggaljatuhtempo as \"Jatuh Tempo\", s.nama_supplier as Supplier ,format(h.total,0,'de_DE') as Total from hutang h, beli b, supplier s where h.id_beli = b.id_beli and h.status='"+status+ "' and h.id_supplier = s.id_supplier order by b.tanggaljatuhtempo asc,Nomer desc", Koneksi.conn);
            ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            DataGridViewColumn col;
            col = dataGridView1.Columns[0]; col.Width = 80;
            col = dataGridView1.Columns[1]; col.Width = 75;
            col = dataGridView1.Columns[2]; col.Width = 200;
            col = dataGridView1.Columns[3]; col.Width = 80; dataGridView1.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            Koneksi.conn.Close();
            this.ActiveControl = tbSearch;
            count();
        }
    }
}
