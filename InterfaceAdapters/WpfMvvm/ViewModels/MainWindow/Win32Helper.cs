using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace WpfMvvm.ViewModels.MainWindow
{
    internal static class Win32Helper
    {
        internal static void SwitchWindowSystemTheme(bool isDark, Window window)
        {
            var handle = new WindowInteropHelper(window).Handle;
            SetWindowSystemTheme(isDark, handle);
            InvalidateWindow(window);
        }

        internal static BitmapSource GetIcon()
        {
            int KEY_ICON_ID = 45; // значок ключика
            IntPtr libHandle = LoadLibrary("shell32.dll");
            IntPtr icoHandle = LoadIcon(libHandle, KEY_ICON_ID);
            var result = CreateIconFromHandle(icoHandle);
            DestroyIcon(icoHandle);
            return result;
        }

        private static BitmapSource CreateIconFromHandle(IntPtr icoHandle) =>
            Imaging.CreateBitmapSourceFromHIcon(icoHandle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        private static void SetWindowSystemTheme(bool isDark, IntPtr handle)
        {
            int[] themeAttrValue = [Convert.ToInt32(isDark)]; // bool to int: 1 = Dark, 0 = Light
            if (DwmSetWindowAttribute(handle, 19, themeAttrValue, 4) != 0)
                DwmSetWindowAttribute(handle, 20, themeAttrValue, 4);
        }

        private static void InvalidateWindow(Window window)
        {
            window.WindowStyle = WindowStyle.ThreeDBorderWindow;
            window.WindowStyle = WindowStyle.SingleBorderWindow;
        }

        #region WinAPI functions
        [DllImport("DwmApi")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);
        [DllImport("Kernel32.dll")]
        private extern static IntPtr LoadLibrary(string libName);
        [DllImport("User32.dll")]
        private extern static IntPtr LoadIcon(IntPtr libHandle, int lpIconName);
        [DllImport("user32.dll")]
        private static extern bool DestroyIcon(IntPtr handle);
        #endregion
    }
}
