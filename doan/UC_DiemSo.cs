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
            UIHelper.Beautify(this);
            UIHelper.StyleButton(btnLuuDiem, Color.MediumSeaGreen);
            UIHelper.StyleButton(btnXoaDiem, Color.Crimson);

            this.Load += (s, e) => {
                dsDiem = DataHelper.DocDiem();
                cbSinhVien.DataSource = DataHelper.DocSV(); // Load Object SinhVien
                cbSinhVien.DisplayMember = "HoTen";
                cbSinhVien.ValueMember = "MaSV";
                HienThiTable();
            };

            btnLuuDiem.Click += BtnLuuDiem_Click;
            btnXoaDiem.Click += (s, e) => {
                if (dgvDiemSo.CurrentRow != null)
                {
                    dsDiem.RemoveAt(dgvDiemSo.CurrentRow.Index); DataHelper.LuuDiem(dsDiem); HienThiTable();
                }
            };
            dgvDiemSo.CellClick += DgvDiemSo_CellClick;
        }

        private void HienThiTable()
        {
            dgvDiemSo.DataSource = null; dgvDiemSo.DataSource = dsDiem;
            if (dgvDiemSo.Columns.Count > 0)
            {
                dgvDiemSo.Columns["MaSV"].Visible = false; // Ẩn Mã
                dgvDiemSo.Columns["TenSV"].HeaderText = "Sinh Viên";
                dgvDiemSo.Columns["Mon"].HeaderText = "Môn Học";
                dgvDiemSo.Columns["Diem"].HeaderText = "Điểm";
            }
        }

        private void BtnLuuDiem_Click(object sender, EventArgs e)
        {
            if (cbSinhVien.SelectedItem == null || string.IsNullOrEmpty(cbMonHoc.Text)) return;
            if (!double.TryParse(txtDiem.Text, out double diem) || diem < 0 || diem > 10)
            {
                MessageBox.Show("Điểm không hợp lệ (0-10)!"); return;
            }

            var sv = cbSinhVien.SelectedItem as SinhVien;
            // Kiểm tra trùng môn
            var exist = dsDiem.FirstOrDefault(x => x.MaSV == sv.MaSV && x.Mon == cbMonHoc.Text);
            if (exist != null)
            {
                if (MessageBox.Show("Môn này đã có điểm. Cập nhật lại?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes) exist.Diem = diem;
            }
            else
            {
                dsDiem.Add(new DiemModel { MaSV = sv.MaSV, TenSV = sv.HoTen, Mon = cbMonHoc.Text, Diem = diem });
            }
            DataHelper.LuuDiem(dsDiem); HienThiTable(); MessageBox.Show("Lưu thành công!");
        }

        private void DgvDiemSo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvDiemSo.Rows[e.RowIndex].DataBoundItem as DiemModel;
                if (row == null) return;
                cbMonHoc.Text = row.Mon; txtDiem.Text = row.Diem.ToString();
                // Chọn lại Combobox đúng sinh viên
                foreach (SinhVien item in cbSinhVien.Items) if (item.MaSV == row.MaSV) cbSinhVien.SelectedItem = item;
            }
        }
    }
}