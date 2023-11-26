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
    public partial class FrmGiaoDich : Form
    {
        bool Them;
        DataTable dtGiaoDich = null;
        DBGiaoDich dbGD;
        DataTable dtNhanVien = null;
        DBNhanVien dbNV;
        public FrmGiaoDich()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            dbNV = new DBNhanVien();
            dbGD = new DBGiaoDich();
        }
        public void LoadData()
        {
            dtGiaoDich = dbGD.LayGiaoDich().Tables[0];
            dgvGiaoDich.Columns["ThoiGianGD"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";

            dgvGiaoDich.DataSource = dtGiaoDich;
            dgvGiaoDich.AutoResizeColumns();

            this.txtMaGiaoDich.ResetText();
            this.txtmakhachhang.ResetText();
            this.txtmanhanvien.ResetText();
            this.txtsotien.ResetText();
            dtpdatetime.MinDate = DateTime.MinValue;



            btnLuu.Enabled = false;
            btnHuy.Enabled = false;

            btnReLoad.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnThoat.Enabled = true;
            //
            panel1.Enabled = false;
            panel3.Enabled = false;
            dgvGiaoDich_SelectionChanged(null, null);
        }
        private void FrmGiaoDich_Load(object sender, EventArgs e)
        {
            LoadData();
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            Them = true;
            txtMaGiaoDich.Enabled = false;
            this.txtmanhanvien.ResetText();
            this.txtmakhachhang.ResetText();
            this.txtsotien.ResetText();
            dtpdatetime.MinDate = DateTime.MinValue;

            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnThoat.Enabled = false;

            btnLuu.Enabled = true;
            btnHuy.Enabled = true;
            btnReLoad.Enabled = true;
            panel1.Enabled = true; 

            this.txtmanhanvien.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            Them = false;

            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnThoat.Enabled = false;

            btnLuu.Enabled = true;
            btnHuy.Enabled = true;
            btnReLoad.Enabled = true;
            panel1.Enabled = true;
           
            this.txtmanhanvien.Focus();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                int r = dgvGiaoDich.CurrentCell.RowIndex;
                string strMaGD = dgvGiaoDich.Rows[r].Cells[0].Value.ToString();
                DialogResult traloi;
                traloi = MessageBox.Show("Chắc xóa mẫu tin này không?", "Trả lời",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                string err = "";
                if (traloi == DialogResult.Yes)
                {
                    bool f = dbGD.XoaGiaoDich(ref err, strMaGD);
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

        private void btnLuu_Click(object sender, EventArgs e)
        {
            bool f = false;
            if (Them)
            {
                string err = "";
                try
                {
                    f = dbGD.ThemGiaoDich(ref err, txtmanhanvien.Text, txtmakhachhang.Text, txtsotien.Text, dtpdatetime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
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
                    f = dbGD.SuaGiaoDich(ref err, txtMaGiaoDich.Text, txtmanhanvien.Text, txtmakhachhang.Text, 
                        txtsotien.Text, dtpdatetime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    
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



        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.txtMaGiaoDich.ResetText();
            this.txtmanhanvien.ResetText();
            this.txtmakhachhang.ResetText();
            this.txtsotien.ResetText();
            dtpdatetime.MinDate = DateTime.MinValue;

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


        private void panel2_Paint(object sender, PaintEventArgs e)
        {

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

        private void FrmGiaoDich_Resize_1(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                FormBorderStyle = FormBorderStyle.None;
            else
                FormBorderStyle = FormBorderStyle.Sizable;
        }

        public void LoadData1()
        {
            dtGiaoDich = dbGD.DanhSachGiaoDich().Tables[0];
            dgvGiaoDich1.Columns["ThoiGianGD2"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";

            dgvGiaoDich1.DataSource = dtGiaoDich;
            dgvGiaoDich1.AutoResizeColumns();
        }

        private void pbThoat_Click(object sender, EventArgs e)
        {
            DialogResult traloi;
            traloi = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK)
                this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if(cbLocTheoMaNV.Checked)
            {
                dtGiaoDich.Clear();
                dtGiaoDich = dbGD.DanhSachGiaoDichTheoMaNV(txtMaNV.Text).Tables[0];
                dgvGiaoDich1.Columns["ThoiGianGD2"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";

                dgvGiaoDich1.DataSource = dtGiaoDich;
                dgvGiaoDich1.AutoResizeColumns();
            }
            else 
                LoadData1();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            try
            {
                string ngayGioGD = dtpdatetime1.Value.ToString("yyyy-HH-dd");
                DataSet dsGiaoDich = dbGD.TimKiemGiaoDich(ngayGioGD);
                if (dsGiaoDich != null && dsGiaoDich.Tables.Count > 0)
                {
                    dgvGiaoDich2.DataSource = dsGiaoDich.Tables[0];
                    dgvGiaoDich2.AutoResizeColumns();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void dtpdatetime_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dgvGiaoDich_SelectionChanged(object sender, EventArgs e)
        {

            int r = dgvGiaoDich.CurrentCell.RowIndex;
            txtMaGiaoDich.Text = dgvGiaoDich.Rows[r].Cells[0].Value.ToString();
            txtmanhanvien.Text = dgvGiaoDich.Rows[r].Cells[1].Value.ToString();
            txtmakhachhang.Text = dgvGiaoDich.Rows[r].Cells[2].Value.ToString();
            txtsotien.Text = dgvGiaoDich.Rows[r].Cells[3].Value.ToString();

            if (dgvGiaoDich.Rows[r].Cells[4].Value != null)
            {
                DateTime ThoiGianGiaoDich;
                if (DateTime.TryParse(dgvGiaoDich.Rows[r].Cells[4].Value.ToString(), out ThoiGianGiaoDich))
                {
                    dtpdatetime.Value = ThoiGianGiaoDich;
                }
            }
        }

        private void cbLocTheoMay_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void cbLocTheoMay_CheckStateChanged(object sender, EventArgs e)
        {
            if (cbLocTheoMaNV.Checked)
            {
                panel3.Enabled = true;
            }
            else
            {
                panel3.Enabled = false;
            }
        }
    }
}
