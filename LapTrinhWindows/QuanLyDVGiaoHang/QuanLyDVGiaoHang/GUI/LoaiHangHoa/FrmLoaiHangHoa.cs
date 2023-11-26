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
using System.Net;
using QuanLyDVGiaoHang.GUI.LoaiHangHoa;
using QuanLyDVGiaoHang.GUI.NhanVien;
using QuanLyDVGiaoHang.GUI.KhoTrungChuyen;

namespace QuanLyDVGiaoHang.GUI.LoaiHangHoa
{
    public partial class FrmLoaiHangHoa : Form
    {
        public FrmLoaiHangHoa(string maVaiTro)
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
                List<LOAIHANGHOA> HangList = context.LOAIHANGHOAs.ToList();
                dgvDSLoaiHangHoa.DataSource = HangList.Select(hh => new
                {
                    hh.MaLoai,
                    hh.TenLoai,
                }).ToList();
                dgvDSLoaiHangHoa.AutoResizeColumns();
                cbTimKiem.Items.Clear();
                cbSapXep.Items.Clear();
                foreach (DataGridViewColumn column in dgvDSLoaiHangHoa.Columns)
                {
                    string columnName = column.DataPropertyName;
                    cbTimKiem.Items.Add(columnName);
                    cbSapXep.Items.Add(columnName);
                }
                cbTimKiem.Text = "Chọn cột cần tìm kiếm";
                cbSapXep.Text = "Chọn cột cần sắp xếp";
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
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
                var ketQuaTimKiem = dbContext.LOAIHANGHOAs
                                    .Where(string.Format("{0}.ToString().Contains(@0)", cotTimKiem), giaTriTimKiem)
                                    .ToList();
                dgvDSLoaiHangHoa.DataSource = ketQuaTimKiem.Select(hh => new
                {
                    hh.MaLoai,
                    hh.TenLoai,
                }).ToList();
                dgvDSLoaiHangHoa.AutoResizeColumns();
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
                IQueryable<LOAIHANGHOA> query = dbContext.LOAIHANGHOAs;
                switch (cotSapXep)
                {
                    case "MaLoai":
                        query = query.OrderBy(hh => hh.MaLoai);
                        break;
                    case "TenLoai":
                        query = query.OrderBy(hh => hh.TenLoai);
                        break;
                }
                List<LOAIHANGHOA> ketQuaSapXep = query.ToList();
                dgvDSLoaiHangHoa.DataSource = ketQuaSapXep.Select(hh => new
                {
                    hh.MaLoai,
                    hh.TenLoai,
                }).ToList();
                dgvDSLoaiHangHoa.AutoResizeColumns();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            FrmThemLoaiHangHoa formThemLoaiHangHoa = new FrmThemLoaiHangHoa();
            formThemLoaiHangHoa.ShowDialog();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvDSLoaiHangHoa.SelectedRows.Count > 0)
            {
                int row = dgvDSLoaiHangHoa.CurrentCell.RowIndex;
                string ID = dgvDSLoaiHangHoa.Rows[row].Cells[0].Value.ToString();

                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    var temp = context.LOAIHANGHOAs.Find(ID);
                    FrmSuaLoaiHangHoa formSuaLoaiHangHoa = new FrmSuaLoaiHangHoa();
                    formSuaLoaiHangHoa.lOAIHANGHOA = temp;
                    formSuaLoaiHangHoa.ShowDialog();
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
            if (dgvDSLoaiHangHoa.SelectedRows.Count > 0)
            {
                int row = dgvDSLoaiHangHoa.CurrentCell.RowIndex;
                string ID = dgvDSLoaiHangHoa.Rows[row].Cells[0].Value.ToString();

                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    var toDelete = context.LOAIHANGHOAs.Find(ID);

                    context.LOAIHANGHOAs.Remove(toDelete);

                    context.SaveChanges();

                    loadData();

                    MessageBox.Show("Xóa thông tin loại hàng hóa thành công", "Thông Báo",
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

        private void FrmLoaiHangHoa_Load(object sender, EventArgs e)
        {
            loadData();
        }
    }
}
