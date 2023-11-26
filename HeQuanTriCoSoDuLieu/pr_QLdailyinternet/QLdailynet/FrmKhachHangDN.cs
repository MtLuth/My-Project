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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace QLdailynet
{
    public partial class FrmKhachHangDN : Form
    {
        private int seconds = 0;
        public Timer timer = new Timer();
        bool Them;
        string txtTenDangNhap, txtBatDauTG, txtMaMay;
        DBDangNhap dbDN;
        DataTable dtDangNhap = null;
        DBMay dbM;
        DataTable dtMay = null;
        public FrmKhachHangDN(string txt)
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;

            dbDN = new DBDangNhap();
            dbM = new DBMay();
            txtTenDangNhap = txt;
            timer.Interval = 1000;
            timer.Tick += timer1_Tick;
            
        }
        private void LoadData()
        {
            dtDangNhap = dbDN.LayThongTinDangNhap(txtTenDangNhap).Tables[0];
            dtMay = dbM.LayThongTinMay(dtDangNhap.Rows[0]["MaMay"].ToString()).Tables[0];
            lbMaMay.Text = dtMay.Rows[0]["MaMay"].ToString() + " giá: " + dtMay.Rows[0]["GiaTien"].ToString();
            txtMaMay = dtMay.Rows[0]["MaMay"].ToString();
            txtmaKH.Text = dtDangNhap.Rows[0]["MaKH"].ToString();
            txtTenKH.Text = dtDangNhap.Rows[0]["TenKH"].ToString();
            if (dtDangNhap.Rows.Count > 0)
            {
                DateTime time = Convert.ToDateTime(dtDangNhap.Rows[0]["TGBatDau"].ToString());
                txtTGDangNhap.Text = time.ToString("HH:mm:ss");
                txtBatDauTG = time.ToString("HH:mm:ss.fffffff");
                if (dtDangNhap.Rows[0]["TGKetThuc"] == DBNull.Value)
                {
                    txtKetThuc.Text = "No data...";
                }
                else
                {
                    DateTime time1 = Convert.ToDateTime(dtDangNhap.Rows[0]["TGKetThuc"].ToString());
                    txtKetThuc.Text = time1.ToString("HH:mm:ss");
                }
            }
            else
            {
                txtTGDangNhap.Text = "No data...";
                txtKetThuc.Text = "No data...";
            }
            txtSoDu.Text = dtDangNhap.Rows[0]["SoDu"].ToString();
        }

        void updateData()
        {
            dbDN.UpdateTGKetThuc(txtBatDauTG, txtTenDangNhap);
            LoadData();
            string err = "";
            try
            {
                dbDN.KiemTraSoDuVaThanhToan(ref err, txtTenDangNhap, txtBatDauTG, txtMaMay);
                if (err != "") // nếu có lỗi thì thông báo lỗi
                {
                    MessageBox.Show("Thông Báo : " + err);
                }
                if(err == "Tài Khoản Đã Hết Tiền. Vui lòng khách hàng nạp thêm để sử dụng !")
                {
                    dbDN.DanhDauDangXuat(txtBatDauTG, txtTenDangNhap);
                    dbM.DanhDauDangXuatMay(txtMaMay);
                    this.Close();
                    FrmDangNhap formmain = new FrmDangNhap();
                    timer.Stop();
                    seconds = 0;
                    txtTGSuDung.Text = "00:00:00";
                    formmain.Show();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Thông Báo : " + ex.Message);
            }
        }


        private void FrmKhachHang_Load(object sender, EventArgs e)
        {
            LoadData();
            timer.Start();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            DialogResult traloi;
            traloi = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK)
            {
                dbDN.DanhDauDangXuat(txtBatDauTG, txtTenDangNhap);
                dbM.DanhDauDangXuatMay(txtMaMay);
                this.Close();
                FrmDangNhap formmain = new FrmDangNhap();
                timer.Stop();
                seconds = 0;
                txtTGSuDung.Text = "00:00:00";
                formmain.Show();
                
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            seconds++;
            txtTGSuDung.Text = TimeSpan.FromSeconds(seconds).ToString(@"hh\:mm\:ss");
            if (seconds % 60 == 0) 
            {
                updateData();
            }
        }


        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            DatDichVu form = new DatDichVu(txtMaMay);
            form.Show();
        }

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            pictureBox3.BorderStyle = BorderStyle.Fixed3D;
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.BorderStyle = BorderStyle.None;
        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            pictureBox2.BorderStyle = BorderStyle.Fixed3D;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.BorderStyle = BorderStyle.None;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}
