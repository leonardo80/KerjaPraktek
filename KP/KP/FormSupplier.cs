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
    public partial class FormSupplier : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;

        public FormSupplier()
        {
            InitializeComponent();
        }

        public void kosongitextbox()
        {
            tbId.Text = "";
            tbNama.Text = "";
            tbAlamat.Text = "";
            tbKota.Text = "";
        }

        public void enabletextbox()
        {
            tbId.Enabled = true;
            tbNama.Enabled = true;
            tbAlamat.Enabled = true;
            tbKota.Enabled = true;
        }

        public void disabletextbox()
        {
            tbId.Enabled = false;
            tbNama.Enabled = false;
            tbAlamat.Enabled = false;
            tbKota.Enabled = false;
        }

        public void refreshlist()
        {
            FormFindSupplier.eraselistsupplier();
            FormFindSupplier.loadlistsupplier();
        }

        private void isidatasupplier()
        {
            tbId.Text = FormFindSupplier.listid[FormFindSupplier.index];
            tbNama.Text = FormFindSupplier.listnama[FormFindSupplier.index];
            tbAlamat.Text = FormFindSupplier.listalamat[FormFindSupplier.index];
            tbKota.Text = FormFindSupplier.listkota[FormFindSupplier.index];
        }

        private void FormSupplier_Load(object sender, EventArgs e)
        {
            Koneksi.openConn();
            this.ActiveControl = btnNew;
            if (FormFindSupplier.index != -1)
            {
                isidatasupplier();
                btnPrev.Enabled = true; btnNext.Enabled = true; btnEdit.Enabled = true;
            }
            Koneksi.conn.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            kosongitextbox(); disabletextbox();
            btnNew.Enabled = true; btnConfirm.Enabled = false; btnCancel.Enabled = false; btnEdit.Enabled = false;
            this.ActiveControl = btnNew;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (tbId.Enabled == false)
            {
                Koneksi.openConn();
                cmd = new MySqlCommand("update supplier set nama_supplier = '" + tbNama.Text + "', alamat_supplier = '" + tbAlamat.Text + "', kota_supplier = '" + tbKota.Text + "' where id_supplier='" + tbId.Text + "' ", Koneksi.conn);
                cmd.ExecuteNonQuery();
                btnCancel.Enabled = false;
                btnConfirm.Enabled = false;
                disabletextbox();
                Koneksi.conn.Close();                
            }
            else if (tbId.Text != "" && tbNama.Text != "" && tbAlamat.Text != "" && tbKota.Text != "" && tbId.TextLength == 4)
            {
                Koneksi.openConn();
                cmd = new MySqlCommand("insert into supplier values('" + tbId.Text + "','" + tbNama.Text + "','" + tbAlamat.Text + "','" + tbKota.Text + "')", Koneksi.conn);
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
            btnConfirm.Enabled = false;
            refreshlist();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Koneksi.openConn();
            enabletextbox();
            kosongitextbox();
            btnCancel.Enabled = true;
            btnConfirm.Enabled = true;
            this.ActiveControl = tbId;
            Koneksi.conn.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            enabletextbox(); tbId.Enabled = false;
            this.ActiveControl = tbNama;
            btnCancel.Enabled = true; btnConfirm.Enabled = true;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            this.Close();
            FormFindSupplier f = new FormFindSupplier();
            f.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (FormFindSupplier.index - 1 < 0)
            {
                FormFindSupplier.index = FormFindSupplier.listid.Count() - 1;
            }
            else
            {
                FormFindSupplier.index--;
            }
            isidatasupplier();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (FormFindSupplier.index + 1 > FormFindSupplier.listid.Count() - 1)
            {
                FormFindSupplier.index = 0;
            }
            else
            {
                FormFindSupplier.index++;
            }
            isidatasupplier();
        }

        private void tbId_TextChanged(object sender, EventArgs e)
        {
            Koneksi.openConn();
            sql = "select nama_supplier from supplier where id_supplier='" + tbId.Text + "'";
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

        private void tbId_Enter(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            FormBarang.selecttextbox(t);
        }
        
    }
}
