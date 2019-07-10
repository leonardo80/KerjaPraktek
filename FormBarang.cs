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
    public partial class FormBarang : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;
        public static string namabarang = "";
        public static bool newbarang = false, selected = false;
        public static List<string> listkategori = new List<string>();

        public FormBarang()
        {
            InitializeComponent();
        }
        
        public void cekstatus()
        {
            Koneksi.openConn();
            sql = "select nama_barang from barang where id_barang='" + tbId.Text + "'";
            cmd = new MySqlCommand(sql, Koneksi.conn);
            try
            {
                string nama = cmd.ExecuteScalar().ToString();
                lbStatus.Text = "Unavailable";
                btnConfirm.Enabled = false;

            }
            catch (Exception)
            {
                lbStatus.Text = "Available";
                btnConfirm.Enabled = true;
            }
            Koneksi.conn.Close();
        }

        public static void selecttextbox(TextBox t)
        {
            t.SelectionStart = 0;
            t.SelectionLength = t.Text.Length;
        }

        public void loadkategori()
        {
            Koneksi.openConn();
            listkategori.Clear();
            cmd = new MySqlCommand("select nama_kategori from kategori order by nama_kategori asc",Koneksi.conn);
            MySqlDataReader dr;
            dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                listkategori.Add(dr.GetString(0));
            }
            cbKategori.DataSource = listkategori;
            Koneksi.conn.Close();
        }

        public void refreshlist()
        {
            FormFindBarang.eraselistbarang();
            FormFindBarang.loadlistbarang();
        }
        public void enabletextbox()
        {
            tbId.Enabled = true;
            tbNama.Enabled = true;
            tbKemasan.Enabled = true;
            tbHarga.Enabled = true;
            cbKategori.Enabled = true;
            tbDeskripsi.Enabled = true;
        }

        public void disabletextbox()
        {
            tbId.Enabled = false;
            tbNama.Enabled = false;
            tbKemasan.Enabled = false;
            tbHarga.Enabled = false;
            cbKategori.Enabled = false;
            tbDeskripsi.Enabled = false;
        }

        public void kosongitextbox()
        {
            tbId.Text = "";tbNama.Text = "";tbKemasan.Text = "";tbHarga.Text = "";
            tbDeskripsi.Text = "";cbKategori.Text = "";
        }

        private void isidatabarang()
        {
            tbId.Text = FormFindBarang.listid[FormFindBarang.index];
            tbNama.Text = FormFindBarang.listnama[FormFindBarang.index];
            tbKemasan.Text = FormFindBarang.listsatuan[FormFindBarang.index];
            tbHarga.Text = FormFindBarang.listharga[FormFindBarang.index];
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (Koneksi.conn!=null && Koneksi.conn.State==ConnectionState.Open)
            {
                Koneksi.conn.Close();
            }
            lbStatus.Text = "Available";
            enabletextbox();
            kosongitextbox();
            loadkategori();
            btnConfirm.Enabled = true;
            btnCancel.Enabled = true;
            btnNew.Enabled = false;
            btnDelete.Enabled = false;

            tbMasuk.Text = "";
            tbKeluar.Text = "";
            tbStok.Text = "";
            newbarang = true;
            this.ActiveControl = tbId;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (tbId.Enabled!=false)
            {
                btnNew.Enabled = true; btnCancel.Enabled = false; btnConfirm.Enabled = false;
                kosongitextbox(); disabletextbox();
            }
            else
            {
                btnCancel.Enabled = false; btnConfirm.Enabled = false;
                btnNew.Enabled = true;disabletextbox(); btnDelete.Enabled = true;
            }
            newbarang = false;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {            
            if (tbId.Enabled==false)
            {
                Koneksi.openConn();
                cmd = new MySqlCommand("update barang set nama_barang ='"+tbNama.Text+"', satuan_barang='"+tbKemasan.Text+"', harga_barang='"+tbHarga.Text.Replace(".","")+"', nama_kategori='"+cbKategori.Text+"', deskripsi='"+tbDeskripsi.Text+"' where id_barang='"+tbId.Text+"' ", Koneksi.conn);
                cmd.ExecuteNonQuery();
                btnCancel.Enabled = false;
                btnConfirm.Enabled = false;
                disabletextbox();            
                Koneksi.conn.Close();
            }
            else if (tbId.Text != "" && tbNama.Text != "" && tbKemasan.Text != "" && tbHarga.Text != "" && tbId.Text.Length==6)
            {
                Koneksi.openConn();

                //insert barang into table barang
                cmd = new MySqlCommand("insert into barang values('" + tbId.Text + "','" + tbNama.Text + "','" + tbKemasan.Text + "','" + tbHarga.Text.Replace(".","") + "','"+cbKategori.Text+"','"+tbDeskripsi.Text+"','Aktif','0')", Koneksi.conn);
                cmd.ExecuteNonQuery();

                //insert into stok table
                cmd = new MySqlCommand("insert into stok values('"+tbId.Text+"', 0,0,0)",Koneksi.conn);
                cmd.ExecuteNonQuery();

                //insert into hpp table
                cmd = new MySqlCommand("insert into hpp values('" + tbId.Text + "',0)",Koneksi.conn);
                cmd.ExecuteNonQuery();

                disabletextbox();
                btnCancel.Enabled = false;
                btnConfirm.Enabled = false;
                btnNew.Enabled = true;
                kosongitextbox();
                newbarang = false;
                this.ActiveControl = btnNew;
                Koneksi.conn.Close();
            }
            else
            {
                MessageBox.Show("Data Belum Lengkap");
            }
            refreshlist();btnNew.Enabled = true;btnDelete.Enabled = true;
        }

        private void tbHarga_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar)) e.Handled = true;
        }
        
        private void tbId_TextChanged(object sender, EventArgs e)
        {
            cekstatus();
            if (FormFindBarang.index != -1 && newbarang == false)
            {
                try
                {
                    Koneksi.openConn();
                    cmd = new MySqlCommand("select * from barang where id_barang='" + tbId.Text + "' ", Koneksi.conn);
                    MySqlDataReader dr;
                    dr = cmd.ExecuteReader();
                    string kategori = "";
                    while (dr.Read())
                    {
                        tbNama.Text = dr.GetString(1);
                        tbKemasan.Text = dr.GetString(2);
                        tbHarga.Text = dr.GetString(3);
                        cbKategori.Text = dr.GetString(4);
                        tbDeskripsi.Text = dr.GetString(5);
                    }
                    cmd = new MySqlCommand("select * from stok where id_barang='" + tbId.Text + "'", Koneksi.conn);
                    MySqlDataReader drr;
                    drr = cmd.ExecuteReader();
                    while (drr.Read())
                    {
                        tbMasuk.Text = drr.GetString(1);
                        tbKeluar.Text = drr.GetString(2);
                        tbStok.Text = drr.GetString(3);
                    }
                    Koneksi.conn.Close();
                    Koneksi.openConn();
                    cmd = new MySqlCommand("select hpp from hpp where id_barang='" + tbId.Text + "'", Koneksi.conn);
                    tbHPP.Text = cmd.ExecuteScalar().ToString();
                    Koneksi.conn.Close();
                }
                catch (Exception x)
                {
                }                
            }
            else
            {
                Koneksi.openConn();
                sql = "select nama_barang from barang where id_barang='" + tbId.Text + "'";
                cmd = new MySqlCommand(sql, Koneksi.conn);
                try
                {
                    string nama = cmd.ExecuteScalar().ToString();
                    lbStatus.Text = "Unavailable";
                    btnConfirm.Enabled = false;

                }
                catch (Exception)
                {
                    lbStatus.Text = "Available";
                    btnConfirm.Enabled = true;
                }
                Koneksi.conn.Close();
            } 
            if(tbId.Text.Length==6)
            {
                this.ActiveControl = tbNama;
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            this.Close();
            FormFindBarang f = new FormFindBarang();
            f.Show();
        }

        private void FormBarang_Activated(object sender, EventArgs e)
        {
            //MessageBox.Show("Test");
        }

        private void FormBarang_Load(object sender, EventArgs e)
        {
            this.ActiveControl = btnNew;
            if (FormFindBarang.index != -1)
            {
                tbId.Text = FormFindBarang.listid[FormFindBarang.index];
                btnPrev.Enabled = true;
                btnNext.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (FormFindBarang.index+1>FormFindBarang.listid.Count()-1)
            {
                FormFindBarang.index = 0;
            }
            else
            {
                FormFindBarang.index++;
            }
            tbId.Text = FormFindBarang.listid[FormFindBarang.index];
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (FormFindBarang.index - 1 < 0)
            {
                FormFindBarang.index = FormFindBarang.listid.Count()-1;
            }
            else
            {
                FormFindBarang.index--;
            }
            tbId.Text = FormFindBarang.listid[FormFindBarang.index];
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            enabletextbox();loadkategori();
            this.ActiveControl = tbNama;
            tbId.Enabled = false;
            btnConfirm.Enabled = true;
            btnCancel.Enabled = true;
            btnNew.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void tbNama_Enter(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            selecttextbox(t);
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            if (tbNama.Text!="")
            {
                namabarang = tbNama.Text;
                SalesInformation s = new SalesInformation();
                s.Show();
            }            
        }

        private void btnPurchase_Click(object sender, EventArgs e)
        {
            if (tbNama.Text!="")
            {
                namabarang = tbNama.Text;
                PurchaseInformation p = new PurchaseInformation();
                p.Show();
            }
            
        }

        private void tbNama_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Koneksi.openConn();
                cmd = new MySqlCommand("select * from stok where nama_barang='" + tbNama.Text + "'", Koneksi.conn);
                dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    tbMasuk.Text = dr.GetString(1);
                    tbKeluar.Text = dr.GetString(2);
                    tbStok.Text = dr.GetString(3);
                }
                Koneksi.conn.Close();
            }
            catch (Exception x)
            {}
        }

        private void FormBarang_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormFindBarang.index = -1;
            FormFindBarang.listid.Clear();
            listkategori.Clear();
        }

        private void tbHarga_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (tbHarga.Text != "")
                {
                    tbHarga.Text = Function.separator(tbHarga.Text);
                    tbHarga.SelectionStart = tbHarga.TextLength;
                    tbHarga.SelectionLength = 0;
                }
            }
            catch (Exception x)
            {}
        }

        private void cbKategori_Enter(object sender, EventArgs e)
        {
            cbKategori.DroppedDown = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (tbId.Text!="")
            {
                Koneksi.openConn();
                cmd = new MySqlCommand("update barang set status='Tidak Aktif' where id_barang='" + tbId.Text + "'", Koneksi.conn);
                cmd.ExecuteNonQuery();
                Koneksi.conn.Close();
                int ind = FormFindBarang.index;
                FormFindBarang.listid.Clear();
                FormFindBarang.loadlistbarang();
                MessageBox.Show("Berhasil Menghapus Data");
                tbId.Text = FormFindBarang.listid[0];
                FormFindBarang.index = ind;
            }            
        }

        private void tbHPP_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (tbHPP.Text != "")
                {
                    tbHPP.Text = Function.separator(tbHPP.Text);
                    tbHPP.SelectionStart = tbHPP.TextLength;
                    tbHPP.SelectionLength = 0;
                }
            }
            catch (Exception x)
            { }
        }

        private void cbKategori_TextChanged(object sender, EventArgs e)
        {
            int index = cbKategori.FindString(cbKategori.Text);
            cbKategori.SelectedIndex = index;
            if (FormFindBarang.index==-1)
            {
                cbKategori.SelectionStart = 0;
                cbKategori.SelectionLength = cbKategori.Text.Length;

            }

        }
    }
}
