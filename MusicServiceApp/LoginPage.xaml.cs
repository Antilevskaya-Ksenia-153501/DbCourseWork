using Microsoft.EntityFrameworkCore;
using MusicService.Data;
using Npgsql;
using System.Windows;
using System.Windows.Controls;


namespace MusicService
{
    public partial class LoginPage : Page
    {
        MusicApplicationContext _dbContext;
        public LoginPage()
        {
            _dbContext = new MusicApplicationContext();
            InitializeComponent();  
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = TxtUsername.Text;
            string password = TxtPassword.Password;
            string query = "SELECT * FROM \"user\" WHERE \"username\" = @Username AND \"password\" = @Password";
            var user = _dbContext.Users.FromSqlRaw(query, new NpgsqlParameter("@Username", username), new NpgsqlParameter("@Password", password)).FirstOrDefault();

            if (user != null)
            {
                MessageBox.Show("Login successful!");
                TxtUsername.Text = string.Empty;
                TxtPassword.Password = string.Empty;
                if (user.RoleIdFk == 1)
                {
                    var queryGetListener = "SELECT * FROM \"listener\" WHERE \"user_id_fk\" = @UserId";
                    var currentListener = _dbContext.Listeners.FromSqlRaw(queryGetListener, new NpgsqlParameter("@UserId", user.UserId)).FirstOrDefault();
                    ListenerPage lstPage = new(currentListener);
                    NavigationService?.Navigate(lstPage);
                }
                else if (user.RoleIdFk == 2)
                {
                    var queryGetArtist = "SELECT * FROM \"artist\" WHERE \"user_id_fk\" = @UserId";
                    var currentArtist = _dbContext.Artists.FromSqlRaw(queryGetArtist, new NpgsqlParameter("@UserId", user.UserId)).FirstOrDefault();
                    ArtistPage artPage = new(currentArtist);
                    NavigationService?.Navigate(artPage);
                }
            }
            else
                MessageBox.Show("Invalid username or password. Please try again.");
        }
    }
}