using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;

namespace doan
{
    public static class DataHelper
    {
        // Chuỗi kết nối
        private static string strCon = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
        //private static string strCon = @"Data Source=LAPTOP-Q0DPP8QD\THAOSQL;Initial Catalog=QuanLyDaoTao;Integrated Security=True;TrustServerCertificate=True";

        private static SqlConnection GetConnection() => new SqlConnection(strCon);

        // --- 1. HÀM THỰC THI CHUNG (INSERT/UPDATE/DELETE) ---
        public static bool ThựcThi(string sql, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(sql, con);
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Database: " + ex.Message);
                return false;
            }
        }

        // --- 2. XỬ LÝ TÀI KHOẢN ---
        public static TaiKhoan KiemTraDangNhap(string user, string pass)
        {
            using (SqlConnection con = GetConnection())
            {
                string sql = "SELECT * FROM TaiKhoan WHERE TenDangNhap = @u AND MatKhau = @p";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@u", user);
                cmd.Parameters.AddWithValue("@p", pass);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    return new TaiKhoan
                    {
                        TenDangNhap = dr["TenDangNhap"].ToString(),
                        HoTen = dr["HoTen"].ToString(),
                        Quyen = dr["Quyen"].ToString()
                    };
                }
            }
            return null;
        }

        // --- 3. XỬ LÝ SINH VIÊN ---
        public static List<SinhVien> DocSV()
        {
            List<SinhVien> list = new List<SinhVien>();
            using (SqlConnection con = GetConnection())
            {
                string sql = "SELECT * FROM SinhVien";
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new SinhVien
                    {
                        MaSV = dr["MaSV"].ToString(),
                        HoTen = dr["HoTen"].ToString(),
                        Email = dr["Email"].ToString(),
                        SoDienThoai = dr["SoDienThoai"].ToString(),
                        NgaySinh = dr["NgaySinh"].ToString(),
                        GioiTinh = dr["GioiTinh"].ToString(),
                        Lop = dr["MaLop"].ToString(),
                        AnhDaiDien = dr["AnhDaiDien"].ToString()
                    });
                }
            }
            return list;
        }

        // --- 4. XỬ LÝ LỚP HỌC ---
        public static List<LopHocModel> DocLop()
        {
            List<LopHocModel> list = new List<LopHocModel>();
            using (SqlConnection con = GetConnection())
            {
                string sql = "SELECT * FROM LopHoc";
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new LopHocModel
                    {
                        MaLop = dr["MaLop"].ToString(),
                        TenLop = dr["TenLop"].ToString(),
                        MaKhoa = dr["MaKhoa"].ToString()
                    });
                }
            }
            return list;
        }

        // --- 5. XỬ LÝ KHOA ---
        public static List<KhoaModel> DocKhoa()
        {
            List<KhoaModel> list = new List<KhoaModel>();
            using (SqlConnection con = GetConnection())
            {
                string sql = "SELECT * FROM Khoa";
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new KhoaModel
                    {
                        MaKhoa = dr["MaKhoa"].ToString(),
                        TenKhoa = dr["TenKhoa"].ToString()
                    });
                }
            }
            return list;
        }

        // --- 6. XỬ LÝ ĐIỂM SỐ ---
        public static List<DiemModel> DocDiem()
        {
            List<DiemModel> list = new List<DiemModel>();
            using (SqlConnection con = GetConnection())
            {
                // Dùng JOIN để lấy tên sinh viên hiển thị lên bảng
                string sql = @"SELECT d.*, s.HoTen FROM Diem d 
                               JOIN SinhVien s ON d.MaSV = s.MaSV";
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new DiemModel
                    {
                        MaSV = dr["MaSV"].ToString(),
                        TenSV = dr["HoTen"].ToString(),
                        Mon = dr["MonHoc"].ToString(),
                        Diem = Convert.ToDouble(dr["Diem"])
                    });
                }
            }
            return list;
        }

        public static List<MonHocModel> DocMonHoc()
        {
            List<MonHocModel> list = new List<MonHocModel>();
            using (SqlConnection con = GetConnection())
            {
                string sql = "SELECT * FROM MonHoc";
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new MonHocModel
                    {
                        MaMon = dr["MaMon"].ToString(),
                        TenMon = dr["TenMon"].ToString(),
                        SoTinChi = Convert.ToInt32(dr["SoTinChi"])
                    });
                }
            }
            return list;
        }

        // --- HÀM LẤY DANH SÁCH TÀI KHOẢN (ADO.NET) ---
        public static List<TaiKhoan> DocTatCaTaiKhoan()
        {
            List<TaiKhoan> list = new List<TaiKhoan>();

            // Sử dụng chuỗi kết nối đã khai báo trong DataHelper
            using (SqlConnection con = GetConnection())
            {
                // Câu lệnh SQL lấy thông tin tài khoản
                string sql = "SELECT TenDangNhap, MatKhau, HoTen, Quyen FROM TaiKhoan";
                SqlCommand cmd = new SqlCommand(sql, con);

                try
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        list.Add(new TaiKhoan
                        {
                            TenDangNhap = dr["TenDangNhap"].ToString(),
                            MatKhau = dr["MatKhau"].ToString(),
                            HoTen = dr["HoTen"].ToString(),
                            Quyen = dr["Quyen"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    // Hiển thị lỗi nếu không kết nối được hoặc sai tên cột
                    MessageBox.Show("Lỗi khi tải danh sách tài khoản: " + ex.Message);
                }
            }
            return list;
        }
    }
}