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
    public partial class ListenerPlaylists : Page
    {
        MusicApplicationContext _dbContext;
        Listener currentListener;
        public ObservableCollection<Playlist> Playlists { get; set; }
        public ListenerPlaylists(Listener currentListener)
        {
            _dbContext = new MusicApplicationContext();
            this.currentListener = currentListener;
            string query = "SELECT * FROM \"playlist\" WHERE \"listener_id_fk\" = @ListenerId";
            Playlists = new ObservableCollection<Playlist>(_dbContext.Playlists.FromSqlRaw(query,
                new NpgsqlParameter("@ListenerId", currentListener.ListenerId)).ToList());
            DataContext = this;
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddPlaylistPage page = new(currentListener, Playlists);
            page.Show();
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = (Button)sender;
            Playlist? playlist = deleteButton.DataContext as Playlist;
            Playlists.Remove(playlist);

            string query = "DELETE FROM playlist WHERE playlist_id = @PlaylistId";
            _dbContext.Database.ExecuteSqlRaw(query, new NpgsqlParameter("@PlaylistId", playlist.PlaylistId));
            _dbContext.SaveChanges();
        }

        private void GetPlaylistSongsButton_Click(object sender, RoutedEventArgs e)
        {
            Button getSongsButton = (Button)sender;
            Playlist playlist = getSongsButton.DataContext as Playlist;
            GetPlaylistSongs page = new(playlist);
            NavigationService?.Navigate(page);
        }
    }
}
