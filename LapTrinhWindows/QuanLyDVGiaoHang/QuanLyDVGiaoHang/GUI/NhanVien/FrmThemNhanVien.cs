using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using QuanLyDVGiaoHang.Models;
using QuanLyDVGiaoHang.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyDVGiaoHang.GUI.NhanVien
{
    public partial class FrmThemNhanVien : Form
    {
        DataTable dtTinhHuyenXa;
        List<TinhHuyenXa> TinhHuyenXaList;
        public FrmThemNhanVien()
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
        void loadData()
        {
            LoadTinhHuyenXa();
            txtDiaChi.Text = null;
            cbTinh.DataSource = TinhHuyenXaList
                .OrderBy(t => t.Tinh)
                .Select(t => t.Tinh).Distinct().ToList();
            cbTinh.SelectedItem = null;
            cbTinh.Text = "Tỉnh / Thành Phố";
            using (var context = new QuanLyDVGiaoHangEntities())
            {
                var vaiTroList = context.VAITROes.ToList();
                cbMaVaiTro.DataSource = vaiTroList;
                cbMaVaiTro.DisplayMember = "TenVaiTro";
                cbMaVaiTro.ValueMember = "MaVaiTro";

                var khoList = context.KHOTRUNGCHUYENs.ToList();
                cbMaKho.DataSource = khoList;
                cbMaKho.DisplayMember = "TenKho";
                cbMaKho.ValueMember = "MaKho";
            }
        }
        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng thêm nhân viên ?", "Thông Báo",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void FrmThemNhanVien_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string maVaiTro = ((VAITRO)cbMaVaiTro.SelectedItem).MaVaiTro;
                string maKho = ((KHOTRUNGCHUYEN)cbMaKho.SelectedItem).MaKho;
                string hoTen = txtHoTen.Text;
                DateTime birthDate = dpNgaySinh.Value;
                string Tinh = cbTinh.SelectedValue.ToString();
                string Huyen = cbHuyen.SelectedValue.ToString();
                string Xa = cbXa.SelectedValue.ToString();
                string diaChi = txtDiaChi.Text + ", " + Xa + ", " + Huyen + ", " + Tinh;
                string soDT = txtSoDT.Text;

                using (var dbContext = new QuanLyDVGiaoHangEntities())
                {
                    NHANVIEN newNhanVien = new NHANVIEN
                    {
                        MaVaiTro = maVaiTro,
                        MaKho = maKho,
                        HoTen = hoTen,
                        NgaySinh = birthDate,
                        DiaChi = diaChi,
                        SoDT = soDT
                    };

                    dbContext.NHANVIENs.Add(newNhanVien);
                    dbContext.SaveChanges();

                    MessageBox.Show("Thêm nhân viên thành công! Mã nhân viên mới: " + newNhanVien.MaNV.ToString(), "Thông báo",
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
