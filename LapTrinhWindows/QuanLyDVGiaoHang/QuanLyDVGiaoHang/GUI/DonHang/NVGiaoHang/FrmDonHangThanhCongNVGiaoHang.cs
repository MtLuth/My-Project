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

namespace QuanLyDVGiaoHang.GUI.DonHang.NVGiaoHang
{
    public partial class FrmDonHangThanhCongNVGiaoHang : Form
    {
        private string TenDangNhap;
        public FrmDonHangThanhCongNVGiaoHang(string tenDangNhap)
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            TenDangNhap = tenDangNhap;
        }
        private void loadData()
        {
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                List<DS_DONHANGCANGIAONHAN> donHangList = context.DS_DONHANGCANGIAONHAN
                    .Where(dh => dh.TKGiaoNhanHang == TenDangNhap && dh.GiaoNhan == true && dh.TrangThai == "Hoàn thành")
                    .ToList();
                dgvDSDonHangNVNhanHang.DataSource = donHangList.Select(dh => new
                {
                    dh.MaDS,
                    dh.MaVanDon,
                    dh.TKGiaoNhanHang,
                    dh.Ngay,
                    dh.Gio,
                    dh.GiaoNhan,
                    dh.TrangThai,
                    dh.GhiChu
                }).ToList();

                dgvDSDonHangNVNhanHang.AutoResizeColumns();
            }
        }

        private void FrmDonHangThanhCongNVGiaoHang_Load(object sender, EventArgs e)
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
                var result = context.DS_DONHANGCANGIAONHAN
                    .Where(dh => dh.TKGiaoNhanHang == TenDangNhap && dh.MaVanDon.ToString().Contains(giatri) && dh.GiaoNhan == true && dh.TrangThai == "Hoàn thành")
                    .Select(dh => new
                    {
                        dh.MaDS,
                        dh.MaVanDon,
                        dh.TKGiaoNhanHang,
                        dh.Ngay,
                        dh.Gio,
                        dh.GiaoNhan,
                        dh.TrangThai,
                        dh.GhiChu
                    })
                    .ToList();

                dgvDSDonHangNVNhanHang.DataSource = result;
            }
        }

        private void bunifuButton7_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng Danh Sách Đơn Hàng Thành Công ?", "Thông Báo",
             MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }
    }
}
