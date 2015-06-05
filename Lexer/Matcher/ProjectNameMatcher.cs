using System.Text.RegularExpressions;

namespace TimeLog.Lexer.Matcher
{
    public class ProjectNameMatcher : IMatcher
    {
        public Token Match(int lineNumber, int startPosition, string value)
        {
            // project name always comes after time period
            if (startPosition == 0) return null;

            string project;

            return IsProject(value, out project)
                ? new Token(TokenType.ProjectName, lineNumber, startPosition, project.Length, project.Trim())
                : null;
        }

        private bool IsProject(string line, out string result)
        {
            result = string.Empty;

            var l = line;
            var match = _project.Match(l);

            if (!match.Success) return false;

            result = match.Groups[1].Value;
            return true;
        }

        private readonly Regex _project = new Regex(@"^([\s]*.*?)\.(.*)$", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
    }
}