using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foresark.Commands
{
    class About : Command
    {
        public About() : base() { }
        public About(Dictionary<string, string> parameters) : base(parameters) { }

        public override object action()
        {
            Output.printMsg("[red]FORESARK FORSAKEN[/red]");
            Output.printMsg("[yellow]Version: " + Program.getVersion() + "[/yellow]");
            Output.printMsg("[yellow]Created by: boasfesta[/yellow]");
            return null;
        }

        public override void help()
        {
            Output.printMsg("This command is simple! What the hell you want from me?");
        }

        public override string ToString()
        {
            return "Show information about the software";
        }
    }
}
