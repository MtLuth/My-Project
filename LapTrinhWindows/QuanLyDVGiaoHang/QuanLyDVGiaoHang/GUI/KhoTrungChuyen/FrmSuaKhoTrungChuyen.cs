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

namespace QuanLyDVGiaoHang.GUI.KhoTrungChuyen
{
    public partial class FrmSuaKhoTrungChuyen : Form
    {
        public KHOTRUNGCHUYEN kHOTRUNGCHUYEN { get; set; }
        public FrmSuaKhoTrungChuyen()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
        }
        void loadData()
        {
            using (var context = new QuanLyDVGiaoHangEntities())
            {

  
                string maKho = kHOTRUNGCHUYEN.MaKho.Trim();
                var Kho = context.KHOTRUNGCHUYENs.FirstOrDefault(mk => mk.MaKho == maKho);
                if (Kho != null)
                {
                    txtMaKho.Text = Kho.MaKho;
                    txtTenKho.Text = Kho.TenKho;
                    txtKhuVucQuanLy.Text = Kho.KhuVucQuanLy;
                    txtHotLine.Text = Kho.HotLine;
                }
            }
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
                    var Kho = dbContext.KHOTRUNGCHUYENs.FirstOrDefault(mk => mk.MaKho == maKho);
                    if (Kho != null)
                    {
                        Kho.MaKho = maKho;
                        Kho.TenKho = tenKho;
                        Kho.KhuVucQuanLy = khuVucquanly;
                        Kho.HotLine = hotLine;

                        dbContext.SaveChanges();

                        MessageBox.Show("Cập nhật thông tin kho trung chuyển thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy kho trung chuyển với mã kho đã cho!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi cập nhật thông tin kho trung chuyển: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng sửa kho trung chuyển ?", "Thông Báo",
             MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void FrmSuaKhoTrungChuyen_Load(object sender, EventArgs e)
        {
            loadData();
        }
    }
}
