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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace QLdailynet
{
    public partial class FrmDiemDanh : Form
    {
        bool Them;
        DataTable dtLichLamViec = null;
        DBNgayThangNam dbLLV;
        DataTable dtDiemDanh = null;
        DBDiemDanh dbDD;
        public FrmDiemDanh()
        {
            InitializeComponent();

            this.MinimumSize = this.Size;
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;

            dbLLV = new DBNgayThangNam();
            dbDD = new DBDiemDanh();
        }


        public void LoadData()
        {
            dtLichLamViec = dbLLV.LayLichLamViec().Tables[0];
            dtDiemDanh = dbDD.LayDiemDanh().Tables[0];

            dgvLichLamViec.Columns["NgayThangNam"].DefaultCellStyle.Format = "yyyy-MM-dd";

            dgvLichLamViec.DataSource = dtLichLamViec;
            dgvLichLamViec.AutoResizeColumns();
            dgvDiemDanh.DataSource = dtDiemDanh;
            dgvDiemDanh.AutoResizeColumns();

            txtMaNV.ResetText();
            txtGhiChu.ResetText();

            btnLuu.Enabled = false;
            btnHuy.Enabled = false;

            btnReload.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnThoat.Enabled = true;

            pnchitietphieuchi.Enabled = false;
            pnsophieu.Enabled = false;

            dgvLichLamViec_CellClick(null, null);
            dgvDiemDanh_CellClick(null, null);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(rbLichLamViec.Checked)
            {
                Them = true;
                dtpdatetime.MinDate = DateTime.MinValue;

                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnThoat.Enabled = false;

                btnLuu.Enabled = true;
                btnHuy.Enabled = true;
                btnReload.Enabled = true;
                pnsophieu.Enabled = true;
                pnchitietphieuchi.Enabled = false;
            }
            else if(rbDiemDanh.Checked)
            {
                Them = true;

                txtMaNV.ResetText();
                txtGhiChu.ResetText();
                dtpNgayThangNam.MinDate = DateTime.MinValue;

                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnThoat.Enabled = false;

                btnLuu.Enabled = true;
                btnHuy.Enabled = true;
                btnReload.Enabled = true;
                pnchitietphieuchi.Enabled = true;
                pnsophieu.Enabled = false;

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

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult traloi;
            traloi = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK)
                this.Close();
        }

        private void FrmDiemDanh_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvLichLamViec_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (cbLoc.Checked)
            {
                if (dgvLichLamViec.CurrentRow == null)
                {
                    MessageBox.Show("Dữ liệu trong DataGridView trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int r = dgvLichLamViec.CurrentCell.RowIndex;
                if (dgvLichLamViec.Rows[r].Cells[0].Value != null)
                {
                    DateTime ThoiGianGiaoDich;
                    if (DateTime.TryParse(dgvLichLamViec.Rows[r].Cells[0].Value.ToString(), out ThoiGianGiaoDich))
                    {
                        dtpdatetime.Value = ThoiGianGiaoDich;
                    }
                }
                string temp = dtpdatetime.Value.ToString("yyyy-MM-dd");
                DataSet dsGiaoDich = dbDD.LayDiemDanhNgayThangNam(temp);
                if (dsGiaoDich != null && dsGiaoDich.Tables.Count > 0)
                {
                    dgvDiemDanh.DataSource = dsGiaoDich.Tables[0];
                    dgvDiemDanh.AutoResizeColumns();
                }
                dgvDiemDanh.AutoResizeColumns();
                dgvDiemDanh_CellClick(null, null);

            }
            else
            {
                if (dgvLichLamViec.CurrentRow == null)
                {
                    MessageBox.Show("Dữ liệu trong DataGridView trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int r = dgvLichLamViec.CurrentCell.RowIndex;
                if (dgvLichLamViec.Rows[r].Cells[0].Value != null)
                {
                    DateTime ThoiGianGiaoDich;
                    if (DateTime.TryParse(dgvLichLamViec.Rows[r].Cells[0].Value.ToString(), out ThoiGianGiaoDich))
                    {
                        dtpdatetime.Value = ThoiGianGiaoDich;
                    }
                }

            }
        }

        private void dgvDiemDanh_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDiemDanh.CurrentRow == null)
            {
                MessageBox.Show("Dữ liệu trong DataGridView trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int r = dgvDiemDanh.CurrentCell.RowIndex;
            txtMaNV.Text = dgvDiemDanh.Rows[r].Cells[0].Value.ToString();

            if (dgvDiemDanh.Rows[r].Cells[1].Value != null)
            {
                DateTime ThoiGian;
                if (DateTime.TryParseExact(dgvDiemDanh.Rows[r].Cells[1].Value.ToString(), "hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out ThoiGian))
                {
                    dtpGioLam.Value = ThoiGian;
                }
            }

            if (dgvDiemDanh.Rows[r].Cells[2].Value != null)
            {
                DateTime ThoiGian2;
                if (DateTime.TryParseExact(dgvDiemDanh.Rows[r].Cells[2].Value.ToString(), "hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out ThoiGian2))
                {
                    dtpGioNghi.Value = ThoiGian2;
                }
            }

            if (dgvDiemDanh.Rows[r].Cells[3].Value != null)
            {
                DateTime ThoiGian3;
                if (DateTime.TryParseExact(dgvDiemDanh.Rows[r].Cells[3].Value.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out ThoiGian3))
                {
                    dtpNgayThangNam.Value = ThoiGian3;
                }
            }

            txtGhiChu.Text = dgvDiemDanh.Rows[r].Cells[4].Value.ToString();
        }


        private void btnSua_Click(object sender, EventArgs e)
        {
            if (rbLichLamViec.Checked)
            {
                Them = false;

                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnThoat.Enabled = false;

                btnLuu.Enabled = true;
                btnHuy.Enabled = true;
                btnReload.Enabled = true;
                pnsophieu.Enabled = true;
                pnchitietphieuchi.Enabled = false;
            }
            else if (rbDiemDanh.Checked)
            {
                Them = false;

                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnThoat.Enabled = false;

                btnLuu.Enabled = true;
                btnHuy.Enabled = true;
                btnReload.Enabled = true;
                pnsophieu.Enabled = false;
                pnchitietphieuchi.Enabled = true;

            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (rbLichLamViec.Checked)
            {
                try
                {
                    int r = dgvLichLamViec.CurrentCell.RowIndex;
                    string str = dgvLichLamViec.Rows[r].Cells[0].Value.ToString();
                    DialogResult traloi;
                    traloi = MessageBox.Show("Chắc xóa mẫu tin này không?", "Trả lời",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    string err = "";
                    if (traloi == DialogResult.Yes)
                    {
                        bool f = dbLLV.XoaLichLamViec(ref err, str);
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
                    MessageBox.Show("Không xóa được. Lỗi : " + ex.Message);
                }
            }
            else if (rbDiemDanh.Checked)
            {
                try
                {
                    int r = dgvDiemDanh.CurrentCell.RowIndex;
                    string str = dgvDiemDanh.Rows[r].Cells[0].Value.ToString();
                    string str1 = dgvDiemDanh.Rows[r].Cells[1].Value.ToString();
                    string str2 = dgvDiemDanh.Rows[r].Cells[2].Value.ToString();
                    string str3 = dgvDiemDanh.Rows[r].Cells[3].Value.ToString();
                    string str4 = dgvDiemDanh.Rows[r].Cells[4].Value.ToString();
                    DialogResult traloi;
                    traloi = MessageBox.Show("Chắc xóa mẫu tin này không?", "Trả lời",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    string err = "";
                    if (traloi == DialogResult.Yes)
                    {
                        bool f = dbDD.XoaDiemDanh(ref err, str, str1, str2, str3, str4);
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
                    MessageBox.Show("Không xóa được. Lỗi : " + ex.Message);
                }
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (rbLichLamViec.Checked)
            {

                btnThem.Enabled = true;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnThoat.Enabled = true;

                btnLuu.Enabled = false;
                btnHuy.Enabled = false;
                pnsophieu.Enabled = false;
            }
            else
           if (rbDiemDanh.Checked)
            {
                this.txtMaNV.ResetText();
                this.txtMaGiaoDich.Enabled = false;
                dtpNgayThangNam.MinDate = DateTime.MinValue;

                btnThem.Enabled = true;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnThoat.Enabled = true;

                btnLuu.Enabled = false;
                btnHuy.Enabled = false;
                pnchitietphieuchi.Enabled = false;
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (rbLichLamViec.Checked)
            {
                bool f = false;
                if (Them)
                {
                    string err = "";
                    try
                    {
                        f = dbLLV.ThemLichLamViec(ref err,dtpdatetime.Value.ToString("yyyy-MM-dd"));
                        if (f)
                        {

                            LoadData();

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
                        f = dbLLV.SuaLichLamViec(ref err, dtpdatetime.Value.ToString("yyyy-MM-dd"));

                        if (f)
                        {
                            LoadData();
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
            else
           if (rbDiemDanh.Checked)
            {
                bool f = false;
                if (Them)
                {
                    string err = "";
                    try
                    {

                        f = dbDD.ThemDiemDanh(ref err, txtMaNV.Text, dtpGioNghi.Value.ToString("HH:mm:ss")
                            , dtpNgayThangNam.Value.ToString("yyyy-MM-dd"), txtGhiChu.Text);
                        if (f)
                        {

                            LoadData();

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
                        f = dbDD.SuaDiemDanh(ref err, txtMaNV.Text, dtpGioLam.Value.ToString("HH:mm:ss"), dtpGioNghi.Value.ToString("HH:mm:ss")
                             , dtpNgayThangNam.Value.ToString("yyyy-MM-dd"), txtGhiChu.Text);
                        if (f)
                        {
                            LoadData();
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
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void FrmDiemDanh_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                FormBorderStyle = FormBorderStyle.None;
            else
                FormBorderStyle = FormBorderStyle.Sizable;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            string temp = dtpdatetime1.Value.ToString("yyyy-MM-dd");
            dtDiemDanh.Clear();
            dtDiemDanh = dbDD.LayDiemDanhNgayThangNam(temp).Tables[0];
            //dgvDanhSachLamViec.Columns["NgayThangNam"].DefaultCellStyle.Format = "yyyy-MM-dd";
            dgvDanhSachLamViec.DataSource = dtDiemDanh;
            dgvDanhSachLamViec.AutoResizeColumns();
        }
    }
}
