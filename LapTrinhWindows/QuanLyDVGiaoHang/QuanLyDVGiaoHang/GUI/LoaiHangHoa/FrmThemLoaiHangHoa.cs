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
    public partial class FrmThemLoaiHangHoa : Form
    {
        public LOAIHANGHOA lOAIHANGHOA { get; set; }
        public FrmThemLoaiHangHoa()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string maLoai = txtMaLoai.Text;
                string tenLoai = txtTenLoai.Text;

                using (var dbContext = new QuanLyDVGiaoHangEntities())
                {
                    LOAIHANGHOA newLoai = new LOAIHANGHOA
                    {
                        MaLoai = maLoai,
                        TenLoai = tenLoai
                    };
                    dbContext.LOAIHANGHOAs.Add(newLoai);
                    dbContext.SaveChanges();

                    MessageBox.Show("Thêm loại hàng hóa thành công! Mã loại mới: " + newLoai.MaLoai.ToString(), "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi thêm loại hàng hóa: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng thêm loại hàng hóa ?", "Thông Báo",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void FrmThemLoaiHangHoa_Load(object sender, EventArgs e)
        {

        }
    }
}
