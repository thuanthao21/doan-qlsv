using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace doan
{
    public partial class UC_LopHoc : UserControl
    {
        List<LopHocModel> danhSachLop = new List<LopHocModel>();

        public UC_LopHoc()
        {
            InitializeComponent();
            ApplyModernLayout();
            this.Load += (s, e) => { danhSachLop = DataHelper.DocLop(); HienThiTable(); };
            RegisterEvents();
        }

        private void ApplyModernLayout()
        {
            UIHelper.Beautify(this);
            UIHelper.StyleButton(btnThem, UIHelper.SuccessColor);
            UIHelper.StyleButton(btnSua, UIHelper.WarningColor);
            UIHelper.StyleButton(btnXoa, UIHelper.DangerColor);
            UIHelper.StyleButton(btnLamMoi, UIHelper.InfoColor);

            gbThongTin.Text = "THÔNG TIN LỚP HỌC";
            gbThongTin.Height = 220;
            gbThongTin.BackColor = UIHelper.White;

            // Bố cục 2 cột
            int col1_L = 50, col1_I = 150;
            int col2_L = 450, col2_I = 550;
            int row1 = 50, row2 = 100;
            int inputH = 32;

            lblMaLop.Location = new Point(col1_L, row1);
            txtMaLop.Location = new Point(col1_I, row1 - 5); txtMaLop.Size = new Size(250, inputH);

            lblKhoa.Location = new Point(col2_L, row1);
            cbKhoa.Location = new Point(col2_I, row1 - 5); cbKhoa.Size = new Size(250, inputH);

            lblTenLop.Location = new Point(col1_L, row2);
            txtTenLop.Location = new Point(col1_I, row2 - 5); txtTenLop.Size = new Size(650, inputH);

            // Nút bấm căn giữa
            int btnY = 160;
            int btnStartX = (this.Width - (4 * 110 + 3 * 20)) / 2;
            btnThem.Location = new Point(btnStartX, btnY);
            btnSua.Location = new Point(btnThem.Right + 20, btnY);
            btnXoa.Location = new Point(btnSua.Right + 20, btnY);
            btnLamMoi.Location = new Point(btnXoa.Right + 20, btnY);

            dgvLopHoc.Location = new Point(0, gbThongTin.Bottom + 20);
            dgvLopHoc.Height = this.Height - dgvLopHoc.Top - 10;
        }

        private void RegisterEvents()
        {
            btnThem.Click -= BtnThem_Click; btnThem.Click += BtnThem_Click;
            btnSua.Click -= BtnSua_Click; btnSua.Click += BtnSua_Click;
            btnXoa.Click -= BtnXoa_Click; btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click -= (s, e) => LamMoiForm(); btnLamMoi.Click += (s, e) => LamMoiForm();
            dgvLopHoc.CellClick -= DgvLopHoc_CellClick; dgvLopHoc.CellClick += DgvLopHoc_CellClick;
        }

        // (Logic giữ nguyên)
        private void HienThiTable() { dgvLopHoc.DataSource = null; dgvLopHoc.DataSource = danhSachLop; if (dgvLopHoc.Columns.Count > 0) { dgvLopHoc.Columns[0].HeaderText = "Mã Lớp"; dgvLopHoc.Columns[1].HeaderText = "Tên Lớp"; dgvLopHoc.Columns[2].HeaderText = "Khoa"; } }
        private void BtnThem_Click(object sender, EventArgs e) { if (string.IsNullOrWhiteSpace(txtMaLop.Text) || danhSachLop.Any(x => x.MaLop == txtMaLop.Text)) { MessageBox.Show("Mã lớp lỗi/trùng!"); return; } danhSachLop.Add(new LopHocModel { MaLop = txtMaLop.Text, TenLop = txtTenLop.Text, Khoa = cbKhoa.Text }); DataHelper.LuuLop(danhSachLop); HienThiTable(); LamMoiForm(); }
        private void BtnSua_Click(object sender, EventArgs e) { var item = danhSachLop.FirstOrDefault(x => x.MaLop == txtMaLop.Text); if (item != null) { item.TenLop = txtTenLop.Text; item.Khoa = cbKhoa.Text; DataHelper.LuuLop(danhSachLop); HienThiTable(); MessageBox.Show("Đã cập nhật!"); } }
        private void BtnXoa_Click(object sender, EventArgs e) { var item = danhSachLop.FirstOrDefault(x => x.MaLop == txtMaLop.Text); if (item == null) return; if (DataHelper.DocSV().Any(sv => sv.Lop == item.TenLop)) { MessageBox.Show($"Lớp {item.TenLop} đang có sinh viên!"); return; } if (MessageBox.Show("Xóa lớp này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes) { danhSachLop.Remove(item); DataHelper.LuuLop(danhSachLop); HienThiTable(); LamMoiForm(); } }
        private void DgvLopHoc_CellClick(object sender, DataGridViewCellEventArgs e) { if (e.RowIndex >= 0) { var row = dgvLopHoc.Rows[e.RowIndex]; txtMaLop.Text = row.Cells[0].Value?.ToString(); txtTenLop.Text = row.Cells[1].Value?.ToString(); cbKhoa.Text = row.Cells[2].Value?.ToString(); txtMaLop.Enabled = false; } }
        private void LamMoiForm() { txtMaLop.Clear(); txtTenLop.Clear(); cbKhoa.SelectedIndex = -1; txtMaLop.Enabled = true; }
    }
}