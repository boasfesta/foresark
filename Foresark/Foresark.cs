using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foresark.Commands;

namespace Foresark
{
    class Foresark
    {
        public static System.Net.Sockets.TcpClient socket = new System.Net.Sockets.TcpClient();

        public Foresark()
        {
            Output.clear();
            Output.printMsg("To see available commands, type [yellow]help[/yellow]");
            readCommand();
        }

        private void readCommand()
        {
            Output.newLine();
            Output.printText("Command: ");
            string command = Output.read();
            handleCommand(command.Trim().ToLower());
        }

        private void handleCommand(string command)
        {
            Dictionary<string, string> commandParameters = new Dictionary<string, string>();

            if (command.Length > 3)
            {
                string[] parse = command.Split(' ');

                commandParameters.Add("action", parse[0]);

                for (int i = 1; i < parse.Length; i++)
                {
                    if (!(parse[i].StartsWith("-"))) {
                        commandParameters.Add("param" + i, parse[i]);
                    }else{
                        string paramValue;
                        if (i + 1 < parse.Length)
                        {
                            paramValue = parse[i+1];
                        }
                        else
                        {
                            paramValue = string.Empty;
                        }
                        commandParameters.Add(parse[i++].Remove(0, 1), paramValue);
                    }
                }

                string action = commandParameters.Where(c => c.Key == "action").FirstOrDefault().Value;

                Output.clear();

                switch (action)
                {
                    case "about":
                        About about = new About(commandParameters);
                        break;
                    case "help":
                        Help help = new Help(commandParameters);
                        break;
                    case "exit":
                        Exit exit = new Exit(commandParameters);
                        return;
                    case "openconnection":
                        OpenConnection openconnection = new OpenConnection(commandParameters);
                        break;
                    default:
                        Output.printMsg("[red]" + action + " is not a valid command![/red]");
                        break;
                }

                readCommand();
            }
            else
            {
                Output.printMsg("[red]Invalid command syntax![/red]");
                readCommand();
            }
        }


    }
}
