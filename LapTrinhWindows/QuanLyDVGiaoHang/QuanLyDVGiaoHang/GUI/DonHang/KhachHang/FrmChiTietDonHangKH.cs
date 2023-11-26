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
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyDVGiaoHang.GUI.DonHang
{
    public partial class FrmChiTietDonHangKH : Form
    {
        DataTable dtTinhHuyenXa;
        List<TinhHuyenXa> TinhHuyenXaList;
        public DONHANG dONHANG { get; set; }
        public FrmChiTietDonHangKH()
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
        private void LoadData()
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
            //1
            string input = dONHANG.DiaChiNguoiGui;
            string[] DiaChi = input.Split(',');
            int n = DiaChi.Length;
            string Tinh = DiaChi[n - 1].Trim();
            string Huyen = DiaChi[n - 2].Trim();
            string Xa = DiaChi[n - 3].Trim();
            var tinhList = TinhHuyenXaList.Select(t => t.Tinh).Distinct().ToList();
            cbTinh.DataSource = tinhList;
            int index = tinhList.FindIndex(t => t == Tinh);
            cbTinh.SelectedIndex = index;
            var huyenList = TinhHuyenXaList.Where(t => t.Tinh == Tinh).Select(t => t.Huyen).Distinct().ToList();
            cbHuyen.DataSource = huyenList;
            index = huyenList.FindIndex(h => h == Huyen);
            cbHuyen.SelectedIndex = index;
            var xaList = TinhHuyenXaList.Where(t => t.Tinh == Tinh && t.Huyen == Huyen).Select(t => t.Xa).Distinct().ToList();
            cbXa.DataSource = xaList;
            index = xaList.FindIndex(x => x == Xa);
            cbXa.SelectedIndex = index;
            string diachi = string.Join(",", DiaChi.Take(n - 3).Select(s => s.Trim()));
            txtDiaChi.Text = diachi;
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


            //txtDiaChi1.Text = diachi;

            //string input1 = dONHANG.DiaChiNguoiGui;
            //string[] DiaChi1 = input1.Split(',');
            //int n1 = DiaChi1.Length;
            //string Tinh1 = DiaChi1[n1 - 1].Trim();
            //string Huyen1 = DiaChi1[n1 - 2].Trim();
            //string Xa1 = DiaChi1[n1 - 3].Trim();
            //int index1 = cbTinh.FindStringExact(Tinh1);
            //cbTinh.SelectedIndex = index1;
            //index1 = cbHuyen.FindStringExact(Huyen1);
            //cbHuyen.SelectedIndex = index1;
            //index1 = cbXa.FindStringExact(Xa1);
            //cbXa.SelectedIndex = index1;
            //string diachi1 = "";
            //for (int i = 0; i < n1 - 3; i++)
            //{
            //    diachi1 += DiaChi1[i].Trim();
            //    if (i + 1 != n1 - 3)
            //        diachi1 += ",";
            //}
            //txtDiaChi1.Text = diachi1;

            //string input2 = dONHANG.DiaChiNguoiNhan;
            //string[] DiaChi2 = input2.Split(',');
            //int n2 = DiaChi2.Length;
            //string Tinh2 = DiaChi2[n2 - 1].Trim();
            //string Huyen2 = DiaChi2[n2 - 2].Trim();
            //string Xa2 = DiaChi2[n2 - 3].Trim();
            //int index2 = cbTinh2.FindStringExact(Tinh2);
            //cbTinh2.SelectedIndex = index2;
            //index2 = cbHuyen2.FindStringExact(Huyen2);
            //cbHuyen2.SelectedIndex = index2;
            //index2 = cbXa2.FindStringExact(Xa2);
            //cbXa2.SelectedIndex = index2;
            //string diachi2 = "";
            //for (int i = 0; i < n2 - 3; i++)
            //{
            //    diachi2 += DiaChi2[i].Trim();
            //    if (i + 1 != n2 - 3)
            //        diachi2 += ",";
            //}
            //txtDiaChi2.Text = diachi2;


        }


        private void FrmChiTietDonHangKH_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cbHuyen1_SelectedValueChanged(object sender, EventArgs e)
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

        private void cbTinh1_SelectedValueChanged(object sender, EventArgs e)
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

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát bảng chi tiết đơn hàng ?", "Thông Báo",
              MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

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
