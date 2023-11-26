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
    public partial class FrmDonHangThanhCongQL : Form
    {
        private string TenDangNhap;
        public FrmDonHangThanhCongQL(string tenDangNhap)
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            TenDangNhap = tenDangNhap;
        }

        private void loadData()
        {
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                var donHangList = (from dh in context.DS_DONHANGDENKHO
                                   join donhang in context.DONHANGs on dh.MaVanDon equals donhang.MaVanDon
                                   where dh.TKKiemTra == TenDangNhap && dh.TuyenTrungChuyenTT == "Từ nhân viên nhận hàng" &&
                                   donhang.TinhTrang == "Kiểm tra đơn hàng thành công"
                                   select new
                                   {
                                       dh.MaDS,
                                       dh.MaVanDon,
                                       dh.TKKiemTra,
                                       dh.Ngay,
                                       dh.Gio,
                                       dh.TuyenTrungChuyenTT
                                   }).ToList();

                dgvDSDonHangNVNhanHang.DataSource = donHangList;
                dgvDSDonHangNVNhanHang.AutoResizeColumns();
            }
        }
        private void FrmDonHangThanhCongQL_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string giatri = txtTimKiem.Text;
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                var donHangList = (from dh in context.DS_DONHANGDENKHO
                                   join donhang in context.DONHANGs on dh.MaVanDon equals donhang.MaVanDon
                                   where dh.TKKiemTra == TenDangNhap && dh.TuyenTrungChuyenTT == "Từ nhân viên nhận hàng" &&
                                         donhang.MaVanDon.ToString().Contains(giatri) && donhang.TinhTrang == "Kiểm tra đơn hàng thành công"
                                   select new
                                   {
                                       dh.MaDS,
                                       dh.MaVanDon,
                                       dh.TKKiemTra,
                                       dh.Ngay,
                                       dh.Gio,
                                       dh.TuyenTrungChuyenTT
                                   }).ToList();

                dgvDSDonHangNVNhanHang.DataSource = donHangList;
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

        private void bunifuButton8_Click(object sender, EventArgs e)
        {
            loadData();
        }
    }
}
