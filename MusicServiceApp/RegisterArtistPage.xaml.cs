using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using MusicService.Data;
using Npgsql;
using NpgsqlTypes;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace MusicService
{
    public partial class RegisterArtistPage : Page
    {
        MusicApplicationContext _dbContext;
        string selectedImagePath;
        public RegisterArtistPage()
        {
            _dbContext = new MusicApplicationContext();
            selectedImagePath = "";
            InitializeComponent();
            string query = "SELECT * FROM \"label\"";
            var labels = _dbContext.Labels.FromSqlRaw(query).ToList();
            foreach (MusicService.Models.Label label in labels)
            {
                RadioButton radioBtn = new();
                radioBtn.Content = label.Name;
                radioBtn.GroupName = "Labels";
                LabelsStackPanel.Children.Add(radioBtn);
            }
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                selectedImagePath = openFileDialog.FileName;
                BitmapImage bitmap = new(new Uri(selectedImagePath));
                ImgSelectedImage.Source = bitmap;
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtName.Text;
            string username = TxtUsername.Text;
            string uniqueFileName = Path.Combine("C:\\Users\\Notebook\\Desktop\\ImagesForMS",
                    $"{Guid.NewGuid()}{Path.GetExtension(selectedImagePath)}");

            File.Copy(selectedImagePath, uniqueFileName);
            string bio = TxtBiography.Text;
            string? label = null;
            string password1 = TxtPassword.Password;
            string password2 = TxtConfirmPassword.Password;
            if (password1 != password2)
            {
                MessageBox.Show("Error! Invalid confirmation password.");
                return;
            }

            foreach (RadioButton radioButton in LabelsStackPanel.Children)
            {
                if (radioButton.IsChecked == true)
                {
                    label = radioButton.Content.ToString();
                    break;
                }
            }
            if (string.IsNullOrEmpty(label))
            {
                MessageBox.Show("Error! Select a label.");
                return;
            }

            string query = "CALL add_artist(@Username, @Password, @Role, @Label, @Name, @Bio, @Image)";
            var parameters = new[]
            {
                new NpgsqlParameter("@Username", NpgsqlDbType.Varchar) { Value = username },
                new NpgsqlParameter("@Password", NpgsqlDbType.Varchar) { Value = password1 },
                new NpgsqlParameter("@Role", NpgsqlDbType.Varchar) { Value = "Artist" },
                new NpgsqlParameter("@Label", NpgsqlDbType.Varchar) { Value = label },
                new NpgsqlParameter("@Name", NpgsqlDbType.Varchar) { Value = name },
                new NpgsqlParameter("@Bio", NpgsqlDbType.Varchar) { Value = bio },
                new NpgsqlParameter("@Image", NpgsqlDbType.Varchar) { Value = uniqueFileName }
            };

            _dbContext.Database.ExecuteSqlRaw(query, parameters);
            _dbContext.SaveChanges();

            TxtName.Text = string.Empty;
            TxtBiography.Text = string.Empty;   
            TxtUsername.Text = string.Empty;
            TxtPassword.Password = string.Empty;
            TxtConfirmPassword.Password = string.Empty;

            MessageBox.Show("You are registered successfully!");
            NavigationService?.GoBack();
        }
    }
}
