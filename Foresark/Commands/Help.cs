using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foresark.Commands
{
    class Help : Command
    {
        public Help() : base() { }
        public Help(Dictionary<string, string> parameters) : base(parameters) { }

        public override object action()
        {
            IEnumerable<Command> availableCommands = typeof(Command)
                        .Assembly.GetTypes()
                        .Where(t => t.IsSubclassOf(typeof(Command)) && !t.IsAbstract)
                        .OrderBy(t => t.Name)
                        .Select(t => (Command)Activator.CreateInstance(t));

            Output.printMsg("[green]Available commands:[/green]");
            Output.printMsg("For more details about a command, type: command -help");
            Output.newLine();

            foreach (Command cmd in availableCommands)
            {
                Output.printMsg("[yellow]" + cmd.GetType().Name.ToLower() + "[/yellow] - " + cmd.ToString());
            }
            return null;
        }

        public override void help()
        {

        }

        public override string ToString()
        {
            return "Show all commands available on Foresark";
        }
    }
}
