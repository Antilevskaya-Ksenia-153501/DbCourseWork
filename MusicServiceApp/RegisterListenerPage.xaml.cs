using Microsoft.EntityFrameworkCore;
using MusicService.Data;
using Npgsql;
using NpgsqlTypes;
using System.Windows;
using System.Windows.Controls;


namespace MusicService
{
    public partial class RegisterPage : Page
    {
        MusicApplicationContext _dbContext;
        public RegisterPage()
        {
            InitializeComponent();
            _dbContext = new();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtName.Text;
            string surname = TxtUsername.Text;
            string username = TxtUsername.Text;
            DateTime dateAdded = BirthdatePicker.SelectedDate ?? DateTime.Now;
            DateTime birthDate = DateTime.SpecifyKind(dateAdded, DateTimeKind.Utc);
            string email = TxtEmail.Text;
            string phoneNumber = TxtPhoneNumber.Text;
            string password1 = TxtPassword.Password;
            string password2 = TxtConfirmPassword.Password;
            if (password1 != password2)
            {
                MessageBox.Show("Error! Invalid confirmation password.");
                return;
            }
           
            string query = "CALL add_listener(@Username, @Password, @Role, @Name, @Surname, @BirthDate, @Email, @Phone)";
            var parameters = new[]
            {
                new NpgsqlParameter("@Username", NpgsqlDbType.Varchar) { Value = username },
                new NpgsqlParameter("@Password", NpgsqlDbType.Varchar) { Value = password1 },
                new NpgsqlParameter("@Role", NpgsqlDbType.Varchar) { Value = "Listener" },
                new NpgsqlParameter("@Name", NpgsqlDbType.Varchar) { Value = name },
                new NpgsqlParameter("@Surname", NpgsqlDbType.Varchar) { Value = surname },
                new NpgsqlParameter("@BirthDate", NpgsqlDbType.Date) { Value = birthDate },
                new NpgsqlParameter("@Email", NpgsqlDbType.Varchar) { Value = email },
                new NpgsqlParameter("@Phone", NpgsqlDbType.Char) { Value = phoneNumber }
            };

            _dbContext.Database.ExecuteSqlRaw(query, parameters);
            _dbContext.SaveChanges();

            TxtName.Text = string.Empty;
            TxtSurname.Text = string.Empty;
            TxtUsername.Text = string.Empty;
            TxtEmail.Text = string.Empty;
            TxtPhoneNumber.Text = string.Empty;
            TxtPassword.Password = string.Empty;
            TxtConfirmPassword.Password = string.Empty;

            MessageBox.Show("You are registered successfully!");
            NavigationService?.GoBack();
        }
    }
}
