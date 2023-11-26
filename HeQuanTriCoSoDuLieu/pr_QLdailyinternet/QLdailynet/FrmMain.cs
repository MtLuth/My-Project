
using BAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLdailynet
{
    public partial class FrmMain : Form
    {
        string txtTenDangNhap, txtMaMay;
        DBDangNhap dbDN;
        DBMay dbM;
        public FrmMain(string txt,string MaMay)
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            txtTenDangNhap = txt;
            txtMaMay = MaMay;
            dbDN = new DBDangNhap();
            dbM = new DBMay();
            pbHienThi.Hide();
            pbHienThi2.Hide();
            pbHienThi3.Hide();
            pbHienThi4.Hide();
            pbHienThi5.Hide();
            pbHienThi6.Hide();
            pbHienThi7.Hide();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            lbtendangnhap.Text = txtTenDangNhap;
        }
        private void FrmMain_MouseDown(object sender, MouseEventArgs e)
        {

        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();    
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void FrmMain_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                FormBorderStyle = FormBorderStyle.None;
            else
                FormBorderStyle = FormBorderStyle.Sizable;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            FrmNhanVien formNV = new FrmNhanVien();
            formNV.MdiParent = this;
            formNV.Show();
            pbHienThi.Show();
            pbHienThi2.Hide();
            pbHienThi3.Hide();
            pbHienThi4.Hide();
            pbHienThi5.Hide();
            pbHienThi6.Hide();
            pbHienThi7.Hide();
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            pbNhanVien.BorderStyle = BorderStyle.Fixed3D;
        }

        private void pbKhachHang_Click(object sender, EventArgs e)
        {
            FrmKhachHang formKH = new FrmKhachHang();
            formKH.MdiParent = this;
            formKH.Show();
            pbHienThi.Hide();
            pbHienThi2.Show();
            pbHienThi3.Hide();
            pbHienThi4.Hide();
            pbHienThi5.Hide();
            pbHienThi6.Hide();
            pbHienThi7.Hide();
        }

        private void pbKhachHang_MouseHover(object sender, EventArgs e)
        {
            pbKhachHang.BorderStyle = BorderStyle.Fixed3D;
        }

        private void pbNhanVien_MouseLeave(object sender, EventArgs e)
        {
            pbNhanVien.BorderStyle = BorderStyle.None;
        }

        private void pbKhachHang_MouseLeave(object sender, EventArgs e)
        {
            pbKhachHang.BorderStyle = BorderStyle.None;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            FrmDichVu form = new FrmDichVu();
            form.MdiParent = this;
            form.Show();
            pbHienThi.Hide();
            pbHienThi2.Hide();
            pbHienThi3.Show();
            pbHienThi4.Hide();
            pbHienThi5.Hide();
            pbHienThi6.Hide();
            pbHienThi7.Hide();
        }

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            pbDichVu.BorderStyle = BorderStyle.Fixed3D;
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pbDichVu.BorderStyle = BorderStyle.None;
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            FrmGiaoDich formKH = new FrmGiaoDich();
            formKH.MdiParent = this;
            formKH.Show();
            pbHienThi.Hide();
            pbHienThi2.Hide();
            pbHienThi3.Hide();
            pbHienThi4.Show();
            pbHienThi5.Hide();
            pbHienThi6.Hide();
            pbHienThi7.Hide();
        }

        private void pictureBox1_MouseHover_1(object sender, EventArgs e)
        {
            pbGiaoDich.BorderStyle = BorderStyle.Fixed3D;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pbGiaoDich.BorderStyle = BorderStyle.None;
        }

        private void pictureBox1_Click_2(object sender, EventArgs e)
        {
            FrmPhieuChi form = new FrmPhieuChi();
            form.MdiParent = this;
            form.Show();
            pbHienThi.Hide();
            pbHienThi2.Hide();
            pbHienThi3.Hide();
            pbHienThi4.Hide();
            pbHienThi5.Show();
            pbHienThi6.Hide();
            pbHienThi7.Hide();
        }

        private void pictureBox1_MouseHover_2(object sender, EventArgs e)
        {
            pbPhieuChi.BorderStyle = BorderStyle.Fixed3D;
        }

        private void pictureBox1_MouseLeave_1(object sender, EventArgs e)
        {
            pbPhieuChi.BorderStyle = BorderStyle.None;
        }

        private void pictureBox1_Click_3(object sender, EventArgs e)
        {
            DialogResult traloi;
            traloi = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK)
            {
                this.Close();
                FrmDangNhap formmain = new FrmDangNhap();
                formmain.Show();
                dbDN.DanhDauDangXuatNV(txtTenDangNhap);
                dbM.DanhDauDangXuatMay(txtMaMay);
            }    
               
            
        }

        private void pictureBox1_MouseHover_3(object sender, EventArgs e)
        {
            pbThoat.BorderStyle = BorderStyle.Fixed3D;
        }

        private void pictureBox1_MouseLeave_2(object sender, EventArgs e)
        {
            pbThoat.BorderStyle = BorderStyle.None;
        }

        private void pictureBox1_Click_4(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pbMay_Click(object sender, EventArgs e)
        {
            FrmMay form = new FrmMay();
            form.MdiParent = this;
            form.Show();
            pbHienThi.Hide();
            pbHienThi2.Hide();
            pbHienThi3.Hide();
            pbHienThi4.Hide();
            pbHienThi5.Hide();
            pbHienThi6.Show();
            pbHienThi7.Hide();
        }

        private void pbMay_MouseHover(object sender, EventArgs e)
        {
            pbMay.BorderStyle = BorderStyle.Fixed3D;
        }

        private void pbMay_MouseLeave(object sender, EventArgs e)
        {
            pbMay.BorderStyle = BorderStyle.None;
        }

        private void pbDiemDanh_MouseHover(object sender, EventArgs e)
        {
            pbDiemDanh.BorderStyle = BorderStyle.Fixed3D;
        }

        private void pbDiemDanh_MouseLeave(object sender, EventArgs e)
        {
            pbDiemDanh.BorderStyle = BorderStyle.None;
        }

        private void pictureBox1_MouseHover_4(object sender, EventArgs e)
        {
            pbMiniSize.BorderStyle = BorderStyle.Fixed3D;
        }

        private void pbMiniSize_MouseLeave(object sender, EventArgs e)
        {
            pbMiniSize.BorderStyle = BorderStyle.None;
        }

        private void pbDiemDanh_Click(object sender, EventArgs e)
        {
            FrmDiemDanh form = new FrmDiemDanh();
            form.MdiParent = this;
            form.Show();
            pbHienThi.Hide();
            pbHienThi2.Hide();
            pbHienThi3.Hide();
            pbHienThi4.Hide();
            pbHienThi5.Hide();
            pbHienThi6.Hide();
            pbHienThi7.Show();
        }

        private void pictureBox1_MouseHover_5(object sender, EventArgs e)
        {
            pbThuChi.BorderStyle = BorderStyle.Fixed3D;
        }

        private void pbThuChi_Click(object sender, EventArgs e)
        {
            FrmThuChi form = new FrmThuChi();
            form.MdiParent = this;
            form.Show();
            pbHienThi.Hide();
            pbHienThi2.Hide();
            pbHienThi3.Hide();
            pbHienThi4.Hide();
            pbHienThi5.Hide();
            pbHienThi6.Hide();
            pbHienThi7.Hide();
            pbThuChii.Show();
        }

        private void pbThuChi_MouseLeave(object sender, EventArgs e)
        {
            pbThuChi.BorderStyle = BorderStyle.None;
        }
    }
}
