using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foresark.Commands
{
    abstract class Command
    {
        public Dictionary<string, string> parameters;

        public Command() { }

        public Command(Dictionary<string, string> parameters)
        {
            this.parameters = parameters;

            if (parameters.ContainsKey("help"))
            {
                Output.printMsg("[yellow]Details:[/yellow]");
                this.help();
            }
            else
            {
                this.action();
            }
        }

        public string GetParameter(string key)
        {
            return parameters.Where(k => k.Key == key).FirstOrDefault().Value;
        }

        public abstract object action();
        public abstract void help();
    }
}
