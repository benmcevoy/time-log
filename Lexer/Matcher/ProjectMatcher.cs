using System.Text.RegularExpressions;

namespace TimeLog.Lexer.Matcher
{
    public class ProjectNameMatcher : IMatcher<string>
    {
        public Token<string> Match(int lineNumber, int startPosition, string value)
        {
            string project;

            return IsProject(value, out project)
                ? new Token<string>(TokenType.ProjectName, lineNumber, startPosition, 100, project)
                : null;
        }

        private bool IsProject(string line, out string result)
        {
            result = string.Empty;

            var l = line.Trim();
            var match = _project.Match(l);

            if (!match.Success) return false;

            result = match.Groups[1].Value;
            return true;
        }

        private readonly Regex _project = new Regex(
@"^[012]?[0-9]:[0-5][0-9]
-
[012]?[0-9]:[0-5][0-9]
[\s|\t]*
(.*?)\.(.*)$", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
    }
}