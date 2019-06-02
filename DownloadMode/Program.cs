using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;

namespace DownloadMode
{
    class Program
    {
        static void Main()
        {
            var consoleWindow = GetConsoleWindow();
            ShowWindow(consoleWindow, SW_HIDE);
            
            CheckForProcesses();
            ConnectionManager.SetInterfaceName("Ethernet");
            Interface.SetupTrayIcon();
            Application.Run();
        }
       
        #region Utility Functions

        private static void CheckForProcesses()
        {
            Process[] existingProcesses = Process.GetProcessesByName("DownloadMode");
            if (existingProcesses.Length > 1)
            {
                for (int i = 1; i < existingProcesses.Length; i++)
                {
                    existingProcesses[i].Kill();
                }
            }
        }

        public static void EnableLaunchOnStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
            (@"Software\\Microsoft\Windows\CurrentVersion\Run", true);
            rk.SetValue(Application.ProductName, @"C:\Program Files\DownloadMode\DownloadMode.exe");

            Settings.Default.LaunchOnStartup = true;
            Settings.Default.Save();
        }

        public static void DisableLaunchOnStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
            (@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            rk.DeleteValue(Application.ProductName, true);

            Settings.Default.LaunchOnStartup = false;
            Settings.Default.Save();
        }

        #endregion


        // The two dll imports below handle the window hiding.

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
    }
}
