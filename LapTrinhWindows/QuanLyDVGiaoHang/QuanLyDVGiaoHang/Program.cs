using QuanLyDVGiaoHang.GUI;
using QuanLyDVGiaoHang.GUI.BangGia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyDVGiaoHang
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FrmDangNhap frmLogin = new FrmDangNhap();
            Application.Run(frmLogin);
            //Application.Run(new FrmMain());
            //Application.Run(new FrmNhanVien());
            if (frmLogin.isSuccessfull)
            {
                Application.Run(new FrmMain(frmLogin.tenDangNhap, frmLogin.maVaiTro));
            }
        }
    }
}
