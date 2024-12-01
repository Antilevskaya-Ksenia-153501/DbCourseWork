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
    public partial class ArtistPage : Page
    {
        MusicApplicationContext _dbContext;
        Artist currentArtist;
        public ArtistPage(Artist currentArtist)
        {
            _dbContext = new MusicApplicationContext();
            this.currentArtist = currentArtist;
            InitializeComponent();
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new LoginPage());
        }
        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new LoginPage());
        }

        private void NavigateToSongs_Click(object sender, RoutedEventArgs e)
        {
            ArtistSongs artSongs = new(currentArtist);
            NavigationService?.Navigate(artSongs);
        }

        private void NavigateToAlbums_Click(object sender, RoutedEventArgs e)
        {
            ArtistAlbums artAlbums = new(currentArtist);
            NavigationService?.Navigate(artAlbums);
        }
    }
}
