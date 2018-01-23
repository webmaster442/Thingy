using AppLib.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.MusicPlayerCore;

namespace Thingy.ViewModels.MusicPlayer
{
    public class MusicPlayerViewModel: BindableBase
    {
        private IAudioEngine _audioEngine;

        public DelegateCommand OpenFileCommand { get; private set; }
        public DelegateCommand PlayCommand { get; private set; }
        public DelegateCommand PauseCommand { get; private set; }
        public DelegateCommand SeekFwdCommand { get; private set; }
        public DelegateCommand SeekBackCommand { get; private set; }

        public MusicPlayerViewModel(IAudioEngine engine)
        {
            _audioEngine = engine;
            OpenFileCommand = Command.ToCommand(OpenFile);
            PlayCommand = Command.ToCommand(Play);
            PauseCommand = Command.ToCommand(Pause);
            SeekBackCommand = Command.ToCommand(SeekBack);
            SeekFwdCommand = Command.ToCommand(SeekFwdCommand);
        }

        private void SeekBack()
        {
            throw new NotImplementedException();
        }

        private void Pause()
        {
            throw new NotImplementedException();
        }

        private void Play()
        {
            throw new NotImplementedException();
        }

        private void OpenFile()
        {
            throw new NotImplementedException();
        }
    }
}
