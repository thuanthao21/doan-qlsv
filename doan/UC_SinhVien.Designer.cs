        namespace doan
        {
            partial class UC_SinhVien
            {
                /// <summary> 
                /// Required designer variable.
                /// </summary>
                private System.ComponentModel.IContainer components = null;

                /// <summary> 
                /// Clean up any resources being used.
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

                #region Component Designer generated code

                /// <summary> 
                /// Required method for Designer support - do not modify 
                /// the contents of this method with the code editor.
                /// </summary>
                private void InitializeComponent()
                {
                    DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                    DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
                    groupBox1 = new GroupBox();
                    btnLamMoi = new Button();
                    btnXoa = new Button();
                    btnSua = new Button();
                    btnThem = new Button();
                    label8 = new Label();
                    cbLop = new ComboBox();
                    label6 = new Label();
                    rbNu = new RadioButton();
                    rbNam = new RadioButton();
                    label5 = new Label();
                    dtpNgaySinh = new DateTimePicker();
                    txtSDT = new TextBox();
                    label4 = new Label();
                    txtEmail = new TextBox();
                    label3 = new Label();
                    txtHoTen = new TextBox();
                    label2 = new Label();
                    txtMaSV = new TextBox();
                    label1 = new Label();
                    dgvSinhVien = new DataGridView();
                    txtTimKiem = new TextBox();
                    label7 = new Label();
                    groupBox1.SuspendLayout();
                    ((System.ComponentModel.ISupportInitialize)dgvSinhVien).BeginInit();
                    SuspendLayout();
                    // 
                    // groupBox1
                    // 
                    groupBox1.Controls.Add(label7);
                    groupBox1.Controls.Add(txtTimKiem);
                    groupBox1.Controls.Add(btnLamMoi);
                    groupBox1.Controls.Add(btnXoa);
                    groupBox1.Controls.Add(btnSua);
                    groupBox1.Controls.Add(btnThem);
                    groupBox1.Controls.Add(label8);
                    groupBox1.Controls.Add(cbLop);
                    groupBox1.Controls.Add(label6);
                    groupBox1.Controls.Add(rbNu);
                    groupBox1.Controls.Add(rbNam);
                    groupBox1.Controls.Add(label5);
                    groupBox1.Controls.Add(dtpNgaySinh);
                    groupBox1.Controls.Add(txtSDT);
                    groupBox1.Controls.Add(label4);
                    groupBox1.Controls.Add(txtEmail);
                    groupBox1.Controls.Add(label3);
                    groupBox1.Controls.Add(txtHoTen);
                    groupBox1.Controls.Add(label2);
                    groupBox1.Controls.Add(txtMaSV);
                    groupBox1.Controls.Add(label1);
                    groupBox1.Dock = DockStyle.Top;
                    groupBox1.Location = new Point(0, 0);
                    groupBox1.Name = "groupBox1";
                    groupBox1.Padding = new Padding(20, 3, 3, 3);
                    groupBox1.Size = new Size(576, 438);
                    groupBox1.TabIndex = 1;
                    groupBox1.TabStop = false;
                    groupBox1.Text = "Thông tin chi tiết sinh viên";
                    // 
                    // btnLamMoi
                    // 
                    btnLamMoi.BackColor = Color.DodgerBlue;
                    btnLamMoi.FlatStyle = FlatStyle.Flat;
                    btnLamMoi.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    btnLamMoi.ForeColor = Color.White;
                    btnLamMoi.Location = new Point(434, 400);
                    btnLamMoi.Name = "btnLamMoi";
                    btnLamMoi.Size = new Size(94, 29);
                    btnLamMoi.TabIndex = 20;
                    btnLamMoi.Text = "Làm mới";
                    btnLamMoi.UseVisualStyleBackColor = false;
                   
                    // 
                    // btnXoa
                    // 
                    btnXoa.BackColor = Color.Crimson;
                    btnXoa.FlatStyle = FlatStyle.Flat;
                    btnXoa.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    btnXoa.ForeColor = Color.White;
                    btnXoa.Location = new Point(334, 400);
                    btnXoa.Name = "btnXoa";
                    btnXoa.Size = new Size(94, 29);
                    btnXoa.TabIndex = 19;
                    btnXoa.Text = "Xóa";
                    btnXoa.UseVisualStyleBackColor = false;
                    btnXoa.Click += btnXoa_Click;
                    // 
                    // btnSua
                    // 
                    btnSua.BackColor = Color.Orange;
                    btnSua.FlatStyle = FlatStyle.Flat;
                    btnSua.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    btnSua.ForeColor = Color.White;
                    btnSua.Location = new Point(234, 400);
                    btnSua.Name = "btnSua";
                    btnSua.Size = new Size(94, 29);
                    btnSua.TabIndex = 18;
                    btnSua.Text = "Sửa";
                    btnSua.UseVisualStyleBackColor = false;
                    // 
                    // btnThem
                    // 
                    btnThem.BackColor = Color.MediumSeaGreen;
                    btnThem.FlatStyle = FlatStyle.Flat;
                    btnThem.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    btnThem.ForeColor = Color.White;
                    btnThem.Location = new Point(133, 400);
                    btnThem.Name = "btnThem";
                    btnThem.Size = new Size(94, 29);
                    btnThem.TabIndex = 17;
                    btnThem.Text = "Thêm";
                    btnThem.UseVisualStyleBackColor = false;
                    btnThem.Click += btnThem_Click;
                    // 
                    // label8
                    // 
                    label8.AutoSize = true;
                    label8.Location = new Point(28, 352);
                    label8.Name = "label8";
                    label8.Size = new Size(62, 20);
                    label8.TabIndex = 16;
                    label8.Text = "Lớp học";
                    // 
                    // cbLop
                    // 
                    cbLop.FormattingEnabled = true;
                    cbLop.Location = new Point(131, 349);
                    cbLop.Name = "cbLop";
                    cbLop.Size = new Size(151, 28);
                    cbLop.TabIndex = 15;
                    // 
                    // label6
                    // 
                    label6.AutoSize = true;
                    label6.Location = new Point(25, 283);
                    label6.Name = "label6";
                    label6.Size = new Size(65, 20);
                    label6.TabIndex = 13;
                    label6.Text = "Giới tính";
                    // 
                    // rbNu
                    // 
                    rbNu.AutoSize = true;
                    rbNu.Location = new Point(145, 309);
                    rbNu.Name = "rbNu";
                    rbNu.Size = new Size(47, 24);
                    rbNu.TabIndex = 11;
                    rbNu.TabStop = true;
                    rbNu.Text = "nữ";
                    rbNu.UseVisualStyleBackColor = true;
                    // 
                    // rbNam
                    // 
                    rbNam.AutoSize = true;
                    rbNam.Location = new Point(145, 279);
                    rbNam.Name = "rbNam";
                    rbNam.Size = new Size(59, 24);
                    rbNam.TabIndex = 10;
                    rbNam.TabStop = true;
                    rbNam.Text = "nam";
                    rbNam.UseVisualStyleBackColor = true;
                    // 
                    // label5
                    // 
                    label5.AutoSize = true;
                    label5.Location = new Point(25, 251);
                    label5.Name = "label5";
                    label5.Size = new Size(74, 20);
                    label5.TabIndex = 9;
                    label5.Text = "Ngày sinh";
                    // 
                    // dtpNgaySinh
                    // 
                    dtpNgaySinh.Location = new Point(128, 246);
                    dtpNgaySinh.Name = "dtpNgaySinh";
                    dtpNgaySinh.Size = new Size(250, 27);
                    dtpNgaySinh.TabIndex = 8;
                    // 
                    // txtSDT
                    // 
                    txtSDT.Location = new Point(128, 202);
                    txtSDT.Name = "txtSDT";
                    txtSDT.Size = new Size(414, 27);
                    txtSDT.TabIndex = 7;
                    // 
                    // label4
                    // 
                    label4.AutoSize = true;
                    label4.Location = new Point(25, 205);
                    label4.Name = "label4";
                    label4.Size = new Size(97, 20);
                    label4.TabIndex = 6;
                    label4.Text = "Số điện thoại";
                    // 
                    // txtEmail
                    // 
                    txtEmail.Location = new Point(128, 154);
                    txtEmail.Name = "txtEmail";
                    txtEmail.Size = new Size(414, 27);
                    txtEmail.TabIndex = 5;
                    // 
                    // label3
                    // 
                    label3.AutoSize = true;
                    label3.Location = new Point(25, 157);
                    label3.Name = "label3";
                    label3.Size = new Size(46, 20);
                    label3.TabIndex = 4;
                    label3.Text = "Email";
                    // 
                    // txtHoTen
                    // 
                    txtHoTen.Location = new Point(128, 108);
                    txtHoTen.Name = "txtHoTen";
                    txtHoTen.Size = new Size(414, 27);
                    txtHoTen.TabIndex = 3;
                    // 
                    // label2
                    // 
                    label2.AutoSize = true;
                    label2.Location = new Point(25, 111);
                    label2.Name = "label2";
                    label2.Size = new Size(54, 20);
                    label2.TabIndex = 2;
                    label2.Text = "Họ tên";
                    // 
                    // txtMaSV
                    // 
                    txtMaSV.Location = new Point(128, 63);
                    txtMaSV.Name = "txtMaSV";
                    txtMaSV.Size = new Size(414, 27);
                    txtMaSV.TabIndex = 1;
                    // 
                    // label1
                    // 
                    label1.AutoSize = true;
                    label1.Location = new Point(25, 66);
                    label1.Name = "label1";
                    label1.Size = new Size(91, 20);
                    label1.TabIndex = 0;
                    label1.Text = "Mã sinh viên";
                    // 
                    // dgvSinhVien
                    // 
                    dataGridViewCellStyle1.BackColor = Color.FromArgb(224, 224, 224);
                    dgvSinhVien.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
                    dgvSinhVien.BackgroundColor = Color.White;
                    dgvSinhVien.BorderStyle = BorderStyle.None;
                    dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dataGridViewCellStyle2.BackColor = Color.RoyalBlue;
                    dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
                    dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
                    dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
                    dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
                    dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
                    dgvSinhVien.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
                    dgvSinhVien.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                    dgvSinhVien.Dock = DockStyle.Fill;
                    dgvSinhVien.Location = new Point(0, 438);
                    dgvSinhVien.Name = "dgvSinhVien";
                    dgvSinhVien.ReadOnly = true;
                    dgvSinhVien.RowHeadersWidth = 51;
                    dgvSinhVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dgvSinhVien.Size = new Size(576, 312);
                    dgvSinhVien.TabIndex = 21;
                    
                    // 
                    // txtTimKiem
                    // 
                    txtTimKiem.Location = new Point(186, 26);
                    txtTimKiem.Name = "txtTimKiem";
                    txtTimKiem.Size = new Size(208, 27);
                    txtTimKiem.TabIndex = 21;
                    // 
                    // label7
                    // 
                    label7.AutoSize = true;
                    label7.Location = new Point(88, 29);
                    label7.Name = "label7";
                    label7.Size = new Size(72, 20);
                    label7.TabIndex = 22;
                    label7.Text = "TÌM KIẾM";
                    // 
                    // UC_SinhVien
                    // 
                    AutoScaleDimensions = new SizeF(8F, 20F);
                    AutoScaleMode = AutoScaleMode.Font;
                    Controls.Add(dgvSinhVien);
                    Controls.Add(groupBox1);
                    Name = "UC_SinhVien";
                    Size = new Size(576, 750);
                    groupBox1.ResumeLayout(false);
                    groupBox1.PerformLayout();
                    ((System.ComponentModel.ISupportInitialize)dgvSinhVien).EndInit();
                    ResumeLayout(false);
                }

                #endregion

                private GroupBox groupBox1;
                private TextBox txtMaSV;
                private Label label1;
                private RadioButton rbNam;
                private Label label5;
                private DateTimePicker dtpNgaySinh;
                private TextBox txtSDT;
                private Label label4;
                private TextBox txtEmail;
                private Label label3;
                private TextBox txtHoTen;
                private Label label2;
                private RadioButton rbNu;
                private Button btnLamMoi;
                private Button btnXoa;
                private Button btnSua;
                private Button btnThem;
                private Label label8;
                private ComboBox cbLop;
                private Label label6;
                private DataGridView dgvSinhVien;
                private Label label7;
                private TextBox txtTimKiem;
            }
        }
