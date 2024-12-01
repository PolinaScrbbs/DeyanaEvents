using System;
using System.Windows;
using System.Windows.Controls;
using DeyanaEvents.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using DeyanaEvents.Data;
using System.Windows.Input;

namespace DeyanaEvents.Views
{
    public partial class Registration : Page
    {
        private ApplicationDbContext _context;

        public Registration()
        {
            InitializeComponent();
        }

        // Метод для установки контекста базы данных
        public void SetDbContext(ApplicationDbContext context)
        {
            _context = context;
        }

        // Обработчик для событий изменения текста в полях
        private void Field_TextChanged(object sender, RoutedEventArgs e)
        {
            // Проверяем, какой элемент вызвал событие
            if (sender is TextBox textBox)
            {
                // Если текст в TextBox не пустой, скрываем Placeholder
                if (!string.IsNullOrEmpty(textBox.Text))
                {
                    // Скрываем Placeholder для Username или FullName
                    if (textBox == UsernameField)
                    {
                        UsernamePlaceholder.Visibility = Visibility.Collapsed;
                    }
                    else if (textBox == FullNameField)
                    {
                        FullNamePlaceholder.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    // Если текст пустой, показываем Placeholder
                    if (textBox == UsernameField)
                    {
                        UsernamePlaceholder.Visibility = Visibility.Visible;
                    }
                    else if (textBox == FullNameField)
                    {
                        FullNamePlaceholder.Visibility = Visibility.Visible;
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

        private void LoginText_Click(object sender, MouseButtonEventArgs e)
        {
            // Переход на страницу логина
            var loginPage = new Login(_context); // Предположим, что у вас есть страница логина
            NavigationService.Navigate(loginPage);
        }


        // Обработчик для кнопки регистрации
        private void RegisterClick(object sender, RoutedEventArgs e)
        {
            if (_context == null)
            {
                MessageBox.Show("Контекст базы данных не был инициализирован.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Получаем введенные данные
            var username = UsernameField.Text;
            var password = PasswordField.Password;
            var fullName = FullNameField.Text;

            // Проверяем на пустые значения
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Хешируем пароль
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            // Создаем объект пользователя
            var newUser = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                FullName = fullName,
                Email = null, // Если email может быть пустым, указываем null
                RoleId = 1, // Например, роль по умолчанию
            };

            try
            {
                // Сохраняем пользователя в базе данных
                _context.Users.Add(newUser);
                _context.SaveChanges();
                MessageBox.Show("Регистрация прошла успешно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Переключаем на страницу логина, передавая контекст
                var loginPage = new Login(_context); // Передаем контекст в Login
                NavigationService.Navigate(loginPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
