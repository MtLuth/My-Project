using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BAL;
using System.Net.NetworkInformation;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Runtime.InteropServices;

namespace QLdailynet
{
    public partial class DatDichVu : Form
    {
        DBDichVu dbDV = null;
        DBDatDichVu dbDDV = null;
        DBLoaiDichVu dbLDV = null;
        private Panel pic = new Panel();
        private Label labelGiaTien = new Label();
        private Label labelTenDV = new Label();
        private Button btnLoaiDV = new Button();
        DataTable dtDichVu = null;
        DataTable dtLoaiDichVu = new DataTable();
        string txtMaMay;   
        public DatDichVu(string txt)
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            dbDV = new DBDichVu();
            dbDDV = new DBDatDichVu();
            dbLDV = new DBLoaiDichVu();
            txtMaMay = txt;
        }

        public void LoadData()
        {
            // Đưa danh sách dịch vụ vào menu
            try
            {
                dtDichVu = new DataTable();
                dtDichVu.Clear();
                dtDichVu = dbDV.LayDichVu().Tables[0];
                foreach (DataRow row in dtDichVu.Rows)
                {
                    pic = new Panel();
                    pic.Height = 150;
                    pic.Width = 150;
                    pic.BackgroundImageLayout = ImageLayout.Stretch;
                    pic.BorderStyle = BorderStyle.FixedSingle;
                    pic.Tag = row["MaDV"].ToString();
                    if (!Convert.IsDBNull(row["HinhAnh"]))
                    {
                        byte[] data = (byte[])row["HinhAnh"];
                        if (data != null)
                        {
                            MemoryStream ms = new MemoryStream(data);
                            Bitmap bmp = new Bitmap(ms);
                            pic.BackgroundImage = bmp;
                        }
                    }
                    else // Thêm hình ảnh mặc định
                    {
                        Icon icon = SystemIcons.Application;
                        Bitmap defaultImage = icon.ToBitmap();
                        pic.BackgroundImage = defaultImage;
                    }

                    labelGiaTien = new Label();
                    labelGiaTien.Text = row["GiaTien"].ToString();
                    labelGiaTien.BackColor = Color.FromArgb(255, 121, 121);
                    labelGiaTien.TextAlign = ContentAlignment.MiddleCenter;
                    labelGiaTien.Width = 50;
                    labelGiaTien.Tag = row["MaDV"].ToString();

                    labelTenDV = new Label();
                    labelTenDV.Text = row["TenDV"].ToString();
                    labelTenDV.BackColor = Color.FromArgb(46, 13, 222);
                    labelTenDV.TextAlign = ContentAlignment.MiddleCenter;
                    labelTenDV.Dock = DockStyle.Bottom;
                    labelTenDV.Tag = row["MaDV"].ToString();

                    pic.Controls.Add(labelGiaTien);
                    pic.Controls.Add(labelTenDV);
                    pic.Click += new EventHandler(OnClick);
                    flowLayoutMenu.Controls.Add(pic);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            // Đưa các nút lọc sản phẩm
            try
            {
                dtLoaiDichVu = dbLDV.LayLoai().Tables[0];
                foreach (DataRow row in dtLoaiDichVu.Rows)
                {
                    btnLoaiDV = new Button();
                    btnLoaiDV.Width = 120;
                    btnLoaiDV.Height = 30;
                    btnLoaiDV.Tag = row["MaLoai"].ToString();
                    btnLoaiDV.Text = row["TenLoaiDichVu"].ToString();
                    flowLayoutButton.Controls.Add(btnLoaiDV);
                    btnLoaiDV.Click += new EventHandler(OnBtnFilterClick);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            btnLoaiDV = new Button();
            btnLoaiDV.Width = 120;
            btnLoaiDV.Height = 30;
            btnLoaiDV.Tag = null;
            btnLoaiDV.Text = "Tất cả";
            btnLoaiDV.Click += new EventHandler(OnBtnFilterClick);
            flowLayoutButton.Controls.Add(btnLoaiDV);
        }

        private void DatDichVu_Load(object sender, EventArgs e)
        {
            LoadData();
        }


        public void OnClick(object sender, EventArgs e)
        {
            
            DataTable dt = new DataTable();
            dt.Clear();
            dt = dbDDV.ChonDichVu(((Panel)sender).Tag.ToString()).Tables[0];
            DataRow r = dt.Rows[0];
            string TenDV = r["TenDV"].ToString();
            int GiaTien = r.Field<int>("GiaTien");
            PictureBox HinhAnh = new PictureBox();
            FrmSanPham frm = new FrmSanPham(txtMaMay);
            if (!Convert.IsDBNull(r["HinhAnh"]))
            {
                byte[] data = (byte[])r["HinhAnh"];
                if (data != null)
                {
                    MemoryStream ms = new MemoryStream(data);
                    Bitmap bmp = new Bitmap(ms);
                    HinhAnh.BackgroundImage = bmp;
                }
            }
            else // Thêm hình ảnh mặc định
            {
                Icon icon = SystemIcons.Application;
                Bitmap defaultImage = icon.ToBitmap();
                HinhAnh.BackgroundImage = defaultImage;
            }
            frm.MaDV = ((Panel)sender).Tag.ToString();
            frm.TenDV = TenDV;
            frm.GiaTien = GiaTien;
            frm.HinhAnh = (Image)HinhAnh.BackgroundImage.Clone();
            frm.Show();
        }

        private void OnBtnFilterClick(object sender, EventArgs e)
        {
            string MaLoai = null;
            if (((Button)sender).Tag != null)
                MaLoai = ((Button)sender).Tag.ToString();
            DataTable dt = new DataTable();
            dt.Clear();
            dt = dbDV.TimKiemDichVuTheoLoai(MaLoai).Tables[0];
            flowLayoutMenu.Controls.Clear();
            foreach (DataRow row in dt.Rows)
            {
                pic = new Panel();
                pic.Height = 150;
                pic.Width = 150;
                pic.BackgroundImageLayout = ImageLayout.Stretch;
                pic.BorderStyle = BorderStyle.FixedSingle;
                pic.Tag = row["MaDV"].ToString();
                if (!Convert.IsDBNull(row["HinhAnh"]))
                {
                    byte[] data = (byte[])row["HinhAnh"];
                    if (data != null)
                    {
                        MemoryStream ms = new MemoryStream(data);
                        Bitmap bmp = new Bitmap(ms);
                        pic.BackgroundImage = bmp;
                    }
                }
                else // Thêm hình ảnh mặc định
                {
                    Icon icon = SystemIcons.Application;
                    Bitmap defaultImage = icon.ToBitmap();
                    pic.BackgroundImage = defaultImage;
                }

                labelGiaTien = new Label();
                labelGiaTien.Text = row["GiaTien"].ToString();
                labelGiaTien.BackColor = Color.FromArgb(255, 121, 121);
                labelGiaTien.TextAlign = ContentAlignment.MiddleCenter;
                labelGiaTien.Width = 50;
                labelGiaTien.Tag = row["MaDV"].ToString();

                labelTenDV = new Label();
                labelTenDV.Text = row["TenDV"].ToString();
                labelTenDV.BackColor = Color.FromArgb(46, 13, 222);
                labelTenDV.TextAlign = ContentAlignment.MiddleCenter;
                labelTenDV.Dock = DockStyle.Bottom;
                labelTenDV.Tag = row["MaDV"].ToString();

                pic.Controls.Add(labelGiaTien);
                pic.Controls.Add(labelTenDV);
                pic.Click += new EventHandler(OnClick);
                flowLayoutMenu.Controls.Add(pic);
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string errorMessage = null;
            string TenDV = null;
            if (txtTenDichVu.Text != "")
                TenDV = txtTenDichVu.Text;
            DataSet ds = dbDV.TimKiemDichVuKH(TenDV, out errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                MessageBox.Show("Lỗi :" + errorMessage);
            }
            else
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                flowLayoutMenu.Controls.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    pic = new Panel();
                    pic.Height = 150;
                    pic.Width = 150;
                    pic.BackgroundImageLayout = ImageLayout.Stretch;
                    pic.BorderStyle = BorderStyle.FixedSingle;
                    pic.Tag = row["MaDV"].ToString();
                    if (!Convert.IsDBNull(row["HinhAnh"]))
                    {
                        byte[] data = (byte[])row["HinhAnh"];
                        if (data != null)
                        {
                            MemoryStream ms = new MemoryStream(data);
                            Bitmap bmp = new Bitmap(ms);
                            pic.BackgroundImage = bmp;
                        }
                    }
                    else // Thêm hình ảnh mặc định
                    {
                        Icon icon = SystemIcons.Application;
                        Bitmap defaultImage = icon.ToBitmap();
                        pic.BackgroundImage = defaultImage;
                    }

                    labelGiaTien = new Label();
                    labelGiaTien.Text = row["GiaTien"].ToString();
                    labelGiaTien.BackColor = Color.FromArgb(255, 121, 121);
                    labelGiaTien.TextAlign = ContentAlignment.MiddleCenter;
                    labelGiaTien.Width = 50;
                    labelGiaTien.Tag = row["MaDV"].ToString();

                    labelTenDV = new Label();
                    labelTenDV.Text = row["TenDV"].ToString();
                    labelTenDV.BackColor = Color.FromArgb(46, 13, 222);
                    labelTenDV.TextAlign = ContentAlignment.MiddleCenter;
                    labelTenDV.Dock = DockStyle.Bottom;
                    labelTenDV.Tag = row["MaDV"].ToString();

                    pic.Controls.Add(labelGiaTien);
                    pic.Controls.Add(labelTenDV);
                    pic.Click += new EventHandler(OnClick);
                    flowLayoutMenu.Controls.Add(pic);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pbThoat_Click(object sender, EventArgs e)
        {
            DialogResult traloi;
            traloi = MessageBox.Show("Chắc không?", "Trả lời",
            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK) 
                this.Close();
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

        private void DatDichVu_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                FormBorderStyle = FormBorderStyle.None;
            else
                FormBorderStyle = FormBorderStyle.Sizable;
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {

        }
    }
}
