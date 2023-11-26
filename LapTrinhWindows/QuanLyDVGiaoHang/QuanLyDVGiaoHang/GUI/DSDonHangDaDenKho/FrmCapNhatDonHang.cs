using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using QuanLyDVGiaoHang.Models;
using QuanLyDVGiaoHang.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyDVGiaoHang.GUI.DSDonHangDaDenKho
{
    public partial class FrmCapNhatDonHang : Form
    {
        DataTable dtTinhHuyenXa;
        List<TinhHuyenXa> TinhHuyenXaList;
        public DONHANG dONHANG { get; set; }
        public FrmCapNhatDonHang()
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
        private void loadData()
        {
            LoadTinhHuyenXa();
            txtTenNguoiGui.Text = dONHANG.NguoiGui;
            txtTenNguoiNhan.Text = dONHANG.NguoiNhan;
            txtMaVanDon.Text = dONHANG.MaVanDon.ToString();
            txtPhiCOD.Text = dONHANG.PhiCOD.ToString();
            txtPhiVanChuyen.Text = dONHANG.PhiVanChuyen.ToString();
            txtThongTinHangHoa.Text = dONHANG.ThongTinHangHoa;
            txtTinhTrang.Text = dONHANG.TinhTrang;

            using (var context = new QuanLyDVGiaoHangEntities())
            {
                var LoaiHangHoaList = context.LOAIHANGHOAs.ToList();
                cbLoaiHangHoa.DataSource = LoaiHangHoaList;
                cbLoaiHangHoa.DisplayMember = "TenLoai";
                cbLoaiHangHoa.ValueMember = "MaLoai";

                var BangGiaList = context.BANGGIAs.ToList();
                cbBangGia.DataSource = BangGiaList;
                cbBangGia.DisplayMember = "KhoiLuong";
                cbBangGia.ValueMember = "KhoiLuong";
            }

            dtpNgayTaoDon.Value = dONHANG.NgayTaoDon ?? DateTime.Now;
            cbBangGia.SelectedValue = dONHANG.KhoiLuong ?? "";
            cbLoaiHangHoa.SelectedValue = dONHANG.LoaiHangHoa ?? "";

            // 1
            string input1 = dONHANG.DiaChiNguoiGui;
            string[] DiaChi1 = input1.Split(',');
            int n1 = DiaChi1.Length;
            string Tinh1 = DiaChi1[n1 - 1].Trim();
            string Huyen1 = DiaChi1[n1 - 2].Trim();
            string Xa1 = DiaChi1[n1 - 3].Trim();

            var tinhList1 = TinhHuyenXaList.Select(t => t.Tinh).Distinct().ToList();
            cbTinh1.DataSource = tinhList1;
            int index1 = tinhList1.FindIndex(t => t == Tinh1);
            cbTinh1.SelectedIndex = index1;

            var huyenList1 = TinhHuyenXaList.Where(t => t.Tinh == Tinh1).Select(t => t.Huyen).Distinct().ToList();
            cbHuyen1.DataSource = huyenList1;
            index1 = huyenList1.FindIndex(h => h == Huyen1);
            cbHuyen1.SelectedIndex = index1;

            var xaList1 = TinhHuyenXaList.Where(t => t.Tinh == Tinh1 && t.Huyen == Huyen1).Select(t => t.Xa).Distinct().ToList();
            cbXa1.DataSource = xaList1;
            index1 = xaList1.FindIndex(x => x == Xa1);
            cbXa1.SelectedIndex = index1;

            string diachi1 = string.Join(",", DiaChi1.Take(n1 - 3).Select(s => s.Trim()));
            txtDiaChi1.Text = diachi1;


            // 2
            string input2 = dONHANG.DiaChiNguoiNhan;
            string[] DiaChi2 = input2.Split(',');
            int n2 = DiaChi2.Length;
            string Tinh2 = DiaChi2[n2 - 1].Trim();
            string Huyen2 = DiaChi2[n2 - 2].Trim();
            string Xa2 = DiaChi2[n2 - 3].Trim();

            var tinhList2 = TinhHuyenXaList.Select(t => t.Tinh).Distinct().ToList();
            cbTinh2.DataSource = tinhList2;
            int index2 = tinhList2.FindIndex(t => t == Tinh2);
            cbTinh2.SelectedIndex = index2;

            var huyenList2 = TinhHuyenXaList.Where(t => t.Tinh == Tinh2).Select(t => t.Huyen).Distinct().ToList();
            cbHuyen2.DataSource = huyenList2;
            index2 = huyenList2.FindIndex(h => h == Huyen2);
            cbHuyen2.SelectedIndex = index2;

            var xaList2 = TinhHuyenXaList.Where(t => t.Tinh == Tinh2 && t.Huyen == Huyen2).Select(t => t.Xa).Distinct().ToList();
            cbXa2.DataSource = xaList2;
            index2 = xaList2.FindIndex(x => x == Xa2);
            cbXa2.SelectedIndex = index2;

            string diachi2 = string.Join(",", DiaChi2.Take(n2 - 3).Select(s => s.Trim()));
            txtDiaChi2.Text = diachi2;

        }


        private void FrmCapNhatDonHang_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng cập nhật đơn hàng ?", "Thông Báo",
              MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void cbTinh1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbTinh1.SelectedItem != null)
            {
                string selectedTinh = cbTinh1.SelectedItem.ToString();
                List<string> huyenList = TinhHuyenXaList
                    .Where(thx => thx.Tinh == selectedTinh)
                    .Select(thx => thx.Huyen)
                    .Distinct()
                    .ToList();

                cbHuyen1.DataSource = huyenList;
                cbHuyen1.SelectedItem = null;
                cbHuyen1.Text = "Huyện / Quận";
            }
        }

        private void cbTinh2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbTinh2.SelectedItem != null)
            {
                string selectedTinh = cbTinh2.SelectedItem.ToString();
                List<string> huyenList = TinhHuyenXaList
                    .Where(thx => thx.Tinh == selectedTinh)
                    .Select(thx => thx.Huyen)
                    .Distinct()
                    .ToList();

                cbHuyen2.DataSource = huyenList;
                cbHuyen2.SelectedItem = null;
                cbHuyen2.Text = "Huyện / Quận";
            }
        }

        private void cbHuyen1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbHuyen1.SelectedItem != null)
            {
                string selectedHuyen = cbHuyen1.SelectedItem.ToString();
                List<string> xaList = TinhHuyenXaList
                    .Where(thx => thx.Tinh == cbTinh1.SelectedItem.ToString() && thx.Huyen == selectedHuyen)
                    .Select(thx => thx.Xa)
                    .Distinct()
                    .ToList();

                cbXa1.DataSource = xaList;
                cbXa1.SelectedItem = null;
                cbXa1.Text = "Xã / Phường";
            }
        }

        private void cbHuyen2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbHuyen2.SelectedItem != null)
            {
                string selectedHuyen = cbHuyen2.SelectedItem.ToString();
                List<string> xaList = TinhHuyenXaList
                    .Where(thx => thx.Tinh == cbTinh2.SelectedItem.ToString() && thx.Huyen == selectedHuyen)
                    .Select(thx => thx.Xa)
                    .Distinct()
                    .ToList();

                cbXa2.DataSource = xaList;
                cbXa2.SelectedItem = null;
                cbXa2.Text = "Xã / Phường";
            }
        }

        private void btnCapNhatDonHang_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtPhiVanChuyen.Text == "" || cbLoaiHangHoa.Text == "" || cbBangGia.Text == "")
                {
                    MessageBox.Show("Vui lòng nhập/chọn các ô cần thiết", "Thông Báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }    
                string nguoiNhan = txtTenNguoiNhan.Text;
                string Tinh1 = cbTinh1.SelectedValue.ToString();
                string Huyen1 = cbHuyen1.SelectedValue.ToString();
                string Xa1 = cbXa1.SelectedValue.ToString();
                string diaChiNguoiGui = txtDiaChi1.Text + ", " + Xa1 + ", " + Huyen1 + ", " + Tinh1;
                string Tinh2 = cbTinh2.SelectedValue.ToString();
                string Huyen2 = cbHuyen2.SelectedValue.ToString();
                string Xa2 = cbXa2.SelectedValue.ToString();
                string diaChiNguoiNhan = txtDiaChi2.Text + ", " + Xa2 + ", " + Huyen2 + ", " + Tinh2;
                string khoiLuong = cbBangGia.SelectedValue.ToString();
                string loaiHangHoa = cbLoaiHangHoa.SelectedValue.ToString();
                string thongTinHangHoa = txtThongTinHangHoa.Text;
                string phiCOD = txtPhiCOD.Text;
                string phiVanChuyen = txtPhiVanChuyen.Text;

                using (var dbContext = new QuanLyDVGiaoHangEntities())
                {
                    if (int.TryParse(txtMaVanDon.Text.Trim(), out int maVanDon))
                    {
                        var donhang = dbContext.DONHANGs.FirstOrDefault(nv => nv.MaVanDon == maVanDon);
                        if (donhang != null)
                        {
                            donhang.NguoiNhan = nguoiNhan;
                            donhang.DiaChiNguoiGui = diaChiNguoiGui;
                            donhang.DiaChiNguoiNhan = diaChiNguoiNhan;
                            donhang.ThongTinHangHoa = thongTinHangHoa;
                            donhang.KhoiLuong = khoiLuong;
                            donhang.LoaiHangHoa = loaiHangHoa;
                            donhang.PhiCOD = !string.IsNullOrEmpty(phiCOD) ? Convert.ToInt32(phiCOD) : (int?)null;
                            donhang.PhiVanChuyen = !string.IsNullOrEmpty(phiVanChuyen) ? Convert.ToInt32(phiVanChuyen) : (int?)null;
                            //donhang.TinhTrang = "Đang kiểm tra đơn hàng thành công";

                            dbContext.SaveChanges();

                            MessageBox.Show("Cập nhật đơn hàng thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy đơn hàng với mã đơn hàng đã cho!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mã vận đơn không hợp lệ!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi cập nhật thông tin đơn hàng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool cbBangGiaHasError = false;

        private void cbBangGia_Validated(object sender, EventArgs e)
        {
            if (!cbBangGiaHasError)
            {
                errorProvider1.SetError(cbBangGia, "");
            }
        }

        private void cbBangGia_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbBangGia.SelectedValue?.ToString()))
            {
                cbBangGiaHasError = true;
                errorProvider1.SetError(cbBangGia, "Vui lòng chọn một giá trị!");
            }
            else
            {
                cbBangGiaHasError = false;
                errorProvider1.SetError(cbBangGia, "");
            }
        }
        private bool cbLoaiHangHoaHasError = false;
        private void cbLoaiHangHoa_Validated(object sender, EventArgs e)
        {
            if (!cbLoaiHangHoaHasError)
            {
                errorProvider1.SetError(cbLoaiHangHoa, "");
            }
        }

        private void cbLoaiHangHoa_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbLoaiHangHoa.SelectedValue?.ToString()))
            {
                cbLoaiHangHoaHasError = true;
                errorProvider1.SetError(cbLoaiHangHoa, "Vui lòng chọn một giá trị!");
            }
            else
            {
                cbLoaiHangHoaHasError = false;
                errorProvider1.SetError(cbLoaiHangHoa, "");
            }
        }
        private bool txtPhiVanChuyenHasError = false;

        private void txtPhiVanChuyen_Validated(object sender, EventArgs e)
        {
            if (!txtPhiVanChuyenHasError)
            {
                errorProvider1.SetError(txtPhiVanChuyen, "");
            }
        }

        private void txtPhiVanChuyen_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPhiVanChuyen.Text))
            {
                txtPhiVanChuyenHasError = true;
                errorProvider1.SetError(txtPhiVanChuyen, "Vui lòng nhập một giá trị!");
            }
            else
            {
                txtPhiVanChuyenHasError = false;
                errorProvider1.SetError(txtPhiVanChuyen, "");
            }
        }
    }
}

