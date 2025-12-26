using System;

namespace doan
{
    public class SinhVien
    {
        public string MaSV { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string Lop { get; set; }
        // [MỚI] Lưu tên file ảnh (ví dụ: SV01.png)
        public string AnhDaiDien { get; set; }

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
        public string MaSV { get; set; }
        public string TenSV { get; set; }
        public string Mon { get; set; }
        public double Diem { get; set; }
    }

    public class KhoaModel
    {
        public string MaKhoa { get; set; }
        public string TenKhoa { get; set; }

        // Để hiển thị trong ComboBox đẹp hơn
        public override string ToString() { return TenKhoa; }
    }

    public class TaiKhoan
    {
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public string Quyen { get; set; } // "Admin" hoặc "SinhVien"
    }
}