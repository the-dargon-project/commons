using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItzWarty
{
    public static class ConsoleTableUtil
    {
        public static bool colorful = false;
        public static void PrintTableBlock(string textToWrap, bool printTopBorder, bool printBottomBorder, bool verticalPadding)
        {
            ConsoleColor bigTemp = ConsoleColor.Blue; //Doesn't matter value

            int ww = Console.WindowWidth;

            if (Console.CursorLeft != 0)
            {
                Console.CursorTop++;
                Console.CursorLeft = 0;
            }
            if (printTopBorder)
                ConsoleTableUtil.PrintSeparator();
            //    Console.Write("+" + "-".Repeat(ww - 2) + "+");
            if (verticalPadding)
                ConsoleTableUtil.PrintBlankRow();
                //Console.Write("|" + " ".Repeat(ww - 2) + "|");

            string[] words = textToWrap.Split(" ");

            if(words.Length > 0)
            {
                int currentY = Console.CursorTop;
                if (colorful)
                {
                    bigTemp = Console.ForegroundColor;
                    Console.ForegroundColor = Console.BackgroundColor;
                }
                Console.Write("| ");
                if(colorful)
                    Console.ForegroundColor = bigTemp; ;
                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = words[i] + " ";
                    bool highlight = words[i].StartsWith("#!");
                    if (highlight) //Flip Foreground/background
                    {
                        words[i] = words[i].Substring(2);
                        ConsoleColor temp = Console.ForegroundColor;
                        Console.ForegroundColor = Console.BackgroundColor;
                        Console.BackgroundColor = temp;
                    }
                    Console.CursorTop = currentY;

                    if (Console.CursorLeft + words[i].Length > ww - 2)
                    {
                        Console.CursorLeft = ww - 2;
                        if (colorful)
                        {
                            bigTemp = Console.ForegroundColor;
                            Console.ForegroundColor = Console.BackgroundColor;
                        }
                        Console.Write(" |");

                        currentY++;
                        Console.CursorLeft = 0;
                        Console.CursorTop = currentY;
                        Console.Write("| ");
                        if(colorful)
                            Console.ForegroundColor = bigTemp;
                    }
                    Console.Write(words[i]);
                    if (highlight) //Flip Foreground/background
                    {
                        ConsoleColor temp = Console.ForegroundColor;
                        Console.ForegroundColor = Console.BackgroundColor;
                        Console.BackgroundColor = temp;
                        Console.CursorLeft--;
                        Console.Write(" ");
                    }

                }
            }
            if (Console.CursorLeft != 0)
            {
                //Console.CursorLeft = ww - 2;
                if (colorful)
                {
                    bigTemp = Console.ForegroundColor;
                    Console.ForegroundColor = Console.BackgroundColor;
                }
                Console.Write(" ".Repeat(ww - Console.CursorLeft - 2));
                Console.Write(" |");
                if(colorful)
                    Console.ForegroundColor = bigTemp;

                Console.CursorLeft = 0;
            }
            if (verticalPadding)
                ConsoleTableUtil.PrintBlankRow();
            if (verticalPadding)
                ConsoleTableUtil.PrintSeparator();
        }
        public static void PrintBlankRow()
        {
            ConsoleColor temp = ConsoleColor.Red;
            if (colorful)
            {
                temp = Console.ForegroundColor;
                Console.ForegroundColor = Console.BackgroundColor;
            }
            Console.CursorLeft = 0;
            Console.WriteLine("|" + " ".Repeat(Console.WindowWidth - 2) + "|");
            Console.CursorTop--;
            if(colorful)
                Console.ForegroundColor = temp;
        }
        public static void PrintSeparator()
        {
            ConsoleColor temp = ConsoleColor.Red;
            if (colorful)
            {
                temp = Console.ForegroundColor;
                Console.ForegroundColor = Console.BackgroundColor;
            }
            Console.CursorLeft = 0;
            Console.WriteLine("+" + "-".Repeat(Console.WindowWidth - 2) + "+");
            Console.CursorTop--;
            if(colorful)
                Console.ForegroundColor = temp;
        }
    }
}
