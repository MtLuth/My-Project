using QuanLyDVGiaoHang.GUI.NhanVien;
using QuanLyDVGiaoHang.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyDVGiaoHang.GUI.BangGia
{
    public partial class FrmBangGia : Form
    {
        public FrmBangGia(string maVaiTro)
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
        private void loadData()
        {
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                List<BANGGIA> BangGiaList = context.BANGGIAs.ToList();
                dgvDSBangGia.DataSource = BangGiaList.Select(bg => new
                {
                    bg.KhoiLuong,
                    bg.GiaNoiTinh,
                    bg.GiaNoiVung,
                    bg.GiaLienVung,
                    bg.GiaCachVung
                }).ToList();
                dgvDSBangGia.AutoResizeColumns();

                var columnNames = dgvDSBangGia.Columns.Cast<DataGridViewColumn>()
                    .Select(column => column.DataPropertyName)
                    .Distinct()
                    .ToList();

                cbTimKiem.Items.Clear();
                cbSapXep.Items.Clear();

                foreach (string columnName in columnNames)
                {
                    cbTimKiem.Items.Add(columnName);
                    cbSapXep.Items.Add(columnName);
                }

                cbTimKiem.Text = "Chọn cột cần tìm kiếm";
                cbSapXep.Text = "Chọn cột cần sắp xếp";
            }
        }

        private void FrmBangGia_Load(object sender, EventArgs e)
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
                    var ketQuaTimKiem = dbContext.BANGGIAs
                                        .Where(string.Format("{0}.ToString().Contains(@0)", cotTimKiem), giaTriTimKiem)
                                        .ToList();
                    dgvDSBangGia.DataSource = ketQuaTimKiem.Select(bg => new
                    {
                        bg.KhoiLuong,
                        bg.GiaNoiTinh,
                        bg.GiaNoiVung,
                        bg.GiaLienVung,
                        bg.GiaCachVung
                    }).ToList();
                    dgvDSBangGia.AutoResizeColumns();
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
                IQueryable<BANGGIA> query = context.BANGGIAs;
                switch (cotSapXep)
                {
                    case "KhoiLuong":
                        query = query.OrderBy(bg => bg.KhoiLuong);
                        break;
                    case "GiaNoiTinh":
                        query = query.OrderBy(bg => bg.GiaNoiTinh);
                        break;
                    case "GiaNoiVung":
                        query = query.OrderBy(bg => bg.GiaNoiVung);
                        break;
                    case "GiaLienVung":
                        query = query.OrderBy(bg => bg.GiaLienVung);
                        break;
                    case "GiaCachVung":
                        query = query.OrderBy(bg => bg.GiaCachVung);
                        break;
                }
                List<BANGGIA> ketQuaSapXep = query.ToList();
                dgvDSBangGia.DataSource = ketQuaSapXep.Select(bg => new
                {
                    bg.KhoiLuong,
                    bg.GiaNoiTinh,
                    bg.GiaNoiVung,
                    bg.GiaLienVung,
                    bg.GiaCachVung
                }).ToList();
                dgvDSBangGia.AutoResizeColumns();
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
                if (dgvDSBangGia.SelectedRows.Count > 0)
                {
                    int rowIndex = dgvDSBangGia.CurrentCell.RowIndex;
                    string khoiLuong = dgvDSBangGia.Rows[rowIndex].Cells["KhoiLuong"].Value.ToString();

                    using (var context = new QuanLyDVGiaoHangEntities())
                    {
                        var bangGia = context.BANGGIAs.FirstOrDefault(bg => bg.KhoiLuong == khoiLuong);
                        if (bangGia != null)
                        {
                            context.BANGGIAs.Remove(bangGia);
                            context.SaveChanges();
                            loadData();
                            MessageBox.Show("Xóa thông tin bảng giá thành công", "Thông Báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy bảng giá để xóa", "Thông Báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một dòng để xóa thông tin", "Thông Báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi trong quá trình xóa thông tin bảng giá: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            FrmThemBangGIa formThemBangGia = new FrmThemBangGIa();
            formThemBangGia.ShowDialog();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvDSBangGia.SelectedRows.Count > 0)
            {
                int rowIndex = dgvDSBangGia.CurrentCell.RowIndex;
                string khoiLuong = dgvDSBangGia.Rows[rowIndex].Cells["KhoiLuong"].Value.ToString();

                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    var bangGia = context.BANGGIAs.FirstOrDefault(bg => bg.KhoiLuong == khoiLuong);
                    if (bangGia != null)
                    {
                        FrmSuaBangGia formSuaBangGia = new FrmSuaBangGia();
                        formSuaBangGia.bANGGIA = bangGia;
                        formSuaBangGia.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy bảng giá với khối lượng đã chọn!", "Thông Báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để chỉnh sửa thông tin", "Thông Báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
    }
}
