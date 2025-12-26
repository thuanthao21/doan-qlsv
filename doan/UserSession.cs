namespace doan
{
    public static class UserSession
    {
        // Lưu quyền hạn: "Admin" hoặc "SinhVien"
        public static string QuyenHan = "";

        // Lưu tên để hiển thị xin chào
        public static string TenHienThi = "";

        // Hàm kiểm tra nhanh xem có phải Admin không
        public static bool IsAdmin()
        {
            return QuyenHan == "Admin";
        }
    }
}