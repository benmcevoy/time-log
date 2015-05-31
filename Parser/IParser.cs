using System.Collections.Generic;
using TimeLog.Model;

namespace TimeLog.Parser
{
    public interface IParser
    {
        Log Parse(IEnumerable<string> lines);
    }
}
