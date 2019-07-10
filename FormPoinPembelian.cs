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
    public partial class FormPoinPembelian : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;
        public static string supplier = "0000";

        public static DataTable dt;

        public static int totalpoin = 0;
        public static int textpoin = 0;
        public static List<string> listnamapromo = new List<string>();
        public static List<string> isipromo = new List<string>();
        public static List<string> listnota = new List<string>();
        public static List<string> listdatabarangdarinota = new List<string>();
        public static List<string> hasil = new List<string>();

        public FormPoinPembelian()
        {
            InitializeComponent();
        }

        public void hitungtotalpoin()
        {
            textpoin = 0;
            for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
            {
                textpoin += Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value.ToString().Replace(".", ""));
            }
            lbTotal.Text = Function.separator(textpoin.ToString());
        }

        public void settingdatagrid()
        {
            dataGridView1.ColumnCount = 2;
            dataGridView1.ColumnHeadersVisible = true;
            dataGridView1.Columns[0].Name = "Nomer Nota";
            dataGridView1.Columns[1].Name = "Poin";

            DataGridViewColumn col;
            col = dataGridView1.Columns[0]; col.Width = 80;
            col = dataGridView1.Columns[1]; col.Width = 80;
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        public void loadnamapromo()
        {
            Koneksi.openConn();
            listnamapromo.Clear();
            cmd = new MySqlCommand("select nama from promo where status='0'", Koneksi.conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                listnamapromo.Add(dr.GetString(0));
            }
            Koneksi.conn.Close();
            cbPromo.Text = "";
            cbPromo.Items.Clear();
            cbPromo.Items.AddRange(listnamapromo.ToArray<string>());
            cbPromo.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbPromo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        }

        private void FormPoinPembelian_Load(object sender, EventArgs e)
        {
            loadnamapromo();
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = false ;checkBox2.Checked = true;
            supplier = "0001";
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            checkBox2.Checked = false; checkBox1.Checked = true;
            supplier = "0000";
        }

        private void btnCount_Click(object sender, EventArgs e)
        {
            totalpoin = 0;
            if (cbPromo.Text!="")
            {
                //ambil detail promo
                Koneksi.openConn();
                cmd = new MySqlCommand("select * from promo where nama='" + cbPromo.Text + "'", Koneksi.conn);
                dr = cmd.ExecuteReader();
                string idpromo = "", start = "", end = "", tahun = "";
                while (dr.Read())
                {
                    idpromo = dr.GetString(0);
                    start = dr.GetString(2);
                    end = dr.GetString(3);
                    tahun = dr.GetString(4).Substring(2, 2);
                }
                Koneksi.conn.Close();

                //ambil data promo berapa barang dpt brp poin
                Koneksi.openConn();
                cmd = new MySqlCommand("select * from dpromo where id_promo='" + idpromo + "'", Koneksi.conn);
                dr = cmd.ExecuteReader();
                string idbarang = "", jumlah = "", poin = "";
                isipromo.Clear();
                while (dr.Read())
                {
                    idbarang = dr.GetString(1);
                    jumlah = dr.GetString(2);
                    poin = dr.GetString(3);
                    isipromo.Add(idbarang + "-" + jumlah + "-" + poin);
                    //isi promo idbarang, jumlah, poin
                }
                Koneksi.conn.Close();

                //select semua nota yang masuk periode promo
                Koneksi.openConn();
                cmd = new MySqlCommand("select id_beli from beli where id_supplier='"+supplier+ "' and tanggal>='01-" + start + "-" + tahun + "' and tanggal<='31-" + end + "-" + tahun + "'", Koneksi.conn);
                dr = cmd.ExecuteReader();
                listnota.Clear();
                while (dr.Read())
                {
                    listnota.Add(dr.GetString(0));
                }
                Koneksi.conn.Close();

                //proses counting
                hasil.Clear();

                for (int i = 0; i < listnota.Count; i++)
                {
                    totalpoin = 0;

                    //ambil data barang berdasarkan nomer nota
                    Koneksi.openConn();
                    cmd = new MySqlCommand("select b.id_barang as id, db.jumlah_barang as jumlah from barang b, dbeli db where b.nama_barang=db.nama_barang and db.id_beli='"+listnota[i]+"'", Koneksi.conn);
                    dr = cmd.ExecuteReader();
                    listdatabarangdarinota.Clear();
                    while (dr.Read())
                    {
                        listdatabarangdarinota.Add(dr.GetString(0) + "-" + dr.GetString(1));
                    }
                    Koneksi.conn.Close();

                    //hitung poin per nota
                    for (int j = 0; j < listdatabarangdarinota.Count; j++)
                    {
                        string[] temp1 = listdatabarangdarinota[j].Split('-');
                        string idbarangnota = temp1[0];
                        int jumlahbarangnota = Convert.ToInt32(temp1[1]);
                        for (int k = 0; k < isipromo.Count; k++)
                        {
                            string[] temp2 = isipromo[k].Split('-');
                            string idbarangpromo = temp2[0];
                            int jumlahbarangpromo = Convert.ToInt32(temp2[1]);
                            int poinbarangpromo = Convert.ToInt32(temp2[2]);
                            if (idbarangnota == idbarangpromo)
                            {
                                totalpoin += (jumlahbarangnota / jumlahbarangpromo) * poinbarangpromo;
                            }
                        }
                    }
                    string textfortotalpoin = Function.separator(totalpoin.ToString());
                    //simpan hasil untuk nanti tampil di datagridview
                    hasil.Add(listnota[i]+";" + textfortotalpoin);
                }
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                settingdatagrid();

                dataGridView1.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill;
                for (int i = 0; i < hasil.Count; i++)
                {
                    string[] temp3 = hasil[i].Split(';');
                    dataGridView1.Rows.Add(temp3[0], temp3[1]);
                }
                hitungtotalpoin();
            }
        }
    }
}
