using Microsoft.EntityFrameworkCore;
using MusicService.Data;
using MusicService.Models;
using Npgsql;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MusicService
{
    public partial class ArtistSongs : Page
    {
        MusicApplicationContext _dbContext;
        Artist currentArtist;
        public Album[] Albums { get; set; }
        public ObservableCollection<Song> Songs { get; set; }

        public ArtistSongs(Artist currentArtist)
        {
            _dbContext = new MusicApplicationContext();
            this.currentArtist = currentArtist;

            string query = "SELECT * FROM \"song\" WHERE \"artist_id_fk\" = @ArtistId";
            Songs = new ObservableCollection<Song>(_dbContext.Songs.FromSqlRaw(query,
                new NpgsqlParameter("@ArtistId", currentArtist.ArtistId)).ToList());

            Albums = _dbContext.Albums.FromSqlRaw("SELECT * FROM \"album\"").ToArray();


            DataContext = this;
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddSongPage page = new(currentArtist, this.Songs);
            page.Show();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var song = (Song)MusicListView.SelectedItem;
            Songs.Remove(song);

            string query = "DELETE FROM song WHERE song_id = @SongId";
            _dbContext.Database.ExecuteSqlRaw(query, new NpgsqlParameter("@SongId", song.SongId));
            _dbContext.SaveChanges();
        }

        private void AddToAlbum_CLick(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ListView albumsListView) throw new ArgumentException("incorrect sender", nameof(sender));

            albumsListView.SelectionChanged -= AddToAlbum_CLick;

            var currentSong = (Song)MusicListView.SelectedItem;
            var album = (Album)albumsListView.SelectedItem;

            album.Songs.Add(currentSong);

            string query = "UPDATE song SET album_id_fk = @AlbumId WHERE song_id = @SongId";
            var parameters = new[]
            {
                new NpgsqlParameter("@AlbumId", album.AlbumId),
                new NpgsqlParameter("@SongId", currentSong.SongId)
            };
            _dbContext.Database.ExecuteSqlRaw(query, parameters);
            _dbContext.SaveChanges();

            albumsListView.SelectedItem = null;

            albumsListView.SelectionChanged += AddToAlbum_CLick;
        }

        private void AddToAlbum_MenuItem_MouseEnter(object sender, MouseEventArgs e)
        {
            AddToAlbumPopup.PlacementTarget ??= (MenuItem)sender;
            AddToAlbumPopup.IsOpen = false;
            AddToAlbumPopup.IsOpen = true;
        }

        private void AddToAlbum_MenuItem_MouseLeave(object sender, MouseEventArgs e)
        {
            e.Handled = true;
            var inputElement = (MenuItem)sender;
            var mousePos = Mouse.GetPosition(inputElement);
            var isMouseOverPopup = mousePos.X > inputElement.ActualWidth - 5.0 &&
                                   (mousePos.Y >= 0 || mousePos.Y <= inputElement.ActualHeight);

            if (!isMouseOverPopup)
            {
                AddToAlbumPopup.IsOpen = false;
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