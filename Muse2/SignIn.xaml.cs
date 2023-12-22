using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Muse2
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class SignIn : Window
    {
        UserVM loggedInUser = null;
        private string resetCode = "";
        string newPassword = "";
        string confirmPassword = "";
        string tempEmail = "";
        string email = "";
        string oldpass = "password";

        public SignIn()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnShowPasswordTop.Visibility = Visibility.Hidden;
            btnShowPasswordBottom.Visibility = Visibility.Visible;
            btnBack.Visibility = Visibility.Hidden;
            txtEmail.Text = "67Easton@gmail.com";
            pwdPassword.Password = "password";
            stkEmailPass.Visibility = Visibility.Visible;
            stkConfirmEmailPass.Visibility = Visibility.Hidden;
            stkVerificationCode.Visibility = Visibility.Collapsed;
        }

        private void CodeCheck()
        {
            string userEnteredCode = txtCode1.Text + txtCode2.Text + txtCode3.Text + txtCode4.Text + txtCode5.Text + txtCode6.Text;

            if (resetCode.Equals(userEnteredCode))
            {
                // wipe away the reset password code entry portion
                stkVerificationCode.Visibility = Visibility.Hidden;
                btnLogin.Content = "Reset Password";

                // set up the reset password portion
                stkEmailPass.Visibility = Visibility.Hidden;
                stkConfirmEmailPass.Visibility = Visibility.Visible;
            }
        }
        private void PasswordCheck()
        {
            stkConfirmEmailPass.Visibility = Visibility.Visible;

            txtCode1.Text = "";
            txtCode2.Text = "";
            txtCode3.Text = "";
            txtCode4.Text = "";
            txtCode5.Text = "";
            txtCode6.Text = "";

            // open up the inputs for the new password and confirming the new password

            string newPassword = pwdNewPassword.Password;
            string confirmPassword = pwdConfirmNewPassword.Password;

            if (!newPassword.IsValidPassword() || !confirmPassword.IsValidPassword())
            {
                MessageBox.Show("That is not a valid password", "Invalid Password",
                MessageBoxButton.OK, MessageBoxImage.Error);
                pwdNewPassword.Password = "";
                pwdConfirmNewPassword.Password = "";
                pwdNewPassword.Focus();
                return;
            }
            if (pwdNewPassword.Password == pwdConfirmNewPassword.Password)
            {
                try
                {
                    UserManager um = new UserManager();
                    um.ResetPassword(email, oldpass, newPassword);
                    MessageBox.Show("Password successfully changed!");
                }
                catch (Exception ex)
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

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (btnLogin.Content.Equals("Reset Password"))
            {
                stkEmailPass.Visibility = Visibility.Collapsed;
                btnShowPasswordTop.Visibility = Visibility.Visible;
                PasswordCheck();
            }
            if (pwdPassword.Visibility == Visibility.Hidden)
            {
                try
                {
                    if (btnLogin.Content.Equals("Enter Code"))
                    {
                        CodeCheck();
                    }
                    if (btnLogin.Content.Equals("Send Email"))
                    { 
                        // Set up the entry textboxes for code entry
                        txtCode1.Focus();
                        btnLogin.Content = "Enter Code";
                        stkVerificationCode.Visibility = Visibility.Visible;

                        // don't let this be the email until it is checked and verified
                        tempEmail = txtEmail.Text;

                        if (!tempEmail.IsValidEmail())
                        {
                            MessageBox.Show("That is not a valid email address", "Invalid Email",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                            txtEmail.SelectAll();
                            txtEmail.Focus();
                            return;
                        }
                        stkEmailPass.Visibility = Visibility.Hidden;
                        SendEmail();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Email Not Sent. Try a different email.");
                    throw;
                }
            }
            if (pwdPassword.Visibility == Visibility.Visible)
            {
                var email = txtEmail.Text;
                var password = pwdPassword.Password;

                if (!email.IsValidEmail())
                {
                    MessageBox.Show("That is not a valid email address", "Invalid Email",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    txtEmail.SelectAll();
                    txtEmail.Focus();
                    return;
                }

                if (!password.IsValidPassword())
                {
                    MessageBox.Show("That is not a valid password", "Invalid Password",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    pwdPassword.SelectAll();
                    pwdPassword.Focus();
                    return;
                }
                try
                {
                    UserManager _um = new UserManager();
                    loggedInUser = _um.LoginUser(email, password);
                    var home = new MainWindow(loggedInUser);
                    this.Close();
                    home.ShowDialog();
                }
                catch (Exception ex)
                {
                    // you may never throw exceptions from the presentation layer
                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Login Failed",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    pwdPassword.SelectAll();
                    pwdPassword.Clear();
                    txtEmail.Focus();
                    return;
                }
            }
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            stkConfirmEmailPass.Visibility = Visibility.Hidden;
            stkEmailPass.Visibility = Visibility.Visible;
            pwdPassword.Visibility = Visibility.Visible;
            txtPasswordLabel.Visibility = Visibility.Visible;
            btnForgotPassword.Visibility = Visibility.Visible;
            txtSubHeader.Text = "Control what you listen to.";

            btnBack.Content = "Back";
            btnBack.Visibility = Visibility.Hidden;
            btnLogin.Content = "Login";
            btnForgotPassword.Content = "Forgot Password";
            txtEmail.Text = "";
            pwdPassword.Password = "";
            txtEmail.Focus();
        }
        private void btnForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            txtSubHeader.Text = "Send this code where?";
            txtPasswordLabel.Visibility = Visibility.Hidden;
            pwdPassword.Visibility = Visibility.Hidden;
            btnBack.Visibility = Visibility.Visible;
            btnBack.Content = "Back";
            btnLogin.Content = "Send Email";
            btnForgotPassword.Visibility = Visibility.Hidden;
        }
        #region Async Send Email Verification
        private void SendEmail()
        {
            // generate the reset code
            Random rnd = new Random();
            resetCode = rnd.Next(111111, 999999).ToString();
            string result = Task.Run(() => requestAPI(resetCode, "Password Reset Code: ")).Result;
            MessageBox.Show("Email Sent Successfully!");
            txtSubHeader.Text = "Enter the code sent to your email";
            txtCode1.Focus();
        }
        private async Task<string> requestAPI(string message, string subject)
        {
            WebClient wc = new WebClient();
            return await wc.DownloadStringTaskAsync($"http://127.0.0.1:8000/email/?recipient={tempEmail}&subject={subject}&body={message}");
        }
        #endregion
        #region Code Checks
        private void txtCode1_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            if(txtCode1.Text != "")
            {
                txtCode2.Focus();
            }
        }
        private void txtCode2_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            if (txtCode2.Text != "")
            {
                txtCode3.Focus();
            }
        }
        private void txtCode3_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            if (txtCode3.Text != "")
            {
                txtCode4.Focus();
            }
        }
        private void txtCode4_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            if (txtCode4.Text != "")
            {
                txtCode5.Focus();
            }
        }
        private void txtCode5_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            if (txtCode5.Text != "")
            {
                txtCode6.Focus();
            }
        }
        private void txtCode6_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            if (txtCode6.Text != "")
            {
                btnLogin.Focus();
            }
        }
        #endregion

        private void btnShowPasswordBottom_Click(object sender, RoutedEventArgs e)
        {
            txtShownPassword.Text = pwdPassword.Password;
        }
    }
}
