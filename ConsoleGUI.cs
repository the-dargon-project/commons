using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItzWarty
{
    /// <summary>
    /// ConsoleGUI supports simple tasks such as drawing a title, messagebox, and ONE MENU at a time.
    /// </summary>
    public static class ConsoleGUI
    {
        //----------------------------
        // Fields
        //----------------------------

        //----------------------------
        // Accessors/Mutators/Properties
        //----------------------------
        public static ConsoleColor BackgroundColor { get; set; }
        public static ConsoleColor BackgroundFontColor  { get; set; }
        public static List<ConsoleGuiElement> Controls { get; private set; }
        public static int[] zbuffer = new int[0];
        public static void Init()
        {
            BackgroundColor = ConsoleColor.Black;
            BackgroundFontColor = ConsoleColor.White;
            Controls = new List<ConsoleGuiElement>();
        }
        public static Label CreateLabel(string text, int x, int y)
        {
            return new Label(text, x, y);
        }

        public static void Render()
        {
            int windowWidth = Console.WindowWidth;
            int windowHeight = Console.WindowHeight;

            if(zbuffer.Length != windowWidth * windowHeight)
                zbuffer = new int[windowWidth * windowHeight];

            //Zero zbuffer. 
            for (int i = 0; i < zbuffer.Length; i++)
                zbuffer[i] = -1;

            foreach (ConsoleGuiElement element in Controls)
                element.Render(ref zbuffer, windowWidth, windowHeight);
        }

        /// <summary>
        /// Console GUI element - most basic class of our console gui, simply a box.
        /// </summary>
        public abstract class ConsoleGuiElement
        {
            public int X { get; set; }
            public int Y { get; set; }
            public virtual int Width { get; set; }
            public virtual int Height { get; set; }
            protected ConsoleGuiElement() : this(0, 0) { }

            protected ConsoleGuiElement(int x, int y) { this.X = x; this.Y = y; }
            public abstract void Render(ref int[] zbuffer, int consoleWidth, int consoleHeight);
        }
        /// <summary>
        /// Label - floating text in our console gui
        /// </summary>
        public class Label : ConsoleGuiElement
        {
            public string Text { get; set; }
            public ConsoleColor ForegroundColor { get; set; }
            public ConsoleColor BackgroundColor { get; set; }
            public Label(string text, int x, int y)
                : base(x, y)
            {
                this.Text = text;
                ForegroundColor = ConsoleColor.White;
                BackgroundColor = ConsoleColor.Black;
                this.FitText();
            }
            /// <summary>
            /// Fits the text to a small box, so nothing is wrapped/cut off
            /// </summary>
            public void FitText()
            {
                string[] lines = Text.Split("\n");
                int longestLine = 0;
                foreach(string line in lines) longestLine = Math.Max(longestLine, line.Length);
                this.Width = longestLine;

                this.Height = lines.Length;
            }
            public override void Render(ref int[] zbuffer, int consoleWidth, int consoleHeight)
            {
                //Basically, we fill our area
                int z = Controls.IndexOf(this) * 6;

                //Move to our top left, start writing
                Console.SetCursorPosition(this.X, this.Y);
                Console.BackgroundColor = this.BackgroundColor;
                Console.ForegroundColor = this.ForegroundColor;

                string[] lines = this.Text.Split("\n");
                bool done = false;
                for (int y = this.Y; y < this.Y + this.Height || !done; y++)
                {
                    string line = "";
                    if (lines.Length > y - this.Y)
                    {
                        line = lines[y - this.Y];
                        done = true;
                    }
                    for (int x = this.X; x < this.X + this.Width || x < this.X + line.Length; x++)
                    {
                        int lineIndex = x - this.X;
                        string c = " ";
                        if (lineIndex >= line.Length) c = " ";
                        else c = line[lineIndex].ToString();

                        {
                            int deltaY = (int)((x - this.X) / (int)this.Width);
                            int curX = (x - this.X) % this.Width + this.X;
                            int curY = y + deltaY;
                            int zbufferOffset = curY * consoleWidth + curX;
                            int zOffset = deltaY;

                            Console.Title = curX.ToString() + " " + curY.ToString();

                            if (zbuffer[zbufferOffset] <= z + zOffset)
                            {
                                Console.SetCursorPosition(curX, curY);
                                Console.Write(c);
                                zbuffer[zbufferOffset] = z + zOffset;
                            }
                        }
                    }
                }
            }
        }
    }
}
