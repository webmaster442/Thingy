using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Webmaster442.LibItunesXmlDb;
using System.Linq;

namespace Thingy.MusicPlayer.Views
{
    /// <summary>
    /// Interaction logic for ITunesMenu.xaml
    /// </summary>
    public partial class ITunesMenu : MenuItem
    {
        private ITunesXmlDb iTunes;

        public ITunesMenu()
        {
            InitializeComponent();
            MenuItunes_Loaded();
        }

        public event EventHandler<IEnumerable<string>> FilesProvidedEvent;

        private void MenuItunes_Loaded()
        {
            if (!ITunesXmlDb.UserHasItunesDb || DesignerProperties.GetIsInDesignMode(this))
            {
                IsEnabled = false;
                return;
            }
            try
            {
                ITunesXmlDbOptions options = new ITunesXmlDbOptions
                {
                    ExcludeNonExistingFiles = true,
                    ParalelParsingEnabled = true
                };
                iTunes = new ITunesXmlDb(ITunesXmlDb.UserItunesDbPath, options);
                CreateMenuItems(MenuAlbums, iTunes.Albums.Where(x => !string.IsNullOrEmpty(x)));
                CreateMenuItems(MenuArtists, iTunes.Artists.Where(x => !string.IsNullOrEmpty(x)));
                CreateMenuItems(MenuGenres, iTunes.Genres.Where(x => !string.IsNullOrEmpty(x)));
                CreateMenuItems(MenuYears, iTunes.Years.Where(x => !string.IsNullOrEmpty(x)));
                CreateMenuItems(MenuPlaylists, iTunes.Playlists.Where(x => !string.IsNullOrEmpty(x)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                IsEnabled = false;
                return;
            }
        }

        private void CreateMenuItems(MenuItem menuTarget, IEnumerable<string> items)
        {
            menuTarget.Items.Clear();
            foreach (var item in items)
            {
                MenuItem subitem = new MenuItem
                {
                    Tag = string.Copy(menuTarget.Tag as string),
                    Header = item
                };
                subitem.Click += Subitem_Click;
                menuTarget.Items.Add(subitem);
            }
        }

        private void Subitem_Click(object sender, RoutedEventArgs e)
        {
            if (FilesProvidedEvent != null)
            {
                if (sender is MenuItem s)
                {
                    IEnumerable<string> files = null;
                    string tag = s.Tag.ToString();
                    string content = s.Header.ToString();
                    switch (tag)
                    {
                        case "Albums":
                            files = iTunes.Filter(FilterKind.Album, content).Select(t => t.FilePath);
                            break;
                        case "Artists":
                            files = iTunes.Filter(FilterKind.Artist, content).Select(t => t.FilePath);
                            break;
                        case "Years":
                            files = iTunes.Filter(FilterKind.Year, content).Select(t => t.FilePath);
                            break;
                        case "Genres":
                            files = iTunes.Filter(FilterKind.Genre, content).Select(t => t.FilePath);
                            break;
                        case "Playlists":
                            files = iTunes.ReadPlaylist(content).Select(t => t.FilePath);
                            break;
                    }
                    FilesProvidedEvent.Invoke(this, files);
                }
            }
        }
    }
}
