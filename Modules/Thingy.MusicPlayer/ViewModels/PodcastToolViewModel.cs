using AppLib.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Thingy.API;
using Thingy.Db;
using Thingy.Db.Entity;
using Thingy.MusicPlayer.Models;
using AppLib.Common.Extensions;
using Thingy.MusicPlayerCore.Formats;
using Thingy.JobCore;

namespace Thingy.MusicPlayer.ViewModels
{
    public class PodcastToolViewModel : ViewModel
    {
        private readonly IApplication _app;
        private readonly IDataBase _db;
        private IExtensionProvider _extensions;

        private string _downloaddir;

        public string DownloadDir
        {
            get { return _downloaddir; }
            set
            {
                if (SetValue(ref _downloaddir, value))
                {
                    if (Feed != null && Feed.Count > 0)
                    {
                        foreach (var item in Feed)
                        {
                            item.LocalFile = GetLocalFile(item.Url, DownloadDir);
                        }
                    }
                    _app.Settings.Set("PodcastDownloadDir", value);
                }
            }
        }

        public ObservableCollection<PodcastUri> Podcasts { get; }
        public ObservableCollection<PodcastFeedItem> Feed { get; }

        public DelegateCommand AddFeedCommand { get; }
        public DelegateCommand<int> RemoveFeedCommand { get; }
        public DelegateCommand<int> DownloadAndParseFeedCommand { get; }
        public DelegateCommand<int> DownloadPodcastCommand { get; }
        public DelegateCommand<int> SendToPlayerCommand { get; }

        public PodcastToolViewModel(IApplication app, IDataBase db)
        {
            _app = app;
            _db = db;
            _extensions = new ExtensionProvider();
            DownloadDir = _app.Settings.Get("PodcastDownloadDir", @"c:\podcasts");
            Podcasts = new ObservableCollection<PodcastUri>(_db.Podcasts.GetAll());
            Feed = new ObservableCollection<PodcastFeedItem>();
            DownloadAndParseFeedCommand = Command.CreateCommand<int>(DownloadAndParseFeed);
            AddFeedCommand = Command.CreateCommand(AddFeed);
            RemoveFeedCommand = Command.CreateCommand<int>(RemoveFeed, CanRemoveFeed);
            DownloadPodcastCommand = Command.CreateCommand<int>(DownloadPodcast, CanDownloadPodcast);
            SendToPlayerCommand = Command.CreateCommand<int>(SendToPlayer, CanSendToPlayer);
        }

        private async Task<SyndicationFeed> DownloadFeed(string url)
        {
            using (WebClient client = new WebClient())
            {
                string xml = await client.DownloadStringTaskAsync(url);
                using (var reader = new StringReader(xml))
                {
                    XmlReader xreader = XmlReader.Create(reader);
                    SyndicationFeed feed = SyndicationFeed.Load(xreader);
                    var title = feed.Title.Text;
                    return feed;
                }
            }
        }

        private IEnumerable<PodcastFeedItem> ParseFeed(SyndicationFeed feed)
        {
            foreach (var item in feed.Items)
            {
                var link = ExtractLink(item.Links);
                var local = GetLocalFile(link, DownloadDir);
                yield return new PodcastFeedItem
                {
                    Description = item.Summary.Text,
                    Url = link,
                    Title = item.Title.Text,
                    LocalFile = local,
                    PublishDate = item.PublishDate.UtcDateTime
                };
            }
        }

        private string GetLocalFile(string link, string dir)
        {
            var file = Path.GetFileName(link);
            return Path.Combine(dir, file);
        }

        private string ExtractLink(Collection<SyndicationLink> links)
        {
            foreach (var link in links)
            {
                var str = link.Uri.ToString();
                if (_extensions.GetFormatKind(str) == FormatKind.Stream)
                    return str;
            }
            return null;
        }

        private async void DownloadAndParseFeed(int index)
        {
            if (index < 0) return;
            try
            {
                var item = Podcasts[index];
                var feed = await DownloadFeed(item.Uri);
                Feed.UpdateWith(ParseFeed(feed));
            }
            catch (Exception ex)
            {
                await _app.ShowMessageBox("Error", "Can't download or parse given url", DialogButtons.Ok);
                _app.Log.Exception(ex);
            }
        }

        private async void AddFeed()
        {
            var dialog = new Views.AddURLDialog();
            bool result = await _app.ShowDialog("Add URL...", dialog, DialogButtons.OkCancel);
            if (result)
            {
                try
                {
                    var feed = await DownloadFeed(dialog.Url);
                    _db.Podcasts.Save(new PodcastUri
                    {
                        Name = feed.Title.Text,
                        Uri = dialog.Url
                    });
                    Podcasts.UpdateWith(_db.Podcasts.GetAll());
                    Feed.UpdateWith(ParseFeed(feed));
                }
                catch (Exception ex)
                {
                    await _app.ShowMessageBox("Error", "Can't download or parse given url", DialogButtons.Ok);
                    _app.Log.Exception(ex);
                }
            }
        }

        private bool CanDownloadPodcast(int obj)
        {
            return obj > -1 && !File.Exists(Feed[obj].LocalFile);
        }

        private async void DownloadPodcast(int obj)
        {
            if (!Directory.Exists(DownloadDir))
            {
                await _app.ShowMessageBox("Download directory not found", "Download directory doesn't exit. Please set it", DialogButtons.Ok);
                return;
            }
            else
            {
                var item = Feed[obj];
                var job = new JobCore.Jobs.DownloadFileJob(item.Url, item.LocalFile);
                await _app.JobRunner.RunJob(job);
            }
        }

        private void SendToPlayer(int obj)
        {
            _app.HandleFiles(new List<string> { Feed[obj].LocalFile });
        }

        private bool CanSendToPlayer(int obj)
        {
            return obj > -1 && File.Exists(Feed[obj].LocalFile);
        }

        private void RemoveFeed(int obj)
        {
            var seledted = Podcasts[obj];
            _db.Podcasts.Delete(seledted.Name);
            Podcasts.UpdateWith(_db.Podcasts.GetAll());
        }

        private bool CanRemoveFeed(int obj)
        {
            return obj > -1;
        }
    }
}
