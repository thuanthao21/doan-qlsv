using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace doan
{
    public partial class FrmLogin : Form
    {
        private Panel pnlContent;
        private Label lblTitle, lblUser, lblPass;
        private TextBox txtUser, txtPass;
        private Button btnLogin, btnExit, btnRegister; // Thêm nút Đăng ký

        public FrmLogin()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(400, 450); // Tăng chiều cao một chút
            this.BackColor = UIHelper.PrimaryColor;

            InitCustomControls();
            ApplyStyles();
        }

        private void InitCustomControls()
        {
            pnlContent = new Panel();
            lblTitle = new Label { Text = "ĐĂNG NHẬP" };
            lblUser = new Label { Text = "Tài khoản:" };
            lblPass = new Label { Text = "Mật khẩu:" };
            txtUser = new TextBox();
            txtPass = new TextBox { UseSystemPasswordChar = true };
            btnLogin = new Button { Text = "ĐĂNG NHẬP" };
            btnRegister = new Button { Text = "Chưa có tài khoản? Đăng ký ngay" }; // Nút Đăng ký
            btnExit = new Button { Text = "X" };

            this.Controls.Add(pnlContent);
            pnlContent.Controls.AddRange(new Control[] { lblTitle, lblUser, lblPass, txtUser, txtPass, btnLogin, btnRegister, btnExit });
        }

        private void ApplyStyles()
        {
            pnlContent.Size = new Size(360, 410);
            pnlContent.Location = new Point(20, 20);
            pnlContent.BackColor = UIHelper.White;

            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.ForeColor = UIHelper.PrimaryColor;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(100, 30);

            lblUser.Location = new Point(30, 90);
            txtUser.Location = new Point(30, 115);
            txtUser.Size = new Size(300, 30);
            txtUser.Font = new Font("Segoe UI", 11);
            txtUser.BackColor = UIHelper.LightGray;

            lblPass.Location = new Point(30, 160);
            txtPass.Location = new Point(30, 185);
            txtPass.Size = new Size(300, 30);
            txtPass.Font = new Font("Segoe UI", 11);
            txtPass.BackColor = UIHelper.LightGray;

            UIHelper.StyleButton(btnLogin, UIHelper.PrimaryColor);
            btnLogin.Width = 300;
            btnLogin.Location = new Point(30, 240);
            btnLogin.Height = 45;

            // Style nút Đăng ký (nhìn như đường link)
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.BackColor = Color.White;
            btnRegister.ForeColor = UIHelper.PrimaryColor;
            btnRegister.Font = new Font("Segoe UI", 9, FontStyle.Underline);
            btnRegister.AutoSize = true;
            btnRegister.Location = new Point(80, 300);
            btnRegister.Cursor = Cursors.Hand;

            btnExit.Size = new Size(40, 40);
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.BackColor = UIHelper.White;
            btnExit.ForeColor = Color.Gray;
            btnExit.Location = new Point(320, 0);

            // --- SỰ KIỆN ---
            btnLogin.Click += BtnLogin_Click;
            btnRegister.Click += BtnRegister_Click; // Sự kiện mở form đăng ký
            btnExit.Click += (s, e) => Application.Exit();
            txtPass.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) BtnLogin_Click(null, null); };
        }

        // --- XỬ LÝ ĐĂNG NHẬP THỰC TẾ ---
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string u = txtUser.Text.Trim();
            string p = txtPass.Text.Trim();

            // 1. Đọc dữ liệu từ file JSON thông qua DataHelper
            var listTaiKhoan = DataHelper.DocTaiKhoan();

            // 2. Tìm kiếm tài khoản trùng khớp
            var user = listTaiKhoan.FirstOrDefault(x => x.TenDangNhap == u && x.MatKhau == p);

            if (user != null)
            {
                // Lưu thông tin người dùng vào Session để dùng ở Form chính
                UserSession.QuyenHan = user.Quyen;
                UserSession.TenHienThi = user.HoTen;

                this.DialogResult = DialogResult.OK; // Báo OK để mở Form1
                this.Close();
            }
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPass.Clear();
                txtPass.Focus();
            }
        }

        // Mở Form Đăng Ký
        private void BtnRegister_Click(object sender, EventArgs e)
        {
            FrmRegister frm = new FrmRegister();
            this.Hide(); // Ẩn form đăng nhập tạm thời
            frm.ShowDialog(); // Hiện form đăng ký và chờ
            this.Show(); // Hiện lại form đăng nhập sau khi đăng ký xong
        }
    }
}