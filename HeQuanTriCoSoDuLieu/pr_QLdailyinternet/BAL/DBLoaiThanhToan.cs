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
    public class DBLoaiThanhToan
    {
        DALayer db = null;
        public DBLoaiThanhToan()
        {
            db = new DALayer();
        }

        public DataSet LayLoaiThanhToan() 
        {
            return db.ExecuteQueryDataSet("SELECT * FROM LOAITHANHTOAN", CommandType.Text, null);
        }
        public bool ThemLoaiThanhToan(ref string err,
            string MaLoaiThanhToan, string TenDichVu, int GiaTien)
        {
            return db.MyExecuteNonQuery("spThemLoaiThanhToan",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaLoaiThanhToan", MaLoaiThanhToan),
                new SqlParameter("@TenDichVu", TenDichVu),
                new SqlParameter("@GiaTien", GiaTien));
        }
        // DELETE
        public bool XoaLoaiThanhToan(ref string err,
            string MaLoaiThanhToan)
        {
            return db.MyExecuteNonQuery("spXoaLoaiThanhToan",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaLoaiThanhToan", MaLoaiThanhToan));
        }
        // UPDATE
        public bool SuaLoaiThanhToan(ref string err,
             string MaLoaiThanhToan, string TenDichVu, int GiaTien)
        {
            return db.MyExecuteNonQuery("spSuaLoaiThanhToan",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaLoaiThanhToan", MaLoaiThanhToan),
                new SqlParameter("@TenDichVu", TenDichVu),
                new SqlParameter("@GiaTien", GiaTien));
        }
    }
}
