using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MusicService.Data;
using MusicService.Models;
using Npgsql;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MusicService
{
    public partial class GetAlbumSongs : Page
    {
        MusicApplicationContext _dbContext;
        public Album CurrentAlbum { get; set; }
        public ObservableCollection<Song> SongsInAlbum { get; set; }
        public GetAlbumSongs(Album album)
        {
            _dbContext = new MusicApplicationContext();
            CurrentAlbum = album;

            string query = "SELECT * FROM \"song\" WHERE \"album_id_fk\" = @AlbumId";
            SongsInAlbum = new ObservableCollection<Song>(_dbContext.Songs.FromSqlRaw(query,
                new NpgsqlParameter("@AlbumId", CurrentAlbum.AlbumId)).ToList());
            
            DataContext = this;
            InitializeComponent();
        }
        private void PlaySong_Click(object sender, RoutedEventArgs e)
        {
            var song = (Song)MusicListView.SelectedItem;
            if (!string.IsNullOrEmpty(song.FilePath))
            {
                MusicPlayer.Source = new Uri(song.FilePath, UriKind.RelativeOrAbsolute);
                MusicPlayer.Play();
            }
        }
        private void StopSong_Click(object sender, RoutedEventArgs e)
        {
            MusicPlayer.Stop();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var song = (Song)MusicListView.SelectedItem;
            string query = "UPDATE song SET album_id_fk = @AlbumId WHERE song_id = @SongId";
            var parameters = new[]
            {
                new NpgsqlParameter("@AlbumId", DBNull.Value),
                new NpgsqlParameter("@SongId", song.SongId)
            };
            SongsInAlbum.Remove(song);
            _dbContext.Database.ExecuteSqlRaw(query, parameters);
            _dbContext.SaveChanges();
           // RefreshMusicListView();
        }

        private void RefreshMusicListView()
        {
            string query = "SELECT * FROM \"song\" WHERE \"album_id_fk\" = @AlbumId";
            MusicListView.ItemsSource = _dbContext.Songs.FromSqlRaw(query, new NpgsqlParameter("@AlbumId", CurrentAlbum.AlbumId)).ToList();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            };
        }
    }
}
