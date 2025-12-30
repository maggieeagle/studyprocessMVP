using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using UI.ViewModels;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for UserEditWindow.xaml
    /// </summary>
    public partial class UserEditWindow : Window
    {
        public UserEditWindow(UserEditViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            // Close window if Save is successful
            viewModel.OnRequestClose += (s, e) => this.Close();
        }

        private void PassBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserEditViewModel vm)
            {
                vm.Password = ((PasswordBox)sender).Password;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
