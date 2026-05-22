using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace PcgTools
{
    internal static class DarkModeHelper
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private static readonly SolidColorBrush BackgroundBrush = MakeFrozen(Color.FromRgb(0x1E, 0x1E, 0x1E));

        private static SolidColorBrush MakeFrozen(Color c)
        {
            var b = new SolidColorBrush(c);
            b.Freeze();
            return b;
        }

        internal static void Apply(Window window)
        {
            if (window == null) return;
            window.Background = BackgroundBrush;
            ApplyDarkTitleBar(window);
        }

        internal static void ApplyDarkTitleBar(Window window)
        {
            try
            {
                var hwnd = new WindowInteropHelper(window).Handle;
                if (hwnd == IntPtr.Zero) return;
                int dark = 1;
                DwmSetWindowAttribute(hwnd, 20, ref dark, sizeof(int));
            }
            catch
            {
                // DWM dark mode not supported on this OS version — ignore
            }
        }
    }
}
