namespace doan
{
    partial class UC_DiemSo
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        private void InitializeComponent()
        {
            this.gbNhapDiem = new System.Windows.Forms.GroupBox();
            this.cbSinhVien = new System.Windows.Forms.ComboBox();
            this.cbMonHoc = new System.Windows.Forms.ComboBox();
            this.txtDiem = new System.Windows.Forms.TextBox();
            this.btnLuuDiem = new System.Windows.Forms.Button();
            this.btnXoaDiem = new System.Windows.Forms.Button();
            this.dgvDiemSo = new System.Windows.Forms.DataGridView();
            this.gbNhapDiem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiemSo)).BeginInit();
            this.SuspendLayout();

            // gbNhapDiem
            this.gbNhapDiem.Controls.Add(this.cbSinhVien);
            this.gbNhapDiem.Controls.Add(this.cbMonHoc);
            this.gbNhapDiem.Controls.Add(this.txtDiem);
            this.gbNhapDiem.Controls.Add(this.btnLuuDiem);
            this.gbNhapDiem.Controls.Add(this.btnXoaDiem);
            this.gbNhapDiem.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbNhapDiem.Height = 160;
            this.gbNhapDiem.Text = "Quản lý điểm số";

            // ComboBoxes & TextBox
            this.cbSinhVien.Location = new System.Drawing.Point(120, 37);
            this.cbSinhVien.Size = new System.Drawing.Size(220, 27);
            this.cbSinhVien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.cbMonHoc.Location = new System.Drawing.Point(120, 77);
            this.cbMonHoc.Size = new System.Drawing.Size(220, 27);
            this.cbMonHoc.Items.AddRange(new object[] { "Lập trình C#", "Cấu trúc dữ liệu", "Cơ sở dữ liệu" });

            this.txtDiem.Location = new System.Drawing.Point(400, 37);
            this.txtDiem.Size = new System.Drawing.Size(100, 27);

            // Buttons
            this.btnLuuDiem.Text = "Lưu điểm";
            this.btnLuuDiem.Location = new System.Drawing.Point(120, 120);
            this.btnLuuDiem.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnLuuDiem.ForeColor = System.Drawing.Color.White;
            this.btnLuuDiem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.btnXoaDiem.Text = "Xóa";
            this.btnXoaDiem.Location = new System.Drawing.Point(220, 120);
            this.btnXoaDiem.BackColor = System.Drawing.Color.Crimson;
            this.btnXoaDiem.ForeColor = System.Drawing.Color.White;
            this.btnXoaDiem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            // DataGridView
            this.dgvDiemSo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDiemSo.Location = new System.Drawing.Point(0, 160);
            this.dgvDiemSo.BackgroundColor = System.Drawing.Color.White;
            this.dgvDiemSo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDiemSo.RowHeadersVisible = false;

            // UC_DiemSo
            this.Controls.Add(this.dgvDiemSo);
            this.Controls.Add(this.gbNhapDiem);
            this.Size = new System.Drawing.Size(882, 655);
            this.gbNhapDiem.ResumeLayout(false);
            this.gbNhapDiem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiemSo)).EndInit();
            this.ResumeLayout(false);
        }
        #endregion

        private System.Windows.Forms.GroupBox gbNhapDiem;
        private System.Windows.Forms.ComboBox cbSinhVien;
        private System.Windows.Forms.ComboBox cbMonHoc;
        private System.Windows.Forms.TextBox txtDiem;
        private System.Windows.Forms.Button btnLuuDiem;
        private System.Windows.Forms.Button btnXoaDiem;
        private System.Windows.Forms.DataGridView dgvDiemSo;
    }
}