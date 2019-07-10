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
    public partial class FormFindPembelian : Form
    {

        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public static string sql;

        public static string kode = "", supp = "";
        public static int index = -1;
        public static List<string> listidsupp = new List<string>();
        public static List<string> listnonota = new List<string>();

        public FormFindPembelian()
        {
            InitializeComponent();
        }

        public void loadnota()
        {
            Koneksi.openConn();
            cmd = new MySqlCommand("select * from beli order by id_beli desc", Koneksi.conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                listidsupp.Add(dr.GetString(2));
                listnonota.Add(dr.GetString(1));
            }
            Koneksi.conn.Close();
        }

        private void FormFindPembelian_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            string stm = "select id_beli as \"Nomer Nota\", id_supplier as Supplier, tanggal as Tanggal, format(totalbeli,0,'de_DE') as Total from beli order by id_beli desc";
            Koneksi.openConn();
            da = new MySqlDataAdapter(stm, Koneksi.conn);
            ds = new DataSet();
            da.Fill(ds);
            Koneksi.conn.Close();
            dataGridView1.DataSource = ds.Tables[0];
            DataGridViewColumn col = dataGridView1.Columns[3];
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            col.Width = 95;
            loadnota();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string search = textBox1.Text;
            if (comboBox1.Text == "KODE")
            {
                da = new MySqlDataAdapter("select id_beli as \"Nomer Nota\", id_supplier as Supplier, tanggal as Tanggal, format(totalbeli,0,'de_DE') as Total from beli where id_beli like '%" + search+"%' order by id_beli desc", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

            }
            if (comboBox1.Text == "SUPPLIER")
            {
                da = new MySqlDataAdapter("select id_beli as \"Nomer Nota\", id_supplier as Supplier, tanggal as Tanggal, format(totalbeli,0,'de_DE') as Total from beli where id_supplier like '%" + search + "%' order by id_beli desc", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
            if (comboBox1.Text == "TANGGAL")
            {
                da = new MySqlDataAdapter("select id_beli as \"Nomer Nota\", id_supplier as Supplier, tanggal as Tanggal, format(totalbeli,0,'de_DE') as Total from beli where tanggal like '%" + search + "%' order by id_beli desc", Koneksi.conn);
                ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
            loadnota();
            DataGridViewColumn col = dataGridView1.Columns[3];
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            col.Width = 95;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            supp = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            for (int i = 0; i < listnonota.Count(); i++)
            {
                if (listnonota[i] == id)
                {
                    index = i;
                }
            }
            this.Close();
            FormPembelian f = new FormPembelian();
            f.Show();
        }
    }
}
