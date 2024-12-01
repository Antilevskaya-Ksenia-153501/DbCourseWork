using Microsoft.EntityFrameworkCore;
using MusicService.Data;
using MusicService.Models;
using Npgsql;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace MusicService
{
    public partial class ArtistAlbums : Page
    {
        MusicApplicationContext _dbContext;
        Artist currentArtist;
        public ObservableCollection<Album> Albums { get; set; }
        public ArtistAlbums(Artist currentArtist)
        {
            _dbContext = new MusicApplicationContext();
            this.currentArtist = currentArtist;
            string query = "SELECT * FROM \"album\" WHERE \"artist_id_fk\" = @ArtistId";
            Albums = new ObservableCollection<Album>(_dbContext.Albums.FromSqlRaw(query,
                new NpgsqlParameter("@ArtistId", currentArtist.ArtistId)).ToList());
            DataContext = this;
            InitializeComponent();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddAlbumPage page = new(currentArtist, Albums);
            page.Show();
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = (Button)sender;
            Album? album = deleteButton.DataContext as Album ;
            Albums.Remove(album);

            string query = "DELETE FROM album WHERE album_id = @AlbumId";
            _dbContext.Database.ExecuteSqlRaw(query, new NpgsqlParameter("@AlbumId", album.AlbumId));
            _dbContext.SaveChanges();
        }

        private void GetAlbumSongsButton_Click(object sender, RoutedEventArgs e)
        {
            Button getSongsButton = (Button)sender;
            Album album = getSongsButton.DataContext as Album;
            GetAlbumSongs page = new(album);
            NavigationService?.Navigate(page);
        }
    }
}
