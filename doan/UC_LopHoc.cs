using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace doan
{
    public partial class UC_LopHoc : UserControl
    {
        List<LopHocModel> danhSachLop = new List<LopHocModel>();

        public UC_LopHoc()
        {
            InitializeComponent();
            UIHelper.Beautify(this); // Làm đẹp UI

            UIHelper.StyleButton(btnThem, Color.MediumSeaGreen);
            UIHelper.StyleButton(btnSua, Color.Orange);
            UIHelper.StyleButton(btnXoa, Color.Crimson);
            UIHelper.StyleButton(btnLamMoi, Color.DodgerBlue);

            this.Load += (s, e) => { danhSachLop = DataHelper.DocLop(); HienThiTable(); };
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += (s, e) => LamMoiForm();
            dgvLopHoc.CellClick += DgvLopHoc_CellClick;
        }

        private void HienThiTable()
        {
            dgvLopHoc.DataSource = null; dgvLopHoc.DataSource = danhSachLop;
            if (dgvLopHoc.Columns.Count > 0)
            {
                dgvLopHoc.Columns[0].HeaderText = "Mã Lớp";
                dgvLopHoc.Columns[1].HeaderText = "Tên Lớp";
                dgvLopHoc.Columns[2].HeaderText = "Khoa";
            }
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaLop.Text) || danhSachLop.Any(x => x.MaLop == txtMaLop.Text))
            {
                MessageBox.Show("Mã lớp không hợp lệ hoặc đã tồn tại!", "Lỗi"); return;
            }
            danhSachLop.Add(new LopHocModel { MaLop = txtMaLop.Text, TenLop = txtTenLop.Text, Khoa = cbKhoa.Text });
            DataHelper.LuuLop(danhSachLop); HienThiTable(); LamMoiForm();
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            var item = danhSachLop.FirstOrDefault(x => x.MaLop == txtMaLop.Text);
            if (item != null)
            {
                item.TenLop = txtTenLop.Text; item.Khoa = cbKhoa.Text;
                DataHelper.LuuLop(danhSachLop); HienThiTable(); MessageBox.Show("Đã cập nhật!");
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            var item = danhSachLop.FirstOrDefault(x => x.MaLop == txtMaLop.Text);
            if (item == null) return;

            // RÀNG BUỘC TOÀN VẸN
            if (DataHelper.DocSV().Any(sv => sv.Lop == item.TenLop))
            {
                MessageBox.Show($"Lớp {item.TenLop} đang có sinh viên. Không thể xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show("Xóa lớp học này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                danhSachLop.Remove(item); DataHelper.LuuLop(danhSachLop); HienThiTable(); LamMoiForm();
            }
        }

        private void DgvLopHoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvLopHoc.Rows[e.RowIndex];
                txtMaLop.Text = row.Cells[0].Value?.ToString(); txtTenLop.Text = row.Cells[1].Value?.ToString();
                cbKhoa.Text = row.Cells[2].Value?.ToString(); txtMaLop.Enabled = false;
            }
        }
        private void LamMoiForm() { txtMaLop.Clear(); txtTenLop.Clear(); cbKhoa.SelectedIndex = -1; txtMaLop.Enabled = true; }
    }
}