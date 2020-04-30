using System;

namespace Taskbar.FlashKiller
{
    internal static class Constants
    {
        public static readonly IntPtr HSHELL_FLASH = new IntPtr(0x8006);
        public const int GWL_EX_STYLE = -20;
        public const int WS_EX_APPWINDOW = 0x00040000;
        public const int WS_EX_TOOLWINDOW = 0x00000080;
        public const int WH_SHELL = 10;
        public const int HSHELL_REDRAW = 6;
        public const int MSG_NOTIFY = 0;
    }
}
