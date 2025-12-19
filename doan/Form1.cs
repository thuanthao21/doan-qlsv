using System;
using System.Drawing;
using System.Windows.Forms;

namespace doan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetupDashboard();
            LoadUserControl(new UC_SinhVien(), btnSinhVien);
        }

        private void SetupDashboard()
        {
            // 1. Cấu hình Form
            this.Text = "HỆ THỐNG QUẢN LÝ ĐÀO TẠO";
            this.Size = new Size(1200, 768); // Kích thước rộng rãi hơn
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = UIHelper.LightGray;

            // 2. Sidebar (Menu trái)
            PanelMenu.BackColor = UIHelper.DarkBlue;
            PanelMenu.Width = 220; // Rộng hơn chút

            // 3. Header (Thanh trên cùng)
            PanelHeader.BackColor = UIHelper.White;
            PanelHeader.Height = 60;
            label1.ForeColor = UIHelper.DarkBlue;
            label1.Font = UIHelper.HeaderFont;
            label1.Text = "TỔNG QUAN"; // Tiêu đề mặc định

            // 4. Style nút Menu
            StyleSideButton(btnSinhVien);
            StyleSideButton(btnLopHoc);
            StyleSideButton(btnDiemSo);

            // Panel chứa nội dung chính
            panelMain.BackColor = UIHelper.LightGray;
            panelMain.Padding = new Padding(20); // Tạo khoảng cách xung quanh nội dung
        }

        private void StyleSideButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = UIHelper.DarkBlue; // Cùng màu nền menu
            btn.ForeColor = Color.FromArgb(180, 180, 180); // Màu chữ xám nhạt khi chưa chọn
            btn.Font = UIHelper.RegularFont;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(25, 0, 0, 0);
            btn.Height = 55;
            btn.Cursor = Cursors.Hand;
            btn.Dock = DockStyle.Top;

            // Hiệu ứng di chuột
            btn.MouseEnter += (s, e) => { if (btn.Tag == null) btn.ForeColor = UIHelper.White; };
            btn.MouseLeave += (s, e) => { if (btn.Tag == null) btn.ForeColor = Color.FromArgb(180, 180, 180); };
        }

        private void LoadUserControl(UserControl uc, Button activeBtn)
        {
            // Reset trạng thái các nút
            ResetButton(btnSinhVien);
            ResetButton(btnLopHoc);
            ResetButton(btnDiemSo);

            // Highlight nút đang chọn
            activeBtn.BackColor = UIHelper.PrimaryColor; // Màu xanh nổi bật
            activeBtn.ForeColor = UIHelper.White;
            activeBtn.Tag = "active"; // Đánh dấu
            label1.Text = activeBtn.Text.ToUpper(); // Cập nhật tiêu đề Header

            // Load UC
            uc.Dock = DockStyle.Fill;
            panelMain.Controls.Clear();
            panelMain.Controls.Add(uc);
        }

        private void ResetButton(Button btn)
        {
            btn.BackColor = UIHelper.DarkBlue;
            btn.ForeColor = Color.FromArgb(180, 180, 180);
            btn.Tag = null;
        }

        private void btnSinhVien_Click(object sender, EventArgs e) => LoadUserControl(new UC_SinhVien(), btnSinhVien);
        private void btnLopHoc_Click(object sender, EventArgs e) => LoadUserControl(new UC_LopHoc(), btnLopHoc);
        private void btnDiemSo_Click(object sender, EventArgs e) => LoadUserControl(new UC_DiemSo(), btnDiemSo);
        private void Form1_Load(object sender, EventArgs e) { }
    }
}