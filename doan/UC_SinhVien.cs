using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace doan
{
    public partial class UC_SinhVien : UserControl
    {
        private List<SinhVien> listSV = new List<SinhVien>();

        public UC_SinhVien()
        {
            InitializeComponent();

            // --- GỌI HÀM LÀM ĐẸP & CĂN CHỈNH ---
            ApplyProfessionalUI();

            // --- DATA & EVENTS ---
            LoadData();
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnLamMoi.Click += btnLamMoi_Click;
            dgvSinhVien.CellClick += dgvSinhVien_CellClick;
            txtTimKiem.TextChanged += txtTimKiem_TextChanged;
        }

        private void ApplyProfessionalUI()
        {
            UIHelper.Beautify(this);

            // Tô màu nút chức năng
            UIHelper.StyleButton(btnThem, Color.MediumSeaGreen);
            UIHelper.StyleButton(btnSua, Color.Orange);
            UIHelper.StyleButton(btnXoa, Color.Crimson);
            UIHelper.StyleButton(btnLamMoi, Color.DodgerBlue);

            // CĂN CHỈNH VỊ TRÍ (Layout Fix)
            groupBox1.Text = "THÔNG TIN SINH VIÊN";
            int xLabel = 30, xInput = 140, yStart = 40, gap = 40;

            // Dòng 1
            label1.Location = new Point(xLabel, yStart);
            txtMaSV.Location = new Point(xInput, yStart - 3); txtMaSV.Size = new Size(150, 30);

            label7.Location = new Point(320, yStart); label7.Text = "Tìm kiếm:";
            txtTimKiem.Location = new Point(400, yStart - 3); txtTimKiem.Size = new Size(200, 30);

            // Dòng 2
            label2.Location = new Point(xLabel, yStart + gap);
            txtHoTen.Location = new Point(xInput, yStart + gap - 3); txtHoTen.Size = new Size(460, 30);

            // Dòng 3
            label3.Location = new Point(xLabel, yStart + gap * 2);
            txtEmail.Location = new Point(xInput, yStart + gap * 2 - 3); txtEmail.Size = new Size(460, 30);

            // Dòng 4
            label4.Location = new Point(xLabel, yStart + gap * 3);
            txtSDT.Location = new Point(xInput, yStart + gap * 3 - 3); txtSDT.Size = new Size(200, 30);

            label6.Location = new Point(370, yStart + gap * 3);
            rbNam.Location = new Point(440, yStart + gap * 3);
            rbNu.Location = new Point(510, yStart + gap * 3);

            // Dòng 5
            label5.Location = new Point(xLabel, yStart + gap * 4);
            dtpNgaySinh.Location = new Point(xInput, yStart + gap * 4 - 3); dtpNgaySinh.Size = new Size(200, 30);

            label8.Location = new Point(370, yStart + gap * 4);
            cbLop.Location = new Point(440, yStart + gap * 4 - 3); cbLop.Size = new Size(160, 30);

            // Nút bấm
            int btnY = 320;
            btnThem.Location = new Point(130, btnY);
            btnSua.Location = new Point(240, btnY);
            btnXoa.Location = new Point(350, btnY);
            btnLamMoi.Location = new Point(460, btnY);

            groupBox1.Height = 380; // Thu gọn GroupBox
        }

        private void LoadData()
        {
            listSV = DataHelper.DocSV();
            HienThiTable(listSV);
            cbLop.Items.Clear();
            foreach (var l in DataHelper.DocLop()) cbLop.Items.Add(l.TenLop);
        }

        private void HienThiTable(List<SinhVien> source)
        {
            dgvSinhVien.DataSource = null;
            dgvSinhVien.DataSource = source;
            if (dgvSinhVien.Columns.Count > 0)
            {
                dgvSinhVien.Columns["MaSV"].HeaderText = "Mã SV";
                dgvSinhVien.Columns["HoTen"].HeaderText = "Họ và Tên";
                dgvSinhVien.Columns["SoDienThoai"].HeaderText = "Điện Thoại";
                dgvSinhVien.Columns["NgaySinh"].HeaderText = "Ngày Sinh";
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text) || listSV.Any(x => x.MaSV == txtMaSV.Text))
            {
                MessageBox.Show("Mã SV không hợp lệ hoặc đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
            }
            XuLyLuu(true);
        }

        private void btnSua_Click(object sender, EventArgs e) { if (!txtMaSV.Enabled) XuLyLuu(false); }

        private void XuLyLuu(bool isNew)
        {
            SinhVien sv = isNew ? new SinhVien() : listSV.First(x => x.MaSV == txtMaSV.Text);
            sv.MaSV = txtMaSV.Text; sv.HoTen = txtHoTen.Text; sv.Email = txtEmail.Text;
            sv.SoDienThoai = txtSDT.Text; sv.NgaySinh = dtpNgaySinh.Value.ToString("dd/MM/yyyy");
            sv.GioiTinh = rbNam.Checked ? "Nam" : "Nữ"; sv.Lop = cbLop.Text;

            if (isNew) listSV.Add(sv);
            DataHelper.LuuSV(listSV); HienThiTable(listSV); btnLamMoi_Click(null, null);
            MessageBox.Show("Thao tác thành công!", "Thông báo");
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            var sv = listSV.FirstOrDefault(x => x.MaSV == txtMaSV.Text);
            if (sv != null && MessageBox.Show("Xóa sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                listSV.Remove(sv); DataHelper.LuuSV(listSV); HienThiTable(listSV); btnLamMoi_Click(null, null);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaSV.Clear(); txtHoTen.Clear(); txtEmail.Clear(); txtSDT.Clear();
            txtMaSV.Enabled = true; txtMaSV.Focus();
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            var keyword = txtTimKiem.Text.ToLower();
            HienThiTable(listSV.Where(x => x.HoTen.ToLower().Contains(keyword) || x.MaSV.ToLower().Contains(keyword)).ToList());
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var sv = dgvSinhVien.Rows[e.RowIndex].DataBoundItem as SinhVien;
                if (sv == null) return;
                txtMaSV.Text = sv.MaSV; txtHoTen.Text = sv.HoTen; txtEmail.Text = sv.Email;
                txtSDT.Text = sv.SoDienThoai; cbLop.Text = sv.Lop;
                try { dtpNgaySinh.Value = DateTime.ParseExact(sv.NgaySinh, "dd/MM/yyyy", null); } catch { dtpNgaySinh.Value = DateTime.Now; }
                if (sv.GioiTinh == "Nam") rbNam.Checked = true; else rbNu.Checked = true;
                txtMaSV.Enabled = false;
            }
        }
    }
}