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
using QuanLyDVGiaoHang.GUI.KhoTrungChuyen;
using System.Net;
using QuanLyDVGiaoHang.GUI.NhanVien;

namespace QuanLyDVGiaoHang.GUI.KhoTrungChuyen
{
    public partial class FrmKhoTrungChuyen : Form
    {
        public FrmKhoTrungChuyen(string maVaiTro)
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

                List<KHOTRUNGCHUYEN> KhoList = context.KHOTRUNGCHUYENs.ToList();
                dgvDSKhoTrungChuyen.DataSource = KhoList.Select(mk => new
                {
                    mk.MaKho,
                    mk.TenKho,
                    mk.KhuVucQuanLy,
                    mk.HotLine,
                }).ToList();
                dgvDSKhoTrungChuyen.AutoResizeColumns();
                cbTimKiem.Items.Clear();
                cbSapXep.Items.Clear();
                foreach (DataGridViewColumn column in dgvDSKhoTrungChuyen.Columns)
                {
                    string columnName = column.DataPropertyName;
                    cbTimKiem.Items.Add(columnName);
                    cbSapXep.Items.Add(columnName);
                }
                cbTimKiem.Text = "Chọn cột cần tìm kiếm";
                cbSapXep.Text = "Chọn cột cần sắp xếp";
            }
        }

        private void FrmKhoTrungChuyen_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
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
                var ketQuaTimKiem = dbContext.KHOTRUNGCHUYENs
                                    .Where(string.Format("{0}.ToString().Contains(@0)", cotTimKiem), giaTriTimKiem)
                                    .ToList();
                dgvDSKhoTrungChuyen.DataSource = ketQuaTimKiem.Select(mk => new
                {
                    mk.MaKho,
                    mk.TenKho,
                    mk.KhuVucQuanLy,
                    mk.HotLine,
                }).ToList();
                dgvDSKhoTrungChuyen.AutoResizeColumns();
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
            using (var dbContext = new QuanLyDVGiaoHangEntities())
            {
                IQueryable<KHOTRUNGCHUYEN> query = dbContext.KHOTRUNGCHUYENs;
                switch (cotSapXep)
                {
                    case "MaKho":
                        query = query.OrderBy(nv => nv.MaKho);
                        break;
                    case "TenKho":
                        query = query.OrderBy(nv => nv.TenKho);
                        break;
                    case "KhuVucQuanLy":
                        query = query.OrderBy(nv => nv.KhuVucQuanLy);
                        break;
                    case "HotLine":
                        query = query.OrderBy(nv => nv.HotLine);
                        break;
                }
                List<KHOTRUNGCHUYEN> ketQuaSapXep = query.ToList();
                dgvDSKhoTrungChuyen.DataSource = ketQuaSapXep.Select(mk => new
                {
                    mk.MaKho,
                    mk.TenKho,
                    mk.KhuVucQuanLy,
                    mk.HotLine, 
                }).ToList();
                dgvDSKhoTrungChuyen.AutoResizeColumns();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            FrmThemKhoTrungChuyen formThemKhoTrungChuyen = new FrmThemKhoTrungChuyen();
            formThemKhoTrungChuyen.ShowDialog();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {

            if (dgvDSKhoTrungChuyen.SelectedRows.Count > 0)
            {
                int row = dgvDSKhoTrungChuyen.CurrentCell.RowIndex;
                string ID = dgvDSKhoTrungChuyen.Rows[row].Cells[0].Value.ToString();

                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    var temp = context.KHOTRUNGCHUYENs.Find(ID);
                    FrmSuaKhoTrungChuyen formSuaKhoTrungChuyen = new FrmSuaKhoTrungChuyen();
                    formSuaKhoTrungChuyen.kHOTRUNGCHUYEN = temp;
                    formSuaKhoTrungChuyen.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để chỉnh sửa thông tin", "Thông Báo",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDSKhoTrungChuyen.SelectedRows.Count > 0)
            {
                int row = dgvDSKhoTrungChuyen.CurrentCell.RowIndex;
                string ID = dgvDSKhoTrungChuyen.Rows[row].Cells[0].Value.ToString();

                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    var toDelete = context.KHOTRUNGCHUYENs.Find(ID);

                    context.KHOTRUNGCHUYENs.Remove(toDelete);

                    context.SaveChanges();

                    loadData();

                    MessageBox.Show("Xóa thông tin kho trung chuyển thành công", "Thông Báo",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa thông tin", "Thông Báo",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn Có Muốn Thoát Chương Trình ?", "Thông Báo",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }
    }
}
