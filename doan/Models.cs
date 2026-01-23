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
        public string Lop { get; set; } // Tương ứng cột MaLop trong CSDL
        public string AnhDaiDien { get; set; }

        public override string ToString() { return $"{MaSV} - {HoTen}"; }
    }

    public class LopHocModel
    {
        public string MaLop { get; set; }
        public string TenLop { get; set; }
        // [ĐÃ SỬA] Đổi 'Khoa' thành 'MaKhoa' để khớp với DataHelper và CSDL
        public string MaKhoa { get; set; }

        public override string ToString() { return TenLop; }
    }

    public class DiemModel
    {
        public string MaSV { get; set; }
        public string TenSV { get; set; }
        public string Mon { get; set; } // Tương ứng cột MonHoc trong CSDL
        public double Diem { get; set; }
    }

    public class KhoaModel
    {
        public string MaKhoa { get; set; }
        public string TenKhoa { get; set; }

        public override string ToString() { return TenKhoa; }
    }

    public class TaiKhoan
    {
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public string Quyen { get; set; }
    }
    public class MonHocModel
    {
        public string MaMon { get; set; }
        public string TenMon { get; set; }
        public int SoTinChi { get; set; }
        public override string ToString() { return TenMon; }
    }
}