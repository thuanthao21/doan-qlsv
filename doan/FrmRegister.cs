using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace doan
{
    public partial class FrmRegister : Form
    {
        private TextBox txtUser, txtPass, txtConfirmPass, txtHoTen;
        private Button btnRegister, btnBack;
        private Panel pnlContent;

        public FrmRegister()
        {
            InitializeComponent();
            SetupUI(); // Vẽ giao diện
        }

        private void SetupUI()
        {
            // Cấu hình Form
            this.Text = "Đăng Ký Tài Khoản";
            this.Size = new Size(400, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = UIHelper.PrimaryColor; // Nền xanh

            // Panel trắng chứa nội dung
            pnlContent = new Panel { Size = new Size(360, 480), Location = new Point(20, 20), BackColor = Color.White };
            this.Controls.Add(pnlContent);

            // Tiêu đề
            Label lblTitle = new Label
            {
                Text = "ĐĂNG KÝ",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = UIHelper.PrimaryColor,
                AutoSize = true,
                Location = new Point(115, 20)
            };
            pnlContent.Controls.Add(lblTitle);

            // Các ô nhập liệu
            pnlContent.Controls.Add(CreateLabel("Họ và Tên:", 70));
            pnlContent.Controls.Add(txtHoTen = CreateTextBox(95));

            pnlContent.Controls.Add(CreateLabel("Tên đăng nhập:", 145));
            pnlContent.Controls.Add(txtUser = CreateTextBox(170));

            pnlContent.Controls.Add(CreateLabel("Mật khẩu:", 220));
            pnlContent.Controls.Add(txtPass = CreateTextBox(245, true));

            pnlContent.Controls.Add(CreateLabel("Nhập lại mật khẩu:", 295));
            pnlContent.Controls.Add(txtConfirmPass = CreateTextBox(320, true));

            // Nút Đăng ký
            btnRegister = new Button
            {
                Text = "ĐĂNG KÝ NGAY",
                BackColor = UIHelper.SuccessColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(300, 45),
                Location = new Point(30, 380),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRegister.FlatAppearance.BorderSize = 0;

            // Nút Quay lại
            btnBack = new Button
            {
                Text = "Quay lại Đăng nhập",
                FlatStyle = FlatStyle.Flat,
                ForeColor = UIHelper.PrimaryColor,
                AutoSize = true,
                Location = new Point(120, 435),
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Underline),
                Cursor = Cursors.Hand
            };
            btnBack.FlatAppearance.BorderSize = 0;

            pnlContent.Controls.Add(btnRegister);
            pnlContent.Controls.Add(btnBack);

            // Sự kiện
            btnRegister.Click += BtnRegister_Click;
            btnBack.Click += (s, e) => this.Close();
        }

        // Hàm hỗ trợ tạo Label nhanh
        private Label CreateLabel(string text, int y) => new Label { Text = text, Location = new Point(30, y), AutoSize = true, ForeColor = Color.Gray, Font = new Font("Segoe UI", 10) };

        // Hàm hỗ trợ tạo TextBox nhanh
        private TextBox CreateTextBox(int y, bool isPass = false) => new TextBox { Location = new Point(30, y), Size = new Size(300, 30), Font = new Font("Segoe UI", 11), UseSystemPasswordChar = isPass, BackColor = UIHelper.LightGray };

        // --- XỬ LÝ LƯU FILE JSON ---
        private void BtnRegister_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra nhập liệu
            if (string.IsNullOrWhiteSpace(txtUser.Text) || string.IsNullOrWhiteSpace(txtPass.Text) || string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thiếu thông tin"); return;
            }

            if (txtPass.Text != txtConfirmPass.Text)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp!", "Lỗi mật khẩu"); return;
            }

            // 2. Đọc file cũ lên để kiểm tra trùng
            var list = DataHelper.DocTaiKhoan();

            if (list.Any(x => x.TenDangNhap.ToLower() == txtUser.Text.Trim().ToLower()))
            {
                MessageBox.Show("Tên đăng nhập này đã có người dùng!", "Trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. Thêm tài khoản mới (Mặc định là SinhVien)
            list.Add(new TaiKhoan
            {
                TenDangNhap = txtUser.Text.Trim(),
                MatKhau = txtPass.Text,
                HoTen = txtHoTen.Text.Trim(),
                Quyen = "SinhVien"
            });

            // 4. Lưu lại xuống file JSON
            DataHelper.LuuTaiKhoan(list);

            MessageBox.Show("Đăng ký thành công! Hãy đăng nhập ngay.", "Chúc mừng");
            this.Close(); // Đóng form đăng ký để quay về đăng nhập
        }
    }
}