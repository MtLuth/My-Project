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
    public class DBNgayThangNam
    {
        DALayer db = null;
        public DBNgayThangNam()
        {
            db = new DALayer();
        }

        public DataSet LayLichLamViec()
        {
            return db.ExecuteQueryDataSet("SELECT * FROM LICHLAMVIEC", CommandType.Text, null);
        }
        public bool XoaLichLamViec(ref string err, string NgayThangNam)
        {
            return db.MyExecuteNonQuery("spXoaNgayThangNam",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@NgayThangNam", NgayThangNam));
        }
        public bool ThemLichLamViec(ref string err, string NgayThangNam)
        {
            return db.MyExecuteNonQuery("spThemNgayThangNam",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@NgayThangNam", NgayThangNam));
        }

        public bool SuaLichLamViec(ref string err, string NgayThangNam)
        {
            return db.MyExecuteNonQuery("spSuaNgayThangNam",
                CommandType.StoredProcedure, ref err,
                 new SqlParameter("@NgayThangNam", NgayThangNam));
        }

    }
}
