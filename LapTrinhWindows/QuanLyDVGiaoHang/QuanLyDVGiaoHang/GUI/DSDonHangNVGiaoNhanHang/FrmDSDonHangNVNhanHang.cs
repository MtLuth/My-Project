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

namespace QuanLyDVGiaoHang.GUI.DSDonHangNVGiaoNhanHang
{
    public partial class FrmDSDonHangNVNhanHang : Form
    {
        private string TenDangNhap;
        public FrmDSDonHangNVNhanHang(string tenDangNhap)
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
                    .Where(dh => dh.TKGiaoNhanHang == TenDangNhap && dh.GiaoNhan == false && dh.TrangThai != "Hoàn thành")
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

        private void FrmDSDonHangNVNhanHang_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            if (dgvDSDonHangNVNhanHang.SelectedRows.Count > 0)
            {
                int row = dgvDSDonHangNVNhanHang.CurrentCell.RowIndex;
                string ID = dgvDSDonHangNVNhanHang.Rows[row].Cells["MaVanDon"].Value.ToString();

                using (var context = new QuanLyDVGiaoHangEntities())
                {

                    var temp = context.DONHANGs.Find(int.Parse(ID));
                    MessageBox.Show("Địa chỉ người gửi của đơn hàng có mã vận đơn: " + temp.MaVanDon.ToString().Trim()
                        + "là: '" + temp.DiaChiNguoiGui.Trim() +"' và người nhận là: '"+ temp.DiaChiNguoiNhan.Trim() +"'"
                        , "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để xem chi tiết đơn hàng", "Thông Báo",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }

        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string giatri = txtTimKiem.Text;
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                var result = context.DS_DONHANGCANGIAONHAN
                    .Where(dh => dh.TKGiaoNhanHang == TenDangNhap && dh.MaVanDon.ToString().Contains(giatri) && dh.GiaoNhan == false && dh.TrangThai != "Hoàn thành")
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

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            if (dgvDSDonHangNVNhanHang.SelectedRows.Count > 0)
            {
                int maVanDon;
                if (int.TryParse(dgvDSDonHangNVNhanHang.SelectedRows[0].Cells["MaVanDon"].Value.ToString(), out maVanDon))
                {
                    using (var context = new QuanLyDVGiaoHangEntities())
                    {
                        DONHANG donhang = context.DONHANGs.FirstOrDefault(dh => dh.MaVanDon == maVanDon);
                        if (donhang != null)
                        {
                            donhang.TinhTrang = "Nhân viên nhận hàng đã nhận đơn hàng";

                            DS_DONHANGCANGIAONHAN dsDonHangCanGiaoNhan = context.DS_DONHANGCANGIAONHAN.FirstOrDefault(ds => ds.MaVanDon == maVanDon);
                            if (dsDonHangCanGiaoNhan != null)
                            {
                                dsDonHangCanGiaoNhan.Ngay = DateTime.Now.Date;
                                dsDonHangCanGiaoNhan.Gio = DateTime.Now.TimeOfDay;
                                dsDonHangCanGiaoNhan.TrangThai = "Nhận thành công";
                                dsDonHangCanGiaoNhan.GhiChu = "Nhân viên nhận hàng đã nhận đơn hàng";
                            }

                            context.SaveChanges();

                            MessageBox.Show("Đơn hàng đã nhận thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy đơn hàng với mã vận đơn đã chọn", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Mã vận đơn không hợp lệ", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }




        private void dgvDSDonHangNVNhanHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void btnDaChuyenVeKho_Click(object sender, EventArgs e)
        {
            if (dgvDSDonHangNVNhanHang.SelectedRows.Count > 0)
            {
                int maVanDon;
                if (int.TryParse(dgvDSDonHangNVNhanHang.SelectedRows[0].Cells["MaVanDon"].Value.ToString(), out maVanDon))
                {
                    using (var context = new QuanLyDVGiaoHangEntities())
                    {
                        DONHANG donhang = context.DONHANGs.FirstOrDefault(dh => dh.MaVanDon == maVanDon);
                        if (donhang != null)
                        {
                            donhang.TinhTrang = "Đơn hàng đã chuyển về kho";

                            DS_DONHANGCANGIAONHAN dsDonHangCanGiaoNhan = context.DS_DONHANGCANGIAONHAN.FirstOrDefault(ds => ds.MaVanDon == maVanDon);
                            if (dsDonHangCanGiaoNhan != null)
                            {
                                dsDonHangCanGiaoNhan.Ngay = DateTime.Now.Date;
                                dsDonHangCanGiaoNhan.Gio = DateTime.Now.TimeOfDay;
                                dsDonHangCanGiaoNhan.TrangThai = "Hoàn thành";
                                dsDonHangCanGiaoNhan.GhiChu = "Đơn hàng đã chuyển về kho";
                            }

                            context.SaveChanges();

                            MessageBox.Show("Đã cập nhật thông tin đơn hàng", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy đơn hàng với mã vận đơn đã chọn", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Mã vận đơn không hợp lệ", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }




        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void bunifuButton7_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng Danh Sách Đơn Hàng Nhân Viên Nhận Hàng ?", "Thông Báo",
              MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            FrmDonHangThanhCongNVNH formDonHangThanhCongNVGH = new FrmDonHangThanhCongNVNH(TenDangNhap);
            formDonHangThanhCongNVGH.ShowDialog();
        }

        private void dgvDSDonHangNVNhanHang_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDSDonHangNVNhanHang.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvDSDonHangNVNhanHang.SelectedRows[0];
                string trangThai = selectedRow.Cells["TrangThai"].Value.ToString();

                if (trangThai == "Nhận thành công")
                {
                    btnDaNhanHang.Enabled = false;
                    btnDaChuyenVeKho.Enabled = true;
                }
                else
                {
                    btnDaNhanHang.Enabled = true;
                    btnDaChuyenVeKho.Enabled = false;
                }
            }
        }
    }
}
