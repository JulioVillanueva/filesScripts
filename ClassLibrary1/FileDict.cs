using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesAndRegex
{
    public class FileDict
    {
        public FileInfo File { get; }

        public FileDict(FileInfo file)
        {
            File = file;
        }
    }
}
