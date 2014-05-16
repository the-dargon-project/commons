using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItzWarty.Code
{
    /// <summary>
    /// Acts as a pointer to a struct.
    /// </summary>
    public class CodeToken
    {
        /// <summary>
        /// Index of the start of the token in our code document's list
        /// of text editor characters
        /// </summary>
        public int StartIndex;

        /// <summary>
        /// Index of the end of the token in our code document's list of
        /// text editor characters.
        /// </summary>
        public int EndIndex;

        /// <summary>
        /// Foreground brush
        /// </summary>
        public Brush ForegroundBrush;
    }
}
