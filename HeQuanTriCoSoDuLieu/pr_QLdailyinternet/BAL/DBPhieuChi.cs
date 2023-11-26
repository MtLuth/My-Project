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
    public class DBPhieuChi
    {
        DALayer db = null;
        public DBPhieuChi()
        {
            db = new DALayer();
        }

        public DataSet LayPhieuChi()
        {
            return db.ExecuteQueryDataSet("SELECT * FROM PHIEUCHI", CommandType.Text, null);
        }
        public DataSet HienThiDanhSachPhieuChi()
        {
            return db.ExecuteQueryDataSet("SELECT * FROM HienThiDanhSachPhieuChi()", CommandType.Text, null);
        }
        public bool ThemPhieuChi(ref string err,
            string MaNV, string NgayGioGD)
        {
            return db.MyExecuteNonQuery("spThemPhieuChi",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaNV", MaNV),
                new SqlParameter("@ThoiGianThanhToan", NgayGioGD));
        }
        public bool ChiPhiTrongThang(ref string err, string thang, string nam, out int tongSoTien)
        {
            tongSoTien = 0;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Thang", thang),
                new SqlParameter("@Nam", nam),
                new SqlParameter("@TongSoTien", SqlDbType.Int)
            };
            parameters[2].Direction = ParameterDirection.Output;

            bool result = db.MyExecuteNonQuery("spTinhTongChiphiTienTheoThang", CommandType.StoredProcedure, ref err, parameters);

            if (result)
            {
                tongSoTien = (int)parameters[2].Value;
            }

            return result;
        }
        // DELETE
        public bool XoaPhieuChi(ref string err,
            string SoPhieu)
        {
            return db.MyExecuteNonQuery("spXoaPhieuChi",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@SoPhieu", SoPhieu));
        }
        // UPDATE
        public bool SuaPhieuChi(ref string err,
             string SoPhieu, string MaNV, string ThoiGianGD)
        {
            return db.MyExecuteNonQuery("spSuaPhieuChi",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@SoPhieu", SoPhieu),
                new SqlParameter("@MaNV", MaNV),
                new SqlParameter("@ThoiGianThanhToan", ThoiGianGD));
        }
    }
}
