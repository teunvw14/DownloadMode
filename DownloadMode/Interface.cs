using System;
using System.Windows.Forms;
using System.Drawing;

namespace DownloadMode
{
    class Interface
    {

        public static NotifyIcon displayIcon = new NotifyIcon();
        static Icon dwnldIcon = new Icon("src/download_logo_DISABLED.ico");
        static Icon notDwnldIcon = new Icon("src/download_logo_ENABLED.ico");

        public static void SetupTrayIcon()
        {
            try
            {
                bool e = ConnectionManager.GetLANEnabled();
                if (e)
                {
                    ConnectionManager.LANEnabled = true;
                    SetEnableButtonSettings();
                }
                else
                {
                    ConnectionManager.LANEnabled = false;
                    SetDisableButtonSettings();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            MenuItem exitMenuItem = new MenuItem("Quit DownloadMode", new EventHandler(Interface.onExitButtonClick));
            MenuItem setOnStartLaunchMenuItem = new MenuItem("Launch on startup", new EventHandler(Interface.onSOSLBClick));
            bool launchOnStartUp = Settings.Default.LaunchOnStartup;
            setOnStartLaunchMenuItem.Checked = launchOnStartUp;
            MenuItem[] menuItems = { setOnStartLaunchMenuItem, exitMenuItem };


            displayIcon.ContextMenu = new ContextMenu(menuItems);
            displayIcon.MouseDown += new MouseEventHandler(OnIconClick);
            displayIcon.Visible = true;
        }

        #region Form Eventhandlers

        public static void onSOSLBClick(object sender, EventArgs e)
        {
            if (Settings.Default.LaunchOnStartup)
            {
                Program.DisableLaunchOnStartup();
                displayIcon.ContextMenu.MenuItems[0].Checked = false;
            }
            else
            {
                Program.EnableLaunchOnStartup();
                displayIcon.ContextMenu.MenuItems[0].Checked = true;
            }
        }

        public static void onExitButtonClick(object sender, EventArgs e)
        {
            ConnectionManager.DisableDownloadMode();
            Application.Exit();
        }

        public static void OnIconClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (ConnectionManager.LANEnabled)
                {
                    ConnectionManager.EnableDownloadMode();
                    SetDisableButtonSettings();
                }
                else
                {
                    ConnectionManager.DisableDownloadMode();
                    SetEnableButtonSettings();
                }
            }
        }

        #endregion


        private static void SetEnableButtonSettings()
        {
            displayIcon.Icon = dwnldIcon;
            displayIcon.Text = "Enable download mode.";
        }

        private static void SetDisableButtonSettings()
        {
            displayIcon.Icon = notDwnldIcon;
            displayIcon.Text = "Disable download mode.";
        }

    }
}
