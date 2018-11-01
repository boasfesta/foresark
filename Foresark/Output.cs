using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foresark
{
    class Output
    {
        public static void printLine(string msg, ConsoleColor color = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black, bool center = false, bool fillLine = false, bool newLine = true)
        {
            if (Console.BackgroundColor != bgColor)
                    Console.BackgroundColor = bgColor;

            if (Console.ForegroundColor != color)
                    Console.ForegroundColor = color;

            if (center)
                Console.SetCursorPosition((Console.WindowWidth - msg.Length) / 2, Console.CursorTop);

            if (fillLine)
                msg = msg.PadRight(Console.WindowWidth - 1);

            if (newLine)
                Console.WriteLine(msg);
            else
                Console.Write(msg);
        }

        public static void newLine() {
            Console.WriteLine(string.Empty);
        }

        public static void printText(string msg, ConsoleColor color = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black)
        {
            printLine(msg, color, bgColor, false, false, false);
        }

        public static void printFullLine(ConsoleColor color)
        {
            printLine("", ConsoleColor.White, color, false, true, true);
        }

        public static void pause()
        {
            resetConsole();
            Console.ReadKey();
        }

        public static string read()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            return Console.ReadLine();
        }

        public static void clear()
        {
            Console.Clear();
            printFullLine(ConsoleColor.DarkRed);
            printLine("FORESARK", ConsoleColor.Red, ConsoleColor.Black, true);
            if (Foresark.socket.Connected)
            {
                printLine("Connected to " + Foresark.socket.Client.RemoteEndPoint, ConsoleColor.Green, ConsoleColor.Black, true);
            }
            printFullLine(ConsoleColor.DarkRed);
        }

        public static void printMsg(string msg)
        {
            List<CustomString> strMsg = formatMsg(msg.Trim());
            foreach (CustomString text in strMsg)
            {
                printText(text.Text, text.Color);
            }
            Console.WriteLine(string.Empty);
        }

        private static List<CustomString> formatMsg(string msg, ConsoleColor color = ConsoleColor.White)
        {
            string prefix = string.Empty;
            string sufix = string.Empty;
            int tagEnd;
            List<CustomString> str = new List<CustomString>();
            int tagIndex = msg.IndexOf("[");
            if (tagIndex >= 0)
            {
                if (tagIndex > 0)
                {
                    prefix = msg.Substring(0, tagIndex);
                    msg = msg.Remove(0, tagIndex);
                }

                int lastTag = msg.LastIndexOf("]");

                if (lastTag != msg.Length)
                {
                    sufix = msg.Substring(lastTag + 1, msg.Length - lastTag - 1);
                    msg = msg.Remove(lastTag + 1, msg.Length - lastTag - 1);
                }

                if (prefix != string.Empty)
                {
                    str.Add(new CustomString(prefix, color));
                }

                while (lastTag >= 0)
                {
                    tagIndex = msg.IndexOf("[");
                    tagEnd = msg.IndexOf("]");
                    string tagColor = msg.Substring(tagIndex + 1, tagEnd - tagIndex - 1);
                    ConsoleColor csColor = getColor(tagColor);
                    msg = msg.Remove(tagIndex, tagEnd - tagIndex + 1);

                    string closeTag = "[/" + tagColor + "]";
                    int tagCloseIndex = msg.LastIndexOf(closeTag);
                    
                    string innerMsg = msg.Substring(0, tagCloseIndex);

                    str.AddRange(formatMsg(innerMsg, csColor));

                    msg = msg.Remove(0, tagCloseIndex + closeTag.Length);

                    lastTag = msg.LastIndexOf("]");
                }

                if (sufix != string.Empty)
                {
                    str.Add(new CustomString(sufix, color));
                }

            }
            else
            {
                str.Add(new CustomString(msg, color));
            }

            return str;
        }

        private static ConsoleColor getColor(string colorName)
        {
            foreach (ConsoleColor color in Enum.GetValues(typeof(ConsoleColor)))
            {
                if (color.ToString().ToLower() == colorName.ToLower())
                {
                    return color;
                }
                     
            }
            return ConsoleColor.White;
        }

        private static void resetConsole()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    class CustomString
    {
        public ConsoleColor Color;
        public String Text;

        public CustomString(string text, ConsoleColor color)
        {
            Color = color;
            Text = text;
        }

        public CustomString(string text)
        {
            Color = ConsoleColor.White;
            Text = text;
        }
    }
}
