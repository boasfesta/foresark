using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Foresark.Commands
{
    class OpenConnection : Command
    {
        private static readonly string[] requiredParameters = { "ip", "port", "process" };

        private static readonly string[] parametersDescription = {
            "IP Address of the target server",
            "Connection port of the target server",
            "Process filename (with .exe) to get IP and Port automatically"
        };

        public OpenConnection() : base() { }
        public OpenConnection(Dictionary<string, string> parameters) : base(parameters) { }

        public override object action()
        {
            if (Foresark.socket.Connected)
            {
                Foresark.socket.Client.Close();
                Foresark.socket = new TcpClient();
            }

            string ip = string.Empty;
            int port = 0;
            string process = GetParameter("process");
            if (process == null)
            {
                ip = GetParameter("ip");
                port = int.Parse(GetParameter("port"));
            }
            else
            {
                string processAddress = string.Empty;
                var addresses = GetAddress.GetAddresses(GetAddress.Netstat());
                if (addresses.TryGetValue(process, out processAddress))
                {
                    var externalAddress = GetAddress.GetIPAndPort(processAddress).Split(':');
                    ip = externalAddress[0];
                    port = int.Parse(externalAddress[1]);
                }
                else
                {
                    Output.printMsg("[red]The process " + process + " was not found![/red]");
                }
            }
            if (ip != string.Empty)
            {
                try
                {
                    Output.printMsg("Trying to open connection to: " + ip + ":" + port);
                    Foresark.socket.Connect(ip, port);
                }
                catch (SocketException e)
                {
                    Output.printMsg("[red]Connection unsuccessful: " + e.Message + "[/red]");
                }
                if (Foresark.socket.Connected)
                {
                    Foresark.targetIP = ip;
                    Foresark.targetPort = port;
                    Output.clear();
                }
            }
            return null;
        }

        public override void help()
        {
            Output.printMsg("[cyan]OPENCONNECTION[/cyan]");
            this.PrintRequiredParameters(requiredParameters, parametersDescription);
            Output.printMsg("[green]Description[/green]");
            Output.printMsg("Use this command for open a direct connection with the target server.");
            Output.printMsg("Only with an opened connection you can use one of the Foresark attack methods.");
        }

        public override string ToString()
        {
            return "Open a direct connection with the target server";
        }

    }
}
