using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;

namespace FilesAndRegex.copyAndReplace
{
    public class WindowsReplaceMatchedFileManager
    {
        string path1;
        string path2;
        List<FileCopied> copiedFiles;
        public WindowsReplaceMatchedFileManager(string path1, string path2)
        {
            this.path1 = path1;
            this.path2 = path2;
        }

        public List<FileCopied> CopiedFiles { get => copiedFiles; set => copiedFiles = value; }

        public void ReplaceFiles()
        {
            var fm = new FileManager();
            FileInfo[] filesFrom = fm.GetAllFiles(path1);
            FileInfo[] filesTo = fm.GetAllFiles(path2);

            CopiedFiles = (from nw in filesFrom
                           join old in filesTo
                             on new { Name = nw.Name.ToLower() } equals new { Name = old.Name.ToLower() }
                           select new FileCopied(oldFile: old, newFile: nw, "")).ToList();

            foreach (var files in CopiedFiles)
            {
                try
                {
                    File.Copy(files.NewFile.FullName, files.OldFile.FullName, true);
                    files.OutputMsg = "Copied";
                }
                catch (Exception e)
                {
                    files.OutputMsg = $"Error: {e.Message}";
                }
            }
        }


    }
}
