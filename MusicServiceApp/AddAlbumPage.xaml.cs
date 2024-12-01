using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using MusicService.Data;
using MusicService.Models;
using Npgsql;
using NpgsqlTypes;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MusicService
{
    public partial class AddAlbumPage : Window
    {
        MusicApplicationContext _dbContext;
        Artist currentArtist;
        string selectedImagePath;
        ObservableCollection<Album> Albums { get; set; }

        public AddAlbumPage(Artist currentArtist, ObservableCollection<Album> ArtistAlbums)
        {
            this.currentArtist = currentArtist;
            Albums = ArtistAlbums;
            _dbContext = new MusicApplicationContext();
            DataContext = this;
            selectedImagePath = "";
            InitializeComponent();
        }
        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                selectedImagePath = openFileDialog.FileName;
                BitmapImage bitmap = new(new Uri(selectedImagePath));
                imgSelectedImage.Source = bitmap;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string albumName = txtAlbumName.Text;
            if (albumName == null || albumName.Length == 0)
            {
                MessageBox.Show("Empty album name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            string uniqueFileName = System.IO.Path.Combine("C:\\Users\\Notebook\\Desktop\\ImagesForMS",
                   $"{Guid.NewGuid()}{System.IO.Path.GetExtension(selectedImagePath)}");

            System.IO.File.Copy(selectedImagePath, uniqueFileName);

            var dateAdded = dpDateAdded.SelectedDate ?? DateTime.Now;
            var utcDateTime = DateTime.SpecifyKind(dateAdded, DateTimeKind.Utc);
            var dateOnly = new DateOnly(utcDateTime.Year, utcDateTime.Month, utcDateTime.Day);

            var queryInsert = "INSERT INTO album(artist_id_fk, title, release_date, cover_image_path) VALUES(@ArtistId, @Title,  @ReleaseDate, @FilePath)";
            var parametersInsert = new[]
            {
                new NpgsqlParameter("@ArtistId", NpgsqlDbType.Integer) { Value = currentArtist.ArtistId },
                new NpgsqlParameter("@Title", NpgsqlDbType.Varchar) { Value = albumName },
                                new NpgsqlParameter("@ReleaseDate", NpgsqlDbType.Date) { Value = dateOnly },
                new NpgsqlParameter("@FilePath", NpgsqlDbType.Varchar) { Value = uniqueFileName }
            };

            _dbContext.Database.ExecuteSqlRaw(queryInsert, parametersInsert);


            var albumLast = _dbContext.Albums.FromSqlRaw("SELECT * FROM \"album\" ORDER BY album_id DESC LIMIT 1").FirstOrDefault();
            
            Albums.Add(new Album
            {
                AlbumId = albumLast.AlbumId,
                ArtistIdFk = currentArtist.ArtistId,
                Title = albumName,
                ReleaseDate = dateOnly,
                CoverImagePath = uniqueFileName,
                SongCount = 0,
                TotalDuration = 0
            }) ;

            txtAlbumName.Text = string.Empty;
            MessageBox.Show("The album added successfully!");
            Window.GetWindow(this)?.Close();
        }
    }
}
