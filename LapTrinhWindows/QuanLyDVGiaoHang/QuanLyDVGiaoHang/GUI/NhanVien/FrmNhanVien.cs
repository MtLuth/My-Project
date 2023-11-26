using QuanLyDVGiaoHang.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq.Dynamic.Core;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyDVGiaoHang.GUI.NhanVien;

namespace QuanLyDVGiaoHang.GUI
{
    public partial class FrmNhanVien : Form
    {
        public FrmNhanVien()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
        }

        private void bunifuTextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void loadData()
        {
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                //cap nhat du lieu vao datagridview
                var vaiTroList = context.VAITROes.ToList();
                MaVaiTro.DataSource = vaiTroList;
                MaVaiTro.DisplayMember = "TenVaiTro";
                MaVaiTro.ValueMember = "MaVaiTro";

                var khoList = context.KHOTRUNGCHUYENs.ToList();
                MaKho.DataSource = khoList;
                MaKho.DisplayMember = "TenKho";
                MaKho.ValueMember = "MaKho";

                List<NHANVIEN> nhanVienList = context.NHANVIENs.ToList();

                dgvDSNhanVien.DataSource = nhanVienList.Select(nv => new
                {
                    nv.MaNV,
                    nv.MaVaiTro,
                    nv.MaKho,
                    nv.HoTen,
                    nv.DiaChi,
                    nv.NgaySinh,
                    nv.SoDT
                }).ToList();
                dgvDSNhanVien.AutoResizeColumns();
                cbTimKiem.Items.Clear();
                cbSapXep.Items.Clear();
                //cap nhat du lieu vao combobox tim kiem, sap xep
                foreach (DataGridViewColumn column in dgvDSNhanVien.Columns)
                {
                    string columnName = column.DataPropertyName;
                    cbTimKiem.Items.Add(columnName);
                    cbSapXep.Items.Add(columnName);
                }
                cbTimKiem.Text = "Chọn cột cần tìm kiếm";
                cbSapXep.Text = "Chọn cột cần sắp xếp";
            }

        }

        private void FrmNhanVien_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTimKiem.Text == "")
                {
                    MessageBox.Show("Vui lòng nhập giá trị tìm kiếm!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (cbTimKiem.SelectedItem == null || cbTimKiem.Text == "Chọn cột cần tìm kiếm")
                {
                    MessageBox.Show("Vui lòng chọn một cột để tìm kiếm!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string giaTriTimKiem = txtTimKiem.Text;
                string cotTimKiem = cbTimKiem.SelectedItem.ToString();

                using (var dbContext = new QuanLyDVGiaoHangEntities())
                {
                    var ketQuaTimKiem = dbContext.NHANVIENs
                                        .Where(string.Format("{0}.ToString().Contains(@0)", cotTimKiem), giaTriTimKiem)
                                        .ToList();
                    dgvDSNhanVien.DataSource = ketQuaTimKiem.Select(nv => new
                    {
                        nv.MaNV,
                        nv.MaVaiTro,
                        nv.MaKho,
                        nv.HoTen,
                        nv.DiaChi,
                        nv.NgaySinh,
                        nv.SoDT
                    }).ToList();
                    dgvDSNhanVien.AutoResizeColumns();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnSapXep_Click(object sender, EventArgs e)
        {
            if (cbSapXep.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một cột để sắp xếp!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string cotSapXep = cbSapXep.SelectedItem.ToString();
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                IQueryable<NHANVIEN> query = context.NHANVIENs;
                switch (cotSapXep)
                {
                    case "MaNV":
                        query = query.OrderBy(nv => nv.MaNV);
                        break;
                    case "MaVaiTro":
                        query =query.OrderBy(nv => nv.MaVaiTro);
                        break;
                    case "MaKho":
                        query =query.OrderBy(nv => nv.MaKho);
                        break;
                    case "HoTen":
                        query =query.OrderBy(nv => nv.HoTen);
                        break;
                    case "NgaySinh":
                        query = query.OrderBy(nv => nv.NgaySinh);
                        break;
                    case "DiaChi":
                        query = query.OrderBy(nv => nv.DiaChi);
                        break;
                    case "SoDT":
                        query = query.OrderBy(nv => nv.SoDT);
                        break;
                }
                List<NHANVIEN> ketQuaSapXep = query.ToList();
                dgvDSNhanVien.DataSource = ketQuaSapXep.Select(nv => new
                {
                    nv.MaNV,
                    nv.MaVaiTro,
                    nv.MaKho,
                    nv.HoTen,
                    nv.DiaChi,
                    nv.NgaySinh,
                    nv.SoDT
                }).ToList();
                dgvDSNhanVien.AutoResizeColumns();
            }
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn Có Muốn Thoát Chương Trình ?", "Thông Báo",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDSNhanVien.SelectedRows.Count > 0)
                {
                    int row = dgvDSNhanVien.CurrentCell.RowIndex;
                    string ID = dgvDSNhanVien.Rows[row].Cells[0].Value.ToString();

                    using (var context = new QuanLyDVGiaoHangEntities())
                    {
                        var toDelete = context.NHANVIENs.Find(int.Parse(ID));
                        context.NHANVIENs.Remove(toDelete);
                        context.SaveChanges();
                        loadData();
                        MessageBox.Show("Xóa thông tin nhân viên thành công", "Thông Báo",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một dòng để xóa thông tin", "Thông Báo",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi trong quá trình xóa thông tin nhân viên: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvDSNhanVien.SelectedRows.Count > 0)
            {
                int row = dgvDSNhanVien.CurrentCell.RowIndex;
                string ID = dgvDSNhanVien.Rows[row].Cells[0].Value.ToString();

                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    var temp = context.NHANVIENs.Find(int.Parse(ID));
                    FrmSuaNhanVien formSuaNhanVien = new FrmSuaNhanVien();
                    formSuaNhanVien.nHANVIEN = temp;
                    formSuaNhanVien.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để chỉnh sửa thông tin", "Thông Báo", 
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            FrmThemNhanVien formThemNhanVien = new FrmThemNhanVien();
            formThemNhanVien.ShowDialog();
        }
    }
}
