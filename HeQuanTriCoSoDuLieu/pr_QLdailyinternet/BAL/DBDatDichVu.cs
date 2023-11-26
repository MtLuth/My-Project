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
    public class DBDatDichVu
    { 
        //minhngltt
        //minh2003
        //minhngltt1
        //minh2002

        //taiquanly
        //tai2023
        DALayer db = new DALayer();
        public DataSet LayDatDichVu()
        {
            return db.ExecuteQueryDataSet("SELECT * FROM HienThiDatDichVu()", CommandType.Text, null);
        }
        public DataSet ChonDichVu(string MaDV)
        {
            string sqlStr = "Select TenDV, GiaTien, SoLuong, HinhAnh From DichVu Where MaDV = @MaDV";
            return db.ExecuteQueryDataSet(sqlStr, CommandType.Text,
                new SqlParameter("@MaDV", MaDV));
        }
        public bool DatDichVu(ref string err, string MaMay, string MaDV, string SoLuong, string GiaTien)
        {
            err = "";
            return db.MyExecuteNonQuery("spDatDichVu", CommandType.StoredProcedure, ref err,
                new SqlParameter("@MaMay", MaMay),
                new SqlParameter("@MaDV", MaDV),
                new SqlParameter("@SoLuong", SoLuong),
                new SqlParameter("@GiaTien", GiaTien));
        }
    }
}
