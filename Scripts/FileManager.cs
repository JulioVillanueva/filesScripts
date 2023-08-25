using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;

namespace Scripts
{
    public class FileManager
    {
        string path1;
        string path2;
        List<FileCopied> copiedFiles;
        public FileManager(string path1, string path2)
        {
            this.path1 = path1;
            this.path2 = path2;
        }

        public List<FileCopied> CopiedFiles { get => copiedFiles; set => copiedFiles = value; }

        public void ReplaceFiles()
        {
            var folderFrom = new DirectoryInfo(path1);
            var folderTo = new DirectoryInfo(path2);

            var filesFrom = folderFrom.GetFiles("*", SearchOption.AllDirectories);
            var filesTo = folderTo.GetFiles("*", SearchOption.AllDirectories);

            this.CopiedFiles = (from nw in filesFrom
                              join old in filesTo
                                on new { Name = nw.Name.ToLower()} equals new { Name = old.Name.ToLower() }
                              select new FileCopied ( oldFile: old, newFile: nw,"")).ToList();

            foreach(var files in this.CopiedFiles)
            {
                try
                {
                    File.Copy(files.NewFile.FullName, files.OldFile.FullName,true);
                    files.OutputMsg = "Copied";
                }
                catch (Exception e)
                {
                    files.OutputMsg = $"Error: {e.Message}";
                }
            }
        }
    }
    public class FileCopied
    {
        public FileCopied(FileInfo oldFile, FileInfo newFile,string outputMsg)
        {
            OldFile = oldFile;
            NewFile = newFile;
            OutputMsg = outputMsg;
        }

        public FileInfo OldFile { get; set; }
        public FileInfo NewFile { get; set; }
        public string OutputMsg { get; set; }
    }
}
