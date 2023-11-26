using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL
{
    public class DALayer
    {
        SqlConnection conn = null;
        SqlCommand comm = null;
        SqlDataAdapter da = null;
        public DALayer()
        {
            string serverName = "(local)";
            string databaseName = "PROJECT_INTERNET";
            string userName = Logins.ID;
            string password = Logins.Pass;
            string ConnStr = $"Data Source={serverName};Initial Catalog={databaseName};User ID={userName};Password={password};";

            conn = new SqlConnection(ConnStr);
            comm = conn.CreateCommand();
        }


        //thực thi một truy vấn SQL trên một kết nối cơ sở dữ liệu đã được thiết lập
        //và trả về dữ liệu được lưu trữ trong một đối tượng DataSet.
        public DataSet ExecuteQueryDataSet(string strSQL, CommandType ct, params SqlParameter[] p)
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
            conn.Open();
            comm.CommandText = strSQL;
            comm.CommandType = ct;
            if (p != null)
            {
                foreach (SqlParameter pa in p)
                {
                    if (p != null)
                    {
                        comm.Parameters.Add(pa);
                    }
                }
            }
            DataSet ds = null;
            da = new SqlDataAdapter(comm);
            try
            {
                if (da != null)
                {
                    ds = new DataSet();
                    da.Fill(ds);
                }
            }
            catch (SqlException ex)
            {
                throw new DataException(ex.Message);
            }
            finally
            {
                comm.Parameters.Clear();
            }
            return ds;
        }
        //Hàm MyExecuteNonQuery có chức năng thực thi một câu lệnh SQL không trả về kết quả (non-query) trên một kết nối đến cơ sở dữ liệu
        public bool MyExecuteNonQuery(string strSQL, CommandType ct, ref string error, params SqlParameter[] param)
        {
            bool f = false;
            if (conn.State == ConnectionState.Open)
                conn.Close();
            conn.Open();
            comm.Parameters.Clear();
            comm.CommandText = strSQL;
            comm.CommandType = ct;
            if (param != null)
            {
                foreach (SqlParameter p in param)
                {
                    if (p != null)
                    {
                        comm.Parameters.Add(p);
                    }
                }
            }
            try
            {
                comm.ExecuteNonQuery();
                f = true;
            }
            catch (SqlException ex)
            {
                error = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return f;
        }
        //thực thi một truy vấn SQL trên cơ sở dữ liệu và trả về một đối tượng SqlDataReader để đọc các bản ghi trả về từ truy vấn
        public SqlDataReader MyExecuteReader(string strSQL, CommandType ct, params SqlParameter[] param)
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
            conn.Open();
            comm.Parameters.Clear();
            comm.CommandText = strSQL;
            comm.CommandType = ct;
            SqlDataReader dr;
            if (param != null)
                foreach (SqlParameter p in param)
                    comm.Parameters.Add(p);
            try
            {
                dr = comm.ExecuteReader();
            }
            catch (SqlException ex)
            {
                throw new DataException(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dr;
        }
    }
}