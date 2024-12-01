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
    public partial class AddPlaylistPage : Window
    {
        MusicApplicationContext _dbContext;
        Listener currentListener;
        string selectedImagePath;
        ObservableCollection<Playlist> Playlists { get; set; }
        public AddPlaylistPage(Listener currentListener, ObservableCollection<Playlist> ListenerPlaylists)
        {
            this.currentListener = currentListener;
            Playlists = ListenerPlaylists;
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
            string playlistName = TxtPlaylistName.Text;
            if (playlistName == null || playlistName.Length == 0)
            {
                MessageBox.Show("Empty playlist name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            string description = TxtDescription.Text;
            string uniqueFileName = System.IO.Path.Combine("C:\\Users\\Notebook\\Desktop\\ImagesForMS",
                   $"{Guid.NewGuid()}{System.IO.Path.GetExtension(selectedImagePath)}");

            System.IO.File.Copy(selectedImagePath, uniqueFileName);

            var queryInsert = "INSERT INTO playlist (listener_id_fk, title, description, cover_image_path) VALUES (@ListenerId, @Title, @Description, @FilePath)";
            var parametersInsert = new[]
            {
                new NpgsqlParameter("@ListenerId", NpgsqlDbType.Integer) { Value = currentListener.ListenerId },
                new NpgsqlParameter("@Title", NpgsqlDbType.Varchar) { Value = playlistName },
                new NpgsqlParameter("Description", NpgsqlDbType.Varchar) { Value = description },
                new NpgsqlParameter("@FilePath", NpgsqlDbType.Varchar) { Value = uniqueFileName }
            };
            _dbContext.Database.ExecuteSqlRaw(queryInsert, parametersInsert);


            var playlistLast = _dbContext.Playlists.FromSqlRaw("SELECT * FROM \"playlist\" ORDER BY playlist_id DESC LIMIT 1").FirstOrDefault();

            Playlists.Add(new Playlist
            {
                PlaylistId = playlistLast.PlaylistId,
                ListenerIdFk = currentListener.ListenerId,
                Title = playlistName,
                Description = description,
                CoverImagePath = uniqueFileName,
                SongCount = 0,
                TotalDuration = 0
            });

            MessageBox.Show("The playlist added successfully!");
            Window.GetWindow(this)?.Close();
        }
    }
}
