using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace doan
{
    public partial class UC_DiemSo : UserControl
    {
        private GroupBox gbNhapDiem;
        private ComboBox cbLocLop, cbSinhVien, cbMonHoc;
        private Label lblLocLop, lblTimKiem;
        private TextBox txtDiem, txtTimKiem;
        private Button btnTimKiem, btnThem, btnSua, btnXoa, btnLamMoi;
        private DataGridView dgvDiemSo;

        private ErrorProvider errorProvider = new ErrorProvider();
        private List<DiemModel> dsDiem = new List<DiemModel>();

        public UC_DiemSo()
        {
            InitializeComponent();
            InitCustomControls();
            ApplyModernLayout();
            InitTableColumns();

            // SỰ KIỆN LOAD: Nơi an toàn nhất để nạp dữ liệu và phân quyền
            this.Load += (s, e) => {
                LoadData();
                LoadComboBoxClass();
                ThucHienPhanQuyen(); // <--- GỌI HÀM PHÂN QUYỀN TẠI ĐÂY
            };

            this.Resize += (s, e) => UpdateLayoutPositions();
            RegisterEvents();
        }

        // --- HÀM PHÂN QUYỀN RIÊNG BIỆT ---
        private void ThucHienPhanQuyen()
        {
            // Kiểm tra: Nếu KHÔNG PHẢI Admin thì ẩn chức năng
            if (UserSession.QuyenHan != "Admin")
            {
                // 1. Ẩn vùng nhập liệu
                gbNhapDiem.Visible = false;

                // 2. Ẩn các nút thao tác
                btnThem.Visible = false;
                btnSua.Visible = false;
                btnXoa.Visible = false;
                btnLamMoi.Visible = false;

                // 3. Kéo bảng lên trên lấp chỗ trống
                if (dgvDiemSo != null)
                {
                    dgvDiemSo.Dock = DockStyle.Fill; // Cho tràn màn hình luôn
                    dgvDiemSo.BringToFront();
                }
            }
        }

        private void InitCustomControls()
        {
            if (gbNhapDiem == null) gbNhapDiem = new GroupBox();
            if (cbLocLop == null) cbLocLop = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            if (lblLocLop == null) lblLocLop = new Label { Text = "Lọc Lớp:" };

            if (cbSinhVien == null) cbSinhVien = new ComboBox { DropDownStyle = ComboBoxStyle.DropDown, AutoCompleteMode = AutoCompleteMode.SuggestAppend, AutoCompleteSource = AutoCompleteSource.ListItems };
            if (cbMonHoc == null) cbMonHoc = new ComboBox { DropDownStyle = ComboBoxStyle.DropDown, AutoCompleteMode = AutoCompleteMode.SuggestAppend, AutoCompleteSource = AutoCompleteSource.ListItems };

            if (txtDiem == null) txtDiem = new TextBox();
            if (txtTimKiem == null) txtTimKiem = new TextBox();
            if (btnTimKiem == null) btnTimKiem = new Button { Text = "Tìm" };
            if (lblTimKiem == null) lblTimKiem = new Label { Text = "Tìm kiếm:" };

            if (btnThem == null) btnThem = new Button { Text = "Nhập điểm" };
            if (btnSua == null) btnSua = new Button { Text = "Sửa điểm" };
            if (btnXoa == null) btnXoa = new Button { Text = "Xóa" };
            if (btnLamMoi == null) btnLamMoi = new Button { Text = "Làm mới" };

            if (dgvDiemSo == null) dgvDiemSo = new DataGridView();

            if (cbMonHoc.Items.Count == 0)
                cbMonHoc.Items.AddRange(new object[] { "Lập trình C#", "Cấu trúc dữ liệu", "Cơ sở dữ liệu", "Tiếng Anh CN", "Toán cao cấp" });

            if (!this.Controls.Contains(gbNhapDiem)) this.Controls.Add(gbNhapDiem);
            if (!gbNhapDiem.Controls.Contains(cbSinhVien))
                gbNhapDiem.Controls.AddRange(new Control[] { lblLocLop, cbLocLop, cbSinhVien, cbMonHoc, txtDiem, lblTimKiem, txtTimKiem, btnTimKiem });
            if (!this.Controls.Contains(dgvDiemSo)) this.Controls.Add(dgvDiemSo);
        }

        private void ApplyModernLayout()
        {
            UIHelper.Beautify(this);

            Control[] buttons = { btnThem, btnSua, btnXoa, btnLamMoi };
            foreach (var btn in buttons)
            {
                if (gbNhapDiem.Controls.Contains(btn)) gbNhapDiem.Controls.Remove(btn);
                this.Controls.Add(btn);
                btn.BringToFront();
            }

            UIHelper.StyleButton(btnThem, UIHelper.SuccessColor);
            UIHelper.StyleButton(btnSua, UIHelper.WarningColor);
            UIHelper.StyleButton(btnXoa, UIHelper.DangerColor);
            UIHelper.StyleButton(btnLamMoi, UIHelper.InfoColor);
            UIHelper.StyleButton(btnTimKiem, UIHelper.PrimaryColor); btnTimKiem.Size = new Size(80, 30);

            gbNhapDiem.Text = "NHẬP ĐIỂM CHI TIẾT";
            gbNhapDiem.Dock = DockStyle.Top;
            gbNhapDiem.Height = 170;
            gbNhapDiem.BackColor = UIHelper.White;

            int y1 = 40; int inputH = 30;
            lblLocLop.Location = new Point(30, y1 + 5); lblLocLop.AutoSize = true;
            cbLocLop.Location = new Point(150, y1); cbLocLop.Size = new Size(180, inputH);

            cbSinhVien.Location = new Point(350, y1); cbSinhVien.Size = new Size(240, inputH);
            if (string.IsNullOrEmpty(cbSinhVien.Text)) cbSinhVien.Text = "-- Chọn sinh viên --";

            cbMonHoc.Location = new Point(610, y1); cbMonHoc.Size = new Size(180, inputH);
            if (string.IsNullOrEmpty(cbMonHoc.Text)) cbMonHoc.Text = "-- Chọn môn --";

            txtDiem.Location = new Point(810, y1); txtDiem.Size = new Size(80, inputH); txtDiem.PlaceholderText = "Điểm";

            int y2 = 90;
            lblTimKiem.Location = new Point(30, y2 + 5); lblTimKiem.AutoSize = true;
            txtTimKiem.Location = new Point(150, y2); txtTimKiem.Size = new Size(250, inputH);
            txtTimKiem.PlaceholderText = "Nhập mã số để tìm...";
            btnTimKiem.Location = new Point(410, y2 - 2);

            dgvDiemSo.Dock = DockStyle.None;
            dgvDiemSo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            UpdateLayoutPositions();
        }

        private void UpdateLayoutPositions()
        {
            if (gbNhapDiem == null || btnThem == null) return;
            // Chỉ cập nhật vị trí nút nếu nó đang hiện (Admin)
            if (btnThem.Visible)
            {
                int btnY = gbNhapDiem.Height + 15;
                int btnStartX = (this.Width - (4 * 110 + 3 * 20)) / 2;
                btnThem.Location = new Point(btnStartX, btnY);
                btnSua.Location = new Point(btnThem.Right + 20, btnY);
                btnXoa.Location = new Point(btnSua.Right + 20, btnY);
                btnLamMoi.Location = new Point(btnXoa.Right + 20, btnY);

                if (dgvDiemSo != null)
                {
                    dgvDiemSo.Location = new Point(0, btnY + 50);
                    dgvDiemSo.Size = new Size(this.Width, this.Height - dgvDiemSo.Top);
                }
            }
        }

        // --- CORE LOGIC ---
        private void InitTableColumns()
        {
            dgvDiemSo.AutoGenerateColumns = false; dgvDiemSo.Columns.Clear();
            AddTextColumn("MaSV", "Mã SV", "MaSV", 100); AddTextColumn("TenSV", "Sinh Viên", "TenSV", 200);
            AddTextColumn("Mon", "Môn Học", "Mon", 200); AddTextColumn("Diem", "Điểm Số", "Diem", 100);
        }
        private void AddTextColumn(string name, string header, string prop, int width) { var col = new DataGridViewTextBoxColumn { Name = name, HeaderText = header, DataPropertyName = prop, Width = width }; dgvDiemSo.Columns.Add(col); }
        private void LoadData() { dsDiem = DataHelper.DocDiem(); BindGrid(dsDiem); }
        private void BindGrid(List<DiemModel> data) { var binding = new BindingSource(); binding.DataSource = data; dgvDiemSo.DataSource = binding; }
        private void LoadComboBoxClass()
        {
            var listLop = DataHelper.DocLop(); cbLocLop.Items.Clear(); cbLocLop.Items.Add("--- Tất cả ---");
            foreach (var lop in listLop) cbLocLop.Items.Add(lop.TenLop); cbLocLop.SelectedIndex = 0;
        }
        private void LoadComboBoxSinhVien(string tenLop = null)
        {
            var listSV = DataHelper.DocSV(); if (!string.IsNullOrEmpty(tenLop) && tenLop != "--- Tất cả ---") listSV = listSV.Where(x => x.Lop == tenLop).ToList();
            cbSinhVien.DataSource = null; cbSinhVien.DataSource = listSV; cbSinhVien.DisplayMember = "HoTen"; cbSinhVien.ValueMember = "MaSV"; cbSinhVien.SelectedIndex = -1;
        }

        private void RegisterEvents()
        {
            btnThem.Click -= BtnThem_Click; btnThem.Click += BtnThem_Click;
            btnSua.Click -= BtnSua_Click; btnSua.Click += BtnSua_Click;
            btnXoa.Click -= BtnXoa_Click; btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click -= (s, e) => LamMoiForm();
            dgvDiemSo.CellClick -= DgvDiemSo_CellClick; dgvDiemSo.CellClick += DgvDiemSo_CellClick;
            btnTimKiem.Click -= BtnTimKiem_Click; btnTimKiem.Click += BtnTimKiem_Click;
            txtTimKiem.KeyDown -= (s, e) => { if (e.KeyCode == Keys.Enter) { ThucHienTimKiem(); e.SuppressKeyPress = true; } };
            cbLocLop.SelectedIndexChanged += (s, e) => LoadComboBoxSinhVien(cbLocLop.SelectedItem?.ToString());
        }

        private void BtnTimKiem_Click(object sender, EventArgs e) => ThucHienTimKiem();
        private void ThucHienTimKiem() { string k = txtTimKiem.Text.Trim().ToLower(); if (string.IsNullOrEmpty(k)) BindGrid(dsDiem); else BindGrid(dsDiem.Where(d => d.MaSV.ToLower().Contains(k) || d.TenSV.ToLower().Contains(k)).ToList()); }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;
            var sv = cbSinhVien.SelectedItem as SinhVien;
            if (dsDiem.Any(d => d.MaSV == sv.MaSV && d.Mon == cbMonHoc.Text)) { MessageBox.Show("Đã có điểm. Dùng nút Sửa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            dsDiem.Add(new DiemModel { MaSV = sv.MaSV, TenSV = sv.HoTen, Mon = cbMonHoc.Text, Diem = double.Parse(txtDiem.Text) });
            LuuVaTaiLai(); MessageBox.Show("Nhập thành công!");
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (cbSinhVien.Enabled) { MessageBox.Show("Chọn dòng dưới bảng!"); return; }
            if (!ValidateInput()) return;
            var sv = cbSinhVien.SelectedItem as SinhVien;
            var item = dsDiem.FirstOrDefault(d => d.MaSV == sv.MaSV && d.Mon == cbMonHoc.Text);
            if (item != null) { item.Diem = double.Parse(txtDiem.Text); LuuVaTaiLai(); MessageBox.Show("Sửa thành công!"); }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDiemSo.CurrentRow == null) return;
            var item = dgvDiemSo.CurrentRow.DataBoundItem as DiemModel;
            if (item != null && MessageBox.Show("Xóa?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) { dsDiem.Remove(item); LuuVaTaiLai(); }
        }

        private void LuuVaTaiLai() { DataHelper.LuuDiem(dsDiem); if (!string.IsNullOrEmpty(txtTimKiem.Text)) ThucHienTimKiem(); else LoadData(); LamMoiForm(); }

        private bool ValidateInput()
        {
            errorProvider.Clear();
            if (cbSinhVien.SelectedIndex < 0) { errorProvider.SetError(cbSinhVien, "Chọn SV!"); return false; }
            if (cbMonHoc.SelectedIndex < 0 || cbMonHoc.Text == "-- Chọn môn --") { errorProvider.SetError(cbMonHoc, "Chọn môn!"); return false; }
            if (!double.TryParse(txtDiem.Text, out double d) || d < 0 || d > 10) { errorProvider.SetError(txtDiem, "Điểm 0-10!"); return false; }
            return true;
        }

        private void DgvDiemSo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvDiemSo.Rows[e.RowIndex].DataBoundItem as DiemModel;
            if (row == null) return;
            cbMonHoc.Text = row.Mon; txtDiem.Text = row.Diem.ToString();
            LoadComboBoxSinhVien("--- Tất cả ---");
            foreach (SinhVien item in cbSinhVien.Items) { if (item.MaSV == row.MaSV) { cbSinhVien.SelectedItem = item; break; } }
            cbLocLop.Enabled = false; cbSinhVien.Enabled = false; cbMonHoc.Enabled = false;
        }

        private void LamMoiForm()
        {
            cbSinhVien.SelectedIndex = -1; cbSinhVien.Text = "-- Chọn sinh viên --";
            cbMonHoc.SelectedIndex = -1; cbMonHoc.Text = "-- Chọn môn --";
            txtDiem.Clear(); cbLocLop.Enabled = true; cbSinhVien.Enabled = true; cbMonHoc.Enabled = true;
            errorProvider.Clear(); LoadComboBoxSinhVien(cbLocLop.SelectedItem?.ToString());
        }
    }
}