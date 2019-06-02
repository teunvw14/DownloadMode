using System;
using System.Diagnostics;

namespace DownloadMode
{
    static class ConnectionManager
    {
        public static bool LANEnabled;

        private static string interfaceName;

        public static void SetInterfaceName(string name)
        {
            interfaceName = name;
        }

        public static void EnableDownloadMode()
        {
            if (LANEnabled)
            {
                var p = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "netsh",
                        Arguments = "interface set interface \"" + interfaceName + "\" disable",
                        Verb = "runas",
                        UseShellExecute = true,
                        CreateNoWindow = true
                    }
                };
                p.Start();
                LANEnabled = false;
            }
        }

        public static void DisableDownloadMode()
        {

            if (!LANEnabled)
            {
                var p = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "netsh",
                        Arguments = "interface set interface \"" + interfaceName + "\" enable",
                        Verb = "runas",
                        UseShellExecute = true,
                        CreateNoWindow = true
                    }
                };
                p.Start();
                LANEnabled = true;
            }
        }

        public static bool GetLANEnabled()
        {
            var p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "netsh",
                    Arguments = "interface show interface \"" + interfaceName + "\" ",
                    UseShellExecute = false,
                    Verb = "runas",
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            p.Start();

            string fullOutput = p.StandardOutput.ReadToEnd();

            if (fullOutput.Contains("Administrative state: Enabled"))
            {
                return true;
            }
            else if (fullOutput.Contains("Administrative state: Disabled"))
            {
                return false;
            }
            else
            {
                throw new Exception("Could not find ethernet connection");
            }
        }
    }
}
