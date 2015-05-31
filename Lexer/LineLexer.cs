using System.Collections.Generic;
using TimeLog.Lexer.Matcher;
using TimeLog.Parser;

namespace TimeLog.Lexer
{
    public class LineLexer
    {
        private static IEnumerable<IMatcher> _matchers = new List<IMatcher>
        {
            new DateMatcher(),
            //new LineMatcher(LogParser.TheIdealLine),
            new TimePeriodMatcher(), 
            new ProjectNameMatcher(), 
            new ProjectCommentMatcher(), 
            //new TextMatcher()
        };

        public IEnumerable<Token> Process(int lineNumber, string line)
        {
            var startPos = 0;

            foreach (var matcher in _matchers)
            {
                var token = matcher.Match(lineNumber, startPos, line.Substring(startPos));

                if (token == null) continue;

                startPos += token.Length;

                yield return token;
            }

            yield return new Token(TokenType.EndOfLine, lineNumber, startPos, 0, null);
        }
    }
}
