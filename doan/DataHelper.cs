using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Windows.Forms;

namespace doan
{
    public static class DataHelper
    {
        // Danh sách các file JSON lưu dữ liệu
        private static string fileSV = "sinhvien.json";
        private static string fileLop = "lophoc.json";
        private static string fileDiem = "diem.json";
        private static string fileKhoa = "khoa.json";
        private static string fileTaiKhoan = "taikhoan.json"; // [MỚI] File lưu tài khoản

        // Cấu hình JSON: Hỗ trợ tiếng Việt và định dạng đẹp
        private static JsonSerializerOptions options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = true
        };

        // --- HÀM GỐC (GENERIC) ---
        public static void LuuFile<T>(string fileName, List<T> data)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(data, options);
                File.WriteAllText(fileName, jsonString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi ghi file {fileName}: {ex.Message}");
            }
        }

        public static List<T> DocFile<T>(string fileName)
        {
            try
            {
                if (!File.Exists(fileName)) return new List<T>();
                string jsonString = File.ReadAllText(fileName);
                return JsonSerializer.Deserialize<List<T>>(jsonString) ?? new List<T>();
            }
            catch
            {
                return new List<T>();
            }
        }

        // --- CÁC HÀM XỬ LÝ DỮ LIỆU CỤ THỂ ---

        // 1. Sinh Viên
        public static void LuuSV(List<SinhVien> ds) => LuuFile(fileSV, ds);
        public static List<SinhVien> DocSV() => DocFile<SinhVien>(fileSV);

        // 2. Lớp Học
        public static void LuuLop(List<LopHocModel> ds) => LuuFile(fileLop, ds);
        public static List<LopHocModel> DocLop() => DocFile<LopHocModel>(fileLop);

        // 3. Khoa
        public static void LuuKhoa(List<KhoaModel> ds) => LuuFile(fileKhoa, ds);
        public static List<KhoaModel> DocKhoa() => DocFile<KhoaModel>(fileKhoa);

        // 4. Điểm Số
        public static void LuuDiem(List<DiemModel> ds) => LuuFile(fileDiem, ds);
        public static List<DiemModel> DocDiem() => DocFile<DiemModel>(fileDiem);

        // 5. [MỚI] Tài Khoản (Đăng nhập/Đăng ký)
        public static void LuuTaiKhoan(List<TaiKhoan> ds) => LuuFile(fileTaiKhoan, ds);

        public static List<TaiKhoan> DocTaiKhoan()
        {
            var list = DocFile<TaiKhoan>(fileTaiKhoan);

            // [TỰ ĐỘNG TẠO] Nếu chưa có tài khoản nào, tạo mặc định để test
            if (list.Count == 0)
            {
                list.Add(new TaiKhoan { TenDangNhap = "admin", MatKhau = "123", HoTen = "Quản Trị Viên", Quyen = "Admin" });
                list.Add(new TaiKhoan { TenDangNhap = "sv", MatKhau = "1", HoTen = "Sinh Viên Test", Quyen = "SinhVien" });

                // Lưu ngay xuống file để lần sau không tạo lại nữa
                LuuTaiKhoan(list);
            }
            return list;
        }
    }
}