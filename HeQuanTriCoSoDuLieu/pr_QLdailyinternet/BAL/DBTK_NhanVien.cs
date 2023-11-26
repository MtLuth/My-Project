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
    public class DBTK_NhanVien
    {
        DALayer db = null;
        public DBTK_NhanVien()
        {
            db = new DALayer();
        }
        public DataSet LayTK_NhanVien()
        {
            return db.ExecuteQueryDataSet("SELECT * FROM TK_NHANVIEN", CommandType.Text, null);
        }
        public bool DangKyTK_NhanVien(ref string err, string MaNV, string TenDangNhap, string MatKhau)
        {
            return db.MyExecuteNonQuery("spDangKyTKNhanVien", CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaNV", MaNV),
                new SqlParameter("@TenDangNhap", TenDangNhap),
                new SqlParameter("@MatKhau", MatKhau));
        }
        public bool DoiMatKhauTK_NhanVien(ref string err, string MaNV, string TenDangNhap, string MatKhauMoi)
        {
            return db.MyExecuteNonQuery("spDoiMatKhauNV", CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaNV", MaNV),
                new SqlParameter("@TenDangNhap", TenDangNhap),
                new SqlParameter("@MatKhauMoi", MatKhauMoi));
        }
        public bool XoaTK_NhanVien(ref string err, string TenDangNhap)
        {
            return db.MyExecuteNonQuery("spXoaTKNhanVien", CommandType.StoredProcedure, ref err,
                new SqlParameter("@TenDangNhap", TenDangNhap));
        }
    }
}
