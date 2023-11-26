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
    public partial class FrmDonHangQuanLy : Form
    {
        private string TenDangNhap;
        public FrmDonHangQuanLy(string tenDangNhap)
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            TenDangNhap = tenDangNhap;
        }
        private void loadData1()
        {
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                List<DONHANG> donHangList = context.DONHANGs
                    .Where(dh => dh.TinhTrang == "Đơn hàng đã chuyển về kho")
                    .ToList();
                dgvDSDonHangNVNhanHang.DataSource = donHangList.Select(dh => new
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
                dgvDSDonHangNVNhanHang.AutoResizeColumns();
            }
        }
        private void loadData2()
        {
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                List<DONHANG> donHangList = context.DONHANGs
                    .Where(dh => dh.TinhTrang == "Đơn hàng đã trung chuyển về kho")
                    .ToList();
                dgvNVTrungChuyen.DataSource = donHangList.Select(dh => new
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
                dgvNVTrungChuyen.AutoResizeColumns();
            }
        }
        private void loadData3()
        {
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                List<DONHANG> donHangList = context.DONHANGs
                    .Where(dh => dh.TinhTrang == "Giao hàng không thành công")
                    .ToList();
                dgvNVGiaoHang.DataSource = donHangList.Select(dh => new
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
                dgvNVGiaoHang.AutoResizeColumns();
            }
        }
        private void tabPage1_Click(object sender, EventArgs e)
        {
            loadData1();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string giatri = txtTimKiem.Text;
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                var result = context.DONHANGs
                    .Where(dh => dh.TinhTrang == "Đơn hàng đã chuyển về kho" && dh.MaVanDon.ToString().Contains(giatri))
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

                dgvDSDonHangNVNhanHang.DataSource = result;
            }
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            if (dgvDSDonHangNVNhanHang.SelectedRows.Count > 0)
            {
                int row = dgvDSDonHangNVNhanHang.CurrentCell.RowIndex;
                string ID = dgvDSDonHangNVNhanHang.Rows[row].Cells[0].Value.ToString();

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

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            loadData1();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng Đơn Hàng ?", "Thông Báo",
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
                        DS_DONHANGDENKHO dsDonHangDaDenKho = new DS_DONHANGDENKHO()
                        {
                            TKKiemTra = TenDangNhap,
                            MaVanDon = donhang.MaVanDon,
                            Ngay = DateTime.Now.Date,
                            Gio = DateTime.Now.TimeOfDay,
                            TuyenTrungChuyenTT = "Từ nhân viên nhận hàng"
                        };
                        donhang.TinhTrang = "Nhân viên đang xử lý đơn hàng";

                        context.DS_DONHANGDENKHO.Add(dsDonHangDaDenKho);
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

        private void bunifuButton10_Click(object sender, EventArgs e)
        {
            string giatri = txtTimKiem.Text;
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                var result = context.DONHANGs
                    .Where(dh => dh.TinhTrang == "Đơn hàng đã trung chuyển về kho" && dh.MaVanDon.ToString().Contains(giatri))
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

                dgvNVTrungChuyen.DataSource = result;
            }
        }

        private void bunifuButton6_Click(object sender, EventArgs e)
        {
            if (dgvNVTrungChuyen.SelectedRows.Count > 0)
            {
                int row = dgvNVTrungChuyen.CurrentCell.RowIndex;
                string ID = dgvNVTrungChuyen.Rows[row].Cells[0].Value.ToString();

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

        private void bunifuButton9_Click(object sender, EventArgs e)
        {
            loadData2();
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {
            loadData2();
        }

        private void bunifuButton7_Click(object sender, EventArgs e)
        {
            if (dgvNVTrungChuyen.SelectedRows.Count > 0)
            {
                int maVanDon = Convert.ToInt32(dgvNVTrungChuyen.SelectedRows[0].Cells["MaVanDon1"].Value);
                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    DONHANG donhang = context.DONHANGs.FirstOrDefault(dh => dh.MaVanDon == maVanDon);
                    if (donhang != null)
                    {
                        DS_DONHANGDENKHO dsDonHangDaDenKho = new DS_DONHANGDENKHO()
                        {
                            TKKiemTra = TenDangNhap,
                            MaVanDon = donhang.MaVanDon,
                            Ngay = DateTime.Now.Date,
                            Gio = DateTime.Now.TimeOfDay,
                            TuyenTrungChuyenTT = "Từ nhân viên trung chuyển"
                        };
                        donhang.TinhTrang = "Nhân viên đang phân loại đơn hàng";

                        context.DS_DONHANGDENKHO.Add(dsDonHangDaDenKho);
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

        private void tabPage2_Click(object sender, EventArgs e)
        {
            loadData3();
        }

        private void bunifuButton14_Click(object sender, EventArgs e)
        {
            loadData3();
        }

        private void bunifuButton13_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng Đơn Hàng ?", "Thông Báo",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void bunifuButton11_Click(object sender, EventArgs e)
        {
            if (dgvNVGiaoHang.SelectedRows.Count > 0)
            {
                int row = dgvNVGiaoHang.CurrentCell.RowIndex;
                string ID = dgvNVGiaoHang.Rows[row].Cells[0].Value.ToString();

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

        private void bunifuButton15_Click(object sender, EventArgs e)
        {
            string giatri = txtTimKiem.Text;
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                var result = context.DONHANGs
                    .Where(dh => dh.TinhTrang == "Giao hàng không thành công" && dh.MaVanDon.ToString().Contains(giatri))
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

                dgvNVGiaoHang.DataSource = result;
            }
        }

        private void bunifuButton12_Click(object sender, EventArgs e)
        {
            if (dgvNVGiaoHang.SelectedRows.Count > 0)
            {
                int maVanDon = Convert.ToInt32(dgvNVGiaoHang.SelectedRows[0].Cells["MaVanDon2"].Value);
                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    DONHANG donhang = context.DONHANGs.FirstOrDefault(dh => dh.MaVanDon == maVanDon);
                    if (donhang != null)
                    {
                        DS_DONHANGDENKHO dsDonHangDaDenKho = new DS_DONHANGDENKHO()
                        {
                            TKKiemTra = TenDangNhap,
                            MaVanDon = donhang.MaVanDon,
                            Ngay = DateTime.Now.Date,
                            Gio = DateTime.Now.TimeOfDay,
                            TuyenTrungChuyenTT = "Từ nhân viên giao hàng"
                        };
                        donhang.TinhTrang = "Giao không thành công do " + txtLyDo.Text;

                        context.DS_DONHANGDENKHO.Add(dsDonHangDaDenKho);
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

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            tpDonHang.SelectedIndex = 0;
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            tpDonHang.SelectedIndex = 1;
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            tpDonHang.SelectedIndex = 2;
        }

        private void FrmDonHangQuanLy_Load(object sender, EventArgs e)
        {
            loadData1();
            loadData2();
            loadData3();
        }

        private void bunifuButton8_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng Đơn Hàng ?", "Thông Báo",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }
    }
}
