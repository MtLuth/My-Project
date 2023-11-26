using QuanLyDVGiaoHang.GUI.DonHang;
using QuanLyDVGiaoHang.GUI.DonHang.NVGiaoHang;
using QuanLyDVGiaoHang.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyDVGiaoHang.GUI.DSDonHangNVGiaoHang
{
    public partial class FrmDSDonHangNVGiaoHang : Form
    {
        private string TenDangNhap;
        public FrmDSDonHangNVGiaoHang(string tenDangNhap)
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
                    .Where(dh => dh.TKGiaoNhanHang == TenDangNhap && dh.GiaoNhan == true && dh.TrangThai != "Hoàn thành")
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
        private void FrmDSDonHangNVGH_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string giatri = txtTimKiem.Text;
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                var result = context.DS_DONHANGCANGIAONHAN
                    .Where(dh => dh.TKGiaoNhanHang == TenDangNhap && dh.MaVanDon.ToString().Contains(giatri) && dh.GiaoNhan == true)
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

        private void bunifuButton8_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void bunifuButton7_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng Danh Sách Đơn Hàng Nhân Viên Giao Hàng ?", "Thông Báo",
             MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void bunifuButton5_Click(object sender, EventArgs e)
        {
            if (dgvDSDonHangNVNhanHang.SelectedRows.Count > 0)
            {
                int maVanDon = Convert.ToInt32(dgvDSDonHangNVNhanHang.SelectedRows[0].Cells["MaVanDon"].Value);
                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    DONHANG donhang = context.DONHANGs.FirstOrDefault(dh => dh.MaVanDon == maVanDon);
                    if (donhang != null)
                    {
                        donhang.TinhTrang = "Giao hàng thành công";

                        DS_DONHANGCANGIAONHAN dsDonHangCanGiaoNhan = context.DS_DONHANGCANGIAONHAN.FirstOrDefault(ds => ds.MaVanDon == maVanDon && ds.GiaoNhan == true);
                        if (dsDonHangCanGiaoNhan != null)
                        {
                            dsDonHangCanGiaoNhan.Ngay = DateTime.Now.Date;
                            dsDonHangCanGiaoNhan.Gio = DateTime.Now.TimeOfDay;
                            dsDonHangCanGiaoNhan.TrangThai = "Hoàn thành";
                            dsDonHangCanGiaoNhan.GhiChu = "Giao hàng thành công";
                        }
                        string tenNguoiNhan = donhang.NguoiGui;
                        TK_KHACHHANG tkKhachHang = context.TK_KHACHHANG.FirstOrDefault(tk => tk.KHACHHANG.TenNguoiBan == tenNguoiNhan);
                        if (tkKhachHang != null)
                        {
                            tkKhachHang.SoDu += donhang.PhiCOD;
                        }

                        context.SaveChanges();

                        MessageBox.Show("Đơn hàng đã giao thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
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
                        donhang.TinhTrang = "Giao hàng không thành công";

                        DS_DONHANGCANGIAONHAN dsDonHangCanGiaoNhan = context.DS_DONHANGCANGIAONHAN.FirstOrDefault(ds => ds.MaVanDon == maVanDon && ds.GiaoNhan == true);
                        if (dsDonHangCanGiaoNhan != null)
                        {
                            dsDonHangCanGiaoNhan.Ngay = DateTime.Now.Date;
                            dsDonHangCanGiaoNhan.Gio = DateTime.Now.TimeOfDay;
                            dsDonHangCanGiaoNhan.TrangThai = "Chưa hoàn thành";
                            dsDonHangCanGiaoNhan.GhiChu = "Giao hàng không thành công";
                        }
                        context.SaveChanges();

                        MessageBox.Show("Đơn hàng giao không thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
        }


        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            FrmDonHangThanhCongNVGiaoHang formDonHangThanhCongNVGH = new FrmDonHangThanhCongNVGiaoHang(TenDangNhap);
            formDonHangThanhCongNVGH.ShowDialog();
        }

        private void bunifuButton9_Click(object sender, EventArgs e)
        {
            FrmDonHangKhongThanhCongNVGiaoHang formDonHangThanhCongNVGH = new FrmDonHangKhongThanhCongNVGiaoHang(TenDangNhap);
            formDonHangThanhCongNVGH.ShowDialog();
        }
    }
}
