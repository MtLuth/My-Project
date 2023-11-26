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
using QuanLyDVGiaoHang.GUI.VaiTro;
using System.Net;
using QuanLyDVGiaoHang.GUI.NhanVien;
using QuanLyDVGiaoHang.GUI.LoaiHangHoa;

namespace QuanLyDVGiaoHang.GUI.VaiTro
{
    public partial class FrmVaiTro : Form
    {
        public FrmVaiTro()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
        }
        private void loadData()
        {
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                List<VAITRO> VaiTroList = context.VAITROes.ToList();
                dgvDSVaiTro.DataSource = VaiTroList.Select(vt => new
                {
                    vt.MaVaiTro,
                    vt.TenVaiTro,
                }).ToList();
                dgvDSVaiTro.AutoResizeColumns();
                cbTimKiem.Items.Clear();
                cbSapXep.Items.Clear();
                foreach (DataGridViewColumn column in dgvDSVaiTro.Columns)
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
                var ketQuaTimKiem = dbContext.VAITROes
                                    .Where(string.Format("{0}.ToString().Contains(@0)", cotTimKiem), giaTriTimKiem)
                                    .ToList();
                dgvDSVaiTro.DataSource = ketQuaTimKiem.Select(vt => new
                {
                    vt.MaVaiTro,
                    vt.TenVaiTro,
                }).ToList();
                dgvDSVaiTro.AutoResizeColumns();
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
                IQueryable<VAITRO> query = dbContext.VAITROes;
                switch (cotSapXep)
                {
                    case "MaVaiTro":
                        query = query.OrderBy(vt => vt.MaVaiTro);
                        break;
                    case "TenvaiTro":
                        query = query.OrderBy(vt => vt.MaVaiTro);
                        break;
                }
                List<VAITRO> ketQuaSapXep = query.ToList();
                dgvDSVaiTro.DataSource = ketQuaSapXep.Select(vt => new
                {
                    vt.MaVaiTro,
                    vt.TenVaiTro,
                }).ToList();
                dgvDSVaiTro.AutoResizeColumns();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvDSVaiTro.SelectedRows.Count > 0)
            {
                int row = dgvDSVaiTro.CurrentCell.RowIndex;
                string ID = dgvDSVaiTro.Rows[row].Cells[0].Value.ToString();

                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    var temp = context.VAITROes.Find(ID);
                    FrmSuaVaiTro formSuaVaiTro = new FrmSuaVaiTro();
                    formSuaVaiTro.vAITRO = temp;
                    formSuaVaiTro.ShowDialog();
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
            if (dgvDSVaiTro.SelectedRows.Count > 0)
            {
                int row = dgvDSVaiTro.CurrentCell.RowIndex;
                string ID = dgvDSVaiTro.Rows[row].Cells[0].Value.ToString();

                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    var toDelete = context.VAITROes.Find(ID);

                    context.VAITROes.Remove(toDelete);

                    context.SaveChanges();

                    loadData();

                    MessageBox.Show("Xóa thông tin vai trò thành công", "Thông Báo",
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

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn Có Muốn Thoát Chương Trình ?", "Thông Báo",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void FrmVaiTro_Load(object sender, EventArgs e)
        {
            loadData();
        }
    }
}
