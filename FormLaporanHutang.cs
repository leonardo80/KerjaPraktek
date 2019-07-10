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
    public partial class FormLaporanHutang : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;

        public static int total = 0;

        public static List<string> listsupplier = new List<string>();

        public FormLaporanHutang()
        {
            InitializeComponent();
        }
        
        private void FormLaporanHutang_Load(object sender, EventArgs e)
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

            //declare datatable
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Nama", typeof(string)));
            dt.Columns.Add(new DataColumn("Nomer", typeof(string)));
            dt.Columns.Add(new DataColumn("Total", typeof(Int32)));
            dt.Columns.Add(new DataColumn("Tanggal", typeof(DateTime)));

            Koneksi.openConn();
            cmd = new MySqlCommand("select s.nama_supplier as Nama, h.id_beli as ID, h.total, b.tanggaljatuhtempo as Tanggal from hutang h, supplier s, beli b where h.id_supplier=s.id_supplier and b.id_beli=h.id_beli and tanggal>='01-" + start + "-" + tahun + "' and tanggal<='31-" + start + "-" + tahun + "' order by Nama asc, ID asc", Koneksi.conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                dt.Rows.Add(dr.GetString(0), dr.GetString(1), dr.GetString(2), dr.GetString(3));
            }
            Koneksi.conn.Close();

            LaporanHutang lh = new LaporanHutang();
            lh.Database.Tables["Piutang"].SetDataSource(dt);
            lh.SetParameterValue("start", Function.getMonth(start));
            lh.SetParameterValue("tahun", "20" + tahun);

            ReportViewer v = new ReportViewer();
            v.crvTransaksi.ReportSource = null;
            v.crvTransaksi.ReportSource = lh;
            v.Show();
        }
        
    }
}
