using System.Collections.Generic;
using System.Drawing;

namespace ItzWarty.Code
{
    public class DefaultSyntaxHighlighter : ISyntaxHighlighter
    {
        /// <summary>
        /// This is the color which all text passed through this syntax highlighter is colored
        /// with.  The default syntax highlighter is something you'd use for something like a
        /// text file.  
        /// </summary>
        public Brush ForegroundBrush { get; set; }

        /// <summary>
        /// Creates a new DefaultSyntaxHighlighter that returns silver
        /// </summary>
        public DefaultSyntaxHighlighter() : this(Brushes.Silver) { }

        /// <summary>
        /// Creates a new DefaultSyntaxHighlighter that returns the given color
        /// </summary>
        /// <param name="c">The new foreground color</param>
        public DefaultSyntaxHighlighter(Brush brush)
        {
            ForegroundBrush = brush;
        }

        /// <summary>
        /// Resets the default syntax highlighter - does nothing, as this syntax highlighter 
        /// always returns a given color.
        /// </summary>
        public void Reset()
        {
        }

        /// <summary>
        /// The default syntax highlighter returns black 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public Brush GetForegroundBrush(char c)
        {
            return ForegroundBrush;
        }

        public IEnumerable<object> RemoveComments()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<object> Tokenize()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<object> GroupBlocks()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<object> AnalyzeBlock()
        {
            throw new System.NotImplementedException();
        }

        public object SaveSyntaxHighlighterStateSnapshot()
        {
            throw new System.NotImplementedException();
        }
    }
}
