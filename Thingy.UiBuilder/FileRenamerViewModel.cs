using AppLib.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;

namespace Thingy.FFMpegGui
{
    public class FileRenamerViewModel: ViewModel
    {
        private string _filenamePattern;
        private string _extensionPattern;
        private string _searchtext;
        private string _replacetext;
        private string _outputFolder;
        private bool _isRegex;
        private IEnumerable<string> _inputFiles;

        private int _counterStart;
        private int _counterIncrement;
        private int _counterDigits;

        public DelegateCommand<KeyValuePair<string, string>> InsertFilePartCommand { get; private set; }
        public DelegateCommand<KeyValuePair<string, string>> InsertExtensionPartCommand { get; private set; }

        public IEnumerable<string> InputFiles
        {
            get { return _inputFiles; }
            set { SetValue(ref _inputFiles, value); }
        }

        public string OutputFolder
        {
            get { return _outputFolder; }
            set { SetValue(ref _outputFolder, value); }
        }

        public ObservableCollection<Tuple<string, string>> RenameTable
        {
            get;
            set;
        }

        public FileRenamerViewModel()
        {
            PatternParts = new Dictionary<string, string>
            {
                { "[Y]", "Year" },
                { "[M]","Month" },
                { "[D]", "Day" },
                { "[H]", "Hour" },
                { "[m]", "Minute" },
                { "[s]", "Second" },
                { "[c]", "Counter"},
                { "[E]", "Extension" },
                { "[N]", "File name" },
            };
            InsertFilePartCommand = Command.ToCommand<KeyValuePair<string, string>>(InsertFilePart);
            InsertExtensionPartCommand = Command.ToCommand<KeyValuePair<string, string>>(InsertExtensionPart);
            CounterStart = 1;
            CounterIncrement = 1;
            CounterDigits = 1;
        }

        private void InsertFilePart(KeyValuePair<string, string> arg)
        {
            FilenamePattern += arg.Key;
        }

        private void InsertExtensionPart(KeyValuePair<string, string> arg)
        {
            ExtensionPattern += arg.Key;
        }

        public Dictionary<string, string> PatternParts
        {
            get;
            private set;
        }

        public string FilenamePattern
        {
            get { return _filenamePattern; }
            set { SetValue(ref _filenamePattern, value); }
        }

        public string ExtensionPattern
        {
            get { return _extensionPattern; }
            set { SetValue(ref _extensionPattern, value); }
        }

        public string SearchText
        {
            get { return _searchtext; }
            set { SetValue(ref _searchtext, value); }
        }

        public string ReplaceText
        {
            get { return _replacetext; }
            set { SetValue(ref _replacetext, value); }
        }

        public bool IsRegex
        {
            get { return _isRegex; }
            set { SetValue(ref _isRegex, value); }
        }

        public int CounterStart
        {
            get { return _counterStart; }
            set { SetValue(ref _counterStart, value); }
        }

        public int CounterIncrement
        {
            get { return _counterIncrement; }
            set { SetValue(ref _counterIncrement, value); }
        }

        public int CounterDigits
        {
            get { return _counterDigits; }
            set { SetValue(ref _counterDigits, value); }
        }

        private void ProcessTags()
        {
            RenameTable.Clear();

            int counter = CounterStart;
            var timeTable = new Dictionary<string, string>
            {
                { "[Y]", DateTime.Now.Year.ToString() },
                { "[M]", DateTime.Now.Month.ToString() },
                { "[D]", DateTime.Now.Day.ToString() },
                { "[H]", DateTime.Now.Hour.ToString() },
                { "[m]", DateTime.Now.Minute.ToString() },
                { "[s]", DateTime.Now.Second.ToString() },
            };

            var targetName = FilenamePattern;
            var targetExtension = ExtensionPattern;

            foreach (var entry in timeTable)
            {
                targetName = targetName.Replace(entry.Key, entry.Value);
                targetExtension = targetExtension.Replace(entry.Key, entry.Value);
            }

            foreach (var file in InputFiles)
            {
                var fileShortName = Path.GetFileName(file);
                var fileExtension = Path.GetExtension(file);
                var counterstring = counter.ToString().PadLeft(CounterDigits, '0');
                counter += CounterIncrement;

                targetName = targetName.Replace("[c]", counterstring);
                targetExtension = targetExtension.Replace("[c]", counterstring);
                targetName = targetName.Replace("[E]", fileExtension);
                targetExtension = targetExtension.Replace("[E]", fileExtension);
                targetName = targetName.Replace("[N]", fileExtension);
                targetExtension = targetExtension.Replace("[N]", fileExtension);

                var completedname = Path.Combine(OutputFolder, $"{targetName}.{targetExtension}");

                if (IsRegex)
                    completedname = Regex.Replace(completedname, SearchText, ReplaceText);
                else
                    completedname = completedname.Replace(SearchText, ReplaceText);

                RenameTable.Add(Tuple.Create(file, completedname));
            }
        }
    }
}
