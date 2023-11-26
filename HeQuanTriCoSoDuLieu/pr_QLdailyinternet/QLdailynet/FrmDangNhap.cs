using DBL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLdailynet
{
    public partial class FrmDangNhap : Form
    {
        string conStr, ID, Pass;
        SqlConnection con;
        SqlCommand com;
        public FrmDangNhap()
        {
            InitializeComponent();
            pictureBox1.BackColor = Color.FromArgb(15, Color.White);
            pictureBox2.BackColor = Color.FromArgb(0, Color.White);
            groupBox2.BackColor = Color.FromArgb(0, Color.White);
        }
        private void pbdangnhap_Click(object sender, EventArgs e)
        {
            ID = txtTaiKhoan.Text.Trim();
            Pass = txtMatKhau.Text.Trim();
            string serverName = "(local)";
            string databaseName = "PROJECT_INTERNET";
            string userName = ID;
            string password = Pass;
            string conStr = $"Data Source={serverName};Initial Catalog={databaseName};User ID={userName};Password={password};";
            using (SqlConnection con = new SqlConnection(conStr))
            {
                using (SqlCommand com = new SqlCommand("PRO_DangNhap", con))
                {
                    com.CommandType = System.Data.CommandType.StoredProcedure;

                    com.Parameters.AddWithValue("@MaMay", txtMaMay.Text);
                    com.Parameters.AddWithValue("@TenDangNhap", txtTaiKhoan.Text);
                    com.Parameters.AddWithValue("@MatKhau", txtMatKhau.Text);

                    // Thêm tham số output cho @LoaiTK
                    SqlParameter loaiTKParam = new SqlParameter("@LoaiTK", System.Data.SqlDbType.Bit);
                    loaiTKParam.Direction = System.Data.ParameterDirection.Output;
                    com.Parameters.Add(loaiTKParam);

                    try
                    {
                        con.Open();
                        Logins.ID = ID;
                        Logins.Pass = Pass;
                        com.ExecuteNonQuery();
                        bool loaiTK = (bool)com.Parameters["@LoaiTK"].Value;
                        if (loaiTK == true)
                        {
                            MessageBox.Show("Đăng nhập thành công với loại tài khoản khách hàng!");
                            this.Hide();
                            FrmKhachHangDN formDNkhachhang = new FrmKhachHangDN(txtTaiKhoan.Text);
                            formDNkhachhang.Show();
                        }
                        else
                        {
                            MessageBox.Show("Đăng nhập thành công với loại tài khoản nhân viên!");
                            this.Hide();
                            FrmMain formmain = new FrmMain(txtTaiKhoan.Text, txtMaMay.Text);
                            formmain.Show();
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Đăng nhập thất bại: " + ex.Message);
                    }
                }
            }
        }

        private void pbthoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn Có Muốn Thoát Chương Trình ?", "Thông Báo",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();
        }

        private void FrmDangNhap_Load(object sender, EventArgs e)
        {
            
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
