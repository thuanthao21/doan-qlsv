using System;
using System.Drawing;
using System.Windows.Forms;

namespace doan
{
    public partial class Form1 : Form
    {
        // Khai báo nút Khoa (nếu chưa có trong Designer)
        private Button btnKhoa;

        public Form1()
        {
            InitializeComponent();
            InitButtonKhoa();  // Tạo nút Khoa bằng code
            SetupDashboard();

            // SỰ KIỆN QUAN TRỌNG: Phân quyền khi form vừa hiện lên
            this.Load += (s, e) => PhanQuyenNguoiDung();
        }

        private void InitButtonKhoa()
        {
            // Kiểm tra xem trong PanelMenu đã có nút btnKhoa chưa
            if (!PanelMenu.Controls.ContainsKey("btnKhoa"))
            {
                btnKhoa = new Button();
                btnKhoa.Name = "btnKhoa";
                btnKhoa.Text = "Quản lý Khoa";

                PanelMenu.Controls.Add(btnKhoa);
                btnKhoa.Dock = DockStyle.Top; // Xếp chồng từ trên xuống

                // Đảo thứ tự để nút Khoa nằm đúng vị trí mong muốn
                btnDiemSo.SendToBack();
                btnLopHoc.SendToBack();
                btnKhoa.SendToBack();
                btnSinhVien.SendToBack();
            }
            else
            {
                btnKhoa = (Button)PanelMenu.Controls["btnKhoa"];
            }

            // Gán sự kiện click cho nút Khoa
            btnKhoa.Click += (s, e) => LoadUserControl(new UC_Khoa(), btnKhoa);
        }

        // --- HÀM PHÂN QUYỀN (MỚI) ---
        private void PhanQuyenNguoiDung()
        {
            // 1. Hiển thị lời chào
            string role = UserSession.QuyenHan == "Admin" ? "Quản Trị Viên" : "Sinh Viên";
            this.Text = $"HỆ THỐNG QUẢN LÝ ĐÀO TẠO - Xin chào: {UserSession.TenHienThi} ({role})";

            // 2. Nếu là Sinh Viên -> Ẩn các nút quản lý
            if (UserSession.QuyenHan != "Admin")
            {
                if (btnSinhVien != null) btnSinhVien.Visible = false;
                if (btnLopHoc != null) btnLopHoc.Visible = false;
                if (btnKhoa != null) btnKhoa.Visible = false;

                // Mặc định nhảy vào trang Điểm số (trang duy nhất họ được xem)
                LoadUserControl(new UC_DiemSo(), btnDiemSo);
            }
            else
            {
                // Nếu là Admin -> Mặc định vào trang Sinh Viên
                LoadUserControl(new UC_SinhVien(), btnSinhVien);
            }
        }

        private void SetupDashboard()
        {
            this.Size = new Size(1200, 768);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = UIHelper.LightGray;

            PanelMenu.BackColor = UIHelper.DarkBlue;
            PanelMenu.Width = 220;

            PanelHeader.BackColor = UIHelper.White;
            PanelHeader.Height = 60;
            label1.ForeColor = UIHelper.DarkBlue;
            label1.Font = UIHelper.HeaderFont;
            label1.Text = "TỔNG QUAN";

            // Style nút menu
            StyleSideButton(btnSinhVien);
            StyleSideButton(btnLopHoc);
            StyleSideButton(btnDiemSo);
            StyleSideButton(btnKhoa);

            panelMain.BackColor = UIHelper.LightGray;
            panelMain.Padding = new Padding(20);
        }

        private void StyleSideButton(Button btn)
        {
            if (btn == null) return;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = UIHelper.DarkBlue;
            btn.ForeColor = Color.FromArgb(180, 180, 180);
            btn.Font = UIHelper.RegularFont;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(25, 0, 0, 0);
            btn.Height = 55;
            btn.Cursor = Cursors.Hand;
            btn.Dock = DockStyle.Top;

            btn.MouseEnter += (s, e) => { if (btn.Tag == null) btn.ForeColor = UIHelper.White; };
            btn.MouseLeave += (s, e) => { if (btn.Tag == null) btn.ForeColor = Color.FromArgb(180, 180, 180); };
        }

        private void LoadUserControl(UserControl uc, Button activeBtn)
        {
            ResetButton(btnSinhVien);
            ResetButton(btnLopHoc);
            ResetButton(btnDiemSo);
            ResetButton(btnKhoa);

            if (activeBtn != null)
            {
                activeBtn.BackColor = UIHelper.PrimaryColor;
                activeBtn.ForeColor = UIHelper.White;
                activeBtn.Tag = "active";
                label1.Text = activeBtn.Text.ToUpper();
            }

            uc.Dock = DockStyle.Fill;
            panelMain.Controls.Clear();
            panelMain.Controls.Add(uc);
        }

        private void ResetButton(Button btn)
        {
            if (btn == null) return;
            btn.BackColor = UIHelper.DarkBlue;
            btn.ForeColor = Color.FromArgb(180, 180, 180);
            btn.Tag = null;
        }

        // Các sự kiện click có sẵn từ Designer
        private void btnSinhVien_Click(object sender, EventArgs e) => LoadUserControl(new UC_SinhVien(), btnSinhVien);
        private void btnLopHoc_Click(object sender, EventArgs e) => LoadUserControl(new UC_LopHoc(), btnLopHoc);
        private void btnDiemSo_Click(object sender, EventArgs e) => LoadUserControl(new UC_DiemSo(), btnDiemSo);
    }
}