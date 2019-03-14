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
    public partial class FormPenjualan : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;
        public bool view=true;
        public static string tempnamabarang = "";
        public int jumlahdatagrid = 0;
        public int jumlahtransaksi = 0;
        public static List<string> listnamacust = new List<string>();
        public static List<string> listidcust = new List<string>();
        public static List<string> listalamatcust = new List<string>();
        public static List<string> listkotacust = new List<string>();
        public static List<string> listnamabarang = new List<string>();
        public static List<string> listnota = new List<string>();

        public FormPenjualan()
        {
            InitializeComponent();
        }

        public void settingdatagrid()
        {
            //dataGridView1.DataSource = null;
            dataGridView1.ColumnCount = 7;
            dataGridView1.ColumnHeadersVisible = true;
            dataGridView1.Columns[0].Name = "Nama Barang";
            dataGridView1.Columns[1].Name = "Jumlah";
            dataGridView1.Columns[2].Name = "Kemasan";
            dataGridView1.Columns[3].Name = "Harga";
            dataGridView1.Columns[4].Name = "Disc 1";
            dataGridView1.Columns[5].Name = "Disc 2";
            dataGridView1.Columns[6].Name = "Total";

            DataGridViewColumn col;
            col = dataGridView1.Columns[0];col.Width = 280;
            col = dataGridView1.Columns[1]; col.Width = 70;
            col = dataGridView1.Columns[2]; col.Width = 60;
            col = dataGridView1.Columns[3]; col.Width = 90;
            col = dataGridView1.Columns[4]; col.Width = 50;
            col = dataGridView1.Columns[5]; col.Width = 50;
            col = dataGridView1.Columns[6]; col.Width = 100;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[6].Value != null)
                {
                    row.Cells[6].Value = Function.separator(row.Cells[6].Value.ToString());
                }
                if (row.Cells[4].Value != null)
                {
                    row.Cells[4].Value = Function.separator(row.Cells[4].Value.ToString());
                }
            }
        }

        public void cekdatagrid()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[6].Value != null)
                {
                    row.Cells[6].Value = Function.separator(row.Cells[6].Value.ToString());
                }
                if (row.Cells[4].Value != null)
                {
                    row.Cells[4].Value = Function.separator(row.Cells[4].Value.ToString());
                }
            }
        }

        public void activate()
        {
            tbKodeJual.Enabled = true;
            tbKodeCust.Enabled = true;
            cbCustomer.Enabled = true;
            tbKodeBarang.Enabled = true;
            cbBarang.Enabled = true;
            tbJumlah.Enabled = true;
            tbHarga.Enabled = true;
            tbDiskon1.Enabled = true;
            tbDiskon2.Enabled = true;
        }

        public void deactivate()
        {
            tbKodeJual.Enabled = false;
            tbKodeCust.Enabled = false;
            cbCustomer.Enabled = false;
            tbKodeBarang.Enabled = false;
            cbBarang.Enabled = false;
            tbJumlah.Enabled = false;
            tbHarga.Enabled = false;
            tbDiskon1.Enabled = false;
            tbDiskon2.Enabled = false;
        }

        public void kosongitexbox()
        {
            tbKodeBarang.Text = "";
            cbBarang.Text = "";
            tbJumlah.Text = "";
            tbHarga.Text = "";
            tbDiskon1.Text = "";
            tbDiskon2.Text = "";
            tbTotalBarang.Text = "";
        }

        public void hitungtotalperbarang(string jumlah, string harga, string d1, string d2)
        {
            double total = 0;
            int qty = Convert.ToInt32(jumlah);
            int price = Convert.ToInt32(harga);
            double diskon1 = Convert.ToDouble(d1);
            double diskon2 = Convert.ToDouble(d2);
            total = qty * price;
            total = total * (100 - diskon1)/100;
            total = total * (100 - diskon2)/100;
            tbTotalBarang.Text= Convert.ToInt32(total).ToString();
            tbTotalBarang.Text = Function.separator(tbTotalBarang.Text);
        }

        public void generatekodejual(string jenis, string tahunbulan)
        {
            DateTime date = DateTime.Now;
            string tanggal = date.Day.ToString();
            string bulan = date.Month.ToString();
            string tahun = date.Year.ToString();
            tanggal = tanggal.PadLeft(2, '0');
            bulan = bulan.PadLeft(2, '0');
            string thn = tahun.Substring(2, 2);
            string tanggalfull = tanggal + "-" + bulan + "-" + thn;
            tbTanggal.Text = tanggalfull;
            tbKodeJual1.Text = thn + bulan;
            Koneksi.openConn();
            sql = "select count(*) from jual where kode='" + jenis + "' and substr(id_jual,1,4)='" + tahunbulan + "';";
            cmd = new MySqlCommand(sql, Koneksi.conn);
            string temp = cmd.ExecuteScalar().ToString();
            Koneksi.conn.Close();
            int urutan = Convert.ToInt32(temp) + 1;
            string kodejual = urutan.ToString().PadLeft(4, '0');
            tbKodeJual2.Text = kodejual;
            tbKodeJual.Text = jenis;
        }

        public void loadcustomer(string kode)
        {
            listidcust.Clear();
            listnamacust.Clear();
            listalamatcust.Clear();
            listkotacust.Clear();
            Koneksi.openConn();
            string stm = "select * from customer where status='"+kode+"' order by nama_customer";
            MySqlCommand cmd = new MySqlCommand(stm, Koneksi.conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                listidcust.Add(dr.GetString(0));
                listnamacust.Add(dr.GetString(1));
                listalamatcust.Add(dr.GetString(2));
                listkotacust.Add(dr.GetString(3));
            }
            Koneksi.conn.Close();
            cbCustomer.Text = "";
            cbCustomer.DataSource = null;
            cbCustomer.DataSource = listnamacust;
        }
        
        public void loaddatacustomer(string idcust)
        {
            Koneksi.openConn();
            string stm = "select * from customer where id_customer='" + idcust + "'";
            cmd = new MySqlCommand(stm, Koneksi.conn);
            dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                cbCustomer.Text = dr.GetString(1);
                tbAlamat.Text = dr.GetString(2);
                tbKota.Text = dr.GetString(3);
            }
            tbKodeCust.Text = idcust;
            Koneksi.conn.Close();
        }

        public void loaddatanota()
        {
            string kode = tbKodeJual.Text;
            string nonota = tbKodeJual1.Text + "-" + tbKodeJual2.Text;
            try
            {
                Koneksi.openConn();
                da = new MySqlDataAdapter("select nama_barang as \"Nama Barang\", jumlah_barang as \"Jumlah\", kemasan as \"Kemasan\", hargasatuan as \"Harga\", diskon1 as D1, diskon2 as D2,totalharga as Total from djual where kode='" + kode + "' and id_jual='" + nonota + "' ", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                //dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                dataGridView1.DataSource = ds.Tables[0];
                Koneksi.conn.Close();
            }
            catch (Exception x)
            { MessageBox.Show(x.Message);}
            
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
            cbBarang.Text="";
            cbBarang.DataSource = listnamabarang;
        }

        public void isidatanota()
        {
            tbKodeJual.Text = FormFindPenjualan.kode;
            string temp = FormFindPenjualan.listnonota[FormFindPenjualan.index];
            string[] temp2 = temp.Split('-');
            tbKodeJual1.Text = temp2[0];
            tbKodeJual2.Text = temp2[1];
            Koneksi.openConn();
            cmd = new MySqlCommand("select tanggal from jual where kode='" + FormFindPenjualan.kode + "' and id_jual='" + temp2[0] + "-" + temp2[1] + "'", Koneksi.conn);
            string tanggal = cmd.ExecuteScalar().ToString();
            tbTanggal.Text = tanggal;
            Koneksi.conn.Close();
        }

        public void loadnota(string jenis)
        {
            Koneksi.openConn();
            listnota.Clear();
            string stm = "select * from jual where kode='"+jenis+"' order by id_jual desc";
            MySqlCommand cmd = new MySqlCommand(stm, Koneksi.conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                listnota.Add(dr.GetString(1));
            }
            loaddetailnota(jenis, listnota[listnota.Count() - 1]);
            Koneksi.conn.Close();
        }

        public void loaddetailnota(string jenis, string nonota)
        {
            try
            {
                Koneksi.openConn();
                string stm = "select nama_barang as \"Nama Barang\", jumlah_barang as \"Jumlah\", hargasatuan as \"Harga\", diskon1 as \"Nama Barang\", diskon2 as \"Nama Barang\",totalharga as \"Nama Barang\" from djual where kode='" + jenis + "' and id_jual='" + nonota + "' order by nama_barang";
                da = new MySqlDataAdapter(stm, Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                Koneksi.conn.Close();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
            
        }

        public void hitungtotaltransaksi()
        {
            int temp = 0;
            for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
            {
                string[] total = dataGridView1.Rows[i].Cells[6].Value.ToString().Split('.');
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

        private void FormPenjualan_Load(object sender, EventArgs e)
        {
            this.ActiveControl = btnNew;
            if (FormFindPenjualan.index!=-1)
            {
                isidatanota();
                loaddatanota();
                loaddatacustomer(FormFindPenjualan.listidcust[FormFindPenjualan.index]);
                btnEdit.Enabled = true;
                hitungtotaltransaksi();
                btnPrev.Enabled = true;
                btnNext.Enabled = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            view = false;FormFindPenjualan.index = -1;
            activate();loadbarang();btnCancel.Enabled = true;
            DateTime date = DateTime.Now;
            string tanggal = date.Day.ToString();
            string bulan = date.Month.ToString();
            string tahun = date.Year.ToString();
            tanggal = tanggal.PadLeft(2, '0');
            bulan = bulan.PadLeft(2, '0');
            string thn = tahun.Substring(2, 2);
            string tanggalfull = tanggal + "-" + bulan + "-" + thn;
            tbTanggal.Text = tanggalfull;
            tbKodeJual1.Text = thn + bulan;
            tbKodeJual.Text = "PJ";
            FormBarang.selecttextbox(tbKodeJual);
            this.ActiveControl = tbKodeJual;
            dataGridView1.DataSource = null;dataGridView1.Refresh();
            generatekodejual(tbKodeJual.Text, tbKodeJual1.Text);
            settingdatagrid();
        }

        private void tbKodeJual_Leave(object sender, EventArgs e)
        {
            if (tbKodeJual.Text != "LP" || tbKodeJual.Text != "PJ")
            {

            }
            else
            {
                MessageBox.Show("Kode Jual Harus PJ / LP");
                this.ActiveControl = tbKodeJual;
            }
        }

        private void tbKodeJual_Enter(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            FormBarang.selecttextbox(t);
        }

        private void cbCustomer_TextChanged(object sender, EventArgs e)
        {
            string temp = cbCustomer.Text;
            temp = temp.ToUpper();
            sql = "select id_customer from customer where substr(nama_customer,1,1)='" + temp + "'";
            try
            {
                Koneksi.openConn();
                cmd = new MySqlCommand(sql, Koneksi.conn);
                string id = cmd.ExecuteScalar().ToString();
                for (int i = 0; i < listidcust.Count(); i++)
                {
                    if (listidcust[i] == id)
                    {
                        cbCustomer.SelectedIndex = i;
                    }
                }
                Koneksi.conn.Close();
            }
            catch (Exception x)
            {

            }
        }

        private void cbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listidcust.Count() != 0)
            {
                tbKodeCust.Text = listidcust[cbCustomer.SelectedIndex];
                tbAlamat.Text = listalamatcust[cbCustomer.SelectedIndex];
                tbKota.Text = listkotacust[cbCustomer.SelectedIndex];
            }           
        }

        private void tbAdd_Click(object sender, EventArgs e)
        {
            if (tbKodeBarang.Text!="" && tbJumlah.Text!="0" && jumlahdatagrid+1!=10)
            {
                dataGridView1.Rows.Add(cbBarang.Text, tbJumlah.Text, tbSatuan.Text, tbHarga.Text, tbDiskon1.Text, tbDiskon2.Text, Function.separator(tbTotalBarang.Text));
                jumlahdatagrid++;
                hitungtotaltransaksi();
                tbJumlahDatagrid.Text = jumlahdatagrid.ToString();
                this.ActiveControl = cbBarang;
            }
            else
            {
                MessageBox.Show("Kode Barang / Jumlah Tidak Boleh Kosong");
            }
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
                sql = "select harga_barang from barang where id_barang='" + tbKodeBarang.Text + "'";
                cmd = new MySqlCommand(sql, Koneksi.conn);
                string harga = cmd.ExecuteScalar().ToString();
                tbHarga.Text = harga;
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

        private void tbKodeCust_TextChanged(object sender, EventArgs e)
        {
            Koneksi.openConn();
            try
            {
                sql = "select nama_customer from customer where id_customer='" + tbKodeCust.Text + "'";
                cmd = new MySqlCommand(sql, Koneksi.conn);
                string nama = cmd.ExecuteScalar().ToString();
                cbCustomer.Text = nama;
                //this.ActiveControl = tbKodeBarang;
            }
            catch (Exception x)
            {

            }
            Koneksi.conn.Clone();
        }

        private void tbJumlah_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar)) e.Handled = true;
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

        private void tbHarga_TextChanged(object sender, EventArgs e)
        {
            if (tbHarga.Text != "")
            {
                tbHarga.Text = Function.separator(tbHarga.Text);
                tbHarga.SelectionStart = tbHarga.TextLength;
                tbHarga.SelectionLength = 0;
            }
        }

        private void tbJumlah_Leave(object sender, EventArgs e)
        {
            string[] temp = tbHarga.Text.Split('.');
            string gabunganharga = "";
            foreach (string a in temp)
            {
                gabunganharga = gabunganharga + a;
            }
            string[] temp2 = tbJumlah.Text.Split('.');
            string gabunganjumlah = "";
            foreach (string a in temp2)
            {
                gabunganjumlah = gabunganjumlah + a;
            }
            string gabungandiskon1 = "", gabungandiskon2 = "";
            try
            {
                string[] temp3 = tbDiskon1.Text.Split('.');
                gabungandiskon1 = temp3[0] + "," + temp3[1];
                
            }
            catch (Exception x)
            {
                gabungandiskon1 = tbDiskon1.Text;
            }
            try
            {
                string[] temp4 = tbDiskon2.Text.Split('.');
                gabungandiskon2 = temp4[0] + "," + temp4[1];

            }
            catch (Exception x)
            {
                gabungandiskon2 = tbDiskon2.Text;
            }
            hitungtotalperbarang(gabunganjumlah, gabunganharga, gabungandiskon1, gabungandiskon2);
        }

        private void cbBarang_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Koneksi.openConn();
                tbDiskon1.Text = "0.00";tbDiskon2.Text = "0.00";
                this.ActiveControl = tbJumlah;
                sql = "select id_barang from barang where nama_barang='" + cbBarang.Text + "'";
                cmd = new MySqlCommand(sql, Koneksi.conn);
                string id = cmd.ExecuteScalar().ToString();
                tbKodeBarang.Text = id;
                sql = "select harga_barang from barang where id_barang='" + tbKodeBarang.Text + "'";
                cmd = new MySqlCommand(sql, Koneksi.conn);
                string harga = cmd.ExecuteScalar().ToString();
                tbHarga.Text = harga;
                sql = "select satuan_barang from barang where id_barang='" + tbKodeBarang.Text + "'";
                cmd = new MySqlCommand(sql, Koneksi.conn);
                string satuan = cmd.ExecuteScalar().ToString();
                tbSatuan.Text = satuan;
                string[] temp = tbHarga.Text.Split('.');
                string gabungan = "";
                foreach (string a in temp)
                {
                    gabungan = gabungan + a;
                }
                hitungtotalperbarang(tbJumlah.Text, gabungan, tbDiskon1.Text, tbDiskon2.Text);
                Koneksi.conn.Close();
            }
            catch (Exception x)
            {

            }
        }

        private void tbKodeJual_TextChanged(object sender, EventArgs e)
        {
            if (FormFindPenjualan.index!=-1)
            {
                loaddatacustomer(FormFindPenjualan.cust);
            }
            else
            {
                loadcustomer(tbKodeJual.Text);
                generatekodejual(tbKodeJual.Text, tbKodeJual1.Text);
            }            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // barang
            cbBarang.DataSource = null;tbKodeBarang.Enabled = false;
            cbBarang.Enabled = false;tbJumlah.Enabled = false;
            tbHarga.Enabled = false;tbDiskon1.Enabled = false;tbDiskon2.Enabled = false;
            tbJumlah.Text = "0"; tbHarga.Text = "0"; tbTotal.Text = "0";
            tbKodeBarang.Text = ""; tbDiskon1.Text = "0.00"; tbDiskon2.Text = "0.00";
            //kop nota
            tbKodeJual.Enabled = false;tbKodeCust.Enabled = false;cbCustomer.Enabled = false;
            tbAlamat.Text = ""; tbKota.Text = ""; tbKodeCust.Text = ""; tbKodeJual.Text = "";
            cbCustomer.DataSource = null;
            tbTanggal.Text = ""; tbKodeJual1.Text = ""; tbKodeJual2.Text = "";
            //datagrid
            dataGridView1.Rows.Clear();dataGridView1.Refresh();tbTotalBarang.Text = "Total";
            dataGridView1.DataSource = null;
           
            //button
            btnCancel.Enabled = false;btnUpdate.Enabled = false;

            //general
            this.ActiveControl = btnNew;
           
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cbBarang.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            tbJumlah.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            tbSatuan.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            tbHarga.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            tbDiskon1.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            tbDiskon2.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            btnUpdate.Enabled = true;tempnamabarang = cbBarang.Text;
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
            jumlahdatagrid--;
            dataGridView1.Rows.RemoveAt(index);
            dataGridView1.Refresh();
            hitungtotaltransaksi();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int index = -1;
            for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.ToString()==tempnamabarang)
                {
                    index = i;
                }
            }
            dataGridView1.Rows[index].Cells[0].Value = cbBarang.Text;
            dataGridView1.Rows[index].Cells[1].Value = tbJumlah.Text;
            dataGridView1.Rows[index].Cells[2].Value = tbSatuan.Text;
            dataGridView1.Rows[index].Cells[3].Value = tbHarga.Text;
            dataGridView1.Rows[index].Cells[4].Value = tbDiskon1.Text;
            dataGridView1.Rows[index].Cells[5].Value = tbDiskon2.Text;
            dataGridView1.Rows[index].Cells[6].Value = tbTotalBarang.Text;
            hitungtotaltransaksi();btnUpdate.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Koneksi.openConn();
            string nama="", jumlah = "", kemasan = "", harga = "", d1 = "", d2 = "", total = "";
            string kodenota = tbKodeJual.Text;
            string nonota = tbKodeJual1.Text + "-" + tbKodeJual2.Text;
            for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
            {
                nama = dataGridView1.Rows[i].Cells[0].Value.ToString();
                jumlah = dataGridView1.Rows[i].Cells[1].Value.ToString();
                kemasan = dataGridView1.Rows[i].Cells[2].Value.ToString();
                harga = dataGridView1.Rows[i].Cells[3].Value.ToString();
                d1 = dataGridView1.Rows[i].Cells[4].Value.ToString();
                d2 = dataGridView1.Rows[i].Cells[5].Value.ToString();
                total = dataGridView1.Rows[i].Cells[6].Value.ToString();
                string[] temp2 = total.Split('.');
                string gabungantotal2 = "";
                foreach (string a in temp2)
                {
                    gabungantotal2 += a;
                }
                string[] temp3 = harga.Split('.');
                string gabungantotal3 = "";
                foreach (string a in temp3)
                {
                    gabungantotal3 += a;
                }
                string[] temp4 = jumlah.Split('.');
                string gabungantotal4 = "";
                foreach (string a in temp4)
                {
                    gabungantotal4 += a;
                }
                //insert into table djual
                cmd = new MySqlCommand("insert into djual values('" + kodenota + "', '" + nonota + "', '" + nama+"', '"+gabungantotal4+"', '"+kemasan+"', '"+gabungantotal3+"', '"+d1+"', '"+d2+"', '"+gabungantotal2+"')", Koneksi.conn);
                cmd.ExecuteNonQuery();

                //update stok
                cmd = new MySqlCommand("select keluar from stok where nama_barang='" + nama + "'", Koneksi.conn);
                int keluar = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                cmd = new MySqlCommand("select stok from stok where nama_barang='" + nama + "'", Koneksi.conn);
                int stok = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                keluar += Convert.ToInt32(gabungantotal4);
                stok -= keluar;
                cmd = new MySqlCommand("update stok set keluar='"+keluar+"' , stok='"+stok+"' where nama_barang='"+nama+"'",Koneksi.conn);
                cmd.ExecuteNonQuery();
                this.ActiveControl = btnNew;
            }
            string[] temp = tbTotal.Text.Split('.');
            string gabungantotal = "";
            foreach (string a in temp)
            {
                gabungantotal += a;
            }
            //insert into table jual
            cmd = new MySqlCommand("insert into jual values('"+kodenota+"', '"+nonota+"', '"+tbKodeCust.Text+"', '"+tbTanggal.Text+"', '"+gabungantotal+"')", Koneksi.conn);
            cmd.ExecuteNonQuery();
            Koneksi.conn.Close();


            // barang
            cbBarang.DataSource = null; tbKodeBarang.Enabled = false;
            cbBarang.Enabled = false; tbJumlah.Enabled = false;
            tbHarga.Enabled = false; tbDiskon1.Enabled = false; tbDiskon2.Enabled = false;tbSatuan.Text = "";
            tbJumlah.Text = "0"; tbHarga.Text = "0"; tbTotal.Text = "0";
            tbKodeBarang.Text = ""; tbDiskon1.Text = "0.00"; tbDiskon2.Text = "0.00";
            //kop nota
            tbKodeJual.Enabled = false; tbKodeCust.Enabled = false; cbCustomer.Enabled = false;
            tbAlamat.Text = ""; tbKota.Text = ""; tbKodeCust.Text = ""; tbKodeJual.Text = "";
            cbCustomer.DataSource = null;
            tbTanggal.Text = ""; tbKodeJual1.Text = ""; tbKodeJual2.Text = "";
            //datagrid
            dataGridView1.Rows.Clear(); dataGridView1.Refresh(); tbTotalBarang.Text = "Total";
            dataGridView1.DataSource = null;

            //button
            btnCancel.Enabled = false; btnUpdate.Enabled = false;

            //general
            this.ActiveControl = btnNew;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            FormFindPenjualan f = new FormFindPenjualan();
            this.Close();
            f.Show();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (FormFindPenjualan.index - 1 < 0)
            {
                FormFindPenjualan.index = FormFindPenjualan.listnonota.Count() - 1;
            }
            else
            {
                FormFindPenjualan.index--;
            }
            isidatanota();loaddatanota();hitungtotaltransaksi();
            loaddatacustomer(FormFindPenjualan.listidcust[FormFindPenjualan.index]);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (FormFindPenjualan.index + 1 > FormFindPenjualan.listnonota.Count() - 1)
            {
                FormFindPenjualan.index = 0;
            }
            else
            {
                FormFindPenjualan.index++;
            }
            isidatanota();loaddatanota();hitungtotaltransaksi();
            loaddatacustomer(FormFindPenjualan.listidcust[FormFindPenjualan.index]);
        }

        private void cbCustomer_Enter(object sender, EventArgs e)
        {
            cbCustomer.DroppedDown = true;
        }

        private void cbBarang_Enter(object sender, EventArgs e)
        {
            cbBarang.DroppedDown = true;
        }
    }
}
