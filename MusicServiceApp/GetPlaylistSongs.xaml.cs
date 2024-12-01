using Microsoft.EntityFrameworkCore;
using MusicService.Data;
using MusicService.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicService
{
    
    public partial class GetPlaylistSongs : Page
    {
        MusicApplicationContext _dbContext;
        public Playlist CurrentPlaylist { get; set; }
        public ObservableCollection<Song> SongsInPlaylist { get; set; }
        public GetPlaylistSongs(Playlist playlist)
        {
            _dbContext = new MusicApplicationContext();
            CurrentPlaylist = playlist;

            string query = "SELECT s.* FROM song s INNER JOIN playlist_song ps ON s.song_id = ps.song_id_fk WHERE playlist_id_fk = @PlaylistId";
            SongsInPlaylist = new ObservableCollection<Song>(_dbContext.Songs.FromSqlRaw(query, new NpgsqlParameter("@PlaylistId", CurrentPlaylist.PlaylistId)).ToList());
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
            string query = "DELETE FROM playlist_song WHERE playlist_id_fk = @PlaylistId AND song_id_fk = @SongId";
            var parameters = new[]
            {
                new NpgsqlParameter("@PlaylistId", CurrentPlaylist.PlaylistId),
                new NpgsqlParameter("@SongId", song.SongId)
            };
            SongsInPlaylist.Remove(song);
            _dbContext.Database.ExecuteSqlRaw(query, parameters);
            _dbContext.SaveChanges();
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
