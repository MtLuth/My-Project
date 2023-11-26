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
    public partial class FrmDonHangNVTrungChuyen : Form
    {
        private string TenDangNhap;
        public FrmDonHangNVTrungChuyen(string tenDangNhap)
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            TenDangNhap = tenDangNhap;
        }
        private void loadData()
        {
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                List<DONHANG> donHangList = context.DONHANGs
                    .Where(dh => dh.TinhTrang == "Kiểm tra đơn hàng thành công")
                    .ToList();
                dgvDSDonHang.DataSource = donHangList.Select(dh => new
                {
                    dh.MaVanDon,
                    dh.NguoiGui,
                    dh.NguoiNhan,
                    dh.DiaChiNguoiGui,
                    dh.DiaChiNguoiNhan,
                    dh.NgayTaoDon,
                    dh.ThongTinHangHoa,
                    dh.LoaiHangHoa,
                    dh.KhoiLuong,
                    dh.PhiVanChuyen,
                    dh.PhiCOD,
                    dh.TinhTrang
                }).ToList();
                dgvDSDonHang.AutoResizeColumns();
            }
        }
        private void FrmDonHangNVTrungChuyen_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string giatri = txtTimKiem.Text;
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                var result = context.DONHANGs
                    .Where(dh => dh.TinhTrang == "Kiểm tra đơn hàng thành công" && dh.MaVanDon.ToString().Contains(giatri))
                    .Select(dh => new
                    {
                        dh.MaVanDon,
                        dh.NguoiGui,
                        dh.NguoiNhan,
                        dh.DiaChiNguoiGui,
                        dh.DiaChiNguoiNhan,
                        dh.NgayTaoDon,
                        dh.ThongTinHangHoa,
                        dh.LoaiHangHoa,
                        dh.KhoiLuong,
                        dh.PhiVanChuyen,
                        dh.PhiCOD,
                        dh.TinhTrang
                    })
                    .ToList();

                dgvDSDonHang.DataSource = result;
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng Đơn Hàng NV Trung Chuyển ?", "Thông Báo",
             MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            if (dgvDSDonHang.SelectedRows.Count > 0)
            {
                int row = dgvDSDonHang.CurrentCell.RowIndex;
                string ID = dgvDSDonHang.Rows[row].Cells[0].Value.ToString();

                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    var temp = context.DONHANGs.Find(int.Parse(ID));
                    FrmChiTietDonHangKH formChiTietDonHangKH = new FrmChiTietDonHangKH();
                    formChiTietDonHangKH.dONHANG = temp;
                    formChiTietDonHangKH.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để xem chi tiết đơn hàng", "Thông Báo",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            if (dgvDSDonHang.SelectedRows.Count > 0)
            {
                int maVanDon = Convert.ToInt32(dgvDSDonHang.SelectedRows[0].Cells["MaVanDon"].Value);
                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    DONHANG donhang = context.DONHANGs.FirstOrDefault(dh => dh.MaVanDon == maVanDon);
                    if (donhang != null)
                    {
                        DS_DONHANGCANTRUNGCHUYEN dsDonHangCanTrungChuyen = new DS_DONHANGCANTRUNGCHUYEN()
                        {
                            TKTrungChuyen = TenDangNhap,
                            MaVanDon = donhang.MaVanDon,
                            Ngay = DateTime.Now.Date,
                            Gio = DateTime.Now.TimeOfDay,
                            TrangThai = "Đã Nhận",
                        };
                        donhang.TinhTrang = "Đơn hàng đang được trung chuyển";

                        context.DS_DONHANGCANTRUNGCHUYEN.Add(dsDonHangCanTrungChuyen);
                        context.SaveChanges();

                        MessageBox.Show("Đơn hàng đã được nhận", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để nhận đơn hàng này", "Thông Báo",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
        }
    }
}
