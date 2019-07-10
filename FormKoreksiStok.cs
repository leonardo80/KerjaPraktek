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
    public partial class FormKoreksiStok : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;

        string selecteditem = "";
        bool updating = false;
        public static List<String> listnamabarang = new List<string>();

        public FormKoreksiStok()
        {
            InitializeComponent();
        }

        public void activate()
        {
            tbKodeBarang.Enabled = true;cbBarang.Enabled = true;tbJumlah.Enabled = true;

            tbAdd.Enabled = true;btnCancel.Enabled = true;btnSave.Enabled = true;
            tbKeterangan.Enabled = true;
        }

        public void cleardatagrid()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
        }

        public void deactivate()
        {
            tbKodeBarang.Enabled = false; cbBarang.Enabled = false; tbJumlah.Enabled = false;

            tbAdd.Enabled = false; btnCancel.Enabled = false; btnSave.Enabled = false;
            tbKeterangan.Enabled = false;
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
            cbBarang.Items.AddRange(listnamabarang.ToArray<string>());
            cbBarang.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbBarang.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        }

        public void settingdatagrid()
        {
            dataGridView1.DataSource = null;
            dataGridView1.ColumnCount = 2;
            dataGridView1.ColumnHeadersVisible = true;
            dataGridView1.Columns[0].Name = "Nama Barang";
            dataGridView1.Columns[1].Name = "Jumlah";

            DataGridViewColumn col;
            col = dataGridView1.Columns[0]; col.Width = 300;
            col = dataGridView1.Columns[1]; col.Width = 160;
        }

        private void FormKoreksiStok_Load(object sender, EventArgs e)
        {
            this.ActiveControl = btnNew;
            loadbarang();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {

        }

        private void btnNext_Click(object sender, EventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            activate();settingdatagrid();
            DateTime dt = DateTime.Now;
            string datenow = dt.ToString("dd-MM-yy");
            tbTanggal.Text = datenow;
            tbKodeNota.Text = "AS";
            tbKodeNota1.Text = dt.ToString("yy") + dt.ToString("MM");
            Koneksi.openConn();
            cmd = new MySqlCommand("select count(*) from koreksistok where substr(id_nota,1,4)='"+tbKodeNota1.Text+"' ",Koneksi.conn);  
            string count = cmd.ExecuteScalar().ToString();
            int urutan = Convert.ToInt32(count) + 1;
            tbKodeNota2.Text = urutan.ToString().PadLeft(4, '0');
            Koneksi.conn.Close();
            this.ActiveControl = tbKodeBarang;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void btnFind_Click(object sender, EventArgs e)
        {

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            int index = -1;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.ToString() == selecteditem)
                {
                    index = i;
                }
            }

            if (index != -1)
            {
                dataGridView1.Rows[index].Cells[0].Value = cbBarang.Text;
                dataGridView1.Rows[index].Cells[1].Value = tbJumlah.Text;
                btnUpdate.Enabled = false;selecteditem = "";
            }
        }

        private void tbAdd_Click(object sender, EventArgs e)
        {
            if (updating)
            {
                MessageBox.Show("Anda Sedang Melakukan Proses Update");
            }
            else
            {
                if (tbJumlah.Text != "" && cbBarang.Text != "")
                {
                    dataGridView1.Rows.Add(cbBarang.Text, tbJumlah.Text);
                    this.ActiveControl = tbKodeBarang;
                    DataGridViewColumn col;
                    col = dataGridView1.Columns[0]; col.Width = 300;
                    col = dataGridView1.Columns[1];col.Width = 160;
                }
                else
                {
                    MessageBox.Show("Periksa Kembali Data Anda");
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string tglnow = tbTanggal.Text;
            string[] splittgl = tglnow.Split('-');
            string insertedtanggal = splittgl[2] + "-" + splittgl[1] + "-" + splittgl[0];

            //insert into table koreksistok
            Koneksi.openConn();
            cmd = new MySqlCommand("insert into koreksistok values('AS','" + tbKodeNota1.Text + "-" + tbKodeNota2.Text + "','" + insertedtanggal + "','" + tbKeterangan.Text + "')", Koneksi.conn);
            cmd.ExecuteNonQuery();
            Koneksi.conn.Close();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string namabarang = dataGridView1.Rows[i].Cells[0].Value.ToString();
                string jumlah = dataGridView1.Rows[i].Cells[1].Value.ToString();

                //get id barang per item
                Koneksi.openConn();
                cmd = new MySqlCommand("select id_barang from barang where nama_barang='" + namabarang + "'", Koneksi.conn);
                string idbarang = cmd.ExecuteScalar().ToString();
                Koneksi.conn.Close();

                //insert into table dkoreksistok
                Koneksi.openConn();
                cmd = new MySqlCommand("insert into dkoreksistok values('" + tbKodeNota1.Text + "-" + tbKodeNota2.Text + "','"+idbarang+"','"+jumlah+"')", Koneksi.conn);
                cmd.ExecuteNonQuery();
                Koneksi.conn.Close();

                //update table stok
                int stok, masuk, keluar;
                if (Convert.ToInt32(jumlah)<0)
                {
                    //masuk penjualan & update keluar
                    Koneksi.openConn();
                    int qty = Convert.ToInt32(jumlah) * -1;
                    cmd = new MySqlCommand("update stok set keluar=keluar+'"+qty+"' where id_barang='"+idbarang+"'", Koneksi.conn);
                    cmd.ExecuteNonQuery();
                    Koneksi.conn.Close();
                }
                else
                {
                    //masuk pembelian & update masuk
                    Koneksi.openConn();
                    cmd = new MySqlCommand("update stok set masuk=masuk+'" + jumlah + "'  where id_barang='" + idbarang + "'", Koneksi.conn);
                    cmd.ExecuteNonQuery();
                    Koneksi.conn.Close();
                }
                //ganti stok
                Koneksi.openConn();
                cmd = new MySqlCommand("update stok set stok=stok+'"+jumlah+ "' where id_barang='" + idbarang + "'", Koneksi.conn);
                cmd.ExecuteNonQuery();
                Koneksi.conn.Close();

                //deactivate
                deactivate();
                cleardatagrid();
            }

        }

        private void cbBarang_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Koneksi.openConn();
                cmd = new MySqlCommand("select id_barang from barang where nama_barang='" + cbBarang.Text + "'", Koneksi.conn);
                tbKodeBarang.Text = cmd.ExecuteScalar().ToString();
                cmd = new MySqlCommand("select stok from stok where id_barang='" + tbKodeBarang.Text + "'", Koneksi.conn);
                lbStok.Text = cmd.ExecuteScalar().ToString();
                Koneksi.conn.Close();
            }
            catch (Exception x)
            {}
        }

        private void tbKodeBarang_TextChanged(object sender, EventArgs e)
        {
            if (tbKodeBarang.TextLength==6)
            {
                //this.ActiveControl = tbJumlah;
            }
            try
            {
                Koneksi.openConn();
                cmd = new MySqlCommand("select nama_barang from barang where id_barang='" + tbKodeBarang.Text + "'", Koneksi.conn);
                cbBarang.Text = cmd.ExecuteScalar().ToString();
                cmd = new MySqlCommand("select stok from stok where id_barang='" + tbKodeBarang.Text + "'", Koneksi.conn);
                lbStok.Text = cmd.ExecuteScalar().ToString();
                Koneksi.conn.Close();
            }
            catch (Exception x)
            {}
        }

        private void tbKodeBarang_Enter(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            FormBarang.selecttextbox(t);
        }

        private void tbJumlah_Enter(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            FormBarang.selecttextbox(t);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnUpdate.Enabled = true;updating = true;
            cbBarang.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            tbJumlah.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            selecteditem = cbBarang.Text;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = -1;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.ToString() == selecteditem)
                {
                    index = i;
                }
            }
            dataGridView1.Rows.RemoveAt(index);
            dataGridView1.Refresh();
        }
    }
}
