using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBL;

namespace BAL
{
    public class DBNhanVien
    {
        DALayer db = null;
        public DBNhanVien()
        {
            db = new DALayer();
        }
        //
        // Các phương thức CRUD
        // SELECT
        public DataSet LayNhanVien()
        {
            return db.ExecuteQueryDataSet("SELECT * FROM NHANVIEN", CommandType.Text, null);
        }
        public DataSet HienThiDanhSachNhanVien()
        {
            return db.ExecuteQueryDataSet("SELECT * FROM HienThiDanhSachNhanVien()", CommandType.Text, null);
        }
        // INSERT
        public bool ThemNhanVien(ref string err,
            string TenNV, DateTime NgaySinh, string DiaChi,
            string Email, string SoDT, string MaVaiTro,
            int Luong, string MaPhong)
        {
            return db.MyExecuteNonQuery("spThemNhanVien",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@TenNV", TenNV),
                new SqlParameter("@NgaySinh", NgaySinh),
                new SqlParameter("@DiaChi", DiaChi),
                new SqlParameter("@Email", Email),
                new SqlParameter("@SoDT", SoDT),
                new SqlParameter("@MaVaiTro", MaVaiTro),
                new SqlParameter("@Luong", Luong),
                new SqlParameter("@MaPhong", MaPhong));
        }
        // DELETE
        public bool XoaNhanVien(ref string err,
            string MaNV)
        {
            return db.MyExecuteNonQuery("spXoaNhanVien",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaNV", MaNV));
        }
        // UPDATE
        public bool SuaNhanVien(ref string err,
            string MaNV, string TenNV, DateTime NgaySinh, string DiaChi,
            string Email, string SoDT, string MaVaiTro,
            int Luong, string MaPhong)
        {
            return db.MyExecuteNonQuery("spSuaNhanVien",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaNV", MaNV),
                new SqlParameter("@TenNV", TenNV),
                new SqlParameter("@NgaySinh", NgaySinh),
                new SqlParameter("@DiaChi", DiaChi),
                new SqlParameter("@Email", Email),
                new SqlParameter("@SoDT", SoDT),
                new SqlParameter("@MaVaiTro", MaVaiTro),
                new SqlParameter("@Luong", Luong),
                new SqlParameter("@MaPhong", MaPhong));
        }
        public DataSet TimKiemNhanVien(string MaNV, string TenNV, DateTime? NgaySinh,
        string DiaChi, string Email, string SoDT, string MaVaiTro, int? Luong, string MaPhong,
        out string error)
        {
            string sqlString = "SELECT * FROM dbo.TimKiemNhanVien(@MaNV, @TenNV, @NgaySinh, @DiaChi, @Email, @SoDT, @MaVaiTro, @Luong, @MaPhong)";
            error = null;
            try
            {
                return db.ExecuteQueryDataSet(sqlString, CommandType.Text,
                    new SqlParameter("@MaNV", (object)MaNV ?? DBNull.Value),
                    new SqlParameter("@TenNV", (object)TenNV ?? DBNull.Value),
                    new SqlParameter("@NgaySinh", (object)NgaySinh ?? DBNull.Value),
                    new SqlParameter("@DiaChi", (object)DiaChi ?? DBNull.Value),
                    new SqlParameter("@Email", (object)Email ?? DBNull.Value),
                    new SqlParameter("@SoDT", (object)SoDT ?? DBNull.Value),
                    new SqlParameter("@MaVaiTro", (object)MaVaiTro ?? DBNull.Value),
                    new SqlParameter("@Luong", (object)Luong ?? DBNull.Value),
                    new SqlParameter("@MaPhong", (object)MaPhong ?? DBNull.Value));
            }
            catch (DataException ex)
            {
                error = ex.Message;
                return null;
            }
        }

    }
}
