using AppLib.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.MusicPlayerCore;

namespace Thingy.ViewModels.MusicPlayer
{
    public class MusicPlayerViewModel: BindableBase, IDisposable
    {
        private IAudioEngine _audioEngine;

        public DelegateCommand OpenFileCommand { get; private set; }
        public DelegateCommand PlayCommand { get; private set; }
        public DelegateCommand PauseCommand { get; private set; }
        public DelegateCommand SeekFwdCommand { get; private set; }
        public DelegateCommand SeekBackCommand { get; private set; }

        public IAudioEngine AudioEngine
        {
            get { return _audioEngine; }
            set { SetValue(ref _audioEngine, value); }
        }

        public MusicPlayerViewModel(IAudioEngine engine)
        {
            AudioEngine = engine;
            OpenFileCommand = Command.ToCommand(OpenFile);
            PlayCommand = Command.ToCommand(Play);
            PauseCommand = Command.ToCommand(Pause);
            SeekBackCommand = Command.ToCommand(SeekBack);
            SeekFwdCommand = Command.ToCommand(SeekFwd);
        }

        private void SeekFwd()
        {
            _audioEngine.Seeking = true;
            double position = _audioEngine.Position;
            _audioEngine.Position = position + 5;
            _audioEngine.Seeking = false;
        }

        private void SeekBack()
        {
            _audioEngine.Seeking = true;
            double position = _audioEngine.Position;
            _audioEngine.Position = position - 5;
            _audioEngine.Seeking = false;
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
