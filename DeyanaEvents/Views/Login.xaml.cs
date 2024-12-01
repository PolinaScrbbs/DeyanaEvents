using DeyanaEvents.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

namespace DeyanaEvents.Views
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow2.xaml
    /// </summary>
    public partial class Login : Page
    {
        private readonly ApplicationDbContext _context;

        public Login(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context;
        }

        private void Field_TextChanged(object sender, RoutedEventArgs e)
        {
            // Проверяем, какой элемент вызвал событие
            if (sender is TextBox textBox)
            {
                // Если текст в TextBox не пустой, скрываем Placeholder
                if (!string.IsNullOrEmpty(textBox.Text))
                {
                    // Скрываем Placeholder для Username или Email
                    if (textBox == UsernameField)
                    {
                        UsernamePlaceholder.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    // Если текст пустой, показываем Placeholder
                    if (textBox == UsernameField)
                    {
                        UsernamePlaceholder.Visibility = Visibility.Visible;
                    }
                }
            }

            // Аналогично для PasswordBox
            if (sender is PasswordBox passwordBox)
            {
                // Если пароль не пустой, скрываем Placeholder
                if (!string.IsNullOrEmpty(passwordBox.Password))
                {
                    PasswordPlaceholder.Visibility = Visibility.Collapsed;
                }
                else
                {
                    PasswordPlaceholder.Visibility = Visibility.Visible;
                }
            }
        }

        private void RegisterText_Click(object sender, MouseButtonEventArgs e)
        {
            // Переход на страницу регистрации
            var registrationPage = new Registration(); // Предположим, что у вас есть страница регистрации
            registrationPage.SetDbContext(_context);
            NavigationService.Navigate(registrationPage);
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            var username = UsernameField.Text;
            var password = PasswordField.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                MessageBox.Show("Неверное имя пользователя или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Если логин успешен, передаем объект пользователя в страницу Index
            var indexPage = new Index(user, _context); // Передаем объект пользователя
            NavigationService.Navigate(indexPage); // Навигация на IndexPage
        }
    }
}
