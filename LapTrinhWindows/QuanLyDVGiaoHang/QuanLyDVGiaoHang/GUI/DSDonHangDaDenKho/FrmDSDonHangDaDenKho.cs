using QuanLyDVGiaoHang.GUI.DonHang;
using QuanLyDVGiaoHang.GUI.DonHang.QuanLy;
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

namespace QuanLyDVGiaoHang.GUI.DSDonHangDaDenKho
{
    public partial class FrmDSDonHangDaDenKho : Form
    {
        private string TenDangNhap;
        public FrmDSDonHangDaDenKho(string tenDangNhap)
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            TenDangNhap = tenDangNhap;
        }

        private void FrmDSDonHangDaDenKho_Load(object sender, EventArgs e)
        {
            loadData1();
            loadData2();
            loadData3();
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
        private void loadData1()
        {
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                var donHangList = (from dh in context.DS_DONHANGDENKHO
                                   join donhang in context.DONHANGs on dh.MaVanDon equals donhang.MaVanDon
                                   where dh.TKKiemTra == TenDangNhap && dh.TuyenTrungChuyenTT == "Từ nhân viên nhận hàng" && 
                                   donhang.TinhTrang != "Kiểm tra đơn hàng thành công"
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

        private void loadData2()
        {
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                var donHangList = (from dh in context.DS_DONHANGDENKHO
                                   join donhang in context.DONHANGs on dh.MaVanDon equals donhang.MaVanDon
                                   where dh.TKKiemTra == TenDangNhap && dh.TuyenTrungChuyenTT == "Từ nhân viên trung chuyển" &&
                                   donhang.TinhTrang != "Kiểm tra đơn hàng thành công"
                                   select new
                                   {
                                       dh.MaDS,
                                       dh.MaVanDon,
                                       dh.TKKiemTra,
                                       dh.Ngay,
                                       dh.Gio,
                                       dh.TuyenTrungChuyenTT
                                   }).ToList();

                dgvNVTrungChuyen.DataSource = donHangList;
                dgvNVTrungChuyen.AutoResizeColumns();
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
                var result = context.DS_DONHANGDENKHO
                    .Where(dh => dh.TKKiemTra == TenDangNhap && dh.MaVanDon.ToString().Contains(giatri) && dh.TuyenTrungChuyenTT == "Từ nhân viên nhận hàng")
                    .Select(dh => new
                    {
                        dh.MaDS,
                        dh.MaVanDon,
                        dh.TKKiemTra,
                        dh.Ngay,
                        dh.Gio,
                        dh.TuyenTrungChuyenTT
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
                    FrmCapNhatDonHang formCapNhatDonHangKH = new FrmCapNhatDonHang();
                    formCapNhatDonHangKH.dONHANG = temp;
                    formCapNhatDonHangKH.ShowDialog();
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
            FrmDonHangThanhCongQL formDonHangThanhCongQL = new FrmDonHangThanhCongQL(TenDangNhap);
            formDonHangThanhCongQL.ShowDialog();
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
                        donhang.TinhTrang = "Kiểm tra đơn hàng thành công";
                        context.SaveChanges();

                        MessageBox.Show("Đã đánh dấu kiểu tra đơn hàng thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {
            loadData2();
        }

        private void bunifuButton15_Click(object sender, EventArgs e)
        {
            loadData2();
        }

        private void bunifuButton8_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng Danh Sách Đơn Hàng ?", "Thông Báo",
           MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void bunifuButton14_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng Danh Sách Đơn Hàng ?", "Thông Báo",
          MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void bunifuButton22_Click(object sender, EventArgs e)
        {
            string giatri = txtTimKiem.Text;
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                var result = context.DS_DONHANGDENKHO
                    .Where(dh => dh.TKKiemTra == TenDangNhap && dh.MaVanDon.ToString().Contains(giatri) && 
                    dh.TuyenTrungChuyenTT == "Từ nhân viên trung chuyển")
                    .Select(dh => new
                    {
                        dh.MaDS,
                        dh.MaVanDon,
                        dh.TKKiemTra,
                        dh.Ngay,
                        dh.Gio,
                        dh.TuyenTrungChuyenTT
                    })
                    .ToList();

                dgvNVTrungChuyen.DataSource = result;
            }
        }

        private void bunifuButton12_Click(object sender, EventArgs e)
        {
            if (dgvNVTrungChuyen.SelectedRows.Count > 0)
            {
                int maVanDon = Convert.ToInt32(dgvNVTrungChuyen.SelectedRows[0].Cells["MaVanDon1"].Value);
                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    DONHANG donhang = context.DONHANGs.FirstOrDefault(dh => dh.MaVanDon == maVanDon);
                    if (donhang != null)
                    {
                        donhang.TinhTrang = "Đang đợi nhân viên giao hàng tiếp nhận đơn hàng";
                        context.SaveChanges();

                        MessageBox.Show("Đã đánh dấu phân loại đơn hàng thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
        }

        private void bunifuButton7_Click(object sender, EventArgs e)
        {
            FrmDonHangThanhCongQLTT formDonHangThanhCongQL = new FrmDonHangThanhCongQLTT(TenDangNhap);
            formDonHangThanhCongQL.ShowDialog();
        }
        private void loadData3()
        {
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                var donHangList = (from dh in context.DS_DONHANGDENKHO
                                   join donhang in context.DONHANGs on dh.MaVanDon equals donhang.MaVanDon
                                   where dh.TKKiemTra == TenDangNhap && dh.TuyenTrungChuyenTT == "Từ nhân viên giao hàng" &&
                                   donhang.TinhTrang != "Kiểm tra đơn hàng thành công"
                                   select new
                                   {
                                       dh.MaDS,
                                       dh.MaVanDon,
                                       dh.TKKiemTra,
                                       dh.Ngay,
                                       dh.Gio,
                                       dh.TuyenTrungChuyenTT
                                   }).ToList();

                dgvNVGiaoHang.DataSource = donHangList;
                dgvNVGiaoHang.AutoResizeColumns();
            }
        }
        private void tabPage2_Click(object sender, EventArgs e)
        {
            loadData3();
        }

        private void bunifuButton23_Click(object sender, EventArgs e)
        {
            if (dgvNVGiaoHang.SelectedRows.Count > 0)
            {
                int maVanDon = Convert.ToInt32(dgvNVGiaoHang.SelectedRows[0].Cells["MaVanDon2"].Value);
                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    DONHANG donhang = context.DONHANGs.FirstOrDefault(dh => dh.MaVanDon == maVanDon);
                    if (donhang != null)
                    {
                        donhang.TinhTrang = "Đang đợi nhân viên giao hàng tiếp nhận đơn hàng";
                        context.SaveChanges();

                        MessageBox.Show("Đã đánh dấu phân loại đơn hàng thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
        }

        private void bunifuButton25_Click(object sender, EventArgs e)
        {
            loadData3();
        }

        private void bunifuButton24_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng Danh Sách Đơn Hàng ?", "Thông Báo",
          MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void bunifuButton32_Click(object sender, EventArgs e)
        {
            string giatri = txtTimKiem.Text;
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                var result = context.DS_DONHANGDENKHO
                    .Where(dh => dh.TKKiemTra == TenDangNhap && dh.MaVanDon.ToString().Contains(giatri) &&
                    dh.TuyenTrungChuyenTT == "Từ nhân viên giao hàng")
                    .Select(dh => new
                    {
                        dh.MaDS,
                        dh.MaVanDon,
                        dh.TKKiemTra,
                        dh.Ngay,
                        dh.Gio,
                        dh.TuyenTrungChuyenTT
                    })
                    .ToList();

                dgvNVTrungChuyen.DataSource = result;
            }
        }

        private void bunifuButton13_Click(object sender, EventArgs e)
        {
            FrmDonHangThanhCongQLNVGH formDonHangThanhCongQL = new FrmDonHangThanhCongQLNVGH(TenDangNhap);
            formDonHangThanhCongQL.ShowDialog();
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void bunifuButton9_Click(object sender, EventArgs e)
        {
            loadData1();
        }
    }
}
