using System;

namespace Thingy.MusicPlayer.Models
{
    public class PodcastFeedItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string LocalFile { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
