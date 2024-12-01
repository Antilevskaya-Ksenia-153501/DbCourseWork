using Microsoft.EntityFrameworkCore;
using MusicService.Data;
using MusicService.Models;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
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
    
    public partial class AllSongs : Page
    {
        MusicApplicationContext _dbContext;
        Listener currentListener;
        public Playlist[] Playlists { get; set; }
        public ObservableCollection<Song> Songs { get; set; }
        public AllSongs(Listener currentListener)
        {
            _dbContext = new MusicApplicationContext();
            this.currentListener = currentListener;

            string query = "SELECT * FROM \"song\"";
            Songs = new ObservableCollection<Song>(_dbContext.Songs.FromSqlRaw(query).ToList());

            Playlists = _dbContext.Playlists.FromSqlRaw("SELECT * FROM \"playlist\" WHERE \"listener_id_fk\" = @ListenerId", 
                new NpgsqlParameter("@ListenerId", currentListener.ListenerId)).ToArray();

            DataContext = this;
            InitializeComponent();
        }

        private void AddToPlaylist_CLick(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ListView playlistsListView) throw new ArgumentException("incorrect sender", nameof(sender));

            playlistsListView.SelectionChanged -= AddToPlaylist_CLick;

            var currentSong = (Song)MusicListView.SelectedItem;
            var playlist = (Playlist)playlistsListView.SelectedItem;
            
            var query = "INSERT INTO playlist_song (playlist_id_fk, song_id_fk) VALUES (@PlaylistId,  @SongId)";
            var parameters = new[]
            {
                new NpgsqlParameter("@PlaylistId", playlist.PlaylistId),
                new NpgsqlParameter("@SongId", currentSong.SongId)
            };
            _dbContext.Database.ExecuteSqlRaw(query, parameters);
            _dbContext.SaveChanges();

            playlistsListView.SelectedItem = null;

            playlistsListView.SelectionChanged += AddToPlaylist_CLick;
        }

        private void AddToPlaylist_MenuItem_MouseEnter(object sender, MouseEventArgs e)
        {
            AddToPlaylistPopup.PlacementTarget ??= (MenuItem)sender;
            AddToPlaylistPopup.IsOpen = false;
            AddToPlaylistPopup.IsOpen = true;
        }

        private void AddToPlaylist_MenuItem_MouseLeave(object sender, MouseEventArgs e)
        {
            e.Handled = true;
            var inputElement = (MenuItem)sender;
            var mousePos = Mouse.GetPosition(inputElement);
            var isMouseOverPopup = mousePos.X > inputElement.ActualWidth - 5.0 &&
                                   (mousePos.Y >= 0 || mousePos.Y <= inputElement.ActualHeight);

            if (!isMouseOverPopup)
            {
                AddToPlaylistPopup.IsOpen = false;
            }
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
    }
}
