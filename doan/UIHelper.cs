using System;
using System.Drawing;
using System.Windows.Forms;

namespace doan
{
    public static class UIHelper
    {
        // Bảng màu Professional (Xanh Navy & Phẳng)
        public static Color PrimaryColor = Color.FromArgb(41, 128, 185);     // Xanh chủ đạo
        public static Color SecondaryColor = Color.FromArgb(52, 73, 94);     // Xanh đen (Sidebar)
        public static Color BackgroundColor = Color.FromArgb(236, 240, 241); // Xám nhạt (Nền)
        public static Color TextColor = Color.FromArgb(44, 62, 80);          // Màu chữ tối

        // Font chữ chuẩn
        public static Font MainFont = new Font("Segoe UI", 10F, FontStyle.Regular);
        public static Font BoldFont = new Font("Segoe UI", 10F, FontStyle.Bold);

        // Hàm làm đẹp Button
        public static void StyleButton(Button btn, Color color)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.Font = BoldFont;
            btn.Cursor = Cursors.Hand;
            btn.Size = new Size(100, 35); // Kích thước chuẩn
        }

        // Hàm làm đẹp DataGridView
        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BorderStyle = BorderStyle.None;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgv.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            dgv.BackgroundColor = Color.White;

            // Header
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = SecondaryColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = BoldFont;
            dgv.ColumnHeadersHeight = 40;

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.RowHeadersVisible = false;
        }

        // Hàm tự động làm đẹp toàn bộ Form/UserControl
        public static void Beautify(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is Button) continue; // Button sẽ style riêng

                if (c is Label) c.Font = MainFont;

                if (c is TextBox || c is ComboBox || c is DateTimePicker)
                {
                    c.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
                    c.BackColor = Color.White;
                }

                if (c is DataGridView) StyleDataGridView((DataGridView)c);

                if (c is GroupBox)
                {
                    c.Font = new Font("Segoe UI", 11F, FontStyle.Bold); // Tiêu đề GroupBox đậm
                    c.ForeColor = SecondaryColor;
                    Beautify(c); // Đệ quy vào bên trong
                }
            }
        }
    }
}