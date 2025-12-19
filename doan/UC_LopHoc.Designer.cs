namespace doan
{
    partial class UC_LopHoc
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
            this.gbThongTin = new System.Windows.Forms.GroupBox();
            this.lblMaLop = new System.Windows.Forms.Label();
            this.lblTenLop = new System.Windows.Forms.Label();
            this.lblKhoa = new System.Windows.Forms.Label();
            this.txtTenLop = new System.Windows.Forms.TextBox();
            this.txtMaLop = new System.Windows.Forms.TextBox();
            this.cbKhoa = new System.Windows.Forms.ComboBox();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.dgvLopHoc = new System.Windows.Forms.DataGridView();
            this.gbThongTin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLopHoc)).BeginInit();
            this.SuspendLayout();

            // gbThongTin
            this.gbThongTin.Controls.Add(this.lblMaLop);
            this.gbThongTin.Controls.Add(this.lblTenLop);
            this.gbThongTin.Controls.Add(this.lblKhoa);
            this.gbThongTin.Controls.Add(this.txtTenLop);
            this.gbThongTin.Controls.Add(this.txtMaLop);
            this.gbThongTin.Controls.Add(this.cbKhoa);
            this.gbThongTin.Controls.Add(this.btnThem);
            this.gbThongTin.Controls.Add(this.btnSua);
            this.gbThongTin.Controls.Add(this.btnXoa);
            this.gbThongTin.Controls.Add(this.btnLamMoi);
            this.gbThongTin.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbThongTin.Location = new System.Drawing.Point(0, 0);
            this.gbThongTin.Size = new System.Drawing.Size(882, 160);
            this.gbThongTin.Text = "Thông tin lớp học";

            this.lblMaLop.Text = "Mã lớp:"; this.lblMaLop.Location = new System.Drawing.Point(30, 40);
            this.txtMaLop.Location = new System.Drawing.Point(120, 37); this.txtMaLop.Size = new System.Drawing.Size(220, 27);

            this.lblTenLop.Text = "Tên lớp:"; this.lblTenLop.Location = new System.Drawing.Point(30, 80);
            this.txtTenLop.Location = new System.Drawing.Point(120, 77); this.txtTenLop.Size = new System.Drawing.Size(220, 27);

            this.lblKhoa.Text = "Khoa:"; this.lblKhoa.Location = new System.Drawing.Point(400, 40);
            this.cbKhoa.Location = new System.Drawing.Point(480, 37); this.cbKhoa.Size = new System.Drawing.Size(220, 27);
            this.cbKhoa.Items.AddRange(new object[] { "CNTT", "Kinh tế", "Ngoại ngữ" });

            this.btnThem.Text = "Thêm"; this.btnThem.Location = new System.Drawing.Point(120, 120); this.btnThem.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnSua.Text = "Sửa"; this.btnSua.Location = new System.Drawing.Point(220, 120); this.btnSua.BackColor = System.Drawing.Color.Orange;
            this.btnXoa.Text = "Xóa"; this.btnXoa.Location = new System.Drawing.Point(320, 120); this.btnXoa.BackColor = System.Drawing.Color.Crimson;
            this.btnLamMoi.Text = "Làm mới"; this.btnLamMoi.Location = new System.Drawing.Point(420, 120); this.btnLamMoi.BackColor = System.Drawing.Color.DodgerBlue;

            // dgvLopHoc
            this.dgvLopHoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLopHoc.Location = new System.Drawing.Point(0, 160);
            this.dgvLopHoc.BackgroundColor = System.Drawing.Color.White;
            this.dgvLopHoc.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            this.Controls.Add(this.dgvLopHoc);
            this.Controls.Add(this.gbThongTin);
            this.Size = new System.Drawing.Size(882, 655);
            this.gbThongTin.ResumeLayout(false);
            this.gbThongTin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLopHoc)).EndInit();
            this.ResumeLayout(false);
        }
        #endregion

        private System.Windows.Forms.GroupBox gbThongTin;
        private System.Windows.Forms.Label lblMaLop;
        private System.Windows.Forms.Label lblTenLop;
        private System.Windows.Forms.Label lblKhoa;
        private System.Windows.Forms.TextBox txtTenLop;
        private System.Windows.Forms.TextBox txtMaLop;
        private System.Windows.Forms.ComboBox cbKhoa;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.DataGridView dgvLopHoc;
    }
}