using DBL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class DBDiemDanh
    {
        DALayer db = null;
        public DBDiemDanh()
        {
            db = new DALayer();
        }
        public DataSet LayDiemDanh()
        {
            return db.ExecuteQueryDataSet("SELECT * FROM DIEMDANH", CommandType.Text, null);
        }
        public DataSet LayDiemDanhNgayThangNam(string NgayThangNam)
        {
            return db.ExecuteQueryDataSet("SELECT * FROM LayDiemDanhNgayThangNam('" + NgayThangNam + "')", CommandType.Text, null);
        }

        public bool XoaDiemDanh(ref string err, string MaNV, string GioLam, string GioNghi,
            string NgayThangNam, string GhiChu)
        {
            return db.MyExecuteNonQuery("spXoaDiemDanh",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaNV", MaNV),
                new SqlParameter("@GioLam", GioLam),
                new SqlParameter("@GioNghi", GioNghi),
                new SqlParameter("@NgayThangNam", NgayThangNam),
                new SqlParameter("@GhiChu", GhiChu));
        }
        public bool ThemDiemDanh(ref string err, string MaNV, string GioNghi,
            string NgayThangNam, string GhiChu)
        {
            return db.MyExecuteNonQuery("spThemDiemDanh",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaNV", MaNV),
                new SqlParameter("@GioNghi", GioNghi),
                new SqlParameter("@NgayThangNam", NgayThangNam),
                new SqlParameter("@GhiChu", GhiChu));
        }

        public bool SuaDiemDanh(ref string err, string MaNV, string GioLam, string GioNghi,
            string NgayThangNam, string GhiChu)
        {
            return db.MyExecuteNonQuery("spSuaDiemDanh",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaNV", MaNV),
                new SqlParameter("@GioLam", GioLam),
                new SqlParameter("@GioNghi", GioNghi),
                new SqlParameter("@NgayThangNam", NgayThangNam),
                new SqlParameter("@GhiChu", GhiChu));
        }
    }
}
