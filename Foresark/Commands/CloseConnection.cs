using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foresark.Commands
{
    class CloseConnection : Command
    {
        public CloseConnection() : base() { }
        public CloseConnection(Dictionary<string, string> parameters) : base(parameters) { }

        public override object action()
        {
            if (Foresark.socket.Connected) {
                Foresark.socket.Client.Close();
                Output.printMsg("Connection closed!");
                Output.clear();
            }else
            {
                Output.printMsg("There is no connection estabilished for closing!");
            }
            return null;
        }

        public override void help()
        {
            Output.printMsg("Closes the actual connection, you don't need anything else");
        }

        public override string ToString()
        {
            return "Closes the actual connection";
        }
    }
}
