using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace doan
{
    public static class DataHelper
    {
        private static string fileSV = "sinhvien.txt";
        private static string fileLop = "lophoc.txt";
        private static string fileDiem = "diem.txt";

        static DataHelper()
        {
            if (!File.Exists(fileSV)) File.Create(fileSV).Close();
            if (!File.Exists(fileLop)) File.Create(fileLop).Close();
            if (!File.Exists(fileDiem)) File.Create(fileDiem).Close();
        }

        private static string Sanitize(string input) { return input == null ? "" : input.Replace("|", "-"); }

        // SINH VIÊN (Cập nhật thêm ảnh)
        public static void LuuSV(List<SinhVien> ds)
        {
            // Thêm cột AnhDaiDien vào cuối
            var lines = ds.Select(s => $"{Sanitize(s.MaSV)}|{Sanitize(s.HoTen)}|{Sanitize(s.Email)}|{Sanitize(s.SoDienThoai)}|{s.NgaySinh}|{s.GioiTinh}|{Sanitize(s.Lop)}|{Sanitize(s.AnhDaiDien)}");
            File.WriteAllLines(fileSV, lines, Encoding.UTF8);
        }

        public static List<SinhVien> DocSV()
        {
            if (!File.Exists(fileSV)) return new List<SinhVien>();
            var list = new List<SinhVien>();
            foreach (var line in File.ReadAllLines(fileSV))
            {
                var p = line.Split('|');
                // Kiểm tra nếu dòng có đủ 8 cột (dữ liệu mới) hoặc 7 cột (dữ liệu cũ)
                if (p.Length >= 7)
                {
                    list.Add(new SinhVien
                    {
                        MaSV = p[0],
                        HoTen = p[1],
                        Email = p[2],
                        SoDienThoai = p[3],
                        NgaySinh = p[4],
                        GioiTinh = p[5],
                        Lop = p[6],
                        // Nếu file cũ chưa có cột 8 thì để trống ảnh
                        AnhDaiDien = p.Length > 7 ? p[7] : ""
                    });
                }
            }
            return list;
        }

        // LỚP HỌC (Giữ nguyên)
        public static void LuuLop(List<LopHocModel> ds)
        {
            var lines = ds.Select(l => $"{Sanitize(l.MaLop)}|{Sanitize(l.TenLop)}|{Sanitize(l.Khoa)}");
            File.WriteAllLines(fileLop, lines, Encoding.UTF8);
        }
        public static List<LopHocModel> DocLop()
        {
            if (!File.Exists(fileLop)) return new List<LopHocModel>();
            return File.ReadAllLines(fileLop).Select(l => l.Split('|')).Where(p => p.Length >= 3)
                .Select(p => new LopHocModel { MaLop = p[0], TenLop = p[1], Khoa = p[2] }).ToList();
        }

        // ĐIỂM SỐ (Giữ nguyên)
        public static void LuuDiem(List<DiemModel> ds)
        {
            var lines = ds.Select(d => $"{Sanitize(d.MaSV)}|{Sanitize(d.TenSV)}|{Sanitize(d.Mon)}|{d.Diem}");
            File.WriteAllLines(fileDiem, lines, Encoding.UTF8);
        }
        public static List<DiemModel> DocDiem()
        {
            if (!File.Exists(fileDiem)) return new List<DiemModel>();
            var list = new List<DiemModel>();
            foreach (var line in File.ReadAllLines(fileDiem))
            {
                var p = line.Split('|');
                if (p.Length >= 4 && double.TryParse(p[3], out double d))
                    list.Add(new DiemModel { MaSV = p[0], TenSV = p[1], Mon = p[2], Diem = d });
            }
            return list;
        }
    }
}