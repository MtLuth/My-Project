using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyDVGiaoHang.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using QuanLyDVGiaoHang.Resources;

namespace QuanLyDVGiaoHang.GUI.KhachHang
{
    public partial class FrmThemKhachHang : Form
    {
        DataTable dtTinhHuyenXa;
        List<TinhHuyenXa> TinhHuyenXaList;
        
        public FrmThemKhachHang()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
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
            txtTenNguoiBan.Text = null;
            txtTenCuaHang.Text = null;
            txtSoDienThoai.Text = null;
            txtEmail.Text = null;
            txtDiaChi.Text = null;
            cbTinh.DataSource = TinhHuyenXaList
                .OrderBy(t=>t.Tinh)
                .Select(t=>t.Tinh).Distinct().ToList();
            cbTinh.SelectedItem = null;
            cbTinh.Text = "Tỉnh / Thành Phố";
        }

        private void FrmThemKhachHang_Load(object sender, EventArgs e)
        {
            LoadData();
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

        private void btnThemKH_Click(object sender, EventArgs e)
        {
            try
            {
                string TenNguoiBan = txtTenNguoiBan.Text;
                string TenCuaHang = txtTenCuaHang.Text;
                string SoDT = txtSoDienThoai.Text;
                string Email = txtEmail.Text;
                string Tinh = cbTinh.SelectedValue.ToString();
                string Huyen = cbHuyen.SelectedValue.ToString();
                string Xa = cbXa.SelectedValue.ToString();
                string Diachi = txtDiaChi.Text + ", " + Xa + ", " + Huyen + ", " + Tinh;

                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    KHACHHANG KhachHangNew = new KHACHHANG()
                    {
                        TenNguoiBan = TenNguoiBan,
                        TenCuaHang = TenCuaHang,
                        SoDT = SoDT,
                        Email = Email,
                        DiaChi = Diachi
                    };
                    context.KHACHHANGs.Add(KhachHangNew);
                    DialogResult r = MessageBox.Show("Bạn có muốn thêm khách hàng này không?", "Thông báo", 
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (r == DialogResult.OK)
                    {
                        context.SaveChanges();
                        MessageBox.Show("Thêm khách hàng mới thành công! Mã khách hàng mới: " + KhachHangNew.MaKhachHang.ToString(), "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FrmKhachHang frm = Application.OpenForms.OfType<FrmKhachHang>().FirstOrDefault();
                        frm?.DataSave();
                        LoadData();
                    }      
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi thêm khách hàng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn hủy thêm khách hàng này không?", "Thông báo",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                this.Close();
            }
        }
    }
}
