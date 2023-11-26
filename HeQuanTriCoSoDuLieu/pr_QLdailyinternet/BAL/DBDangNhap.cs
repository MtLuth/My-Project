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
    public class DBDangNhap
    {
        DALayer db = null;
        public DBDangNhap()
        {
            db = new DALayer();
        }
        public DataSet LayThongTinDangNhap(string TenDangNhap)
        {
            return db.ExecuteQueryDataSet("EXEC dbo.spLayThongTinDangNhap '"+ TenDangNhap + "'", CommandType.Text, null);
        }
        public DataSet DanhDauDangXuat(string TGBatDau, string TenDangNhap)
        {
            return db.ExecuteQueryDataSet("EXEC dbo.spDanhDauDangXuat '" + TGBatDau + "' ,'" + TenDangNhap + "'",
                CommandType.Text, null);
        }
        public DataSet DanhDauDangXuatNV(string TenDangNhap)
        {
            return db.ExecuteQueryDataSet("EXEC dbo.spDanhDauDangXuatNV '" + TenDangNhap + "'",
                CommandType.Text, null);
        }
        public DataSet UpdateTGKetThuc(string TGBatDau, string TenDangNhap)
        {
            return db.ExecuteQueryDataSet("EXEC dbo.UpdateDANGNHAP_TGKetThuc '" + TGBatDau + "' ,'" + TenDangNhap + "'",
                CommandType.Text, null);
        }

        public bool KiemTraSoDuVaThanhToan(ref string err,string TenDangNhap, string TGBatDau, string MaMay)
        {
            return db.MyExecuteNonQuery("spKiemTraSoDuVaThanhToan",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@tendangnhap", TenDangNhap),
                new SqlParameter("@id_may", MaMay),
                new SqlParameter("@start_time", TGBatDau));
        }
    }

 }
