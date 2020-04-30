using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Taskbar.FlashMethods
{
    public static class TaskbarFlash
    {
        private static class NativeMethods
        {
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool FlashWindowEx(ref FLASHWINFO pwfi);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct FLASHWINFO
        {
            /// <summary>
            /// The size of the structure in bytes.
            /// </summary>
            public uint cbSize;
            /// <summary>
            /// A Handle to the Window to be Flashed. The window can be either opened or minimized.
            /// </summary>
            public IntPtr hwnd;
            /// <summary>
            /// The Flash Status.
            /// </summary>
            public uint dwFlags;
            /// <summary>
            /// The number of times to Flash the window.
            /// </summary>
            public uint uCount;
            /// <summary>
            /// The rate at which the Window is to be flashed, in milliseconds. If Zero, the function uses the default cursor blink rate.
            /// </summary>
            public uint dwTimeout;
        }

        private static class Constants
        {
            public const uint FLASHW_STOP = 0;

            public const uint FLASHW_CAPTION = 1;

            public const uint FLASHW_TRAY = 2;

            public const uint FLASHW_ALL = 3;

            public const uint FLASHW_TIMER = 4;

            public const uint FLASHW_TIMERNOFG = 12;
        }

        private static bool Flash(IntPtr handle, uint flags, uint count = uint.MaxValue, uint timeout = 0)
        {
            FLASHWINFO info = new FLASHWINFO
            {
                hwnd = handle,
                dwFlags = flags,
                uCount = count,
                dwTimeout = timeout,
                cbSize = Convert.ToUInt32(Marshal.SizeOf(typeof(FLASHWINFO)))
            };
            return NativeMethods.FlashWindowEx(ref info);
        }

        /// <summary>
        /// Flash the specified Window until it comes in the foreground
        /// </summary>
        public static bool Flash(IntPtr handle)
        {
            return TaskbarFlash.Flash(handle, Constants.FLASHW_ALL | Constants.FLASHW_TIMERNOFG);
        }

        /// <summary>
        /// Flash the specified Window for the specified number of times
        /// </summary>
        public static bool Flash(IntPtr handle, uint count)
        {
            return TaskbarFlash.Flash(handle, Constants.FLASHW_ALL, count);
        }

        /// <summary>
        /// Start Flashing the specified Window
        /// </summary>
        public static bool Start(IntPtr handle)
        {
            return TaskbarFlash.Flash(handle, Constants.FLASHW_ALL);
        }

        /// <summary>
        /// Stop Flashing the specified Window
        /// </summary>
        public static bool Stop(IntPtr handle)
        {
            return TaskbarFlash.Flash(handle, Constants.FLASHW_STOP);
        }
    }
}
