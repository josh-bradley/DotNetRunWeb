using System;
using System.Collections.Generic;
using System.Linq;

namespace LightAnchor.Extensions.RunWeb
{
    public class Options
    {
        string[] OpenBrowserOptionAlias = { "-o", "--open" };
        string[] PortNumberOptionAlias = { "-r", "--port" };
        string[] HelpOptionsAlias = { "-h", "--help" };

        public Options(string[] args)
        {
            ShouldOpenBrowser = FindOptionIndex(args, OpenBrowserOptionAlias) >= 0;
            SetPortNumberFromOption(args);
            FindUnknownOptions(args);
            Help = FindOptionIndex(args, HelpOptionsAlias) >= 0;
        }

        public List<string> GetHelpTextLines()
        {
            var help = new List<string> {
                ".Net Run Web Command",
                " ",
                "Usage: dotnet runw [options]",
                " ",
                "Options:",
                "-h|--help\tShow help information",
                "-o|--open\tOpen in default browser",
                "-r|--port\tPort number to use when running the app",
                "As well as any dotnet run options"
            };
            return help;
        }

        private void SetPortNumberFromOption(string[] args)
        {
            var indexPortNumberArg = FindOptionIndex(args, PortNumberOptionAlias);

            if (indexPortNumberArg >= 0)
            {
                if (indexPortNumberArg + 1 == args.Length)
                    OptionErrors.Add("No port number provided");
                else
                {
                    int portNumber;
                    if (Int32.TryParse(args[indexPortNumberArg + 1], out portNumber) 
                        && portNumber < 65535
                        && portNumber != 0)
                    {
                        PortNumber = Math.Abs(portNumber).ToString();
                        PortNumberProvided = true;
                    }
                    else
                        OptionErrors.Add("Invalid port number format");
                }

            }
        }

        private void FindUnknownOptions(string[] args)
        {
            bool lastArgWasPortNumber = false;
            var knownArguments = OpenBrowserOptionAlias.Concat(PortNumberOptionAlias);
            foreach (var arg in args)
            {
                if (!knownArguments.Contains(arg) && (!lastArgWasPortNumber || arg.StartsWith("-")))
                {
                    UnknownArgs.Add(arg);
                }

                lastArgWasPortNumber = PortNumberOptionAlias.Contains(arg);
            }
        }

        private static int FindOptionIndex(string[] array, string[] values)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (Array.IndexOf(values, array[i]) >= 0)
                    return i;
            }

            return -1;
        }

        public bool Help { get; private set; }

        public bool ShouldOpenBrowser { get; private set; }

        public string PortNumber { get; private set; }

        public bool PortNumberProvided { get; private set; } = false;

        public List<string> UnknownArgs { get; set; } = new List<string>();

        public List<string> OptionErrors { get; private set; } = new List<string>();
    }
}
