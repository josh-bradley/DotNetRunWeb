using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LightAnchor.Extensions.RunWeb
{
    public class Program
    { 
        const string DefaultURIEnvironmentVariable = "ASPNETCORE_URLS";
        const string NowListeningOn = "Now listening on:";
        public static void Main(string[] args)
        {
            Console.WriteLine("take it easy big fella");
            var options = new Options(args);
            if(options.Help) 
            {
                options.GetHelpTextLines().ForEach(Console.WriteLine);
                return;
            }
            options.OptionErrors.ForEach(Console.Error.WriteLine);            
   
            var uri = options.PortNumberProvided ? BuildUrl(options.PortNumber) : null;
            
            var process = StartDotNetRun(options.UnknownArgs, uri);
            var actualUri = string.Empty;
            var reader = process.StandardOutput;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                actualUri = GetUriFromConsoleLine(line);
                if (actualUri.Length > 0 && options.ShouldOpenBrowser)
                {
                    var launchBrowser = BrowserLauncher.CreateForPlatform();
                    if(launchBrowser == null)
                        Console.Out.WriteLine("Unable to detect platform, cannot launch browser.");
                    else
                        launchBrowser.Launch(actualUri);
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

        private static string BuildUrl(string portNumber) => $"http://localhost:{portNumber}";

        private static string GetUriFromConsoleLine(string line) 
        {
            if(line.Contains(NowListeningOn))
                return line.Substring(line.IndexOf(NowListeningOn) + NowListeningOn.Length + 1);

            return string.Empty;
        }
    }
}
