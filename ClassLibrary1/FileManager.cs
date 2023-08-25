namespace FilesAndRegex
{
    public class FileManager
    {
        public FileInfo[] GetAllFiles(string path, string wildcard = "*")
        {
            var folder = new DirectoryInfo(path);
            var files = folder.GetFiles(wildcard, SearchOption.AllDirectories);
            return files;
        }
    }
}