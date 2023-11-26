using QuanLyDVGiaoHang.GUI.DSDonHangDaDenKho;
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
    public partial class FrmAllDonHang : Form
    {
        public FrmAllDonHang()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
        }
        private void loadData()
        {
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                List<DONHANG> donHangList = context.DONHANGs.ToList();

                dgvDSDonHang.DataSource = donHangList
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
                dgvDSDonHang.AutoResizeColumns();
            }
        }
        private void FrmAllDonHang_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
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
                    formChiTietDonHangKH.Show();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để xem chi tiết đơn hàng", "Thông Báo",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng Đơn Hàng ?", "Thông Báo",
             MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string giatri = txtTimKiem.Text;
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                var result = context.DONHANGs
                    .Where(dh => dh.MaVanDon.ToString().Contains(giatri))
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

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            if (dgvDSDonHang.SelectedRows.Count > 0)
            {
                int row = dgvDSDonHang.CurrentCell.RowIndex;
                string ID = dgvDSDonHang.Rows[row].Cells["MaVanDon"].Value.ToString();

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
    }
}
