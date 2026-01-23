namespace doan
{
    public static class UserSession
    {
        // Lưu Tên đăng nhập (ID) để truy vấn dữ liệu riêng tư nếu cần
        public static string TenDangNhap = "";

        // Lưu quyền hạn: "Admin" hoặc "SinhVien"
        public static string QuyenHan = "";

        // Lưu tên đầy đủ để hiển thị lên giao diện (Xin chào...)
        public static string TenHienThi = "";

        // Kiểm tra nhanh xem có phải Admin không
        public static bool IsAdmin()
        {
            return QuyenHan == "Admin";
        }

        // Hàm xóa phiên làm việc khi Đăng xuất
        public static void LogOut()
        {
            TenDangNhap = "";
            QuyenHan = "";
            TenHienThi = "";
        }
    }
}