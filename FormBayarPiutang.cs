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
    public partial class FormBayarPiutang : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;

        public static string kodenota="";
        public static string nonota="";
        public static string status = "";

        public FormBayarPiutang()
        {
            InitializeComponent();
        }

        public void settingdatagrid()
        {
            DataGridViewColumn col;
            col = dataGridView1.Columns[0]; col.Width = 35;
            col = dataGridView1.Columns[1]; col.Width = 65;
            col = dataGridView1.Columns[2]; col.Width = 70;
            col = dataGridView1.Columns[3]; col.Width = 120;
            col = dataGridView1.Columns[4]; col.Width = 85;
            col = dataGridView1.Columns[5]; col.Width = 65;
            dataGridView1.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["Dibayarkan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        public void count()
        {
            Koneksi.openConn();
            int total = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string tagihan = dataGridView1.Rows[i].Cells[4].Value.ToString();
                string dibayarkan = dataGridView1.Rows[i].Cells[5].Value.ToString();
                tagihan = tagihan.Replace(".", "");
                dibayarkan = dibayarkan.Replace(".", "");
                total+=Convert.ToInt32(tagihan);
                total -= Convert.ToInt32(dibayarkan);
            }
            tbTotal.Text = total + "";
            tbTotal.Text = Function.separator(tbTotal.Text);
            Koneksi.conn.Close();
        }

        private void FormBayarPiutang_Load(object sender, EventArgs e)
        {
            Koneksi.openConn();
            da = new MySqlDataAdapter("select p.kode as Kode, p.id_jual as Nomor, j.tanggaljatuhtempo as \"Jatuh Tempo\", c.nama_customer as Customer ,format(p.total,0,'de_DE') as Total, format(p.dibayarkan,0,'de_DE') as Dibayarkan from piutang p, jual j, customer c where p.kode = j.kode and p.id_jual = j.id_jual and p.status='" + status + "' and p.id_customer = c.id_customer order by j.tanggaljatuhtempo asc,Nomor desc", Koneksi.conn);
            ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            settingdatagrid();
            Koneksi.conn.Close();
            this.ActiveControl = tbSearch;
            count();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Koneksi.openConn();
            da = new MySqlDataAdapter("select p.kode as Kode, p.id_jual as Nomor, j.tanggaljatuhtempo as \"Jatuh Tempo\", c.nama_customer as Customer ,format(p.total,0,'de_DE') as Total, format(p.dibayarkan,0,'de_DE') as Dibayarkan from piutang p, jual j, customer c where p.kode = j.kode and p.id_jual = j.id_jual and p.id_customer = c.id_customer and p.status='" + status + "'  and c.nama_customer like '%" + tbSearch.Text+ "%' order by j.tanggaljatuhtempo asc,Nomor desc", Koneksi.conn);
            ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            settingdatagrid();
            Koneksi.conn.Close(); count();
        }

        private void btnBayar_Click(object sender, EventArgs e)
        {
            if (tbJumlah.Text!="" && nonota!="")
            {
                int tag = 0; int bay = 0;
                tag = Convert.ToInt32(tbTagihan.Text.Replace(".",""));
                bay = Convert.ToInt32(tbJumlah.Text.Replace(".",""));
                if (tag-bay<0)
                {
                    MessageBox.Show("Periksa Kembali Nominal Anda");
                    this.ActiveControl = tbJumlah;
                }
                else
                {
                    Koneksi.openConn();
                    cmd = new MySqlCommand("update piutang set dibayarkan=dibayarkan+'"+tbJumlah.Text.Replace(".","")+"' where kode='"+kodenota+"' and id_jual='"+nonota+"' ",Koneksi.conn);
                    cmd.ExecuteNonQuery();
                    if (tag-bay==0)
                    {
                        cmd = new MySqlCommand("update piutang set status='1' where kode='" + kodenota + "' and id_jual='" + nonota + "' ", Koneksi.conn);
                        cmd.ExecuteNonQuery();
                    }
                    Koneksi.conn.Close();

                    DateTime dt = DateTime.Now;
                    //insert into table dpiutang
                    Koneksi.openConn();
                    cmd = new MySqlCommand("insert into dpiutang values('"+kodenota+"','"+nonota+"','"+dt.ToString("dd-MM-yy")+"','"+tbJumlah.Text.Replace(".","")+"')", Koneksi.conn);
                    cmd.ExecuteNonQuery();
                    Koneksi.conn.Close();

                    tbTagihan.Text = ""; tbJumlah.Text = "";
                    kodenota = ""; nonota = "";
                }
            }
            Koneksi.openConn();
            da = new MySqlDataAdapter("select p.kode as Kode, p.id_jual as Nomor, j.tanggaljatuhtempo as \"Jatuh Tempo\", c.nama_customer as Customer ,format(p.total,0,'de_DE') as Total, format(p.dibayarkan,0,'de_DE') as Dibayarkan from piutang p, jual j, customer c where p.kode = j.kode and p.id_jual = j.id_jual and p.id_customer = c.id_customer and p.status='" + status+"' and c.nama_customer like '%" + tbSearch.Text + "%' order by j.tanggaljatuhtempo asc,Nomor desc", Koneksi.conn);
            ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            dataGridView1.DataSource = ds.Tables[0];
            settingdatagrid();
            Koneksi.conn.Close(); count();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {

                string tagihan = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                string dibayarkan = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                string gabunganTagihan = tagihan.Replace(".", "");
                string gabunganBayar = dibayarkan.Replace(".", "");
                tbTagihan.Text = Convert.ToInt32(gabunganTagihan) - Convert.ToInt32(gabunganBayar) + "";
                tbTagihan.Text = Function.separator(tbTagihan.Text);
                tbJumlah.Text = Convert.ToInt32(gabunganTagihan) - Convert.ToInt32(gabunganBayar) + "";
                this.ActiveControl = tbJumlah;
                kodenota = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                nonota = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                count();
            }
        }

        private void tbJumlah_Enter(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            FormBarang.selecttextbox(t);
        }

        private void tbJumlah_TextChanged(object sender, EventArgs e)
        {
            if (tbJumlah.Text != "")
            {
                tbJumlah.Text = Function.separator(tbJumlah.Text);
                tbJumlah.SelectionStart = tbJumlah.TextLength;
                tbJumlah.SelectionLength = 0;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked==true)
            {
                status = "1";
                tbJumlah.Text = "";

            }
            else
            {
                status = "0";
            }
            Koneksi.openConn();
            da = new MySqlDataAdapter("select p.kode as Kode, p.id_jual as Nomor, j.tanggaljatuhtempo as \"Jatuh Tempo\", c.nama_customer as Customer ,format(p.total,0,'de_DE') as Total, format(p.dibayarkan,0,'de_DE') as Dibayarkan from piutang p, jual j, customer c where p.kode = j.kode and p.id_jual = j.id_jual and p.status='" + status + "' and p.id_customer = c.id_customer order by j.tanggaljatuhtempo asc,Nomor desc", Koneksi.conn);
            ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            settingdatagrid();
            Koneksi.conn.Close(); count();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Koneksi.openConn();
            da = new MySqlDataAdapter("select p.kode as Kode, p.id_jual as Nomor, j.tanggaljatuhtempo as \"Jatuh Tempo\", c.nama_customer as Customer ,format(p.total,0,'de_DE') as Total, format(p.dibayarkan,0,'de_DE') as Dibayarkan from piutang p, jual j, customer c where p.kode = j.kode and p.id_jual = j.id_jual and p.status='" + status + "' and p.id_customer = c.id_customer order by j.tanggaljatuhtempo asc,Nomor desc", Koneksi.conn);
            ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            settingdatagrid();
            Koneksi.conn.Close();
            this.ActiveControl = tbSearch;
            count();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (nonota!="")
            {
                FormDetailPembayaran f = new FormDetailPembayaran();
                f.Show();
            }
        }
    }
}
