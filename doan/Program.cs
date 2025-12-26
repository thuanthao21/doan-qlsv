using System;
using System.Windows.Forms;

namespace doan
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // [QUAN TRỌNG] Thêm dòng này để fix lỗi mờ giao diện
            // Lưu ý: Chỉ chạy được trên .NET Core 3.0 trở lên (.NET 5, 6, 7, 8...)
            Application.SetHighDpiMode(HighDpiMode.SystemAware);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // --- ĐOẠN LOGIC CŨ CỦA BẠN ---
            FrmLogin login = new FrmLogin();

            if (login.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new Form1());
            }
            else
            {
                Application.Exit();
            }
            // -----------------------------
        }
    }
}