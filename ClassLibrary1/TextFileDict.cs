using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesAndRegex
{
    public class TextFileDict : FileDict
    {
        private IEnumerable<TextFileDict> childFiles;

        public IEnumerable<TextFileDict> ChildFiles { get => childFiles; set => childFiles = value ?? new List<TextFileDict>(); }
        public string Text { get; set; }
        public TextFileDict(FileInfo file) : base(file)
        {
            ChildFiles = new List<TextFileDict>();
        }
    }
}
