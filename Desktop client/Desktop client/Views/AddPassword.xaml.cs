using Desktop_client.ViewModels;
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

namespace Desktop_client.Views
{
    /// <summary>
    /// Логика взаимодействия для AddPassword.xaml
    /// </summary>
    public partial class AddPassword : Page
    {
        public AddPassword()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AddPasswordViewModel viewModel = DataContext as AddPasswordViewModel;

            PasswordBox.Password = viewModel.Password;
            viewModel.PasswordGeneratedEvent += PasswordGenerated;

            if (viewModel.Title == "Изменение пароля")
            {
                PasswordBox.Password = viewModel.PasswordTemp;
                viewModel.Password = viewModel.Password;
            }
        }

        private void PasswordGenerated(string password)
        {
            PasswordBox.Password = password;
        }
    }
}
