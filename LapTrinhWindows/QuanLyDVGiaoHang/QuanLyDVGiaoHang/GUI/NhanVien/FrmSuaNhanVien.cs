using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using QuanLyDVGiaoHang.Models;
using QuanLyDVGiaoHang.Resources;
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

namespace QuanLyDVGiaoHang.GUI.NhanVien
{
    public partial class FrmSuaNhanVien : Form
    {
        public NHANVIEN nHANVIEN { get; set; }
        DataTable dtTinhHuyenXa;
        List<TinhHuyenXa> TinhHuyenXaList;
        public FrmSuaNhanVien()
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

                int maNhanVien = nHANVIEN.MaNV;
                var nhanVien = context.NHANVIENs.FirstOrDefault(nv => nv.MaNV == maNhanVien);
                if (nhanVien != null)
                {
                    txtMaNhanVien.Text = maNhanVien.ToString();
                    cbMaVaiTro.SelectedValue = nhanVien.MaVaiTro;
                    cbMaKho.SelectedValue = nhanVien.MaKho;
                    dpNgaySinh.Value = nhanVien.NgaySinh;
                    txtHoTen.Text = nhanVien.HoTen;

                    cbTinh.DataSource = TinhHuyenXaList
                                        .OrderBy(t => t.Tinh)
                                        .Select(t => t.Tinh).Distinct().ToList();
                    // Xử lý phần địa chỉ
                    string input = nHANVIEN.DiaChi;
                    string[] DiaChi = input.Split(',');
                    int n = DiaChi.Length;
                    string Tinh = DiaChi[n - 1].Trim();
                    string Huyen = DiaChi[n - 2].Trim();
                    string Xa = DiaChi[n - 3].Trim();
                    int index = cbTinh.FindStringExact(Tinh);
                    cbTinh.SelectedIndex = index;
                    index = cbHuyen.FindStringExact(Huyen);
                    cbHuyen.SelectedIndex = index;
                    index = cbXa.FindStringExact(Xa);
                    cbXa.SelectedIndex = index;
                    string diachi = "";
                    for (int i = 0; i < n - 3; i++)
                    {
                        diachi += DiaChi[i].Trim();
                        if (i + 1 != n - 3)
                            diachi += ",";
                    }
                    txtDiaChi.Text = diachi;
                    txtSoDT.Text = nhanVien.SoDT;
                }
            }
        }
        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            try
            {
                int maNhanVien = nHANVIEN.MaNV;
                string maVaiTro = cbMaVaiTro.SelectedValue.ToString();
                string maKho = cbMaKho.SelectedValue.ToString();
                string hoTen = txtHoTen.Text;
                DateTime birthDate = dpNgaySinh.Value;
                string diaChi = txtDiaChi.Text + "," + cbXa.SelectedValue.ToString() + "," + cbHuyen.SelectedValue.ToString()
                 + "," + cbTinh.SelectedValue.ToString();
                string soDT = txtSoDT.Text;

                using (var dbContext = new QuanLyDVGiaoHangEntities())
                {
                    var nhanVien = dbContext.NHANVIENs.FirstOrDefault(nv => nv.MaNV == maNhanVien);
                    if (nhanVien != null)
                    {
                        nhanVien.MaVaiTro = maVaiTro;
                        nhanVien.MaKho = maKho;
                        nhanVien.HoTen = hoTen;
                        nhanVien.NgaySinh = birthDate;
                        nhanVien.DiaChi = diaChi;
                        nhanVien.SoDT = soDT;

                        dbContext.SaveChanges();

                        MessageBox.Show("Cập nhật thông tin nhân viên thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy nhân viên với mã nhân viên đã cho!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi cập nhật thông tin nhân viên: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FrmSuaNhanVien_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng thêm nhân viên ?", "Thông Báo",
              MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
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
