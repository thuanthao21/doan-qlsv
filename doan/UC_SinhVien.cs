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

        // ErrorProvider để báo lỗi nhập liệu
        private ErrorProvider errorProvider = new ErrorProvider();

        public UC_SinhVien()
        {
            InitializeComponent();
            SetupControls();        // Tạo nút chọn ảnh

            // [QUAN TRỌNG] Các hàm này phải gọi theo thứ tự
            ApplyModernLayout();    // 1. Sắp xếp giao diện, lôi nút ra ngoài
            InitTableColumns();     // 2. Tạo cột cho bảng
            LoadData();             // 3. Đổ dữ liệu vào bảng
            RegisterEvents();       // 4. Bắt sự kiện click
        }

        // --- 1. SETUP GIAO DIỆN & NÚT BẤM (FIX LỖI MẤT NÚT) ---

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

        private void ApplyModernLayout()
        {
            UIHelper.Beautify(this);

            // [FIX LỖI NÚT BỊ CHE] Lôi các nút ra khỏi GroupBox và đưa về Form chính
            if (groupBox1.Controls.Contains(btnThem)) groupBox1.Controls.Remove(btnThem);
            if (groupBox1.Controls.Contains(btnSua)) groupBox1.Controls.Remove(btnSua);
            if (groupBox1.Controls.Contains(btnXoa)) groupBox1.Controls.Remove(btnXoa);
            if (groupBox1.Controls.Contains(btnLamMoi)) groupBox1.Controls.Remove(btnLamMoi);

            this.Controls.Add(btnThem);
            this.Controls.Add(btnSua);
            this.Controls.Add(btnXoa);
            this.Controls.Add(btnLamMoi);

            // Đưa nút lên lớp trên cùng để không bị bảng đè
            btnThem.BringToFront(); btnSua.BringToFront(); btnXoa.BringToFront(); btnLamMoi.BringToFront();

            // Style nút
            UIHelper.StyleButton(btnThem, UIHelper.SuccessColor);
            UIHelper.StyleButton(btnSua, UIHelper.WarningColor);
            UIHelper.StyleButton(btnXoa, UIHelper.DangerColor);
            UIHelper.StyleButton(btnLamMoi, UIHelper.InfoColor);
            UIHelper.StyleButton(btnChonAnh, UIHelper.PrimaryColor); btnChonAnh.Width = 120;

            // Layout GroupBox
            groupBox1.Text = "THÔNG TIN CHI TIẾT";
            groupBox1.Dock = DockStyle.Top; // Ghim lên trên cùng
            groupBox1.Height = 360;         // Chiều cao cố định
            groupBox1.BackColor = UIHelper.White;

            // Căn chỉnh các ô nhập liệu
            int col1_L = 30, col1_I = 150;
            int col2_L = 450, col2_I = 550;
            int rowStart = 40, rowGap = 45, inputHeight = 30;

            label1.Location = new Point(col1_L, rowStart);
            txtMaSV.Location = new Point(col1_I, rowStart - 5); txtMaSV.Size = new Size(200, inputHeight);

            label7.Location = new Point(col2_L, rowStart);
            txtTimKiem.Location = new Point(col2_I, rowStart - 5); txtTimKiem.Size = new Size(250, inputHeight); txtTimKiem.PlaceholderText = "Nhập tên hoặc mã...";

            label2.Location = new Point(col1_L, rowStart + rowGap);
            txtHoTen.Location = new Point(col1_I, rowStart + rowGap - 5); txtHoTen.Size = new Size(250, inputHeight);

            label5.Location = new Point(col2_L, rowStart + rowGap);
            dtpNgaySinh.Location = new Point(col2_I, rowStart + rowGap - 5); dtpNgaySinh.Size = new Size(250, inputHeight);

            label3.Location = new Point(col1_L, rowStart + rowGap * 2);
            txtEmail.Location = new Point(col1_I, rowStart + rowGap * 2 - 5); txtEmail.Size = new Size(250, inputHeight);

            label6.Location = new Point(col2_L, rowStart + rowGap * 2);
            rbNam.Location = new Point(col2_I, rowStart + rowGap * 2);
            rbNu.Location = new Point(col2_I + 80, rowStart + rowGap * 2);

            label4.Location = new Point(col1_L, rowStart + rowGap * 3);
            txtSDT.Location = new Point(col1_I, rowStart + rowGap * 3 - 5); txtSDT.Size = new Size(250, inputHeight);

            label8.Location = new Point(col2_L, rowStart + rowGap * 3);
            cbLop.Location = new Point(col2_I, rowStart + rowGap * 3 - 5); cbLop.Size = new Size(250, inputHeight);

            int photoX = 850;
            pbAnhDaiDien.Location = new Point(photoX, rowStart - 5);
            pbAnhDaiDien.Size = new Size(130, 160);
            btnChonAnh.Location = new Point(photoX + 5, pbAnhDaiDien.Bottom + 10);

            // [FIX VỊ TRÍ NÚT] Đặt nút nằm ngay dưới GroupBox
            int btnY = groupBox1.Height + 15;
            int btnStartX = (this.Width - (4 * 120 + 3 * 20)) / 2;

            btnThem.Location = new Point(btnStartX, btnY);
            btnSua.Location = new Point(btnThem.Right + 20, btnY);
            btnXoa.Location = new Point(btnSua.Right + 20, btnY);
            btnLamMoi.Location = new Point(btnXoa.Right + 20, btnY);

            // [FIX LỖI BẢNG ĐÈ LÊN NÚT] Tắt Dock Fill và đẩy bảng xuống dưới nút
            dgvSinhVien.Dock = DockStyle.None;
            dgvSinhVien.Location = new Point(0, btnY + 50); // Cách nút 50px
            dgvSinhVien.Size = new Size(this.Width, this.Height - dgvSinhVien.Top); // Chiều cao còn lại
            dgvSinhVien.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right; // Tự co giãn
        }

        // --- 2. XỬ LÝ DỮ LIỆU & BẢNG (FIX LỖI CRASH "Index -1") ---

        private void InitTableColumns()
        {
            dgvSinhVien.AutoGenerateColumns = false;
            dgvSinhVien.Columns.Clear();

            // Cột Ảnh
            DataGridViewImageColumn imgCol = new DataGridViewImageColumn();
            imgCol.Name = "ColAnhThat";
            imgCol.HeaderText = "Ảnh";
            imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
            imgCol.Width = 80;
            dgvSinhVien.Columns.Add(imgCol);

            // Các cột Text
            AddTextColumn("MaSV", "Mã SV", "MaSV", 100);
            AddTextColumn("HoTen", "Họ và Tên", "HoTen", 200);
            AddTextColumn("Email", "Email", "Email", 150);
            AddTextColumn("SoDienThoai", "Điện Thoại", "SoDienThoai", 120);
            AddTextColumn("NgaySinh", "Ngày Sinh", "NgaySinh", 100);
            AddTextColumn("GioiTinh", "Giới Tính", "GioiTinh", 80);
            AddTextColumn("Lop", "Lớp", "Lop", 100);
        }

        private void AddTextColumn(string name, string header, string property, int width)
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.Name = name;
            col.HeaderText = header;
            col.DataPropertyName = property;
            col.Width = width;
            dgvSinhVien.Columns.Add(col);
        }

        private void HienThiTable(List<SinhVien> source)
        {
            // [FIX CRASH] Dùng BindingSource làm trung gian để tránh lỗi Index -1
            var bindingSource = new BindingSource();
            bindingSource.DataSource = source;
            dgvSinhVien.DataSource = bindingSource;

            // Load ảnh
            string imgFolder = Path.Combine(Application.StartupPath, "Images");
            foreach (DataGridViewRow row in dgvSinhVien.Rows)
            {
                row.Height = 70;
                var sv = row.DataBoundItem as SinhVien;

                if (sv != null && !string.IsNullOrEmpty(sv.AnhDaiDien))
                {
                    string path = Path.Combine(imgFolder, sv.AnhDaiDien);
                    if (File.Exists(path))
                    {
                        try
                        {
                            using (var temp = new Bitmap(path))
                            {
                                row.Cells["ColAnhThat"].Value = new Bitmap(temp);
                            }
                        }
                        catch { }
                    }
                }
            }
        }

        private void LoadData()
        {
            listSV = DataHelper.DocSV();
            HienThiTable(listSV);

            // Load ComboBox Lớp
            cbLop.Items.Clear();
            foreach (var l in DataHelper.DocLop())
                cbLop.Items.Add(l.TenLop);
        }

        // --- 3. XỬ LÝ SỰ KIỆN ---

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

        private void btnThem_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtMaSV.Text)) { errorProvider.SetError(txtMaSV, "Nhập mã!"); isValid = false; }
            if (string.IsNullOrWhiteSpace(txtHoTen.Text)) { errorProvider.SetError(txtHoTen, "Nhập tên!"); isValid = false; }

            if (!isValid) return;

            var svTrung = listSV.FirstOrDefault(x => x.MaSV.ToLower() == txtMaSV.Text.Trim().ToLower());
            if (svTrung != null)
            {
                MessageBox.Show($"Mã '{txtMaSV.Text}' đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            XuLyLuu(true);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaSV.Enabled) { MessageBox.Show("Chọn sinh viên để sửa!"); return; }
            XuLyLuu(false);
        }

        private void XuLyLuu(bool isNew)
        {
            SinhVien sv = isNew ? new SinhVien() : listSV.First(x => x.MaSV == txtMaSV.Text);

            if (!string.IsNullOrEmpty(currentImagePath))
            {
                if (!isNew && !string.IsNullOrEmpty(sv.AnhDaiDien))
                {
                    string oldPath = Path.Combine(Application.StartupPath, "Images", sv.AnhDaiDien);
                    if (File.Exists(oldPath)) try { File.Delete(oldPath); } catch { }
                }
                sv.AnhDaiDien = CopyImageToAppFolder(currentImagePath, txtMaSV.Text);
            }

            sv.MaSV = txtMaSV.Text.Trim();
            sv.HoTen = txtHoTen.Text.Trim();
            sv.Email = txtEmail.Text.Trim();
            sv.SoDienThoai = txtSDT.Text.Trim();
            sv.NgaySinh = dtpNgaySinh.Value.ToString("dd/MM/yyyy");
            sv.GioiTinh = rbNam.Checked ? "Nam" : "Nữ";
            sv.Lop = cbLop.Text;

            if (isNew) listSV.Add(sv);
            DataHelper.LuuSV(listSV);

            HienThiTable(listSV);
            btnLamMoi_Click(null, null);
            MessageBox.Show("Lưu thành công!");
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            var sv = listSV.FirstOrDefault(x => x.MaSV == txtMaSV.Text);
            if (sv != null && MessageBox.Show("Xóa sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                listSV.Remove(sv);
                DataHelper.LuuSV(listSV);
                HienThiTable(listSV);
                btnLamMoi_Click(null, null);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaSV.Clear(); txtHoTen.Clear(); txtEmail.Clear(); txtSDT.Clear();
            cbLop.SelectedIndex = -1; rbNam.Checked = true;
            if (pbAnhDaiDien.Image != null) pbAnhDaiDien.Image.Dispose();
            pbAnhDaiDien.Image = null; currentImagePath = "";
            txtMaSV.Enabled = true; errorProvider.Clear();
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            var keyword = txtTimKiem.Text.ToLower().Trim();
            var filtered = listSV.Where(x => x.HoTen.ToLower().Contains(keyword) || x.MaSV.ToLower().Contains(keyword)).ToList();
            HienThiTable(filtered);
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            try
            {
                // Sử dụng 'as SinhVien' an toàn hơn
                var sv = dgvSinhVien.Rows[e.RowIndex].DataBoundItem as SinhVien;
                if (sv == null) return;

                txtMaSV.Text = sv.MaSV;
                txtHoTen.Text = sv.HoTen;
                txtEmail.Text = sv.Email;
                txtSDT.Text = sv.SoDienThoai;
                cbLop.Text = sv.Lop;

                try { dtpNgaySinh.Value = DateTime.ParseExact(sv.NgaySinh, "dd/MM/yyyy", null); } catch { dtpNgaySinh.Value = DateTime.Now; }
                if (sv.GioiTinh == "Nam") rbNam.Checked = true; else rbNu.Checked = true;

                LoadImageToPictureBox(sv.AnhDaiDien);
                txtMaSV.Enabled = false;
                errorProvider.Clear();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private void BtnChonAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pbAnhDaiDien.Image = Image.FromFile(ofd.FileName);
                currentImagePath = ofd.FileName;
            }
        }

        private string CopyImageToAppFolder(string sourcePath, string maSV)
        {
            if (string.IsNullOrEmpty(sourcePath) || !File.Exists(sourcePath)) return "";
            string imagesFolder = Path.Combine(Application.StartupPath, "Images");
            if (!Directory.Exists(imagesFolder)) Directory.CreateDirectory(imagesFolder);
            string newFileName = $"{maSV}_{DateTime.Now.Ticks}{Path.GetExtension(sourcePath)}";
            File.Copy(sourcePath, Path.Combine(imagesFolder, newFileName), true);
            return newFileName;
        }

        private void LoadImageToPictureBox(string fileName)
        {
            if (pbAnhDaiDien.Image != null) pbAnhDaiDien.Image.Dispose();
            pbAnhDaiDien.Image = null; currentImagePath = "";
            if (string.IsNullOrEmpty(fileName)) return;
            string imagePath = Path.Combine(Application.StartupPath, "Images", fileName);
            if (File.Exists(imagePath)) try { using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read)) { pbAnhDaiDien.Image = Image.FromStream(fs); } } catch { }
        }
    }
}