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
    public partial class FormLabaRugi : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;

        public FormLabaRugi()
        {
            InitializeComponent();
        }

        private void btnCount_Click(object sender, EventArgs e)
        {
            string start = cbStart.Text;
            string tahun = cbTahun.Text;

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Kode", typeof(string)));
            dt.Columns.Add(new DataColumn("Nomer", typeof(string)));
            dt.Columns.Add(new DataColumn("Total", typeof(Int32)));
            dt.Columns.Add(new DataColumn("Laba", typeof(Int32)));

            //datanota values (kode,nomer,total)
            List<string> datanota = new List<string>();
            //listbarang values (idbarang,jumlah,total)
            List<string> listbarang = new List<string>();
            //listhpp values (idbarang, hpp)
            List<string> listhpp = new List<string>();
            datanota.Clear();

            //get nomer nota yang sesuai periode
            Koneksi.openConn();
            cmd = new MySqlCommand("select kode,id_jual,totaljual from jual where substr(tanggal,4,2)='"+start+"'",Koneksi.conn);
            dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                datanota.Add(dr.GetString(0)+";"+dr.GetString(1) + ";" + dr.GetString(2));
            }
            Koneksi.conn.Close();

            //get hpp semua barang
            Koneksi.openConn();
            cmd = new MySqlCommand("select * from hpp",Koneksi.conn);
            dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                listhpp.Add(dr.GetString(0) + "-" + dr.GetString(1));
            }
            Koneksi.conn.Close();

            //for setiap nomer nota yang ada
            for (int i = 0; i < datanota.Count; i++)
            {
                string[] temp = datanota[i].Split(';');
                string kode = temp[0];
                string nomernota = temp[1];
                int total = Convert.ToInt32(temp[2]);
                int laba = 0;
                listbarang.Clear();                

                //get semua barang yang ada satu satu
                Koneksi.openConn();
                cmd = new MySqlCommand("select b.id_barang, dj.jumlah_barang, dj.totalharga,dj.hpp from djual dj, barang b where b.nama_barang=dj.nama_barang and dj.kode='"+kode+"' and dj.id_jual='"+nomernota+"'", Koneksi.conn);
                dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    listbarang.Add(dr.GetString(0) + "-" + dr.GetString(1) + "-" + dr.GetString(2) + "-" + dr.GetString(3));
                }
                Koneksi.conn.Close();

                for (int j = 0; j < listbarang.Count; j++)
                {
                    string[] a = listbarang[j].Split('-');
                    string idbarang = a[0];
                    string jumlah = a[1];
                    string totalharga = a[2];
                    string currhpp = a[3];
                    double hpp = Convert.ToDouble(totalharga) / Convert.ToDouble(jumlah);
                    int selisih = (int)Math.Round(hpp) - Convert.ToInt32(currhpp);
                    int cuan = selisih * Convert.ToInt32(jumlah);
                    laba += cuan;
                }
                
                dt.Rows.Add(kode,nomernota,total,laba);
            }
            //dataGridView1.DataSource = dt;

            LaporanLabaRugi lt = new LaporanLabaRugi();
            lt.Database.Tables["Labarugi"].SetDataSource(dt);
            lt.SetParameterValue("start", Function.getMonth(start));
            lt.SetParameterValue("tahun", "20" + tahun);

            //view laporan
            ReportViewer v = new ReportViewer();
            v.crvTransaksi.ReportSource = null;
            v.crvTransaksi.ReportSource = lt;
            v.Show();
        }

        private void FormLabaRugi_Load(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            cbStart.Text = now.ToString("MM");
            cbTahun.Text = now.ToString("yy");
            this.ActiveControl = cbStart;
        }
    }
}
