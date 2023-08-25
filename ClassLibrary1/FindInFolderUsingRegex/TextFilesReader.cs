using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace FilesAndRegex.FindInFolderUsingRegex
{
    public class TextFilesReader
    {
        private Logger log;

        public TextFilesReader()
        {
            log = new Logger();
        }

        public IEnumerable<TextFileDict> ApplyRegexToAllFilesInFolder(string regex_pattern, string extension, string path)
        {
            log.Log("Starting");
           ReadTextInFiles reader = new ReadTextInFiles(path);
            var filesWithRightExtension = reader.GetAllTextFiles(extension);
            var regex = new Regex(regex_pattern);
            var RegexAction = new Func<string, string>((text) => string.Join("\n",regex.Matches(text)?.Where(s => !string.IsNullOrWhiteSpace(s.Value)).Select(match => match.Value)));
            var filesWithRegex = reader.ApplyRegexToTextFilesDict(filesWithRightExtension, RegexAction);
            return filesWithRegex;
        }
    }
}
