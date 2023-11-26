using QuanLyDVGiaoHang.GUI.BangGia;
using QuanLyDVGiaoHang.GUI.DonHang;
using QuanLyDVGiaoHang.GUI.DSDonHangDaDenKho;
using QuanLyDVGiaoHang.GUI.DSDonHangNVGiaoHang;
using QuanLyDVGiaoHang.GUI.DSDonHangNVGiaoNhanHang;
using QuanLyDVGiaoHang.GUI.DSDonHangNVVanChuyen;
using QuanLyDVGiaoHang.GUI.KhachHang;
using QuanLyDVGiaoHang.GUI.KhoTrungChuyen;
using QuanLyDVGiaoHang.GUI.LoaiHangHoa;
using QuanLyDVGiaoHang.GUI.NhanVien;
using QuanLyDVGiaoHang.GUI.TaiKhoan;
using QuanLyDVGiaoHang.GUI.TaoDon;
using QuanLyDVGiaoHang.GUI.ThongTinTaiKhoan;
using QuanLyDVGiaoHang.GUI.VaiTro;
using QuanLyDVGiaoHang.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyDVGiaoHang.GUI
{
    public partial class FrmMain : Form
    {
        private bool isFormNhanVienOpen = false;
        private bool isFormBangGiaOpen = false;
        private bool isFormKhachHangOpen = false;
        private bool isFormThongTinTK = false; 
        private bool isFormTaoDon = false;
        private bool isFormDonHangKH = false;
        private bool isFormDonHangNVNhanHang = false;
        private bool isFormDSDonHangNVNhanHang = false;
        private bool isFormDSDonHangNVGiaoHang = false; 
        private bool isFormDSDonHangDaDenKho = false;
        private bool isFormKhoTrungChuyen = false;
        private bool isFormLoaiHangHoa = false;
        private bool isFormVaiTro = false; 
        private bool isFormTaiKhoan = false;
        private bool isFormDonHangNVGiaoHang = false; 
        private bool isFrmDSDonHangNVVanChuyen = false; 
        private bool isFormTatCaDonHang = false;

        string TenDangNhap, MaVaiTro, TenKhachHang;
        public FrmMain(string tenDangNhap, string maVaitro)
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            TenDangNhap = tenDangNhap;
            MaVaiTro = maVaitro;
            if(maVaitro == "Khách hàng")
            {
                imbNhanVien.Visible = false;
                imbKhachHang.Visible = false;
                imbVaiTro.Visible = false;
                imbTaiKhoan.Visible = false;
                imbDSDonHangNVGiaoHang.Visible = false;
                imbDSDonHangNVVanChuyen.Visible = false;
                imbDonHangGiao.Visible = false;
                imbDonHangNhan.Visible = false;
                imbDanhSachDaDenKho.Visible = false;
                imbDSNVNhanHang.Visible = false;
                imbAllDonHang.Visible = false;
            }
            if (maVaitro == "Nhân viên trung chuyển")
            {
                imbNhanVien.Visible = false;
                imbTaoDon.Visible = false;
                imbVaiTro.Visible = false;
                imbTaiKhoan.Visible = false;
                imbThongTinTK.Visible = false;
                imbDonHangNhan.Visible = false;
                imbDonHangGiao.Visible = false;
                imbDSNVNhanHang.Visible = false;
                imbDSDonHangNVGiaoHang.Visible = false;
                //imbDSDonHangNVVanChuyen.Visible=false;
                imbDanhSachDaDenKho.Visible=false;
                imbAllDonHang.Visible = false;
            }
            if (maVaitro == "Nhân viên giao hàng")
            {
                imbNhanVien.Visible = false;
                imbTaoDon.Visible = false;
                imbDonHang.Visible = false;
                imbVaiTro.Visible = false;
                imbTaiKhoan.Visible = false;
                imbThongTinTK.Visible = false;
                imbDSDonHangNVVanChuyen.Visible = false;
                imbDanhSachDaDenKho.Visible = false;
                imbAllDonHang.Visible = false;
            }
            if (maVaitro == "Quản lý")
            {
                imbThongTinTK.Visible = false;
                imbTaoDon.Visible = false;
                imbDonHangNhan.Visible = false;
                imbDonHangGiao.Visible = false;
                imbDSDonHangNVVanChuyen.Visible = false;
                imbDSDonHangNVGiaoHang.Visible = false;
                imbDSNVNhanHang.Visible = false;
            }
        }
        private void loadData()
        {
            if (MaVaiTro == "Khách hàng")
            {
                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    var khachHang = context.TK_KHACHHANG
                        .Where(tk => tk.TenDangNhap.Trim() == TenDangNhap)
                        .Select(tk => new { tk.KHACHHANG.TenNguoiBan, tk.KHACHHANG.MaKhachHang, tk.SoDu })
                        .FirstOrDefault();

                    if (khachHang != null)
                    {
                        TenKhachHang = khachHang.TenNguoiBan.Trim();
                        lbXinChao.Text = "Xin Chào, " + khachHang.TenNguoiBan.Trim() + " ID: " + khachHang.MaKhachHang.ToString().Trim() + " Số dư: " + khachHang.SoDu.ToString();
                    }
                }
            }

            if (MaVaiTro == "Quản lý")
            {
                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    var nhanVien = context.TK_QUANLY
                        .Where(tk => tk.TenDangNhap.Trim() == TenDangNhap)
                        .Select(tk => tk.NHANVIEN)
                        .FirstOrDefault();

                    if (nhanVien != null)
                    {
                        lbXinChao.Text = "Xin Chào, " + nhanVien.HoTen.Trim() + " ID: " + nhanVien.MaNV.ToString().Trim();
                    }
                }
            }
            if (MaVaiTro == "Nhân viên trung chuyển")
            {
                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    var nhanVien = context.TK_NVTRUNGCHUYEN
                        .Where(tk => tk.TenDangNhap.Trim() == TenDangNhap)
                        .Select(tk => tk.NHANVIEN)
                        .FirstOrDefault();

                    if (nhanVien != null)
                    {
                        lbXinChao.Text = "Xin Chào, " + nhanVien.HoTen.Trim() + " ID: " + nhanVien.MaNV.ToString().Trim();
                    }
                }
            }
            if (MaVaiTro == "Nhân viên giao hàng")
            {
                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    var nhanVien = context.TK_NVGIAOHANG
                        .Where(tk => tk.TenDangNhap.Trim() == TenDangNhap)
                        .Select(tk => tk.NHANVIEN)
                        .FirstOrDefault();

                    if (nhanVien != null)
                    {
                        lbXinChao.Text = "Xin Chào, " + nhanVien.HoTen.Trim() + " ID: " + nhanVien.MaNV.ToString().Trim();
                    }
                }
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            if (!isFormNhanVienOpen)
            {
                FrmNhanVien formNhanVien = new FrmNhanVien();
                formNhanVien.MdiParent = this;
                formNhanVien.FormClosed += FormNhanVien_FormClosed;
                formNhanVien.Show();
                isFormNhanVienOpen = true;
            }
            else
            {
                MessageBox.Show("Bạn đã mở bảng Nhân Viên. Vui lòng kiểm tra lại !",
                    "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormNhanVien_FormClosed(object sender, FormClosedEventArgs e)
        {
            isFormNhanVienOpen = false;
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            if (!isFormBangGiaOpen)
            {
                FrmBangGia formBangGia = new FrmBangGia(MaVaiTro);
                formBangGia.MdiParent = this;
                formBangGia.FormClosed += FormBangGia_FormClosed;
                formBangGia.Show();
                isFormBangGiaOpen = true;
            }
            else
            {
                MessageBox.Show("Bạn đã mở bảng Bảng Giá. Vui lòng kiểm tra lại !",
                    "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FormBangGia_FormClosed(object sender, FormClosedEventArgs e)
        {
            isFormBangGiaOpen = false;
        }

        private void bunifuImageButton1_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn Có Muốn Thoát Chương Trình ?", "Thông Báo",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                FrmDangNhap frmLogin = new FrmDangNhap();
                frmLogin.isSuccessfull = false;
                this.Close();
            }    
        }

        private void imbKhachHang_Click(object sender, EventArgs e)
        {
            if (!isFormKhachHangOpen)
            {
                FrmKhachHang formKhachHang = new FrmKhachHang(MaVaiTro);
                formKhachHang.MdiParent = this;
                formKhachHang.FormClosed += FormKhachHang_FormClosed;
                formKhachHang.Show();
                isFormKhachHangOpen = true;
            }
            else
            {
                MessageBox.Show("Bạn đã mở bảng Khách Hàng. Vui lòng kiểm tra lại !",
                    "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FormKhachHang_FormClosed(object sender, FormClosedEventArgs e)
        {
            isFormKhachHangOpen = false;
        }

        private void bunifuImageButton3_Click_1(object sender, EventArgs e)
        {
            if (!isFormThongTinTK)
            {
                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    FrmThongTinTK formThongTinTK = new FrmThongTinTK(TenDangNhap);
                    formThongTinTK.MdiParent = this;
                    formThongTinTK.FormClosed += FormTTTaiKhoan_FormClosed;
                    var tkKhachHang = context.TK_KHACHHANG.FirstOrDefault(tk => tk.TenDangNhap.Trim() == TenDangNhap);
                    if (tkKhachHang != null)
                    {
                        var khachHang = context.KHACHHANGs.FirstOrDefault(kh => kh.MaKhachHang == tkKhachHang.MaKhachHang);
                        formThongTinTK.kHACHHANG = khachHang;
                        formThongTinTK.Show();
                    }
                    else
                    {
                        MessageBox.Show("Tài khoản không tồn tại!",
                            "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                isFormThongTinTK = true;
            }
            else
            {
                MessageBox.Show("Bạn đã mở bảng Thông Tin Tài Khoản. Vui lòng kiểm tra lại !",
                    "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FormTTTaiKhoan_FormClosed(object sender, FormClosedEventArgs e)
        {
            isFormThongTinTK = false;
        }
        private void bunifuImageButton5_Click(object sender, EventArgs e)
        {
            if (flowLayoutPanel1.Size == new Size(354, 802))
            {
                flowLayoutPanel1.Size = new Size(173, 797);
                imbMoRong.Visible = true;
                imbDongMoRong.Visible = false;
            }
            else
            {
                flowLayoutPanel1.Size = new Size(354, 802);
                imbMoRong.Visible = false;
                imbDongMoRong.Visible = true;
            }
        }

        private void imbDongMoRong_Click(object sender, EventArgs e)
        {
            if (flowLayoutPanel1.Size == new Size(173, 797))
            {
                flowLayoutPanel1.Size = new Size(354, 802);
                imbMoRong.Visible = false;
                imbDongMoRong.Visible = true;
            }
            else
            {
                flowLayoutPanel1.Size = new Size(173, 797);
                imbMoRong.Visible = true;
                imbDongMoRong.Visible = false;
            }
        }

        private void bunifuImageButton3_Click_2(object sender, EventArgs e)
        {
            if (!isFormTaoDon)
            {
                FrmTaoDonKH formTaoDonKH = new FrmTaoDonKH(TenKhachHang);
                formTaoDonKH.MdiParent = this;
                formTaoDonKH.FormClosed += FormTaoDon_FormClosed;
                formTaoDonKH.Show();
                isFormTaoDon = true;
            }
            else
            {
                MessageBox.Show("Bạn đã mở bảng Tạo Đơn. Vui lòng kiểm tra lại !",
                    "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FormTaoDon_FormClosed(object sender, FormClosedEventArgs e)
        {
            isFormTaoDon = false;
        }

        private void imbDonHang_Click(object sender, EventArgs e)
        {
            if(MaVaiTro == "Khách hàng")
            {
                if (!isFormDonHangKH)
                {
                    FrmDonHangKH formDonHangKH = new FrmDonHangKH(TenKhachHang);
                    formDonHangKH.MdiParent = this;
                    formDonHangKH.FormClosed += FormDonHangKH_FormClosed;
                    formDonHangKH.Show();
                    isFormDonHangKH = true;
                }
                else
                {
                    MessageBox.Show("Bạn đã mở bảng Đơn Hàng. Vui lòng kiểm tra lại !",
                        "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (MaVaiTro == "Quản lý")
            {
                if (!isFormDonHangKH)
                {
                    FrmDonHangQuanLy formDonHangQL = new FrmDonHangQuanLy(TenDangNhap);
                    formDonHangQL.MdiParent = this;
                    formDonHangQL.FormClosed += FormDonHangKH_FormClosed;
                    formDonHangQL.Show();
                    isFormDonHangKH = true;
                }
                else
                {
                    MessageBox.Show("Bạn đã mở bảng Đơn Hàng. Vui lòng kiểm tra lại !",
                        "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
            if (MaVaiTro == "Nhân viên trung chuyển")
            {
                if (!isFormDonHangKH)
                {
                    FrmDonHangNVTrungChuyen formDonHangTT = new FrmDonHangNVTrungChuyen(TenDangNhap);
                    formDonHangTT.MdiParent = this;
                    formDonHangTT.FormClosed += FormDonHangKH_FormClosed;
                    formDonHangTT.Show();
                    isFormDonHangKH = true;
                }
                else
                {
                    MessageBox.Show("Bạn đã mở bảng Đơn Hàng. Vui lòng kiểm tra lại !",
                        "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void FormDonHangKH_FormClosed(object sender, FormClosedEventArgs e)
        {
            isFormDonHangKH = false;
        }
        private void FrmDonHangNVNhanHang_FormClosed(object sender, FormClosedEventArgs e)
        {
            isFormDonHangNVNhanHang = false;
        }

        private void bunifuImageButton3_Click_3(object sender, EventArgs e)
        {
            if (!isFormDSDonHangNVNhanHang)
            {
                FrmDSDonHangNVNhanHang formDonHangNVNhanHang = new FrmDSDonHangNVNhanHang(TenDangNhap);
                formDonHangNVNhanHang.MdiParent = this;
                formDonHangNVNhanHang.FormClosed += FrmDSDonHangNVNhanHang_FormClosed;
                formDonHangNVNhanHang.Show();
                isFormDSDonHangNVNhanHang = true;
            }
            else
            {
                MessageBox.Show("Bạn đã mở bảng Danh Sách Đơn Hàng Nhân Viên Nhận Hàng. Vui lòng kiểm tra lại !",
                    "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FrmDSDonHangNVGiaoHang_FormClosed(object sender, FormClosedEventArgs e)
        {
            isFormDSDonHangNVGiaoHang = false;
        }

        private void bunifuImageButton8_Click(object sender, EventArgs e)
        {
            if (!isFormDonHangNVNhanHang)
            {
                FrmDonHangNVNhanHang formDonHangNVGiaoHang = new FrmDonHangNVNhanHang(TenDangNhap);
                formDonHangNVGiaoHang.MdiParent = this;
                formDonHangNVGiaoHang.FormClosed += FrmDonHangNVNhanHang_FormClosed;
                formDonHangNVGiaoHang.Show();
                isFormDonHangNVNhanHang = true;
            }
            else
            {
                MessageBox.Show("Bạn đã mở bảng Đơn Hàng Nhận. Vui lòng kiểm tra lại !",
                    "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        

        private void bunifuImageButton5_Click_1(object sender, EventArgs e)
        {
            if (!isFormDSDonHangNVGiaoHang)
            {
                FrmDSDonHangNVGiaoHang formDonHangNVNhanHang = new FrmDSDonHangNVGiaoHang(TenDangNhap);
                formDonHangNVNhanHang.MdiParent = this;
                formDonHangNVNhanHang.FormClosed += FrmDSDonHangNVGiaoHang_FormClosed;
                formDonHangNVNhanHang.Show();
                isFormDSDonHangNVGiaoHang = true;
            }
            else
            {
                MessageBox.Show("Bạn đã mở bảng Danh Sách Đơn Hàng Nhân Viên Giao Hàng. Vui lòng kiểm tra lại !",
                    "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void FrmDSDonHangNVNhanHang_FormClosed(object sender, FormClosedEventArgs e)
        {
            isFormDSDonHangNVNhanHang = false;
        }

        private void imbKhoTrungChuyen_Click(object sender, EventArgs e)
        {
            if (!isFormKhoTrungChuyen)
            {
                FrmKhoTrungChuyen formKhoTrungChuyen = new FrmKhoTrungChuyen(MaVaiTro);
                formKhoTrungChuyen.MdiParent = this;
                formKhoTrungChuyen.FormClosed += FrmKhoTrungChuyen_FormClosed;
                formKhoTrungChuyen.Show();
                isFormKhoTrungChuyen = true;
            }
            else
            {
                MessageBox.Show("Bạn đã mở bảng Kho Trung Chuyển. Vui lòng kiểm tra lại !",
                    "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FrmKhoTrungChuyen_FormClosed(object sender, FormClosedEventArgs e)
        {
            isFormKhoTrungChuyen = false;
        }

        private void imbLoaiHangHoa_Click(object sender, EventArgs e)
        {
            if (!isFormLoaiHangHoa)
            {
                FrmLoaiHangHoa formLoaiHangHoa = new FrmLoaiHangHoa(MaVaiTro);
                formLoaiHangHoa.MdiParent = this;
                formLoaiHangHoa.FormClosed += FrmLoaiHangHoa_FormClosed;
                formLoaiHangHoa.Show();
                isFormLoaiHangHoa = true;
            }
            else
            {
                MessageBox.Show("Bạn đã mở bảng Loại Hàng Hóa. Vui lòng kiểm tra lại !",
                    "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FrmLoaiHangHoa_FormClosed(object sender, FormClosedEventArgs e)
        {
            isFormLoaiHangHoa = false;
        }

        private void imbVaiTro_Click(object sender, EventArgs e)
        {
            if (!isFormVaiTro)
            {
                FrmVaiTro formVaiTro = new FrmVaiTro();
                formVaiTro.MdiParent = this;
                formVaiTro.FormClosed += FrmVaiTro_FormClosed;
                formVaiTro.Show();
                isFormVaiTro = true;
            }
            else
            {
                MessageBox.Show("Bạn đã mở bảng Danh Sách Đơn Hàng Đã Đến Kho. Vui lòng kiểm tra lại !",
                    "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FrmVaiTro_FormClosed(object sender, FormClosedEventArgs e)
        {
            isFormVaiTro = false;
        }

        private void imbTaiKhoan_Click(object sender, EventArgs e)
        {
            if (!isFormTaiKhoan)
            {
                FrmTaiKhoan formDSDonHangDaDenKho = new FrmTaiKhoan();
                formDSDonHangDaDenKho.MdiParent = this;
                formDSDonHangDaDenKho.FormClosed += FrmTaiKhoan_FormClosed;
                formDSDonHangDaDenKho.Show();
                isFormTaiKhoan = true;
            }
            else
            {
                MessageBox.Show("Bạn đã mở bảng Danh Sách Đơn Hàng Đã Đến Kho. Vui lòng kiểm tra lại !",
                    "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FrmTaiKhoan_FormClosed(object sender, FormClosedEventArgs e)
        {
            isFormTaiKhoan = false;
        }

        private void imbDonHangGiao_Click(object sender, EventArgs e)
        {
            if (!isFormDonHangNVGiaoHang)
            {
                FrmDonHangNVGiaoHang formDonHangNVGiaoHang = new FrmDonHangNVGiaoHang(TenDangNhap);
                formDonHangNVGiaoHang.MdiParent = this;
                formDonHangNVGiaoHang.FormClosed += FrmDonHangNVGiaoHang_FormClosed;
                formDonHangNVGiaoHang.Show();
                isFormDonHangNVGiaoHang = true;
            }
            else
            {
                MessageBox.Show("Bạn đã mở bảng Đơn Hàng Giao. Vui lòng kiểm tra lại !",
                    "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FrmDonHangNVGiaoHang_FormClosed(object sender, FormClosedEventArgs e)
        {
            isFormDonHangNVGiaoHang = false;
        }

        private void imbDSDonHangNVVanChuyen_Click(object sender, EventArgs e)
        {
            if (!isFrmDSDonHangNVVanChuyen)
            {
                FrmDSDonHangNVVanChuyen formDSDonHangNVVanChuyen = new FrmDSDonHangNVVanChuyen(TenDangNhap);
                formDSDonHangNVVanChuyen.MdiParent = this;
                formDSDonHangNVVanChuyen.FormClosed += FrmDSDonHangNVVanChuyen_FormClosed;
                formDSDonHangNVVanChuyen.Show();
                isFrmDSDonHangNVVanChuyen = true;
            }
            else
            {
                MessageBox.Show("Bạn đã mở bảng Danh Sách Đơn Hàng Đã Đến Kho. Vui lòng kiểm tra lại !",
                    "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FrmDSDonHangNVVanChuyen_FormClosed(object sender, FormClosedEventArgs e)
        {
            isFrmDSDonHangNVVanChuyen = false;
        }

        private void bunifuImageButton3_Click_4(object sender, EventArgs e)
        {
            if (!isFormTatCaDonHang)
            {
                FrmAllDonHang formDSDonHangDaDenKho = new FrmAllDonHang();
                formDSDonHangDaDenKho.MdiParent = this;
                formDSDonHangDaDenKho.FormClosed += FrmTatCaDonHang_FormClosed;
                formDSDonHangDaDenKho.Show();
                isFormTatCaDonHang = true;
            }
            else
            {
                MessageBox.Show("Bạn đã mở bảng Danh Sách Đơn Hàng Đã Đến Kho. Vui lòng kiểm tra lại !",
                    "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FrmTatCaDonHang_FormClosed(object sender, FormClosedEventArgs e)
        {
            isFormTatCaDonHang = false;
        }

        private void bunifuImageButton6_Click(object sender, EventArgs e)
        {
            if (!isFormDSDonHangDaDenKho)
            {
                FrmDSDonHangDaDenKho formDSDonHangDaDenKho = new FrmDSDonHangDaDenKho(TenDangNhap);
                formDSDonHangDaDenKho.MdiParent = this;
                formDSDonHangDaDenKho.FormClosed += FrmDSDonHangDaDenKho_FormClosed;
                formDSDonHangDaDenKho.Show();
                isFormDSDonHangDaDenKho = true;
            }
            else
            {
                MessageBox.Show("Bạn đã mở bảng Danh Sách Đơn Hàng Đã Đến Kho. Vui lòng kiểm tra lại !",
                    "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FrmDSDonHangDaDenKho_FormClosed(object sender, FormClosedEventArgs e)
        {
            isFormDSDonHangDaDenKho = false;
        }
    }
}
