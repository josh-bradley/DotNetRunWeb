using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LightAnchor.Extensions.RunWeb
{
    public class Program
    { 
        const string DefaultURIEnvironmentVariable = "ASPNETCORE_URLS";
        const string AspDotNetCoreDefaultPort = "5000";
        public static void Main(string[] args)
        {
            var options = new Options(args);
            if(options.Help) 
            {
                options.GetHelpTextLines().ForEach(Console.WriteLine);
                return;
            }
            options.OptionErrors.ForEach(Console.Error.WriteLine);            
   
            var uri = options.PortNumberProvided ? BuildUrl(options.PortNumber) : null;
            
            var process = StartDotNetRun(options.UnknownArgs, uri);
            var actualUri = GetActualUri(process);
            var reader = process.StandardOutput;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (IsWebServerOnlineMessage(line) && options.ShouldOpenBrowser)
                {
                    var launchBrowser = GetLaunchBrowserAction(); 
                    if(launchBrowser == null)
                        Console.Out.WriteLine("Unable to detect platform, cannot launch browser.");
                    else
                        launchBrowser(actualUri);
                }
                Console.Out.WriteLine(line);
            }
        }

        private static Process StartDotNetRun(IEnumerable<string> argumentsForDotNetRun, string uri)
        {
            var process = new Process();
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.Arguments = $"run {String.Join(" ", argumentsForDotNetRun)}";
            if(uri != null)
            {
                if(process.StartInfo.Environment.ContainsKey(DefaultURIEnvironmentVariable))
                    process.StartInfo.Environment[DefaultURIEnvironmentVariable] = uri;
                else
                    process.StartInfo.Environment.Add(DefaultURIEnvironmentVariable, uri);
            }

            process.Start();

            return process;
        }

        private static Action<string> GetLaunchBrowserAction()
        {
            Action<string> openCommand = null;
            if(System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                openCommand = (uri) => Browser.WindowsPlatformLaunch(uri);
            else if(System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                openCommand = (uri) => Browser.CommandLaunch(uri, "xdg-open");
            else if(System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                openCommand = (uri) => Browser.CommandLaunch(uri, "open");

            return openCommand;
        }

        private static string BuildUrl(string portNumber) => $"http://localhost:{portNumber}";

        private static bool IsWebServerOnlineMessage(string line) => line.Contains("Now listening on");

        private static string GetActualUri(Process process) => process.StartInfo.Environment.ContainsKey(DefaultURIEnvironmentVariable) ?
                                                                process.StartInfo.Environment[DefaultURIEnvironmentVariable] :
                                                                BuildUrl(AspDotNetCoreDefaultPort);
    }
}
