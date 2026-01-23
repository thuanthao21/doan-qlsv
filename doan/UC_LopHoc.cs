using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace doan
{
    public partial class UC_LopHoc : UserControl
    {
        private List<LopHocModel> danhSachLop = new List<LopHocModel>();
        private ErrorProvider errorProvider = new ErrorProvider();

        public UC_LopHoc()
        {
            InitializeComponent();

            ApplyModernLayout();
            InitTableColumns();

            this.Load += (s, e) => {
                LoadData();
                LoadComboBoxKhoa();
            };

            RegisterEvents();
        }

        // --- 1. GIAO DIỆN ---
        private void ApplyModernLayout()
        {
            UIHelper.Beautify(this);

            if (gbThongTin.Controls.Contains(btnThem)) gbThongTin.Controls.Remove(btnThem);
            if (gbThongTin.Controls.Contains(btnSua)) gbThongTin.Controls.Remove(btnSua);
            if (gbThongTin.Controls.Contains(btnXoa)) gbThongTin.Controls.Remove(btnXoa);
            if (gbThongTin.Controls.Contains(btnLamMoi)) gbThongTin.Controls.Remove(btnLamMoi);

            this.Controls.Add(btnThem);
            this.Controls.Add(btnSua);
            this.Controls.Add(btnXoa);
            this.Controls.Add(btnLamMoi);

            btnThem.BringToFront(); btnSua.BringToFront(); btnXoa.BringToFront(); btnLamMoi.BringToFront();

            UIHelper.StyleButton(btnThem, UIHelper.SuccessColor);
            UIHelper.StyleButton(btnSua, UIHelper.WarningColor);
            UIHelper.StyleButton(btnXoa, UIHelper.DangerColor);
            UIHelper.StyleButton(btnLamMoi, UIHelper.InfoColor);

            gbThongTin.Text = "QUẢN LÝ LỚP HỌC";
            gbThongTin.Dock = DockStyle.Top;
            gbThongTin.Height = 200;
            gbThongTin.BackColor = UIHelper.White;

            int col1_L = 50, col1_I = 150;
            int col2_L = 450, col2_I = 550;
            int row1 = 50, row2 = 100;
            int inputH = 30;

            lblMaLop.Location = new Point(col1_L, row1);
            txtMaLop.Location = new Point(col1_I, row1 - 5); txtMaLop.Size = new Size(250, inputH);

            lblKhoa.Location = new Point(col2_L, row1);
            cbKhoa.Location = new Point(col2_I, row1 - 5); cbKhoa.Size = new Size(250, inputH);

            lblTenLop.Location = new Point(col1_L, row2);
            txtTenLop.Location = new Point(col1_I, row2 - 5); txtTenLop.Size = new Size(650, inputH);

            int btnY = gbThongTin.Height + 15;
            int btnStartX = (this.Width - (4 * 110 + 3 * 20)) / 2;

            btnThem.Location = new Point(btnStartX, btnY);
            btnSua.Location = new Point(btnThem.Right + 20, btnY);
            btnXoa.Location = new Point(btnSua.Right + 20, btnY);
            btnLamMoi.Location = new Point(btnXoa.Right + 20, btnY);

            dgvLopHoc.Dock = DockStyle.None;
            dgvLopHoc.Location = new Point(0, btnY + 50);
            dgvLopHoc.Size = new Size(this.Width, this.Height - dgvLopHoc.Top);
            dgvLopHoc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }

        // --- 2. DỮ LIỆU ---
        private void InitTableColumns()
        {
            dgvLopHoc.AutoGenerateColumns = false;
            dgvLopHoc.Columns.Clear();
            AddTextColumn("MaLop", "Mã Lớp", "MaLop", 150);
            AddTextColumn("TenLop", "Tên Lớp", "TenLop", 250);
            AddTextColumn("MaKhoa", "Mã Khoa", "MaKhoa", 200); // Đã đổi thành MaKhoa
        }

        private void AddTextColumn(string name, string header, string prop, int width)
        {
            dgvLopHoc.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                DataPropertyName = prop,
                Width = width
            });
        }

        private void LoadData()
        {
            danhSachLop = DataHelper.DocLop();
            var bindingSource = new BindingSource { DataSource = danhSachLop };
            dgvLopHoc.DataSource = bindingSource;
        }

        private void LoadComboBoxKhoa()
        {
            try
            {
                var dsKhoa = DataHelper.DocKhoa();
                cbKhoa.DataSource = dsKhoa;
                cbKhoa.DisplayMember = "TenKhoa";  // Hiển thị tên khoa
                cbKhoa.ValueMember = "MaKhoa";    // Giá trị thực tế là mã khoa
                cbKhoa.SelectedIndex = -1;
            }
            catch { }
        }

        private void RegisterEvents()
        {
            btnThem.Click -= BtnThem_Click; btnThem.Click += BtnThem_Click;
            btnSua.Click -= BtnSua_Click; btnSua.Click += BtnSua_Click;
            btnXoa.Click -= BtnXoa_Click; btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click -= (s, e) => LamMoiForm();
            dgvLopHoc.CellClick -= DgvLopHoc_CellClick; dgvLopHoc.CellClick += DgvLopHoc_CellClick;
        }

        // --- 3. LOGIC NGHIỆP VỤ (ADO.NET) ---
        private void BtnThem_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            if (string.IsNullOrWhiteSpace(txtMaLop.Text)) { errorProvider.SetError(txtMaLop, "Nhập mã lớp!"); return; }
            if (string.IsNullOrWhiteSpace(txtTenLop.Text)) { errorProvider.SetError(txtTenLop, "Nhập tên lớp!"); return; }
            if (cbKhoa.SelectedValue == null) { errorProvider.SetError(cbKhoa, "Chọn khoa!"); return; }

            if (danhSachLop.Any(x => x.MaLop.ToLower() == txtMaLop.Text.Trim().ToLower()))
            {
                MessageBox.Show("Mã lớp đã tồn tại!", "Lỗi");
                return;
            }

            string sql = "INSERT INTO LopHoc (MaLop, TenLop, MaKhoa) VALUES (@ma, @ten, @khoa)";
            SqlParameter[] paras = {
                new SqlParameter("@ma", txtMaLop.Text.Trim()),
                new SqlParameter("@ten", txtTenLop.Text.Trim()),
                new SqlParameter("@khoa", cbKhoa.SelectedValue.ToString())
            };

            if (DataHelper.ThựcThi(sql, paras))
            {
                MessageBox.Show("Thêm lớp học thành công!");
                LoadData();
                LamMoiForm();
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (txtMaLop.Enabled) { MessageBox.Show("Chọn lớp cần sửa!"); return; }

            string sql = "UPDATE LopHoc SET TenLop = @ten, MaKhoa = @khoa WHERE MaLop = @ma";
            SqlParameter[] paras = {
                new SqlParameter("@ten", txtTenLop.Text.Trim()),
                new SqlParameter("@khoa", cbKhoa.SelectedValue.ToString()),
                new SqlParameter("@ma", txtMaLop.Text.Trim())
            };

            if (DataHelper.ThựcThi(sql, paras))
            {
                MessageBox.Show("Cập nhật thông tin thành công!");
                LoadData();
                LamMoiForm();
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaLop.Enabled) return;

            // KIỂM TRA RÀNG BUỘC: Không cho xóa nếu lớp đang có sinh viên
            var dsSV = DataHelper.DocSV();
            if (dsSV.Any(sv => sv.Lop == txtMaLop.Text)) // Check theo mã lớp
            {
                MessageBox.Show("Lớp này đang có sinh viên học, không thể xóa!", "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show("Xóa lớp học này khỏi Database?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string sql = "DELETE FROM LopHoc WHERE MaLop = @ma";
                SqlParameter[] paras = { new SqlParameter("@ma", txtMaLop.Text.Trim()) };

                if (DataHelper.ThựcThi(sql, paras))
                {
                    LoadData();
                    LamMoiForm();
                }
            }
        }

        private void DgvLopHoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvLopHoc.Rows[e.RowIndex].DataBoundItem as LopHocModel;
                if (row == null) return;

                txtMaLop.Text = row.MaLop;
                txtTenLop.Text = row.TenLop;
                cbKhoa.SelectedValue = row.MaKhoa;

                txtMaLop.Enabled = false;
                errorProvider.Clear();
            }
        }

        private void LamMoiForm()
        {
            txtMaLop.Clear(); txtTenLop.Clear(); cbKhoa.SelectedIndex = -1;
            txtMaLop.Enabled = true; txtMaLop.Focus(); errorProvider.Clear();
        }
    }
}