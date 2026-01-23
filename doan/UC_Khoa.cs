using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient; // Đảm bảo đã thêm thư viện này

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
            RegisterEvents();

            this.Resize += (s, e) => UpdateLayoutPositions();
        }

        // --- 1. SỰ KIỆN ---
        private void RegisterEvents()
        {
            btnThem.Click -= BtnThem_Click; btnThem.Click += BtnThem_Click;
            btnSua.Click -= BtnSua_Click; btnSua.Click += BtnSua_Click;
            btnXoa.Click -= BtnXoa_Click; btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click -= BtnLamMoi_Click; btnLamMoi.Click += BtnLamMoi_Click;
            dgvKhoa.CellClick -= DgvKhoa_CellClick; dgvKhoa.CellClick += DgvKhoa_CellClick;
        }

        private void BtnLamMoi_Click(object sender, EventArgs e) => LamMoiForm();

        // --- 2. LOGIC NGHIỆP VỤ (ADO.NET) ---
        private void BtnThem_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            if (string.IsNullOrWhiteSpace(txtMaKhoa.Text)) { errorProvider.SetError(txtMaKhoa, "Nhập mã!"); return; }
            if (string.IsNullOrWhiteSpace(txtTenKhoa.Text)) { errorProvider.SetError(txtTenKhoa, "Nhập tên!"); return; }

            // Kiểm tra trùng mã trực tiếp trong danh sách hiện tại (đã load từ DB)
            if (dsKhoa.Any(k => k.MaKhoa.ToLower() == txtMaKhoa.Text.Trim().ToLower()))
            {
                MessageBox.Show("Mã khoa đã tồn tại trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Thực thi INSERT vào SQL
            string sql = "INSERT INTO Khoa (MaKhoa, TenKhoa) VALUES (@ma, @ten)";
            SqlParameter[] paras = {
                new SqlParameter("@ma", txtMaKhoa.Text.Trim()),
                new SqlParameter("@ten", txtTenKhoa.Text.Trim())
            };

            if (DataHelper.ThựcThi(sql, paras))
            {
                MessageBox.Show("Thêm khoa mới thành công!");
                LoadData();
                LamMoiForm();
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (txtMaKhoa.Enabled)
            {
                MessageBox.Show("Vui lòng chọn Khoa cần sửa trên bảng!", "Thông báo");
                return;
            }

            // Thực thi UPDATE vào SQL
            string sql = "UPDATE Khoa SET TenKhoa = @ten WHERE MaKhoa = @ma";
            SqlParameter[] paras = {
                new SqlParameter("@ten", txtTenKhoa.Text.Trim()),
                new SqlParameter("@ma", txtMaKhoa.Text.Trim())
            };

            if (DataHelper.ThựcThi(sql, paras))
            {
                MessageBox.Show("Cập nhật thông tin khoa thành công!");
                LoadData();
                LamMoiForm();
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaKhoa.Enabled) return;

            // KIỂM TRA RÀNG BUỘC: Nếu khoa có lớp thì không cho xóa
            var dsLop = DataHelper.DocLop();
            if (dsLop.Any(l => l.MaKhoa == txtMaKhoa.Text)) // Dùng MaKhoa để check chuẩn hơn
            {
                MessageBox.Show("Khoa này đang có lớp học trực thuộc, không thể xóa!", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa khoa này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "DELETE FROM Khoa WHERE MaKhoa = @ma";
                SqlParameter[] paras = { new SqlParameter("@ma", txtMaKhoa.Text.Trim()) };

                if (DataHelper.ThựcThi(sql, paras))
                {
                    MessageBox.Show("Đã xóa khoa thành công!");
                    LoadData();
                    LamMoiForm();
                }
            }
        }

        private void LoadData()
        {
            // Gọi hàm DocKhoa từ DataHelper (đã chuyển sang ADO.NET)
            dsKhoa = DataHelper.DocKhoa();
            var bindingSource = new BindingSource();
            bindingSource.DataSource = dsKhoa;
            dgvKhoa.DataSource = bindingSource;
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
            txtMaKhoa.Enabled = true;
            txtMaKhoa.Focus();
            errorProvider.Clear();
        }

        // --- 3. GIAO DIỆN & LAYOUT (GIỮ NGUYÊN) ---
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

            Control[] buttons = { btnThem, btnSua, btnXoa, btnLamMoi };
            foreach (var btn in buttons)
            {
                if (gbThongTin.Controls.Contains(btn)) gbThongTin.Controls.Remove(btn);
                this.Controls.Add(btn);
                btn.BringToFront();
            }

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

        private void InitTableColumns()
        {
            dgvKhoa.AutoGenerateColumns = false;
            dgvKhoa.Columns.Clear();
            AddTextColumn("MaKhoa", "Mã Khoa", "MaKhoa");
            AddTextColumn("TenKhoa", "Tên Khoa", "TenKhoa");
        }

        private void AddTextColumn(string name, string header, string prop)
        {
            var col = new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                DataPropertyName = prop,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            dgvKhoa.Columns.Add(col);
        }
    }
}