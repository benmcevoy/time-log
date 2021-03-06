﻿using System.Linq;

namespace TimeLog.Lexer.Matcher
{
    public class LineMatcher : IMatcher
    {
        private readonly string _idealLine;

        public LineMatcher(string idealLine)
        {
            _idealLine = idealLine;
        }

        public Token Match(int lineNumber, int startPosition, string value)
        {
            string line;

            return IsDateLineSecondLine(value, out line)
                ? new Token(TokenType.Line, lineNumber, startPosition, value.Length, line) 
                : null;
        }

        private bool IsDateLineSecondLine(string line, out string result)
        {
            result = _idealLine;
            return (line.All(l => l == '-') && line.Length > 5);
        }
    }
}