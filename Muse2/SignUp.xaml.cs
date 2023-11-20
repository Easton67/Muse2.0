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
using DataObjects;
using LogicLayer;
using Microsoft.Win32;

namespace Muse2
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        private string _accountImage;
        private string _email;
        private string _password;

        public SignUp()
        {
            InitializeComponent();
        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (txtProfileName.Text != "" && txtEmail.Text != "" && pwdPassword.Password != "" && pwdConfirmPassword.Password != "")
            {
                if (pwdPassword.Password == pwdConfirmPassword.Password)
                {
                    // Set the variables
                    _password = pwdPassword.Password;
                    _email = txtEmail.Text;

                    // Validation Checks
                    if (!_email.IsValidEmail())
                    {
                        MessageBox.Show("That is not a valid email address", "Invalid Email",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        txtEmail.SelectAll();
                        txtEmail.Focus();
                        return;
                    }
                    if (!_password.IsValidPassword())
                    {
                        MessageBox.Show("That is not a valid password", "Invalid Password",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        pwdPassword.SelectAll();
                        pwdPassword.Focus();
                        return;
                    }

                    // Hide current and show new screen
                    imgAcccountImage.Visibility = Visibility.Visible;
                    btnOpenFile.Visibility = Visibility.Visible;
                    btnNext.Visibility = Visibility.Hidden;
                    btnConfirmSignUp.Visibility = Visibility.Visible;
                    lblHeader.Content = "Choose a Profile Picture";

                    lblProfileName.Visibility = Visibility.Hidden;
                    lblEmail.Visibility = Visibility.Hidden;
                    lblPassword.Visibility = Visibility.Hidden;
                    lblConfirmPassword.Visibility = Visibility.Hidden;

                    txtProfileName.Visibility = Visibility.Hidden;
                    txtEmail.Visibility = Visibility.Hidden;
                    pwdPassword.Visibility = Visibility.Hidden;
                    pwdConfirmPassword.Visibility = Visibility.Hidden;
                }
                else
                {
                    MessageBox.Show("Password and confirm password don't match." + "\n\n" + "Please re-enter them");
                    pwdPassword.Password = string.Empty;
                    pwdConfirmPassword.Password = string.Empty;
                    pwdPassword.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Please enter all fields to continue");
            }
        }
        private void btnConfirmSignUp_Click(object sender, RoutedEventArgs e)
        {
            // Create the user
            var newUser = new User()
            {
                ProfileName = txtProfileName.Text,
                Email = txtEmail.Text,
                ImageFilePath = _accountImage
            };
            try
            {
                UserManager um = new UserManager();
                um.InsertUser(newUser, _password);
                MessageBox.Show("Account Created!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "User could not be created. Please try again.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnConfirmSignUp.Visibility = Visibility.Hidden;
            btnOpenFile.Visibility = Visibility.Hidden;
            imgAcccountImage.Visibility = Visibility.Hidden;

        }
        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.Title = "Open File";
                openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;)|*.jpg;*.jpeg;*.png;|All Files (*.*)|*.*";

                bool? result = openFileDialog.ShowDialog();

                if (result == true)
                {
                    _accountImage = openFileDialog.FileName;
                    var AccountImage = new BitmapImage(new System.Uri(_accountImage));
                    imgAcccountImage.Source = AccountImage;
                }
                else
                {
                    // user closes the file explorer before picking a photo
                    MessageBox.Show("Choose a photo to update your current account photo.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid image." + " " + ex.Message);
            }
        }
    }
}
