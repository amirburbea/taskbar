﻿using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Taskbar.FlashMethods;

namespace Taskbar.FlashKiller
{
    partial class MainWindow
    {
        public MainWindow() => this.InitializeComponent();

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            this.WindowState = WindowState.Minimized;
            HwndSource source = (HwndSource)PresentationSource.FromVisual(this);
            NativeMethods.RegisterShellHookWindow(source.Handle);
            // Prevent the window from appearing in Alt+Tab.
            int styles = NativeMethods.GetWindowLongPtr(source.Handle, Constants.GWL_EX_STYLE);
            if (0 == NativeMethods.SetWindowLongPtr(source.Handle, Constants.GWL_EX_STYLE, (styles | Constants.WS_EX_TOOLWINDOW) & ~Constants.WS_EX_APPWINDOW))
            {
                int error = Marshal.GetLastWin32Error();
                throw new Win32Exception(error);
            }
            uint launcherThreadId = NativeMethods.GetWindowThreadProcessId(NativeMethods.GetDesktopWindow(), default);
            uint currentThreadId = NativeMethods.GetCurrentThreadId();
            if (currentThreadId != launcherThreadId)
            {
                NativeMethods.AttachThreadInput(launcherThreadId, currentThreadId, true);
            }
            source.AddHook(MainWindow.HwndHook);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            HwndSource source = (HwndSource)PresentationSource.FromVisual(this);
            source.RemoveHook(MainWindow.HwndHook);
            uint launcherThreadId = NativeMethods.GetWindowThreadProcessId(NativeMethods.GetDesktopWindow(), default);
            uint currentThreadId = NativeMethods.GetCurrentThreadId();
            if (currentThreadId != launcherThreadId)
            {
                NativeMethods.AttachThreadInput(launcherThreadId, currentThreadId, false);
            }
            base.OnClosing(e);
        }

        private static IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (wParam == Constants.HSHELL_FLASH)
            {
                TaskbarFlash.Stop(lParam);
                handled = true;
            }
            return IntPtr.Zero;
        }
    }
}
