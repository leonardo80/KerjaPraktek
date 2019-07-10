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
        public bool lagiedit = false;
        public static List<string> isidatagrid = new List<string>();
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

        public void cleardatagrid()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
        }

        public void cekJumlahDatagrid()
        {
            jumlahdatagrid = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                jumlahdatagrid++;
            }
            tbJumlahDatagrid.Text = jumlahdatagrid.ToString();
        }

        public void settingdatagrid()
        {
            dataGridView1.DataSource = null;
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
            col = dataGridView1.Columns[0]; col.Width = 280;
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
            tbTanggal.Enabled = true;
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
            tbTanggal.Enabled = false;
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
            Koneksi.openConn();
            listnamacust.Clear();
            string stm = "select nama_customer from customer";
            MySqlCommand cmd = new MySqlCommand(stm, Koneksi.conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                listnamacust.Add(dr.GetString(0));
            }
            Koneksi.conn.Close();
            cbCustomer.Text = "";
            cbCustomer.Items.AddRange(listnamacust.ToArray<string>());
            cbCustomer.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbCustomer.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //listidcust.Clear();
            //listnamacust.Clear();
            //listalamatcust.Clear();
            //listkotacust.Clear();
            //Koneksi.openConn();
            //string stm = "select * from customer where status='"+kode+"' order by nama_customer";
            //MySqlCommand cmd = new MySqlCommand(stm, Koneksi.conn);
            //dr = cmd.ExecuteReader();
            //while (dr.Read())
            //{
            //    listidcust.Add(dr.GetString(0));
            //    listnamacust.Add(dr.GetString(1));
            //    listalamatcust.Add(dr.GetString(2));
            //    listkotacust.Add(dr.GetString(3));
            //}
            //Koneksi.conn.Close();
            //cbCustomer.Text = "";
            //cbCustomer.DataSource = null;
            //cbCustomer.DataSource = listnamacust;
        }
        
        public void loaddatacustomer(string idcust)
        {
            Koneksi.openConn();
            string stm = "select id_customer,nama_customer,alamat_customer,kota_customer from customer where id_customer='" + idcust + "'";
            cmd = new MySqlCommand(stm, Koneksi.conn);
            MySqlDataReader dr2;
            dr2 = cmd.ExecuteReader();
            while (dr2.Read())
            {
                cbCustomer.Text = dr2.GetString(1);
                tbAlamat.Text = dr2.GetString(2);
                tbKota.Text = dr2.GetString(3);
            }
            tbKodeCust.Text = idcust;
            Koneksi.conn.Close();
        }

        public void loaddatanota()
        {
            string kode = tbKodeJual.Text;
            string nonota = tbKodeJual1.Text + "-" + tbKodeJual2.Text;
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            try
            {
                Koneksi.openConn();
                da = new MySqlDataAdapter("select nama_barang as \"Nama Barang\", jumlah_barang as \"Jumlah\", kemasan as \"Kemasan\", format(hargasatuan,0,'de_DE') as \"Harga\", diskon1 as D1, diskon2 as D2,totalharga as Total from djual where kode='" + kode + "' and id_jual='" + nonota + "' ", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                settingdatagrid();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dataGridView1.Rows.Add(ds.Tables[0].Rows[i][0].ToString(),Function.separator(ds.Tables[0].Rows[i][1].ToString()), ds.Tables[0].Rows[i][2].ToString(), Function.separator(ds.Tables[0].Rows[i][3].ToString()), ds.Tables[0].Rows[i][4].ToString(), ds.Tables[0].Rows[i][5].ToString(), Function.separator(ds.Tables[0].Rows[i][6].ToString()));
                }
                Koneksi.conn.Close();
                Koneksi.openConn();
                cmd = new MySqlCommand("select void from jual where kode='" + kode + "' and id_jual='" + nonota + "' ", Koneksi.conn);
                if (cmd.ExecuteScalar().ToString()=="1")
                {
                    lbVoid.Visible = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                }
                else
                {
                    lbVoid.Visible = false;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                }
                cmd=new MySqlCommand("select printed from jual where kode='" + kode + "' and id_jual='" + nonota + "' ", Koneksi.conn);
                lbPrinted.Text = cmd.ExecuteScalar().ToString();
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
            cbBarang.Items.AddRange(listnamabarang.ToArray<string>());
            cbBarang.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbBarang.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
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
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string gabungan = dataGridView1.Rows[i].Cells[6].Value.ToString().Replace(".", "");
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
                btnPrev.Enabled = true;
                btnNext.Enabled = true;
            }
            else
            {
                Koneksi.openConn();
                cmd = new MySqlCommand("select id_jual, id_customer,tanggal from jual order by id_jual desc limit 1", Koneksi.conn);
                MySqlDataReader dr;
                dr = cmd.ExecuteReader();
                tbKodeJual.Text = "PJ";
                string[] nota;
                while (dr.Read())
                {
                    nota = dr.GetString(0).Split('-');
                    tbKodeJual1.Text = nota[0];
                    tbKodeJual2.Text = nota[1];
                    tbKodeCust.Text = dr.GetString(1);
                    tbTanggal.Text = dr.GetString(2);
                }
                loaddatanota();
                Koneksi.conn.Close();
            }
            hitungtotaltransaksi();
            cekJumlahDatagrid();
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
            this.ActiveControl = tbTanggal;
            dataGridView1.DataSource = null;dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            generatekodejual(tbKodeJual.Text, tbKodeJual1.Text);
            settingdatagrid();
            tbAdd.Enabled = true;
            btnNew.Enabled = false; btnEdit.Enabled = false;
            btnDelete.Enabled = false;  btnFind.Enabled = false;
            btnPrint.Enabled = false;
            lbVoid.Visible = false;
            checkBox1.Visible = true;
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
            try
            {
                Koneksi.openConn();
                cmd = new MySqlCommand("select id_customer from customer where nama_customer='"+cbCustomer.Text+"'", Koneksi.conn);
                tbKodeCust.Text = cmd.ExecuteScalar().ToString();
                Koneksi.conn.Close();
            }
            catch (Exception x)
            {}
            //temp = temp.toupper();
            //sql = "select id_customer from customer where substr(nama_customer,1,1)='" + temp + "'";
            //try
            //{
            //    koneksi.openconn();
            //    cmd = new mysqlcommand(sql, koneksi.conn);
            //    string id = cmd.executescalar().tostring();
            //    for (int i = 0; i < listidcust.count(); i++)
            //    {
            //        if (listidcust[i] == id)
            //        {
            //            cbcustomer.selectedindex = i;
            //        }
            //    }
            //    koneksi.conn.close();
            //}
            //catch (exception x)
            //{

            //}
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
            if (checkBox1.Checked==false)
            {
                if (tbKodeBarang.Text != "" && tbJumlah.Text != "0" && jumlahdatagrid + 1 != 10)
                {
                    int stok = Convert.ToInt32(lbStok.Text);
                    string gabungantotal2 = tbJumlah.Text.Replace(".", "");
                    int jmlh = Convert.ToInt32(gabungantotal2);
                    if (stok - jmlh >= 0)
                    {
                        dataGridView1.Rows.Add(cbBarang.Text, tbJumlah.Text, tbSatuan.Text, tbHarga.Text, tbDiskon1.Text, tbDiskon2.Text, Function.separator(tbTotalBarang.Text));
                        hitungtotaltransaksi();
                        this.ActiveControl = tbKodeBarang;
                    }
                    else
                    {
                        MessageBox.Show("Stok Barang Tidak Mencukupi");
                    }
                }
                else
                {
                    MessageBox.Show("Kode Barang / Jumlah Tidak Boleh Kosong");
                }
            }
            else
            {
                if (tbKodeBarang.Text != "" && tbJumlah.Text != "0" && jumlahdatagrid + 1 != 10)
                {
                    string gabungantotal2 = tbJumlah.Text.Replace(".", "");
                    dataGridView1.Rows.Add(cbBarang.Text, tbJumlah.Text, tbSatuan.Text, tbHarga.Text, tbDiskon1.Text, tbDiskon2.Text, Function.separator(tbTotalBarang.Text));
                    hitungtotaltransaksi();
                    this.ActiveControl = tbKodeBarang;
                }
                else
                {
                    MessageBox.Show("Kode Barang / Jumlah Tidak Boleh Kosong");
                }
            }
            
            cekJumlahDatagrid();
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

                cmd = new MySqlCommand("select stok from stok where id_barang='" + tbKodeBarang.Text + "'", Koneksi.conn);
                lbStok.Text = cmd.ExecuteScalar().ToString();
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
                Koneksi.conn.Close();
                Koneksi.openConn();
                cmd = new MySqlCommand("select alamat_customer, kota_customer from customer where id_customer='"+tbKodeCust.Text+"'",Koneksi.conn);
                dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    tbAlamat.Text = dr.GetString(0);
                    tbKota.Text = dr.GetString(1);
                }

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
            string gabunganharga = tbHarga.Text.Replace(".", "");
            string gabunganjumlah = tbJumlah.Text.Replace(".", "");
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
                //this.ActiveControl = tbJumlah;
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
                cmd = new MySqlCommand("select stok from stok where id_barang='" + tbKodeBarang.Text + "'", Koneksi.conn);
                lbStok.Text = cmd.ExecuteScalar().ToString();
                //cmd = new MySqlCommand("select nama_barang from barang where nama_barang like '" + cbBarang.Text + "%' and status='Aktif'", Koneksi.conn);
                //cbBarang.Text = cmd.ExecuteScalar().ToString();
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
            cleardatagrid();
            if (lagiedit==true)
            {
                lagiedit = false;
                string kodenota = tbKodeJual.Text;
                string nonota = tbKodeJual1.Text + "-" + tbKodeJual2.Text;
                for (int i = 0; i < isidatagrid.Count; i++)
                {
                    string[] temp = isidatagrid[i].Split('-');
                    dataGridView1.Rows.Add(temp[0], temp[1], temp[2], temp[3],temp[4],temp[5],temp[6]);
                }
                Koneksi.openConn();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    cmd = new MySqlCommand("select id_barang from barang where nama_barang='" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "'", Koneksi.conn);
                    string idbarang = cmd.ExecuteScalar().ToString();
                    cmd = new MySqlCommand("select hpp from hpp where id_barang='" + idbarang + "'", Koneksi.conn);
                    string currhpp = cmd.ExecuteScalar().ToString();
                    cmd = new MySqlCommand("insert into djual values('" + kodenota + "','" + nonota + "','" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[1].Value.ToString().Replace(".","") + "','" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[3].Value.ToString().Replace(".", "") + "','" + dataGridView1.Rows[i].Cells[4].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[5].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[6].Value.ToString() + "','"+currhpp+"')", Koneksi.conn);
                    cmd.ExecuteNonQuery();
                }
                Koneksi.conn.Close();
            }
            // barang
            cbBarang.DataSource = null; tbKodeBarang.Enabled = false;
            cbBarang.Enabled = false; tbJumlah.Enabled = false;
            tbHarga.Enabled = false; tbDiskon1.Enabled = false; tbDiskon2.Enabled = false;
            tbJumlah.Text = "0"; tbHarga.Text = "0"; tbTotal.Text = "0";
            tbKodeBarang.Text = ""; tbDiskon1.Text = "0.00"; tbDiskon2.Text = "0.00";lbStok.Text = "0";

            //kop nota
            tbKodeJual.Enabled = false; tbKodeCust.Enabled = false; cbCustomer.Enabled = false;tbTanggal.Enabled = false;
            ///*tbAlamat.Text = ""; tbKota.Text = ""; tbKodeCust.Text = ""; */tbKodeJual.Text = "";
            //cbCustomer.DataSource = null;
            //tbTanggal.Text = ""; tbKodeJual1.Text = ""; tbKodeJual2.Text = "";

            //dataGridView1.Rows.Clear();
            //dataGridView1.Refresh();tbTotalBarang.Text = "Total";
            //dataGridView1.DataSource = null;

            //button
            btnCancel.Enabled = false;btnUpdate.Enabled = false;
            btnNew.Enabled = true;btnEdit.Enabled = true;btnDelete.Enabled = true;btnFind.Enabled = true;

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
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.ToString() == tempnamabarang)
                {
                    index = i;
                }
            }
            //jumlahdatagrid--;
            dataGridView1.Rows.RemoveAt(index);
            dataGridView1.Refresh();
            hitungtotaltransaksi();
            cekJumlahDatagrid();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int index = -1;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.ToString()==tempnamabarang)
                {
                    index = i;
                }
            }

            if (index!=-1)
            {
                string hargabarang = tbHarga.Text.Replace(".", "");
                string jumlahbarang = tbJumlah.Text.Replace(".", ""); 
                string totalbarang = tbTotalBarang.Text.Replace(".", "");
                dataGridView1.Rows[index].Cells[0].Value = cbBarang.Text;
                dataGridView1.Rows[index].Cells[1].Value = Function.separator(jumlahbarang);
                dataGridView1.Rows[index].Cells[2].Value = tbSatuan.Text;
                dataGridView1.Rows[index].Cells[3].Value = Function.separator(hargabarang);
                dataGridView1.Rows[index].Cells[4].Value = tbDiskon1.Text;
                dataGridView1.Rows[index].Cells[5].Value = tbDiskon2.Text;
                dataGridView1.Rows[index].Cells[6].Value = Function.separator(totalbarang);
                hitungtotaltransaksi(); btnUpdate.Enabled = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string nama="", jumlah = "", kemasan = "", harga = "", d1 = "", d2 = "", total = "";
            string kodenota = tbKodeJual.Text;
            string nonota = tbKodeJual1.Text + "-" + tbKodeJual2.Text;

            //dropship
            int totalbeli = 0;
            if (checkBox1.Checked==true)
            {
                //generate kode nota
                Koneksi.openConn();
                string temp = tbTanggal.Text;
                string[] temp2 = temp.Split('-');
                string date = temp2[2] + temp2[1];
                cmd = new MySqlCommand("select count(*) from beli where substr(id_beli,1,4)='" + date + "'", Koneksi.conn);
                string ctr = cmd.ExecuteScalar().ToString();
                int urutan = Convert.ToInt32(ctr);
                urutan += 1;
                string kodebeli = urutan.ToString().PadLeft(4, '0');
                string nomernota = date + '-' + kodebeli;
                Koneksi.conn.Close();

                Koneksi.openConn();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    nama = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    jumlah = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    kemasan = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    string strjumlah = jumlah.Replace(".", "");

                    //get id barang
                    cmd = new MySqlCommand("select id_barang from barang where nama_barang='" + nama + "'", Koneksi.conn);
                    string idbarang = cmd.ExecuteScalar().ToString();

                    //get hpp barang
                    cmd = new MySqlCommand("select hpp from hpp where id_barang='"+idbarang+"'",Koneksi.conn);
                    string hppnow = cmd.ExecuteScalar().ToString();

                    //insert into table dbeli
                    cmd = new MySqlCommand("insert into dbeli values('B', '" + nomernota + "', '" + nama + "', '" + strjumlah + "', '" + kemasan + "','" + hppnow + "', '0', '0', '" + Convert.ToInt32(strjumlah)*Convert.ToInt32(hppnow)+ "')", Koneksi.conn);
                    cmd.ExecuteNonQuery();

                    totalbeli+= Convert.ToInt32(strjumlah) * Convert.ToInt32(hppnow);
                    
                    //update stok                
                    cmd = new MySqlCommand("select masuk from stok where id_barang='" + idbarang + "'", Koneksi.conn);
                    int masuk = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    cmd = new MySqlCommand("select stok from stok where id_barang='" + idbarang + "'", Koneksi.conn);
                    int stok = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    masuk += Convert.ToInt32(strjumlah);
                    stok += Convert.ToInt32(strjumlah);
                    cmd = new MySqlCommand("update stok set masuk='" + masuk + "' , stok='" + stok + "' where id_barang='" + idbarang + "'", Koneksi.conn);
                    cmd.ExecuteNonQuery();
                    this.ActiveControl = btnNew;
                }

                //date time itu yyyy-mm-dd
                string tglnow = tbTanggal.Text;
                string[] splittgl = tglnow.Split('-');
                string insertedtanggal = splittgl[2] + "-" + splittgl[1] + "-" + splittgl[0];

                //get idsupp
                string kodesupp = "";
                if (cbSupp.SelectedIndex == 0) kodesupp = "0000";
                if (cbSupp.SelectedIndex == 1) kodesupp = "0001";
                               
                //insert into table beli
                DateTime dt = Convert.ToDateTime(tbTanggal.Text);
                dt = dt.AddDays(42);
                cmd = new MySqlCommand("insert into beli values('B', '" + nomernota + "', '" + kodesupp
                    + "', '" + insertedtanggal + "', '" + totalbeli + "','" + dt.ToString("yyyy-MM-dd") + "')", Koneksi.conn);
                cmd.ExecuteNonQuery();
                Koneksi.conn.Close();

                //insert into table hutang
                Koneksi.openConn();
                cmd = new MySqlCommand("insert into hutang values('" + nomernota + "','" + kodesupp + "','" + totalbeli + "','0')", Koneksi.conn);
                cmd.ExecuteNonQuery();
                Koneksi.conn.Close();
                Koneksi.conn.Close();
            }

            //insert
            Koneksi.openConn();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                nama = dataGridView1.Rows[i].Cells[0].Value.ToString();
                jumlah = dataGridView1.Rows[i].Cells[1].Value.ToString();
                kemasan = dataGridView1.Rows[i].Cells[2].Value.ToString();
                harga = dataGridView1.Rows[i].Cells[3].Value.ToString();
                d1 = dataGridView1.Rows[i].Cells[4].Value.ToString();
                d2 = dataGridView1.Rows[i].Cells[5].Value.ToString();
                total = dataGridView1.Rows[i].Cells[6].Value.ToString();
                string gabungantotal2 = total.Replace(".", "");
                string gabungantotal3 = harga.Replace(".", "");
                string gabungantotal4 = jumlah.Replace(".", "");

                //get current hpp
                cmd = new MySqlCommand("select id_barang from barang where nama_barang='" + nama + "'", Koneksi.conn);
                string idbarang = cmd.ExecuteScalar().ToString();
                cmd = new MySqlCommand("select hpp from hpp where id_barang='" + idbarang + "'", Koneksi.conn);
                string currhpp = cmd.ExecuteScalar().ToString();

                //insert into table djual
                cmd = new MySqlCommand("insert into djual values('" + kodenota + "', '" + nonota + "', '" + nama + "', '" + gabungantotal4 + "', '" + kemasan + "', '" + gabungantotal3 + "', '" + d1 + "', '" + d2 + "', '" + gabungantotal2 + "','"+currhpp+"')", Koneksi.conn);
                cmd.ExecuteNonQuery();

                //update stok
                cmd = new MySqlCommand("select keluar from stok where id_barang='" + idbarang + "'", Koneksi.conn);
                int keluar = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                cmd = new MySqlCommand("select stok from stok where id_barang='" + idbarang + "'", Koneksi.conn);
                int stok = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                stok -= Convert.ToInt32(gabungantotal4);
                keluar += Convert.ToInt32(gabungantotal4);
                cmd = new MySqlCommand("update stok set keluar='" + keluar + "' , stok='" + stok + "' where id_barang='" + idbarang + "'", Koneksi.conn);
                cmd.ExecuteNonQuery();
                this.ActiveControl = btnNew;
            }
            string gabungantotal = tbTotal.Text.Replace(".", "");

            if (!lagiedit)
            {
                string tglnow = tbTanggal.Text;
                string[] splittgl = tglnow.Split('-');
                string insertedtanggal = splittgl[2] + "-" + splittgl[1] + "-" + splittgl[0];

                //insert into table jual
                DateTime dt = Convert.ToDateTime(tbTanggal.Text);
                dt = dt.AddDays(42);
                cmd = new MySqlCommand("insert into jual values('" + kodenota + "', '" + nonota + "', '" + tbKodeCust.Text + "', '" + insertedtanggal + "', '" + gabungantotal + "','0','0','" + dt.ToString("yyyy-MM-dd") + "')", Koneksi.conn);
                cmd.ExecuteNonQuery();
                Koneksi.conn.Close();
                
                //insert into table piutang
                Koneksi.openConn();
                cmd = new MySqlCommand("insert into piutang values('" + kodenota + "','" + nonota + "','" + tbKodeCust.Text + "','" + gabungantotal + "','0','0')", Koneksi.conn);
                cmd.ExecuteNonQuery();
                Koneksi.conn.Close();
            }
            
            // barang
            cbBarang.DataSource = null; tbKodeBarang.Enabled = false;
            cbBarang.Enabled = false; tbJumlah.Enabled = false;
            tbHarga.Enabled = false; tbDiskon1.Enabled = false; tbDiskon2.Enabled = false; tbSatuan.Text = "";
            tbJumlah.Text = "0"; tbHarga.Text = "0"; tbTotal.Text = "0";
            tbKodeBarang.Text = ""; tbDiskon1.Text = "0.00"; tbDiskon2.Text = "0.00";lbStok.Text = "0";tbJumlahDatagrid.Text = "0";
            cbBarang.Text = "";
            //kop nota
            tbKodeJual.Enabled = false; tbKodeCust.Enabled = false; cbCustomer.Enabled = false;tbTanggal.Enabled = false;
            tbAlamat.Text = ""; tbKota.Text = ""; tbKodeCust.Text = ""; tbKodeJual.Text = "";
            cbCustomer.DataSource = null;
            tbTanggal.Text = ""; tbKodeJual1.Text = ""; tbKodeJual2.Text = "";
            //datagrid
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh(); tbTotalBarang.Text = "Total";

            //button
            btnCancel.Enabled = false; btnUpdate.Enabled = false;tbAdd.Enabled = false;
            btnNew.Enabled = true;btnEdit.Enabled = true;btnDelete.Enabled = true;
            btnFind.Enabled = true; btnPrint.Enabled = true;

            //checkBox1.Visible = false;
            //cbSupp.Visible = false;
            
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
            //cbCustomer.DroppedDown = true;
        }

        private void cbBarang_Enter(object sender, EventArgs e)
        {
            //cbBarang.DroppedDown = true;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (Login.activeuser=="SUPERVISOR")
            {
                if (tbKodeJual1.Text != "")
                {
                    lagiedit = true;
                    activate(); loadbarang(); btnCancel.Enabled = true;
                    string kodenota = tbKodeJual.Text;
                    string nonota = tbKodeJual1.Text + "-" + tbKodeJual2.Text;
                    isidatagrid.Clear();
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        string nama = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        string jumlah = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        string kemasan = dataGridView1.Rows[i].Cells[2].Value.ToString();
                        string harga = dataGridView1.Rows[i].Cells[3].Value.ToString();
                        string d1 = dataGridView1.Rows[i].Cells[4].Value.ToString();
                        string d2 = dataGridView1.Rows[i].Cells[5].Value.ToString();
                        string total = dataGridView1.Rows[i].Cells[6].Value.ToString();
                        isidatagrid.Add(nama + "-" + jumlah + "-" + kemasan + "-" + harga + "-" + d1 + "-" + d2 + "-" + total);
                    }
                    //kembalikan stok ke semula
                    List<string> barangdaritable = new List<string>();
                    MySqlDataReader dr3;
                    Koneksi.openConn();
                    barangdaritable.Clear();
                    cmd = new MySqlCommand("select b.id_barang, dj.jumlah_barang from djual dj, barang b where b.nama_barang=dj.nama_barang and kode='" + kodenota + "' and id_jual='" + nonota + "'", Koneksi.conn);
                    dr3 = cmd.ExecuteReader();
                    while(dr3.Read())
                    {
                        barangdaritable.Add(dr3.GetString(0) + "-" + dr3.GetString(1));
                    }
                    Koneksi.conn.Close();

                    for (int i = 0; i < barangdaritable.Count; i++)
                    {
                        string[] temp = barangdaritable[i].Split('-');
                        //Koneksi.openConn();
                        //cmd = new MySqlCommand("select id_barang from barang where nama_barang='" + temp[0] + "'", Koneksi.conn);
                        //string idbarang = cmd.ExecuteScalar().ToString();
                        //Koneksi.conn.Close();

                        Koneksi.openConn();
                        cmd = new MySqlCommand("select stok from stok where id_barang='" + temp[0] + "'", Koneksi.conn);
                        MessageBox.Show(cmd.ExecuteScalar().ToString());
                        cmd = new MySqlCommand("update stok set keluar=keluar-"+temp[1]+", stok=stok+"+temp[1]+" where id_barang='"+temp[0]+"'",Koneksi.conn);
                        cmd.ExecuteNonQuery();
                        Koneksi.conn.Close();
                    }

                    //hapus data barang dari djual
                    Koneksi.openConn();
                    cmd = new MySqlCommand("delete from djual where kode='" + kodenota + "' and id_jual='" + nonota + "'", Koneksi.conn);
                    cmd.ExecuteNonQuery();
                    Koneksi.conn.Close();


                    //kop
                    tbTanggal.Enabled = false; tbKodeCust.Enabled = false;
                    cbCustomer.Enabled = false; tbKodeJual.Enabled = false;
                    tbAdd.Enabled = true;

                }
            }
            else
            {
                MessageBox.Show("Anda Tidak Punya Hak");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {            
            if (Login.activeuser=="SUPERVISOR")
            {
                DialogResult res = MessageBox.Show("Hapus Nota?", "Notice", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    Koneksi.openConn();
                    cmd = new MySqlCommand("update jual set void='1' where kode='" + tbKodeJual.Text + "' and id_jual='" + tbKodeJual1.Text + "-" + tbKodeJual2.Text + "'", Koneksi.conn);
                    cmd.ExecuteNonQuery();
                    Koneksi.conn.Close();

                    //kembalikan stok
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        Koneksi.openConn();
                        cmd = new MySqlCommand("select id_barang from barang where nama_barang='" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "'", Koneksi.conn);
                        string idbarang = cmd.ExecuteScalar().ToString();
                        Koneksi.conn.Close();
                        Koneksi.openConn();
                        cmd = new MySqlCommand("update stok set keluar=keluar-" + dataGridView1.Rows[i].Cells[1].Value.ToString() + ", stok=stok+" + dataGridView1.Rows[i].Cells[1].Value.ToString() + " where id_barang='" + idbarang + "'", Koneksi.conn);
                        cmd.ExecuteNonQuery();
                        Koneksi.conn.Close();
                    }
                }
                loaddatanota();
            }
            else
            {
                MessageBox.Show("Anda Tidak Punya Hak");
            }
        }

        private void tbTanggal_Leave(object sender, EventArgs e)
        {
            DateTime temp;
            if (!DateTime.TryParse(tbTanggal.Text, out temp))
            {
                if (tbTanggal.TextLength != 8)
                {
                    MessageBox.Show("Periksa Kembali Date Format");
                    this.ActiveControl = tbTanggal;
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Nama", typeof(string)));
            dt.Columns.Add(new DataColumn("Jumlah", typeof(string)));
            dt.Columns.Add(new DataColumn("Kemasan", typeof(string)));
            dt.Columns.Add(new DataColumn("Harga", typeof(string)));
            dt.Columns.Add(new DataColumn("Diskon1", typeof(string)));
            dt.Columns.Add(new DataColumn("Diskon2", typeof(string)));
            dt.Columns.Add(new DataColumn("Total", typeof(string)));

            barang1 = new Dataset.Barang();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dt.Rows.Add(
                    dataGridView1.Rows[i].Cells[0].Value.ToString(),
                    dataGridView1.Rows[i].Cells[1].Value.ToString(),
                    dataGridView1.Rows[i].Cells[2].Value.ToString(),
                    dataGridView1.Rows[i].Cells[3].Value.ToString(),
                    dataGridView1.Rows[i].Cells[4].Value.ToString(),
                    dataGridView1.Rows[i].Cells[5].Value.ToString(),
                    dataGridView1.Rows[i].Cells[6].Value.ToString()
                    );
            }

            Nota np = new Nota();
            np.Database.Tables["Barang"].SetDataSource(dt);
            np.SetParameterValue("totalnota", tbTotal.Text);
            np.SetParameterValue("tanggal", tbTanggal.Text);
            np.SetParameterValue("kodenota", tbKodeJual.Text + "/" + tbKodeJual1.Text + "-" + tbKodeJual2.Text);
            np.SetParameterValue("nama_customer", cbCustomer.Text);
            np.SetParameterValue("alamat", tbAlamat.Text);
            np.SetParameterValue("kota", tbKota.Text);

            ReportViewer v = new ReportViewer();
            v.crvTransaksi.ReportSource = null;
            v.crvTransaksi.ReportSource = np;
            v.crvTransaksi.Refresh();
            v.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked==true)
            {
                cbSupp.Visible = true;
            }
            else
            {
                cbSupp.Visible = false;
            }
            cbSupp.SelectedIndex = 0;
        }
    }
}
