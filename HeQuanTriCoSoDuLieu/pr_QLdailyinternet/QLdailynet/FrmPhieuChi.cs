using BAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLdailynet
{
    public partial class FrmPhieuChi : Form
    {
        bool Them, Them1, Them2;
        DataTable dtLoaiThanhToan = null;
        DBLoaiThanhToan dbLTT;
        DataTable dtPhieuChi = null;
        DBPhieuChi dbPC;
        DataTable dtChiTietPhieuChi = null;
        DBChiTietPhieuChi dbCTPC;

        DataView dtvPhieuChi = null;
        DataView dtvCTPhieuChi = null;
        public FrmPhieuChi()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            dbLTT = new DBLoaiThanhToan();
            dbCTPC = new DBChiTietPhieuChi();
            dbPC = new DBPhieuChi();
        }
        public void LoadData()
        {
            dtLoaiThanhToan = dbLTT.LayLoaiThanhToan().Tables[0];
            dtChiTietPhieuChi = dbCTPC.LayChiTietPhieuChi().Tables[0];
            dtPhieuChi = dbPC.LayPhieuChi().Tables[0];
            dgvPhieuChi.Columns["ThoiGianThanhToan"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";

            dgvChiTietPhieuChi.DataSource = dtChiTietPhieuChi;
            dgvChiTietPhieuChi.AutoResizeColumns();

            dgvPhieuChi.DataSource = dtPhieuChi;
            dgvPhieuChi.AutoResizeColumns();

            dgvLoaiThanhToan.DataSource = dtLoaiThanhToan;
            dgvLoaiThanhToan.AutoResizeColumns();

            this.txtMaLoaiThanhToan.ResetText();
            this.txtTenDichVu.ResetText();
            this.txtGiaTien.ResetText();

            btnLuu.Enabled = false;
            btnHuy.Enabled = false;

            btnReLoad.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnThoat.Enabled = true;
            
            pnchitietphieuchi.Enabled = false;
            pnsophieu.Enabled = false;
            panel5.Enabled = false;
            
            dgvPhieuChi_CellClick(null, null);
            dgvChiTietPhieuChi_CellClick(null, null);
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            Them = true;
            this.txtMaLoaiThanhToan.ResetText();
            this.txtTenDichVu.ResetText();
            this.txtGiaTien.ResetText();

            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnThoat.Enabled = false;

            btnLuu.Enabled = true;
            btnHuy.Enabled = true;
            btnReLoad.Enabled = true;
            panel5.Enabled = true;

            this.txtMaLoaiThanhToan.Focus();
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
            panel5.Enabled = true;

            this.txtMaLoaiThanhToan.Focus();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                int r = dgvLoaiThanhToan.CurrentCell.RowIndex;
                string strMaTT = dgvLoaiThanhToan.Rows[r].Cells[0].Value.ToString();
                DialogResult traloi;
                traloi = MessageBox.Show("Chắc xóa mẫu tin này không?", "Trả lời",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                string err = "";
                if (traloi == DialogResult.Yes)
                {
                    bool f = dbLTT.XoaLoaiThanhToan(ref err, strMaTT);
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

        private void dgvLoaiThanhToan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvLoaiThanhToan.CurrentRow == null)
            {
                MessageBox.Show("Dữ liệu trong DataGridView trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int r = dgvLoaiThanhToan.CurrentCell.RowIndex;
            txtMaLoaiThanhToan.Text = dgvLoaiThanhToan.Rows[r].Cells[0].Value.ToString();
            txtTenDichVu.Text = dgvLoaiThanhToan.Rows[r].Cells[1].Value.ToString();
            txtGiaTien.Text = dgvLoaiThanhToan.Rows[r].Cells[2].Value.ToString();
            
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void FrmPhieuChi_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            bool f = false;
            if (Them)
            {
                string err = "";
                try
                {
                    int i = int.Parse(txtGiaTien.Text);
                    f = dbLTT.ThemLoaiThanhToan(ref err, txtMaLoaiThanhToan.Text, txtTenDichVu.Text, i);
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
                    txtMaLoaiThanhToan.Enabled = false;
                    int k = int.Parse(txtGiaTien.Text);
                    f = dbLTT.SuaLoaiThanhToan(ref err, txtMaLoaiThanhToan.Text, txtTenDichVu.Text, k);

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
            this.txtMaLoaiThanhToan.ResetText();
            this.txtTenDichVu.ResetText();
            this.txtGiaTien.ResetText();

            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnThoat.Enabled = true;

            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
            panel5.Enabled = false;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult traloi;
            traloi = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK)
                this.Close();
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

        private void panel6_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void FrmPhieuChi_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                FormBorderStyle = FormBorderStyle.None;
            else
                FormBorderStyle = FormBorderStyle.Sizable;
        }

        private void btnReLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (rbPhieuChi.Checked)
            {
                Them1 = false;

                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnThoat.Enabled = false;

                btnLuu.Enabled = true;
                btnHuy.Enabled = true;
                btnReLoad.Enabled = true;
                pnsophieu.Enabled = true;
                pnchitietphieuchi.Enabled = false;
                this.txtsophieu1.Focus();
            }
            else
            if (rbChiTietPhieuChi.Checked)
            {
                Them2 = false;

                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnThoat.Enabled = false;

                btnLuu.Enabled = true;
                btnHuy.Enabled = true;
                btnReLoad.Enabled = true;
                pnsophieu.Enabled = false;
                pnchitietphieuchi.Enabled = true;
                this.txtsophieu2.Focus();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (rbPhieuChi.Checked)
            {
                bool f = false;
                if (Them1)
                {
                    string err = "";
                    try
                    {
                        f = dbPC.ThemPhieuChi(ref err, txtmanv.Text, dtpdatetime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
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
                        f = dbPC.SuaPhieuChi(ref err, txtsophieu1.Text, txtmanv.Text, dtpdatetime.Value.ToString("yyyy-MM-dd HH:mm:ss"));

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
            if (rbChiTietPhieuChi.Checked)
            {
                bool f = false;
                if (Them2)
                {
                    string err = "";
                    try
                    {
                        int h = int.Parse(txtsoluong.Text);
                        f = dbCTPC.ThemChiTietPhieuChi(ref err, txtsophieu2.Text, txtloaithanhtoan.Text, h);
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
                        int k = int.Parse(txtsoluong.Text);
                        f = dbCTPC.SuaChiTietPhieuChi(ref err, txtsophieu2.Text, txtMaLoaiThanhToan.Text, k);

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

        private void button6_Click(object sender, EventArgs e)
        {
            if (rbPhieuChi.Checked)
            {
                try
                {
                    int r = dgvPhieuChi.CurrentCell.RowIndex;
                    string str = dgvPhieuChi.Rows[r].Cells[0].Value.ToString();
                    DialogResult traloi;
                    traloi = MessageBox.Show("Chắc xóa mẫu tin này không?", "Trả lời",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    string err = "";
                    if (traloi == DialogResult.Yes)
                    {
                        bool f = dbPC.XoaPhieuChi(ref err, str);
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

            else
           if (rbChiTietPhieuChi.Checked)
            {
                try
                {
                    int r = dgvChiTietPhieuChi.CurrentCell.RowIndex;
                    string str1 = dgvChiTietPhieuChi.Rows[r].Cells[0].Value.ToString();
                    string str2 = dgvChiTietPhieuChi.Rows[r].Cells[1].Value.ToString();
                    string str33 = dgvChiTietPhieuChi.Rows[r].Cells[2].Value.ToString();
                    string str44 = dgvChiTietPhieuChi.Rows[r].Cells[3].Value.ToString();
                    int str3 = int.Parse(str33);
                    int str4 = int.Parse(str44);
                    
                    DialogResult traloi;

                    traloi = MessageBox.Show("Chắc xóa mẫu tin này không?", "Trả lời",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    string err = "";
                    if (traloi == DialogResult.Yes)
                    {
                        bool f = dbCTPC.XoaChiTietPhieuChi(ref err, str1, str2, str3, str4);
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

        private void button3_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (rbPhieuChi.Checked)
            {
                this.txtsophieu2.ResetText();
                this.txtMaLoaiThanhToan.ResetText();
                this.txtsoluong.ResetText();
                this.txtsotien.Enabled = false;

                btnThem.Enabled = true;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnThoat.Enabled = true;

                btnLuu.Enabled = false;
                btnHuy.Enabled = false;
                pnsophieu.Enabled = false;
            }
            else
           if (rbChiTietPhieuChi.Checked)
            {
                txtsophieu1.Enabled = false;
                this.txtmanv.ResetText();
                this.txtongtien.Enabled = false;
                dtpdatetime.MinDate = DateTime.MinValue;

                btnThem.Enabled = true;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnThoat.Enabled = true;

                btnLuu.Enabled = false;
                btnHuy.Enabled = false;
                pnchitietphieuchi.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult traloi;
            traloi = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK)
                this.Close();
        }

        private void dgvPhieuChi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(cbLoc.Checked)
            {
                if (dgvPhieuChi.CurrentRow == null)
                {
                    MessageBox.Show("Dữ liệu trong DataGridView trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int r = dgvPhieuChi.CurrentCell.RowIndex;
                txtsophieu1.Text = dgvPhieuChi.Rows[r].Cells[0].Value.ToString();
                txtmanv.Text = dgvPhieuChi.Rows[r].Cells[1].Value.ToString();
                txtongtien.Text = dgvPhieuChi.Rows[r].Cells[2].Value.ToString();
                if (dgvPhieuChi.Rows[r].Cells[3].Value != null)
                {
                    DateTime ThoiGianGiaoDich;
                    if (DateTime.TryParse(dgvPhieuChi.Rows[r].Cells[3].Value.ToString(), out ThoiGianGiaoDich))
                    {
                        dtpdatetime.Value = ThoiGianGiaoDich;
                    }
                }
                
                DataSet dsGiaoDich = dbCTPC.GetChiTietPhieuChi(txtsophieu1.Text);
                if (dsGiaoDich != null && dsGiaoDich.Tables.Count > 0)
                {
                    dgvChiTietPhieuChi.DataSource = dsGiaoDich.Tables[0];
                    dgvChiTietPhieuChi.AutoResizeColumns();
                }
                dgvChiTietPhieuChi.AutoResizeColumns();
                dgvChiTietPhieuChi_CellClick(null, null);

            }else
            {
                if (dgvPhieuChi.CurrentRow == null)
                {
                    MessageBox.Show("Dữ liệu trong DataGridView trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int r = dgvPhieuChi.CurrentCell.RowIndex;
                txtsophieu1.Text = dgvPhieuChi.Rows[r].Cells[0].Value.ToString();
                txtmanv.Text = dgvPhieuChi.Rows[r].Cells[1].Value.ToString();
                txtongtien.Text = dgvPhieuChi.Rows[r].Cells[2].Value.ToString();
                if (dgvPhieuChi.Rows[r].Cells[3].Value != null)
                {
                    DateTime ThoiGianGiaoDich;
                    if (DateTime.TryParse(dgvPhieuChi.Rows[r].Cells[3].Value.ToString(), out ThoiGianGiaoDich))
                    {
                        dtpdatetime.Value = ThoiGianGiaoDich;
                    }
                }
                dtvPhieuChi = new DataView(dtPhieuChi);
                dtvCTPhieuChi = new DataView(dtChiTietPhieuChi);
            }
            
        }

        private void dgvChiTietPhieuChi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvChiTietPhieuChi.CurrentRow == null)
            {
                MessageBox.Show("Dữ liệu trong DataGridView trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int r = dgvChiTietPhieuChi.CurrentCell.RowIndex;
            txtsophieu2.Text = dgvChiTietPhieuChi.Rows[r].Cells[0].Value.ToString();
            txtloaithanhtoan.Text = dgvChiTietPhieuChi.Rows[r].Cells[1].Value.ToString();
            txtsoluong.Text = dgvChiTietPhieuChi.Rows[r].Cells[2].Value.ToString();
            txtsotien.Text = dgvChiTietPhieuChi.Rows[r].Cells[3].Value.ToString();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            dtPhieuChi = dbPC.HienThiDanhSachPhieuChi().Tables[0];
            dgvDanhSach.Columns["ThoiGianThanhToan1"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
            dgvDanhSach.DataSource = dtPhieuChi;
            dgvDanhSach.AutoResizeColumns();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(rbPhieuChi.Checked)
            {
                Them1 = true;
                txtsophieu1.Enabled = false;
                this.txtmanv.ResetText();
                this.txtongtien.Enabled =false;
                dtpdatetime.MinDate = DateTime.MinValue;

                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnThoat.Enabled = false;

                btnLuu.Enabled = true;
                btnHuy.Enabled = true;
                btnReLoad.Enabled = true;
                pnsophieu.Enabled = true;
                pnchitietphieuchi.Enabled = false;
                this.txtmanv.Focus();
            }
            else 
            if(rbChiTietPhieuChi.Checked)
            {
                Them2 = true;
                this.txtsophieu2.ResetText();
                this.txtMaLoaiThanhToan.ResetText();
                this.txtsoluong.ResetText();
                this.txtsotien.Enabled = false;
                
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnThoat.Enabled = false;

                btnLuu.Enabled = true;
                btnHuy.Enabled = true;
                btnReLoad.Enabled = true;
                pnchitietphieuchi.Enabled = true;
                pnsophieu.Enabled=false;
                this.txtsophieu2.Focus();
            }
                



        }
    }
}
