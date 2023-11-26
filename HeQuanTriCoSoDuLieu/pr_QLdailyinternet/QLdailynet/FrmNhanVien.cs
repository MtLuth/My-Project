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
    public partial class FrmNhanVien : Form
    {
        DataTable dtNhanVien = null;
        DataTable dtVaiTro = null;
        DataTable dtPhong = null;
        DataTable dtTK_NhanVien = null;
        DataTable dtTimKiemNhanVien;
        bool Them;
        DBNhanVien dbNV;
        DBVaiTro dbVT;
        DBPhong dbP;
        DBTK_NhanVien dbTKNV;
        public FrmNhanVien()
        {
            InitializeComponent();

            this.MinimumSize = this.Size;
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;

            dbNV = new DBNhanVien();
            dbVT = new DBVaiTro();
            dbP = new DBPhong();
            dbTKNV = new DBTK_NhanVien();
        }

        public void LoadData()
        {
            //tab CRUD
            dgvNhanVien.Columns["NgaySinh"].DefaultCellStyle.Format = "dd/MM/yyyy";
            // Vận chuyển dữ liệu vào DataTable
            dtNhanVien = new DataTable();
            dtNhanVien.Clear();
            dtNhanVien = dbNV.LayNhanVien().Tables[0];
            dtVaiTro = dbVT.LayVaiTro().Tables[0];
            dtPhong = dbP.LayPhong().Tables[0];
            // Đưa dữ liệu nhân viên lên dtgv
            dgvNhanVien.DataSource = dtNhanVien;
            // Thay đổi độ rộng cột
            dgvNhanVien.AutoResizeColumns();
            // Đưa dữ liệu lên cboVaiTro
            cboVaiTro.DataSource = dtVaiTro;
            cboVaiTro.DisplayMember = "TenVaiTro";
            cboVaiTro.ValueMember = "MaVaiTro";
            cboVaiTro.SelectedValue = "";
            // Đưa dữ liệu lên cboPhong
            cboPhongBan.DataSource = dtPhong;
            cboPhongBan.DisplayMember = "TenPhong";
            cboPhongBan.ValueMember = "MaPhong";
            cboPhongBan.SelectedValue = "";
            // Xóa trống các đối tượng trong panel
            this.txtMaNhanVien.ResetText();
            this.txtTenNV.ResetText();
            this.txtNgaySinh.ResetText();
            this.txtDiaChi.ResetText();
            this.txtEmail.ResetText();
            this.txtSoDT.ResetText();
            this.cboVaiTro.Text = "";
            this.txtLuong.ResetText();
            this.cboPhongBan.Text = "";
            // Không cho thao tác các nút lưu, hủy bỏ
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
            // Cho thao tác trên các nút ReLoad, thêm, sửa, xóa, thoát
            btnReLoad.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnThoat.Enabled = true;
            //
            panel1.Enabled = false;
            // Tab tai khoan
            dtTK_NhanVien = new DataTable();
            dtTK_NhanVien.Clear();
            dtTK_NhanVien = dbTKNV.LayTK_NhanVien().Tables[0];
            dgvTK_NhanVien.DataSource = dtTK_NhanVien;
            dgvTK_NhanVien.AutoResizeColumns();
            pictureBox4.Enabled = false;
            //
            this.txtMaNhanVien2.ResetText();
            this.txtTenDangNhap.ResetText();
            this.txtMatKhau.ResetText();
            //
            //Tab Tìm kiếm
            dgvTimKiemNV.DataSource = dtNhanVien;
            dgvTimKiemNV.Columns["NgaySinh3"].DefaultCellStyle.Format = "dd/MM/yyyy";
            txtMaNhanVien3.ResetText();
            txtTenNV3.ResetText();
            txtNgaySinh3.ResetText();
            txtDiaChi3.ResetText();
            txtEmail3.ResetText();
            txtSoDT3.ResetText();
            
            txtLuong3.ResetText();
            

            cboVaiTro3.DataSource = dtVaiTro;
            cboVaiTro3.DisplayMember = "TenVaiTro";
            cboVaiTro3.ValueMember = "MaVaiTro";
            

            cboPhongBan3.DataSource = dtPhong;
            cboPhongBan3.DisplayMember = "TenPhong";
            cboPhongBan3.ValueMember = "MaPhong";
            cboVaiTro3.SelectedValue = "";
            cboPhongBan3.SelectedValue = "";

            //
            // Page vai trò
            txtMaVaiTro.ResetText();
            txtTenVaiTro.ResetText();
            dgvVaiTro.DataSource = dbVT.LayVaiTro().Tables[0];
            dgvVaiTro.AutoResizeColumns();
            //
            pnTimKiem.Enabled = true;
            // Không cho thao tác các nút lưu, hủy bỏ
            btnLuu4.Enabled = false;
            btnHuy4.Enabled = false;
            // Cho thao tác trên các nút ReLoad, thêm, sửa, xóa, thoát
            btnTaiLai4.Enabled = true;
            btnThem4.Enabled = true;
            btnSua4.Enabled = true;
            btnThoat4.Enabled = true;


            dgvNhanVien_SelectionChanged(null, null);
        }

        private void FrmNhanVien_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'pROJECT_INTERNETDataSet1.PHONG' table. You can move, or remove it, as needed.
            this.pHONGTableAdapter.Fill(this.pROJECT_INTERNETDataSet1.PHONG);
            // TODO: This line of code loads data into the 'pROJECT_INTERNETDataSet.VAITRO' table. You can move, or remove it, as needed.
            this.vAITROTableAdapter.Fill(this.pROJECT_INTERNETDataSet.VAITRO);
            LoadData();
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvNhanVien.CurrentRow == null)
            {
                // Xử lý khi không có dòng nào được chọn
                MessageBox.Show("Dữ liệu trong DataGridView trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // Thứ tự dòng hiện hành
            int r = dgvNhanVien.CurrentCell.RowIndex;
            // Chuyển thông tin lên Panel
            txtMaNhanVien.Text =
                dgvNhanVien.Rows[r].Cells[0].Value.ToString();
            txtTenNV.Text =
                dgvNhanVien.Rows[r].Cells[1].Value.ToString();
            // Chuyển đổi định dạng Ngày
            DateTime ngaysinh = (DateTime)dgvNhanVien.Rows[r].Cells[2].Value;
            Console.WriteLine(ngaysinh.ToString());
            txtNgaySinh.Text = ngaysinh.ToString("dd/MM/yyyy");
            txtDiaChi.Text =
                dgvNhanVien.Rows[r].Cells[3].Value.ToString();
            txtEmail.Text =
                dgvNhanVien.Rows[r].Cells[4].Value.ToString();
            txtSoDT.Text =
                dgvNhanVien.Rows[r].Cells[5].Value.ToString();
            txtLuong.Text =
                dgvNhanVien.Rows[r].Cells[7].Value.ToString();
            foreach (var item in cboVaiTro.Items)
            {
                if ((item as DataRowView)[cboVaiTro.DisplayMember].ToString() == dgvNhanVien.Rows[r].Cells[6].FormattedValue.ToString())
                {
                    cboVaiTro.SelectedItem = item;
                    break;
                }
            }
            foreach (var item1 in cboPhongBan.Items)
            {
                if ((item1 as DataRowView)[cboPhongBan.DisplayMember].ToString() == dgvNhanVien.Rows[r].Cells[8].FormattedValue.ToString())
                {
                    cboPhongBan.SelectedItem = item1;
                    break;
                }
            }
        }

        private void btnReLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            Them = true;
            //
            this.txtMaNhanVien.Enabled = false;
            // Xóa trống các đối tượng trong panel
            this.txtMaNhanVien.ResetText();
            this.txtTenNV.ResetText();
            this.txtNgaySinh.ResetText();
            this.txtDiaChi.ResetText();
            this.txtEmail.ResetText();
            this.txtSoDT.ResetText();
            this.cboVaiTro.Text = "";
            this.txtLuong.ResetText();
            this.cboPhongBan.Text = "";
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
            this.txtMaNhanVien.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Kích hoạt biến thêm
            Them = false;
            //
            txtMaNhanVien.Enabled = false;
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
            this.txtMaNhanVien.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            bool f = false;
            if (Them)
            {
                string err = "";
                try
                {
                    DateTime ngaysinh = DateTime.ParseExact(txtNgaySinh.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    int luong = Convert.ToInt32(txtLuong.Text);
                    f = dbNV.ThemNhanVien(ref err, txtTenNV.Text,
                        ngaysinh, txtDiaChi.Text, txtEmail.Text, txtSoDT.Text,
                        cboVaiTro.SelectedValue.ToString(), luong, cboPhongBan.SelectedValue.ToString());
                    if (f)
                    {
                        // Load lại dữ liệu trên DataGridView 
                        LoadData();
                        // Thông báo 
                        MessageBox.Show("Đã thêm xong!");
                    }
                    else
                    {
                        MessageBox.Show("Thêm không thàng công!\n\r" + "Lỗi:" + err);
                    }
                }
                catch (SqlException ex)
                {
                        MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
            else
            {
                string err = "";
                try
                {
                    DateTime ngaysinh = DateTime.ParseExact(txtNgaySinh.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    ngaysinh = ngaysinh.Date;
                    int luong = Convert.ToInt32(txtLuong.Text);
                    f = dbNV.SuaNhanVien(ref err, txtMaNhanVien.Text, txtTenNV.Text,
                        ngaysinh, txtDiaChi.Text, txtEmail.Text, txtSoDT.Text,
                        cboVaiTro.SelectedValue.ToString(), luong, cboPhongBan.SelectedValue.ToString());
                    if (f)
                    {
                        // Load lại dữ liệu trên DataGridView 
                        LoadData();
                        // Thông báo 
                        MessageBox.Show("Đã cập nhật xong!");
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật không thành công!\n\r" + "Lỗi:" + err);
                    }
                }
                catch (SqlException ex)
                {
                    
                    MessageBox.Show("Lỗi: " + ex.Message);
                }

            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult traloi;
            traloi = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK)
                this.Close();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                int r = dgvNhanVien.CurrentCell.RowIndex;
                string strTho = dgvNhanVien.Rows[r].Cells[0].Value.ToString();
                DialogResult traloi;
                traloi = MessageBox.Show("Chắc xóa mẫu tin này không?", "Trả lời",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                string err = "";
                if (traloi == DialogResult.Yes)
                {
                    bool f = dbNV.XoaNhanVien(ref err, strTho);
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
            catch (SqlException)
            {
                MessageBox.Show("Không xóa được. Lỗi rồi!!!");
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.txtMaNhanVien.ResetText();
            this.txtTenNV.ResetText();
            this.txtNgaySinh.ResetText();
            this.txtDiaChi.ResetText();
            this.txtEmail.ResetText();
            this.txtSoDT.ResetText();
            this.cboVaiTro.Text = "";
            this.txtLuong.ResetText();
            this.cboPhongBan.Text = "";

            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnThoat.Enabled = true;

            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
            panel1.Enabled = false;
        }

        

        private void labelDoiMatKhau_Click(object sender, EventArgs e)
        {
            FrmDoiMatKhau frm = new FrmDoiMatKhau();
            frm.Show();
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            string err = "";
            bool f;
            try
            {
                f = dbTKNV.DangKyTK_NhanVien(ref err, txtMaNhanVien2.Text, txtTenDangNhap.Text, txtMatKhau.Text);
                if (f)
                {
                    LoadData();
                    MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Đăng ký không thành công!\n\r"+ txtMaNhanVien2.Text+ 
                        txtTenDangNhap.Text+ txtMatKhau.Text + "Lỗi: " + err, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Đăng ký không thành công!\n\r" + "Lỗi: " + ex.Message, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvTK_NhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            pictureBox4.Enabled = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DateTime? ngaysinh = new DateTime();
            ngaysinh = null;

            if (txtNgaySinh3.Text != "")
            {
                try { ngaysinh = DateTime.ParseExact(txtNgaySinh3.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture); }
                catch (Exception)
                {
                    MessageBox.Show("Ngày không hợp lệ!\n\r Định dạng: dd/MM/yyyy");
                }
            }
            int? luong = null;
            if (txtLuong3.Text != "")
            {
                try { luong = Convert.ToInt32(txtLuong3.Text); }
                catch (Exception)
                {
                    MessageBox.Show("Tiền lương phải là số!");
                    return;
                }
            }
            string vaitro = null;
            if (cboVaiTro3.SelectedValue != null)
            {
                vaitro = cboVaiTro3.SelectedValue.ToString();
            }
            string phongban = null;
            if (cboPhongBan3.SelectedValue != null)
            {
                phongban = cboPhongBan.SelectedValue.ToString();
            }
            string MaNV = null;
            if (txtMaNhanVien3.Text != "")
                MaNV = txtMaNhanVien3.Text;
            string TenNV = null;
            if (txtTenNV3.Text != "")
                TenNV = txtTenNV3.Text;
            string DiaChi = null;
            if (txtDiaChi3.Text != "")
                DiaChi = txtDiaChi3.Text;
            string Email = null;
            if (txtEmail3.Text != "")
                Email = txtEmail3.Text;
            string SoDT = null;
            if (txtSoDT3.Text != "")
                SoDT = txtSoDT3.Text;
            string errorMessage;
            DataSet ds = dbNV.TimKiemNhanVien(MaNV, TenNV,
                                ngaysinh, DiaChi, Email, SoDT, vaitro,
                                luong, phongban, out errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                MessageBox.Show("Lỗi :" +errorMessage);
            }
            else
            {
                dtTimKiemNhanVien = ds.Tables[0];
                dgvTimKiemNV.DataSource = dtTimKiemNhanVien;
                txtMaNhanVien3.ResetText();
                txtTenNV3.ResetText();
                txtNgaySinh3.ResetText();
                txtDiaChi3.ResetText();
                txtEmail3.ResetText();
                txtSoDT3.ResetText();
                cboVaiTro3.SelectedValue = "";
                txtLuong3.ResetText();
                cboPhongBan3.SelectedValue = "";
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvNhanVien_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow == null)
            {
                MessageBox.Show("Dữ liệu trong DataGridView trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // Thứ tự dòng hiện hành
            int r = dgvNhanVien.CurrentCell.RowIndex;
            // Chuyển thông tin lên Panel
            txtMaNhanVien.Text =
                dgvNhanVien.Rows[r].Cells[0].Value.ToString();
            txtTenNV.Text =
                dgvNhanVien.Rows[r].Cells[1].Value.ToString();
            // Chuyển đổi định dạng Ngày
            DateTime ngaysinh = (DateTime)dgvNhanVien.Rows[r].Cells[2].Value;
            txtNgaySinh.Text = ngaysinh.ToString("dd/MM/yyyy");
            txtDiaChi.Text =
                dgvNhanVien.Rows[r].Cells[3].Value.ToString();
            txtEmail.Text =
                dgvNhanVien.Rows[r].Cells[4].Value.ToString();
            txtSoDT.Text =
                dgvNhanVien.Rows[r].Cells[5].Value.ToString();
            txtLuong.Text =
                dgvNhanVien.Rows[r].Cells[7].Value.ToString();
            foreach (var item in cboVaiTro.Items)
            {
                if ((item as DataRowView)[cboVaiTro.DisplayMember].ToString() == dgvNhanVien.Rows[r].Cells[6].FormattedValue.ToString())
                {
                    cboVaiTro.SelectedItem = item;
                    break;
                }
            }
            foreach (var item1 in cboPhongBan.Items)
            {
                if ((item1 as DataRowView)[cboPhongBan.DisplayMember].ToString() == dgvNhanVien.Rows[r].Cells[8].FormattedValue.ToString())
                {
                    cboPhongBan.SelectedItem = item1;
                    break;
                }
            }
        }

        private void btnThem4_Click(object sender, EventArgs e)
        {
            Them = true;
            // Xóa trống các đối tượng trong panel
            this.txtMaVaiTro.ResetText();
            this.txtTenVaiTro.ResetText();
            // Không cho thao tác trên các nút thêm, sửa, xóa, thoát
            btnThem4.Enabled = false;
            btnSua4.Enabled = false;
            btnXoa4.Enabled = false;
            btnThoat4.Enabled = false;
            // Cho thao tác trên các nút lưu, hủy, reload và mở panel
            btnLuu4.Enabled = true;
            btnHuy4.Enabled = true;
            btnTaiLai4.Enabled = true;
            pnTimKiem.Enabled = true;
            // 
            this.txtMaVaiTro.Focus();
        }

        private void btnSua4_Click(object sender, EventArgs e)
        {
            // Kích hoạt biến thêm
            Them = false;
            //
            txtMaVaiTro.Enabled = false;
            // Không cho thao tác trên các nút thêm, sửa, xóa, thoát
            btnThem4.Enabled = false;
            btnSua4.Enabled = false;
            btnXoa4.Enabled = false;
            btnThoat4.Enabled = false;
            // Cho thao tác trên các nút lưu, hủy, reload và mở panel
            btnLuu4.Enabled = true;
            btnHuy4.Enabled = true;
            btnTaiLai4.Enabled = true;
            pnTimKiem.Enabled = true;
            // 
            this.txtTenVaiTro.Focus();
        }

        private void btnXoa4_Click(object sender, EventArgs e)
        {
            try
            {
                int r = dgvVaiTro.CurrentCell.RowIndex;
                string strMa = dgvVaiTro.Rows[r].Cells[0].Value.ToString();
                DialogResult traloi;
                traloi = MessageBox.Show("Chắc xóa mẫu tin này không?", "Trả lời",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                string err = "";
                if (traloi == DialogResult.Yes)
                {
                    bool f = dbVT.XoaVaiTro(ref err, strMa);
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
            catch (SqlException)
            {
                MessageBox.Show("Không xóa được. Lỗi rồi!!!");
            }
        }

        private void btnTaiLai4_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnThoat4_Click(object sender, EventArgs e)
        {
            DialogResult traloi;
            traloi = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK)
                this.Close();
        }

        private void btnHuy4_Click(object sender, EventArgs e)
        {
            this.txtMaVaiTro.ResetText();
            this.txtTenVaiTro.ResetText();

            btnThem4.Enabled = true;
            btnSua4.Enabled = true;
            btnXoa4.Enabled = true;
            btnThoat4.Enabled = true;

            btnLuu4.Enabled = false;
            btnHuy4.Enabled = false;
            pnTimKiem.Enabled = false;
        }

        private void btnLuu4_Click(object sender, EventArgs e)
        {
            bool f;
            if (Them)
            {
                string err = "";
                try
                {
                    f = dbVT.ThemVaiTro(ref err, txtMaVaiTro.Text,
                        txtTenVaiTro.Text);
                    if (f)
                    {
                        // Load lại dữ liệu trên DataGridView 
                        LoadData();
                        // Thông báo 
                        MessageBox.Show("Đã thêm xong!");
                    }
                    else
                    {
                        MessageBox.Show("Thêm không thàng công!\n\r" + "Lỗi:" + err);
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
            else
            {
                string err = "";
                try
                {
                    f = dbVT.SuaVaiTro(ref err, txtMaVaiTro.Text,
                        txtTenVaiTro.Text);
                    if (f)
                    {
                        // Load lại dữ liệu trên DataGridView 
                        LoadData();
                        // Thông báo 
                        MessageBox.Show("Đã cập nhật xong!");
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật không thành công!\n\r" + "Lỗi:" + err);
                    }
                }
                catch (SqlException ex)
                {

                    MessageBox.Show("Lỗi: " + ex.Message);
                }

            }
        }

        private void dgvVaiTro_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvVaiTro.CurrentRow == null)
            {
                MessageBox.Show("Dữ liệu trong DataGridView trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // Thứ tự dòng hiện hành
            int r = dgvVaiTro.CurrentCell.RowIndex;
            // Chuyển thông tin lên Panel
            txtMaVaiTro.Text =
                dgvVaiTro.Rows[r].Cells[0].Value.ToString();
            txtTenVaiTro.Text =
                dgvVaiTro.Rows[r].Cells[1].Value.ToString();
            btnXoa4.Enabled = true;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            string err = "";
            bool f;
            try
            {
                f = dbTKNV.DangKyTK_NhanVien(ref err, txtMaNhanVien2.Text, txtTenDangNhap.Text, txtMatKhau.Text);
                if (f)
                {
                    LoadData();
                    MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Đăng ký không thành công!\n\r" + txtMaNhanVien2.Text +
                        txtTenDangNhap.Text + txtMatKhau.Text + "Lỗi: " + err, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Đăng ký không thành công!\n\r" + "Lỗi: " + ex.Message, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {
            int r = dgvTK_NhanVien.CurrentCell.RowIndex;
            DialogResult traloi;
            traloi = MessageBox.Show("Bạn có chắc muốn xóa tài khoản này không!", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK)
            {
                try
                {
                    string err = "";
                    bool f = dbTKNV.XoaTK_NhanVien(ref err, dgvTK_NhanVien.Rows[r].Cells["TenDangNhap"].Value.ToString());
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

        private void pbThoat_Click(object sender, EventArgs e)
        {
            DialogResult traloi;
            traloi = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK)
                this.Close();
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

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void FrmNhanVien_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                FormBorderStyle = FormBorderStyle.None;
            else
                FormBorderStyle = FormBorderStyle.Sizable;
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            dgvDanhSachNhanVien.Columns["NgaySinh2"].DefaultCellStyle.Format = "dd/MM/yyyy";
            // Vận chuyển dữ liệu vào DataTable
            dtNhanVien = new DataTable();
            dtNhanVien.Clear();
            dtNhanVien = dbNV.HienThiDanhSachNhanVien().Tables[0];
            dgvDanhSachNhanVien.DataSource = dtNhanVien;
            dgvDanhSachNhanVien.AutoResizeColumns();
        }
    }
}
