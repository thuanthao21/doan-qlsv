using System;
using System.Drawing;
using System.Windows.Forms;

namespace doan
{
    public partial class Form1 : Form
    {
        // Khai báo các nút khởi tạo thêm bằng code
        private Button btnKhoa;
        private Button btnMonHoc;
        private Button btnQuanLyTK; // Nút mới thay cho chức năng đăng ký tự do
        private Button btnLogout;

        public Form1()
        {
            InitializeComponent();
            InitAdditionalButtons(); // Khởi tạo các nút chức năng bổ sung
            SetupDashboard();        // Cấu hình giao diện tổng thể

            // SỰ KIỆN LOAD: Thực hiện phân quyền ngay khi mở Form
            this.Load += (s, e) => PhanQuyenNguoiDung();
        }

        private void InitAdditionalButtons()
        {
            // --- 1. KHỞI TẠO NÚT QUẢN LÝ KHOA ---
            if (!PanelMenu.Controls.ContainsKey("btnKhoa"))
            {
                btnKhoa = new Button { Name = "btnKhoa", Text = "Quản lý Khoa" };
                PanelMenu.Controls.Add(btnKhoa);
                btnKhoa.Dock = DockStyle.Top;
            }
            else
            {
                btnKhoa = (Button)PanelMenu.Controls["btnKhoa"];
            }
            btnKhoa.Click += (s, e) => LoadUserControl(new UC_Khoa(), btnKhoa);

            // --- 2. KHỞI TẠO NÚT QUẢN LÝ MÔN HỌC ---
            if (!PanelMenu.Controls.ContainsKey("btnMonHoc"))
            {
                btnMonHoc = new Button { Name = "btnMonHoc", Text = "Quản lý Môn học" };
                PanelMenu.Controls.Add(btnMonHoc);
                btnMonHoc.Dock = DockStyle.Top;
            }
            else
            {
                btnMonHoc = (Button)PanelMenu.Controls["btnMonHoc"];
            }
            btnMonHoc.Click += (s, e) => LoadUserControl(new UC_MonHoc(), btnMonHoc);

            // --- 3. KHỞI TẠO NÚT QUẢN LÝ TÀI KHOẢN (THAY CHO ĐĂNG KÝ) ---
            btnQuanLyTK = new Button { Name = "btnQuanLyTK", Text = "Quản lý Tài khoản" };
            PanelMenu.Controls.Add(btnQuanLyTK);
            btnQuanLyTK.Dock = DockStyle.Top;
            btnQuanLyTK.Click += (s, e) => LoadUserControl(new UC_QuanLyTaiKhoan(), btnQuanLyTK);

            // --- 4. KHỞI TẠO NÚT ĐĂNG XUẤT ---
            btnLogout = new Button
            {
                Name = "btnLogout",
                Text = "Đăng xuất",
                Dock = DockStyle.Bottom, // Luôn nằm ở đáy menu
                Height = 55
            };
            PanelMenu.Controls.Add(btnLogout);
            btnLogout.Click += BtnLogout_Click;

            // Sắp xếp thứ tự hiển thị nút từ trên xuống dưới: SV -> TK -> Khoa -> Môn -> Lớp -> Điểm
            btnDiemSo.SendToBack();
            btnLopHoc.SendToBack();
            btnMonHoc.SendToBack();
            btnKhoa.SendToBack();
            btnQuanLyTK.SendToBack();
            btnSinhVien.SendToBack();
        }

        private void PhanQuyenNguoiDung()
        {
            // Hiển thị lời chào dựa trên Session đăng nhập
            string roleName = UserSession.IsAdmin() ? "Quản Trị Viên" : "Sinh Viên";
            this.Text = $"HỆ THỐNG QUẢN LÝ ĐÀO TẠO - [ {UserSession.TenHienThi} | {roleName} ]";

            if (!UserSession.IsAdmin())
            {
                // Nếu là Sinh viên: Ẩn TOÀN BỘ chức năng quản lý
                if (btnSinhVien != null) btnSinhVien.Visible = false;
                if (btnKhoa != null) btnKhoa.Visible = false;
                if (btnMonHoc != null) btnMonHoc.Visible = false;
                if (btnLopHoc != null) btnLopHoc.Visible = false;
                if (btnQuanLyTK != null) btnQuanLyTK.Visible = false;

                // Tự động mở trang xem điểm duy nhất cho sinh viên
                LoadUserControl(new UC_DiemSo(), btnDiemSo);
            }
            else
            {
                // Nếu là Admin: Mặc định mở trang quản lý Sinh viên
                LoadUserControl(new UC_SinhVien(), btnSinhVien);
            }
        }

        private void SetupDashboard()
        {
            this.Size = new Size(1300, 850);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = UIHelper.LightGray;

            PanelMenu.BackColor = UIHelper.DarkBlue;
            PanelMenu.Width = 230;

            PanelHeader.BackColor = UIHelper.White;
            PanelHeader.Height = 65;
            label1.ForeColor = UIHelper.DarkBlue;
            label1.Font = UIHelper.HeaderFont;
            label1.Text = "HỆ THỐNG QUẢN LÝ";

            // Áp dụng Style đồng bộ cho tất cả các nút Sidebar
            StyleSideButton(btnSinhVien);
            StyleSideButton(btnQuanLyTK);
            StyleSideButton(btnKhoa);
            StyleSideButton(btnMonHoc);
            StyleSideButton(btnLopHoc);
            StyleSideButton(btnDiemSo);
            StyleSideButton(btnLogout);

            btnLogout.ForeColor = Color.FromArgb(255, 150, 150); // Màu đỏ nhạt cho Đăng xuất

            panelMain.BackColor = UIHelper.LightGray;
            panelMain.Padding = new Padding(20);
        }

        private void StyleSideButton(Button btn)
        {
            if (btn == null) return;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = UIHelper.DarkBlue;
            btn.ForeColor = Color.FromArgb(190, 190, 190);
            btn.Font = UIHelper.RegularFont;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(30, 0, 0, 0);
            btn.Height = 60;
            btn.Cursor = Cursors.Hand;
            btn.Dock = DockStyle.Top;

            btn.MouseEnter += (s, e) => { if (btn.Tag == null) btn.ForeColor = UIHelper.White; };
            btn.MouseLeave += (s, e) => { if (btn.Tag == null) btn.ForeColor = Color.FromArgb(190, 190, 190); };
        }

        private void LoadUserControl(UserControl uc, Button activeBtn)
        {
            // Reset trạng thái tất cả các nút
            ResetButton(btnSinhVien);
            ResetButton(btnQuanLyTK);
            ResetButton(btnKhoa);
            ResetButton(btnMonHoc);
            ResetButton(btnLopHoc);
            ResetButton(btnDiemSo);
            ResetButton(btnLogout);

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
            btn.ForeColor = Color.FromArgb(190, 190, 190);
            btn.Tag = null;
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                UserSession.LogOut();
                this.Hide();
                FrmLogin loginForm = new FrmLogin();
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    Form1 f1 = new Form1();
                    f1.Show();
                    this.Close();
                }
                else { Application.Exit(); }
            }
        }

        private void btnSinhVien_Click(object sender, EventArgs e) => LoadUserControl(new UC_SinhVien(), btnSinhVien);
        private void btnLopHoc_Click(object sender, EventArgs e) => LoadUserControl(new UC_LopHoc(), btnLopHoc);
        private void btnDiemSo_Click(object sender, EventArgs e) => LoadUserControl(new UC_DiemSo(), btnDiemSo);
    }
}