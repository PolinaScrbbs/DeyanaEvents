using System.Windows;
using System.Windows.Controls;
using DeyanaEvents.Data;
using DeyanaEvents.Models;

namespace DeyanaEvents.Views
{
    public partial class Index : Page
    {
        private User _user; // Храним объект пользователя
        private ApplicationDbContext _context;

        // Принимаем объект пользователя целиком
        public Index(User user, ApplicationDbContext context)
        {
            InitializeComponent();

            // Устанавливаем объект пользователя
            _user = user;
            _context = context;

            if (_user != null)
            {
                // Устанавливаем имя пользователя в поле WelcomeText
                WelcomeText.Text = $"Добро пожаловать, @{_user.Username} ({_user.FullName})!";
            }
            else
            {
                MessageBox.Show("Пользователь не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика выхода — переходим на страницу логина
            var loginPage = new Login(_context); // Страница логина
            NavigationService.Navigate(loginPage);
        }
    }
}
