using System;
using Microsoft.Win32;
using System.Diagnostics;

namespace LightAnchor.Extensions.RunWeb
{
    public class Browser
    {
        public static void StartBrowser(string uri)
        {
            var defaultBrowserProgId = GetDefaultBrowserProgId();

            if (defaultBrowserProgId == null)
            {
                Console.Error.WriteLine("Default browser not set.");
                return;
            }

            var browserExe = GetBrowserExe(defaultBrowserProgId);

            if (browserExe == null)
            {
                Console.Error.WriteLine("Unable to find default browser executable.");
                return;
            }

            var process = new Process();
            process.StartInfo.FileName = browserExe;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.Arguments = uri;
            process.Start();
        }

        private static string GetDefaultBrowserProgId()
        {
            const string userChoice = @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice";

            using (RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey(userChoice))
            {
                if (userChoiceKey == null)
                    return null;

                object progIdValue = userChoiceKey.GetValue("Progid");
                if (progIdValue == null)
                    return null;

                return progIdValue.ToString();
            }
        }

        private static string GetBrowserExe(string progId)
        {
            const string exeSuffix = ".exe";
            string path = null;
            using (RegistryKey pathKey = Registry.ClassesRoot.OpenSubKey(progId + @"\shell\open\command"))
            {
                if (pathKey == null)
                {
                    return null;
                }

                try
                {
                    path = pathKey.GetValue(null).ToString().ToLower().Replace("\"", "");
                    if (!path.EndsWith(exeSuffix))
                    {
                        path = path.Substring(0, path.LastIndexOf(exeSuffix, StringComparison.Ordinal) + exeSuffix.Length);
                    }
                }
                catch
                {
                }

                return path;
            }
        }
    }
}
