using AppLib.Common.Extensions;
using AppLib.MVVM;
using System;
using System.Windows;
using Thingy.MusicPlayerCore;
using Thingy.MusicPlayerCore.DataObjects;
using Thingy.MusicPlayerCore.Formats;
using Thingy.Views.Interfaces;

namespace Thingy.ViewModels.MusicPlayer
{
    public class MusicPlayerViewModel: ViewModel<IMusicPlayer>, IDisposable
    {
        private IAudioEngine _audioEngine;
        private PlayListViewModel _playlist;

        public DelegateCommand OpenFileCommand { get; private set; }
        public DelegateCommand PlayCommand { get; private set; }
        public DelegateCommand PauseCommand { get; private set; }
        public DelegateCommand SeekFwdCommand { get; private set; }
        public DelegateCommand SeekBackCommand { get; private set; }

        public DelegateCommand NextTrackCommand { get; private set; }
        public DelegateCommand PrevousTrackCommand { get; private set; }

        public DelegateCommand DragStartedCommand { get; private set; }
        public DelegateCommand<double> DragCompletedCommand { get; private set; }
        public DelegateCommand<int> SelectedDeviceChangedCommand { get; private set; }
        public DelegateCommand<int> PlayListDoubleClickCommand { get; private set; }

        public DelegateCommand<Chapter> JumpToChapterCommand { get; private set; }

        public IAudioEngine AudioEngine
        {
            get { return _audioEngine; }
            set
            {
                bool subscribe = false;
                if (value == null &&_audioEngine != null)
                {
                    _audioEngine.SongFinishedEvent -= SongFinished;
                    _audioEngine.Dispose();
                }
                else if (value != null)
                {
                    subscribe = true;
                }
                SetValue(ref _audioEngine, value);
                if (subscribe)
                {
                    _audioEngine.SongFinishedEvent += SongFinished;
                }
            }
        }

        private void SongFinished(object sender, RoutedEventArgs e)
        {
            if (Playlist.IsPossibleToChangeTrack(1))
            {
                Playlist.CurrentIndex = Playlist.CurrentIndex + 1;
                _audioEngine.Load(Playlist.CurrrentFile);
                _audioEngine.Play();
            }
            else
            {
                _audioEngine.Stop();
            }
        }

        public PlayListViewModel Playlist
        {
            get { return _playlist; }
            set { SetValue(ref _playlist, value); }
        }

        public MusicPlayerViewModel(IMusicPlayer view, IApplication app, IAudioEngine engine) : base(view)
        {
            AudioEngine = engine;
            Playlist = new PlayListViewModel(app);
            OpenFileCommand = Command.ToCommand(OpenFile);
            PlayCommand = Command.ToCommand(Play);
            PauseCommand = Command.ToCommand(Pause);
            SeekBackCommand = Command.ToCommand(SeekBack);
            SeekFwdCommand = Command.ToCommand(SeekFwd);
            DragStartedCommand = Command.ToCommand(DragStarted);
            DragCompletedCommand = Command.ToCommand<double>(DragCompleted);
            PrevousTrackCommand = Command.ToCommand(PrevousTrack);
            NextTrackCommand = Command.ToCommand(NextTrack);
            SelectedDeviceChangedCommand = Command.ToCommand<int>(SelectedDeviceChanged);
            PlayListDoubleClickCommand = Command.ToCommand<int>(PlayListDoubleClick);
            JumpToChapterCommand = Command.ToCommand<Chapter>(JumpToChapter);
        }

        private void PlayListDoubleClick(int index)
        {
            if (index < 0) return;
            string file = Playlist.List[index];
            _audioEngine.Load(file);
            _audioEngine.Play();
            View.SwithToTab(MusicPlayerTabs.Player);
            Playlist.CurrentIndex = index;
        }

        private void SelectedDeviceChanged(int obj)
        {
            int counter = 0;
            foreach (var item in AudioEngine.OutputDevices)
            {
                if (counter == obj)
                {
                    _audioEngine.PlayBackDeviceIndex = item.Value;
                    break;
                }
                ++counter;
            }
        }

        private void DragCompleted(double obj)
        {
            AudioEngine.Position = obj;
            AudioEngine.Seeking = false;
        }

        private void DragStarted()
        {
            AudioEngine.Seeking = true;
        }


        private void SeekBack()
        {
            _audioEngine.Seeking = true;
            double position = _audioEngine.Position;
            _audioEngine.Position = position - 5;
            _audioEngine.Seeking = false;
        }

        private void SeekFwd()
        {
            _audioEngine.Seeking = true;
            double position = _audioEngine.Position;
            _audioEngine.Position = position + 5;
            _audioEngine.Seeking = false;
        }

        private void JumpToChapter(Chapter obj)
        {
            _audioEngine.Seeking = true;
            _audioEngine.Position = obj.Position;
            _audioEngine.Seeking = false;
        }

        private void PrevousTrack()
        {
            if (Playlist.IsPossibleToChangeTrack(-1))
            {
                Playlist.CurrentIndex = Playlist.CurrentIndex - 1;
                _audioEngine.Load(Playlist.CurrrentFile);
                _audioEngine.Play();
            }
        }

        private void NextTrack()
        {
            if (Playlist.IsPossibleToChangeTrack(1))
            {
                Playlist.CurrentIndex = Playlist.CurrentIndex + 1;
                _audioEngine.Load(Playlist.CurrrentFile);
                _audioEngine.Play();
            }
        }

        public void HandleFiles(params string[] files)
        {
            Playlist.List.AddRange(files);
            View.SwithToTab(MusicPlayerTabs.Playlist);
        }

        private async void OpenFile()
        {
            var ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = AudioEngine.ExtensionProvider.AllFormatsAndPlaylistsFilterString;
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (ofd.FileNames.Length > 1)
                {
                    foreach (var file in ofd.FileNames)
                    {
                        var format = AudioEngine.ExtensionProvider.GetFormatKind(file);
                        if (format == FormatKind.Playlist)
                            await Playlist.DoOpenList(file, true);
                        else if (format == FormatKind.Stream)
                            Playlist.List.Add(file);
                    }
                    View.SwithToTab(MusicPlayerTabs.Playlist);
                }
                else
                {
                    var format = AudioEngine.ExtensionProvider.GetFormatKind(ofd.FileName);
                    if (format == FormatKind.Playlist)
                    {
                        await Playlist.DoOpenList(ofd.FileName, true);
                        View.SwithToTab(MusicPlayerTabs.Playlist);
                    }
                    else if (format == FormatKind.Stream)
                    {
                        Playlist.List.Add(ofd.FileName);
                        if (Playlist.List.Count == 1)
                        {
                            _audioEngine.Load(Playlist.List[0]);
                            _audioEngine.Play();
                            View.SwithToTab(MusicPlayerTabs.Player);
                        }
                        else
                            View.SwithToTab(MusicPlayerTabs.Playlist);
                    }
                }
            }
        }

        private void Pause()
        {
            _audioEngine.Pause();
        }

        private void Play()
        {
            _audioEngine.Play();
        }

        public void Dispose()
        {
            if (AudioEngine != null)
            {
                AudioEngine.Stop();
            }
        }
    }
}
