namespace FilesAndRegex.copyAndReplace
{
    public class FileCopied
    {
        public FileCopied(FileInfo oldFile, FileInfo newFile, string outputMsg)
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
