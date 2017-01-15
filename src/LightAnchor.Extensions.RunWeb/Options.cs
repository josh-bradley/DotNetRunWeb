using System;
using System.Collections.Generic;
using System.Linq;

namespace LightAnchor.Extensions.RunWeb
{
    public class Options
    {
        string[] OpenBrowserOptionAlias = { "-o", "--open" };
        string[] PortNumberOptionAlias = { "-r", "--port" };

        public Options(string[] args)
        {
            ShouldOpenBrowser = FindOptionIndex(args, OpenBrowserOptionAlias) >= 0;
            SetPortNumberFromOption(args);
            FindUnknownOptions(args);
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
                    if (Int32.TryParse(args[indexPortNumberArg + 1], out portNumber))
                    {
                        PortNumber = portNumber.ToString();
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

        public bool ShouldOpenBrowser { get; private set; }

        public string PortNumber { get; private set; } = "5001";

        public List<string> UnknownArgs { get; set; } = new List<string>();

        public List<string> OptionErrors { get; private set; } = new List<string>();
    }
}
