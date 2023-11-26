using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyDVGiaoHang.GUI;
using QuanLyDVGiaoHang.Models;

namespace QuanLyDVGiaoHang
{
    public partial class FrmDangNhap : Form
    {
        public bool isSuccessfull = false;
        private QuanLyDVGiaoHangEntities db;
        public string tenDangNhap, maVaiTro;
        public FrmDangNhap()
        {
            InitializeComponent();
            pnDangNhap.BackColor = Color.FromArgb(0, Color.White);
            db = new QuanLyDVGiaoHangEntities();
            var roles = db.VAITROes.ToList();
            var KH = new VAITRO
            {
                MaVaiTro = "KH",
                TenVaiTro = "Khách hàng"
            };
            roles.Add(KH);
            cbVaiTro.DisplayMember = "TenVaiTro";
            cbVaiTro.ValueMember = "MaVaiTro"; 
            cbVaiTro.DataSource = roles;
            cbVaiTro.Text = "Khách hàng";
        }
        private bool txtDangNhapHasError = false;

        private void txtDangNhap_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDangNhap.Text))
            {
                txtDangNhapHasError = true;
                errorProviderUser.SetError(txtDangNhap, "Vui lòng nhập tên đăng nhập!");
            }
            else
            {
                txtDangNhapHasError = false;
                errorProviderUser.SetError(txtDangNhap, "");
            }
        }

        private void txtDangNhap_Validated_1(object sender, EventArgs e)
        {
            if (!txtDangNhapHasError)
            {
                errorProviderUser.SetError(txtDangNhap, "");
            }
        }
        private bool txtMatKhauHasError = false;

        private void txtMatKhau_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                txtMatKhauHasError = true;
                errorProviderUser.SetError(txtMatKhau, "Vui lòng nhập mật khẩu!");
            }
            else
            {
                txtMatKhauHasError = false;
                errorProviderUser.SetError(txtMatKhau, "");
            }
        }

        private void txtMatKhau_Validated(object sender, EventArgs e)
        {
            if (!txtMatKhauHasError)
            {
                errorProviderUser.SetError(txtMatKhau, ""); 
            }
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            if (txtDangNhap.Text == "")
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông Báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            if (txtMatKhau.Text == "")
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông Báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            string username = txtDangNhap.Text;
            string password = txtMatKhau.Text;
            if (cbVaiTro.Text == "Quản lý")
            {
                var user = db.TK_QUANLY.FirstOrDefault(u => u.TenDangNhap.Trim() == username && u.MatKhau.Trim() == password);
                if (user != null)
                {
                    // Đăng nhập thành công
                    MessageBox.Show("Đăng nhập thành công với tài khoản quản lý!", "Thông Báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    tenDangNhap = txtDangNhap.Text.Trim();
                    maVaiTro = cbVaiTro.Text.Trim();
                    isSuccessfull = true;
                    this.Close();
                }
                else
                {
                    // Đăng nhập thất bại
                    MessageBox.Show("Đăng nhập thất bại!");
                }
            }
            if (cbVaiTro.Text == "Nhân viên giao hàng")
            {
                var user = db.TK_NVGIAOHANG.FirstOrDefault(u => u.TenDangNhap.Trim() == username && u.MatKhau.Trim() == password);
                if (user != null)
                {
                    // Đăng nhập thành công
                    MessageBox.Show("Đăng nhập thành công với tài khoản nhân viên giao hàng!", "Thông Báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    tenDangNhap = txtDangNhap.Text.Trim();
                    maVaiTro = cbVaiTro.Text.Trim();
                    isSuccessfull = true;
                    this.Close();
                }
                else
                {
                    // Đăng nhập thất bại
                    MessageBox.Show("Đăng nhập thất bại!");
                }
            }
            if (cbVaiTro.Text == "Nhân viên trung chuyển")
            {
                var user = db.TK_NVTRUNGCHUYEN.FirstOrDefault(u => u.TenDangNhap.Trim() == username && u.MatKhau.Trim() == password);
                if (user != null)
                {
                    // Đăng nhập thành công
                    MessageBox.Show("Đăng nhập thành công với tài khoản nhân viên trung chuyển!", "Thông Báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    tenDangNhap = txtDangNhap.Text.Trim();
                    maVaiTro = cbVaiTro.Text.Trim();
                    isSuccessfull = true;
                    this.Close();
                }
                else
                {
                    // Đăng nhập thất bại
                    MessageBox.Show("Đăng nhập thất bại!");
                }
            }
            if (cbVaiTro.Text == "Khách hàng")
            {
                var user = db.TK_KHACHHANG.FirstOrDefault(u => u.TenDangNhap.Trim() == username && u.MatKhau.Trim() == password);
                if (user != null)
                {
                    // Đăng nhập thành công
                    MessageBox.Show("Đăng nhập thành công với tài khoản khách hàng!", "Thông Báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    tenDangNhap = txtDangNhap.Text.Trim();
                    maVaiTro = cbVaiTro.Text.Trim();
                    isSuccessfull = true;
                    this.Close();
                }
                else
                {
                    // Đăng nhập thất bại
                    MessageBox.Show("Đăng nhập thất bại!");
                }
            }
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn Có Muốn Thoát Chương Trình ?", "Thông Báo",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();
        }

        private void pbHide_Click(object sender, EventArgs e)
        {
            txtMatKhau.PasswordChar = '\0';
            pbUnHide.Visible = true; 
            pbHide.Visible = false;
        }

        private void pbUnHide_Click(object sender, EventArgs e)
        {
            txtMatKhau.PasswordChar = '*';
            pbUnHide.Visible = false;
            pbHide.Visible = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            FrmDangKyTKDN formDangKy = new FrmDangKyTKDN();
            formDangKy.ShowDialog();
        }
    }
}
