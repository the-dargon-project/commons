using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;

namespace ItzWarty.wSL
{
    public static class Tokenizer
    {
        private static Regex numberRegex = new Regex("/[0-9,x]+/");
        private static Regex nameRegex = new Regex("/[a-z,A-Z][a-z,A-Z,0-9]+/");
        private static string[] keywords = new string[]{
            "function",
            "new", "delete",
            "throw", "catch",
            "public", "private", "protected", "static"
        };
        private static string[] operators = new string[]{
            "-", "+", "*", "/", "%", "^", "<", ">", "=", "|", "&",
            ">=", "<=", "==", "|=", "&=", "(", ")", ".", "[", "]",
            ":", 
            ".", "::", "->"
        };
        private static string[] identifierEnders = new string[]{
            ";", " "
        }.Concat(operators).ToArray();

        public static List<Token> Tokenize(string _statement)
        {
            List<char> untokenized = new List<char>(_statement.ToCharArray());

            string currentToken = "";
            TokenType currentType = 0;
            List<Token> finalTokens = new List<Token>();

            while (untokenized.Count != 0)
            {
                switch (currentType)
                {
                    #region TokenType.undefined
                    case TokenType.undefined:
                        switch (resolveTokenType(untokenized[0]))
                        {
                            case TokenType.CBC:
                                finalTokens.Add(new Token(TokenType.CBC, "}"));
                                untokenized.RemoveAt(0);
                                break;
                            case TokenType.CBO:
                                finalTokens.Add(new Token(TokenType.CBO, "{"));
                                untokenized.RemoveAt(0);
                                break;
                            case TokenType.Comma:
                                finalTokens.Add(new Token(TokenType.Comma, ","));
                                untokenized.RemoveAt(0);
                                break;
                            case TokenType.Semicolon:
                                finalTokens.Add(new Token(TokenType.Semicolon, ";"));
                                untokenized.RemoveAt(0);
                                break;
                            case TokenType.Operator:
                                if (operators.Contains(untokenized[0].ToString()) && operators.Contains(untokenized[1].ToString()))
                                {
                                    //it must be a 2-size operator
                                    finalTokens.Add(new Token(TokenType.Operator, untokenized[0].ToString() + untokenized[1].ToString()));
                                    untokenized.RemoveRange(0, 2);
                                }
                                else
                                {
                                    finalTokens.Add(new Token(TokenType.Operator, new String(new char[]{untokenized[0]})));
                                    untokenized.RemoveAt(0);
                                }
                                break;
                            default:
                                currentType = resolveTokenType(untokenized[0]);
                                currentToken = untokenized[0].ToString();
                                untokenized.RemoveAt(0);
                                break;
                        }
                        break;
                    #endregion
                    #region TokenType.CharLiteral
                    case TokenType.CharLiteral:
                    {
                        char currentPart = untokenized[0];
                        switch (currentPart)
                        {
                            case '\'':
                                currentToken += "'";
                                finalTokens.Add(new Token(TokenType.CharLiteral, currentToken));
                                currentType = TokenType.undefined;
                                untokenized.RemoveAt(0); 
                                break;
                            case '\\':
                                switch (untokenized[1])
                                {
                                    case 'n':
                                        currentToken += '\n';
                                        untokenized.RemoveRange(0, 2);
                                        break;
                                    case 'r':
                                        currentToken += '\r';
                                        untokenized.RemoveRange(0, 2);
                                        break;
                                    case '\'':
                                        currentToken += '\'';
                                        untokenized.RemoveRange(0, 2);
                                        break;
                                    default:
                                        currentToken += "?";
                                        untokenized.RemoveAt(0);
                                        break;
                                }
                                break;
                            default:
                                currentToken += untokenized[0];
                                untokenized.RemoveAt(0);
                                break;
                        }
                        break;
                    }
                    #endregion
                    #region TokenType.StringLiteral
                    case TokenType.StringLiteral:
                    {
                        char currentPart = untokenized[0];
                        switch (currentPart)
                        {
                            case '\"':
                                currentToken += "'";
                                finalTokens.Add(new Token(TokenType.StringLiteral, currentToken));
                                currentType = TokenType.undefined;
                                untokenized.RemoveAt(0);
                                break;
                            case '\\':
                                switch (untokenized[1])
                                {
                                    case 'n':
                                        currentToken += '\n';
                                        untokenized.RemoveRange(0, 2);
                                        break;
                                    case 'r':
                                        currentToken += '\r';
                                        untokenized.RemoveRange(0, 2);
                                        break;
                                    case '\'':
                                        currentToken += '\'';
                                        untokenized.RemoveRange(0, 2);
                                        break;
                                    case '\"':
                                        currentToken += '\"';
                                        untokenized.RemoveRange(0, 2);
                                        break;
                                    default:
                                        currentToken += "?";
                                        untokenized.RemoveAt(0);
                                        break;
                                }
                                break;
                            default:
                                currentToken += untokenized[0];
                                untokenized.RemoveAt(0);
                                break;
                        }
                        break;
                    }
                    #endregion
                    #region TokenType.Identifier
                    case TokenType.Identifier:
                    {
                        char currentPart = untokenized[0];
                        if (identifierEnders.Contains(currentPart.ToString()))
                        {
                            //end identifier
                            finalTokens.Add(new Token(TokenType.Identifier, currentToken));
                            currentType = TokenType.undefined;
                        }
                        else
                        {
                            currentToken += currentPart;
                            untokenized.RemoveAt(0);
                        }
                        break;
                    }
                    #endregion
                    case TokenType.Keyword: { break; }  //This won't happen. keywords are identified as
                                                        //Identifiers and "renamed" later on...
                    #region TokenType.NumberLiteral
                    case TokenType.NumberLiteral:
                    {
                        char currentPart = untokenized[0];
                        if (numberRegex.Matches(currentPart.ToString()).Count == 1)
                        {
                            currentToken += currentPart;
                        }
                        else
                        {
                            finalTokens.Add(new Token(TokenType.NumberLiteral, currentToken));
                            currentType = TokenType.undefined;
                        }
                        break;
                    }
                    #endregion
                }
            }
            return finalTokens;
        }
        private static TokenType resolveTokenType(char c)
        {
            if (c == '}') return TokenType.CBC;
            else if (c == '{') return TokenType.CBO;
            else if (c == ',') return TokenType.Comma;
            else if (numberRegex.Matches(c.ToString()).Count == 1) return TokenType.NumberLiteral;
            else if (operators.Contains(c.ToString())) return TokenType.Operator;
            else if (c == ';') return TokenType.Semicolon;
            else if (c == '\"') return TokenType.StringLiteral;
            else if (c == '\'') return TokenType.CharLiteral;
            else return TokenType.Identifier;
        }
        private static string StripComments(string s)
        {
            string[] lines = s.Split("\n");
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                bool inSQuote = false;
                bool inDQuote = false;
            }
            return "";
        }
    }
}
