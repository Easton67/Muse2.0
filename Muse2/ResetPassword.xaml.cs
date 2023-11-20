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
using System.Windows.Shapes;
using LogicLayer;
using DataObjects;

namespace Muse2
{
    /// <summary>
    /// Interaction logic for ResetPassword.xaml
    /// </summary>
    public partial class ResetPassword : Window
    {
        private string _email;

        public ResetPassword(string email)
        {
            InitializeComponent();
            _email = email;
        }
        private void btnResetPassword_Click(object sender, RoutedEventArgs e)
        {
            UserManager um = new UserManager();
            string oldpass = pwdOldPassword.Password;
            string newPassword = pwdNewPassword.Password;
            string confirmPassword = pwdConfirmNewPassword.Password;

            if (!newPassword.IsValidPassword() || !confirmPassword.IsValidPassword())
            {
                MessageBox.Show("That is not a valid password", "Invalid Password",
                MessageBoxButton.OK, MessageBoxImage.Error);
                pwdOldPassword.Password = "";
                pwdNewPassword.Password = "";
                pwdConfirmNewPassword.Password = "";
                pwdOldPassword.Focus();
                return;
            }
            if (pwdNewPassword.Password == pwdConfirmNewPassword.Password)
            {
                try
                {
                    um.ResetPassword(_email, oldpass, newPassword);
                    MessageBox.Show("Password successfully changed!");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not update your password. Please try again.",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Password and confirm password do not match.");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtEmail.Text = _email;
            pwdOldPassword.Focus();
        }
    }
}
