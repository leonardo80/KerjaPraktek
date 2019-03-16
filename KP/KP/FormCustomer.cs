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
    public partial class FormCustomer : Form
    {

        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;

        public FormCustomer()
        {
            InitializeComponent();
        }

        public void kosongitextbox()
        {
            tbId.Text = "";
            tbNama.Text = "";
            tbAlamat.Text = "";
            tbKota.Text = "";
            tbStatus.Text = "";
        }

        public void enabletextbox()
        {
            tbId.Enabled = true;
            tbNama.Enabled = true;
            tbAlamat.Enabled = true;
            tbKota.Enabled = true;
            tbStatus.Enabled = true;
        }

        public void disabletextbox()
        {
            tbId.Enabled = false;
            tbNama.Enabled = false;
            tbAlamat.Enabled = false;
            tbKota.Enabled = false;
            tbStatus.Enabled = false;
        }

        public void refreshlist()
        {
            FormFindCustomer.eraselistcustomer();
            FormFindCustomer.loadlistcustomer();
        }

        private void isidatacustomer()
        {
            tbId.Text = FormFindCustomer.listid[FormFindCustomer.index];
            tbNama.Text = FormFindCustomer.listnama[FormFindCustomer.index];
            tbAlamat.Text = FormFindCustomer.listalamat[FormFindCustomer.index];
            tbKota.Text = FormFindCustomer.listkota[FormFindCustomer.index];
            tbStatus.Text = FormFindCustomer.liststatus[FormFindCustomer.index];
        }

        private void FormCustomer_Load(object sender, EventArgs e)
        {
            Koneksi.openConn();
            this.ActiveControl = btnNew;
            if (FormFindCustomer.index != -1)
            {
                isidatacustomer();
                btnPrev.Enabled = true; btnNext.Enabled = true; btnEdit.Enabled = true;
            }
            Koneksi.conn.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            kosongitextbox();disabletextbox();
            btnNew.Enabled = true;btnConfirm.Enabled = false;btnCancel.Enabled = false;btnEdit.Enabled = false;
            this.ActiveControl = btnNew;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (tbId.Enabled == false)
            {
                if (tbStatus.Text == "PJ" || tbStatus.Text == "LP")
                {
                    Koneksi.openConn();
                    cmd = new MySqlCommand("update customer set nama_customer = '" + tbNama.Text + "', alamat_customer = '" + tbAlamat.Text + "', kota_customer = '" + tbKota.Text + "', status = '" + tbStatus.Text + "' where id_customer='" + tbId.Text + "' ", Koneksi.conn);
                    cmd.ExecuteNonQuery();
                    btnCancel.Enabled = false;
                    btnConfirm.Enabled = false;
                    disabletextbox();
                    Koneksi.conn.Close();
                }
                else
                {
                    MessageBox.Show("Status Harus Berisi PJ / LP");
                }                    
            }
            else if (tbId.Text != "" && tbNama.Text != "" && tbAlamat.Text != "" && tbKota.Text != "" && tbStatus.Text!= "" && tbId.TextLength == 4)
            {
                if (tbStatus.Text=="PJ" || tbStatus.Text=="LP")
                {
                    Koneksi.openConn();
                    cmd = new MySqlCommand("insert into customer values('" + tbId.Text + "','" + tbNama.Text + "','" + tbAlamat.Text + "','" + tbKota.Text + "','" + tbStatus.Text + "')", Koneksi.conn);
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
                    MessageBox.Show("Status Customer Hanya PJ / LP");
                }
            }
            else
            {
                MessageBox.Show("Data Belum Lengkap");
            }
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

        private void tbId_TextChanged(object sender, EventArgs e)
        {
            Koneksi.openConn();
            sql = "select nama_customer from customer where id_customer='" + tbId.Text + "'";
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            this.Close();
            FormFindCustomer f = new FormFindCustomer();
            f.Show();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            enabletextbox();tbId.Enabled = false;
            this.ActiveControl = tbNama;
            btnCancel.Enabled = true;btnConfirm.Enabled = true;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (FormFindCustomer.index + 1 > FormFindCustomer.listid.Count() - 1)
            {
                FormFindCustomer.index = 0;
            }
            else
            {
                FormFindCustomer.index++;
            }
            isidatacustomer();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (FormFindCustomer.index - 1 < 0)
            {
                FormFindCustomer.index = FormFindCustomer.listid.Count() - 1;
            }
            else
            {
                FormFindCustomer.index--;
            }
            isidatacustomer();
        }
        
    }
}
