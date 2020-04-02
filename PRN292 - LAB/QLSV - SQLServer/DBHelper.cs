using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV___SQLServer
{
    class DBHelper
    {
        public SqlConnection cnn { get; set; }
        public DBHelper(string cnnstr)
        {
            this.cnn = new SqlConnection(cnnstr);
        }

        //Hàm lấy record đổ ra Db
        public DataTable getRecord(string querry)
        {
            DataTable dt = new DataTable();

            SqlDataAdapter da = new SqlDataAdapter(querry, this.cnn);

            this.cnn.Open();
            da.Fill(dt);
            this.cnn.Close();
            return dt;
        }
        //hàm tương tác vs DB
        public void InitDB(string querry)
        {
            SqlCommand cmd = new SqlCommand(querry, this.cnn);

            this.cnn.Open();
            cmd.ExecuteNonQuery();
            this.cnn.Close();


        }
    }
}
