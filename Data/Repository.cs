using System.IO;

namespace TimeLog.Data
{
    public class Repository
    {
        private readonly string _filePathAndName;
        private readonly string _todoFilePathAndName;

        public Repository(string filePath, string todoFilePath)
        {
            _filePathAndName = filePath;
            _todoFilePathAndName = todoFilePath;
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
