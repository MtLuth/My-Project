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
    public class DBKhachHang
    {
        DALayer db = null;
        public DBKhachHang()
        {
            db = new DALayer();
        }

        public DataSet LayKhachHang()
        {
            return db.ExecuteQueryDataSet("SELECT * FROM KHACHHANG", CommandType.Text, null);
        }
        public DataSet HienThiDanhSachKhachHang()
        {
            return db.ExecuteQueryDataSet("SELECT * FROM HienThiDanhSachKhachHang()", CommandType.Text, null);
        }
        public DataSet HienThiDanhSachKhachHangConTien()
        {
            return db.ExecuteQueryDataSet("SELECT * FROM HienThiDanhSachKhachHangConTien()", CommandType.Text, null);
        }
        public bool ThemKhachHang(ref string err,
            string TenKH, string DiaChi,
            string Email, string SoDT)
        {
            return db.MyExecuteNonQuery("spThemKhachHang",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@TenKH", TenKH),
                new SqlParameter("@DiaChi", DiaChi),
                new SqlParameter("@Email", Email),
                new SqlParameter("@SoDT", SoDT));
        }
        // DELETE
        public bool XoaKhachHang(ref string err,
            string MaKH)
        {
            return db.MyExecuteNonQuery("spXoaKhachHang",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaKH", MaKH));
        }
        // UPDATE
        public bool SuaKhachHang(ref string err,
            string MaKH, string TenKH, string DiaChi,
            string Email, string SoDT)
        {
            return db.MyExecuteNonQuery("spSuaKhachHang",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaKH", MaKH),
                new SqlParameter("@TenKH", TenKH),
                new SqlParameter("@DiaChi", DiaChi),
                new SqlParameter("@Email", Email),
                new SqlParameter("@SoDT", SoDT));
        }
        public DataSet TimKiemKhachHang(string MaKH, string TenKH,
           string DiaChi, string Email, string SoDT, out string error)
        {
            string sqlString = "SELECT * FROM dbo.TimKiemKhachHang(@MaKH, @TenKH, @DiaChi, @Email, @SoDT)";
            error = null;
            try
            {
                return db.ExecuteQueryDataSet(sqlString, CommandType.Text,
                    new SqlParameter("@MaKH", (object)MaKH ?? DBNull.Value),
                    new SqlParameter("@TenKH", (object)TenKH ?? DBNull.Value),
                    new SqlParameter("@DiaChi", (object)DiaChi ?? DBNull.Value),
                    new SqlParameter("@Email", (object)Email ?? DBNull.Value),
                    new SqlParameter("@SoDT", (object)SoDT ?? DBNull.Value));
            }
            catch (DataException ex)
            {
                error = ex.Message;
                return null;
            }
        }
    }
}
