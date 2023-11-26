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
    public class DBLoaiDichVu
    {
        DALayer db = null;
        public DBLoaiDichVu()
        {
            db = new DALayer();
        }
        //
        // Các phương thức CRUD
        //SeLect
        public DataSet LayLoai()
        {
            return db.ExecuteQueryDataSet("SELECT * FROM LOAIDICHVU", CommandType.Text, null);
        }
        public bool ThemLoaiDichVu(ref string err, string MaLoai, string TenLoaiDichVu)
        {
            return db.MyExecuteNonQuery("spThemLoaiDichVu",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@Ma_LOAI", MaLoai),
                new SqlParameter("@ten_loai_dich_vu", TenLoaiDichVu));


        }
        // DELETE
        public bool XoaLoaiDichVu(ref string err, string MaLoai)
        {
            return db.MyExecuteNonQuery("spXoaLoaiDichVu",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@Ma_LOAI", MaLoai));
        }
        // UPDATE
        public bool CapNhatLoaiDichVu(ref string err, string MaLoai, string TenLoaiDichVu)
        {
            return db.MyExecuteNonQuery("spSuaLoaiDichVu",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@Ma_LOAI", MaLoai),
                new SqlParameter("@ten_loai_dich_vu", TenLoaiDichVu));
        }
    }
}
