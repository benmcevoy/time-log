using System.IO;
using System.Configuration;

namespace TimeLog
{
    public class Repository
    {
        private readonly string _filePathAndName;
        private readonly string _todoFilePathAndName;

        public Repository()
        {
            _filePathAndName = ConfigurationManager.AppSettings["filePath"];
            _todoFilePathAndName = ConfigurationManager.AppSettings["todoFilePath"];
        }

        public void Save(string log)
        {
            File.WriteAllText(_filePathAndName, log);            
        }

        public void SaveToDo(string log)
        {
            File.WriteAllText(_todoFilePathAndName, log);
        }

        public string Load()
        {
            return File.ReadAllText(_filePathAndName);
        }

        public string LoadToDo()
        {
            return File.ReadAllText(_todoFilePathAndName);
        }
    }
}
