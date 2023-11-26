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

namespace QuanLyDVGiaoHang.GUI.TaoDon
{
    public partial class FrmTaoDonKH : Form
    {
        DataTable dtTinhHuyenXa;
        List<TinhHuyenXa> TinhHuyenXaList;
        private string TenKhachHang;
        public FrmTaoDonKH(string tenKH)
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            TenKhachHang = tenKH;
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
            txtTenNguoiGui.Text = TenKhachHang;
            txtDiaChi1.Text = null;
            cbTinh1.DataSource = TinhHuyenXaList
                .OrderBy(t => t.Tinh)
                .Select(t => t.Tinh).Distinct().ToList();
            cbTinh1.SelectedItem = null;
            cbTinh1.Text = "Tỉnh / Thành Phố";
            txtDiaChi2.Text = null;
            cbTinh2.DataSource = TinhHuyenXaList
                .OrderBy(t => t.Tinh)
                .Select(t => t.Tinh).Distinct().ToList();
            cbTinh2.SelectedItem = null;
            cbTinh2.Text = "Tỉnh / Thành Phố";
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng tạo đơn không ?", "Thông Báo",
              MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void TaoDonKH_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void cbTinh1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbTinh1.SelectedItem != null)
            {
                cbHuyen1.DataSource = TinhHuyenXaList
                    .Where(t => t.Tinh == cbTinh1.SelectedValue.ToString())
                    .OrderBy(t => t.Huyen)
                    .Select(t => t.Huyen).Distinct().ToList();
            }
            cbHuyen1.SelectedItem = null;
            cbHuyen1.Text = "Huyện / Quận";
        }

        private void cbTinh2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbTinh2.SelectedItem != null)
            {
                cbHuyen2.DataSource = TinhHuyenXaList
                    .Where(t => t.Tinh == cbTinh2.SelectedValue.ToString())
                    .OrderBy(t => t.Huyen)
                    .Select(t => t.Huyen).Distinct().ToList();
            }
            cbHuyen2.SelectedItem = null;
            cbHuyen2.Text = "Huyện / Quận";
        }

        private void cbHuyen1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbHuyen1.SelectedItem != null)
            {
                cbXa1.DataSource = TinhHuyenXaList
                .Where(t => t.Huyen == cbHuyen1.SelectedValue.ToString())
                .OrderBy(t => t.Xa)
                .Select(t => t.Xa)
                .Distinct()
                .ToList();
            }
            cbXa1.SelectedItem = null;
            cbXa1.Text = "Xã / Phường";
        }

        private void cbHuyen2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbHuyen2.SelectedItem != null)
            {
                cbXa2.DataSource = TinhHuyenXaList
                .Where(t => t.Huyen == cbHuyen2.SelectedValue.ToString())
                .OrderBy(t => t.Xa)
                .Select(t => t.Xa)
                .Distinct()
                .ToList();
            }
            cbXa2.SelectedItem = null;
            cbXa2.Text = "Xã / Phường";
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string nguoiGui = txtTenNguoiGui.Text;
                string nguoiNhan = txtTenNguoiNhan.Text;
                string Tinh1 = cbTinh1.SelectedValue.ToString();
                string Huyen1 = cbHuyen1.SelectedValue.ToString();
                string Xa1 = cbXa1.SelectedValue.ToString();
                string diaChiNguoiGui = txtDiaChi1.Text + ", " + Xa1 + ", " + Huyen1 + ", " + Tinh1;
                string Tinh2 = cbTinh2.SelectedValue.ToString();
                string Huyen2 = cbHuyen2.SelectedValue.ToString();
                string Xa2 = cbXa2.SelectedValue.ToString();
                string diaChiNguoiNhan = txtDiaChi2.Text + ", " + Xa2 + ", " + Huyen2 + ", " + Tinh2;
                DateTime ngayTaoDon = dtpNgayTaoDon.Value;
                string thongTinHangHoa = txtThongTinHangHoa.Text;
                string phiCOD = txtPhiCOD.Text;

                using (var dbContext = new QuanLyDVGiaoHangEntities())
                {
                    DONHANG newDonHang = new DONHANG
                    {
                        NguoiGui = nguoiGui,
                        NguoiNhan = nguoiNhan,
                        DiaChiNguoiGui = diaChiNguoiGui,
                        DiaChiNguoiNhan = diaChiNguoiNhan,
                        ThongTinHangHoa = thongTinHangHoa,
                        NgayTaoDon = ngayTaoDon,
                        PhiCOD = Convert.ToInt32(phiCOD),
                        TinhTrang = "Đang liên hệ nhân viên nhận hàng"
                    };

                    dbContext.DONHANGs.Add(newDonHang);
                    dbContext.SaveChanges();

                    MessageBox.Show("Tạo đơn hàng thành công! Mã vận đơn mới: " + newDonHang.MaVanDon.ToString(), "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tạo đơn hàng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
