using AppLib.Common.Extensions;
using AppLib.MVVM;
using AppLib.WPF;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Media;
using Thingy.API;
using Thingy.CoreModules.Models;

namespace Thingy.CoreModules.ViewModels
{
    public class XKCDReaderViewModel: ViewModel
    {
        private IApplication _app;
        private int _lastIndex;
        private bool _progressVisible;

        public DelegateCommand NextComicCommand { get; set; }
        public DelegateCommand PreviousComicCommand { get; set; }
        public DelegateCommand<int> LoadSpecificComicCommand { get; set; }
        public DelegateCommand LoadLatestComicCommand { get; set; }
        public DelegateCommand FirstComicCommand { get; set; }
        public DelegateCommand LastComicCommand { get; set; }

        private string _alt;

        public string Alt
        {
            get { return _alt; }
            set { SetValue(ref _alt, value); }
        }

        private DateTime _releaseDay;

        public DateTime ReleaseDay
        {
            get { return _releaseDay; }
            set { SetValue(ref _releaseDay, value); }
        }

        private ImageSource _image;

        public ImageSource Image
        {
            get { return _image; }
            set { SetValue(ref _image, value); }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetValue(ref _title, value); }
        }

        private int _currentComicId;

        public int CurrentComicId
        {
            get { return _currentComicId; }
            set { SetValue(ref _currentComicId, value); }
        }

        public bool ProgressVisible
        {
            get { return _progressVisible; }
            set { SetValue(ref _progressVisible, value); }
        }

        private bool _contentVisible;

        public bool ContentVisible
        {
            get { return _contentVisible; }
            set { SetValue(ref _contentVisible, value); }
        }


        public ObservableCollection<int> PreviousComics { get; set; }

        public XKCDReaderViewModel(IApplication app)
        {
            _app = app;
            PreviousComics = new ObservableCollection<int>();
            LoadLatestComicCommand = Command.CreateCommand(LoadLatestComic);
            NextComicCommand = Command.CreateCommand(NextComic, CanDoNext);
            PreviousComicCommand = Command.CreateCommand(PreviousComic, CanDoPrevious);
            LoadSpecificComicCommand = Command.CreateCommand<int>(LoadSpecificComic);
            FirstComicCommand = Command.CreateCommand(FirstComic);
            LastComicCommand = Command.CreateCommand(LastComic);
        }

        private async Task LoadDataFromJson(string path = "info.0.json")
        {
            ProgressVisible = true;
            ContentVisible = false;
            WebClient client = new WebClient();

            IWebProxy defaultProxy = WebRequest.DefaultWebProxy;
            if (defaultProxy != null)
            {
                defaultProxy.Credentials = CredentialCache.DefaultCredentials;
                client.Proxy = defaultProxy;
            }

            try
            {
                var adress = new Uri("https://xkcd.com/" + path);
                string json = await client.DownloadStringTaskAsync(adress);
                XKCDResponse response = JsonConvert.DeserializeObject<XKCDResponse>(json);

                byte[] image = await client.DownloadDataTaskAsync(response.img);
                using (var ms = new System.IO.MemoryStream(image))
                {
                    Image = BitmapHelper.FrozenBitmap(ms);
                }

                ReleaseDay = response.PublicationDate;
                Alt = response.alt;
                Title = $"{response.title} #{response.num}";
                CurrentComicId = response.num;

                if (path == "info.0.json")
                {
                    _lastIndex = response.num;
                    PreviousComics.UpdateWith(Enumerable.Range(1, response.num));
                }

                ProgressVisible = false;
                ContentVisible = true;
            }
            catch (Exception ex)
            {
                await _app.ShowMessageBox("Error", ex.Message, DialogButtons.Ok);
                ProgressVisible = false;
                ContentVisible = false;
                return;
            }
        }

        private async void LoadSpecificComic(int obj)
        {
            await LoadDataFromJson($"{obj}/info.0.json");
        }

        private bool CanDoPrevious()
        {
            return CurrentComicId > 0;
        }

        private async void PreviousComic()
        {
            await LoadDataFromJson($"{--CurrentComicId}/info.0.json");
        }

        private bool CanDoNext()
        {
            return CurrentComicId < _lastIndex;
        }

        private async void LastComic()
        {
            await LoadDataFromJson($"{_lastIndex}/info.0.json");
        }

        private async void FirstComic()
        {
            await LoadDataFromJson($"1/info.0.json");
        }

        private async void NextComic()
        {
            await LoadDataFromJson($"{++CurrentComicId}/info.0.json");
        }


        private async void LoadLatestComic()
        {
            await LoadDataFromJson();
        }
    }
}
