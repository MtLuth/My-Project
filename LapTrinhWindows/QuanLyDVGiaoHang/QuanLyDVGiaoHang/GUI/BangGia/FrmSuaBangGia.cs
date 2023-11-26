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

namespace QuanLyDVGiaoHang.GUI.BangGia
{
    public partial class FrmSuaBangGia : Form
    {
        public BANGGIA bANGGIA { get; set; }
        public FrmSuaBangGia()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
        }
        void loadData()
        {
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                string khoiLuong = bANGGIA.KhoiLuong;
                var bangGia = context.BANGGIAs.FirstOrDefault(bg => bg.KhoiLuong == khoiLuong);
                if (bangGia != null)
                {
                    txtKhoiLuong.Text = bangGia.KhoiLuong;
                    txtGiaNoiTinh.Text = bangGia.GiaNoiTinh.ToString();
                    txtGiaNoiVung.Text = bangGia.GiaNoiVung.ToString();
                    txtGiaLienVung.Text = bangGia.GiaLienVung.ToString();
                    txtGiaCachVung.Text = bangGia.GiaCachVung.ToString();
                }
            }

        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string khoiLuong = txtKhoiLuong.Text;
                double giaNoiTinh = Convert.ToDouble(txtGiaNoiTinh.Text);
                double giaNoiVung = Convert.ToDouble(txtGiaNoiVung.Text);
                double giaLienVung = Convert.ToDouble(txtGiaLienVung.Text);
                double giaCachVung = Convert.ToDouble(txtGiaCachVung.Text);

                using (var dbContext = new QuanLyDVGiaoHangEntities())
                {
                    var bangGia = dbContext.BANGGIAs.FirstOrDefault(bg => bg.KhoiLuong == khoiLuong);
                    if (bangGia != null)
                    {
                        bangGia.GiaNoiTinh = giaNoiTinh;
                        bangGia.GiaNoiVung = giaNoiVung;
                        bangGia.GiaLienVung = giaLienVung;
                        bangGia.GiaCachVung = giaCachVung;

                        dbContext.SaveChanges();

                        MessageBox.Show("Cập nhật thông tin bảng giá thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy bảng giá với khối lượng đã cho!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi cập nhật thông tin bảng giá: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void FrmSuaBangGia_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng thêm nhân viên ?", "Thông Báo",
              MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }
    }
}
