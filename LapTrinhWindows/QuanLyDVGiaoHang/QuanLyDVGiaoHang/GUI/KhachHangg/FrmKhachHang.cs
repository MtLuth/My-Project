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
using System.Linq.Dynamic.Core;

namespace QuanLyDVGiaoHang.GUI.KhachHang
{
    public partial class FrmKhachHang : Form
    {
        public FrmKhachHang(string maVaiTro)
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            if (maVaiTro == "Khách hàng" || maVaiTro == "Nhân viên trung chuyển" || maVaiTro == "Nhân viên giao hàng")
            {
                btnThem.Visible = false;
                btnSua.Visible = false;
                btnXoa.Visible = false;
            }
        }
        void LoadData()
        {
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                List<KHACHHANG> KhachHangList = context.KHACHHANGs.ToList();
                dgvDSKhachHang.DataSource = KhachHangList.Select(kh => new
                {
                    kh.MaKhachHang,
                    kh.TenNguoiBan,
                    kh.TenCuaHang,
                    kh.SoDT,
                    kh.Email,
                    kh.DiaChi
                }).ToList();
            }
            // cap nhat len cbo
            cbTimKiem.Items.Clear();
            cbSapXep.Items.Clear();
            foreach (DataGridViewColumn column in dgvDSKhachHang.Columns)
            {
                cbTimKiem.Items.Add(column.Name);
                cbSapXep.Items.Add(column.Name);
            }
            cbTimKiem.Text = "Chọn thuộc tính cần tìm";
            cbSapXep.Text = "Chọn thuộc tính cần sắp xếp";
        }
        private void FrmKhachHang_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (txtTimKiem.Text == "" )
            {
                MessageBox.Show("Hãy nhập giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbTimKiem.SelectedItem == null || cbTimKiem.Text == "Chọn cột cần tìm kiếm")
            {
                MessageBox.Show("Vui lòng chọn một cột để tìm kiếm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string thuoctinh = cbTimKiem.SelectedItem.ToString();
            string giatri = txtTimKiem.Text;
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                var result = context.KHACHHANGs
                    .Where(string.Format("{0}.ToString().Contains(@0)", thuoctinh), giatri)
                    .ToList();
                dgvDSKhachHang.DataSource = result.Select(kh => new
                {
                    kh.MaKhachHang,
                    kh.TenNguoiBan,
                    kh.TenCuaHang,
                    kh.SoDT,
                    kh.Email,
                    kh.DiaChi
                }).ToList();
            }
        }

        private void btnSapXep_Click(object sender, EventArgs e)
        {
            if (cbSapXep.SelectedItem == null || cbSapXep.Text == "Chọn cột cần sắp xếp")
            {
                MessageBox.Show("Vui lòng chọn một cột để tìm kiếm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string thuoctinh = cbSapXep.SelectedItem.ToString();
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                IQueryable<KHACHHANG> query = context.KHACHHANGs;
                switch(thuoctinh)
                {
                    case "MaKhachHang":
                        query = query.OrderBy(kh => kh.MaKhachHang);
                        break;
                    case "TenNguoiBan":
                        query = query.OrderBy(kh => kh.TenNguoiBan);
                        break;
                    case "TenCuaHang":
                        query = query.OrderBy(kh => kh.TenCuaHang);
                        break;
                    case "SoDT":
                        query = query.OrderBy(kh => kh.SoDT);
                        break;
                    case "Email":
                        query = query.OrderBy(kh => kh.Email);
                        break;
                    case "DiaChi":
                        query = query.OrderBy(kh => kh.DiaChi);
                        break;
                }
                dgvDSKhachHang.DataSource = query.Select(kh=>new
                {
                    kh.MaKhachHang,
                    kh.TenNguoiBan,
                    kh.TenCuaHang,
                    kh.SoDT,
                    kh.Email,
                    kh.DiaChi
                }).ToList();
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo", 
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                this.Close();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            FrmThemKhachHang frmThemKhachHang = new FrmThemKhachHang();
            frmThemKhachHang.ShowDialog();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvDSKhachHang.SelectedCells.Count > 0)
            {
                int row = dgvDSKhachHang.CurrentCell.RowIndex;
                string id = dgvDSKhachHang.Rows[row].Cells[0].Value.ToString();
                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    var temp = context.KHACHHANGs.Find(int.Parse(id));
                    FrmSuaKhachHang frm = new FrmSuaKhachHang();
                    frm.KhachHangCanSua = temp;
                    frm.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để chỉnh sửa thông tin", "Thông Báo",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
        }
        public void DataSave()
        {
            LoadData();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDSKhachHang.SelectedRows.Count > 0)
            {
                try
                {
                    int r = dgvDSKhachHang.CurrentCell.RowIndex;
                    string id = dgvDSKhachHang.Rows[r].Cells[0].Value.ToString();
                    if (id != null)
                    {
                        using (var context = new QuanLyDVGiaoHangEntities())
                        {
                            var khcanxoa = context.KHACHHANGs.Find(int.Parse(id));
                            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa khách hàng này ra khỏi danh sách không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (result == DialogResult.OK)
                            {
                                context.KHACHHANGs.Remove(khcanxoa);
                                context.SaveChanges();
                                LoadData();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi trong quá trình xóa thông tin khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
