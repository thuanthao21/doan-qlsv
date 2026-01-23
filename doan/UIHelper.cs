using System;
using System.Drawing;
using System.Windows.Forms;

namespace doan
{
    public static class UIHelper
    {
        // BẢNG MÀU HIỆN ĐẠI (Modern Palette)
        public static Color PrimaryColor = Color.FromArgb(0, 122, 204);      // Xanh dương chủ đạo
        public static Color DarkBlue = Color.FromArgb(45, 45, 48);           // Xanh đen đậm (Sidebar)
        public static Color LightGray = Color.FromArgb(240, 242, 245);       // Xám nhạt (Nền chính)
        public static Color White = Color.White;                             // Trắng
        public static Color TextColor = Color.FromArgb(64, 64, 64);          // Màu chữ chính

        // MÀU NÚT CHỨC NĂNG
        public static Color SuccessColor = Color.FromArgb(40, 167, 69);      // Xanh lá (Thêm)
        public static Color WarningColor = Color.FromArgb(255, 193, 7);      // Vàng cam (Sửa)
        public static Color DangerColor = Color.FromArgb(220, 53, 69);       // Đỏ (Xóa)
        public static Color InfoColor = Color.FromArgb(23, 162, 184);        // Xanh lơ (Làm mới)

        // FONT CHỮ CHUẨN (Tăng size lên 11 để chữ rõ nét hơn)
        public static Font RegularFont = new Font("Segoe UI", 11F, FontStyle.Regular);
        public static Font BoldFont = new Font("Segoe UI", 11F, FontStyle.Bold);
        public static Font HeaderFont = new Font("Segoe UI", 16F, FontStyle.Bold);

        // 1. Làm đẹp Nút bấm
        public static void StyleButton(Button btn, Color bgColor)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = bgColor;
            btn.ForeColor = White;
            btn.Font = BoldFont;
            btn.Cursor = Cursors.Hand;
            btn.Size = new Size(120, 40); // Tăng nhẹ kích thước cho dễ bấm
        }

        // 2. Làm đẹp Bảng (DataGridView)
        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BorderStyle = BorderStyle.None;
            dgv.BackgroundColor = White;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(230, 230, 230);

            // Header
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = DarkBlue;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = White;
            dgv.ColumnHeadersDefaultCellStyle.Font = BoldFont;
            dgv.ColumnHeadersHeight = 50; // Tăng chiều cao header

            // Rows
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            dgv.DefaultCellStyle.BackColor = White;
            dgv.DefaultCellStyle.ForeColor = TextColor;
            dgv.DefaultCellStyle.Font = RegularFont;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(204, 232, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgv.RowTemplate.Height = 40; // Tăng chiều cao hàng để chữ không bị kẹt
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        // 3. Hàm tự động áp dụng Style
        public static void Beautify(Control parent)
        {
            parent.BackColor = LightGray;
            foreach (Control c in parent.Controls)
            {
                if (c is Label) { c.Font = RegularFont; c.ForeColor = TextColor; }

                // Style cho các ô nhập liệu (Input)
                if (c is TextBox t)
                {
                    t.Font = RegularFont;
                    t.BorderStyle = BorderStyle.FixedSingle; // Viền mỏng hiện đại
                }

                if (c is ComboBox cb)
                {
                    cb.Font = RegularFont;
                    cb.FlatStyle = FlatStyle.Standard;
                }

                if (c is DateTimePicker dtp)
                {
                    dtp.Font = RegularFont;
                }

                if (c is DataGridView dgv) StyleDataGridView(dgv);

                if (c is GroupBox gb)
                {
                    gb.Font = BoldFont;
                    gb.ForeColor = PrimaryColor; // Tiêu đề GroupBox dùng màu chính cho nổi bật
                    gb.BackColor = White; // Cho GroupBox nền trắng để nổi trên nền xám
                    Beautify(gb);
                }

                if (c is Panel p) Beautify(p);
            }
        }
    }
}