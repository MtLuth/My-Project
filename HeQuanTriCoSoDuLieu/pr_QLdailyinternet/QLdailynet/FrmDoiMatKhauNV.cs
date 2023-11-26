using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BAL;
using DBL;

namespace QLdailynet
{
    public partial class FrmDoiMatKhau : Form
    {
        DBTK_NhanVien dbTKNV = null;
        public FrmDoiMatKhau()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            dbTKNV = new DBTK_NhanVien();
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (this.txtMaNhanVien2.Text == "")
            {
                MessageBox.Show("Mã nhân viên không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaNhanVien2.Focus();
                return;
            }
            if (this.txtTenDangNhap.Text == "")
            {
                MessageBox.Show("Tên đăng nhập không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTenDangNhap.Focus();
                return;
            }
            if (this.txtMatKhau.Text == "")
            {
                MessageBox.Show("Mật khẩu không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatKhau.Focus();
                return;
            }
            try
            {
                bool f = false;
                string err = "";
                f = dbTKNV.DoiMatKhauTK_NhanVien(ref err, txtMaNhanVien2.Text, txtTenDangNhap.Text, txtMatKhau.Text);
                if (f)
                {
                    FrmNhanVien frm = (FrmNhanVien)Application.OpenForms["FrmNhanVien"];
                    if (frm != null)
                    {
                        frm.LoadData();
                    }
                    MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK);
                    txtMaNhanVien2.ResetText();
                    txtTenDangNhap.ResetText();
                    txtMatKhau.ResetText();
                }
                else
                {
                    MessageBox.Show("Đổi mật khẩu thất bại!\n\r Lỗi: " + err, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }    
                
            }
            catch (SqlException ex) 
            {
                MessageBox.Show("Đổi mật khẩu không thành công!\n\r" +ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pbThoat_Click(object sender, EventArgs e)
        {
            DialogResult traloi;
            traloi = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK)
                this.Close();
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
