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
    public partial class FormLaporanPiutang : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;
        public static int total = 0;

        public static List<string> listcustomer = new List<string>();

        public FormLaporanPiutang()
        {
            InitializeComponent();
        }

        private void FormLaporanPiutang_Load(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            cbStart.Text = now.ToString("MM");
            cbTahun.Text = now.ToString("yy");
            this.ActiveControl = cbStart;
        }

        private void btnCount_Click(object sender, EventArgs e)
        {
            string tahun = cbTahun.Text;
            string start = cbStart.Text;

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Nama", typeof(string)));
            dt.Columns.Add(new DataColumn("Kode", typeof(string)));
            dt.Columns.Add(new DataColumn("Nomer", typeof(string)));
            dt.Columns.Add(new DataColumn("Total", typeof(Int32)));
            dt.Columns.Add(new DataColumn("Tanggal", typeof(DateTime)));

            //all
            Koneksi.openConn();
            cmd = new MySqlCommand("select c.nama_customer as Nama, p.kode, p.id_jual, (p.total-p.dibayarkan), j.tanggaljatuhtempo as Tanggal from customer c, piutang p, jual j where p.id_customer=c.id_customer and j.id_jual=p.id_jual and j.kode=p.kode and tanggal>='01-" + start + "-" + tahun + "' and tanggal<='31-" + start + "-" + tahun + "' order by Nama asc, Tanggal asc ", Koneksi.conn);

            //cmd = new MySqlCommand("select c.nama_customer as Nama, format(sum(p.total-p.dibayarkan),0,'de_DE') as Total from customer c, piutang p, jual j where c.id_customer=p.id_customer and p.kode=j.kode and p.id_jual=j.id_jual and (substr(j.tanggal,4,2)='"+start+"' or substr(j.tanggal,4,2)='"+end+"') and substr(j.tanggal,7,2)='"+tahun+"' group by Nama order by Nama", Koneksi.conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                dt.Rows.Add(dr.GetString(0), dr.GetString(1), dr.GetString(2), dr.GetString(3), dr.GetString(4));
            }
            Koneksi.conn.Close();
            
            LaporanPiutang lh = new LaporanPiutang();
            lh.Database.Tables["Hutang"].SetDataSource(dt);
            lh.SetParameterValue("start", Function.getMonth(start));
            lh.SetParameterValue("tahun", "20" + tahun);

            ReportViewer v = new ReportViewer();
            v.crvTransaksi.ReportSource = null;
            v.crvTransaksi.ReportSource = lh;
            v.Show();
        }
        
    }
}
