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
    public class DBTK_KhachHang
    {
        DALayer db = null;
        public DBTK_KhachHang()
        {
            db = new DALayer();
        }
        public DataSet LayTK_KhachHang()
        {
            return db.ExecuteQueryDataSet("SELECT * FROM TK_KHACHHANG", CommandType.Text, null);
        }
        public bool DangKyTK_KhachHang(ref string err, string MaKH, string TenDangNhap, string MatKhau)
        {
            return db.MyExecuteNonQuery("spDangKyTKKhachHang", CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaKH", MaKH),
                new SqlParameter("@TenDangNhap", TenDangNhap),
                new SqlParameter("@MatKhau", MatKhau));
        }
        public bool DoiMatKhauTK_KhachHang(ref string err, string MaKH, string TenDangNhap, string MatKhauMoi)
        {
            return db.MyExecuteNonQuery("spDoiMatKhauKH", CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaKH", MaKH),
                new SqlParameter("@TenDangNhap", TenDangNhap),
                new SqlParameter("@MatKhauMoi", MatKhauMoi));
        }
        public bool XoaTK_KhachHang(ref string err, string TenDangNhap)
        {
            return db.MyExecuteNonQuery("spXoaTKKhachHang", CommandType.StoredProcedure, ref err,
                new SqlParameter("@TenDangNhap", TenDangNhap));
        }
    }
}
