using MusicService.Data;
using MusicService.Models;
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
    public partial class ListenerPage : Page
    {
        MusicApplicationContext _dbContext;
        Listener currentListener;
        public ListenerPage(Listener currentListener)
        {
            _dbContext = new MusicApplicationContext();
            this.currentListener = currentListener;
            InitializeComponent();
        }
        private void Home_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Обработка действий при нажатии на гиперссылку
            // Здесь можно добавить код для перехода на другую страницу или выполнения других действий
        }
        private void Songs_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AllSongs allSongs = new(currentListener);
            NavigationService?.Navigate(allSongs);
        }
        private void Playlists_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListenerPlaylists listenerPlaylists = new(currentListener);
            NavigationService?.Navigate(listenerPlaylists);
        }
    }
}
