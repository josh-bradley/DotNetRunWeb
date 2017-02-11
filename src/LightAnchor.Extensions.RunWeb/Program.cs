using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace LightAnchor.Extensions.RunWeb
{
    public class Program
    { 
        const string DefaultURIEnvironmentVariable = "ASPNETCORE_URLS";
        public static void Main(string[] args)
        {
            var options = new Options(args);
            options.OptionErrors.ForEach(Console.Error.WriteLine);

            var uri = $"http://localhost:{options.PortNumber}";
            
            var process = StartDotNetRun(options.UnknownArgs, uri);
            StreamReader reader = process.StandardOutput;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (options.ShouldOpenBrowser && IsWebServerOnlineMessage(line))
                {
                    Browser.StartBrowser(uri);
                }
                Console.Out.WriteLine(line);
            }
        }

        private static bool IsWebServerOnlineMessage(string line) => line.Contains("Now listening on");

        private static Process StartDotNetRun(IEnumerable<string> argumentsForDotNetRun, string uri)
        {
            var process = new Process();

            process.StartInfo.FileName = "dotnet.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.Environment.Add(DefaultURIEnvironmentVariable, uri);
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.Arguments = $"run {String.Join(" ", argumentsForDotNetRun)}";
            process.Start();

            return process;
        }
    }
}
