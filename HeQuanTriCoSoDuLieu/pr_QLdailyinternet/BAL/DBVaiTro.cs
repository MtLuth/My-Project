using DBL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class DBVaiTro
    {
        DALayer db = null;
        public DBVaiTro()
        {
            db = new DALayer();
        }
        public DataSet LayVaiTro()
        {
            return db.ExecuteQueryDataSet("SELECT * FROM VAITRO", CommandType.Text, null);
        }
        // Các phương thức CRUD
        public bool ThemVaiTro(ref string err, string MaVaiTro, string TenVaiTro)
        {
            return db.MyExecuteNonQuery("spThemVaiTro", CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaVaiTro", (object)MaVaiTro ?? DBNull.Value),
                new SqlParameter("TenVaiTro", (object)TenVaiTro ?? DBNull.Value));
        }
        public bool SuaVaiTro(ref string err, string MaVaiTro, string TenVaiTro)
        {
            return db.MyExecuteNonQuery("spSuaVaiTro", CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaVaiTro", MaVaiTro),
                new SqlParameter("TenVaiTro", TenVaiTro));
        }
        public bool XoaVaiTro(ref string err, string MaVaiTro)
        {
            return db.MyExecuteNonQuery("spXoaVaiTro", CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaVaiTro", MaVaiTro));
        }
    }
}
