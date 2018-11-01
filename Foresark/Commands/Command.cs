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
            executeCommand();
        }

        public Command(Dictionary<string, string> parameters, string[] requiredParameters)
        {
            this.parameters = parameters;
            executeCommand(requiredParameters);
        }

        private void executeCommand(string[] requiredParameters = null)
        {
            bool canStart = true;
            if (parameters.ContainsKey("help"))
            {
                Output.printMsg("[yellow]Details:[/yellow]");
                this.help();
            }
            else
            {
                if (requiredParameters != null)
                {
                    foreach (string p in requiredParameters)
                    {
                        if (!parameters.ContainsKey(p))
                        {
                            Output.printMsg("[red]Parameter missing: [/red]" + p);
                            canStart = false;
                        }
                    }
                }

                if (canStart)
                    this.action();
                else
                    Output.printMsg("[yellow]Failed to execute command! For more information Type:[/yellow] command -help");
            }
        }

        public string GetParameter(string key)
        {
            return parameters.Where(k => k.Key == key).FirstOrDefault().Value;
        }

        public void PrintRequiredParameters(string[] requiredParameters, string[] parametersDescription)
        {
            Output.printMsg("[green]Required parameters:[/green]");
            for(int i = 0; i < requiredParameters.Length; i++)
            {
                Output.printMsg("[yellow]-" + requiredParameters[i] + "[/yellow]: " + parametersDescription[i]);
            }
        }

        public abstract object action();
        public abstract void help();
    }
}
