using AppLib.Common.Console;
using AppLib.Common.Extensions;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Threading;
using Thingy.API;
using Thingy.Implementation;

namespace Thingy.Controls
{
    /// <summary>
    /// Interaction logic for CommandLine.xaml
    /// </summary>
    public partial class CommandLine : UserControl, ICmdHost
    {
        private string _currentDir;
        private const int _maxBufferLines = 250;

        public CommandLine()
        {
            InitializeComponent();
            AutoCompleteSource = new ObservableCollection<string>();
            Buffer = new ObservableCollection<string>();
            TerminalControl.AutoCompletionsSource = AutoCompleteSource;
            TerminalControl.ItemsSource = Buffer;
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _currentDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            UpdateAutoCompleteSource();
            SetPrompt();
        }


        public IModuleLoader ModuleLoader { get; set; }

        public IApplication App { get; set; }

        public ObservableCollection<string> Buffer { get; }

        public ObservableCollection<string> AutoCompleteSource { get; }

        public string CurrentDirectory
        {
            get { return _currentDir; }
            set
            {
                _currentDir = value;
                UpdateAutoCompleteSource();
                SetPrompt();
            }
        }

        Dispatcher ICmdHost.HostDispatcher
        {
            get { return App.CurrentDispatcher; }
        }

        private void SetPrompt()
        {
            TerminalControl.Prompt = $"{CurrentDirectory} $ ";
        }

        private void UpdateAutoCompleteSource()
        {
            AutoCompleteSource.Clear();
            AutoCompleteSource.AddRange(ModuleLoader.CommandNames);
            var files = Directory.GetFiles(CurrentDirectory).Select(f => Path.GetFileName(f));
            var dirs = Directory.GetDirectories(CurrentDirectory).Select(d => Path.GetDirectoryName(d));
            AutoCompleteSource.AddRange(files);
            AutoCompleteSource.AddRange(dirs);
        }

        private void AppendLines(params string[] lines)
        {
            if (Buffer.Count > _maxBufferLines)
            {
                int linesToDelete = Buffer.Count - _maxBufferLines;
                for (int i = 0; i < linesToDelete; i++)
                {
                    Buffer.RemoveAt(0);
                }
            }
            Buffer.AddRange(lines);
        }

        public void Clear()
        {
            Buffer.Clear();
        }

        private CmdArguments GetArguments()
        {
            var parser = new ParameterParser(TerminalControl.Line, true, true);
            return new CmdArguments(parser.StandaloneSwitches, parser.Files, parser.SwitchesWithValue);
        }

        private async void TerminalControl_LineEntered(object sender, EventArgs e)
        {
            try
            {
                var cmdname = TerminalControl.Line.Split(' ')[0];
                var moduleToRun = ModuleLoader.GetCommandLineModuleByName(cmdname);
                if (moduleToRun == null)
                    throw new Exception("Unrecognized command: " + cmdname);

                var arguments = GetArguments();
                var results = await moduleToRun.Run(arguments);
                if (results != null)
                {
                    AppendLines(results.ToArray());
                }
            }
            catch (Exception ex)
            {
                AppendLines(ex.Message);
                App.Log.Error(ex);
            }
        }

        void ICmdHost.Clear()
        {
            throw new NotImplementedException();
        }
    }
}
