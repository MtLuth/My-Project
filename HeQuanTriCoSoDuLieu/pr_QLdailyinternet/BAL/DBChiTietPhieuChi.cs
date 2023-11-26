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
    public class DBChiTietPhieuChi
    {
        DALayer db = null;
        public DBChiTietPhieuChi()
        {
            db = new DALayer();
        }
        public DataSet LayChiTietPhieuChi()
        {
            return db.ExecuteQueryDataSet("SELECT * FROM CHITIET_PHIEUCHI", CommandType.Text, null);
        }

        public DataSet GetChiTietPhieuChi(string SoPhieu)
        {
            return db.ExecuteQueryDataSet("SELECT * FROM LayChiTietHoaDon ('" + SoPhieu+ "')", CommandType.Text, null);
        }

        public bool XoaChiTietPhieuChi(ref string err, string SoPhieu, string MaLoaiThanhToan, int SoLuong, int sotien)
        {
            return db.MyExecuteNonQuery("spXoaChiTietPhieuChi",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@SoPhieu", SoPhieu),
                new SqlParameter("@MaLoaiThanhToan", MaLoaiThanhToan),
                new SqlParameter("@SoLuong", SoLuong),
                new SqlParameter("@sotien", sotien));
        }
        public bool ThemChiTietPhieuChi(ref string err, string SoPhieu, string MaLoaiThanhToan, int SoLuong)
        {
            return db.MyExecuteNonQuery("spThemChiTietPhieuChi",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@SoPhieu", SoPhieu),
                new SqlParameter("@MaLoaiThanhToan", MaLoaiThanhToan),
                new SqlParameter("@SoLuong", SoLuong));
        }

        public bool SuaChiTietPhieuChi(ref string err, string SoPhieu, string MaLoaiThanhToan, int SoLuong)
        {
            return db.MyExecuteNonQuery("spSuaChiTietPhieuChi",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@SoPhieu", SoPhieu),
                new SqlParameter("@MaLoaiThanhToan", MaLoaiThanhToan),
                new SqlParameter("@SoLuong", SoLuong));
        }
    }
}
