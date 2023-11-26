using QuanLyDVGiaoHang.Models;
using QuanLyDVGiaoHang.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace QuanLyDVGiaoHang.GUI
{
    public partial class FrmDangKyTKDN : Form
    {
        DataTable dtTinhHuyenXa;
        List<TinhHuyenXa> TinhHuyenXaList;
        public FrmDangKyTKDN()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
        }
        private void FrmDangKyTKDN_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng thêm nhân viên ?", "Thông Báo",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }
        void LoadTinhHuyenXa()
        {
            string filePath = "TinhHuyenXa.xlsx";
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(file);
                ISheet sheet = workbook.GetSheetAt(0); // Lấy sheet đầu tiên

                dtTinhHuyenXa = new DataTable();
                IRow headerRow = sheet.GetRow(0);

                // Tạo các cột trong DataTable từ dữ liệu trong hàng đầu tiên
                foreach (ICell cell in headerRow.Cells)
                {
                    dtTinhHuyenXa.Columns.Add(cell.StringCellValue);
                }

                // Đọc dữ liệu từ các hàng còn lại trong sheet và đưa vào DataTable
                for (int rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
                {
                    IRow dataRow = sheet.GetRow(rowIndex);
                    DataRow newRow = dtTinhHuyenXa.NewRow();

                    for (int cellIndex = 0; cellIndex < headerRow.Cells.Count; cellIndex++)
                    {
                        ICell cell = dataRow.GetCell(cellIndex);
                        newRow[cellIndex] = cell?.ToString();
                    }
                    dtTinhHuyenXa.Rows.Add(newRow);
                }
            }
            TinhHuyenXaList = dtTinhHuyenXa.AsEnumerable()
                    .Select(row => new TinhHuyenXa
                    {
                        Tinh = row["Tinh"].ToString(),
                        Huyen = row["Huyen"].ToString(),
                        Xa = row["Xa"].ToString()
                    })
                    .ToList();
        }
        void LoadData()
        {
            LoadTinhHuyenXa();
            txtHoTen.Text = null;
            txtTenCuaHang.Text = null;
            txtSoDT.Text = null;
            txtEmail.Text = null;
            txtDiaChi.Text = null;
            cbTinh.DataSource = TinhHuyenXaList
                .OrderBy(t => t.Tinh)
                .Select(t => t.Tinh).Distinct().ToList();
            cbTinh.SelectedItem = null;
            cbTinh.Text = "Tỉnh / Thành Phố";
        }
        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string tenKhachHang = txtHoTen.Text;
                string tenCuaHang = txtTenCuaHang.Text;
                string email = txtEmail.Text;
                string Tinh = cbTinh.SelectedValue.ToString();
                string Huyen = cbHuyen.SelectedValue.ToString();
                string Xa = cbXa.SelectedValue.ToString();
                string diachi = txtDiaChi.Text + ", " + Xa + ", " + Huyen + ", " + Tinh;
                string soDT = txtSoDT.Text;
                string tenDangNhap = txtTenDangNhap.Text;
                string matKhau = txtMatKhau.Text;

                using (var dbContext = new QuanLyDVGiaoHangEntities())
                {
                    KHACHHANG newKhachHang = new KHACHHANG
                    {
                        TenNguoiBan = tenKhachHang,
                        TenCuaHang = tenCuaHang,
                        Email = email,
                        DiaChi = diachi,
                        SoDT = soDT
                    };
                    dbContext.KHACHHANGs.Add(newKhachHang);
                    dbContext.SaveChanges();
                    int maKhachHangMoi = newKhachHang.MaKhachHang;
                    TK_KHACHHANG newTKKhachHang = new TK_KHACHHANG
                    {
                        MaKhachHang = maKhachHangMoi,
                        TenDangNhap = tenDangNhap,
                        MatKhau = matKhau
                    };
                    dbContext.TK_KHACHHANG.Add(newTKKhachHang);
                    dbContext.SaveChanges();

                    MessageBox.Show("Chúc mừng bạn đã đăng ký tài khoản KH: '" + maKhachHangMoi.ToString()
                        + "' thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi thêm nhân viên: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void cbTinh_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbTinh.SelectedItem != null)
            {
                cbHuyen.DataSource = TinhHuyenXaList
                    .Where(t => t.Tinh == cbTinh.SelectedValue.ToString())
                    .OrderBy(t => t.Huyen)
                    .Select(t => t.Huyen).Distinct().ToList();
            }
            cbHuyen.SelectedItem = null;
            cbHuyen.Text = "Huyện / Quận";
        }

        private void cbHuyen_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbHuyen.SelectedItem != null)
            {
                cbXa.DataSource = TinhHuyenXaList
                .Where(t => t.Huyen == cbHuyen.SelectedValue.ToString())
                .OrderBy(t => t.Xa)
                .Select(t => t.Xa)
                .Distinct()
                .ToList();
            }
            cbXa.SelectedItem = null;
            cbXa.Text = "Xã / Phường";
        }
    }
}
