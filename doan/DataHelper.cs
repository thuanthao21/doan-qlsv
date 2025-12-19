using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace doan
{
    public static class DataHelper
    {
        private static string fileSV = "sinhvien.txt";
        private static string fileLop = "lophoc.txt";
        private static string fileDiem = "diem.txt";

        // Hàm khởi tạo: Tự động tạo file nếu chưa tồn tại
        static DataHelper()
        {
            if (!File.Exists(fileSV)) File.Create(fileSV).Close();
            if (!File.Exists(fileLop)) File.Create(fileLop).Close();
            if (!File.Exists(fileDiem)) File.Create(fileDiem).Close();
        }

        // --- Xử lý an toàn chuỗi (Loại bỏ ký tự phân tách | nếu người dùng cố tình nhập) ---
        private static string Sanitize(string input)
        {
            return input == null ? "" : input.Replace("|", "-");
        }

        // --- SINH VIÊN ---
        public static void LuuSV(List<SinhVien> ds)
        {
            var lines = ds.Select(s => $"{Sanitize(s.MaSV)}|{Sanitize(s.HoTen)}|{Sanitize(s.Email)}|{Sanitize(s.SoDienThoai)}|{s.NgaySinh}|{s.GioiTinh}|{Sanitize(s.Lop)}");
            File.WriteAllLines(fileSV, lines, Encoding.UTF8);
        }

        public static List<SinhVien> DocSV()
        {
            if (!File.Exists(fileSV)) return new List<SinhVien>();
            var list = new List<SinhVien>();
            foreach (var line in File.ReadAllLines(fileSV))
            {
                var p = line.Split('|');
                if (p.Length >= 7)
                {
                    list.Add(new SinhVien { MaSV = p[0], HoTen = p[1], Email = p[2], SoDienThoai = p[3], NgaySinh = p[4], GioiTinh = p[5], Lop = p[6] });
                }
            }
            return list;
        }

        // --- LỚP HỌC ---
        public static void LuuLop(List<LopHocModel> ds)
        {
            var lines = ds.Select(l => $"{Sanitize(l.MaLop)}|{Sanitize(l.TenLop)}|{Sanitize(l.Khoa)}");
            File.WriteAllLines(fileLop, lines, Encoding.UTF8);
        }

        public static List<LopHocModel> DocLop()
        {
            if (!File.Exists(fileLop)) return new List<LopHocModel>();
            return File.ReadAllLines(fileLop)
                .Select(l => l.Split('|'))
                .Where(p => p.Length >= 3)
                .Select(p => new LopHocModel { MaLop = p[0], TenLop = p[1], Khoa = p[2] })
                .ToList();
        }

        // --- ĐIỂM SỐ ---
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
                {
                    list.Add(new DiemModel { MaSV = p[0], TenSV = p[1], Mon = p[2], Diem = d });
                }
            }
            return list;
        }
    }
}