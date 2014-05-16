using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItzWarty.wSL
{
    public enum TokenType : byte
    {
        undefined = 0,
        StringLiteral,
        CharLiteral,
        NumberLiteral,
        Operator,
        Keyword,
        Semicolon,
        Comma,
        CBO,            //Curly bracket open/close
        CBC,
        Identifier,
        CodeBlock       //For the CodeBlock type
    }
    public class Token
    {
        private TokenType type;
        private string value;
        public Token(TokenType type, string value)
        {
            this.type = type;
            this.value = value;
        }
        public TokenType Type { get { return type; } }
        public string Value { get { return value; } }
    }
}
