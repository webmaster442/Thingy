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

namespace Thingy.MusicPlayer.ViewModels
{
    public class PodcastToolViewModel: ViewModel
    {
        private readonly IApplication _app;
        private readonly IDataBase _db;

        public ObservableCollection<PodcastUri> Podcasts { get; }
        public ObservableCollection<PodcastFeedItem> Feed { get; }

        public DelegateCommand AddFeedCommand { get; }
        public DelegateCommand RemoveFeedCommand { get; }

        public PodcastToolViewModel(IApplication app, IDataBase db)
        {
            _app = app;
            _db = db;
            Podcasts = new ObservableCollection<PodcastUri>(_db.Podcasts.GetPodcasts());
            Feed = new ObservableCollection<PodcastFeedItem>();
            AddFeedCommand = Command.ToCommand(AddFeed);
        }

        private async Task<SyndicationFeed> DownloadAndParseFeed(string url)
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

        private IEnumerable<PodcastFeedItem> DownloadAndParseFeed(SyndicationFeed feed)
        {
            foreach (var item in feed.Items)
            {
                yield return new PodcastFeedItem
                {
                    Description = item.Content.ToString(),
                    Url = item.BaseUri.ToString(),
                    Title = item.Title.Text,
                    LocalFile = "",
                    PublishDate = item.PublishDate.UtcDateTime
                };
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
                    var feed = await DownloadAndParseFeed(dialog.Url);
                    _db.Podcasts.SavePodcast(new PodcastUri
                    {
                        Name = feed.Title.Text,
                        Uri = dialog.Url
                    });
                    Podcasts.UpdateWith(_db.Podcasts.GetPodcasts());
                    Feed.UpdateWith(DownloadAndParseFeed(feed));
                }
                catch (Exception ex)
                {
                    await _app.ShowMessageBox("Error", "Can't download or parse given url", DialogButtons.Ok);
                    _app.Log.Error(ex);
                }
            }
        }
    }
}
