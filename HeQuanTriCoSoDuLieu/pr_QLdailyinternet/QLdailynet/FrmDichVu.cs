using BAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLdailynet
{
    public partial class FrmDichVu : Form
    {
        DataTable dtDICHVU = null;
        DataTable dtLOAIDICHVU = null;
        DataTable dtTimKiemDV;
        DataTable dtDatDichVu;
        bool Them;
        DBDichVu dbDichvu;
        DBDatDichVu dbDDV;
        DBLoaiDichVu dbLoaiDichVu;
        public FrmDichVu()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            dbDichvu = new DBDichVu();
            dbLoaiDichVu = new DBLoaiDichVu();
            dbDDV = new DBDatDichVu();
        }
        void LoadData()
        {
            try
            {
                dtDatDichVu = new DataTable();
                dtDatDichVu.Clone();
                dtDatDichVu = dbDDV.LayDatDichVu().Tables[0];
                dgvDatDichVu.DataSource = dtDatDichVu;

                dtLOAIDICHVU = new DataTable();
                dtLOAIDICHVU.Clear();
                dtLOAIDICHVU = dbLoaiDichVu.LayLoai().Tables[0];
                // Đưa dữ liệu lên ComboBox
                MaLoai.DataSource = dtLOAIDICHVU;
                MaLoai.DisplayMember = "MaLoai";
                MaLoai.ValueMember = "MaLoai";

                dtDICHVU = new DataTable();
                dtDICHVU.Clear();
                dtDICHVU = dbDichvu.LayDichVu().Tables[0];
                dgvDV.DataSource = dtDICHVU;
                // Đưa dữ liệu lên DataGridView 
                dgvDV.DataSource = dtDICHVU;
                dgvDV.AutoResizeColumns();
                this.txtMaDV.ResetText();
                this.txtTenDV.ResetText();
                this.txtGT.ResetText();
                this.txtSoLuong.ResetText();
                // Không cho thao tác trên các nút Lưu / Hủy
                this.btnSave.Enabled = false;
                this.btnCancel.Enabled = false;
                this.panel.Enabled = false;
                // Cho thao tác trên các nút Thêm / Sửa / Xóa / Thoát
                this.btnAdd.Enabled = true;
                this.btnChange.Enabled = true;
                this.btnDelete.Enabled = true;
                this.btnExit.Enabled = true;
                //tab Tim kiem
                dgvTimKiemDv.DataSource = dtDICHVU;
                txtMaDV1.ResetText();
                cbbMaLoai1.SelectedValue = "";
                txtTdv1.ResetText();
                txtGt1.ResetText();
                txtSoluong1.ResetText();
                cbbMaLoai1.DataSource = dtLOAIDICHVU;
                cbbMaLoai1.DisplayMember = "MaLoai";
                cbbMaLoai1.ValueMember = "MaLoai";
                cbbMaLoai1.SelectedValue = "";
                dgvDV_CellClick(null, null);

                this.txtMaLoai.ResetText();
                this.txtTLDV.ResetText();

                // Không cho thao tác trên các nút Lưu / Hủy
                this.btnSave1.Enabled = false;
                this.btnCancel1.Enabled = false;
                this.panel4.Enabled = false;
                // Cho thao tác trên các nút Thêm / Sửa / Xóa / Thoát
                this.btnAdd1.Enabled = true;
                this.btnChange1.Enabled = true;
                this.btnDelete1.Enabled = true;
                this.btnExit1.Enabled = true;
                dgvLDV.DataSource = dtLOAIDICHVU;
                dgvLDV_CellClick(null, null);
            }
            catch (SqlException)
            {
                MessageBox.Show("Không lấy được nội dung trong DataBase. Lỗi rồi!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void dgvDV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDV.CurrentRow == null)
            {
                MessageBox.Show("Dữ liệu trong DataGridView trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Thứ tự dòng hiện hành 
            this.cbbMaLoai.DataSource = dtLOAIDICHVU;
            this.cbbMaLoai.DisplayMember = "MaLoai";
            this.cbbMaLoai.ValueMember = "MaLoai";
            // Thứ tự dòng hiện hành 
            int r = dgvDV.CurrentCell.RowIndex;
            // Chuyển thông tin lên panel 
            this.txtMaDV.Text =
            dgvDV.Rows[r].Cells[0].Value.ToString();
            this.cbbMaLoai.SelectedValue =
            dgvDV.Rows[r].Cells[1].Value.ToString();
            this.txtTenDV.Text =
            dgvDV.Rows[r].Cells[2].Value.ToString();
            this.txtGT.Text =
            dgvDV.Rows[r].Cells[3].Value.ToString();
            this.txtSoLuong.Text =
            dgvDV.Rows[r].Cells[4].Value.ToString();
            if (!Convert.IsDBNull(dgvDV.Rows[r].Cells[5].Value))
            {
                byte[] imagePath = (byte[])dgvDV.Rows[r].Cells[5].Value;
                if (imagePath != null)
                {
                    MemoryStream ms = new MemoryStream(imagePath);
                    Image img = Image.FromStream(ms);
                    pictureDichVu.Image = img;
                }
            }
            else
            {
                pictureDichVu.Image = null; // nếu không có ảnh thì hiển thị hình ảnh mặc định hoặc xóa hình ảnh hiện tại
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Them = true;
            txtMaDV.Text = "";
            cbbMaLoai.Text = "";
            txtSoLuong.Text = "";
            txtTenDV.Text = "";
            txtGT.Text = "";

            // Cho thao tác trên các nút Lưu / Hủy / Panel
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            panel.Enabled = true;
            // Không cho thao tác trên các nút Thêm / Xóa / Thoát
            btnAdd.Enabled = false;
            btnChange.Enabled = false;
            btnDelete.Enabled = false;
            btnExit.Enabled = false;
            this.cbbMaLoai.DataSource = dtLOAIDICHVU;
            this.cbbMaLoai.DisplayMember = "MaLoai";
            this.cbbMaLoai.ValueMember = "MaLoai";
            txtMaDV.Focus();
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            Them = false;
            this.panel.Enabled = true;
            this.btnSave.Enabled = true;
            this.btnCancel.Enabled = true;
            this.panel.Enabled = true;
            this.btnAdd.Enabled = false;
            this.btnChange.Enabled = false;
            this.btnDelete.Enabled = false;
            this.btnExit.Enabled = false;
            this.cbbMaLoai.DataSource = dtLOAIDICHVU;
            this.cbbMaLoai.DisplayMember = "TenLoaiDichVu";
            this.cbbMaLoai.ValueMember = "MaLoai";
            txtMaDV.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int r = dgvDV.CurrentCell.RowIndex;
                string strMaMay =
                dgvDV.Rows[r].Cells[0].Value.ToString();
                DialogResult traloi;
                traloi = MessageBox.Show("Chắc xóa máy này không?", "Trả lời",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                string err = "";
                if (traloi == DialogResult.Yes)
                {
                    bool f = dbDichvu.XoaDichVu(ref err, txtMaDV.Text);
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool f = false;
            if (Them)
            {
                string imagepath = "";
                if (txtfilePath.Text != "")
                {
                    imagepath = txtfilePath.Text;
                }
                byte[] picbyte = null;
                if (imagepath != "")
                    picbyte = chuyenHinhAnhSangByte(imagepath);
                string err = "";
                try
                {

                    f = dbDichvu.ThemDichVu(ref err, txtMaDV.Text, cbbMaLoai.SelectedValue.ToString(), txtTenDV.Text,
                        txtGT.Text, txtSoLuong.Text, picbyte);
                    if (f)
                    {

                        LoadData();

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
            else
            {
                
                string imagepath = "";
                if (txtfilePath.Text != "")
                {
                    imagepath = txtfilePath.Text;
                }
                byte[] picbyte = null;
                if (imagepath != "")
                    picbyte = chuyenHinhAnhSangByte(imagepath);
                string err = "";
                try
                {
                    f = dbDichvu.CapNhatDichVu(ref err, txtMaDV.Text, cbbMaLoai.SelectedValue.ToString(), txtTenDV.Text,
                        txtGT.Text, txtSoLuong.Text, picbyte);
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
        private byte[] chuyenHinhAnhSangByte(string FilePath)
        {
            FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            byte[] picbyte = new byte[fs.Length];
            fs.Read(picbyte, 0, Convert.ToInt32(fs.Length));
            fs.Close();
            return picbyte;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.txtMaDV.ResetText();
            this.txtGT.ResetText();
            this.txtTenDV.ResetText();
            this.txtSoLuong.ResetText();
        
            this.btnAdd.Enabled = true;
            this.btnChange.Enabled = true;
            this.btnDelete.Enabled = true;
            this.btnExit.Enabled = true;
           
            this.btnSave.Enabled = false;
            this.btnCancel.Enabled = false;
            this.panel.Enabled = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult traloi;
            traloi = MessageBox.Show("Chắc không?", "Trả lời",
            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK) Application.Exit();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void FrmDichVu_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'pROJECT_INTERNETDataSet4.LOAIDICHVU' table. You can move, or remove it, as needed.
            this.lOAIDICHVUTableAdapter.Fill(this.pROJECT_INTERNETDataSet4.LOAIDICHVU);
            LoadData();
        }



        private void pbThoat_Click_1(object sender, EventArgs e)
        {
            DialogResult traloi;
            traloi = MessageBox.Show("Chắc không?", "Trả lời",
            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK) this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void FrmDichVu_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                FormBorderStyle = FormBorderStyle.None;
            else
                FormBorderStyle = FormBorderStyle.Sizable;
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

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string MaDV = null;
            if (txtMaDV1.Text != "")
                MaDV = txtMaDV1.Text;
            string MaLoai = null;
            if (cbbMaLoai1.SelectedValue != null)
            {
                MaLoai = cbbMaLoai1.SelectedValue.ToString();
            }
            string TenDV = null;
            if (txtTdv1.Text != "")
                TenDV = txtMaDV1.Text;
            string Giatien = null;
            if (txtGt1.Text != "")
                Giatien = txtGt1.Text;
            string SoLuong = null;
            if (txtSoluong1.Text != "")
                SoLuong = txtSoluong1.Text;
            string errorMessage;
            DataSet ds = dbDichvu.TimKiemDichVu(MaDV, MaLoai,
                                TenDV, Giatien, SoLuong,
                                out errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                MessageBox.Show("Lỗi: " + errorMessage);
            }
            else
            {
                dtTimKiemDV = ds.Tables[0];
                dgvTimKiemDv.DataSource = dtTimKiemDV;
                txtMaDV1.ResetText();
                cbbMaLoai1.SelectedValue = "";
                txtTdv1.ResetText();
                txtGt1.ResetText();
                txtSoluong1.ResetText();

            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string MaDV = null;
            if (txtMaDV1.Text != "")
                MaDV = txtMaDV1.Text;
            string MaLoai = null;
            if (cbbMaLoai1.SelectedValue != null)
            {
                MaLoai = cbbMaLoai1.SelectedValue.ToString();
            }
            string TenDV = null;
            if (txtTdv1.Text != "")
                TenDV = txtMaDV1.Text;
            string Giatien = null;
            if (txtGt1.Text != "")
                Giatien = txtGt1.Text;
            string SoLuong = null;
            if (txtSoluong1.Text != "")
                SoLuong = txtSoluong1.Text;
            string errorMessage;
            DataSet ds = dbDichvu.TimKiemDichVu(MaDV, MaLoai,
                                TenDV, Giatien, SoLuong,
                                out errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                MessageBox.Show("Lỗi: " + errorMessage);
            }
            else
            {
                dtTimKiemDV = ds.Tables[0];
                dgvTimKiemDv.DataSource = dtTimKiemDV;
                txtMaDV1.ResetText();
                cbbMaLoai1.SelectedValue = "";
                txtTdv1.ResetText();
                txtGt1.ResetText();
                txtSoluong1.ResetText();

            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnAdd1_Click(object sender, EventArgs e)
        {
            Them = true;
            txtMaLoai.Text = "";
            txtMaLoai.ResetText();

            // Cho thao tác trên các nút Lưu / Hủy / Panel
            btnSave1.Enabled = true;
            btnCancel1.Enabled = true;
            panel4.Enabled = true;
            // Không cho thao tác trên các nút Thêm / Xóa / Thoát
            btnAdd1.Enabled = false;
            btnChange1.Enabled = false;
            btnDelete1.Enabled = false;
            btnExit1.Enabled = false;
            txtMaLoai.Focus();
        }

        private void btnChange1_Click(object sender, EventArgs e)
        {
            Them = false;
            this.panel4.Enabled = true;
            dgvLDV_CellClick(null, null);
            this.btnSave1.Enabled = true;
            this.btnCancel1.Enabled = true;
            this.panel4.Enabled = true;
            this.btnAdd1.Enabled = false;
            this.btnChange1.Enabled = false;
            this.btnDelete1.Enabled = false;
            this.btnExit1.Enabled = false;
            txtMaLoai.Focus();
        }

        private void btnDelete1_Click(object sender, EventArgs e)
        {
            try
            {
                int r = dgvLDV.CurrentCell.RowIndex;
                string strMaLoai =
                dgvLDV.Rows[r].Cells[0].Value.ToString();
                DialogResult traloi;
                // Hiện hộp thoại hỏi đáp 
                traloi = MessageBox.Show("Chắc xóa máy này không?", "Trả lời",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                string err = "";
                if (traloi == DialogResult.Yes)
                {
                    bool f = dbLoaiDichVu.XoaLoaiDichVu(ref err, txtMaLoai.Text);
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
                    // Thông báo 
                    MessageBox.Show("Không thực hiện việc xóa mẫu tin!");
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Không xóa được. Lỗi rồi!!!");
            }
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            bool f = false;
            if (Them)
            {
                string err = "";
                try
                {

                    f = dbLoaiDichVu.ThemLoaiDichVu(ref err, txtMaLoai.Text, txtTLDV.Text);
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
                    f = dbLoaiDichVu.CapNhatLoaiDichVu(ref err, txtMaLoai.Text, txtTLDV.Text);
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

        private void btnCancel1_Click(object sender, EventArgs e)
        {
            // Xóa trống các đối tượng trong Panel
            this.txtMaLoai.ResetText();
            this.txtTLDV.ResetText();

            // Cho thao tác trên các nút Thêm / Sửa / Xóa / Thoát
            this.btnAdd1.Enabled = true;
            this.btnChange1.Enabled = true;
            this.btnDelete1.Enabled = true;
            this.btnExit1.Enabled = true;
            // Không cho thao tác trên các nút Lưu / Hủy / Panel
            this.btnSave1.Enabled = false;
            this.btnCancel1.Enabled = false;
            this.panel4.Enabled = false;
        }

        private void btnReload1_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnExit1_Click(object sender, EventArgs e)
        {
            // Khai báo biến traloi
            DialogResult traloi;
            // Hiện hộp thoại hỏi đáp
            traloi = MessageBox.Show("Chắc không?", "Trả lời",
            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            // Kiểm tra có nhắp chọn nút Ok không?
            if (traloi == DialogResult.OK) this.Close();
        }

        private void dgvLDV_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLDV.CurrentRow == null)
            {
                int r = dgvLDV.CurrentCell.RowIndex;
                txtMaLoai.Text = dgvDV.Rows[r].Cells[0].Value.ToString();
                txtTLDV.Text = dgvDV.Rows[r].Cells[1].Value.ToString();
            }
        }

        private void dgvLDV_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvLDV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnChonTep_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JPG files (*.jpg)|*.jpg|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtfilePath.Text = openFileDialog.FileName;
                pictureDichVu.ImageLocation = openFileDialog.FileName;
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
                LoadData();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if(cbDichVu.Checked)
            {
                dtDICHVU = new DataTable();
                dtDICHVU.Clear();
                dtDICHVU = dbDichvu.HienThiDanhSachDichVuVanCon().Tables[0];
                dgvDanhSach.DataSource = dtDICHVU;
                dgvDanhSach.AutoResizeColumns();
            }    
            else
            {
                dtDICHVU = new DataTable();
                dtDICHVU.Clear();
                dtDICHVU = dbDichvu.HienThiDanhSachDichVu().Tables[0];
                dgvDanhSach.DataSource = dtDICHVU;
                dgvDanhSach.AutoResizeColumns();

            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }
    }
}
