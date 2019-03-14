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


        public FormBarang()
        {
            InitializeComponent();
        }

        public static void selecttextbox(TextBox t)
        {
            t.SelectionStart = 0;
            t.SelectionLength = t.Text.Length;
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
        }

        public void disabletextbox()
        {
            tbId.Enabled = false;
            tbNama.Enabled = false;
            tbKemasan.Enabled = false;
            tbHarga.Enabled = false;
        }

        public void kosongitextbox()
        {
            tbId.Text = "";tbNama.Text = "";tbKemasan.Text = "";tbHarga.Text = "";
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
            enabletextbox();
            kosongitextbox();
            btnConfirm.Enabled = true;
            btnCancel.Enabled = true;
            btnNew.Enabled = false;
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
                disabletextbox(); btnCancel.Enabled = false; btnConfirm.Enabled = false;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {            
            if (tbId.Enabled==false)
            {
                Koneksi.openConn();
                cmd = new MySqlCommand("update barang set nama_barang ='"+tbNama.Text+"', satuan_barang='"+tbKemasan.Text+"', harga_barang='"+tbHarga.Text+"' where id_barang='"+tbId.Text+"' ", Koneksi.conn);
                cmd.ExecuteNonQuery();
                btnCancel.Enabled = false;
                btnConfirm.Enabled = false;
                disabletextbox();            
                Koneksi.conn.Close();
            }
            else if (tbId.Text != "" && tbNama.Text != "" && tbKemasan.Text != "" && tbHarga.Text != "")
            {
                Koneksi.openConn();

                //insert barang into table barang
                cmd = new MySqlCommand("insert into barang values('" + tbId.Text + "','" + tbNama.Text + "','" + tbKemasan.Text + "','" + tbHarga.Text + "')", Koneksi.conn);
                cmd.ExecuteNonQuery();

                //insert into stok table
                cmd = new MySqlCommand("insert into stok values('"+tbNama.Text+"', '0', '0', '0'", Koneksi.conn);
                cmd.ExecuteNonQuery();

                disabletextbox();
                btnCancel.Enabled = false; btnConfirm.Enabled = false;
                btnNew.Enabled = true;
                kosongitextbox();
                this.ActiveControl = btnNew;
                Koneksi.conn.Close();
            }
            else
            {
                MessageBox.Show("Data Belum Lengkap");
            }
            refreshlist();
        }

        private void tbHarga_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar)) e.Handled = true;
        }
        
        private void tbId_TextChanged(object sender, EventArgs e)
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
                isidatabarang();
                btnPrev.Enabled = true; btnNext.Enabled = true; btnEdit.Enabled = true;
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
            isidatabarang();
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
            isidatabarang();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            enabletextbox();
            this.ActiveControl = tbNama;
            tbId.Enabled = false;
            btnConfirm.Enabled = true;
            btnCancel.Enabled = true;
        }

        private void tbNama_Enter(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            selecttextbox(t);
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            namabarang = tbNama.Text;
            SalesInformation s = new SalesInformation();
            s.Show();
        }

        private void btnPurchase_Click(object sender, EventArgs e)
        {
            namabarang = tbNama.Text;
            PurchaseInformation p = new PurchaseInformation();
            p.Show();
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
    }
}
