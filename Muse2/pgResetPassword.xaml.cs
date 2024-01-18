using System;
using DataObjects;
using LogicLayer;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Text.RegularExpressions;

namespace Muse2
{
    /// <summary>
    /// Interaction logic for pgResetPassword.xaml
    /// </summary>
    public partial class pgResetPassword : Page
    {
        UserManager um = new UserManager();
        string resetCode;
        string tempEmail;
        Regex numericalRegex = new Regex("[^0-9]+");
        Page pgSignUp = new pgSignUp();
        string oldPassword;
        string newPassword;
        string email;
        UserPass oldUser;
        UserVM _loggedInUser = new UserVM();
        bool btnShowPasswordTopIsClicked;
        bool btnShowPasswordBottomIsClicked;
        bool emailSent;

        public pgResetPassword(UserVM loggedInUser)
        {
            InitializeComponent();

            if(loggedInUser != null)
            {
                _loggedInUser = loggedInUser;
                txtEmail.Text = loggedInUser.Email;
            }
        }
        private void SetPassword()
        {
            if (pwdPassword.Password == pwdConfirmPassword.Password && pwdPassword.Password.IsValidPassword())
            {
                newPassword = pwdConfirmPassword.Password;
            }
            else
            {
                MessageBox.Show("That is not a valid password", "Invalid Password",
                MessageBoxButton.OK, MessageBoxImage.Error);
                pwdPassword.SelectAll();
                pwdPassword.Focus();
                return;
            }
        }
        private void SetEmail()
        {
            MessageBox.Show(txtEmail.Text);
            email = txtEmail.Text;
            if (!email.IsValidEmail())
            {
                MessageBox.Show("That is not a valid email address", "Invalid Email",
                MessageBoxButton.OK, MessageBoxImage.Error);
                txtEmail.SelectAll();
                txtEmail.Focus();
                return;
            }
            else
            {
                email = txtEmail.Text;
            }
        }
        private void ResetPassword()
        {
            // run check if not all fields are put in
            if (txtEmail.Text == "" || pwdPassword.Password == "" || pwdConfirmPassword.Password == "")
            {
                MessageBox.Show("Please fill out all fields to continue.");
                return;
            }

            try
            {
                if (pwdPassword.Password == pwdConfirmPassword.Password)
                {
                    // seperated into different methods for validation checks
                    SetPassword();
                    SetEmail();

                    try
                    {
                        oldUser = um.SelectPasswordHashByEmail(email);
                        oldPassword = oldUser.PasswordHash;
                        um.ResetPassword(email, oldPassword, newPassword);
                        MessageBox.Show("Your password reset was successful!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not reset your password. Please try again.",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Password and confirm password do not match.");
                    pwdPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Unable to find your old password. Please try again.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void btnShowPasswordTop_Click(object sender, RoutedEventArgs e)
        {
            if (btnShowPasswordTopIsClicked == false)
            {
                txtShownPasswordTop.Visibility = Visibility.Visible;
                txtShownPasswordTop.Text = pwdPassword.Password;
                btnShowPasswordTopIsClicked = true;
                return;
            }
            else
            {
                pwdPassword.Visibility = Visibility.Visible;
                txtShownPasswordTop.Visibility = Visibility.Hidden;
                btnShowPasswordTopIsClicked = false;
            }
        }
        private void btnShowPasswordBottom_Click(object sender, RoutedEventArgs e)
        {
            if (btnShowPasswordBottomIsClicked == false)
            {
                txtShownPasswordBottom.Visibility = Visibility.Visible;
                txtShownPasswordBottom.Text = pwdConfirmPassword.Password;
                btnShowPasswordBottomIsClicked = true;
            }
            else
            {
                pwdConfirmPassword.Visibility = Visibility.Visible;
                txtShownPasswordBottom.Visibility = Visibility.Hidden;
                btnShowPasswordBottomIsClicked = false;
            }
        }
        private void btnResetPassword_Click(object sender, RoutedEventArgs e)
        {
            ResetPassword();
        }
    }
}
