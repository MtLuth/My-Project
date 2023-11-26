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
    public partial class FrmThemBangGIa : Form
    {
        public FrmThemBangGIa()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
        }
        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng thêm nhân viên ?", "Thông Báo",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
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
                    BANGGIA newBangGia = new BANGGIA
                    {
                        KhoiLuong = khoiLuong,
                        GiaNoiTinh = giaNoiTinh,
                        GiaNoiVung = giaNoiVung,
                        GiaLienVung = giaLienVung,
                        GiaCachVung = giaCachVung
                    };

                    dbContext.BANGGIAs.Add(newBangGia);
                    dbContext.SaveChanges();

                    MessageBox.Show("Thêm bảng giá thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi thêm bảng giá: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
