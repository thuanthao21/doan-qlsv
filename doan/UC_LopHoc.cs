using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace doan
{
    public partial class UC_LopHoc : UserControl
    {
        private List<LopHocModel> danhSachLop = new List<LopHocModel>();

        // Khai báo ErrorProvider để báo lỗi nhập liệu chuyên nghiệp
        private ErrorProvider errorProvider = new ErrorProvider();

        public UC_LopHoc()
        {
            InitializeComponent();

            // 1. Sắp xếp giao diện & Nút bấm
            ApplyModernLayout();

            // 2. Tạo cột bảng (chỉ 1 lần để không bị nháy)
            InitTableColumns();

            // 3. Load dữ liệu
            this.Load += (s, e) => {
                LoadData();
                LoadComboBoxKhoa();
            };

            // 4. Đăng ký sự kiện
            RegisterEvents();
        }

        // --- 1. GIAO DIỆN (Đã fix lỗi nút bị che) ---
        private void ApplyModernLayout()
        {
            UIHelper.Beautify(this);

            // [QUAN TRỌNG] Lôi nút ra khỏi GroupBox để không bị che
            if (gbThongTin.Controls.Contains(btnThem)) gbThongTin.Controls.Remove(btnThem);
            if (gbThongTin.Controls.Contains(btnSua)) gbThongTin.Controls.Remove(btnSua);
            if (gbThongTin.Controls.Contains(btnXoa)) gbThongTin.Controls.Remove(btnXoa);
            if (gbThongTin.Controls.Contains(btnLamMoi)) gbThongTin.Controls.Remove(btnLamMoi);

            this.Controls.Add(btnThem);
            this.Controls.Add(btnSua);
            this.Controls.Add(btnXoa);
            this.Controls.Add(btnLamMoi);

            // Đưa nút lên trên cùng
            btnThem.BringToFront(); btnSua.BringToFront(); btnXoa.BringToFront(); btnLamMoi.BringToFront();

            // Style nút
            UIHelper.StyleButton(btnThem, UIHelper.SuccessColor);
            UIHelper.StyleButton(btnSua, UIHelper.WarningColor);
            UIHelper.StyleButton(btnXoa, UIHelper.DangerColor);
            UIHelper.StyleButton(btnLamMoi, UIHelper.InfoColor);

            // GroupBox
            gbThongTin.Text = "THÔNG TIN LỚP HỌC";
            gbThongTin.Dock = DockStyle.Top;
            gbThongTin.Height = 200;
            gbThongTin.BackColor = UIHelper.White;

            // Căn chỉnh ô nhập liệu
            int col1_L = 50, col1_I = 150;
            int col2_L = 450, col2_I = 550;
            int row1 = 50, row2 = 100;
            int inputH = 30;

            lblMaLop.Location = new Point(col1_L, row1);
            txtMaLop.Location = new Point(col1_I, row1 - 5); txtMaLop.Size = new Size(250, inputH);

            lblKhoa.Location = new Point(col2_L, row1);
            cbKhoa.Location = new Point(col2_I, row1 - 5); cbKhoa.Size = new Size(250, inputH);

            lblTenLop.Location = new Point(col1_L, row2);
            txtTenLop.Location = new Point(col1_I, row2 - 5); txtTenLop.Size = new Size(650, inputH);

            // Vị trí nút nằm dưới GroupBox
            int btnY = gbThongTin.Height + 15;
            int btnStartX = (this.Width - (4 * 110 + 3 * 20)) / 2;

            btnThem.Location = new Point(btnStartX, btnY);
            btnSua.Location = new Point(btnThem.Right + 20, btnY);
            btnXoa.Location = new Point(btnSua.Right + 20, btnY);
            btnLamMoi.Location = new Point(btnXoa.Right + 20, btnY);

            // [FIX] Tắt Dock Fill của bảng để không đè lên nút
            dgvLopHoc.Dock = DockStyle.None;
            dgvLopHoc.Location = new Point(0, btnY + 50);
            dgvLopHoc.Size = new Size(this.Width, this.Height - dgvLopHoc.Top);
            dgvLopHoc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }

        // --- 2. DỮ LIỆU (Dùng BindingSource chống lỗi Index -1) ---
        private void InitTableColumns()
        {
            dgvLopHoc.AutoGenerateColumns = false;
            dgvLopHoc.Columns.Clear();
            AddTextColumn("MaLop", "Mã Lớp", "MaLop", 150);
            AddTextColumn("TenLop", "Tên Lớp", "TenLop", 250);
            AddTextColumn("Khoa", "Khoa", "Khoa", 200);
        }

        private void AddTextColumn(string name, string header, string prop, int width)
        {
            var col = new DataGridViewTextBoxColumn { Name = name, HeaderText = header, DataPropertyName = prop, Width = width };
            dgvLopHoc.Columns.Add(col);
        }

        private void LoadData()
        {
            danhSachLop = DataHelper.DocLop();
            // Dùng BindingSource làm trung gian an toàn
            var bindingSource = new BindingSource();
            bindingSource.DataSource = danhSachLop;
            dgvLopHoc.DataSource = bindingSource;
        }

        private void LoadComboBoxKhoa()
        {
            try
            {
                var dsKhoa = DataHelper.DocKhoa();
                cbKhoa.DataSource = dsKhoa;
                cbKhoa.DisplayMember = "TenKhoa";
                cbKhoa.ValueMember = "TenKhoa";
                cbKhoa.SelectedIndex = -1;
            }
            catch { }
        }

        // --- 3. SỰ KIỆN & NGHIỆP VỤ ---
        private void RegisterEvents()
        {
            btnThem.Click -= BtnThem_Click; btnThem.Click += BtnThem_Click;
            btnSua.Click -= BtnSua_Click; btnSua.Click += BtnSua_Click;
            btnXoa.Click -= BtnXoa_Click; btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click -= LamMoi_Click; btnLamMoi.Click += LamMoi_Click;
            dgvLopHoc.CellClick -= DgvLopHoc_CellClick; dgvLopHoc.CellClick += DgvLopHoc_CellClick;
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            bool isValid = true;

            // Bắt lỗi nhập liệu
            if (string.IsNullOrWhiteSpace(txtMaLop.Text)) { errorProvider.SetError(txtMaLop, "Nhập mã lớp!"); isValid = false; }
            if (string.IsNullOrWhiteSpace(txtTenLop.Text)) { errorProvider.SetError(txtTenLop, "Nhập tên lớp!"); isValid = false; }
            if (cbKhoa.SelectedIndex < 0 && string.IsNullOrWhiteSpace(cbKhoa.Text)) { errorProvider.SetError(cbKhoa, "Chọn khoa!"); isValid = false; }

            if (!isValid) return;

            // Kiểm tra trùng
            if (danhSachLop.Any(x => x.MaLop.ToLower() == txtMaLop.Text.Trim().ToLower()))
            {
                MessageBox.Show("Mã lớp đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Thêm mới
            danhSachLop.Add(new LopHocModel
            {
                MaLop = txtMaLop.Text.Trim(),
                TenLop = txtTenLop.Text.Trim(),
                Khoa = cbKhoa.Text
            });

            DataHelper.LuuLop(danhSachLop);
            LoadData();
            LamMoiForm();
            MessageBox.Show("Thêm thành công!");
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (txtMaLop.Enabled) { MessageBox.Show("Chọn lớp cần sửa!"); return; }

            var item = danhSachLop.FirstOrDefault(x => x.MaLop == txtMaLop.Text);
            if (item != null)
            {
                item.TenLop = txtTenLop.Text;
                item.Khoa = cbKhoa.Text;
                DataHelper.LuuLop(danhSachLop);
                LoadData();
                MessageBox.Show("Cập nhật xong!");
                LamMoiForm();
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            var item = danhSachLop.FirstOrDefault(x => x.MaLop == txtMaLop.Text);
            if (item == null) return;

            // Ràng buộc dữ liệu: Không xóa lớp nếu có sinh viên
            if (DataHelper.DocSV().Any(sv => sv.Lop == item.TenLop))
            {
                MessageBox.Show($"Lớp '{item.TenLop}' đang có sinh viên.\nKhông thể xóa!", "Cấm xóa", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show("Xóa lớp này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                danhSachLop.Remove(item);
                DataHelper.LuuLop(danhSachLop);
                LoadData();
                LamMoiForm();
            }
        }

        private void DgvLopHoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvLopHoc.Rows[e.RowIndex].DataBoundItem as LopHocModel;
                if (row == null) return;
                txtMaLop.Text = row.MaLop;
                txtTenLop.Text = row.TenLop;
                cbKhoa.Text = row.Khoa;

                txtMaLop.Enabled = false; // Khóa mã
                errorProvider.Clear();
            }
        }

        private void LamMoi_Click(object sender, EventArgs e) => LamMoiForm();

        private void LamMoiForm()
        {
            txtMaLop.Clear(); txtTenLop.Clear(); cbKhoa.SelectedIndex = -1;
            txtMaLop.Enabled = true; txtMaLop.Focus(); errorProvider.Clear();
        }
    }
}