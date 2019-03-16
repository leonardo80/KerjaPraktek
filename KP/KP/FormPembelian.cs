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
    public partial class FormPembelian : Form
    {

        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;

        public static string tempnamabarang = "";
        public static List<string> listnamasupplier = new List<string>();
        public static List<string> listidsupplier = new List<string>();
        public static List<string> listalamatsupplier = new List<string>();
        public static List<string> listkotasupplier = new List<string>();
        public static List<string> listnamabarang = new List<string>();
        public static List<string> listnota = new List<string>();

        public FormPembelian()
        {
            InitializeComponent();
        }

        public void hitungtotaltransaksi()
        {
            int temp = 0;
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                string[] total = dataGridView1.Rows[i].Cells[3].Value.ToString().Split('.');
                string gabungan = "";
                foreach (string a in total)
                {
                    gabungan = gabungan + a;
                }
                temp = temp + Convert.ToInt32(gabungan);
            }
            tbTotal.Text = temp.ToString();
            tbTotal.Text = Function.separator(tbTotal.Text);
        }

        public void settingdatagrid()
        {
            dataGridView1.ColumnCount = 4;
            dataGridView1.ColumnHeadersVisible = true;
            dataGridView1.Columns[0].Name = "Nama Barang";
            dataGridView1.Columns[1].Name = "Jumlah";
            dataGridView1.Columns[2].Name = "Kemasan";
            dataGridView1.Columns[3].Name = "Total";
            
            DataGridViewColumn col;
            col = dataGridView1.Columns[0]; col.Width = 260;
            col = dataGridView1.Columns[1]; col.Width = 90;
            col = dataGridView1.Columns[2]; col.Width = 90;
            col = dataGridView1.Columns[3]; col.Width = 150;
        }

        public void loaddatasupplier()
        {
            Koneksi.openConn();
            cmd = new MySqlCommand("select * from supplier", Koneksi.conn);
            dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                listnamasupplier.Add(dr.GetString(1));
            }
            cbSupplier.DataSource = listnamasupplier;
            Koneksi.conn.Close();
        }

        public void generatekodebeli()
        {
            Koneksi.openConn();
            string temp = tbTanggal.Text;
            string[] temp2 = temp.Split('-');
            string date = temp2[2] + temp2[1];
            cmd = new MySqlCommand("select count(*) from beli where substr(id_beli,1,4)='"+date+"'",Koneksi.conn);
            string ctr = cmd.ExecuteScalar().ToString();
            int urutan = Convert.ToInt32(ctr);
            urutan += 1;
            string kodebeli = urutan.ToString().PadLeft(4, '0');
            tbKodeBeli2.Text = kodebeli;
            tbKodeBeli1.Text=date;
            Koneksi.conn.Close();
        }

        public void loadbarang()
        {
            Koneksi.openConn();
            listnamabarang.Clear();
            string stm = "select * from barang order by nama_barang";
            MySqlCommand cmd = new MySqlCommand(stm, Koneksi.conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                listnamabarang.Add(dr.GetString(1));
            }
            Koneksi.conn.Close();
            cbBarang.Text = "";
            cbBarang.DataSource = listnamabarang;
        }

        public void activate()
        {
            tbKodeBeli.Enabled = true;
            tbKodeSupp.Enabled = true;
            cbSupplier.Enabled = true;
            tbKodeBarang.Enabled = true;
            cbBarang.Enabled = true;
            tbJumlah.Enabled = true;
            tbTotalBarang.Enabled = true;
        }

        private void FormPembelian_Load(object sender, EventArgs e)
        {
            this.ActiveControl = btnNew;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            activate(); loadbarang();loaddatasupplier();
            this.ActiveControl = cbSupplier;
            btnCancel.Enabled = true;
            DateTime date = DateTime.Now;
            string tanggal = date.Day.ToString();
            string bulan = date.Month.ToString();
            string tahun = date.Year.ToString();
            tanggal = tanggal.PadLeft(2, '0');
            bulan = bulan.PadLeft(2, '0');
            string thn = tahun.Substring(2, 2);
            string tanggalfull = tanggal + "-" + bulan + "-" + thn;
            tbTanggal.Text = tanggalfull;
            tbKodeBeli.Text = "B";
            dataGridView1.DataSource = null; dataGridView1.Refresh();
            generatekodebeli();settingdatagrid();
        }

        private void cbSupplier_Enter(object sender, EventArgs e)
        {
            cbSupplier.DroppedDown = true;
        }

        private void cbSupplier_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Koneksi.openConn();
                cmd = new MySqlCommand("select * from supplier where nama_supplier='" + cbSupplier.Text + "'", Koneksi.conn);
                dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    tbKodeSupp.Text = dr.GetString(0);
                    tbAlamat.Text = dr.GetString(2);
                    tbKota.Text = dr.GetString(3);
                }
                Koneksi.conn.Close();
            }
            catch (Exception x)
            {}
        }

        private void tbKodeSupp_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Koneksi.openConn();
                cmd = new MySqlCommand("select * from supplier where id_supplier='" + tbKodeSupp.Text + "'", Koneksi.conn);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    cbSupplier.Text = dr.GetString(1);
                    tbAlamat.Text = dr.GetString(2);
                    tbKota.Text = dr.GetString(3);
                }
                Koneksi.conn.Close();
            }
            catch (Exception x)
            { }
        }

        private void tbKodeSupp_Enter(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            FormBarang.selecttextbox(t);
        }

        private void cbBarang_Enter(object sender, EventArgs e)
        {
            cbBarang.DroppedDown = true;
        }

        private void cbBarang_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Koneksi.openConn();
                //this.ActiveControl = tbJumlah;
                sql = "select id_barang from barang where nama_barang='" + cbBarang.Text + "'";
                cmd = new MySqlCommand(sql, Koneksi.conn);
                string id = cmd.ExecuteScalar().ToString();
                tbKodeBarang.Text = id;
                sql = "select satuan_barang from barang where id_barang='" + tbKodeBarang.Text + "'";
                cmd = new MySqlCommand(sql, Koneksi.conn);
                string satuan = cmd.ExecuteScalar().ToString();
                tbSatuan.Text = satuan;
                Koneksi.conn.Close();
            }
            catch (Exception x)
            {}
        }

        private void tbKodeBarang_TextChanged(object sender, EventArgs e)
        {
            Koneksi.openConn();
            try
            {
                sql = "select nama_barang from barang where id_barang='" + tbKodeBarang.Text + "'";
                cmd = new MySqlCommand(sql, Koneksi.conn);
                string nama = cmd.ExecuteScalar().ToString();
                cbBarang.Text = nama;
                this.ActiveControl = cbBarang;
                sql = "select satuan_barang from barang where id_barang='" + tbKodeBarang.Text + "'";
                cmd = new MySqlCommand(sql, Koneksi.conn);
                string satuan = cmd.ExecuteScalar().ToString();
                tbSatuan.Text = satuan;
            }
            catch (Exception x)
            {

            }
            Koneksi.conn.Close();
        }

        private void cbBarang_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar==(char)Keys.Space)
            //{
            //    cbBarang.DroppedDown = true;
            //}
        }

        private void tbAdd_Click(object sender, EventArgs e)
        {
            if (tbKodeBarang.Text != "" && tbJumlah.Text != "0")
            {
                dataGridView1.Rows.Add(cbBarang.Text, tbJumlah.Text, tbSatuan.Text,tbTotalBarang.Text);                
                this.ActiveControl = tbKodeBarang;
                hitungtotaltransaksi();
            }
            else
            {
                MessageBox.Show("Kode Barang / Jumlah Tidak Boleh Kosong");
            }
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

        private void tbTotalBarang_TextChanged(object sender, EventArgs e)
        {
            if (tbTotalBarang.Text != "")
            {
                tbTotalBarang.Text = Function.separator(tbTotalBarang.Text);
                tbTotalBarang.SelectionStart = tbTotalBarang.TextLength;
                tbTotalBarang.SelectionLength = 0;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cbBarang.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            tbJumlah.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            tbSatuan.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            tbTotalBarang.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            btnUpdate.Enabled = true; tempnamabarang = cbBarang.Text;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = -1;
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.ToString() == tempnamabarang)
                {
                    index = i;
                }
            }
            dataGridView1.Rows.RemoveAt(index);
            dataGridView1.Refresh();
            hitungtotaltransaksi();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // barang
            cbBarang.DataSource = null; tbKodeBarang.Enabled = false;
            cbBarang.Enabled = false; tbJumlah.Enabled = false;tbTotalBarang.Enabled = false;
            tbJumlah.Text = "0";tbTotal.Text = "0";
            tbKodeBarang.Text = "";tbTotalBarang.Text = "0";
            //kop nota
            tbKodeBeli.Enabled = false; tbKodeSupp.Enabled = false; cbSupplier.Enabled = false;
            tbAlamat.Text = ""; tbKota.Text = ""; tbKodeSupp.Text = ""; tbKodeBeli.Text = "";
            cbSupplier.DataSource = null;
            tbTanggal.Text = ""; tbKodeBeli1.Text = ""; tbKodeBeli2.Text = "";
            //datagrid
            dataGridView1.Rows.Clear(); dataGridView1.Refresh(); tbTotalBarang.Text = "0";
            dataGridView1.DataSource = null;

            //button
            btnCancel.Enabled = false; btnUpdate.Enabled = false;

            //general
            this.ActiveControl = btnNew;
        }
    }
}
