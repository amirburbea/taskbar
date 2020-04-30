using System;
using System.Windows;
using System.Windows.Interop;
using Taskbar.FlashMethods;

namespace Taskbar.FlashTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static int _count;

        public MainWindow()
        {
            this.InitializeComponent();
            this.Title = $"Window #{++MainWindow._count}";
        }

        private void NewWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Window spawn = new MainWindow();
            spawn.SourceInitialized += this.Spawn_SourceInitialized;
            spawn.Show();
        }

        private void Spawn_SourceInitialized(object sender, EventArgs e)
        {
            Window other = (Window)sender;
            other.SourceInitialized -= this.Spawn_SourceInitialized;
            this.Dispatcher.InvokeAsync(() =>
            {
                this.Activate();
                this.Focus();
                TaskbarFlash.Flash(new WindowInteropHelper(other).Handle, 8);
            });
        }
    }
}
