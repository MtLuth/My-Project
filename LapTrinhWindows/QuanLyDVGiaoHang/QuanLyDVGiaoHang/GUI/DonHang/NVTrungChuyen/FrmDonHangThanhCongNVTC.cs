using QuanLyDVGiaoHang.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyDVGiaoHang.GUI.DonHang
{
    public partial class FrmDonHangThanhCongNVTC : Form
    {
        private string TenDangNhap;
        public FrmDonHangThanhCongNVTC(string tenDangNhap)
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            TenDangNhap = tenDangNhap;
        }
        private void loadData()
        {
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                List<DS_DONHANGCANTRUNGCHUYEN> donHangList = context.DS_DONHANGCANTRUNGCHUYEN
                    .Where(dh => dh.TKTrungChuyen == TenDangNhap && dh.TrangThai == "Hoàn thành")
                    .ToList();
                dgvDSDonHangNVNhanHang.DataSource = donHangList.Select(dh => new
                {
                    dh.MaDS,
                    dh.MaVanDon,
                    dh.TKTrungChuyen,
                    dh.Ngay,
                    dh.Gio,
                    dh.TrangThai,
                }).ToList();

                dgvDSDonHangNVNhanHang.AutoResizeColumns();
            }
        }

        private void bunifuButton7_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng Danh Sách Đơn Hàng Thành Công ?", "Thông Báo",
             MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void FrmDonHangThanhCongNVTC_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void bunifuButton8_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string giatri = txtTimKiem.Text;
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                var result = context.DS_DONHANGCANTRUNGCHUYEN
                    .Where(dh => dh.TKTrungChuyen == TenDangNhap && dh.MaVanDon.ToString().Contains(giatri) && dh.TrangThai == "Hoàn thành")
                    .Select(dh => new
                    {
                        dh.MaDS,
                        dh.MaVanDon,
                        dh.TKTrungChuyen,
                        dh.Ngay,
                        dh.Gio,
                        dh.TrangThai
                    })
                    .ToList();

                dgvDSDonHangNVNhanHang.DataSource = result;
            }
        }
    }
}
