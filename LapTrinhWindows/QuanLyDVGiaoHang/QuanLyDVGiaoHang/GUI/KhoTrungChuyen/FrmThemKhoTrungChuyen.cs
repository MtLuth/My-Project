using QuanLyDVGiaoHang.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyDVGiaoHang.GUI.KhoTrungChuyen
{
    public partial class FrmThemKhoTrungChuyen : Form
    {
        public KHOTRUNGCHUYEN kHOTRUNGCHUYEN { get; set; }
        public FrmThemKhoTrungChuyen()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string maKho = txtMaKho.Text;
                string tenKho = txtTenKho.Text;
                string khuVucquanly = txtKhuVucQuanLy.Text;
                string hotLine = txtHotLine.Text;

                using (var dbContext = new QuanLyDVGiaoHangEntities())
                {
                    KHOTRUNGCHUYEN newKho = new KHOTRUNGCHUYEN
                    {
                        MaKho = maKho,
                        TenKho = tenKho,
                        KhuVucQuanLy = khuVucquanly,
                        HotLine = hotLine
                    };
                    dbContext.KHOTRUNGCHUYENs.Add(newKho);
                    dbContext.SaveChanges();

                    MessageBox.Show("Thêm kho trung chuyển thành công! Mã kho mới: " + newKho.MaKho.ToString(), "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi thêm kho trung chuyển: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
                       
        private void btnThoat_Click(object sender, EventArgs e)
        {
                DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng thêm kho trung chuyển ?", "Thông Báo",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                    this.Close();
        }

        private void FrmThemKhoTrungChuyen_Load(object sender, EventArgs e)
        {
                
        }
    }
}
