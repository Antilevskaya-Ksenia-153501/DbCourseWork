using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using MusicService.Data;
using MusicService.Models;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace MusicService
{
    public partial class AddSongPage : Window
    {
        MusicApplicationContext _dbContext;
        Artist currentArtist;
        string? selectedFilePath = null;
        ObservableCollection<Song> Songs { get; set; }
        public ObservableCollection<Genre> Genres { get; set; }

        List<Genre> selectedGenres;       
        public AddSongPage(Artist currentArtist, ObservableCollection<Song> ArtistSongs)
        {
            this.currentArtist = currentArtist;
            Songs = ArtistSongs;
            _dbContext = new MusicApplicationContext();

            selectedGenres = new List<Genre>();
            DataContext = this;
            Genres = new ObservableCollection<Genre>(_dbContext.Genres.FromSqlRaw("SELECT * FROM \"genre\"").ToList());
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is Genre genre)
            {
                if (checkBox.IsChecked == true)
                {
                    selectedGenres.Add(genre);
                }
            }
        }
        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Choose MP3 file",
                Filter = "Files MP3|*.mp3",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
                selectedFilePath = openFileDialog.FileName;
        }

        public int GetMp3Duration(string filePath)
        {
            int durationInSeconds = 0;

            try
            {
                var tagLibFile = TagLib.File.Create(filePath);
                if (tagLibFile != null)
                    durationInSeconds = Convert.ToInt32(tagLibFile.Properties.Duration.TotalSeconds);
            }
            catch (Exception ex)
            {
                // Обработка исключений, если они возникнут
            }
            return durationInSeconds;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string songName = txtSongName.Text;
            if (songName == null || songName.Length == 0)
            {
                MessageBox.Show("Empty song name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            int duration = GetMp3Duration(selectedFilePath);
            string uniqueFileName = System.IO.Path.Combine("C:\\Users\\Notebook\\Desktop\\MP3ForMS",
                    $"{Guid.NewGuid()}{System.IO.Path.GetExtension(selectedFilePath)}");

            File.Copy(selectedFilePath, uniqueFileName);
            
            var queryInsert = "INSERT INTO song(artist_id_fk, album_id_fk, title, file_path, duration) VALUES(@ArtistId, @AlbumId, @Title, @FilePath, @Duration)";
            var parametersInsert = new[]
            {
                new NpgsqlParameter("@ArtistId", NpgsqlDbType.Integer) { Value = currentArtist.ArtistId },
                new NpgsqlParameter("AlbumId", NpgsqlDbType.Integer) { Value = DBNull.Value },
                new NpgsqlParameter("@Title", NpgsqlDbType.Varchar) { Value = songName },
                new NpgsqlParameter("@FilePath", NpgsqlDbType.Varchar) { Value = uniqueFileName },
                new NpgsqlParameter("@Duration", NpgsqlDbType.Integer) { Value = duration }
            };
            
            _dbContext.Database.ExecuteSqlRaw(queryInsert, parametersInsert);


            var songId = _dbContext.Songs.FromSqlRaw("SELECT * FROM \"song\" ORDER BY song_id DESC LIMIT 1").FirstOrDefault();


            var queryGenre = "INSERT INTO song_genre (song_id_fk, genre_id_fk) VALUES ";
            var valueStrings = new List<string>();

            foreach (Genre selectedGenre in selectedGenres)
                valueStrings.Add($"({songId.SongId}, {selectedGenre.GenreId})");
            
            queryGenre += string.Join(", ", valueStrings);

            _dbContext.Database.ExecuteSqlRaw(queryGenre);
            

            txtSongName.Text = string.Empty;

            Songs.Add(new Song
            {
                SongId = songId.SongId,
                ArtistIdFk = currentArtist.ArtistId,
                AlbumIdFk = null,
                Title = songName,
                FilePath = uniqueFileName,
                Duration = duration
            });

            selectedGenres.Clear();
            MessageBox.Show("The song added successfully!");
            Window.GetWindow(this)?.Close();
        }
    }
}
