using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Foresark.Commands
{
    class OpenConnection : Command
    {
        public OpenConnection() : base() { }
        public OpenConnection(Dictionary<string, string> parameters) : base(parameters) { }

        public override object action()
        {
            string ip = GetParameter("ip");
            int port = int.Parse(GetParameter("port"));
            try
            {
                Foresark.socket.Connect(ip, port);
            }
            catch (SocketException e)
            {
                Output.printMsg("[red]Connection unsucessful[/red]");
            }
            if (Foresark.socket.Connected)
            {
                Output.printMsg("[green]Socket connected to " + Foresark.socket.Client.LocalEndPoint + "[/green]");
            }
            return null;
        }

        public override void help()
        {
            Output.printMsg("This command is simple! What the hell you want from me?");
        }

        public override string ToString()
        {
            return "Closes foresark";
        }

    }
}
