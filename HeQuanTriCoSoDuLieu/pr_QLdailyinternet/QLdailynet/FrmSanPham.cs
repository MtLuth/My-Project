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
    public partial class FrmSanPham : Form
    {
        DBDatDichVu dbDDV = null;
        public Image HinhAnh;
        public string TenDV;
        public string MaDV;
        public int SoLuong;
        public int GiaTien;
        public int TongTien;
        string txtMaMay;
        public FrmSanPham(string txt)
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            dbDDV = new DBDatDichVu();
            txtMaMay = txt;
        }
        public void LoadData()
        {
            labelTenDV.Text = TenDV;
            labelGiaTien.Text = "Giá tiền: " + GiaTien.ToString();
            TongTien = GiaTien;
            picBoxDichVu.BackgroundImageLayout = ImageLayout.Stretch;
            picBoxDichVu.BackgroundImage = HinhAnh;
            numericUpDown1.Value = 1;
            SoLuong = Convert.ToInt32(numericUpDown1.Value.ToString());
        }

        private void FrmSanPham_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            SoLuong = Convert.ToInt32(numericUpDown1.Value.ToString());
            TongTien = GiaTien * SoLuong;
            labelGiaTien.Text = "Giá tiền: " + TongTien.ToString();
        }

        private void FrmSanPham_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                FormBorderStyle = FormBorderStyle.None;
            else
                FormBorderStyle = FormBorderStyle.Sizable;
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string err = "";
            try
            {
                bool f = dbDDV.DatDichVu(ref err, txtMaMay, this.MaDV, this.SoLuong.ToString(), this.TongTien.ToString());
                if (f)
                {
                    MessageBox.Show("Đặt dịch vụ thành công!");
                }
                else
                {
                    MessageBox.Show("Lỗi: " + err);
                    return;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            DialogResult traloi;
            traloi = MessageBox.Show("Chắc không?", "Trả lời",
            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK) this.Close();
        }

        private void pbThoat_Click(object sender, EventArgs e)
        {
            DialogResult traloi;
            traloi = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK)
                this.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
