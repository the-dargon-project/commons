using System;
using System.Collections.Generic;
using System.Drawing;

namespace ItzWarty.Code
{
    public interface ISyntaxHighlighter
    {
        /// <summary>
        /// Resets the syntax highlighter to the state it was at when first constructed.
        /// </summary>
        void Reset();

        /// <summary>
        /// Gets the color to color the given character with.
        /// 
        /// This color is effected by previous state.
        /// </summary>
        Brush GetForegroundBrush(char c);

        /// <summary>
        /// Removes comments from the given code, which will slow down the parser, and
        /// potentially slow down performance due to things such as tokenization and
        /// token type identification.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Object> RemoveComments();

        /// <summary>
        /// Passes through the content of the document, tokenizing it, for analysis later on.
        /// </summary>
        IEnumerable<Object> Tokenize();

        /// <summary>
        /// Finds the globally accessible entities in the given document, and groups them into
        /// blocks, which are analyzed later on.  This is done through grouping our tokens.
        /// 
        /// In c++, this is the equivalent of scanning a document for the method headers, which
        /// can be used later on.
        /// </summary>
        IEnumerable<Object> GroupBlocks();

        /// <summary>
        /// Passes through a block and identifies its tokens
        /// </summary>
        IEnumerable<Object> AnalyzeBlock();

        /// <summary>
        /// Saves the state of the Syntax Highlighter.  This is necessary as we don't want to parse
        /// from the top of a document every single time a change is made.  Instead, we'll take a
        /// snapshot of the state of the syntax highlighter every once in a while.
        /// </summary>
        /// <returns></returns>
        object SaveSyntaxHighlighterStateSnapshot();
    }
}
