using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FilesAndRegex
{
    public class ReadTextInFiles
    {
        public string path;
        private FileManager fm;

        public Logger Log { get; private set; }

        public ReadTextInFiles(string path)
        {
            this.path = path;
            this.fm = new FileManager();
            Log = new Logger();
        }

        public IEnumerable<TextFileDict> GetAllTextFiles(string wildcard = ".txt")
        {
            var files = fm.GetAllFiles(path, wildcard);
            foreach (var file in files)
            {
                using (var streamReader = file.OpenText())
                {
                    string text = streamReader.ReadToEnd();
                    var fileDict = new TextFileDict(file) {  Text = text };
                    Log.Log($"file | {file.DirectoryName} | {file.Name}");
                    yield return fileDict;
                }
            }
        }

        public IEnumerable<TextFileDict> ApplyRegexToTextFilesDict(IEnumerable<TextFileDict> textFiles, Func<string, string> regexAction)
        {
            if (textFiles != null)
            { 
                foreach (var file in textFiles)
                {
                    var newFile = new TextFileDict(file.File) { Text = regexAction?.Invoke(file?.Text) };
                    if (file.ChildFiles != null)
                        newFile.ChildFiles = ApplyRegexToTextFilesDict(file.ChildFiles, regexAction);
                    var textInFiles = newFile.Text.Split("\n").Where(s => !string.IsNullOrWhiteSpace(s));
                    if(textInFiles != null && !string.IsNullOrWhiteSpace(newFile.Text))
                        foreach (string line in textInFiles)
                            if(!string.IsNullOrWhiteSpace(line))
                            Log.Log($"line | {newFile.File.DirectoryName} | {newFile.File.Name} | {line} |");
                    if(!string.IsNullOrWhiteSpace(newFile.Text))
                        yield return newFile;
                }
            }
        }



    }
}
