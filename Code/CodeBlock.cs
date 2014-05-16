using System.Collections.Generic;

using ItzWarty;

namespace ItzWarty.Code
{
    /// <summary>
    /// Implements the most basic aspects of a code block, those which would be necessary
    /// for the syntax highlighter to be happy.
    /// </summary>
    public class CodeBlock : Node
    {
        /// <summary>
        /// Characters associated with the given code block.
        /// </summary>
        public List<TextEditorCharacter> Characters { get; set; }
    }
}