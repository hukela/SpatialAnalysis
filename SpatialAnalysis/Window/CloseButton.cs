using System;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace SpatialAnalysis.MyWindow
{
    /// <summary>
    /// 用于禁用窗口上的关闭按键
    /// </summary>
    class CloseButton
    {
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int GetSystemMenu(int hwnd, int revert);
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int EnableMenuItem(int menu, int ideEnableItem, int enable);

        private const int SC_CLOSE = 0xF060;
        private const int MF_BYCOMMAND = 0x00000000;
        private const int MF_GRAYED = 0x00000001;
        private const int MF_ENABLED = 0x00000002;
        private CloseButton() { /*屏蔽构造函数*/ }
        public static void Disable(int handle)
        {
            // The return value specifies the previous state of the menu item
            // (it is either MF_ENABLED or MF_GRAYED). 0xFFFFFFFF indicates that
            // the menu item does not exist.
            switch (EnableMenuItem(GetSystemMenu(handle, 0), SC_CLOSE, MF_BYCOMMAND | MF_GRAYED))
            {
                case MF_ENABLED:
                    break;
                case MF_GRAYED:
                    break;
                case -1:
                    throw new Exception("The Close menu item does not exist.");
                default:
                    break;
            }
        }
    }
}
