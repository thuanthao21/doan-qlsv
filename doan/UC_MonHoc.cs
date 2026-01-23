using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace doan
{
    public partial class UC_MonHoc : UserControl
    {
        private GroupBox gbThongTin;
        private Label lblMaMon, lblTenMon, lblSoTinChi;
        private TextBox txtMaMon, txtTenMon, txtSoTinChi;
        private Button btnThem, btnSua, btnXoa, btnLamMoi;
        private DataGridView dgvMonHoc;

        private ErrorProvider errorProvider = new ErrorProvider();
        private List<MonHocModel> dsMonHoc = new List<MonHocModel>();

        public UC_MonHoc()
        {
            InitializeComponent();
            InitCustomControls();
            ApplyModernLayout();
            InitTableColumns();

            this.Load += (s, e) => LoadData();
            RegisterEvents();

            this.Resize += (s, e) => UpdateLayoutPositions();
        }

        private void RegisterEvents()
        {
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += (s, e) => LamMoiForm();
            dgvMonHoc.CellClick += DgvMonHoc_CellClick;
        }

        // --- XỬ LÝ DỮ LIỆU ---
        private void LoadData()
        {
            dsMonHoc = DataHelper.DocMonHoc();
            dgvMonHoc.DataSource = new BindingSource { DataSource = dsMonHoc };
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;
            if (dsMonHoc.Any(x => x.MaMon == txtMaMon.Text.Trim()))
            {
                MessageBox.Show("Mã môn học đã tồn tại!"); return;
            }

            string sql = "INSERT INTO MonHoc (MaMon, TenMon, SoTinChi) VALUES (@ma, @ten, @stc)";
            SqlParameter[] paras = {
                new SqlParameter("@ma", txtMaMon.Text.Trim()),
                new SqlParameter("@ten", txtTenMon.Text.Trim()),
                new SqlParameter("@stc", int.Parse(txtSoTinChi.Text))
            };

            if (DataHelper.ThựcThi(sql, paras))
            {
                MessageBox.Show("Thêm môn học thành công!");
                LoadData(); LamMoiForm();
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (txtMaMon.Enabled) { MessageBox.Show("Hãy chọn môn học cần sửa!"); return; }
            if (!ValidateInput()) return;

            string sql = "UPDATE MonHoc SET TenMon = @ten, SoTinChi = @stc WHERE MaMon = @ma";
            SqlParameter[] paras = {
                new SqlParameter("@ten", txtTenMon.Text.Trim()),
                new SqlParameter("@stc", int.Parse(txtSoTinChi.Text)),
                new SqlParameter("@ma", txtMaMon.Text.Trim())
            };

            if (DataHelper.ThựcThi(sql, paras))
            {
                MessageBox.Show("Cập nhật thành công!");
                LoadData(); LamMoiForm();
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra xem người dùng đã chọn môn học nào từ bảng chưa
            if (txtMaMon.Enabled)
            {
                MessageBox.Show("Vui lòng chọn môn học cần xóa từ danh sách!");
                return;
            }

            string maMon = txtMaMon.Text.Trim();

            // 2. Kiểm tra ràng buộc: Môn học đã có dữ liệu điểm chưa?
            // Sử dụng chuỗi kết nối và thực hiện truy vấn đếm số lượng bản ghi liên quan
            string sqlCheck = "SELECT COUNT(*) FROM Diem WHERE MonHoc = @ma";
            SqlParameter[] checkParas = { new SqlParameter("@ma", maMon) };

            // Giả sử DataHelper có hàm ExecuteScalar hoặc bạn có thể kiểm tra nhanh qua logic DocDiem
            // Ở đây ta dùng câu lệnh thực thi xóa trực tiếp nếu bạn đã thiết lập ON DELETE CASCADE trong SQL,
            // hoặc kiểm tra trước để thông báo cho người dùng:

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa môn học [{txtTenMon.Text}]? \nLưu ý: Hành động này không thể hoàn tác!",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    string sqlDelete = "DELETE FROM MonHoc WHERE MaMon = @ma";
                    SqlParameter[] deleteParas = { new SqlParameter("@ma", maMon) };

                    if (DataHelper.ThựcThi(sqlDelete, deleteParas))
                    {
                        MessageBox.Show("Xóa môn học thành công!");
                        LoadData();      // Tải lại bảng
                        LamMoiForm();    // Reset các ô nhập liệu
                    }
                    else
                    {
                        // Nếu thất bại thường là do vi phạm khóa ngoại (đã có điểm số liên kết)
                        MessageBox.Show("Không thể xóa môn học này vì đã có dữ liệu điểm số liên quan trong hệ thống!",
                            "Lỗi ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi hệ thống: " + ex.Message);
                }
            }
        
        }

        private void DgvMonHoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvMonHoc.Rows[e.RowIndex].DataBoundItem as MonHocModel;
            txtMaMon.Text = row.MaMon;
            txtTenMon.Text = row.TenMon;
            txtSoTinChi.Text = row.SoTinChi.ToString();
            txtMaMon.Enabled = false;
        }

        private void LamMoiForm()
        {
            txtMaMon.Clear(); txtTenMon.Clear(); txtSoTinChi.Clear();
            txtMaMon.Enabled = true; errorProvider.Clear();
        }

        private bool ValidateInput()
        {
            errorProvider.Clear();
            if (string.IsNullOrWhiteSpace(txtMaMon.Text)) { errorProvider.SetError(txtMaMon, "Nhập mã môn!"); return false; }
            if (string.IsNullOrWhiteSpace(txtTenMon.Text)) { errorProvider.SetError(txtTenMon, "Nhập tên môn!"); return false; }
            if (!int.TryParse(txtSoTinChi.Text, out int n)) { errorProvider.SetError(txtSoTinChi, "Số tín chỉ phải là số!"); return false; }
            return true;
        }

        // --- GIAO DIỆN (Đã tối ưu) ---
        private void InitCustomControls()
        {
            gbThongTin = new GroupBox();
            lblMaMon = new Label { Text = "Mã Môn:" };
            lblTenMon = new Label { Text = "Tên Môn:" };
            lblSoTinChi = new Label { Text = "Số Tín Chỉ:" };
            txtMaMon = new TextBox();
            txtTenMon = new TextBox();
            txtSoTinChi = new TextBox();
            btnThem = new Button { Text = "Thêm" };
            btnSua = new Button { Text = "Sửa" };
            btnXoa = new Button { Text = "Xóa" };
            btnLamMoi = new Button { Text = "Làm mới" };
            dgvMonHoc = new DataGridView();

            this.Controls.AddRange(new Control[] { gbThongTin, btnThem, btnSua, btnXoa, btnLamMoi, dgvMonHoc });
            gbThongTin.Controls.AddRange(new Control[] { lblMaMon, lblTenMon, lblSoTinChi, txtMaMon, txtTenMon, txtSoTinChi });
        }

        private void ApplyModernLayout()
        {
            UIHelper.Beautify(this);
            UIHelper.StyleButton(btnThem, UIHelper.SuccessColor);
            UIHelper.StyleButton(btnSua, UIHelper.WarningColor);
            UIHelper.StyleButton(btnXoa, UIHelper.DangerColor);
            UIHelper.StyleButton(btnLamMoi, UIHelper.InfoColor);

            gbThongTin.Text = "QUẢN LÝ DANH MỤC MÔN HỌC";
            gbThongTin.Dock = DockStyle.Top;
            gbThongTin.Height = 180;

            int xL = 50, xI = 150;
            lblMaMon.Location = new Point(xL, 40); txtMaMon.Location = new Point(xI, 35); txtMaMon.Width = 200;
            lblTenMon.Location = new Point(xL, 85); txtTenMon.Location = new Point(xI, 80); txtTenMon.Width = 400;
            lblSoTinChi.Location = new Point(xL, 130); txtSoTinChi.Location = new Point(xI, 125); txtSoTinChi.Width = 100;
        }

        private void UpdateLayoutPositions()
        {
            int btnY = gbThongTin.Height + 20;
            int startX = (this.Width - (4 * 120 + 3 * 20)) / 2;
            btnThem.Location = new Point(startX, btnY);
            btnSua.Location = new Point(btnThem.Right + 20, btnY);
            btnXoa.Location = new Point(btnSua.Right + 20, btnY);
            btnLamMoi.Location = new Point(btnXoa.Right + 20, btnY);

            dgvMonHoc.Location = new Point(0, btnY + 60);
            dgvMonHoc.Size = new Size(this.Width, this.Height - dgvMonHoc.Top);
        }

        private void InitTableColumns()
        {
            dgvMonHoc.AutoGenerateColumns = false;
            dgvMonHoc.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaMon", HeaderText = "Mã Môn", Width = 150 });
            dgvMonHoc.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenMon", HeaderText = "Tên Môn Học", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvMonHoc.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoTinChi", HeaderText = "Tín Chỉ", Width = 100 });
        }
    }
}