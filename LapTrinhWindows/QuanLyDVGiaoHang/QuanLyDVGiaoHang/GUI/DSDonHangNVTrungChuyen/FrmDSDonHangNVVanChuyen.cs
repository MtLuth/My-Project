using QuanLyDVGiaoHang.GUI.DonHang;
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

namespace QuanLyDVGiaoHang.GUI.DSDonHangNVVanChuyen
{
    public partial class FrmDSDonHangNVVanChuyen : Form
    {
        private string TenDangNhap;
        public FrmDSDonHangNVVanChuyen(string tenDangNhap)
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
                    .Where(dh => dh.TKTrungChuyen == TenDangNhap && dh.TrangThai != "Hoàn thành")
                    .ToList();
                dgvDSDonHangNVNhanHang.DataSource = donHangList.Select(dh => new
                {
                    dh.MaDS,
                    dh.MaVanDon,
                    dh.TKTrungChuyen,
                    dh.Ngay,
                    dh.Gio,
                    dh.TrangThai
                }).ToList();

                dgvDSDonHangNVNhanHang.AutoResizeColumns();
            }
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string giatri = txtTimKiem.Text;
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                var result = context.DS_DONHANGCANTRUNGCHUYEN
                    .Where(dh => dh.TKTrungChuyen == TenDangNhap && dh.MaVanDon.ToString().Contains(giatri) && dh.TrangThai != "Hoàn thành")
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

        private void FrmDSDonHangNVVanChuyen_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void bunifuButton6_Click(object sender, EventArgs e)
        {
            if (dgvDSDonHangNVNhanHang.SelectedRows.Count > 0)
            {
                int row = dgvDSDonHangNVNhanHang.CurrentCell.RowIndex;
                string ID = dgvDSDonHangNVNhanHang.Rows[row].Cells["MaVanDon"].Value.ToString();

                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    var temp = context.DONHANGs.Find(int.Parse(ID));
                    MessageBox.Show("Địa chỉ người gửi của đơn hàng có mã vận đơn: " + temp.MaVanDon.ToString().Trim()
                         + "là: '" + temp.DiaChiNguoiGui.Trim() + "' và người nhận là: '" + temp.DiaChiNguoiNhan.Trim() + "'"
                         , "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để xem chi tiết đơn hàng", "Thông Báo",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            if (dgvDSDonHangNVNhanHang.SelectedRows.Count > 0)
            {
                int maVanDon = Convert.ToInt32(dgvDSDonHangNVNhanHang.SelectedRows[0].Cells["MaVanDon"].Value);
                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    DONHANG donhang = context.DONHANGs.FirstOrDefault(dh => dh.MaVanDon == maVanDon);
                    if (donhang != null)
                    {
                        donhang.TinhTrang = "Đơn hàng đã trung chuyển về kho";

                        // Cập nhật thông tin DS_DONHANGCANTRUNGCHUYEN
                        DS_DONHANGCANTRUNGCHUYEN dsDonHangCanGiaoNhan = context.DS_DONHANGCANTRUNGCHUYEN
                            .FirstOrDefault(ds => ds.DONHANG.MaVanDon == maVanDon);
                        if (dsDonHangCanGiaoNhan != null)
                        {
                            dsDonHangCanGiaoNhan.Ngay = DateTime.Now.Date;
                            dsDonHangCanGiaoNhan.Gio = DateTime.Now.TimeOfDay;
                            dsDonHangCanGiaoNhan.TrangThai = "Hoàn thành";
                        }

                        context.SaveChanges();

                        MessageBox.Show("Đơn hàng đã chuyển về kho", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }

        }

        private void bunifuButton8_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void bunifuButton7_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng Danh Sách Đơn Hàng Nhân Viên Trung Chuyển ?", "Thông Báo",
              MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            FrmDonHangThanhCongNVTC formDonHangThanhCongNVGH = new FrmDonHangThanhCongNVTC(TenDangNhap);
            formDonHangThanhCongNVGH.ShowDialog();
        }
    }
}
