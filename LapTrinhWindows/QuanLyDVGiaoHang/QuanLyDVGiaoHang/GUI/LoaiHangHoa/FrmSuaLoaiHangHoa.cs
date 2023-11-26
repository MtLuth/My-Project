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

namespace QuanLyDVGiaoHang.GUI.LoaiHangHoa
{
    public partial class FrmSuaLoaiHangHoa : Form
    {
        public LOAIHANGHOA lOAIHANGHOA { get; set; }
        public FrmSuaLoaiHangHoa()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
        }
        private void loadData()
        {
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                string maLoai = lOAIHANGHOA.MaLoai;
                //char maKho = kHOTRUNGCHUYEN.MaKho;
                var Loai = context.LOAIHANGHOAs.FirstOrDefault(hh => hh.MaLoai == maLoai);
                if (Loai != null)
                {
                    txtMaLoai.Text = Loai.MaLoai;
                    txtTenLoai.Text = Loai.TenLoai;
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                string maLoai = txtMaLoai.Text;
                string tenLoai = txtTenLoai.Text;

                using (var dbContext = new QuanLyDVGiaoHangEntities())
                {
                    var Loai = dbContext.LOAIHANGHOAs.FirstOrDefault(hh => hh.MaLoai == maLoai);
                    if (Loai != null)
                    {
                        Loai.MaLoai = maLoai;
                        Loai.TenLoai = tenLoai;

                        dbContext.SaveChanges();

                        MessageBox.Show("Cập nhật thông tin loại hàng hóa thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy loại hàng hóa với mã loại đã cho!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi cập nhật thông tin loại hàng hóa: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng sửa loại hàng hóa ?", "Thông Báo",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void FrmSuaLoaiHangHoa_Load(object sender, EventArgs e)
        {
            loadData();
        }
    }
}
