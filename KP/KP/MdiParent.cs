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
    public partial class MdiParent : Form
    {
        public static MySqlCommand cmd;
        public static MySqlDataReader dr;
        public static MySqlDataAdapter da;
        public static DataSet ds;
        public MdiParent()
        {
            InitializeComponent();
        }
        

        //private void insertdata()
        //{
        //    connDb();
        //    cmd = new MySqlCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandText = "INSERT INTO barang(barang_id) VALUES(@ID)";
        //    cmd.Prepare();

        //    cmd.Parameters.AddWithValue("@ID", "C0001");
        //    cmd.ExecuteNonQuery();
        //    closeDb();
        //}

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        private void produkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormBarang f = new FormBarang();
            f.Show();
        }

        private void penjualanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPenjualan f = new FormPenjualan();
            f.Show();
        }

        private void customerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCustomer f = new FormCustomer();
            f.Show();
        }

        private void supplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSupplier f = new FormSupplier();
            f.Show();
        }

        private void pembelianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPembelian p = new FormPembelian();
            p.Show();
        }
    }
}
