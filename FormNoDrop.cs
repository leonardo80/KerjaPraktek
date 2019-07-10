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
    public partial class FormNoDrop : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;

        public FormNoDrop()
        {
            InitializeComponent();
        }

        public void cleardatagrid()
        {
            //dataGridView1.Rows.Clear();
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
        }

        private void FormNoDrop_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //string start = dtStart.Value.ToString("dd-MM-yy");
            //string end = dtEnd.Value.ToString("dd-MM-yy");
            //if (checkBox1.Checked==true)
            //{
            //    cleardatagrid();
            //    btnSearch.Enabled = false;tbSearch.Enabled = false;
            //    Koneksi.openConn();
            //    da = new MySqlDataAdapter("select db.id_beli as \"Nomer Nota\", format(sum(db.totalharga),0,'de_DE') as Total from dbeli db, barang b,beli be where db.nama_barang=b.nama_barang and b.nama_kategori='Lem' and db.id_beli=be.id_beli and be.tanggal>='"+start+"' and be.tanggal<='"+end+"' group by db.id_beli order by db.id_beli asc", Koneksi.conn);
            //    ds = new DataSet();
            //    da.Fill(ds);
            //    dataGridView1.DataSource = ds.Tables[0];
            //    Koneksi.conn.Close();
            //}
            //else
            //{
            //    btnSearch.Enabled = true; tbSearch.Enabled = true;
            //}
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string start = dtStart.Value.ToString("dd-MM-yy");
            string end = dtEnd.Value.ToString("dd-MM-yy");
            cleardatagrid();
            if (tbSearch.Text!="")
            {
                if (checkBox1.Checked==true)
                {
                    try
                    {
                        Koneksi.openConn();
                        da = new MySqlDataAdapter("select db.id_beli as \"Nomer Nota\",be.tanggal as Tanggal, format(sum(db.totalharga),0,'de_DE') as Total from dbeli db, barang b,beli be, supplier s where s.id_supplier=be.id_supplier and s.nama_supplier like '%" + tbSearch.Text + "%' and db.nama_barang=b.nama_barang and b.nama_kategori='"+comboBox1.Text+"' and db.id_beli=be.id_beli and be.tanggal>='" + start + "' and be.tanggal<='" + end + "' group by db.id_beli order by db.id_beli asc", Koneksi.conn);
                        ds = new DataSet();
                        da.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0];
                        Koneksi.conn.Close();
                    }
                    catch (Exception x)
                    {}
                }
                else
                {
                    try
                    {
                        Koneksi.openConn();
                        da = new MySqlDataAdapter("select dj.id_jual as \"Nomer Nota\", j.tanggal as Tanggal, format(sum(dj.totalharga),0,'de_DE') as Total from djual dj, jual j, customer c, barang b where c.nama_customer like '%"+tbSearch.Text+ "%' and dj.id_jual=j.id_jual and j.id_customer=c.id_customer and dj.nama_barang=b.nama_barang and b.nama_kategori='" + comboBox1.Text + "' and j.tanggal>='" + start+"' and j.tanggal<='"+end+"' group by dj.id_jual order by dj.id_jual asc", Koneksi.conn);
                        ds = new DataSet();
                        da.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0];
                        Koneksi.conn.Close();
                    }
                    catch (Exception x)
                    {}
                }
            }
        }
    }
}
