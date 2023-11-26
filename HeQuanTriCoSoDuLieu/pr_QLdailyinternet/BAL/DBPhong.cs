using DBL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class DBPhong
    {
        DALayer db = null;
        public DBPhong()
        {
            db = new DALayer();
        }
        //
        // Các phương thức CRUD
        //SeLect
        public DataSet LayPhong()
        {
            return db.ExecuteQueryDataSet("SELECT * FROM PHONG", CommandType.Text, null);
        }
    }
}
