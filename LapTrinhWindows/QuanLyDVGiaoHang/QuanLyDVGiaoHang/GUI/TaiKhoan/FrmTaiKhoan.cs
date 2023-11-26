using System;
using QuanLyDVGiaoHang.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NPOI.SS.Formula.Atp;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using QuanLyDVGiaoHang.Resources;
using System.IO;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic;

namespace QuanLyDVGiaoHang.GUI.TaiKhoan
{
    public partial class FrmTaiKhoan : Form
    {
        //Biến kiểm tra đăng ký loại nhân viên nào
        int LoaiNV = -1;
        DataTable dtTinhHuyenXa;
        List<TinhHuyenXa> TinhHuyenXaList;
        public FrmTaiKhoan()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
        }
        void LoadTinhHuyenXa()
        {
            string filePath = "TinhHuyenXa.xlsx";

            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(file);
                ISheet sheet = workbook.GetSheetAt(0); // Lấy sheet đầu tiên

                dtTinhHuyenXa = new DataTable();
                IRow headerRow = sheet.GetRow(0);

                // Tạo các cột trong DataTable từ dữ liệu trong hàng đầu tiên
                foreach (ICell cell in headerRow.Cells)
                {
                    dtTinhHuyenXa.Columns.Add(cell.StringCellValue);
                }

                // Đọc dữ liệu từ các hàng còn lại trong sheet và đưa vào DataTable
                for (int rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
                {
                    IRow dataRow = sheet.GetRow(rowIndex);
                    DataRow newRow = dtTinhHuyenXa.NewRow();

                    for (int cellIndex = 0; cellIndex < headerRow.Cells.Count; cellIndex++)
                    {
                        ICell cell = dataRow.GetCell(cellIndex);
                        newRow[cellIndex] = cell?.ToString();
                    }
                    dtTinhHuyenXa.Rows.Add(newRow);
                }
            }
            TinhHuyenXaList = dtTinhHuyenXa.AsEnumerable()
                    .Select(row => new TinhHuyenXa
                    {
                        Tinh = row["Tinh"].ToString(),
                        Huyen = row["Huyen"].ToString(),
                        Xa = row["Xa"].ToString()
                    })
                    .ToList();
        }
        void LoadData()
        {
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                //Tab Nhân viên trung chuyển
                List<TK_NVTRUNGCHUYEN> TK_NVTrungChuyenList = context.TK_NVTRUNGCHUYEN.ToList();
                dgvTKNhanVienTrungChuyen.DataSource = TK_NVTrungChuyenList.Select(tk => new
                {
                    tk.MaNVTrungChuyen,
                    tk.TenDangNhap,
                    tk.MatKhau,
                    tk.TuyenTrungChuyen
                }).ToList();
                //Tab Nhân Viên Giao Hàng
                List<TK_NVGIAOHANG> TK_NVGiaoHangList = context.TK_NVGIAOHANG.ToList();
                dgvTKNhanVienGiaoHang.DataSource = TK_NVGiaoHangList.Select(tk => new
                {
                    tk.MaNVGiaoHang,
                    tk.TenDangNhap,
                    tk.MatKhau,
                    tk.KhuVucGiaoHang
                }).ToList();
                //Tab Nhân Viên Quản Lý Kho
                List<TK_QUANLY> TK_QuanLyList = context.TK_QUANLY.ToList();
                List<NHANVIEN> NhanVienList = context.NHANVIENs.ToList();
                dgvTKQuanLy.DataSource = TK_QuanLyList.SelectMany(tk => NhanVienList.Where(nv => nv.MaNV == tk.MaNVQuanLy),
                (tk, nv) => new
                {
                    tk.MaNVQuanLy,
                    tk.TenDangNhap,
                    tk.MatKhau,
                    nv.MaKho
                }).ToList();
                //Tab Khách Hàng
                List<TK_KHACHHANG> TK_KhachHangList = context.TK_KHACHHANG.ToList();
                dgvTKKhachHang.DataSource = TK_KhachHangList.Select(tk => new
                {
                    tk.MaKhachHang,
                    tk.TenDangNhap,
                    tk.MatKhau,
                    tk.SoDu
                }).ToList();
                // 
                LoadTinhHuyenXa();
                // Ẩn mục phụ
                labelNhiemVu.Visible = false;
                txtNhiemVu.Visible = false;
                cbTinh.Visible = false;
                cbHuyen.Visible = false;
                cbXa.Visible = false;
                //Làm trống các đối tượng
                txtMaNV.Text = "";
                txtHoVaTen.Text = "";
                txtTenDangNhap.Text = "";
                txtMatKhau.Text = "";
                txtNhapLaiMatKhau.Text = "";
                txtNhiemVu.Text = "";
                //Load các mục tìm kiếm, sắp xếp
                //Trung Chuyển
                cbTimKiem1.Items.Clear();
                cbSapXep1.Items.Clear();
                cbTimKiem2.Items.Clear();
                cbSapXep2.Items.Clear();
                cbTimKiem3.Items.Clear();
                cbSapXep3.Items.Clear();
                cbTimKiem4.Items.Clear();
                cbSapXep4.Items.Clear();
                foreach (DataGridViewColumn column in dgvTKNhanVienTrungChuyen.Columns)
                {
                    cbTimKiem1.Items.Add(column.DataPropertyName);
                    cbSapXep1.Items.Add(column.DataPropertyName);
                }
                cbTimKiem1.Text = "Chọn thuộc tính cần tìm";
                cbSapXep1.Text = "Chọn thuộc tính cần sắp xếp";
                //Giao Hàng
                foreach (DataGridViewColumn column in dgvTKNhanVienGiaoHang.Columns)
                {
                    cbTimKiem2.Items.Add(column.DataPropertyName);
                    cbSapXep2.Items.Add(column.DataPropertyName);
                }
                cbTimKiem2.Text = "Chọn thuộc tính cần tìm";
                cbSapXep2.Text = "Chọn thuộc tính cần sắp xếp";
                //Quản lý
                foreach (DataGridViewColumn column in dgvTKQuanLy.Columns)
                {
                    cbTimKiem3.Items.Add(column.DataPropertyName);
                    cbSapXep3.Items.Add(column.DataPropertyName);
                }
                cbTimKiem3.Text = "Chọn thuộc tính cần tìm";
                cbSapXep3.Text = "Chọn thuộc tính cần sắp xếp";
                // Khách hàng
                foreach (DataGridViewColumn column in dgvTKKhachHang.Columns)
                {
                    cbTimKiem4.Items.Add(column.DataPropertyName);
                    cbSapXep4.Items.Add(column.DataPropertyName);
                }
                cbTimKiem4.Text = "Chọn thuộc tính cần tìm";
                cbSapXep4.Text = "Chọn thuộc tính cần sắp xếp";
            }
        }
        private void FrmTaiKhoan_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);
            if (r == DialogResult.OK)
                this.Close();
        }

        private void labelCheck_Click(object sender, EventArgs e)
        {
            txtNhiemVu.Text = "";
            if (txtMaNV.Text == "")
                MessageBox.Show("Bạn chưa nhập mã nhân viên!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Question);
            else
            {
                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    string MaNV = txtMaNV.Text;
                    List<NHANVIEN> NhanVienList = context.NHANVIENs.ToList();
                    bool isExist = NhanVienList.Any(nv => nv.MaNV.ToString() == MaNV);
                    if (isExist)
                    {
                        var NV = NhanVienList.Where(nv => nv.MaNV.ToString() == MaNV)
                            .Select(nv => new
                            {
                                nv.HoTen,
                                nv.MaVaiTro,
                                nv.MaKho
                            }).ToList();
                        string TenNV = NV[0].HoTen;
                        txtHoVaTen.Text = TenNV;
                        string MaVaiTro = NV[0].MaVaiTro;
                        string MaKho = NV[0].MaKho;
                        if (MaVaiTro == "VT_QL1")
                        {
                            labelNhiemVu.Text = "Kho trung chuyển:";
                            txtNhiemVu.Text = MaKho;
                            labelNhiemVu.Visible = true;
                            txtNhiemVu.Visible = true;
                            cbTinh.Visible = false;
                            cbHuyen.Visible = false;
                            cbXa.Visible = false;
                            LoaiNV = 0;
                        }
                        else if (MaVaiTro == "VT_TC1")
                        {
                            labelNhiemVu.Text = "Tuyến trung chuyển:";
                            txtNhiemVu.PlaceholderText = "KHOSG1 -> KHOSG2";
                            labelNhiemVu.Visible = true;
                            txtNhiemVu.Visible = true;
                            cbTinh.Visible = false;
                            cbHuyen.Visible = false;
                            cbXa.Visible = false;
                            LoaiNV = 1;
                        }
                        else
                        {
                            labelNhiemVu.Text = "Khu vực vận chuyển:";
                            labelNhiemVu.Visible = true;
                            cbTinh.Visible = true;
                            cbHuyen.Visible = true;
                            cbXa.Visible = true;
                            txtNhiemVu.Visible = false;
                            cbTinh.DataSource = TinhHuyenXaList
                            .OrderBy(t => t.Tinh)
                            .Select(t => t.Tinh).Distinct().ToList();
                            cbTinh.SelectedItem = null;
                            cbTinh.Text = "Thành Phố / Tỉnh";
                            LoaiNV = 2;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mã nhân viên này không tồn tại!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            if (txtMaNV.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập mã nhân viên!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtNhiemVu.Text == "" && LoaiNV != 2)
            {
                string thongbao = null;
                if (LoaiNV == 1)
                {
                    thongbao = "Bạn chưa nhập tuyến trung chuyển!";
                    MessageBox.Show(thongbao, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else if (LoaiNV == 2)
            {
                if (cbTinh.SelectedValue == null || cbHuyen.SelectedValue == null || cbXa.SelectedValue == null)
                {
                    MessageBox.Show("Bạn hãy hoàn thành khu vực giao hàng!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            if (txtTenDangNhap.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập tên đăng nhập!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtMatKhau.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập mật khẩu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtNhapLaiMatKhau.Text == "")
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtMatKhau.Text != txtNhapLaiMatKhau.Text)
            {
                MessageBox.Show("Mật khẩu không trùng khớp!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Text = "";
                txtNhapLaiMatKhau.Text = "";
                return;
            }
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                // Tài khoản được tạo cho nhân viên quản lý
                string MaNV = txtMaNV.Text;
                string TenDN = txtTenDangNhap.Text;
                string MatKhau = txtMatKhau.Text;
                if (LoaiNV == 0)
                {
                    bool kt = context.TK_QUANLY.Any(tk => tk.MaNVQuanLy.ToString() == MaNV);
                    if (kt)
                    {
                        MessageBox.Show("Nhân viên này đã được tạo tài khoản!", "Thông Báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    try
                    {
                        TK_QUANLY TaiKhoanMoi = new TK_QUANLY
                        {
                            MaNVQuanLy = int.Parse(MaNV),
                            TenDangNhap = TenDN,
                            MatKhau = MatKhau
                        };
                        context.TK_QUANLY.Add(TaiKhoanMoi);
                        context.SaveChanges();
                        MessageBox.Show("Tài khoản " + TenDN + " được tạo thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FrmTaiKhoan frm = Application.OpenForms.OfType<FrmTaiKhoan>().FirstOrDefault();
                        frm?.LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể tạo vì: " + ex.Message, "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                // Tạo tài khoản cho nhân viên trung chuyển
                else if (LoaiNV == 1)
                {
                    bool kt = context.TK_NVTRUNGCHUYEN.Any(tk => tk.MaNVTrungChuyen.ToString() == MaNV);
                    if (kt)
                    {
                        MessageBox.Show("Nhân viên này đã được tạo tài khoản!", "Thông Báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    try
                    {
                        string TuyenTrungChuyen = txtNhiemVu.Text;
                        TK_NVTRUNGCHUYEN TaiKhoanMoi = new TK_NVTRUNGCHUYEN
                        {
                            MaNVTrungChuyen = int.Parse(MaNV),
                            TenDangNhap = TenDN,
                            MatKhau = MatKhau,
                            TuyenTrungChuyen = TuyenTrungChuyen
                        };
                        context.TK_NVTRUNGCHUYEN.Add(TaiKhoanMoi);
                        context.SaveChanges();
                        MessageBox.Show("Tài khoản " + TenDN + " được tạo thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FrmTaiKhoan frm = Application.OpenForms.OfType<FrmTaiKhoan>().FirstOrDefault();
                        frm?.LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể tạo vì: " + ex.Message, "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (LoaiNV == 2)
                {
                    bool kt = context.TK_NVGIAOHANG.Any(tk => tk.MaNVGiaoHang.ToString() == MaNV);
                    if (kt)
                    {
                        MessageBox.Show("Nhân viên này đã được tạo tài khoản!", "Thông Báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    try
                    {
                        string Tinh = cbTinh.SelectedValue.ToString();
                        string Huyen = cbHuyen.SelectedValue.ToString();
                        string Xa = cbXa.SelectedValue.ToString();
                        string KVGiaoHang = Xa + ", " + Huyen + ", " + Tinh;
                        TK_NVGIAOHANG TaiKhoanMoi = new TK_NVGIAOHANG
                        {
                            MaNVGiaoHang = int.Parse(MaNV),
                            TenDangNhap = TenDN,
                            MatKhau = MatKhau,
                            KhuVucGiaoHang = KVGiaoHang
                        };
                        context.TK_NVGIAOHANG.Add(TaiKhoanMoi);
                        context.SaveChanges();
                        MessageBox.Show("Tài khoản " + TenDN + " được tạo thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FrmTaiKhoan frm = Application.OpenForms.OfType<FrmTaiKhoan>().FirstOrDefault();
                        frm?.LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể tạo vì: " + ex.Message, "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
        }

        private void cbTinh_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbTinh.SelectedItem != null)
            {
                cbHuyen.DataSource = TinhHuyenXaList
                    .Where(t => t.Tinh == cbTinh.SelectedValue.ToString())
                    .OrderBy(t => t.Huyen)
                    .Select(t => t.Huyen).Distinct().ToList();
            }
            cbHuyen.SelectedItem = null;
            cbHuyen.Text = "Huyện / Quận";
        }

        private void cbHuyen_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbHuyen.SelectedItem != null)
            {
                cbXa.DataSource = TinhHuyenXaList
                .Where(t => t.Huyen == cbHuyen.SelectedValue.ToString())
                .OrderBy(t => t.Xa)
                .Select(t => t.Xa)
                .Distinct()
                .ToList();
            }
            cbXa.SelectedItem = null;
            cbXa.Text = "Phường / Xã";
        }

        private void btnXoa1_Click(object sender, EventArgs e)
        {
            if (dgvTKNhanVienTrungChuyen.SelectedRows.Count > 0)
            {
                int r = dgvTKNhanVienTrungChuyen.CurrentCell.RowIndex;
                string TenDN = dgvTKNhanVienTrungChuyen.Rows[r].Cells[1].Value.ToString();
                txtTimKiem1.Text = TenDN;
                if (TenDN != null)
                {
                    using (var context = new QuanLyDVGiaoHangEntities())
                    {
                        var tkcanxoa = context.TK_NVTRUNGCHUYEN.Find(TenDN);
                        DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa tài khoản này ra khỏi danh sách không?"
                            , "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (result == DialogResult.OK && tkcanxoa != null)
                        {
                            context.TK_NVTRUNGCHUYEN.Remove(tkcanxoa);
                            context.SaveChanges();
                            FrmTaiKhoan frm = Application.OpenForms.OfType<FrmTaiKhoan>().FirstOrDefault();
                            frm?.LoadData();
                        }
                    }
                }
            }
        }

        private void btnXoa2_Click(object sender, EventArgs e)
        {
            if (dgvTKNhanVienGiaoHang.SelectedRows.Count > 0)
            {
                int r = dgvTKNhanVienGiaoHang.CurrentCell.RowIndex;
                string TenDN = dgvTKNhanVienGiaoHang.Rows[r].Cells[1].Value.ToString();
                if (TenDN != null)
                {
                    using (var context = new QuanLyDVGiaoHangEntities())
                    {
                        var tkcanxoa = context.TK_NVGIAOHANG.Find(TenDN);
                        DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa tài khoản này ra khỏi danh sách không?"
                            , "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (result == DialogResult.OK)
                        {
                            context.TK_NVGIAOHANG.Remove(tkcanxoa);
                            context.SaveChanges();
                            FrmTaiKhoan frm = Application.OpenForms.OfType<FrmTaiKhoan>().FirstOrDefault();
                            frm?.LoadData();
                        }
                    }
                }
            }
        }

        private void btnXoa3_Click(object sender, EventArgs e)
        {
            if (dgvTKQuanLy.SelectedRows.Count > 0)
            {
                int r = dgvTKQuanLy.CurrentCell.RowIndex;
                string TenDN = dgvTKQuanLy.Rows[r].Cells[1].Value.ToString();
                if (TenDN != null)
                {
                    using (var context = new QuanLyDVGiaoHangEntities())
                    {
                        var tkcanxoa = context.TK_QUANLY.Find(TenDN);
                        DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa tài khoản này ra khỏi danh sách không?"
                            , "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (result == DialogResult.OK)
                        {
                            context.TK_QUANLY.Remove(tkcanxoa);
                            context.SaveChanges();
                            FrmTaiKhoan frm = Application.OpenForms.OfType<FrmTaiKhoan>().FirstOrDefault();
                            frm?.LoadData();
                        }
                    }
                }
            }
        }

        private void btnXoa4_Click(object sender, EventArgs e)
        {
            if (dgvTKQuanLy.SelectedRows.Count > 0)
            {
                int r = dgvTKKhachHang.CurrentCell.RowIndex;
                string TenDN = dgvTKKhachHang.Rows[r].Cells[1].Value.ToString();
                if (TenDN != null)
                {
                    using (var context = new QuanLyDVGiaoHangEntities())
                    {
                        var tkcanxoa = context.TK_KHACHHANG.Find(TenDN);
                        DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa tài khoản này ra khỏi danh sách không?"
                            , "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (result == DialogResult.OK)
                        {
                            context.TK_KHACHHANG.Remove(tkcanxoa);
                            context.SaveChanges();
                            FrmTaiKhoan frm = Application.OpenForms.OfType<FrmTaiKhoan>().FirstOrDefault();
                            frm?.LoadData();
                        }
                    }
                }
            }
        }

        private void btnTimKiem1_Click(object sender, EventArgs e)
        {
            if (txtTimKiem1.Text == "")
            {
                MessageBox.Show("Hãy nhập giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbTimKiem1.SelectedItem == null || cbTimKiem1.Text == "Chọn cột cần tìm kiếm")
            {
                MessageBox.Show("Vui lòng chọn một cột để tìm kiếm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string thuoctinh = cbTimKiem1.SelectedItem.ToString();
            string giatri = txtTimKiem1.Text;
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                var result = context.TK_NVTRUNGCHUYEN
                    .Where(string.Format("{0}.ToString().Contains(@0)", thuoctinh), giatri)
                    .ToList();
                dgvTKNhanVienTrungChuyen.DataSource = result.Select(tk => new
                {
                    tk.MaNVTrungChuyen,
                    tk.TenDangNhap,
                    tk.MatKhau,
                    tk.TuyenTrungChuyen
                }).ToList();
            }
        }

        private void btnTaiLai1_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnTimKiem2_Click(object sender, EventArgs e)
        {
            if (txtTimKiem2.Text == "")
            {
                MessageBox.Show("Hãy nhập giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbTimKiem2.SelectedItem == null || cbTimKiem2.Text == "Chọn cột cần tìm kiếm")
            {
                MessageBox.Show("Vui lòng chọn một cột để tìm kiếm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string thuoctinh = cbTimKiem2.SelectedItem.ToString();
            string giatri = txtTimKiem2.Text;
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                var result = context.TK_NVGIAOHANG
                    .Where(string.Format("{0}.ToString().Contains(@0)", thuoctinh), giatri)
                    .ToList();
                dgvTKNhanVienGiaoHang.DataSource = result.Select(tk => new
                {
                    tk.MaNVGiaoHang,
                    tk.TenDangNhap,
                    tk.MatKhau,
                    tk.KhuVucGiaoHang
                }).ToList();
            }
        }

        private void btnTimKiem3_Click(object sender, EventArgs e)
        {
            if (txtTimKiem3.Text == "")
            {
                MessageBox.Show("Hãy nhập giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbTimKiem3.SelectedItem == null || cbTimKiem3.Text == "Chọn cột cần tìm kiếm")
            {
                MessageBox.Show("Vui lòng chọn một cột để tìm kiếm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string thuoctinh = cbTimKiem3.SelectedItem.ToString();
            string giatri = txtTimKiem3.Text;
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                var result = context.TK_QUANLY
                    .Where(string.Format("{0}.ToString().Contains(@0)", thuoctinh), giatri)
                    .ToList();
                dgvTKQuanLy.DataSource = result.SelectMany(tk => context.NHANVIENs.ToList()
                .Where(nv => nv.MaNV == tk.MaNVQuanLy),
                (tk, nv) => new
                {
                    tk.MaNVQuanLy,
                    tk.TenDangNhap,
                    tk.MatKhau,
                    nv.MaKho
                }).ToList();
            }
        }

        private void btnTimKiem4_Click(object sender, EventArgs e)
        {
            if (txtTimKiem4.Text == "")
            {
                MessageBox.Show("Hãy nhập giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbTimKiem4.SelectedItem == null || cbTimKiem4.Text == "Chọn cột cần tìm kiếm")
            {
                MessageBox.Show("Vui lòng chọn một cột để tìm kiếm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string thuoctinh = cbTimKiem4.SelectedItem.ToString();
            string giatri = txtTimKiem4.Text;
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                var result = context.TK_KHACHHANG
                    .Where(string.Format("{0}.ToString().Contains(@0)", thuoctinh), giatri)
                    .ToList();
                dgvTKKhachHang.DataSource = result.Select(tk => new
                {
                    tk.MaKhachHang,
                    tk.TenDangNhap,
                    tk.MatKhau,
                    tk.SoDu
                }).ToList();
            }
        }

        private void btnSapXep1_Click(object sender, EventArgs e)
        {
            if (cbSapXep1.SelectedItem == null || cbSapXep1.Text == "Chọn cột cần sắp xếp")
            {
                MessageBox.Show("Vui lòng chọn một cột để tìm kiếm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string thuoctinh = cbSapXep1.SelectedItem.ToString();
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                IQueryable<TK_NVTRUNGCHUYEN> query = context.TK_NVTRUNGCHUYEN;
                switch (thuoctinh)
                {
                    case "MaNVTrungChuyen":
                        query = query.OrderBy(tk => tk.MaNVTrungChuyen);
                        break;
                    case "TenDangNhap":
                        query = query.OrderBy(tk => tk.TenDangNhap);
                        break;
                    case "MatKhau":
                        query = query.OrderBy(tk => tk.MatKhau);
                        break;
                    case "TuyenTrungChuyen":
                        query = query.OrderBy(tk => tk.TuyenTrungChuyen);
                        break;
                }
                dgvTKNhanVienTrungChuyen.DataSource = query.Select(tk => new
                {
                    tk.MaNVTrungChuyen,
                    tk.TenDangNhap,
                    tk.MatKhau,
                    tk.TuyenTrungChuyen
                }).ToList();
            }
        }

        private void btnSapXep2_Click(object sender, EventArgs e)
        {
            if (cbSapXep2.SelectedItem == null || cbSapXep2.Text == "Chọn cột cần sắp xếp")
            {
                MessageBox.Show("Vui lòng chọn một cột để tìm kiếm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string thuoctinh = cbSapXep2.SelectedItem.ToString();
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                IQueryable<TK_NVGIAOHANG> query = context.TK_NVGIAOHANG;
                switch (thuoctinh)
                {
                    case "MaNVGiaoHang":
                        query = query.OrderBy(tk => tk.MaNVGiaoHang);
                        break;
                    case "TenDangNhap":
                        query = query.OrderBy(tk => tk.TenDangNhap);
                        break;
                    case "MatKhau":
                        query = query.OrderBy(tk => tk.MatKhau);
                        break;
                    case "KhuVucGiaoHang":
                        query = query.OrderBy(tk => tk.KhuVucGiaoHang);
                        break;
                }
                dgvTKNhanVienGiaoHang.DataSource = query.Select(tk => new
                {
                    tk.MaNVGiaoHang,
                    tk.TenDangNhap,
                    tk.MatKhau,
                    tk.KhuVucGiaoHang
                }).ToList();
            }
        }

        private void btnSapXep3_Click(object sender, EventArgs e)
        {
            if (cbSapXep3.SelectedItem == null || cbSapXep3.Text == "Chọn cột cần sắp xếp")
            {
                MessageBox.Show("Vui lòng chọn một cột để tìm kiếm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string thuoctinh = cbSapXep3.SelectedItem.ToString();
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                switch (thuoctinh)
                {
                    case "MaNVQuanLy":
                        var r1 = context.TK_QUANLY.SelectMany(tk => context.NHANVIENs.ToList().Where(nv => tk.MaNVQuanLy == nv.MaNV),
                            (tk, nv) => new
                            {
                                tk.MaNVQuanLy,
                                tk.TenDangNhap,
                                tk.MatKhau,
                                nv.MaKho
                            }).OrderBy(n => n.MaNVQuanLy).ToList();
                        dgvTKQuanLy.DataSource = r1;
                        break;
                    case "TenDangNhap":
                        var r2 = context.TK_QUANLY.SelectMany(tk => context.NHANVIENs.ToList().Where(nv => tk.MaNVQuanLy == nv.MaNV),
                            (tk, nv) => new
                            {
                                tk.MaNVQuanLy,
                                tk.TenDangNhap,
                                tk.MatKhau,
                                nv.MaKho
                            }).OrderBy(n => n.TenDangNhap).ToList();
                        dgvTKQuanLy.DataSource = r2;
                        break;
                    case "MatKhau":
                        var r3 = context.TK_QUANLY.SelectMany(tk => context.NHANVIENs.ToList().Where(nv => tk.MaNVQuanLy == nv.MaNV),
                            (tk, nv) => new
                            {
                                tk.MaNVQuanLy,
                                tk.TenDangNhap,
                                tk.MatKhau,
                                nv.MaKho
                            }).OrderBy(n => n.MatKhau).ToList();
                        dgvTKQuanLy.DataSource = r3;
                        break;
                    case "MaKho":
                        var r4 = context.TK_QUANLY.SelectMany(tk => context.NHANVIENs.ToList().Where(nv => tk.MaNVQuanLy == nv.MaNV),
                            (tk, nv) => new
                            {
                                tk.MaNVQuanLy,
                                tk.TenDangNhap,
                                tk.MatKhau,
                                nv.MaKho
                            }).OrderBy(n => n.MaKho).ToList();
                        dgvTKQuanLy.DataSource = r4;
                        break;
                }
            }
        }

        private void btnSapXep4_Click(object sender, EventArgs e)
        {
            if (cbSapXep4.SelectedItem == null || cbSapXep4.Text == "Chọn cột cần sắp xếp")
            {
                MessageBox.Show("Vui lòng chọn một cột để tìm kiếm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string thuoctinh = cbSapXep4.SelectedItem.ToString();
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                IQueryable<TK_KHACHHANG> query = context.TK_KHACHHANG;
                switch (thuoctinh)
                {
                    case "MaKhachHang":
                        query = query.OrderBy(tk => tk.MaKhachHang);
                        break;
                    case "TenDangNhap":
                        query = query.OrderBy(tk => tk.TenDangNhap);
                        break;
                    case "MatKhau":
                        query = query.OrderBy(tk => tk.MatKhau);
                        break;
                    case "SoDu":
                        query = query.OrderBy(tk => tk.SoDu);
                        break;
                }
                dgvTKKhachHang.DataSource = query.Select(tk => new
                {
                    tk.MaKhachHang,
                    tk.TenDangNhap,
                    tk.MatKhau,
                    tk.SoDu
                }).ToList();
            }
        }
    }
}
