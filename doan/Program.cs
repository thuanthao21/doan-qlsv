using System;
using System.Windows.Forms;

namespace doan
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // --- ĐOẠN QUAN TRỌNG NHẤT ---
            // 1. Chạy Form Đăng nhập trước
            FrmLogin login = new FrmLogin();

            // 2. Chỉ khi đăng nhập thành công (OK) thì mới mở Form1
            if (login.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new Form1());
            }
            else
            {
                Application.Exit(); // Nếu tắt form đăng nhập thì thoát luôn
            }
            // -----------------------------
        }
    }
}