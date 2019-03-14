using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace KP
{
    class Koneksi
    {
        public static string connstring = "datasource=localhost;username=root;database=toko;";
        public static MySqlConnection conn;
        public static void openConn()
        {
            conn = new MySqlConnection(connstring);
            try
            {
                conn.Open();
            }
            catch (Exception)
            {
                conn.Close();
            }            
        }
    }
}
