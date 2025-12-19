using System;

namespace doan
{
    // Class chứa các hằng số màu sắc để đồng bộ giao diện toàn app
    public static class AppTheme
    {
        public static System.Drawing.Color PrimaryColor = System.Drawing.Color.FromArgb(51, 153, 255); // Xanh dương hiện đại
        public static System.Drawing.Color SecondaryColor = System.Drawing.Color.FromArgb(240, 244, 248); // Xám nhạt nền
        public static System.Drawing.Color TextColor = System.Drawing.Color.FromArgb(50, 50, 50);
        public static System.Drawing.Color DangerColor = System.Drawing.Color.Crimson;
        public static System.Drawing.Color SuccessColor = System.Drawing.Color.SeaGreen;
    }

    public class SinhVien
    {
        public string MaSV { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string Lop { get; set; }

        // Hiển thị thông tin tóm tắt (dùng cho ComboBox)
        public override string ToString() { return $"{MaSV} - {HoTen}"; }
    }

    public class LopHocModel
    {
        public string MaLop { get; set; }
        public string TenLop { get; set; }
        public string Khoa { get; set; }

        public override string ToString() { return TenLop; }
    }

    public class DiemModel
    {
        public string MaSV { get; set; } // Lưu Mã SV thay vì Tên để định danh chính xác
        public string TenSV { get; set; } // Lưu thêm tên để hiển thị nhanh
        public string Mon { get; set; }
        public double Diem { get; set; }
    }
}