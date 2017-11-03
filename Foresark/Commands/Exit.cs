using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foresark.Commands
{
    class Exit : Command
    {
        public Exit() : base() { }
        public Exit(Dictionary<string, string> parameters) : base(parameters) { }

        public override object action()
        {
            Environment.Exit(0);
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
