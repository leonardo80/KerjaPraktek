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

        public void separator()
        {
            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[4].Value = Function.separator(dataGridView1.Rows[i].Cells[4].Value.ToString());
                }

            }
            catch (Exception x)
            { MessageBox.Show(x.Message);}
        }

        public void hitungtotaltransaksi()
        {
            int temp = 0;
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                string[] total = dataGridView1.Rows[i].Cells[4].Value.ToString().Split('.');
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
            dataGridView1.ColumnCount = 5;
            dataGridView1.ColumnHeadersVisible = true;
            dataGridView1.Columns[0].Name = "Nama Barang";
            dataGridView1.Columns[1].Name = "Jumlah";
            dataGridView1.Columns[2].Name = "Harga";
            dataGridView1.Columns[3].Name = "Kemasan";
            dataGridView1.Columns[4].Name = "Total";
            
            DataGridViewColumn col;
            col = dataGridView1.Columns[0]; col.Width = 250;
            col = dataGridView1.Columns[1]; col.Width = 70;
            col = dataGridView1.Columns[2]; col.Width = 80;
            col = dataGridView1.Columns[3]; col.Width = 70;
            col = dataGridView1.Columns[4]; col.Width = 130;
        }

        public void isidatanota()
        {
            tbKodeBeli.Text = "B";
            string temp = FormFindPembelian.listnonota[FormFindPembelian.index];
            string[] nota = temp.Split('-');
            tbKodeBeli1.Text = nota[0];
            tbKodeBeli2.Text = nota[1];
            Koneksi.openConn();
            cmd = new MySqlCommand("select tanggal from beli where id_beli='"+temp+"'", Koneksi.conn);
            string tanggal = cmd.ExecuteScalar().ToString();
            tbTanggal.Text = tanggal;
            Koneksi.conn.Close();
        }

        public void loaddatanota()
        {
            string temp = FormFindPembelian.listnonota[FormFindPembelian.index];
            try
            {
                Koneksi.openConn();
                da = new MySqlDataAdapter("select nama_barang as \"Nama Barang\", jumlah_barang as \"Jumlah\",kemasan as Kemasan, hargasatuan as \"Harga\",totalharga as Total from dbeli where id_beli='" + temp + "' ", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                //dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                dataGridView1.DataSource = ds.Tables[0];

                DataGridViewColumn col;
                col = dataGridView1.Columns[0]; col.Width = 250;
                col = dataGridView1.Columns[1]; col.Width = 70;
                col = dataGridView1.Columns[2]; col.Width = 80;
                col = dataGridView1.Columns[3]; col.Width = 70;
                col = dataGridView1.Columns[4]; col.Width = 130;
                Koneksi.conn.Close();
            }
            catch (Exception x)
            {}
        }

        public void loadinfosupp(string id)
        {
            Koneksi.openConn();
            string stm = "select * from supplier where id_supplier='" + id + "'";
            cmd = new MySqlCommand(stm, Koneksi.conn);
            MySqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                cbSupplier.Text = dr.GetString(1);
                tbAlamat.Text = dr.GetString(2);
                tbKota.Text = dr.GetString(3);
            }
            tbKodeSupp.Text = id;
            Koneksi.conn.Close();
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
            if (FormFindPembelian.index != -1)
            {
                isidatanota();
                loaddatanota();
                loadinfosupp(FormFindPembelian.supp);
                btnEdit.Enabled = true;
                hitungtotaltransaksi();
                btnPrev.Enabled = true;
                btnNext.Enabled = true;
            }
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
            tbKodeBeli.Enabled = false;
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
                Koneksi.conn.Close();
                Koneksi.openConn();
                sql = "select satuan_barang from barang where id_barang='" + tbKodeBarang.Text + "'";
                cmd = new MySqlCommand(sql, Koneksi.conn);
                string satuan = cmd.ExecuteScalar().ToString();
                tbSatuan.Text = satuan;

                Koneksi.conn.Close();
                Koneksi.openConn();
                cmd = new MySqlCommand("select nama_barang from barang where nama_barang like '" + cbBarang.Text + "%' and status='Aktif'", Koneksi.conn);
                cbBarang.Text = cmd.ExecuteScalar().ToString();
                Koneksi.conn.Close();
                Koneksi.openConn();
                cmd = new MySqlCommand("select hpp from hpp where id_barang='" + tbKodeBarang.Text + "'", Koneksi.conn);
                tbHarga.Text = cmd.ExecuteScalar().ToString();
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
                dataGridView1.Rows.Add(cbBarang.Text, tbJumlah.Text, tbHarga.Text, tbSatuan.Text,tbTotalBarang.Text);                
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
            tbSatuan.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            tbTotalBarang.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
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
            tbHarga.Text = "0";
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            Koneksi.openConn();
            string nama = "", jumlah = "",total = "",harga="",kemasan="";
            string kodenota = tbKodeBeli.Text;
            string nonota = tbKodeBeli1.Text + "-" + tbKodeBeli2.Text;
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                nama = dataGridView1.Rows[i].Cells[0].Value.ToString();
                jumlah = dataGridView1.Rows[i].Cells[1].Value.ToString();
                harga = dataGridView1.Rows[i].Cells[2].Value.ToString();
                kemasan= dataGridView1.Rows[i].Cells[3].Value.ToString();
                total = dataGridView1.Rows[i].Cells[4].Value.ToString();
                string[] temp2 = total.Split('.');
                string gabungantotal2 = ""; // total per barang
                foreach (string a in temp2)
                {
                    gabungantotal2 += a;
                }
                string[] temp4 = jumlah.Split('.');
                string gabungantotal4 = ""; //jumlah barang
                foreach (string a in temp4)
                {
                    gabungantotal4 += a;
                }
                string[] temp5 = harga.Split('.');
                string gabungantotal5 = ""; //harga barang
                foreach (string a in temp5)
                {
                    gabungantotal5 += a;
                }
                //insert into table dbeli
                cmd = new MySqlCommand("insert into dbeli values('" + kodenota + "', '" + nonota + "', '" + nama + "', '" + gabungantotal4 + "', '"+kemasan+"','"+gabungantotal5+"', '0', '0', '" + gabungantotal2 + "')", Koneksi.conn);
                cmd.ExecuteNonQuery();

                //update table hpp
                cmd = new MySqlCommand("select id_barang from barang where nama_barang='" + nama + "'", Koneksi.conn);
                string idbarang = cmd.ExecuteScalar().ToString();
                int tempstok = 0, temphpp = 0;
                double newhpp = 0;
                cmd = new MySqlCommand("select stok from stok where id_barang='" + idbarang + "'", Koneksi.conn);
                tempstok = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                cmd = new MySqlCommand("select hpp from hpp where id_barang='" + idbarang + "'", Koneksi.conn);
                temphpp = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                newhpp = (tempstok * temphpp) + (Convert.ToInt32(gabungantotal4) * Convert.ToInt32(gabungantotal5));
                newhpp = newhpp / (tempstok + Convert.ToInt32(gabungantotal4));
                cmd = new MySqlCommand("update hpp set hpp='" + newhpp + "' where id_barang='" + idbarang + "'", Koneksi.conn);
                cmd.ExecuteNonQuery();

                //update stok                
                cmd = new MySqlCommand("select keluar from stok where id_barang='" + idbarang + "'", Koneksi.conn);
                int keluar = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                cmd = new MySqlCommand("select stok from stok where id_barang='" + idbarang + "'", Koneksi.conn);
                int stok = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                keluar += Convert.ToInt32(gabungantotal4);
                stok += keluar;
                cmd = new MySqlCommand("update stok set masuk='" + keluar + "' , stok='" + stok + "' where id_barang='" + idbarang + "'", Koneksi.conn);
                cmd.ExecuteNonQuery();
                this.ActiveControl = btnNew;
            }
            string[] temp = tbTotal.Text.Split('.');
            string gabungantotal = ""; //total nota beli
            foreach (string a in temp)
            {
                gabungantotal += a;
            }
            //insert into table beli
            cmd = new MySqlCommand("insert into beli values('" + kodenota + "', '" + nonota + "', '" + tbKodeSupp.Text + "', '" + tbTanggal.Text + "', '" + gabungantotal + "')", Koneksi.conn);
            cmd.ExecuteNonQuery();
            Koneksi.conn.Close();
            
            // barang
            cbBarang.DataSource = null; tbKodeBarang.Enabled = false;
            cbBarang.Enabled = false; tbJumlah.Enabled = false;tbSatuan.Text = "";
            tbJumlah.Text = "0"; tbTotal.Text = "0";tbTotal.Enabled = false;
            tbKodeBarang.Text = "";cbBarang.Text = "";tbHarga.Text = "0";
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int index = -1;
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.ToString() == tempnamabarang)
                {
                    index = i;
                }
            }
            dataGridView1.Rows[index].Cells[0].Value = cbBarang.Text;
            dataGridView1.Rows[index].Cells[1].Value = tbJumlah.Text;
            dataGridView1.Rows[index].Cells[3].Value = tbSatuan.Text;
            dataGridView1.Rows[index].Cells[4].Value = tbTotalBarang.Text;
            hitungtotaltransaksi(); btnUpdate.Enabled = false;
        }

        private void tbHarga_TextChanged(object sender, EventArgs e)
        {
            if (tbHarga.Text != "")
            {
                tbHarga.Text = Function.separator(tbHarga.Text);
                tbHarga.SelectionStart = tbHarga.TextLength;
                tbHarga.SelectionLength = 0;
            }
        }

        private void tbTotalBarang_Leave(object sender, EventArgs e)
        {
            if (tbTotalBarang.Text != "0" && tbJumlah.Text != "0")
            {
                double temp = 0;
                string[] tempJumlah = tbJumlah.Text.Split('.');
                string gabunganJumlah = "";
                foreach (string item in tempJumlah)
                {
                    gabunganJumlah += item;
                }
                string[] tempTotal = tbTotalBarang.Text.Split('.');
                string gabunganTotal = "";
                foreach (string item in tempTotal)
                {
                    gabunganTotal += item;
                }
                double jumlah = Convert.ToDouble(gabunganJumlah);
                double total = Convert.ToDouble(gabunganTotal);
                temp = total / jumlah;
                temp = Math.Round(temp, 0);
                tbHarga.Text = Function.separator(temp.ToString());
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            this.Close();
            FormFindPembelian f = new FormFindPembelian();
            f.Show();
        }
    }
}
