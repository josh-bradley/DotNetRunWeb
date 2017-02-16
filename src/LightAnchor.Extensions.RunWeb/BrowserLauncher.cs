using System.Diagnostics;
#if !NET461
using System.Runtime.InteropServices;
#endif

namespace LightAnchor.Extensions.RunWeb
{
    public class BrowserLauncher
    {
        private readonly string command;
        private readonly string argumentFormat;
        private readonly bool useShellExecute;

        private BrowserLauncher(string command, string argumentFormat = "{0}", bool useShellExecute = true)
        {
            this.command = command;
            this.argumentFormat = argumentFormat;
            this.useShellExecute = useShellExecute;
        }

        public static BrowserLauncher CreateForPlatform()
        {
            BrowserLauncher browserLauncher = null;
#if NET461
            browserLauncher = new BrowserLauncher("CMD.exe", "/C start {0}", false);
#else
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                browserLauncher = new BrowserLauncher("CMD.exe", "/C start {0}", false);
            else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                browserLauncher = new BrowserLauncher("xdg-open");
            else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                browserLauncher = new BrowserLauncher("open");
#endif
            return browserLauncher;
        }

        public void Launch(string uri)
        {
            var process = new Process();
            process.StartInfo.FileName = command;
            process.StartInfo.UseShellExecute = useShellExecute;
            process.StartInfo.Arguments = string.Format(argumentFormat, uri);
            process.Start();
        }

    }
}
