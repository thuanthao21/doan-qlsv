using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace doan
{
    public partial class UC_DiemSo : UserControl
    {
        List<DiemModel> dsDiem = new List<DiemModel>();

        public UC_DiemSo()
        {
            InitializeComponent();
            ApplyModernLayout();
            this.Load += (s, e) => { dsDiem = DataHelper.DocDiem(); cbSinhVien.DataSource = DataHelper.DocSV(); cbSinhVien.DisplayMember = "HoTen"; cbSinhVien.ValueMember = "MaSV"; HienThiTable(); };
            RegisterEvents();
        }

        private void ApplyModernLayout()
        {
            UIHelper.Beautify(this);
            UIHelper.StyleButton(btnLuuDiem, UIHelper.SuccessColor);
            UIHelper.StyleButton(btnXoaDiem, UIHelper.DangerColor);

            gbNhapDiem.Text = "NHẬP ĐIỂM SINH VIÊN";
            gbNhapDiem.Height = 200;
            gbNhapDiem.BackColor = UIHelper.White;

            int inputH = 32;
            // Sắp xếp 3 ô nhập trên 1 dòng
            cbSinhVien.Location = new Point(50, 50); cbSinhVien.Size = new Size(300, inputH);
            cbMonHoc.Location = new Point(380, 50); cbMonHoc.Size = new Size(300, inputH);
            txtDiem.Location = new Point(710, 50); txtDiem.Size = new Size(150, inputH); txtDiem.PlaceholderText = "Điểm (0-10)";

            // Nút bấm căn giữa
            int btnY = 120;
            int btnStartX = (this.Width - (2 * 110 + 20)) / 2;
            btnLuuDiem.Location = new Point(btnStartX, btnY);
            btnXoaDiem.Location = new Point(btnLuuDiem.Right + 20, btnY);

            dgvDiemSo.Location = new Point(0, gbNhapDiem.Bottom + 20);
            dgvDiemSo.Height = this.Height - dgvDiemSo.Top - 10;
        }

        private void RegisterEvents()
        {
            btnLuuDiem.Click -= BtnLuuDiem_Click; btnLuuDiem.Click += BtnLuuDiem_Click;
            btnXoaDiem.Click -= BtnXoaDiem_Click; btnXoaDiem.Click += BtnXoaDiem_Click;
            dgvDiemSo.CellClick -= DgvDiemSo_CellClick; dgvDiemSo.CellClick += DgvDiemSo_CellClick;
        }

        // (Logic giữ nguyên)
        private void HienThiTable() { dgvDiemSo.DataSource = null; dgvDiemSo.DataSource = dsDiem; if (dgvDiemSo.Columns.Count > 0) { dgvDiemSo.Columns["MaSV"].Visible = false; dgvDiemSo.Columns["TenSV"].HeaderText = "Sinh Viên"; dgvDiemSo.Columns["Mon"].HeaderText = "Môn Học"; dgvDiemSo.Columns["Diem"].HeaderText = "Điểm"; } }
        private void BtnLuuDiem_Click(object sender, EventArgs e) { if (cbSinhVien.SelectedItem == null || string.IsNullOrEmpty(cbMonHoc.Text)) return; if (!double.TryParse(txtDiem.Text, out double diem) || diem < 0 || diem > 10) { MessageBox.Show("Điểm không hợp lệ (0-10)!"); return; } var sv = cbSinhVien.SelectedItem as SinhVien; var exist = dsDiem.FirstOrDefault(x => x.MaSV == sv.MaSV && x.Mon == cbMonHoc.Text); if (exist != null) { if (MessageBox.Show("Môn này đã có điểm. Cập nhật?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes) exist.Diem = diem; } else { dsDiem.Add(new DiemModel { MaSV = sv.MaSV, TenSV = sv.HoTen, Mon = cbMonHoc.Text, Diem = diem }); } DataHelper.LuuDiem(dsDiem); HienThiTable(); MessageBox.Show("Lưu thành công!"); }
        private void BtnXoaDiem_Click(object sender, EventArgs e) { if (dgvDiemSo.CurrentRow != null) { dsDiem.RemoveAt(dgvDiemSo.CurrentRow.Index); DataHelper.LuuDiem(dsDiem); HienThiTable(); } }
        private void DgvDiemSo_CellClick(object sender, DataGridViewCellEventArgs e) { if (e.RowIndex >= 0) { var row = dgvDiemSo.Rows[e.RowIndex].DataBoundItem as DiemModel; if (row == null) return; cbMonHoc.Text = row.Mon; txtDiem.Text = row.Diem.ToString(); foreach (SinhVien item in cbSinhVien.Items) if (item.MaSV == row.MaSV) cbSinhVien.SelectedItem = item; } }
    }
}