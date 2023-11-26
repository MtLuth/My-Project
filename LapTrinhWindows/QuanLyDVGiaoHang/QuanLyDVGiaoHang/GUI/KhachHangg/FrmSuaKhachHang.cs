using NPOI.HSSF.Record.Chart;
using NPOI.OpenXmlFormats.Shared;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using QuanLyDVGiaoHang.Models;
using QuanLyDVGiaoHang.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyDVGiaoHang.GUI.KhachHang
{
    public partial class FrmSuaKhachHang : Form
    {
        DataTable dtTinhHuyenXa;
        List<TinhHuyenXa> TinhHuyenXaList;
        public KHACHHANG KhachHangCanSua { get; set; }
        public FrmSuaKhachHang()
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
            // Xử lý dữ liệu Khách hàng
            txtTenNguoiBan.Text = KhachHangCanSua.TenNguoiBan;
            txtTenCuaHang.Text = KhachHangCanSua.TenCuaHang;
            txtSoDienThoai.Text = KhachHangCanSua.SoDT;
            txtEmail.Text = KhachHangCanSua.Email;
            // Load các tỉnh vào danh sách
            cbTinh.DataSource = TinhHuyenXaList
                .OrderBy(t => t.Tinh)
                .Select(t => t.Tinh).Distinct().ToList();
            // Xử lý phần địa chỉ
            string input = KhachHangCanSua.DiaChi;
            string[] DiaChi = input.Split(',');
            int n = DiaChi.Length;
            string Tinh = DiaChi[n-1].Trim();
            string Huyen = DiaChi[n-2].Trim();
            string Xa = DiaChi[n-3].Trim();
            int index = cbTinh.FindStringExact(Tinh);
            cbTinh.SelectedIndex = index;
            index = cbHuyen.FindStringExact(Huyen);
            cbHuyen.SelectedIndex = index;
            index = cbXa.FindStringExact(Xa);
            cbXa.SelectedIndex = index;
            string diachi = "";
            for (int i = 0; i < n-3; i++)
            {
                diachi += DiaChi[i].Trim();
                if (i+1!=n-3)
                    diachi += ",";
            }
            txtDiaChi.Text = diachi;
        }

        private void SuaKhachHang_Load(object sender, EventArgs e)
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

        private void btnSuaKH_Click(object sender, EventArgs e)
        {
            int MaKhachHang = KhachHangCanSua.MaKhachHang;
            string TenNguoiBan = txtTenNguoiBan.Text;
            string TenCuaHang = txtTenCuaHang.Text;
            string SoDT = txtSoDienThoai.Text;
            string Email = txtEmail.Text;
            string DiaChi = txtDiaChi.Text + "," + cbXa.SelectedValue.ToString() + "," + cbHuyen.SelectedValue.ToString()
                + "," + cbTinh.SelectedValue.ToString();
            try
            {
                using (var context = new QuanLyDVGiaoHangEntities())
                {
                    var khachhang = context.KHACHHANGs.FirstOrDefault(kh => kh.MaKhachHang == MaKhachHang);
                    if (khachhang != null)
                    {
                        khachhang.TenNguoiBan = TenNguoiBan;
                        khachhang.TenCuaHang = TenCuaHang;
                        khachhang.SoDT = SoDT;
                        khachhang.Email = Email;
                        khachhang.DiaChi = DiaChi;
                    }
                    DialogResult r = MessageBox.Show("Bạn có chắc muốn sửa đổi thông tin khách hàng này không?", "Thông báo",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (r == DialogResult.OK)
                    {
                        context.SaveChanges();
                        MessageBox.Show("Đã sửa thông tin khách hàng thành công", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FrmKhachHang frm = Application.OpenForms.OfType<FrmKhachHang>().FirstOrDefault();
                        frm?.DataSave();
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sửa thông tin khách hàng không thành công! Lỗi:" + ex.Message, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
