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
    public partial class FrmMay : Form
    {
        SqlDataAdapter da=null;
        SqlConnection conn = null;
        DataTable dtMAY = null;
        DataTable dtPHONG = null;
       
        // Khai báo biến kiểm tra việc Thêm hay Sửa dữ liệu 
        bool Them;
        DBMay dbMay;
        DBPhong dbPhong;
        
        public FrmMay()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            dbMay = new DBMay();
            dbPhong = new DBPhong();
           
        }

        private void FrmMay_Load(object sender, EventArgs e)
        {
            LoadData();
        }
       
        void LoadData()
        {

            try
            {
                dtPHONG = new DataTable();
                dtPHONG.Clear();
                dtPHONG = dbPhong.LayPhong().Tables[0];
                // Đưa dữ liệu lên ComboBox
                MaPhong.DataSource = dtPHONG;
                MaPhong.DisplayMember = "TenPhong";
                MaPhong.ValueMember = "MaPhong";
                dtMAY = new DataTable();
                dtMAY.Clear();
                dtMAY = dbMay.LayMay().Tables[0];
                dgvMay.DataSource = dtMAY;               
                
                
                // Thay đổi độ rộng cột 
                dgvMay.AutoResizeColumns();
                this.txtMaMay.ResetText();

                this.txtTrgth.ResetText();
                this.txtGT.ResetText();
                this.txtGhiChu.ResetText();
                // Không cho thao tác trên các nút Lưu / Hủy
                this.btnSave.Enabled = false;
                this.btnCancel.Enabled = false;
                this.panel.Enabled = false;
                // Cho thao tác trên các nút Thêm / Sửa / Xóa / Thoát
                this.btnAdd.Enabled = true;
                this.btnChange.Enabled = true;
                this.btnDelete.Enabled = true;
                this.btnExit.Enabled = true;

                //Tab tìm kiếm máy
                // Đưa dữ liệu lên ComboBox
                MaPhong2.DataSource = dtPHONG;
                MaPhong2.DisplayMember = "TenPhong";
                MaPhong2.ValueMember = "MaPhong";

                cboPhong2.DataSource = dtPHONG;
                cboPhong2.DisplayMember = "TenPhong";
                cboPhong2.ValueMember = "MaPhong";
              
                dgvTimKiemMay.DataSource = dtMAY;
                dgvTimKiemMay.AutoResizeColumns();

                txtMaMay2.ResetText();
                cboPhong2.SelectedItem = null;
                txtTrangThai2.ResetText();
                txtGiaTien2.ResetText();
                txtGhiChu2.ResetText();
                checkBoxTinhTrang.Checked = false;
                checkBoxBaoTri.Checked = false;

                // Thay đổi độ rộng cột 
                dgvMay.AutoResizeColumns();
                this.txtMaMay.ResetText();
                dgvMay_CellContentClick(null, null);
            }
            catch (SqlException)
            {
                MessageBox.Show("Không lấy được nội dung trong table MAY.Lỗi rồi!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void dgvMay_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMay.CurrentRow == null)
            {
                MessageBox.Show("Dữ liệu trong DataGridView trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Thứ tự dòng hiện hành 
            this.cbbMaPhong.DataSource = dtPHONG;
            this.cbbMaPhong.DisplayMember = "TenPhong";
            this.cbbMaPhong.ValueMember = "MaPhong";
            // Thứ tự dòng hiện hành 
            int r = dgvMay.CurrentCell.RowIndex;
            // Chuyển thông tin lên panel 
            this.txtMaMay.Text =
                dgvMay.Rows[r].Cells[0].Value.ToString();
            this.cbbMaPhong.SelectedValue =
                dgvMay.Rows[r].Cells[1].Value.ToString();
            if (Them == false)
            {
                txtTrgth.Text = dgvMay.Rows[r].Cells[2].Value.ToString();
            }
            else
            {
                if (dgvMay.Rows[r].Cells[2].Value.ToString() == "True")
                    txtTrgth.Text = "Đang được sử dụng";
                else
                    txtTrgth.Text = "Không được sử dụng";
            }
            this.txtGT.Text =
                dgvMay.Rows[r].Cells[3].Value.ToString();
            this.txtGhiChu.Text =
                dgvMay.Rows[r].Cells[4].Value.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Kich hoạt biến Them
            Them = true;
            txtMaMay.Text = "";
            txtMaMay.ResetText();

            // Cho thao tác trên các nút Lưu / Hủy / Panel
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            panel.Enabled = true;
            // Không cho thao tác trên các nút Thêm / Xóa / Thoát
            btnAdd.Enabled = false;
            btnChange.Enabled = false;
            btnDelete.Enabled = false;
            btnExit.Enabled = false;
            // Đưa dữ liệu lên ComboBox
            cbbMaPhong.DataSource = dtPHONG;
            cbbMaPhong.DisplayMember = "TenPhong";
            cbbMaPhong.ValueMember = "MaPhong";
            // Đưa con trỏ đến TextField txtMaKH
            txtMaMay.Focus();
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            Them = false;
            this.panel.Enabled = true;
            dgvMay_CellContentClick(null, null);
            this.btnSave.Enabled = true;
            this.btnCancel.Enabled = true;
            this.panel.Enabled = true;
            this.btnAdd.Enabled = false;
            this.btnChange.Enabled = false;
            this.btnDelete.Enabled = false;
            this.btnExit.Enabled = false;
            // Đưa dữ liệu lên ComboBox
            this.cbbMaPhong.DataSource = dtPHONG;
            this.cbbMaPhong.DisplayMember = "MaPhong";
            this.cbbMaPhong.ValueMember = "MaPhong";
            // Đưa con trỏ đến TextField txtMaKH
            this.txtMaMay.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {

                // Lấy thứ tự record hiện hành 
                int r = dgvMay.CurrentCell.RowIndex;
                // Lấy MaKH của record hiện hành 
                string strMaMay =
                dgvMay.Rows[r].Cells[0].Value.ToString();
                // Viết câu lệnh SQL 
                // Hiện thông báo xác nhận việc xóa mẫu tin 
                // Khai báo biến traloi 
                DialogResult traloi;
                // Hiện hộp thoại hỏi đáp 
                traloi = MessageBox.Show("Chắc xóa máy này không?", "Trả lời",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                // Kiểm tra có nhắp chọn nút Ok không? 
                string err = "";
                if (traloi == DialogResult.Yes)
                {

                    // Thực hiện câu lệnh SQL 
                    bool f = dbMay.XoaMay(ref err, txtMaMay.Text);
                    if (f)
                    {
                        // Cập nhật lại DataGridView 
                        LoadData();
                        // Thông báo 
                        MessageBox.Show("Đã xóa xong!");
                    }
                    else
                    {
                        MessageBox.Show("Không xóa được!\n\r" + "Lỗi:" + err);
                    }
                }
                else
                {
                    // Thông báo 
                    MessageBox.Show("Không thực hiện việc xóa mẫu tin!");
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Không xóa được. Lỗi rồi!!!");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool f = false;
            if (Them)
            {
                string err = "";
                try
                {

                    f = dbMay.ThemMAY(ref err, txtMaMay.Text, cbbMaPhong.SelectedValue.ToString(), txtTrgth.Text,
                        txtGT.Text, txtGhiChu.Text);
                    if (f)
                    {
                        // Load lại dữ liệu trên DataGridView 
                        LoadData();
                        // Thông báo 
                        MessageBox.Show("Đã thêm xong!");
                    }
                    else
                    {
                        MessageBox.Show("Lỗi:" + err);
                    }
                }
                catch (SqlException)
                {
                    MessageBox.Show("Không thêm được. Lỗi rồi!");
                }
            }
            else // Sửa MAY
            {
                string err = "";
                try
                {
                    f = dbMay.CapNhatMay(ref err, txtMaMay.Text, cbbMaPhong.SelectedValue.ToString(), txtTrgth.Text,
                        txtGT.Text, txtGhiChu.Text);
                    if (f)
                    {
                        // Load lại dữ liệu trên DataGridView 
                        LoadData();
                        // Thông báo 
                        MessageBox.Show("Đã cập nhật xong!");
                    }
                    else
                    {
                        MessageBox.Show("Lỗi:" + err);
                    }
                }
                catch (SqlException)
                {
                    MessageBox.Show("Không cập nhật được. Lỗi rồi!");
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Xóa trống các đối tượng trong Panel
            this.txtMaMay.ResetText();
            this.cbbMaPhong.Text = "";
            this.txtGT.ResetText();
            this.txtTrgth.ResetText();
            this.txtGhiChu.ResetText();
            // Cho thao tác trên các nút Thêm / Sửa / Xóa / Thoát
            this.btnAdd.Enabled = true;
            this.btnChange.Enabled = true;
            this.btnDelete.Enabled = true;
            this.btnExit.Enabled = true;
            // Không cho thao tác trên các nút Lưu / Hủy / Panel
            this.btnSave.Enabled = false;
            this.btnCancel.Enabled = false;
            this.panel.Enabled = false;

            dgvMay_CellContentClick(null, null);
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadData();

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            // Khai báo biến traloi
            DialogResult traloi;
            // Hiện hộp thoại hỏi đáp
            traloi = MessageBox.Show("Chắc không?", "Trả lời",
            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            // Kiểm tra có nhắp chọn nút Ok không?
            if (traloi == DialogResult.OK) this.Close();
        }
        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {

            string MaMay = null;
            if (txtMaMay2.Text != "")
                MaMay = txtMaMay2.Text;
            string Phong = null;
            if (cboPhong2.SelectedValue != null)
            {
                Phong = cboPhong2.SelectedValue.ToString();
                Console.WriteLine(Phong);
            }
            int? GiaTien = null;
            if (txtGiaTien2.Text != "")
            {
                try { GiaTien = Convert.ToInt32(txtGiaTien2.Text);
                    Console.WriteLine(GiaTien);
                }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi: Giá tiền phải là số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            int? TrangThai = null;
            if (txtTrangThai2.Text != "")
            {
                try { TrangThai = Convert.ToInt32(txtTrangThai2.Text); }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi: Trạng thái phải là số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            string GhiChu = null;
            if (txtGhiChu2.Text != "")
                GhiChu = txtGhiChu2.Text;
            int? BaoTri = null;
            if (checkBoxBaoTri.Checked ==  true) 
            {
                BaoTri = 1;
            }
            if (checkBoxTinhTrang.Checked == true && TrangThai == null) 
            {
                TrangThai = 1;
            }
            string errorMessage;
            DataSet ds = dbMay.TimKiemMay(MaMay, Phong, TrangThai, GiaTien, GhiChu, BaoTri, out errorMessage);
            if (!String.IsNullOrEmpty(errorMessage)) 
            {
                MessageBox.Show("Lỗi: " + errorMessage, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                dgvTimKiemMay.DataSource = dt;
                txtMaMay2.ResetText();
                cboPhong2.SelectedItem = null;
                txtTrangThai2.ResetText();
                txtGiaTien2.ResetText();
                txtGhiChu2.ResetText();
            }    
        }

        private void txtTrangThai2_TextChanged(object sender, EventArgs e)
        {
            checkBoxTinhTrang.Enabled = false;
            if (txtTrangThai2.Text == "")
                checkBoxTinhTrang.Enabled = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string MaMay = null;
            if (txtMaMay2.Text != "")
                MaMay = txtMaMay2.Text;
            string Phong = null;    
            if (cboPhong2.SelectedValue != null)
            {
                Phong = cboPhong2.SelectedValue.ToString();
                Console.WriteLine(Phong);
            }
            int? GiaTien = null;
            if (txtGiaTien2.Text != "")
            {
                try
                {
                    GiaTien = Convert.ToInt32(txtGiaTien2.Text);
                    Console.WriteLine(GiaTien);
                }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi: Giá tiền phải là số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            int? TrangThai = null;
            if (txtTrangThai2.Text != "")
            {
                try { TrangThai = Convert.ToInt32(txtTrangThai2.Text); }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi: Trạng thái phải là số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            string GhiChu = null;
            if (txtGhiChu2.Text != "")
                GhiChu = txtGhiChu2.Text;
            int? BaoTri = null;
            if (checkBoxBaoTri.Checked == true)
            {
                BaoTri = 1;
            }
            if (checkBoxTinhTrang.Checked == true && TrangThai == null)
            {
                TrangThai = 1;
            }
            string errorMessage;
            DataSet ds = dbMay.TimKiemMay(MaMay, Phong, TrangThai, GiaTien, GhiChu, BaoTri, out errorMessage);
            if (!String.IsNullOrEmpty(errorMessage))
            {
                MessageBox.Show("Lỗi: " + errorMessage, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                dgvTimKiemMay.DataSource = dt;
                txtMaMay2.ResetText();
                cboPhong2.SelectedItem = null;
                txtTrangThai2.ResetText();
                txtGiaTien2.ResetText();
                txtGhiChu2.ResetText();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvTimKiemMay_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void pbThoat_Click(object sender, EventArgs e)
        {
            DialogResult traloi;
            traloi = MessageBox.Show("Chắc không?", "Trả lời",
            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK) this.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void FrmMay_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                FormBorderStyle = FormBorderStyle.None;
            else
                FormBorderStyle = FormBorderStyle.Sizable;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if ((cbMayBat.Checked && cbMayTat.Checked) || (!cbMayBat.Checked && !cbMayTat.Checked))
            {
                dtMAY = new DataTable();
                dtMAY.Clear();
                dtMAY = dbMay.HienThiThongTinMay().Tables[0];
                dgvThongtinMay.DataSource = dtMAY;
                dgvThongtinMay.AutoResizeColumns();
            }
              
            else if(cbMayBat.Checked)
                {
                    dtMAY = new DataTable();
                    dtMAY.Clear();
                    dtMAY = dbMay.HienThiThongTinMayBat().Tables[0];
                    dgvThongtinMay.DataSource = dtMAY;
                    dgvThongtinMay.AutoResizeColumns();
                }else if (cbMayTat.Checked)
                    {
                        dtMAY = new DataTable();
                        dtMAY.Clear();
                        dtMAY = dbMay.HienThiThongTinMayTat().Tables[0];
                        dgvThongtinMay.DataSource = dtMAY;
                        dgvThongtinMay.AutoResizeColumns();
                    }

        }
    }
}
