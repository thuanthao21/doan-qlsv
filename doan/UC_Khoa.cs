using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace doan
{
    public partial class UC_Khoa : UserControl
    {
        private GroupBox gbThongTin;
        private Label lblMaKhoa, lblTenKhoa;
        private TextBox txtMaKhoa, txtTenKhoa;
        private Button btnThem, btnSua, btnXoa, btnLamMoi;
        private DataGridView dgvKhoa;

        private ErrorProvider errorProvider = new ErrorProvider();
        private List<KhoaModel> dsKhoa = new List<KhoaModel>();

        public UC_Khoa()
        {
            InitializeComponent();
            InitCustomControls();
            ApplyModernLayout();
            InitTableColumns();

            this.Load += (s, e) => LoadData();
            RegisterEvents(); // Đăng ký sự kiện nút bấm

            this.Resize += (s, e) => UpdateLayoutPositions();
        }

        // --- 1. SỰ KIỆN (ĐÃ SỬA LẠI CHO CHUẨN) ---
        private void RegisterEvents()
        {
            // Hủy đăng ký cũ trước khi thêm mới để tránh bị click đúp
            btnThem.Click -= BtnThem_Click; btnThem.Click += BtnThem_Click;
            btnSua.Click -= BtnSua_Click; btnSua.Click += BtnSua_Click;
            btnXoa.Click -= BtnXoa_Click; btnXoa.Click += BtnXoa_Click;

            // [ĐÃ SỬA] Dùng hàm riêng thay vì viết gộp, đảm bảo nút hoạt động
            btnLamMoi.Click -= BtnLamMoi_Click; btnLamMoi.Click += BtnLamMoi_Click;

            dgvKhoa.CellClick -= DgvKhoa_CellClick; dgvKhoa.CellClick += DgvKhoa_CellClick;
        }

        // Hàm xử lý nút Làm mới riêng biệt
        private void BtnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoiForm();
        }

        // --- 2. LOGIC NGHIỆP VỤ ---
        private void BtnThem_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtMaKhoa.Text)) { errorProvider.SetError(txtMaKhoa, "Nhập mã!"); isValid = false; }
            if (string.IsNullOrWhiteSpace(txtTenKhoa.Text)) { errorProvider.SetError(txtTenKhoa, "Nhập tên!"); isValid = false; }

            if (!isValid) return;

            if (dsKhoa.Any(k => k.MaKhoa.ToLower() == txtMaKhoa.Text.Trim().ToLower()))
            {
                MessageBox.Show("Trùng mã khoa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dsKhoa.Add(new KhoaModel { MaKhoa = txtMaKhoa.Text.Trim(), TenKhoa = txtTenKhoa.Text.Trim() });
            DataHelper.LuuKhoa(dsKhoa);
            LoadData();
            LamMoiForm();
            MessageBox.Show("Thêm thành công!");
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (txtMaKhoa.Enabled)
            {
                MessageBox.Show("Vui lòng chọn Khoa cần sửa trên bảng!", "Thông báo");
                return;
            }

            var item = dsKhoa.FirstOrDefault(k => k.MaKhoa == txtMaKhoa.Text);
            if (item != null)
            {
                item.TenKhoa = txtTenKhoa.Text;
                DataHelper.LuuKhoa(dsKhoa);
                LoadData();
                MessageBox.Show("Đã sửa thành công!");
                LamMoiForm();
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            var item = dsKhoa.FirstOrDefault(k => k.MaKhoa == txtMaKhoa.Text);
            if (item == null) return;

            var dsLop = DataHelper.DocLop();
            if (dsLop.Any(l => l.Khoa == item.TenKhoa))
            {
                MessageBox.Show($"Khoa này đang có lớp học, không thể xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show("Xóa khoa này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                dsKhoa.Remove(item);
                DataHelper.LuuKhoa(dsKhoa);
                LoadData();
                LamMoiForm();
            }
        }

        private void DgvKhoa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvKhoa.Rows[e.RowIndex].DataBoundItem as KhoaModel;
                if (row == null) return;
                txtMaKhoa.Text = row.MaKhoa;
                txtTenKhoa.Text = row.TenKhoa;

                txtMaKhoa.Enabled = false; // Khóa mã khi đang chọn sửa
                errorProvider.Clear();
            }
        }

        private void LamMoiForm()
        {
            txtMaKhoa.Clear();
            txtTenKhoa.Clear();
            txtMaKhoa.Enabled = true; // Mở lại ô nhập mã
            txtMaKhoa.Focus();
            errorProvider.Clear();
        }

        private void LoadData()
        {
            dsKhoa = DataHelper.DocKhoa();
            var bindingSource = new BindingSource();
            bindingSource.DataSource = dsKhoa;
            dgvKhoa.DataSource = bindingSource;
        }

        // --- 3. GIAO DIỆN & LAYOUT ---
        private void UpdateLayoutPositions()
        {
            if (gbThongTin == null || btnThem == null) return;
            int btnY = gbThongTin.Height + 20;
            int contentWidth = 4 * 110 + 3 * 20;
            int btnCenterX = (this.Width - contentWidth) / 2;
            if (btnCenterX < 10) btnCenterX = 10;

            btnThem.Location = new Point(btnCenterX, btnY);
            btnSua.Location = new Point(btnThem.Right + 20, btnY);
            btnXoa.Location = new Point(btnSua.Right + 20, btnY);
            btnLamMoi.Location = new Point(btnXoa.Right + 20, btnY);

            if (dgvKhoa != null)
            {
                dgvKhoa.Location = new Point(0, btnY + 60);
                dgvKhoa.Size = new Size(this.Width, this.Height - dgvKhoa.Top);
            }
        }

        private void InitCustomControls()
        {
            if (gbThongTin == null) gbThongTin = new GroupBox();
            if (lblMaKhoa == null) lblMaKhoa = new Label { Text = "Mã Khoa:" };
            if (lblTenKhoa == null) lblTenKhoa = new Label { Text = "Tên Khoa:" };
            if (txtMaKhoa == null) txtMaKhoa = new TextBox();
            if (txtTenKhoa == null) txtTenKhoa = new TextBox();
            if (btnThem == null) btnThem = new Button { Text = "Thêm" };
            if (btnSua == null) btnSua = new Button { Text = "Sửa" };
            if (btnXoa == null) btnXoa = new Button { Text = "Xóa" };
            if (btnLamMoi == null) btnLamMoi = new Button { Text = "Làm mới" };
            if (dgvKhoa == null) dgvKhoa = new DataGridView();

            if (!this.Controls.Contains(gbThongTin)) this.Controls.Add(gbThongTin);
            if (!gbThongTin.Controls.Contains(txtMaKhoa)) gbThongTin.Controls.AddRange(new Control[] { lblMaKhoa, lblTenKhoa, txtMaKhoa, txtTenKhoa });
            if (!this.Controls.Contains(dgvKhoa)) this.Controls.Add(dgvKhoa);
        }

        private void ApplyModernLayout()
        {
            UIHelper.Beautify(this);

            if (gbThongTin.Controls.Contains(btnThem)) gbThongTin.Controls.Remove(btnThem);
            if (gbThongTin.Controls.Contains(btnSua)) gbThongTin.Controls.Remove(btnSua);
            if (gbThongTin.Controls.Contains(btnXoa)) gbThongTin.Controls.Remove(btnXoa);
            if (gbThongTin.Controls.Contains(btnLamMoi)) gbThongTin.Controls.Remove(btnLamMoi);

            this.Controls.Add(btnThem);
            this.Controls.Add(btnSua);
            this.Controls.Add(btnXoa);
            this.Controls.Add(btnLamMoi);

            btnThem.BringToFront(); btnSua.BringToFront(); btnXoa.BringToFront(); btnLamMoi.BringToFront();

            UIHelper.StyleButton(btnThem, UIHelper.SuccessColor);
            UIHelper.StyleButton(btnSua, UIHelper.WarningColor);
            UIHelper.StyleButton(btnXoa, UIHelper.DangerColor);
            UIHelper.StyleButton(btnLamMoi, UIHelper.InfoColor);

            gbThongTin.Text = "QUẢN LÝ KHOA";
            gbThongTin.Dock = DockStyle.Top;
            gbThongTin.Height = 180;
            gbThongTin.BackColor = UIHelper.White;

            int startX = 250, startY = 50, gap = 50;
            lblMaKhoa.Location = new Point(startX, startY);
            txtMaKhoa.Location = new Point(startX + 100, startY - 5);
            txtMaKhoa.Size = new Size(300, 32);

            lblTenKhoa.Location = new Point(startX, startY + gap);
            txtTenKhoa.Location = new Point(startX + 100, startY + gap - 5);
            txtTenKhoa.Size = new Size(300, 32);

            dgvKhoa.Dock = DockStyle.None;
            dgvKhoa.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            UpdateLayoutPositions();
        }

        private void InitTableColumns()
        {
            dgvKhoa.AutoGenerateColumns = false;
            dgvKhoa.Columns.Clear();
            AddTextColumn("MaKhoa", "Mã Khoa", "MaKhoa");
            AddTextColumn("TenKhoa", "Tên Khoa", "TenKhoa");
        }

        private void AddTextColumn(string name, string header, string prop)
        {
            var col = new DataGridViewTextBoxColumn { Name = name, HeaderText = header, DataPropertyName = prop };
            dgvKhoa.Columns.Add(col);
        }
    }
}