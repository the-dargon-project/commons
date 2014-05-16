using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItzWarty.LoL.TextGui
{
    public delegate void OnEntrySelected();
    public class TextEntry
    {
        private Boolean isFocused = false;
        private string value = "";
        public TextEntry() { }
        public TextEntry(string value)
        {
            this.value = value;
        }
        public event OnEntrySelected OnSelected;
        public void InvokeOnSelected()
        {
            if (OnSelected != null) OnSelected();
        }
        public bool HasOnSelectedHandler() { return OnSelected != null; }
        public void PrintToConsole()
        {
            if (this.isFocused)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else
            {
                if (this.OnSelected == null)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            if (this.isFocused)
                Console.Write("[" + this.value + "]");
            else
                if (this.OnSelected == null)
                    Console.Write(this.value);
                else
                Console.Write(" " + this.value + " ");
        }
        public string GetOutput()
        {
            if (this.isFocused)
            {
                return "[" + this.value + "]";
            }
            else
            {
                if (this.OnSelected == null)
                    return this.value;
                else
                    return " " + this.value + " ";
            }
        }
        public void Focus() { this.isFocused = true; }
        public void Blur() { this.isFocused = false; }
        public void SetText(string s) { this.value = s; }
    }
}
