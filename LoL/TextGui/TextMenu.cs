using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SlimDX;
using SlimDX.DirectInput;

namespace ItzWarty.LoL.TextGui
{
    public class TextMenu
    {
        private List<TextEntry> entries = new List<TextEntry>();
        private Keyboard keyboard = null;
        private TextEntry focusedEntry = null;
        private DateTime keydownCooldownEnd = DateTime.Now;
        private Boolean enabled = false;
        public TextMenu(Keyboard keyboard)
        {
            this.keyboard = keyboard;
        }
        public void AddRange(TextEntry[] entries)
        {
            this.entries.AddRange(entries);
        }
        public void PrintToConsole()
        {
            foreach (TextEntry e in entries)
                e.PrintToConsole();
        }
        public string GetTextOutput()
        {
            StringBuilder final = new StringBuilder();
            foreach (TextEntry e in entries)
                final.Append(e.GetOutput());
            return final.ToString();
        }
        public void Manage()
        {
            if (focusedEntry == null)
                for (int i = 0; i < entries.Count && focusedEntry == null; i++)
                    if (entries[i].HasOnSelectedHandler())
                        focusedEntry = entries[i];

            focusedEntry.Focus();

            KeyboardState ks = keyboard.GetCurrentState();
            if(enabled)
            {
                if (keydownCooldownEnd > DateTime.Now)
                {
                    if (!ks.PressedKeys.Contains(Key.LeftArrow) &&
                        !ks.PressedKeys.Contains(Key.DownArrow) &&
                        !ks.PressedKeys.Contains(Key.RightArrow) &&
                        !ks.PressedKeys.Contains(Key.UpArrow)
                    )
                    {
                        keydownCooldownEnd = DateTime.Now;
                    }
                }
                else
                {
                    keydownCooldownEnd = DateTime.Now + new TimeSpan(0, 0, 0, 0, 100);
                    if (ks.PressedKeys.Contains(Key.LeftArrow))
                    {
                        #region lArrow
                        focusedEntry.Blur();

                        int beforeSelectedIndex = entries.IndexOf(focusedEntry);
                        //Try to find something before, but on the same newline

                        int startLineIndex = 0;
                        for (int i = beforeSelectedIndex; i >= 0 && startLineIndex == 0; i--)
                            if (entries[i] is EntryNewline)
                                startLineIndex = i + 1;

                        int endLineIndex = entries.Count;
                        //Try to find the newline that ends this line
                        for (int i = beforeSelectedIndex; i < entries.Count && endLineIndex == entries.Count; i++)
                            if (entries[i] is EntryNewline)
                                endLineIndex = i - 1;

                        TextEntry beforeEntry = focusedEntry;
                        //Find a selectable textentry before our current entry
                        for (int i = beforeSelectedIndex - 1; i > startLineIndex && beforeEntry == focusedEntry; i--)
                            if (entries[i].HasOnSelectedHandler())
                                beforeEntry = entries[i];

                        beforeEntry.Focus();
                        focusedEntry = beforeEntry;
                        #endregion
                    }
                    else if (ks.PressedKeys.Contains(Key.RightArrow))
                    {
                        #region rArrow
                        focusedEntry.Blur();

                        int beforeSelectedIndex = entries.IndexOf(focusedEntry);
                        //Try to find something before, but on the same newline

                        int startLineIndex = 0;
                        for (int i = beforeSelectedIndex; i >= 0 && startLineIndex == 0; i--)
                            if (entries[i] is EntryNewline)
                                startLineIndex = i + 1;

                        int endLineIndex = entries.Count;
                        //Try to find the newline that ends this line
                        for (int i = beforeSelectedIndex; i < entries.Count && endLineIndex == entries.Count; i++)
                            if (entries[i] is EntryNewline)
                                endLineIndex = i - 1;

                        TextEntry beforeEntry = focusedEntry;
                        //Find a selectable textentry after our current entry
                        for (int i = beforeSelectedIndex + 1; i < endLineIndex && beforeEntry == focusedEntry; i++)
                            if (entries[i].HasOnSelectedHandler())
                                beforeEntry = entries[i];

                        beforeEntry.Focus();
                        focusedEntry = beforeEntry;
                        #endregion
                    }
                    else if (ks.PressedKeys.Contains(Key.DownArrow))
                    {
                        #region dArrow
                        focusedEntry.Blur();

                        int beforeSelectedIndex = entries.IndexOf(focusedEntry);
                        //Try to find something before, but on the same newline

                        int startLineIndex = 0;
                        for (int i = beforeSelectedIndex; i >= 0 && startLineIndex == 0; i--)
                            if (entries[i] is EntryNewline)
                                startLineIndex = i + 1;

                        int endLineIndex = entries.Count;
                        //Try to find the newline that ends this line
                        for (int i = beforeSelectedIndex; i < entries.Count && endLineIndex == entries.Count; i++)
                            if (entries[i] is EntryNewline)
                                endLineIndex = i - 1;

                        TextEntry newFocusedEntry = focusedEntry;
                        //Find a selectable textentry after our current entry's proceeding newline
                        for (int i = endLineIndex + 1; i < entries.Count && newFocusedEntry == focusedEntry; i++)
                            if (entries[i].HasOnSelectedHandler())
                                newFocusedEntry = entries[i];

                        //Find the first selectable textentry of that line, and count n entries, where n is equal to our position of selection in our line
                        //As in, the first selectable entry of a line is 0, the next is 1, etc...

                        int newStartLineIndex = 0;
                        for (int i = entries.IndexOf(newFocusedEntry); i >= 0 && newStartLineIndex == 0; i--)
                            if (entries[i] is EntryNewline)
                                newStartLineIndex = i + 1;

                        int newEndLineIndex = entries.Count;
                        //Try to find the newline that ends this line
                        for (int i = entries.IndexOf(newFocusedEntry); i < entries.Count && newEndLineIndex == entries.Count; i++)
                            if (entries[i] is EntryNewline)
                                newEndLineIndex = i - 1;

                        //Count how many selectable textentries led up to us before...
                        int beforePosition = 0;
                        for (int i = startLineIndex; i < beforeSelectedIndex; i++)
                            if (entries[i].HasOnSelectedHandler()) beforePosition++;

                        //Find the entry on our line who has the same position as us... default to the last one if it doesn't exist
                        int positionCount = beforePosition;
                        TextEntry lastSelectable = null;
                        for (int i = newStartLineIndex; i < newEndLineIndex && positionCount >= 0; i++)
                            if (entries[i].HasOnSelectedHandler())
                            {
                                positionCount--;
                                lastSelectable = entries[i];
                            }

                        newFocusedEntry = lastSelectable;
                        newFocusedEntry.Focus();
                        focusedEntry = newFocusedEntry;
                        #endregion
                    }
                    else if (ks.PressedKeys.Contains(Key.UpArrow))
                    {
                        #region uArrow
                        focusedEntry.Blur();

                        int beforeSelectedIndex = entries.IndexOf(focusedEntry);
                        //Try to find something before, but on the same newline

                        int startLineIndex = 0;
                        for (int i = beforeSelectedIndex; i >= 0 && startLineIndex == 0; i--)
                            if (entries[i] is EntryNewline)
                                startLineIndex = i + 1;

                        int endLineIndex = entries.Count;
                        //Try to find the newline that ends this line
                        for (int i = beforeSelectedIndex; i < entries.Count && endLineIndex == entries.Count; i++)
                            if (entries[i] is EntryNewline)
                                endLineIndex = i - 1;

                        TextEntry newFocusedEntry = focusedEntry;
                        //Find a selectable textentry after our current entry's proceeding newline
                        for (int i = startLineIndex - 1; i > 0 && newFocusedEntry == focusedEntry; i--)
                            if (entries[i].HasOnSelectedHandler())
                                newFocusedEntry = entries[i];

                        //Find the first selectable textentry of that line, and count n entries, where n is equal to our position of selection in our line
                        //As in, the first selectable entry of a line is 0, the next is 1, etc...

                        int newStartLineIndex = 0;
                        for (int i = entries.IndexOf(newFocusedEntry); i >= 0 && newStartLineIndex == 0; i--)
                            if (entries[i] is EntryNewline)
                                newStartLineIndex = i + 1;

                        int newEndLineIndex = entries.Count;
                        //Try to find the newline that ends this line
                        for (int i = entries.IndexOf(newFocusedEntry); i < entries.Count && newEndLineIndex == entries.Count; i++)
                            if (entries[i] is EntryNewline)
                                newEndLineIndex = i - 1;

                        //Count how many selectable textentries led up to us before...
                        int beforePosition = 0;
                        for (int i = startLineIndex; i < beforeSelectedIndex; i++)
                            if (entries[i].HasOnSelectedHandler()) beforePosition++;

                        //Find the entry on our line who has the same position as us... default to the last one if it doesn't exist
                        int positionCount = beforePosition;
                        TextEntry lastSelectable = null;
                        for (int i = newStartLineIndex; i < newEndLineIndex && positionCount >= 0; i++)
                            if (entries[i].HasOnSelectedHandler())
                            {
                                positionCount--;
                                lastSelectable = entries[i];
                            }

                        newFocusedEntry = lastSelectable;
                        newFocusedEntry.Focus();
                        focusedEntry = newFocusedEntry;
                        #endregion
                    }
                    else if (ks.PressedKeys.Contains(Key.Space))
                    {
                        focusedEntry.InvokeOnSelected();
                    }
                    else
                    {
                        keydownCooldownEnd = DateTime.Now;
                    }
                }
            }
        }
        public void SetEnabled(Boolean b) { this.enabled = b; }
    }
}
