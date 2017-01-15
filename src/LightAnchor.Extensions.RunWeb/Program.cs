using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LightAnchor.Extensions.RunWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string DefaultURIEnvironmentVariable = "ASPNETCORE_URLS";

            var options = new Options(args);
            options.OptionErrors.ForEach(Console.Error.WriteLine);

            var uri = $"http://localhost:{options.PortNumber}";
            Environment.SetEnvironmentVariable(DefaultURIEnvironmentVariable, uri);

            var process = StartDotNetRun(options.UnknownArgs);
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

        private static Process StartDotNetRun(IEnumerable<string> argumentsForDotNetRun)
        {
            var process = new Process();

            process.StartInfo.FileName = "dotnet.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.Arguments = $"run {String.Join(" ", argumentsForDotNetRun)}";
            process.Start();

            return process;
        }
    }
}
