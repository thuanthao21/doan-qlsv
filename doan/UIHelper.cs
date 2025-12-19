using System;
using System.Drawing;
using System.Windows.Forms;

namespace doan
{
    public static class UIHelper
    {
        // BẢNG MÀU HIỆN ĐẠI (Modern Palette)
        public static Color PrimaryColor = Color.FromArgb(0, 122, 204);      // Xanh dương chủ đạo (VS Code style)
        public static Color DarkBlue = Color.FromArgb(45, 45, 48);           // Xanh đen đậm (Sidebar, Header)
        public static Color LightGray = Color.FromArgb(240, 242, 245);       // Xám nhạt (Nền chính)
        public static Color White = Color.White;                               // Trắng (Nền bảng, Input)
        public static Color TextColor = Color.FromArgb(64, 64, 64);          // Màu chữ chính

        // MÀU NÚT CHỨC NĂNG (Tươi sáng hơn)
        public static Color SuccessColor = Color.FromArgb(40, 167, 69);      // Xanh lá (Thêm/Lưu)
        public static Color WarningColor = Color.FromArgb(255, 193, 7);      // Vàng cam (Sửa)
        public static Color DangerColor = Color.FromArgb(220, 53, 69);       // Đỏ (Xóa)
        public static Color InfoColor = Color.FromArgb(23, 162, 184);        // Xanh lơ (Làm mới)

        // FONT CHỮ CHUẨN
        public static Font RegularFont = new Font("Segoe UI", 10F, FontStyle.Regular);
        public static Font BoldFont = new Font("Segoe UI", 10F, FontStyle.Bold);
        public static Font HeaderFont = new Font("Segoe UI", 14F, FontStyle.Bold); // Cho tiêu đề lớn

        // 1. Làm đẹp Nút bấm (Flat & Bo góc nhẹ)
        public static void StyleButton(Button btn, Color bgColor)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = bgColor;
            btn.ForeColor = White;
            btn.Font = BoldFont;
            btn.Cursor = Cursors.Hand;
            btn.Size = new Size(110, 38); // Kích thước chuẩn
        }

        // 2. Làm đẹp Bảng (DataGridView - Sạch sẽ, dễ đọc)
        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BorderStyle = BorderStyle.None;
            dgv.BackgroundColor = White;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(230, 230, 230); // Màu đường kẻ nhạt

            // Header
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = DarkBlue; // Header màu tối
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = White;
            dgv.ColumnHeadersDefaultCellStyle.Font = BoldFont;
            dgv.ColumnHeadersHeight = 45;

            // Rows
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250); // Màu xen kẽ rất nhạt
            dgv.DefaultCellStyle.BackColor = White;
            dgv.DefaultCellStyle.ForeColor = TextColor;
            dgv.DefaultCellStyle.Font = RegularFont;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(204, 232, 255); // Màu chọn xanh nhạt dịu mắt
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        // 3. Hàm tự động áp dụng Style cho toàn bộ Form/UC
        public static void Beautify(Control parent)
        {
            parent.BackColor = LightGray; // Nền chung màu xám nhạt
            foreach (Control c in parent.Controls)
            {
                if (c is Label) { c.Font = RegularFont; c.ForeColor = TextColor; }

                // Style cho các ô nhập liệu (Input)
                if (c is TextBox || c is ComboBox || c is DateTimePicker)
                {
                    c.Font = RegularFont;
                    c.BackColor = White;
                }

                if (c is DataGridView) StyleDataGridView((DataGridView)c);

                if (c is GroupBox)
                {
                    c.Font = BoldFont;
                    c.ForeColor = DarkBlue;
                    c.BackColor = LightGray; // Nền GroupBox cùng màu nền chính
                    Beautify(c); // Đệ quy vào bên trong
                }

                if (c is Panel) Beautify(c);
            }
        }
    }
}