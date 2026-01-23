using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace doan
{
    public partial class UC_QuanLyTaiKhoan : UserControl
    {
        private List<TaiKhoan> listTK = new List<TaiKhoan>();
        private GroupBox gbInfo;
        private Label lblUser, lblPass, lblHoTen, lblQuyen;
        private TextBox txtUser, txtPass, txtHoTen;
        private ComboBox cbQuyen;
        private Button btnThem, btnSua, btnXoa, btnLamMoi;
        private DataGridView dgvTaiKhoan;

        public UC_QuanLyTaiKhoan()
        {
            InitializeComponent();
            SetupUI();
            InitTable();
            LoadData();
            RegisterEvents();
        }

        private void SetupUI()
        {
            UIHelper.Beautify(this);

            // Khởi tạo GroupBox
            gbInfo = new GroupBox { Text = "THÔNG TIN TÀI KHOẢN", Dock = DockStyle.Top, Height = 180, BackColor = Color.White };

            // Khởi tạo Labels
            lblUser = new Label { Text = "Tên đăng nhập:", Location = new Point(30, 40), AutoSize = true };
            lblPass = new Label { Text = "Mật khẩu:", Location = new Point(30, 85), AutoSize = true };
            lblHoTen = new Label { Text = "Họ tên hiển thị:", Location = new Point(400, 40), AutoSize = true };
            lblQuyen = new Label { Text = "Quyền hạn:", Location = new Point(400, 85), AutoSize = true };

            // Khởi tạo Inputs
            txtUser = new TextBox { Location = new Point(150, 35), Width = 200 };
            txtPass = new TextBox { Location = new Point(150, 80), Width = 200, UseSystemPasswordChar = true };
            txtHoTen = new TextBox { Location = new Point(520, 35), Width = 250 };
            cbQuyen = new ComboBox { Location = new Point(520, 80), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cbQuyen.Items.AddRange(new string[] { "Admin", "SinhVien" });

            // Khởi tạo Nút bấm
            btnThem = new Button { Text = "Thêm mới", Size = new Size(110, 40), Location = new Point(150, 125) };
            btnSua = new Button { Text = "Cập nhật", Size = new Size(110, 40), Location = new Point(280, 125) };
            btnXoa = new Button { Text = "Xóa TK", Size = new Size(110, 40), Location = new Point(410, 125) };
            btnLamMoi = new Button { Text = "Làm mới", Size = new Size(110, 40), Location = new Point(540, 125) };

            UIHelper.StyleButton(btnThem, UIHelper.SuccessColor);
            UIHelper.StyleButton(btnSua, UIHelper.WarningColor);
            UIHelper.StyleButton(btnXoa, UIHelper.DangerColor);
            UIHelper.StyleButton(btnLamMoi, UIHelper.InfoColor);

            gbInfo.Controls.AddRange(new Control[] { lblUser, lblPass, lblHoTen, lblQuyen, txtUser, txtPass, txtHoTen, cbQuyen, btnThem, btnSua, btnXoa, btnLamMoi });

            // Khởi tạo DataGridView
            dgvTaiKhoan = new DataGridView { Dock = DockStyle.Fill, BackgroundColor = Color.White, SelectionMode = DataGridViewSelectionMode.FullRowSelect, ReadOnly = true, AllowUserToAddRows = false };

            this.Controls.Add(dgvTaiKhoan);
            this.Controls.Add(gbInfo);
            dgvTaiKhoan.BringToFront();
        }

        private void InitTable()
        {
            dgvTaiKhoan.AutoGenerateColumns = false;
            dgvTaiKhoan.Columns.Clear();
            dgvTaiKhoan.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên đăng nhập", DataPropertyName = "TenDangNhap", Width = 150 });
            dgvTaiKhoan.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Họ tên", DataPropertyName = "HoTen", Width = 200 });
            dgvTaiKhoan.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mật khẩu", DataPropertyName = "MatKhau", Width = 150 });
            dgvTaiKhoan.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Quyền hạn", DataPropertyName = "Quyen", Width = 120 });
        }

        private void RegisterEvents()
        {
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += (s, e) => LamMoi();
            dgvTaiKhoan.CellClick += DgvTaiKhoan_CellClick;
        }

        private void LoadData()
        {
            listTK = DataHelper.DocTatCaTaiKhoan();
            dgvTaiKhoan.DataSource = new BindingSource { DataSource = listTK };
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text)) return;
            string sql = "INSERT INTO TaiKhoan (TenDangNhap, MatKhau, HoTen, Quyen) VALUES (@u, @p, @h, @q)";
            SqlParameter[] p = {
                new SqlParameter("@u", txtUser.Text.Trim()),
                new SqlParameter("@p", txtPass.Text),
                new SqlParameter("@h", txtHoTen.Text.Trim()),
                new SqlParameter("@q", cbQuyen.Text)
            };
            if (DataHelper.ThựcThi(sql, p)) { MessageBox.Show("Thêm thành công!"); LoadData(); LamMoi(); }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (txtUser.Enabled) return; // Chỉ cho sửa khi đã chọn từ bảng
            string sql = "UPDATE TaiKhoan SET MatKhau=@p, HoTen=@h, Quyen=@q WHERE TenDangNhap=@u";
            SqlParameter[] p = {
                new SqlParameter("@p", txtPass.Text),
                new SqlParameter("@h", txtHoTen.Text.Trim()),
                new SqlParameter("@q", cbQuyen.Text),
                new SqlParameter("@u", txtUser.Text.Trim())
            };
            if (DataHelper.ThựcThi(sql, p)) { MessageBox.Show("Cập nhật thành công!"); LoadData(); LamMoi(); }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra xem đã chọn tài khoản nào chưa
            if (txtUser.Enabled) return;

            string userXoa = txtUser.Text.Trim();

            // 2. CHẶN TỰ XÓA CHÍNH MÌNH (QUAN TRỌNG)
            if (userXoa.ToLower() == UserSession.TenDangNhap.ToLower())
            {
                MessageBox.Show("Bạn không thể tự xóa tài khoản đang đăng nhập!",
                                "Cảnh báo bảo mật", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // 3. Chặn xóa tài khoản Admin gốc (nếu cần)
            if (userXoa.ToLower() == "admin")
            {
                MessageBox.Show("Không thể xóa tài khoản Quản trị viên hệ thống!",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 4. Thực hiện xóa sau khi xác nhận
            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa tài khoản [{userXoa}]?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "DELETE FROM TaiKhoan WHERE TenDangNhap = @u";
                SqlParameter[] p = { new SqlParameter("@u", userXoa) };

                if (DataHelper.ThựcThi(sql, p))
                {
                    MessageBox.Show("Đã xóa tài khoản thành công!");
                    LoadData(); // Tải lại bảng
                    LamMoi();   // Xóa trắng form
                }
            }
        }

        private void DgvTaiKhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var tk = dgvTaiKhoan.Rows[e.RowIndex].DataBoundItem as TaiKhoan;
            if (tk != null)
            {
                txtUser.Text = tk.TenDangNhap;
                txtPass.Text = tk.MatKhau;
                txtHoTen.Text = tk.HoTen;
                cbQuyen.Text = tk.Quyen;
                txtUser.Enabled = false; // Không cho sửa Username
            }
        }

        private void LamMoi()
        {
            txtUser.Clear(); txtPass.Clear(); txtHoTen.Clear();
            cbQuyen.SelectedIndex = -1;
            txtUser.Enabled = true;
        }
    }
}