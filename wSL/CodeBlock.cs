using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItzWarty.wSL
{
    public class CodeBlock:Token
    {
        Token[] containedTokens = null;
        public CodeBlock(Token[] containedTokens): base(TokenType.CodeBlock, "")
        {
            this.containedTokens = containedTokens;
        }
        public Token[] ContainedTokens
        {
            get
            {
                return containedTokens;
            }
        }
    }
}
