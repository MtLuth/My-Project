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

namespace QuanLyDVGiaoHang.GUI.VaiTro
{
    public partial class FrmSuaVaiTro : Form
    {
        public VAITRO vAITRO { get; set; }
        public FrmSuaVaiTro()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
        }
        private void loadData()
        {
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                string maVaiTro = vAITRO.MaVaiTro;
                //char maKho = kHOTRUNGCHUYEN.MaKho;
                var VaiTro = context.VAITROes.FirstOrDefault(vt => vt.MaVaiTro == maVaiTro);
                if (VaiTro != null)
                {
                    txtMaVT.Text = VaiTro.MaVaiTro;
                    txtTenVT.Text = VaiTro.TenVaiTro;
                }
            }
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string maVaiTro = txtMaVT.Text;
                string tenVaiTro = txtTenVT.Text;

                using (var dbContext = new QuanLyDVGiaoHangEntities())
                {
                    var VaiTro = dbContext.VAITROes.FirstOrDefault(vt => vt.MaVaiTro == maVaiTro);
                    if (VaiTro != null)
                    {
                        VaiTro.MaVaiTro = maVaiTro;
                        VaiTro.TenVaiTro = tenVaiTro;

                        dbContext.SaveChanges();

                        MessageBox.Show("Cập nhật thông tin vai trò thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy vai trò với mã vai trò đã cho!", "Lỗi",
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

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng sửa vai trò ?", "Thông Báo",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void FrmSuaVaiTro_Load(object sender, EventArgs e)
        {
            loadData();
        }
    }
}
