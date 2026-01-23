using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace doan
{
    public partial class UC_SinhVien : UserControl
    {
        private List<SinhVien> listSV = new List<SinhVien>();
        private PictureBox pbAnhDaiDien;
        private Button btnChonAnh;
        private string currentImagePath = "";
        private ErrorProvider errorProvider = new ErrorProvider();

        public UC_SinhVien()
        {
            InitializeComponent();
            SetupControls();

            ApplyModernLayout();
            InitTableColumns();
            LoadData();
            RegisterEvents();
        }

        // --- 1. SETUP GIAO DIỆN ---
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

            if (groupBox1.Controls.Contains(btnThem)) groupBox1.Controls.Remove(btnThem);
            if (groupBox1.Controls.Contains(btnSua)) groupBox1.Controls.Remove(btnSua);
            if (groupBox1.Controls.Contains(btnXoa)) groupBox1.Controls.Remove(btnXoa);
            if (groupBox1.Controls.Contains(btnLamMoi)) groupBox1.Controls.Remove(btnLamMoi);

            this.Controls.Add(btnThem);
            this.Controls.Add(btnSua);
            this.Controls.Add(btnXoa);
            this.Controls.Add(btnLamMoi);

            btnThem.BringToFront(); btnSua.BringToFront(); btnXoa.BringToFront(); btnLamMoi.BringToFront();

            UIHelper.StyleButton(btnThem, UIHelper.SuccessColor);
            UIHelper.StyleButton(btnSua, UIHelper.WarningColor);
            UIHelper.StyleButton(btnXoa, UIHelper.DangerColor);
            UIHelper.StyleButton(btnLamMoi, UIHelper.InfoColor);
            UIHelper.StyleButton(btnChonAnh, UIHelper.PrimaryColor); btnChonAnh.Width = 120;

            groupBox1.Text = "THÔNG TIN SINH VIÊN (CSDL SQL SERVER)";
            groupBox1.Dock = DockStyle.Top;
            groupBox1.Height = 360;
            groupBox1.BackColor = UIHelper.White;

            int col1_L = 30, col1_I = 150;
            int col2_L = 450, col2_I = 550;
            int rowStart = 40, rowGap = 45, inputHeight = 30;

            label1.Location = new Point(col1_L, rowStart);
            txtMaSV.Location = new Point(col1_I, rowStart - 5); txtMaSV.Size = new Size(200, inputHeight);
            label7.Location = new Point(col2_L, rowStart);
            txtTimKiem.Location = new Point(col2_I, rowStart - 5); txtTimKiem.Size = new Size(250, inputHeight);
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

            int btnY = groupBox1.Height + 15;
            int btnStartX = (this.Width - (4 * 120 + 3 * 20)) / 2;
            btnThem.Location = new Point(btnStartX, btnY);
            btnSua.Location = new Point(btnThem.Right + 20, btnY);
            btnXoa.Location = new Point(btnSua.Right + 20, btnY);
            btnLamMoi.Location = new Point(btnXoa.Right + 20, btnY);

            dgvSinhVien.Dock = DockStyle.None;
            dgvSinhVien.Location = new Point(0, btnY + 50);
            dgvSinhVien.Size = new Size(this.Width, this.Height - dgvSinhVien.Top);
            dgvSinhVien.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }

        // --- 2. XỬ LÝ DỮ LIỆU SQL SERVER ---
        private void InitTableColumns()
        {
            dgvSinhVien.AutoGenerateColumns = false;
            dgvSinhVien.Columns.Clear();

            DataGridViewImageColumn imgCol = new DataGridViewImageColumn
            {
                Name = "ColAnhThat",
                HeaderText = "Ảnh",
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                Width = 80
            };
            dgvSinhVien.Columns.Add(imgCol);

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
            dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                DataPropertyName = property,
                Width = width
            });
        }

        private void LoadData()
        {
            // Gọi hàm từ DataHelper kết nối ADO.NET
            listSV = DataHelper.DocSV();
            HienThiTable(listSV);

            
            var dsLop = DataHelper.DocLop();
            cbLop.DataSource = dsLop;
            cbLop.DisplayMember = "TenLop"; // Hiển thị tên (c#)
            cbLop.ValueMember = "MaLop";   // Lấy giá trị mã (123)
            cbLop.SelectedIndex = -1;      // Mặc định không chọn gì
        }

        private void HienThiTable(List<SinhVien> source)
        {
            var bindingSource = new BindingSource { DataSource = source };
            dgvSinhVien.DataSource = bindingSource;

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
                            using (var temp = new Bitmap(path)) { row.Cells["ColAnhThat"].Value = new Bitmap(temp); }
                        }
                        catch { }
                    }
                }
            }
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

        // --- 3. CÁC HÀM THỰC THI SQL (ADO.NET) ---
        private void btnThem_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            if (string.IsNullOrWhiteSpace(txtMaSV.Text)) { errorProvider.SetError(txtMaSV, "Nhập mã!"); return; }
            if (listSV.Any(x => x.MaSV.ToLower() == txtMaSV.Text.Trim().ToLower()))
            {
                MessageBox.Show("Mã này đã tồn tại!"); return;
            }
            XuLyLuuSQL(true);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaSV.Enabled) { MessageBox.Show("Hãy chọn SV dưới bảng!"); return; }
            XuLyLuuSQL(false);
        }

        private void XuLyLuuSQL(bool isNew)
        {
            // 1. Lấy đối tượng SinhVien cần thao tác
            SinhVien sv = isNew ? new SinhVien() : listSV.First(x => x.MaSV == txtMaSV.Text);

            // 2. Xử lý ảnh
            if (!string.IsNullOrEmpty(currentImagePath))
            {
                sv.AnhDaiDien = CopyImageToAppFolder(currentImagePath, txtMaSV.Text);
            }

            // 3. Gán dữ liệu từ giao diện vào đối tượng
            sv.MaSV = txtMaSV.Text.Trim();
            sv.HoTen = txtHoTen.Text.Trim();
            sv.Email = txtEmail.Text.Trim();
            sv.SoDienThoai = txtSDT.Text.Trim();
            sv.NgaySinh = dtpNgaySinh.Value.ToString("dd/MM/yyyy");
            sv.GioiTinh = rbNam.Checked ? "Nam" : "Nữ";

            // LẤY MÃ LỚP (ValueMember) ĐỂ LƯU VÀO DATABASE
            sv.Lop = cbLop.SelectedValue != null ? cbLop.SelectedValue.ToString() : "";

            // --- KHAI BÁO BIẾN SQL TẠI ĐÂY ---
            string sql = "";

            // 4. Thiết lập tham số cho ADO.NET
            SqlParameter[] paras = {
        new SqlParameter("@ma", sv.MaSV),
        new SqlParameter("@ten", sv.HoTen),
        new SqlParameter("@mail", sv.Email),
        new SqlParameter("@sdt", sv.SoDienThoai),
        new SqlParameter("@ns", sv.NgaySinh),
        new SqlParameter("@gt", sv.GioiTinh),
        new SqlParameter("@lop", sv.Lop), // Lưu mã '123' thay vì tên 'c#'
        new SqlParameter("@anh", (object)sv.AnhDaiDien ?? DBNull.Value)
    };

            // 5. Xác định câu lệnh truy vấn
            if (isNew)
                sql = "INSERT INTO SinhVien (MaSV, HoTen, Email, SoDienThoai, NgaySinh, GioiTinh, MaLop, AnhDaiDien) VALUES (@ma, @ten, @mail, @sdt, @ns, @gt, @lop, @anh)";
            else
                sql = "UPDATE SinhVien SET HoTen=@ten, Email=@mail, SoDienThoai=@sdt, NgaySinh=@ns, GioiTinh=@gt, MaLop=@lop, AnhDaiDien=@anh WHERE MaSV=@ma";

            // 6. Thực thi
            if (DataHelper.ThựcThi(sql, paras))
            {
                MessageBox.Show("Lưu vào SQL Server thành công!");
                LoadData();
                btnLamMoi_Click(null, null);
            }
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaSV.Enabled) return;
            if (MessageBox.Show("Xóa sinh viên này khỏi Database?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string sql = "DELETE FROM SinhVien WHERE MaSV = @ma";
                SqlParameter[] paras = { new SqlParameter("@ma", txtMaSV.Text) };
                if (DataHelper.ThựcThi(sql, paras))
                {
                    LoadData();
                    btnLamMoi_Click(null, null);
                }
            }
        }

        // --- CÁC HÀM HỖ TRỢ GIỮ NGUYÊN ---
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
            var sv = dgvSinhVien.Rows[e.RowIndex].DataBoundItem as SinhVien;
            if (sv == null) return;

            txtMaSV.Text = sv.MaSV;
            txtHoTen.Text = sv.HoTen;
            txtEmail.Text = sv.Email;
            txtSDT.Text = sv.SoDienThoai;
            cbLop.SelectedValue = sv.Lop;
            try { dtpNgaySinh.Value = DateTime.ParseExact(sv.NgaySinh, "dd/MM/yyyy", null); } catch { dtpNgaySinh.Value = DateTime.Now; }
            if (sv.GioiTinh == "Nam") rbNam.Checked = true; else rbNu.Checked = true;
            LoadImageToPictureBox(sv.AnhDaiDien);
            txtMaSV.Enabled = false;
        }

        private void BtnChonAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Image Files|*.jpg;*.jpeg;*.png" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pbAnhDaiDien.Image = Image.FromFile(ofd.FileName);
                currentImagePath = ofd.FileName;
            }
        }

        private string CopyImageToAppFolder(string sourcePath, string maSV)
        {
            string imagesFolder = Path.Combine(Application.StartupPath, "Images");
            if (!Directory.Exists(imagesFolder)) Directory.CreateDirectory(imagesFolder);
            string newFileName = $"{maSV}_{DateTime.Now.Ticks}{Path.GetExtension(sourcePath)}";
            File.Copy(sourcePath, Path.Combine(imagesFolder, newFileName), true);
            return newFileName;
        }

        private void LoadImageToPictureBox(string fileName)
        {
            if (pbAnhDaiDien.Image != null) pbAnhDaiDien.Image.Dispose();
            pbAnhDaiDien.Image = null;
            if (string.IsNullOrEmpty(fileName)) return;
            string imagePath = Path.Combine(Application.StartupPath, "Images", fileName);
            if (File.Exists(imagePath)) try { using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read)) { pbAnhDaiDien.Image = Image.FromStream(fs); } } catch { }
        }
    }
}