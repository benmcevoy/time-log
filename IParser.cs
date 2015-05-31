using System.Collections.Generic;

namespace TimeLog
{
    public interface IParser
    {
        Log Parse(IEnumerable<string> lines);
    }
}
