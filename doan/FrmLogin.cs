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
        private Button btnLogin, btnExit;

        public FrmLogin()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(400, 420); // Thu gọn chiều cao vì đã bỏ nút đăng ký
            this.BackColor = UIHelper.PrimaryColor;

            InitCustomControls();
            ApplyStyles();
        }

        private void InitCustomControls()
        {
            pnlContent = new Panel();
            lblTitle = new Label { Text = "HỆ THỐNG ĐÀO TẠO" };
            lblUser = new Label { Text = "Tên đăng nhập:" };
            lblPass = new Label { Text = "Mật khẩu:" };
            txtUser = new TextBox();
            txtPass = new TextBox { UseSystemPasswordChar = true };
            btnLogin = new Button { Text = "ĐĂNG NHẬP" };
            btnExit = new Button { Text = "X" };

            this.Controls.Add(pnlContent);
            pnlContent.Controls.AddRange(new Control[] { lblTitle, lblUser, lblPass, txtUser, txtPass, btnLogin, btnExit });
        }

        private void ApplyStyles()
        {
            pnlContent.Size = new Size(360, 380);
            pnlContent.Location = new Point(20, 20);
            pnlContent.BackColor = UIHelper.White;

            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.ForeColor = UIHelper.PrimaryColor;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(65, 35); // Căn chỉnh lại tiêu đề

            lblUser.Location = new Point(30, 100);
            txtUser.Location = new Point(30, 125);
            txtUser.Size = new Size(300, 30);
            txtUser.Font = new Font("Segoe UI", 11);
            txtUser.BackColor = UIHelper.LightGray;

            lblPass.Location = new Point(30, 175);
            txtPass.Location = new Point(30, 200);
            txtPass.Size = new Size(300, 30);
            txtPass.Font = new Font("Segoe UI", 11);
            txtPass.BackColor = UIHelper.LightGray;

            UIHelper.StyleButton(btnLogin, UIHelper.PrimaryColor);
            btnLogin.Width = 300;
            btnLogin.Location = new Point(30, 260);
            btnLogin.Height = 45;

            btnExit.Size = new Size(35, 35);
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.BackColor = UIHelper.White;
            btnExit.ForeColor = Color.Gray;
            btnExit.Location = new Point(325, 0);

            // --- SỰ KIỆN ---
            btnLogin.Click += BtnLogin_Click;
            btnExit.Click += (s, e) => Application.Exit();

            // Nhấn Enter để đăng nhập nhanh
            txtPass.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) BtnLogin_Click(null, null); };
            txtUser.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) txtPass.Focus(); };
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string u = txtUser.Text.Trim();
            string p = txtPass.Text.Trim();

            if (string.IsNullOrEmpty(u) || string.IsNullOrEmpty(p))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            // Gọi hàm từ DataHelper kết nối SQL Server
            var user = DataHelper.KiemTraDangNhap(u, p);

            if (user != null)
            {
                // Lưu thông tin vào Session
                UserSession.TenDangNhap = user.TenDangNhap;
                UserSession.QuyenHan = user.Quyen;
                UserSession.TenHienThi = user.HoTen;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}