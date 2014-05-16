using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using ItzWarty;

namespace ItzWarty.Code
{
    /// <summary>
    /// Textbox control which supports lots of features such as block selection
    /// </summary>
    public sealed class WRichTextBox : Control
    {
        /// <summary>
        /// Descriptor for how the user is currently selecting stuff
        /// 
        /// It should be noted that x=0 is to the left of the first character.
        /// 
        /// Flags of higher value take precedence over lower flags.
        /// </summary>
        internal enum CursorMode
        {
            /// <summary>
            /// In point selection mode, we're working as an editor that has no text selected, with
            /// "insertion mode" disabled.  In this case, we simply move the cursor around, 
            /// respecting line and document boundaries.
            /// 
            /// When the user types, we insert a character at the given location and shift
            /// everything to the right, unless the key is return or backspace.
            /// 
            /// For rendering, we simply draw a line character at the startX/startY position
            /// 
            /// Mode Switching:
            /// If insert is pressed, then we set Insert mode.
            /// else if shift+alt are pressed, and we move, we set Block Selection Mode.
            /// else If shift is pressed, and we move, then we set Line selection mode.
            /// 
            /// This is our default selection mode.
            /// </summary>
            Point = 0x01,
            
            /// <summary> 
            /// When the user types, we swap the character at the cursor's location with our new
            /// character, unless the key is something like return or backspace.
            /// 
            /// For rendering, we simply draw a block at the startX/startY position
            /// 
            /// Insertion mode falls back to Line mode for anything else.
            /// 
            /// Mode Switching:
            /// If insert is pressed, then we unset Insert mode.
            /// else if shift+alt are pressed, and we move, we set Block Selection Mode.
            /// else If shift is pressed, and we move, then we set Line selection mode.
            /// </summary>
            Insert = 0x04,

            /// <summary>
            /// If we are not in selection mode, then we fall back to Point procedures.
            /// 
            /// If we are in selection mode, we begin by storing the start location at the cursor.
            /// When left/right arrow keys are pressed, we decrement or increment the X value of
            /// the end position.  If the end position is beyond the end of a line, or before it,
            /// then we change Y's value.  
            /// 
            /// For rendering, we fill along every character until we reach our end.
            /// 
            /// When the user types, we replace the selected block with the user's value.  If we
            /// are not in selection mode, we fall back to Point mode.
            /// 
            /// Mode Switching: 
            /// If insert is pressed, then we flip Insert mode, though we still take precedence
            /// else if shift+alt are pressed, and we move, we set Block Selection Mode and we
            ///                                             unset Line Selection mode.
            /// else if shift is not pressed, and we move, then we unset Line selection mode and
            ///    if the key is up or down, we move based on the end position
            ///    else if the key is left, we move to the minor endpoint of selection
            ///    else if the key is right, we move to the major endpoint of selection
            /// 
            /// The major endpoint of selection is defined as the endpoint with the greatest Y, and
            ///   if two Y are equal, we pick the larger x.
            /// 
            /// The minor endpoint of selection is the other endpoint.
            /// </summary>
            Line = 0x10,

            /// <summary>
            /// If we are not in selection mode, we fall back to Point procedures.
            /// 
            /// If we are in selection mode, we we begin by storing the start location of the
            /// cursor, and store that position at the end location as well.  When the user
            /// moves the cursor, they're moving the end position, which CAN extend beyond the
            /// end of a line.
            /// 
            /// For rendering, we simply draw a rectangle showing the selected block selection.
            /// 
            /// When the user types, we're typing to every line in our block selection.
            /// 
            /// Mode Switching;
            /// If insert is pressed, then we flip Insert mode, though we still take precedence
            /// 
            /// else if shift is not pressed, and we move, then we unset Block selection mode and
            ///    if the key is up or down, we move based on the end position
            ///    else if the key is left, we move to the minor endpoint of selection
            ///    else if the key is right, we move to the major endpoint of selection
            /// 
            /// The major endpoint of selection is defined as the endpoint with the greatest Y, and
            ///   if two Y are equal, we pick the larger x.
            /// 
            /// The minor endpoint of selection is the other endpoint.
            /// </summary>
            Block = 0x40
        }
        /// <summary>
        /// The maximum length (in characters) of a line.
        /// </summary>
        public static readonly int kMaximumLineLength = 1024;

        //-----------------------------------------------------------------------------------------
        // Properties and Public Fields and their Back Ends
        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Background color of our control
        /// </summary>
        public Color BackgroundColor = Color.FromArgb(255, 30, 30, 30);

        /// <summary>
        /// The line number of the topmost line, where lines are zero-indexed.
        /// </summary>
        public int ScrollY = 0;

        /// <summary>
        /// The syntax highlighter, which is returned by ISyntaxHighlighter
        /// </summary>
        private ISyntaxHighlighter m_syntaxHighlighter = new DefaultSyntaxHighlighter(Brushes.LightGray);

        /// <summary>
        /// Whether or not the WRichTextbox is working as a code editor
        /// Code editing mode only works 
        /// </summary>
        [DefaultValue(false)]
        public ISyntaxHighlighter SyntaxHighlighter { get { return m_syntaxHighlighter; } set { m_syntaxHighlighter = value; } }

        //-----------------------------------------------------------------------------------------
        // Private Fields
        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// The content of our textbox.  It is filled with character arrays of size kMaximumLineLength
        /// When the user tries to write beyond that, we tell them they can't do that.
        /// </summary>
        private List<char[]> m_lines = new List<char[]>();

        /// <summary>
        /// Tokens for our syntax highlighter
        /// </summary>
        private List<string> m_tokens = new List<string>();

        /// <summary>
        /// Describes the current way the user is working with text
        /// </summary>
        private CursorMode m_cursorMode = CursorMode.Point;
        
        //Descriptor for our current text selection.  We interact with text based on this and the m_cursorMode
        private int m_selectionStartX = 0;
        private int m_selectionStartY = 0;
        private int m_selectionEndX = 1;
        private int m_selectionEndY = 1;

        //-----------------------------------------------------------------------------------------
        // Rendering-specific stuff
        //-----------------------------------------------------------------------------------------
        private Bitmap m_backbuffer;
        private Graphics m_backbufferG;

        /// <summary>
        /// Creates a new WRichTextbox, which offers numerous features such as syntax highlighting,
        /// block selection.
        /// </summary>
        public WRichTextBox()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            //Set our default font.  Note that fonts must be monospace
            Font = new Font("Consolas", 12.0f, FontStyle.Regular, GraphicsUnit.Pixel);
            m_lines.Add("This is line one".ToCharArray());
            m_lines.Add("This is line two".ToCharArray());

            ManageBackbuffer();
        }

        /// <summary>
        /// Writes the given context to the textbox
        /// </summary>
        private static void WriteContent(string content)
        {
            var lines = from candidate in content.Split("\n")
                        select candidate.Trim();

            foreach (string line in lines)
            {

            }
        }

        /// <summary>
        /// Paint event handler - draws our content to a backbuffer, and then displays the content
        /// to the given render target.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            Console.WriteLine("wRichTextbox Redraw requested at rectangle " + e.ClipRectangle);
            //For debugging purposes
#if DEBUG
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            //Top left of our drawing region
            Point topLeft = e.ClipRectangle.Location;
            SuspendLayout();

            //Current location of where we're drawing
            Point currentLocation = topLeft; //Note point is struct, so is copy
            int startY = ScrollY + e.ClipRectangle.Top / MeasureCellHeight(); //Division would round down
            Console.WriteLine(e.ClipRectangle.Top);
            int endY = ScrollY + e.ClipRectangle.Bottom / MeasureCellHeight(); //Division would round down
            for (int i = Math.Max(0, startY - 1); i < endY + 1 && i < m_lines.Count; i++)
            {
                char[] chars = m_lines[i];
                for(int j = 0; j < chars.Length; j++)
                {
                    //Essentially, tokenize
                    e.Graphics.DrawString(chars[j].ToString(), Font, Brushes.LightGray, 
                        new Point(
                            j * (MeasureCellWidth()),
                            (i - ScrollY) * MeasureCellHeight()
                        )
                    );
                }
            }
            ResumeLayout(false);
#if DEBUG
            Console.WriteLine("WRichTextBox finished rendering after " + stopwatch.Elapsed.TotalMilliseconds + "ms");
#endif
        }

        /// <summary>
        /// Paints the background of our control
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using(SolidBrush brush = new SolidBrush(BackgroundColor))
                e.Graphics.FillRectangle(
                    brush,
                    e.ClipRectangle
                );
        }

        //-----------------------------------------------------------------------------------------
        // Helper Methods
        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// Manages the backbuffer, creating it if it doesn't exist, and resizing it
        /// if it's too small.
        /// </summary>
        private void ManageBackbuffer()
        {
            //int maximumLineWidth = (from count in m_lines
            //                        select m_lines.Count).Max();
            //Size desiredSize = new Size(
            //    maximumLineWidth * MeasureCellWidth()
            //    m_lines.Count * MeasureCellHeight()
            //)
            m_backbuffer = new Bitmap(kMaximumLineLength * MeasureCellWidth(), 1024);
            m_backbufferG = Graphics.FromImage(m_backbuffer);
        }

        #region Helper Methods - Measuring Cells
        public int MeasureVisibleVerticalCells()
        {
            return Height / MeasureCellHeight();
        }

        /// <summary>
        /// Calculates how many horizontal cells can be seen at a given line
        /// </summary>
        public void MeasureVisibleHorizontalCells()
        {

        }

        /// <summary>
        /// Calculates the width of a single cell on our grid
        /// </summary>
        private int MeasureCellWidth()
        {
            return MeasureCell().Width;
        }

        /// <summary>
        /// Calculates the height of a single cell on our grid
        /// </summary>
        private int MeasureCellHeight()
        {
            return MeasureCell().Height;
        }

        private Font m_lsMeasureCellMeasuredFont = null;
        private Size m_lsMeasureCellCellSize = Size.Empty;
        /// <summary>
        /// Measures the width and height of a cell on our grid
        /// </summary>
        private Size MeasureCell()
        {
            if (m_lsMeasureCellMeasuredFont == Font)
            {
                return m_lsMeasureCellCellSize;
            }
            else
            {
                //We want to measure the size of a character, without padding.
                // If we measure @@ then we get that, with padding .....
                //               @@                                .@.@.
                //                                                 .....
                //                                                 .@.@.
                //                                                 .....
                // If we expand a bit, we get ....... so we get a delta size
                //                            .@.@.@. of a character plus right padding
                //                            .......
                //                            .@.@.@.
                //                            .......
                //
                Graphics g = CreateGraphics();
                //Size measureString = m_lsMeasureCellCellSize = g.MeasureString("@", Font).ToSize();
                //Size size = TextRenderer.MeasureText("@", Font);
                //System.Console.WriteLine(measureString + " " + size);
                StringFormat format = new StringFormat();
                format.SetMeasurableCharacterRanges(new CharacterRange[]{new CharacterRange(0, 1)});
                Region[] regions = g.MeasureCharacterRanges("@", Font, new RectangleF(0, 0, 1234, 1234), format);
                m_lsMeasureCellCellSize = regions[0].GetBounds(g).Size.ToSize();
                g.Dispose();
                m_lsMeasureCellMeasuredFont = Font;
                return m_lsMeasureCellCellSize;
            }
        }
        #endregion
    }
}
