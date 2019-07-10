using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace KP
{
    public partial class FormLaporanTransaksi : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;

        public FormLaporanTransaksi()
        {
            InitializeComponent();
        }

        private void btnCount_Click(object sender, EventArgs e)
        {
            string tahun = cbTahun.Text;
            string start = cbStart.Text;
            if (checkBox1.Checked==true)
            {
                //declare data table
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("TANGGAL", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("KODE", typeof(string)));
                dt.Columns.Add(new DataColumn("NOMER", typeof(string)));
                dt.Columns.Add(new DataColumn("JUAL", typeof(Int32)));
                Koneksi.openConn();
                cmd = new MySqlCommand("select tanggal, kode, id_jual, totaljual from jual where substr(tanggal,4,2)='" + start+"' order by kode asc, id_jual asc",Koneksi.conn);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    //MessageBox.Show(dr.GetDateTime(0).ToString("dd-MM-yy"));
                    dt.Rows.Add(dr.GetDateTime(0).ToString("dd-MM-yy"), dr.GetString(1), dr.GetString(2), dr.GetString(3));
                }
                Koneksi.conn.Close();
                
                //pasing data to cr
                LaporanTransaksiHarian lt = new LaporanTransaksiHarian();
                lt.Database.Tables["Nota"].SetDataSource(dt);
                lt.SetParameterValue("start", Function.getMonth(start));
                lt.SetParameterValue("tahun", "20" + tahun);

                //view laporan
                ReportViewer v = new ReportViewer();
                v.crvTransaksi.ReportSource = null;
                v.crvTransaksi.ReportSource = lt;
                v.Show();
            }
            else
            {
                //declare data table
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("NamaCustomer", typeof(string)));
                dt.Columns.Add(new DataColumn("Nomer", typeof(string)));
                dt.Columns.Add(new DataColumn("Total", typeof(Int32)));
                Koneksi.openConn();
                cmd = new MySqlCommand("select c.nama_customer,j.id_jual, j.totaljual from jual j, customer c where c.id_customer=j.id_customer and substr(tanggal,4,2)='" + start + "' ", Koneksi.conn);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dt.Rows.Add(dr.GetString(0), dr.GetString(1), dr.GetString(2));
                }
                Koneksi.conn.Close();

                //pasing data to cr
                LaporanTransaksiCustomer lt = new LaporanTransaksiCustomer
                    ();
                lt.Database.Tables["Nota2"].SetDataSource(dt);
                lt.SetParameterValue("start", Function.getMonth(start));
                lt.SetParameterValue("tahun", "20" + tahun);

                //view laporan
                ReportViewer v = new ReportViewer();
                v.crvTransaksi.ReportSource = null;
                v.crvTransaksi.ReportSource = lt;
                v.Show();
            }          
        }
        

        private void FormLaporanTransaksi_Load(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            cbStart.Text = now.ToString("MM");
            cbTahun.Text = now.ToString("yy");
            checkBox1.Checked = true;
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            checkBox2.Checked = false;

        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            checkBox2.Checked = true;
            checkBox1.Checked = false;
        }
    }
}
