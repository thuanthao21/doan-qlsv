namespace doan
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            PanelMenu = new Panel();
            btnDiemSo = new Button();
            btnLopHoc = new Button();
            btnSinhVien = new Button();
            PanelHeader = new Panel();
            label1 = new Label();
            panelMain = new Panel();
            PanelMenu.SuspendLayout();
            PanelHeader.SuspendLayout();
            SuspendLayout();
            // 
            // PanelMenu
            // 
            PanelMenu.BackColor = Color.FromArgb(0, 0, 0, 50);
            PanelMenu.Controls.Add(btnDiemSo);
            PanelMenu.Controls.Add(btnLopHoc);
            PanelMenu.Controls.Add(btnSinhVien);
            PanelMenu.Dock = DockStyle.Left;
            PanelMenu.Location = new Point(0, 0);
            PanelMenu.Name = "PanelMenu";
            PanelMenu.Size = new Size(200, 603);
            PanelMenu.TabIndex = 0;
            // 
            // btnDiemSo
            // 
            btnDiemSo.BackColor = Color.RoyalBlue;
            btnDiemSo.Dock = DockStyle.Top;
            btnDiemSo.FlatAppearance.BorderSize = 0;
            btnDiemSo.FlatAppearance.MouseDownBackColor = SystemColors.HotTrack;
            btnDiemSo.FlatAppearance.MouseOverBackColor = Color.CornflowerBlue;
            btnDiemSo.FlatStyle = FlatStyle.Flat;
            btnDiemSo.ForeColor = Color.White;
            btnDiemSo.Location = new Point(0, 78);
            btnDiemSo.Name = "btnDiemSo";
            btnDiemSo.Size = new Size(200, 39);
            btnDiemSo.TabIndex = 2;
            btnDiemSo.Text = "Điểm Số";
            btnDiemSo.TextAlign = ContentAlignment.MiddleLeft;
            btnDiemSo.UseVisualStyleBackColor = false;
            btnDiemSo.Click += btnDiemSo_Click;
            // 
            // btnLopHoc
            // 
            btnLopHoc.BackColor = Color.RoyalBlue;
            btnLopHoc.Dock = DockStyle.Top;
            btnLopHoc.FlatAppearance.BorderSize = 0;
            btnLopHoc.FlatAppearance.MouseDownBackColor = SystemColors.HotTrack;
            btnLopHoc.FlatAppearance.MouseOverBackColor = Color.CornflowerBlue;
            btnLopHoc.FlatStyle = FlatStyle.Flat;
            btnLopHoc.ForeColor = Color.White;
            btnLopHoc.Location = new Point(0, 39);
            btnLopHoc.Name = "btnLopHoc";
            btnLopHoc.Size = new Size(200, 39);
            btnLopHoc.TabIndex = 1;
            btnLopHoc.Text = "Lớp Học";
            btnLopHoc.TextAlign = ContentAlignment.MiddleLeft;
            btnLopHoc.UseVisualStyleBackColor = false;
            btnLopHoc.Click += btnLopHoc_Click;
            // 
            // btnSinhVien
            // 
            btnSinhVien.BackColor = Color.RoyalBlue;
            btnSinhVien.Dock = DockStyle.Top;
            btnSinhVien.FlatAppearance.BorderSize = 0;
            btnSinhVien.FlatAppearance.MouseDownBackColor = SystemColors.HotTrack;
            btnSinhVien.FlatAppearance.MouseOverBackColor = Color.CornflowerBlue;
            btnSinhVien.FlatStyle = FlatStyle.Flat;
            btnSinhVien.ForeColor = Color.White;
            btnSinhVien.Location = new Point(0, 0);
            btnSinhVien.Name = "btnSinhVien";
            btnSinhVien.Size = new Size(200, 39);
            btnSinhVien.TabIndex = 0;
            btnSinhVien.Text = "Sinh Viên";
            btnSinhVien.TextAlign = ContentAlignment.MiddleLeft;
            btnSinhVien.UseVisualStyleBackColor = false;
            btnSinhVien.Click += btnSinhVien_Click;
            // 
            // PanelHeader
            // 
            PanelHeader.BackColor = SystemColors.ButtonFace;
            PanelHeader.Controls.Add(label1);
            PanelHeader.Dock = DockStyle.Top;
            PanelHeader.Location = new Point(200, 0);
            PanelHeader.Name = "PanelHeader";
            PanelHeader.Size = new Size(882, 60);
            PanelHeader.TabIndex = 1;
            PanelHeader.Tag = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.FlatStyle = FlatStyle.Flat;
            label1.Font = new Font("Segoe UI", 22.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(211, 7);
            label1.Name = "label1";
            label1.Size = new Size(188, 50);
            label1.TabIndex = 0;
            label1.Text = "Trang Chủ";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelMain
            // 
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(200, 60);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(882, 543);
            panelMain.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveBorder;
            ClientSize = new Size(1082, 603);
            Controls.Add(panelMain);
            Controls.Add(PanelHeader);
            Controls.Add(PanelMenu);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Quản lý Sinh viên";
            Load += Form1_Load;
            PanelMenu.ResumeLayout(false);
            PanelHeader.ResumeLayout(false);
            PanelHeader.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel PanelMenu;
        private Panel PanelHeader;
        private Panel panelMain;
        private Button btnSinhVien;
        private Button btnDiemSo;
        private Button btnLopHoc;
        private Label label1;
    }
}
