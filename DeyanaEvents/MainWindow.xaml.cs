using DeyanaEvents.Data;
using DeyanaEvents.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;

namespace DeyanaEvents
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var connectionString = "Host=localhost;Database=DeyanaEvents;Username=PolinaScrbbs;Password=Que337";
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(connectionString)
                .Options;

            var dbContext = new ApplicationDbContext(options);
            if (dbContext == null)
            {
                MessageBox.Show("Ошибка при инициализации контекста базы данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Убедитесь, что передача контекста в Registration происходит корректно
            var registrationPage = new Registration();
            registrationPage.SetDbContext(dbContext);

            MainFrame.Navigate(registrationPage);
        }
    }

}
