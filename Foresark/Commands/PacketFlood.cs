using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foresark.Commands
{
    class PacketFlood : Command
    {
        public PacketFlood() : base() { }
        public PacketFlood(Dictionary<string, string> parameters) : base(parameters) { }

        private static int packetLength = 1073741823;
        private string attackType;
        private string stackPacket;
        private char endChar;

        public override object action()
        {
            if (!Foresark.socket.Connected)
            {
                Output.printLine("This command require a open connection to work!", ConsoleColor.Red);
                return null;
            }

            if (this.GetParameter("config") == null)
            {
                attackType = this.GetParameter("type");

                switch (attackType)
                {
                    case "stack":
                        string endCharParam = this.GetParameter("char");
                        int endCharNum;
                        if (int.TryParse(endCharParam, out endCharNum))
                        {
                            endChar = (char)endCharNum;
                            stackPacket.PadLeft(packetLength, endChar);
                        }
                        else
                        {
                            Output.printLine("Invalid end char num format", ConsoleColor.Red);
                        }

                        break;
                    default:
                        Output.printLine("Attack type not found!", ConsoleColor.Red);
                        break;
                }
            }
            else
            {
                string configParam = this.GetParameter("config");
                string value = this.GetParameter("value");
                if (value == null)
                {
                    Output.printLine("There is no new value specified!", ConsoleColor.Red);
                }
                switch (configParam)
                {
                    case "packetlength":
                        packetLength = int.Parse(value);
                        Output.printMsg("Packet length changed to: [yellow]" + packetLength + "[/yellow]");
                        break;
                    default:
                        Output.printLine("Config parameter not found", ConsoleColor.Red);
                        break;
                }
            }
            return null;
        }

        public override void help()
        {
            Output.printLine("REQUIRE A OPEN CONNECTION TO WORK",ConsoleColor.Red);
            Output.printMsg("Send a flood of packets to the server.");
            Output.printMsg("[green]Configuration parameters (Type: packetflood -config paramToConfig -value newValue):[/green]");
            Output.printMsg("packetlength (Total of end chars. [cyan]default is 1,073,741,823[/cyan])");
            Output.printMsg("[green]For attack type, select one of the follow:[/green]");
            Output.printMsg("[yellow]STACK[/yellow]");
            Output.printMsg("For String communication type, send a packet full of end characters.");
            Output.printMsg("[red]REQUIRED EXTRA PARAMETER:[/red]: -char (end char ascii number)");
        }

        public override string ToString()
        {
            return "Send a packet flood to the server, in a variant methods";
        }
    }
}
