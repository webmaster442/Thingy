using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Thingy.FFMpegGui
{
    /// <summary>
    /// Interaction logic for FileRenamer.xaml
    /// </summary>
    public partial class FileRenamer : UserControl
    {
        public FileRenamer()
        {
            InitializeComponent();
            FilenamePattern = "";
            ExtensionPattern = "";
            SearchText = "";
            ReplaceText = "";
            RenameTable = new ObservableCollection<Tuple<string, string>>();
            InputFiles = new ObservableCollection<string>();
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
            FileNamePatterns.ItemsSource = PatternParts;
            ExtensionPatterns.ItemsSource = PatternParts;
        }

        public Dictionary<string, string> PatternParts
        {
            get;
            private set;
        }

        public string OutputFolder
        {
            get { return (string)GetValue(OutputFolderProperty); }
            set { SetValue(OutputFolderProperty, value); }
        }

        public static readonly DependencyProperty OutputFolderProperty =
            DependencyProperty.Register("OutputFolder", typeof(string), typeof(FileRenamer), new PropertyMetadata(null, DPChanged));

        public ObservableCollection<Tuple<string, string>> RenameTable
        {
            get { return (ObservableCollection<Tuple<string, string>>)GetValue(RenameTableProperty); }
            set { SetValue(RenameTableProperty, value); }
        }

        public static readonly DependencyProperty RenameTableProperty =
            DependencyProperty.Register("RenameTable", typeof(ObservableCollection<Tuple<string, string>>), typeof(FileRenamer), new PropertyMetadata(null));

        public ObservableCollection<string> InputFiles
        {
            get { return (ObservableCollection<string>)GetValue(InputFilesProperty); }
            set { SetValue(InputFilesProperty, value); }
        }

        public static readonly DependencyProperty InputFilesProperty =
            DependencyProperty.Register("InputFiles", typeof(ObservableCollection<string>), typeof(FileRenamer), new PropertyMetadata(null, InputChanged));

        public string FilenamePattern
        {
            get { return (string)GetValue(FilenamePatternProperty); }
            set { SetValue(FilenamePatternProperty, value); }
        }

        public static readonly DependencyProperty FilenamePatternProperty =
            DependencyProperty.Register("FilenamePattern", typeof(string), typeof(FileRenamer), new PropertyMetadata(null, DPChanged));

        public string ExtensionPattern
        {
            get { return (string)GetValue(ExtensionPatternProperty); }
            set { SetValue(ExtensionPatternProperty, value); }
        }

        public static readonly DependencyProperty ExtensionPatternProperty =
            DependencyProperty.Register("ExtensionPattern", typeof(string), typeof(FileRenamer), new PropertyMetadata(null, DPChanged));

        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(FileRenamer), new PropertyMetadata(null, DPChanged));

        public string ReplaceText
        {
            get { return (string)GetValue(ReplaceTextProperty); }
            set { SetValue(ReplaceTextProperty, value); }
        }

        public static readonly DependencyProperty ReplaceTextProperty =
            DependencyProperty.Register("ReplaceText", typeof(string), typeof(FileRenamer), new PropertyMetadata(null, DPChanged));

        public bool IsRegex
        {
            get { return (bool)GetValue(IsRegexProperty); }
            set { SetValue(IsRegexProperty, value); }
        }

        public static readonly DependencyProperty IsRegexProperty =
            DependencyProperty.Register("IsRegex", typeof(bool), typeof(FileRenamer), new PropertyMetadata(false, DPChanged));

        public int CounterStart
        {
            get { return (int)GetValue(CounterStartProperty); }
            set { SetValue(CounterStartProperty, value); }
        }

        public static readonly DependencyProperty CounterStartProperty =
            DependencyProperty.Register("CounterStart", typeof(int), typeof(FileRenamer), new PropertyMetadata(1, DPChanged));

        public int CounterIncrement
        {
            get { return (int)GetValue(CounterIncrementProperty); }
            set { SetValue(CounterIncrementProperty, value); }
        }

        public static readonly DependencyProperty CounterIncrementProperty =
            DependencyProperty.Register("CounterIncrement", typeof(int), typeof(FileRenamer), new PropertyMetadata(1, DPChanged));

        public int CounterDigits
        {
            get { return (int)GetValue(CounterDigitsProperty); }
            set { SetValue(CounterDigitsProperty, value); }
        }

        public static readonly DependencyProperty CounterDigitsProperty =
            DependencyProperty.Register("CounterDigits", typeof(int), typeof(FileRenamer), new PropertyMetadata(1, DPChanged));

        private static void DPChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FileRenamer fr = d as FileRenamer;
            fr.ProcessTags();
        }

        private static void InputChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FileRenamer fr = d as FileRenamer;
            var current = e.NewValue as ObservableCollection<string>;

            if (fr.InputFiles != null)
                fr.InputFiles.CollectionChanged -= fr.InputCollectionChanged;
            else
                fr.InputFiles.CollectionChanged += fr.InputCollectionChanged;
        }

        private void InputCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ProcessTags();
        }

        private void ProcessTags()
        {
            if (InputFiles == null || InputFiles.Count == 0) return;

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

            foreach (var file in InputFiles)
            {
                var targetName = FilenamePattern;
                var targetExtension = ExtensionPattern;

                foreach (var entry in timeTable)
                {
                    targetName = targetName.Replace(entry.Key, entry.Value);
                    targetExtension = targetExtension.Replace(entry.Key, entry.Value);
                }

                var fileShortName = Path.GetFileNameWithoutExtension(file);
                var fileExtension = Path.GetExtension(file);
                var counterstring = counter.ToString().PadLeft(CounterDigits, '0');
                counter += CounterIncrement;

                targetName = targetName.Replace("[c]", counterstring);
                targetExtension = targetExtension.Replace("[c]", counterstring);

                targetName = targetName.Replace("[E]", fileExtension);
                targetExtension = targetExtension.Replace("[E]", fileExtension);

                targetName = targetName.Replace("[N]", fileShortName);
                targetExtension = targetExtension.Replace("[N]", fileShortName);

                if (!string.IsNullOrEmpty(targetExtension))
                {
                    targetExtension = "." + targetExtension;
                }
                var completedname = Path.Combine(OutputFolder, $"{targetName}{targetExtension}");

                if (!string.IsNullOrEmpty(SearchText))
                {
                    if (IsRegex)
                        completedname = Regex.Replace(completedname, SearchText, ReplaceText);
                    else
                        completedname = completedname.Replace(SearchText, ReplaceText);
                }

                RenameTable.Add(Tuple.Create(file, completedname));
            }
        }

        private void InsertFilenamePattern_Click(object sender, RoutedEventArgs e)
        {
            if (FileNamePatterns.SelectedItem == null) return;
            var item = (KeyValuePair<string, string>)FileNamePatterns.SelectedItem;
            FilenamePattern += item.Key;
        }

        private void InsertExtensionPattern_Click(object sender, RoutedEventArgs e)
        {
            if (ExtensionPatterns.SelectedItem == null) return;
            var item = (KeyValuePair<string, string>)ExtensionPatterns.SelectedItem;
            ExtensionPattern += item.Key;
        }
    }
}
