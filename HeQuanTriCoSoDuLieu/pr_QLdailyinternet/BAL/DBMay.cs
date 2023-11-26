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
    public class DBMay
    {
        DALayer db = null;
        public DBMay()
        {
            db = new DALayer();
        }
        public DataSet LayMay()
        {
            return db.ExecuteQueryDataSet(
                "select * from MAY", CommandType.Text, null);
        }
        public DataSet LayThongTinMay(string MaMay)
        {
            return db.ExecuteQueryDataSet("EXEC dbo.spLayThongTinMay '" + MaMay + "'", CommandType.Text, null);
        }
        public DataSet DanhDauDangXuatMay(string MaMay)
        {
            return db.ExecuteQueryDataSet("EXEC dbo.spDanhDauDangXuatMay '" + MaMay + "'", CommandType.Text, null);
        }

        public DataSet HienThiThongTinMay()
        {
            return db.ExecuteQueryDataSet("SELECT * FROM dbo.HienThiThongTinMay()", CommandType.Text, null);
        }
        public DataSet HienThiThongTinMayTat()
        {
            return db.ExecuteQueryDataSet("SELECT * FROM dbo.HienThiThongTinMayTat()", CommandType.Text, null);
        }
        public DataSet HienThiThongTinMayBat()
        {
            return db.ExecuteQueryDataSet("SELECT * FROM dbo.HienThiThongTinMayBat()", CommandType.Text, null);
        }

        //
        // INSERT
        public bool ThemMAY(ref string err, string MaMay, string MaPhong, string Trangthai, string Giatien, string GhiChu)
        {
            return db.MyExecuteNonQuery("spThemMay",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@Ma_May", MaMay),
                new SqlParameter("@Ma_Phong", MaPhong),
                new SqlParameter("@Trang_thai", Trangthai),
                new SqlParameter("@Gia_tien", Giatien),
                new SqlParameter("@Ghi_chu", GhiChu));

        }
        // DELETE
        public bool XoaMay(ref string err, string MaMay)
        {
            return db.MyExecuteNonQuery("spXoaMay",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@Ma_May", MaMay));
        }
        // UPDATE
        public bool CapNhatMay(ref string err, string MaMay, string MaPhong, string Trangthai, string Giatien, string GhiChu)
        {
            return db.MyExecuteNonQuery("spSuaMay",
                CommandType.StoredProcedure, ref err,
                 new SqlParameter("@Ma_May", MaMay),
                new SqlParameter("@Ma_Phong", MaPhong),
                new SqlParameter("@Trang_thai", Trangthai),
                new SqlParameter("@Gia_tien", Giatien),
                new SqlParameter("@Ghi_chu", GhiChu));
        }

        public DataSet TimKiemMay(string MaMay, string MaPhong, int? TrangThai, int? GiaTien, string GhiChu, int? BaoTri, out string err)
        {
            err = null;
            string sqlString = "SELECT * FROM dbo.TimKiemMay(@MaMay, @MaPhong, @TrangThai, @GiaTien, @GhiChu, @BaoTri)";
            try
            {
                return db.ExecuteQueryDataSet(sqlString, CommandType.Text,
                    new SqlParameter("@MaMay", (Object)MaMay ?? DBNull.Value),
                    new SqlParameter("MaPhong", (Object)MaPhong ?? DBNull.Value),
                    new SqlParameter("TrangThai", (Object)TrangThai ?? DBNull.Value),
                    new SqlParameter("@GiaTien", (Object)GiaTien ?? DBNull.Value),
                    new SqlParameter("@GhiChu", (Object)GhiChu ?? DBNull.Value),
                    new SqlParameter("@BaoTri", (Object)BaoTri ?? DBNull.Value));
            }
            catch (DataException ex)
            {
                err = ex.Message;
                return null;
            }
        }
    }
}
