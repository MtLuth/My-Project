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
    public partial class FrmThuChi : Form
    {
        DBGiaoDich dbGD;
        DBPhieuChi dbPC;
        public FrmThuChi()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            dbGD = new DBGiaoDich();
            dbPC = new DBPhieuChi();
        }
        public void LoadData()
        {
            string err = "";
            try
            {
                int tongSoTien = 0;
                if (!dbGD.DoanhThuTrongThang(ref err, txtThang.Text, txtNam.Text, out tongSoTien))
                {
                    MessageBox.Show("Thông Báo : " + err);
                }
                else
                {
                    lbDoanhThu.Text = tongSoTien.ToString();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Thông Báo : " + ex.Message);
            }
        }
        public void LoadData1()
        {
            string err = "";
            try
            {
                int tongSoTien = 0;
                if (!dbPC.ChiPhiTrongThang(ref err, txtThang.Text, txtNam.Text, out tongSoTien))
                {
                    MessageBox.Show("Thông Báo : " + err);
                }
                else
                {
                    lbChiPhi.Text = tongSoTien.ToString();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Thông Báo : " + ex.Message);
            }
        }
        private void FrmThuChi_Load(object sender, EventArgs e)
        {
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            LoadData1();
        }

        private void pbThoat_Click(object sender, EventArgs e)
        {
            DialogResult traloi;
            traloi = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK)
                this.Close();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
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

        private void FrmThuChi_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                FormBorderStyle = FormBorderStyle.None;
            else
                FormBorderStyle = FormBorderStyle.Sizable;
        }
    }
}
