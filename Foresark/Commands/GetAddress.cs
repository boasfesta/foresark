using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foresark.Commands
{
    class GetAddress : Command
    {
        private static readonly string[] requiredParameters = { "p" };

        private static readonly string[] parametersDescription = {
            "Specify the process that you want to get the IP Address"
        };

        public GetAddress() : base() { }
        public GetAddress(Dictionary<string, string> parameters) : base(parameters) { }

        public override object action()
        {
            var output = Netstat();

            ProcessOutput(output);

            return null;
        }

        public override void help()
        {
            Output.printMsg("[cyan]GETADDRESS[/cyan]");
            this.PrintRequiredParameters(requiredParameters, parametersDescription);
            Output.printMsg("Get the connected IPs on your machine or a process specified IP");
        }

        public override string ToString()
        {
            return "Get the connected IPs on your machine or a process specified IP";
        }

        public static string Netstat()
        {
            System.Diagnostics.Process cmd = new System.Diagnostics.Process();
            cmd.StartInfo.FileName = Environment.ExpandEnvironmentVariables("%SystemRoot%") + @"\System32\cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.Verb = "runas";
            cmd.Start();

            cmd.StandardInput.WriteLine("netstat -n -b");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            string output = cmd.StandardOutput.ReadToEnd();

            cmd.Close();
            return output;
        }

        public void ProcessOutput(string output)
        {
            var processAddress = GetAddresses(output);

            if (processAddress.Count == 0)
            {
                Output.printMsg("[red]No addresses was found, check if you started Foresark as Administrator[/red]");
            }
            else
            {
                String process = this.GetParameter("p");
                string addresses = String.Empty;

                if (process == null)
                {
                    foreach(var key in processAddress.Keys)
                    {
                        if (processAddress.TryGetValue(key, out addresses))
                        {
                            Output.printMsg("[yellow](" + key + ")[/yellow]");
                            Output.printMsg(addresses);
                        }
                    }
                }
                else
                {
                    if (processAddress.TryGetValue(process, out addresses))
                    {
                        Output.printMsg("[yellow](" + process + ")[/yellow]");
                        Output.printMsg(addresses);
                    }
                    else
                    {
                        Output.printMsg("[red]The process " + process + " could not be found[/red]");
                    }
                }
            }
        }

        public static Dictionary<string, string> GetAddresses(string output)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            while (output.IndexOf("[") > 0)
            {
                var startIndex = 0;
                var endIndex = output.IndexOf("[");
                var processName = output.Substring(endIndex + 1, output.IndexOf("]") - endIndex - 1);
                var limitProcessAddresses = output.Substring(endIndex + 1).IndexOf("[");
                string processAddresses = string.Empty;
                if (limitProcessAddresses > 0)
                {
                    processAddresses = output.Substring(endIndex + 1, limitProcessAddresses - 1);
                    output = output.Substring(endIndex + 1);
                }
                else
                {
                    processAddresses = output.Substring(endIndex + 1);
                    output = string.Empty;
                }

                if (!result.Keys.Any(p => p == processName.ToLower()))
                {
                    result.Add(processName.ToLower(), processAddresses);
                }
                else
                {
                    string value = string.Empty;
                    if (result.TryGetValue(processName.ToLower(), out value))
                    {
                        value = value + processAddresses;
                        result.Remove(processName.ToLower());
                        result.Add(processName.ToLower(), value);
                    }
                }
            }

            //Remove the version info
            result.Remove(result.Keys.First());

            return result;
        }

        public static string GetIPAndPort(string address)
        {
            var externalAddressStart = address.Substring(address.IndexOf(':') + 1).IndexOf(':') + address.IndexOf(':');
            int externalAddressEnd = externalAddressStart;

            while (!address[externalAddressStart].Equals(' ') || !address[externalAddressEnd].Equals(' '))
            {
                if (!address[externalAddressStart].Equals(' '))
                {
                    externalAddressStart -= 1;
                }
                if (!address[externalAddressEnd].Equals(' '))
                {
                    externalAddressEnd += 1;
                }
            }

            return address.Substring(externalAddressStart, externalAddressEnd - externalAddressStart).Trim();
        }

    }
}
