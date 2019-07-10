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
    public partial class FormPoinPenjualan : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;
        public static int textpoin = 0;
        public static DataTable dt;
        public static DataTable dttemp;
        public static DataTable dtTable;
        public static int totalpoin = 0;
        public static List<string> listnamapromo = new List<string>();
        public static List<string> isipromo = new List<string>(); 
        public static List<string> listnota = new List<string>(); 
        public static List<string> listdatabarangdarinota = new List<string>();
        public static List<string> hasil = new List<string>();


        public FormPoinPenjualan()
        {
            InitializeComponent();
            dtTable = new DataTable();
            dtTable.Columns.Add(new DataColumn("NAMA CUSTOMER", typeof(string)));
            dtTable.Columns.Add(new DataColumn("KODE", typeof(string)));
            dtTable.Columns.Add(new DataColumn("NOMOR NOTA", typeof(string)));
            dtTable.Columns.Add(new DataColumn("POIN", typeof(string)));
            dataGridView1.DataSource = dtTable;

            dttemp = new DataTable();
            dttemp.Columns.Add(new DataColumn("NAMA CUSTOMER", typeof(string)));
            dttemp.Columns.Add(new DataColumn("KODE", typeof(string)));
            dttemp.Columns.Add(new DataColumn("NOMOR NOTA", typeof(string)));
            dttemp.Columns.Add(new DataColumn("POIN", typeof(string)));
        }

        public void hitungtotalpoin()
        {
            textpoin = 0;
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                textpoin += Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value.ToString().Replace(".", ""));
            }
            lbTotal.Text = Function.separator(textpoin.ToString());
        }

        public void settingdatagrid()
        {
            dataGridView1.ColumnCount = 4;
            dataGridView1.ColumnHeadersVisible = true;
            dataGridView1.Columns[0].Name = "Customer";
            dataGridView1.Columns[1].Name = "Kode";
            dataGridView1.Columns[2].Name = "Nomer Nota";
            dataGridView1.Columns[3].Name = "Poin";

            DataGridViewColumn col;
            col = dataGridView1.Columns[0]; col.Width = 155;
            col = dataGridView1.Columns[1]; col.Width = 40;
            col = dataGridView1.Columns[2]; col.Width = 70;
            col = dataGridView1.Columns[3]; col.Width = 80;
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
        
        private void FormPoinPenjualan_Load(object sender, EventArgs e)
        {
            loadnamapromo();
        }

        private void btnCount_Click(object sender, EventArgs e)
        {
            totalpoin = 0;
            try
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
            }
            catch (Exception x)
            {}
            if (cbPromo.Text!="")
            {
                tbSearch.Enabled = true;
                //ambil detail promo
                Koneksi.openConn();
                cmd = new MySqlCommand("select * from promo where nama='"+cbPromo.Text+"'",Koneksi.conn);
                dr = cmd.ExecuteReader();
                string idpromo="", start="", end="", tahun="";
                while(dr.Read())
                {
                    idpromo = dr.GetString(0);
                    start = dr.GetString(2);
                    end = dr.GetString(3);
                    tahun = dr.GetString(4).Substring(2,2);
                }
                Koneksi.conn.Close();

                //ambil data promo berapa barang dpt brp poin
                Koneksi.openConn();
                cmd = new MySqlCommand("select * from dpromo where id_promo='" +idpromo + "'", Koneksi.conn);
                dr = cmd.ExecuteReader();
                string idbarang = "", jumlah = "", poin = "";
                isipromo.Clear();
                while (dr.Read())
                {
                    idbarang = dr.GetString(1);
                    jumlah = dr.GetString(2);
                    poin = dr.GetString(3);
                    isipromo.Add(idbarang + "-" + jumlah + "-" + poin);
                }
                Koneksi.conn.Close();
                tahun = "19";
                //select semua nota yang masuk periode promo
                Koneksi.openConn();
                cmd = new MySqlCommand("select c.nama_customer as Customer, j.kode as Kode,j.id_jual as ID from jual j, customer c where c.id_customer=j.id_customer and tanggal>='01-"+start+"-"+tahun+"' and tanggal<='31-"+end+"-"+tahun+"'", Koneksi.conn);
                dr = cmd.ExecuteReader();
                listnota.Clear();
                while(dr.Read())
                {
                    listnota.Add(dr.GetString(0)+";"+dr.GetString(1)+";"+dr.GetString(2));
                }
                Koneksi.conn.Close();

                //isipromo id barang , jumlah, poin
                //list nota nama customer, kode nota, nomer nota
                hasil.Clear(); //reset data untuk datagridview

                //proses counting                
                for (int i = 0; i < listnota.Count; i++)
                {
                    string[] temp= listnota[i].Split(';');
                    string namacustomer = temp[0]; // KOTA JAYA
                    string kodenota = temp[1]; // PJ / LP
                    string nomernota = temp[2]; // 1904-0001
                    totalpoin = 0; //reset total poin

                    //ambil data barang berdasarkan nomer nota
                    Koneksi.openConn();
                    cmd = new MySqlCommand("select b.id_barang as Id,dj.jumlah_barang as Jumlah from djual dj, barang b where b.nama_barang=dj.nama_barang and kode='"+kodenota+"' and id_jual='"+nomernota+"'", Koneksi.conn);
                    dr = cmd.ExecuteReader();
                    listdatabarangdarinota.Clear();
                    while(dr.Read())
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
                            if (idbarangnota==idbarangpromo)
                            {
                                totalpoin += (jumlahbarangnota / jumlahbarangpromo) * poinbarangpromo;
                            }
                        }
                    }
                    //MessageBox.Show(totalpoin.ToString());
                    string textfortotalpoin = Function.separator(totalpoin.ToString());
                    //simpan hasil untuk nanti tampil di datagridview
                    hasil.Add(namacustomer+";"+kodenota+";"+nomernota+";"+textfortotalpoin);
                }
                
                for (int i = 0; i < hasil.Count; i++)
                {
                    string[] temp3 = hasil[i].Split(';');
                    //dataGridView1.Rows.Add(temp3[0],temp3[1],temp3[2],temp3[3]);
                    DataRow newRow = dtTable.NewRow();
                    newRow["NAMA CUSTOMER"] = temp3[0];
                    newRow["KODE"] = temp3[1];
                    newRow["NOMOR NOTA"] = temp3[2];
                    newRow["POIN"] = temp3[3];
                    dtTable.Rows.Add(newRow);
                }
                dataGridView1.Refresh();
                hitungtotalpoin();
                DataGridViewColumn col;
                col = dataGridView1.Columns[0]; col.Width = 120;
                col = dataGridView1.Columns[1]; col.Width = 40;
                col = dataGridView1.Columns[3]; col.Width = 70;
                col = dataGridView1.Columns[3];
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                
            }
        }

        private void tbCustomer_TextChanged(object sender, EventArgs e)
        {
            dttemp.Rows.Clear();
            //DataView dv = new DataView(dt);
            //dv.RowFilter = "Customer = " + tbSearch.Text ;
            for (int i = 0; i < dtTable.Rows.Count; i++)
            {
                if (dtTable.Rows[i][0].ToString().ToLower().Contains(tbSearch.Text.ToLower()))
                {
                    DataRow newRow = dttemp.NewRow();
                    newRow[0] = dtTable.Rows[i][0];
                    newRow[1] = dtTable.Rows[i][1];
                    newRow[2] = dtTable.Rows[i][2];
                    newRow[3] = dtTable.Rows[i][3];
                    dttemp.Rows.Add(newRow);
                }
            }
            dataGridView1.DataSource = dttemp;
            DataGridViewColumn col;
            col = dataGridView1.Columns[0]; col.Width = 120;
            col = dataGridView1.Columns[1]; col.Width = 40;
            col = dataGridView1.Columns[3]; col.Width = 70;
            col = dataGridView1.Columns[3];
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            hitungtotalpoin();
        }
    }
}
