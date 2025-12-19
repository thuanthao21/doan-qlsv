using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace doan
{
    public partial class UC_SinhVien : UserControl
    {
        private List<SinhVien> listSV = new List<SinhVien>();
        private PictureBox pbAnhDaiDien;
        private Button btnChonAnh;
        private string currentImagePath = "";

        public UC_SinhVien()
        {
            InitializeComponent();
            SetupControls();
            ApplyModernLayout();
            LoadData();
            RegisterEvents();
        }

        private void SetupControls()
        {
            if (this.Controls.Find("pbAnhDaiDien", true).Length == 0)
            {
                pbAnhDaiDien = new PictureBox { Name = "pbAnhDaiDien", SizeMode = PictureBoxSizeMode.Zoom, BorderStyle = BorderStyle.FixedSingle, BackColor = UIHelper.White };
                btnChonAnh = new Button { Name = "btnChonAnh", Text = "Chọn ảnh" };
                groupBox1.Controls.Add(pbAnhDaiDien);
                groupBox1.Controls.Add(btnChonAnh);
            }
            else
            {
                pbAnhDaiDien = (PictureBox)this.Controls.Find("pbAnhDaiDien", true)[0];
                btnChonAnh = (Button)this.Controls.Find("btnChonAnh", true)[0];
            }
        }

        // --- HÀM CĂN CHỈNH BỐ CỤC MỚI ---
        private void ApplyModernLayout()
        {
            UIHelper.Beautify(this); // Áp dụng style chung

            // Style nút chức năng
            UIHelper.StyleButton(btnThem, UIHelper.SuccessColor);
            UIHelper.StyleButton(btnSua, UIHelper.WarningColor);
            UIHelper.StyleButton(btnXoa, UIHelper.DangerColor);
            UIHelper.StyleButton(btnLamMoi, UIHelper.InfoColor);
            UIHelper.StyleButton(btnChonAnh, UIHelper.PrimaryColor); btnChonAnh.Width = 120;

            // CẤU HÌNH GROUPBOX
            groupBox1.Text = "THÔNG TIN CHI TIẾT";
            groupBox1.Height = 400; // Tăng chiều cao
            groupBox1.BackColor = UIHelper.White; // Nền trắng cho vùng nhập liệu
            groupBox1.FlatStyle = FlatStyle.Flat;

            // HỆ THỐNG LƯỚI (Grid System)
            int col1_Label = 30;
            int col1_Input = 150;
            int col2_Label = 450; // Cột 2 bắt đầu từ đây
            int col2_Input = 550;

            int rowStart = 50;
            int rowGap = 50; // Khoảng cách giữa các dòng
            int inputHeight = 32; // Chiều cao chuẩn ô nhập

            // DÒNG 1: Mã SV | Tìm kiếm
            label1.Location = new Point(col1_Label, rowStart);
            txtMaSV.Location = new Point(col1_Input, rowStart - 5); txtMaSV.Size = new Size(200, inputHeight);

            label7.Location = new Point(col2_Label, rowStart);
            txtTimKiem.Location = new Point(col2_Input, rowStart - 5); txtTimKiem.Size = new Size(250, inputHeight); txtTimKiem.PlaceholderText = "Nhập tên hoặc mã để tìm...";

            // DÒNG 2: Họ tên | Ngày sinh
            label2.Location = new Point(col1_Label, rowStart + rowGap);
            txtHoTen.Location = new Point(col1_Input, rowStart + rowGap - 5); txtHoTen.Size = new Size(250, inputHeight);

            label5.Location = new Point(col2_Label, rowStart + rowGap);
            dtpNgaySinh.Location = new Point(col2_Input, rowStart + rowGap - 5); dtpNgaySinh.Size = new Size(250, inputHeight);

            // DÒNG 3: Email | Giới tính
            label3.Location = new Point(col1_Label, rowStart + rowGap * 2);
            txtEmail.Location = new Point(col1_Input, rowStart + rowGap * 2 - 5); txtEmail.Size = new Size(250, inputHeight);

            label6.Location = new Point(col2_Label, rowStart + rowGap * 2);
            rbNam.Location = new Point(col2_Input, rowStart + rowGap * 2);
            rbNu.Location = new Point(col2_Input + 80, rowStart + rowGap * 2);

            // DÒNG 4: SĐT | Lớp
            label4.Location = new Point(col1_Label, rowStart + rowGap * 3);
            txtSDT.Location = new Point(col1_Input, rowStart + rowGap * 3 - 5); txtSDT.Size = new Size(250, inputHeight);

            label8.Location = new Point(col2_Label, rowStart + rowGap * 3);
            cbLop.Location = new Point(col2_Input, rowStart + rowGap * 3 - 5); cbLop.Size = new Size(250, inputHeight);

            // KHU VỰC ẢNH (Bên phải ngoài cùng)
            int photoX = 850;
            pbAnhDaiDien.Location = new Point(photoX, rowStart - 5);
            pbAnhDaiDien.Size = new Size(160, 200);
            btnChonAnh.Location = new Point(photoX + 20, pbAnhDaiDien.Bottom + 15);

            // KHU VỰC NÚT CHỨC NĂNG (Dưới cùng, căn giữa)
            int btnY = groupBox1.Bottom + 20;
            int btnStartX = (this.Width - (4 * 120 + 3 * 20)) / 2; // Căn giữa
            btnThem.Location = new Point(btnStartX, btnY);
            btnSua.Location = new Point(btnThem.Right + 20, btnY);
            btnXoa.Location = new Point(btnSua.Right + 20, btnY);
            btnLamMoi.Location = new Point(btnXoa.Right + 20, btnY);

            // Tùy chỉnh bảng
            dgvSinhVien.Location = new Point(0, btnY + 60);
            dgvSinhVien.Height = this.Height - dgvSinhVien.Top - 10;
            dgvSinhVien.RowTemplate.Height = 70; // Dòng cao để hiện ảnh rõ
        }

        private void RegisterEvents()
        {
            btnThem.Click -= btnThem_Click; btnThem.Click += btnThem_Click;
            btnSua.Click -= btnSua_Click; btnSua.Click += btnSua_Click;
            btnXoa.Click -= btnXoa_Click; btnXoa.Click += btnXoa_Click;
            btnLamMoi.Click -= btnLamMoi_Click; btnLamMoi.Click += btnLamMoi_Click;
            txtTimKiem.TextChanged -= txtTimKiem_TextChanged; txtTimKiem.TextChanged += txtTimKiem_TextChanged;
            dgvSinhVien.CellClick -= dgvSinhVien_CellClick; dgvSinhVien.CellClick += dgvSinhVien_CellClick;
            btnChonAnh.Click -= BtnChonAnh_Click; btnChonAnh.Click += BtnChonAnh_Click;
        }

        // --- CÁC HÀM LOGIC (Giữ nguyên, chỉ cập nhật phần hiển thị ảnh trong bảng) ---
        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e) { if (e.RowIndex < 0) return; try { var sv = dgvSinhVien.Rows[e.RowIndex].DataBoundItem as SinhVien; if (sv == null) return; txtMaSV.Text = sv.MaSV; txtHoTen.Text = sv.HoTen; txtEmail.Text = sv.Email; txtSDT.Text = sv.SoDienThoai; cbLop.Text = sv.Lop; try { dtpNgaySinh.Value = DateTime.ParseExact(sv.NgaySinh, "dd/MM/yyyy", null); } catch { dtpNgaySinh.Value = DateTime.Now; } if (sv.GioiTinh == "Nam") rbNam.Checked = true; else rbNu.Checked = true; LoadImageToPictureBox(sv.AnhDaiDien); txtMaSV.Enabled = false; } catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); } }
        private void BtnChonAnh_Click(object sender, EventArgs e) { OpenFileDialog ofd = new OpenFileDialog(); ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"; if (ofd.ShowDialog() == DialogResult.OK) { pbAnhDaiDien.Image = Image.FromFile(ofd.FileName); currentImagePath = ofd.FileName; } }
        private string CopyImageToAppFolder(string sourcePath, string maSV) { if (string.IsNullOrEmpty(sourcePath) || !File.Exists(sourcePath)) return ""; string imagesFolder = Path.Combine(Application.StartupPath, "Images"); if (!Directory.Exists(imagesFolder)) Directory.CreateDirectory(imagesFolder); string ext = Path.GetExtension(sourcePath); string newFileName = $"{maSV}_{DateTime.Now.Ticks}{ext}"; string destinationPath = Path.Combine(imagesFolder, newFileName); File.Copy(sourcePath, destinationPath, true); return newFileName; }
        private void LoadImageToPictureBox(string fileName) { if (pbAnhDaiDien.Image != null) pbAnhDaiDien.Image.Dispose(); pbAnhDaiDien.Image = null; currentImagePath = ""; if (string.IsNullOrEmpty(fileName)) return; string imagePath = Path.Combine(Application.StartupPath, "Images", fileName); if (File.Exists(imagePath)) { try { using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read)) { pbAnhDaiDien.Image = Image.FromStream(fs); } } catch { } } }
        private void LoadData() { listSV = DataHelper.DocSV(); HienThiTable(listSV); cbLop.Items.Clear(); foreach (var l in DataHelper.DocLop()) cbLop.Items.Add(l.TenLop); }
        private void HienThiTable(List<SinhVien> source) { dgvSinhVien.DataSource = null; dgvSinhVien.Columns.Clear(); dgvSinhVien.DataSource = source; if (dgvSinhVien.Columns.Contains("AnhDaiDien")) dgvSinhVien.Columns["AnhDaiDien"].Visible = false; DataGridViewImageColumn imgCol = new DataGridViewImageColumn(); imgCol.Name = "ColAnhThat"; imgCol.HeaderText = "Ảnh"; imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom; imgCol.Width = 80; dgvSinhVien.Columns.Insert(0, imgCol); dgvSinhVien.Columns["MaSV"].HeaderText = "Mã SV"; dgvSinhVien.Columns["HoTen"].HeaderText = "Họ và Tên"; dgvSinhVien.Columns["SoDienThoai"].HeaderText = "Điện Thoại"; dgvSinhVien.Columns["NgaySinh"].HeaderText = "Ngày Sinh"; foreach (DataGridViewRow row in dgvSinhVien.Rows) row.Height = 70; string imgFolder = Path.Combine(Application.StartupPath, "Images"); foreach (DataGridViewRow row in dgvSinhVien.Rows) { var sv = row.DataBoundItem as SinhVien; if (sv != null && !string.IsNullOrEmpty(sv.AnhDaiDien)) { string path = Path.Combine(imgFolder, sv.AnhDaiDien); if (File.Exists(path)) { try { using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) { row.Cells["ColAnhThat"].Value = Image.FromStream(fs).Clone(); } } catch { } } } } }
        private void btnThem_Click(object sender, EventArgs e) { string maSVInput = txtMaSV.Text.Trim(); if (string.IsNullOrEmpty(maSVInput)) { MessageBox.Show("Bạn chưa nhập Mã Sinh Viên!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtMaSV.Focus(); return; } var svTrung = listSV.FirstOrDefault(x => x.MaSV.ToLower() == maSVInput.ToLower()); if (svTrung != null) { MessageBox.Show($"Mã '{maSVInput}' đã tồn tại!", "Trùng dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error); txtMaSV.SelectAll(); txtMaSV.Focus(); return; } XuLyLuu(true); }
        private void XuLyLuu(bool isNew) { SinhVien sv = isNew ? new SinhVien() : listSV.First(x => x.MaSV == txtMaSV.Text); if (!string.IsNullOrEmpty(currentImagePath)) { if (!isNew && !string.IsNullOrEmpty(sv.AnhDaiDien)) { string oldPath = Path.Combine(Application.StartupPath, "Images", sv.AnhDaiDien); if (File.Exists(oldPath)) try { File.Delete(oldPath); } catch { } } sv.AnhDaiDien = CopyImageToAppFolder(currentImagePath, txtMaSV.Text); } sv.MaSV = txtMaSV.Text; sv.HoTen = txtHoTen.Text; sv.Email = txtEmail.Text; sv.SoDienThoai = txtSDT.Text; sv.NgaySinh = dtpNgaySinh.Value.ToString("dd/MM/yyyy"); sv.GioiTinh = rbNam.Checked ? "Nam" : "Nữ"; sv.Lop = cbLop.Text; if (isNew) listSV.Add(sv); DataHelper.LuuSV(listSV); HienThiTable(listSV); btnLamMoi_Click(null, null); MessageBox.Show("Lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void btnSua_Click(object sender, EventArgs e) { if (txtMaSV.Enabled) { MessageBox.Show("Vui lòng chọn sinh viên cần sửa!", "Thông báo"); return; } XuLyLuu(false); }
        private void btnXoa_Click(object sender, EventArgs e) { var sv = listSV.FirstOrDefault(x => x.MaSV == txtMaSV.Text); if (sv != null && MessageBox.Show("Xóa sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes) { if (!string.IsNullOrEmpty(sv.AnhDaiDien)) { string path = Path.Combine(Application.StartupPath, "Images", sv.AnhDaiDien); if (File.Exists(path)) try { File.Delete(path); } catch { } } listSV.Remove(sv); DataHelper.LuuSV(listSV); HienThiTable(listSV); btnLamMoi_Click(null, null); } }
        private void btnLamMoi_Click(object sender, EventArgs e) { txtMaSV.Clear(); txtHoTen.Clear(); txtEmail.Clear(); txtSDT.Clear(); cbLop.SelectedIndex = -1; rbNam.Checked = true; dtpNgaySinh.Value = DateTime.Now; if (pbAnhDaiDien.Image != null) pbAnhDaiDien.Image.Dispose(); pbAnhDaiDien.Image = null; currentImagePath = ""; txtMaSV.Enabled = true; txtMaSV.Focus(); }
        private void txtTimKiem_TextChanged(object sender, EventArgs e) { var keyword = txtTimKiem.Text.ToLower(); var filtered = listSV.Where(x => x.HoTen.ToLower().Contains(keyword) || x.MaSV.ToLower().Contains(keyword)).ToList(); HienThiTable(filtered); }
    }
}