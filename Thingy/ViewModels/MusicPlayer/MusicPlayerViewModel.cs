using AppLib.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Thingy.MusicPlayerCore;

namespace Thingy.ViewModels.MusicPlayer
{
    public class MusicPlayerViewModel: BindableBase, IDisposable
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

        public IAudioEngine AudioEngine
        {
            get { return _audioEngine; }
            set
            {
                bool subscribe = false;
                if (value == null &&_audioEngine != null)
                {
                    _audioEngine.SongFinishedEvent -= songFinished;
                    _audioEngine.Dispose();
                }
                else if (value != null)
                {
                    subscribe = true;
                }
                SetValue(ref _audioEngine, value);
                if (subscribe)
                {
                    _audioEngine.SongFinishedEvent += songFinished;
                }
            }
        }

        private void songFinished(object sender, RoutedEventArgs e)
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

        public MusicPlayerViewModel(IApplication app, IAudioEngine engine)
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

        private void OpenFile()
        {
            var ofd = new System.Windows.Forms.OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _audioEngine.Load(ofd.FileName);
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
                AudioEngine.Dispose();
                AudioEngine = null;
            }
        }
    }
}
