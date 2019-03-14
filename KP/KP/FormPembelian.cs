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
            this.ActiveControl = cbBarang;
            dataGridView1.DataSource = null; dataGridView1.Refresh();
            generatekodebeli();
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
    }
}
