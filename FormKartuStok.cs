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
    public partial class FormKartuStok : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;

        public static List<string> listnamabarang = new List<string>();

        public FormKartuStok()
        {
            InitializeComponent();
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

        private void FormKartuStok_Load(object sender, EventArgs e)
        {
            loadbarang();
            this.ActiveControl = cbBarang;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Koneksi.openConn();
            if (cbBarang.Text!="")
            {
                dataGridView1.DataSource = null;
                dataGridView1.Refresh();
                da = new MySqlDataAdapter("select b.tanggal as Tanggal, b.kode as Kode, b.id_beli as \"Nomer Nota\", s.nama_supplier as \"Customer/Supplier\", format(db.jumlah_barang,0,'de_DE') as Jumlah from beli b,supplier s,dbeli db where b.tanggal>='" + dtStart.Value.ToString("dd-MM-yy") + "' and b.tanggal<='" + dtEnd.Value.ToString("dd-MM-yy") + "' and b.kode=db.kode and b.id_beli=db.id_beli and b.id_supplier=s.id_supplier and db.nama_barang='" + cbBarang.Text + "' " + 
                    "union " +
                    "select j.tanggal as Tanggal, j.kode as Kode, j.id_jual as \"Nomer Nota\", c.nama_customer as \"Customer/Supplier\", concat('-',format(dj.jumlah_barang,0,'de_DE')) as Jumlah from jual j,djual dj,customer c where j.tanggal>='" + dtStart.Value.ToString("dd-MM-yy") + "' and j.tanggal<='" + dtEnd.Value.ToString("dd-MM-yy") + "' and j.kode=dj.kode and j.id_jual=dj.id_jual and j.id_customer=c.id_customer and dj.nama_barang='" + cbBarang.Text + "' " +
                    "union" +
                    " select k.tanggal as Tanggal,  k.kode, k.id_nota, 'Supervisor', dk.jumlah from koreksistok k, dkoreksistok dk, barang b where k.id_nota=dk.id_nota and b.id_barang=dk.id_barang and b.nama_barang='"+cbBarang.Text+"' and k.tanggal>='"+dtStart.Value.ToString("dd-MM-yy") +"' and k.tanggal<='"+dtEnd.Value.ToString("dd-MM-yy") +"' order by Tanggal", Koneksi.conn);
                //da = new MySqlDataAdapter("select j.tanggal as Tanggal,j.kode as Kode, j.id_jual as \"Nomer Nota\", c.nama_customer as \"Customer/Supplier\", concat('-',format(dj.jumlah_barang,0,'de_DE')) as Jumlah from jual j,djual dj,customer c where j.tanggal>='" + dtStart.Value.ToString("dd-MM-yy") + "' and j.tanggal<='" + dtEnd.Value.ToString("dd-MM-yy") + "' and j.kode=dj.kode and j.id_jual=dj.id_jual and j.id_customer=c.id_customer and dj.nama_barang='" + cbBarang.Text + "' union select b.tanggal as Tanggal, b.kode as Kode, b.id_beli as \"Nomer Nota\", s.nama_supplier as \"Customer/Supplier\", format(db.jumlah_barang,0,'de_DE') as Jumlah from beli b,supplier s,dbeli db where b.tanggal>='" + dtStart.Value.ToString("dd-MM-yy") + "' and b.tanggal<='" + dtEnd.Value.ToString("dd-MM-yy") + "' and b.kode=db.kode and b.id_beli=db.id_beli and b.id_supplier=s.id_supplier and db.nama_barang='" + cbBarang.Text + "' union select k.tanggal as Tanggal,  k.kode, k.id_nota, 'Supervisor', dk.jumlah from koreksistok k, dkoreksistok dk, barang b where k.id_nota=dk.id_nota and b.id_barang=dk.id_barang and b.nama_barang='" + cbBarang.Text + "' and k.tanggal>='" + dtStart.Value.ToString("dd-MM-yy") + "' and k.tanggal<='" + dtEnd.Value.ToString("dd-MM-yy") + "' order by Tanggal", Koneksi.conn);
                //da = new MySqlDataAdapter("select j.tanggal as Tanggal,j.kode as Kode, j.id_jual, c.nama_customer, concat('-',format(dj.jumlah_barang,0,'de_DE')) as Jumlah from jual j,djual dj,customer c where j.tanggal between '"+dtStart.Value.ToString("yyyy-MM-dd")+"' and '"+dtEnd.Value.ToString("yyyy-MM-dd")+"' and j.kode=dj.kode and j.id_customer=c.id_customer and dj.nama_barang='Avian Cat Kayu 1 kg';", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                
                DataGridViewColumn col;
                col = dataGridView1.Columns[0]; col.Width = 65;
                col = dataGridView1.Columns[1]; col.Width = 45;
                col = dataGridView1.Columns[2]; col.Width = 80;
                col = dataGridView1.Columns[3]; col.Width = 170;
                col = dataGridView1.Columns[4]; col.Width = 60;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            Koneksi.conn.Close();
        }
    }
}
