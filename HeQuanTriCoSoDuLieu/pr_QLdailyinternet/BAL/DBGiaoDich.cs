using DBL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class DBGiaoDich
    {
        DALayer db = null;
        public DBGiaoDich()
        {
            db = new DALayer();
        }
        public DataSet LayGiaoDich()
        {
            return db.ExecuteQueryDataSet("SELECT * FROM GIAODICH", CommandType.Text, null);
        }
        public DataSet DanhSachGiaoDich()
        {
            return db.ExecuteQueryDataSet("SELECT * FROM dbo.ViewGiaoDich()", CommandType.Text, null);
        }
        public DataSet DanhSachGiaoDichTheoMaNV(string MaNV)
        {
            return db.ExecuteQueryDataSet("SELECT * FROM dbo.DanhSachGiaoDichTheoMaNV('"+ MaNV+"')", CommandType.Text, null);
        }
        public DataSet TimKiemGiaoDich(string ngayGioGD)
        {
            return db.ExecuteQueryDataSet("EXECUTE dbo.TimKiemGiaoDich '" + ngayGioGD + "'", CommandType.Text, null);
        }
        public bool DoanhThuTrongThang(ref string err, string thang, string nam, out int tongSoTien)
        {
            tongSoTien = 0;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Thang", thang),
                new SqlParameter("@Nam", nam),
                new SqlParameter("@TongSoTien", SqlDbType.Int)
            };
            parameters[2].Direction = ParameterDirection.Output;

            bool result = db.MyExecuteNonQuery("spTinhTongSoTienTheoThang", CommandType.StoredProcedure, ref err, parameters);

            if (result)
            {
                tongSoTien = (int)parameters[2].Value;
            }

            return result;
        }


        public bool XoaGiaoDich(ref string err, string MaGD)
        {
            return db.MyExecuteNonQuery("spXoaGiaoDich",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaGD", MaGD));
        }
        public bool ThemGiaoDich(ref string err, string MaNV, string MaKH, string SoTien, string NgayGioGD)
        {
            return db.MyExecuteNonQuery("spThemGiaoDich",
                CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaNV", MaNV),
                new SqlParameter("@MaKH", MaKH),
                new SqlParameter("@SoTien", SoTien),
                new SqlParameter("@ThoiGianGD", NgayGioGD));
        }

        public bool SuaGiaoDich(ref string err, string MaGD, string MaNV, string MaKH, string SoTien, string NgayGioGD)
        {
            return db.MyExecuteNonQuery("spSuaGiaoDich",
                CommandType.StoredProcedure, ref err,
                 new SqlParameter("@MaGD", MaGD),
                new SqlParameter("@MaNV", MaNV),
                new SqlParameter("@MaKH", MaKH),
                new SqlParameter("@SoTien", SoTien),
                new SqlParameter("@ThoiGianGD", NgayGioGD));
        }

    }
}
