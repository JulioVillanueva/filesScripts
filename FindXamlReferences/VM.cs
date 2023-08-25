using FilesAndRegex;
using FilesAndRegex.FindInFolderUsingRegex;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace FindXamlReferences
{
    public class VM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string PropName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropName));
        }

        public VM()
        {
            ExecuteCommand = new RelayCommand(Execute, CanExecute);
            ClearDebugPanelCommand = new RelayCommand(ClearDebugPanel, CanClearDebugPanel);
            Path = "";
            Regex = "";
            Extension = "";
            ResetStringWriter();
            this.logger = new Logger();
        }

        private bool isExecuting;
        public bool IsExecuting
        {
            get { return isExecuting; }
            set { isExecuting = value; OnPropertyChanged(); }
        }

        private string path;
        public string Path
        {
            get { return path; }
            set { path = value; OnPropertyChanged(); }
        }

        private string regex;
        public string Regex
        {
            get { return regex; }
            set { regex = value; OnPropertyChanged(); }
        }

        private string extension;
        public string Extension
        {
            get { return extension; }
            set { extension = value; OnPropertyChanged(); }
        }

        private string debugPanel;
        public string DebugPanel
        {
            get { return debugPanel; }
            set { debugPanel = value; OnPropertyChanged(); }
        }

        private void UpdatedebugPanel(object sender, EventArgs args)
        {
            if (sender is StringWriterWithEvent strWriter)
            {
                this.DebugPanel = strWriter.ToString();
            }
        }

        public RelayCommand ExecuteCommand { get; set; }
        public async void Execute()
        {
            try
            {
                IsExecuting = true;
                await ExecuteAsync(this.regex, this.extension, this.path);
            }
            catch (Exception ex)
            {
                IsExecuting = false;
                logger.Log(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
        public async Task ExecuteAsync(string pattern,string extension, string path)
        {
            logger.Log("Starting...\n\n");
            await Task.Delay(1000);
            //throw new NotImplementedException();
            var regexReader = new TextFilesReader();
            var files = regexReader.ApplyRegexToAllFilesInFolder(pattern, extension, path).ToList();

            logger.Log("Printing distinct output \n\n" );

            IEnumerable<string> lines = getAllLinesFromFiles(files);
            if (lines != null)
            {
                foreach (var line in lines)
                {
                    logger.Log(" | " + line);
                }
            }
            //logger.Log(" distinct output printed  \n\n");

            Application.Current.Dispatcher.BeginInvoke(new Action (() => this.IsExecuting = false));
        }

        private IEnumerable<string> getAllLinesFromFiles(IEnumerable<FilesAndRegex.TextFileDict> files)
        {
            var childs = files.SelectMany(f => f?.ChildFiles).Union(files).ToList();
            var lines = childs.SelectMany(f => f?.Text?.Split("\n").Select(t => $"{f?.File.DirectoryName} | {f.File.Name} | {t}")).ToList();
            return lines;
        }

        public bool CanExecute()
        {
            if (isExecuting)
                return false;

            var inputVariables = new string[] { Path, Regex, Extension };

            foreach (var input in inputVariables)
                if (string.IsNullOrWhiteSpace(input))
                    return false;

            return true;
        }

        public RelayCommand ClearDebugPanelCommand { get; set; }
        public void ClearDebugPanel()
        {
            try
            {
                this.DebugPanel = "";
                ResetStringWriter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ResetStringWriter()
        {
            var sw = new StringWriterWithEvent(true);
            SetUpNewStringWriterInConsole(sw);
            if (StrWriter != null)
                DisposeStringWriter(StrWriter);

            this.StrWriter = sw;
        }

        private void DisposeStringWriter(StringWriterWithEvent newStringWriter)
        {
            newStringWriter.Flushed -= UpdatedebugPanel;
            newStringWriter.Flush();
            newStringWriter.Close();
            newStringWriter.Dispose();
        }

        private StringWriterWithEvent SetUpNewStringWriterInConsole(StringWriterWithEvent newStringWriter)
        {
            Console.SetOut(newStringWriter);
            Console.SetError(newStringWriter);
            newStringWriter.Flushed += UpdatedebugPanel;
            return newStringWriter;
        }

        public bool CanClearDebugPanel()
        {
            return !string.IsNullOrWhiteSpace(this.DebugPanel);

        }

        public StringWriterWithEvent StrWriter { get; set; }
        public ILogger logger { get; set; }
    }
}
