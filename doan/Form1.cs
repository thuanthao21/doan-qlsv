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
            // Mặc định mở trang Sinh Viên
            LoadUserControl(new UC_SinhVien(), btnSinhVien);
        }

        private void SetupDashboard()
        {
            // Cài đặt Form
            this.Text = "HỆ THỐNG QUẢN LÝ ĐÀO TẠO";
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Màu sắc Menu
            PanelMenu.BackColor = UIHelper.SecondaryColor;
            PanelHeader.BackColor = Color.White;
            label1.ForeColor = UIHelper.SecondaryColor;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold);

            // Style nút Menu
            StyleSideButton(btnSinhVien);
            StyleSideButton(btnLopHoc);
            StyleSideButton(btnDiemSo);
        }

        private void StyleSideButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = UIHelper.SecondaryColor;
            btn.ForeColor = Color.Gainsboro;
            btn.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(15, 0, 0, 0);
            btn.Height = 50;
            btn.Cursor = Cursors.Hand;
        }

        private void LoadUserControl(UserControl uc, Button activeBtn)
        {
            // Reset màu
            btnSinhVien.BackColor = UIHelper.SecondaryColor;
            btnLopHoc.BackColor = UIHelper.SecondaryColor;
            btnDiemSo.BackColor = UIHelper.SecondaryColor;

            // Highlight nút đang chọn
            activeBtn.BackColor = UIHelper.PrimaryColor;
            label1.Text = activeBtn.Text.ToUpper();

            // Load UC
            uc.Dock = DockStyle.Fill;
            panelMain.Controls.Clear();
            panelMain.Controls.Add(uc);
        }

        private void btnSinhVien_Click(object sender, EventArgs e) => LoadUserControl(new UC_SinhVien(), btnSinhVien);
        private void btnLopHoc_Click(object sender, EventArgs e) => LoadUserControl(new UC_LopHoc(), btnLopHoc);
        private void btnDiemSo_Click(object sender, EventArgs e) => LoadUserControl(new UC_DiemSo(), btnDiemSo);

        private void Form1_Load(object sender, EventArgs e) { }
    }
}