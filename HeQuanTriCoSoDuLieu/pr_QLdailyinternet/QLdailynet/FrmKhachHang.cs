using BAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLdailynet
{
    public partial class FrmKhachHang : Form
    {
        DataTable dtKhachHang = null;
        DataTable dtTK_KhachHang = null;
        DataTable dtTimKiemKhachHang = null;
        bool Them;
        DBKhachHang dbKH;
        DBTK_KhachHang dbTKKH;
        public FrmKhachHang()
        {
            InitializeComponent();

            this.MinimumSize = this.Size;
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            dbTKKH = new DBTK_KhachHang();

            dbKH = new DBKhachHang();
        }
        public void LoadData()
        {

            dtKhachHang = new DataTable();
            dtKhachHang.Clear();
            dtKhachHang = dbKH.LayKhachHang().Tables[0];

            dgvKhachHang.DataSource = dtKhachHang;
            dgvKhachHang.AutoResizeColumns();
            dgvKH_TK.DataSource = dtKhachHang;
            dgvKH_TK.AutoResizeColumns();

            dtTK_KhachHang = new DataTable();
            dtTK_KhachHang.Clear();
            dtTK_KhachHang = dbTKKH.LayTK_KhachHang().Tables[0];
            dgvTK_KhachHang.DataSource = dtTK_KhachHang;

            // Xóa trống các đối tượng trong panel
            this.txtMaKH.ResetText();
            this.txtTenKH.ResetText();
            this.txtDiaChi.ResetText();
            this.txtEmail.ResetText();
            this.txtSoDT.ResetText();
            this.txtSoDu.Enabled = false;
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
            // Cho thao tác trên các nút ReLoad, thêm, sửa, xóa, thoát
            btnReLoad.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnThoat.Enabled = true;
            //
            panel1.Enabled = false;
            dgvKhachHang_CellClick(null, null);
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            Them = true;
            this.txtMaKH.Enabled = false;
            this.txtMaKH.ResetText();
            this.txtTenKH.ResetText();
            this.txtDiaChi.ResetText();
            this.txtEmail.ResetText();
            this.txtSoDT.ResetText();

            // Không cho thao tác trên các nút thêm, sửa, xóa, thoát
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnThoat.Enabled = false;
            // Cho thao tác trên các nút lưu, hủy, reload và mở panel
            btnLuu.Enabled = true;
            btnHuy.Enabled = true;
            btnReLoad.Enabled = true;
            panel1.Enabled = true;
            // 
            this.txtMaKH.Focus();
        }

        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = dgvKhachHang.CurrentCell.RowIndex;
            // Chuyển thông tin lên panel 
            this.txtMaKH.Text =
            dgvKhachHang.Rows[r].Cells[0].Value.ToString();
            this.txtTenKH.Text =
            dgvKhachHang.Rows[r].Cells[1].Value.ToString();
            this.txtDiaChi.Text =
            dgvKhachHang.Rows[r].Cells[2].Value.ToString();
            this.txtEmail.Text =
            dgvKhachHang.Rows[r].Cells[3].Value.ToString();
            this.txtSoDT.Text =
            dgvKhachHang.Rows[r].Cells[4].Value.ToString();
        }

        private void FrmKhachHang_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            Them = false;
            //
            txtMaKH.Enabled = false;
            // Không cho thao tác trên các nút thêm, sửa, xóa, thoát
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnThoat.Enabled = false;
            // Cho thao tác trên các nút lưu, hủy, reload và mở panel
            btnLuu.Enabled = true;
            btnHuy.Enabled = true;
            btnReLoad.Enabled = true;
            panel1.Enabled = true;
            // 
            this.txtTenKH.Focus();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                int r = dgvKhachHang.CurrentCell.RowIndex;
                string strTho = dgvKhachHang.Rows[r].Cells[0].Value.ToString();
                DialogResult traloi;
                traloi = MessageBox.Show("Chắc xóa mẫu tin này không?", "Trả lời",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                string err = "";
                if (traloi == DialogResult.Yes)
                {
                    bool f = dbKH.XoaKhachHang(ref err, strTho);
                    if (f)
                    {
                        LoadData();
                        MessageBox.Show("Đã xóa xong!");
                    }
                    else
                    {
                        MessageBox.Show("Không xóa được!\n\r" + "Lỗi:" + err);
                    }
                }
                else
                {
                    MessageBox.Show("Không thực hiện việc xóa mẫu tin!");
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Không xóa được. Lỗi :" + ex.Message);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            bool f = false;
            if (Them)
            {
                string err = "";
                try
                {
                    f = dbKH.ThemKhachHang(ref err, txtTenKH.Text, txtDiaChi.Text, txtEmail.Text, 
                        txtSoDT.Text);
                    if (f)
                    {
                        LoadData();
                        MessageBox.Show("Đã thêm xong!");
                    }
                    else
                    {
                        MessageBox.Show("Đã thêm chưa xong!\n\r" + "Lỗi:" + err);
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Không thêm được. Lỗi :" + ex.Message);
                }
            }
            else
            {
                string err = "";
                try
                {
                    f = dbKH.SuaKhachHang(ref err, txtMaKH.Text, txtTenKH.Text, txtDiaChi.Text, 
                        txtEmail.Text, txtSoDT.Text);
                    if (f)
                    {
                        LoadData();
                        MessageBox.Show("Đã cập nhật xong!");
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật chưa xong!\n\r" + "Lỗi:" + err);
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Không cập nhật được.  Lỗi :" + ex.Message);
                }
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.txtMaKH.ResetText();
            this.txtTenKH.ResetText();
            this.txtDiaChi.ResetText();
            this.txtEmail.ResetText();
            this.txtSoDT.ResetText();

            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnThoat.Enabled = true;

            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
            panel1.Enabled = false;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult traloi;
            traloi = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK)
                this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string MaKH = null;
            if (txtMaKH1.Text != "")
                MaKH = txtMaKH1.Text;
            string TenKH = null;
            if (txtTenKH1.Text != "")
                TenKH = txtTenKH1.Text;
            string DiaChi = null;
            if (txtDiaChi1.Text != "")
                DiaChi = txtDiaChi1.Text;
            string Email = null;
            if (txtEmail1.Text != "")
                Email = txtEmail1.Text;
            string SoDT = null;
            if (txtSoDT1.Text != "")
                SoDT = txtSoDT1.Text;

            string errorMessage;
            DataSet ds = dbKH.TimKiemKhachHang(MaKH, TenKH, DiaChi, Email, SoDT, out errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                MessageBox.Show("Lỗi : " +errorMessage);
            }
            else
            {
                dtTimKiemKhachHang = ds.Tables[0];
                dgvKH_TK.DataSource = dtTimKiemKhachHang;
                txtMaKH1.ResetText();
                txtTenKH1.ResetText();
                txtDiaChi1.ResetText();
                txtEmail1.ResetText();
                txtSoDT1.ResetText();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void pbThoat_Click(object sender, EventArgs e)
        {
            DialogResult traloi;
            traloi = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK)
                this.Close();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void FrmKhachHang_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                FormBorderStyle = FormBorderStyle.None;
            else
                FormBorderStyle = FormBorderStyle.Sizable;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            string err = "";
            bool f;
            try
            {
                f = dbTKKH.DangKyTK_KhachHang(ref err, txtMaKhachHang2.Text, txtTenDangNhap.Text, txtMatKhau.Text);
                if (f)
                {
                    LoadData();
                    MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Đăng ký không thành công!\n\r" + "Lỗi: " + err, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Đăng ký không thành công!\n\r" + "Lỗi: " + ex.Message, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void labelDoiMatKhau_Click(object sender, EventArgs e)
        {
            FrmDoiMatKhauKH frm = new FrmDoiMatKhauKH();
            frm.Show();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            int r = dgvTK_KhachHang.CurrentCell.RowIndex;
            DialogResult traloi;
            traloi = MessageBox.Show("Bạn có chắc muốn xóa tài khoản này không!", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK)
            {
                try
                {
                    string err = "";
                    bool f = dbTKKH.XoaTK_KhachHang(ref err, dgvTK_KhachHang.Rows[r].Cells["TenDangNhap"].Value.ToString());
                    if (f)
                    {
                        LoadData();
                        MessageBox.Show("Đã xóa thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Xóa không thành công!\n\r Lỗi: " + err);
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Xóa không thành công!\n\r Lỗi: " + ex.Message);
                }
            }
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if(cbSoDuKhaDung.Checked)
            {
                dtKhachHang = new DataTable();
                dtKhachHang.Clear();
                dtKhachHang = dbKH.HienThiDanhSachKhachHangConTien().Tables[0];

                dgvDanhSach.DataSource = dtKhachHang;
                dgvDanhSach.AutoResizeColumns();
            }
            else
            {
                dtKhachHang = new DataTable();
                dtKhachHang.Clear();
                dtKhachHang = dbKH.HienThiDanhSachKhachHang().Tables[0];

                dgvDanhSach.DataSource = dtKhachHang;
                dgvDanhSach.AutoResizeColumns();
            }
        }
    }
}
