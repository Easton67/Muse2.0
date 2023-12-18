using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
using System.Net;
using System.Security.Cryptography.X509Certificates;
using DataObjects;
using Muse2;
using LogicLayer;

namespace Wpf_LoginForm.View
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class SignIn : Window
    {
        private string validEmail = "";
        UserManager _userManager = null;
        UserVM loggedInUser = null;
        public SignIn()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnBack.Visibility = Visibility.Hidden;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if(pwdPassword.Visibility == Visibility.Hidden)
            {
                try
                {
                    //SendEmail();
                    //MessageBox.Show("Email Sent Successfully!");
                }
                catch (Exception)
                {
                    MessageBox.Show("Email Not Sent. Try a different email.");
                    throw;
                }
            }
            if(pwdPassword.Visibility == Visibility.Visible)
            {
                string email = txtEmail.Text;
                string password = pwdPassword.Password;

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
                    loggedInUser = _userManager.LoginUser(email, password);
                    var home = new MainWindow(loggedInUser);
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

        private void btnForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            txtSubHeader.Text = "Enter the email you would like to send the reset password link to below";
            txtPasswordLabel.Visibility = Visibility.Hidden;
            pwdPassword.Visibility = Visibility.Hidden;
            btnBack.Visibility = Visibility.Visible;
            btnBack.Content = "Back";
            btnLogin.Content = "Send Email";
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            txtSubHeader.Text = "Control what you listen to.";
            txtPasswordLabel.Visibility = Visibility.Visible;
            pwdPassword.Visibility = Visibility.Visible;
            btnBack.Visibility = Visibility.Hidden;
            btnBack.Content = "Back";
            btnLogin.Content = "Login";
            txtEmail.Text = "";
            txtEmail.Focus();
        }

        private void SendEmail()
        {
            //MailMessage mailMessage = new MailMessage();
            //mailMessage.From = new MailAddress("nicholasgurr502@gmail.com");
            //mailMessage.To.Add("nicholasgurr502@gmail.com");
            //mailMessage.Subject = "Subject";
            //mailMessage.Body = "This is test email";

            //System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
            //smtpClient.Host = "smtp.google.com";
            //smtpClient.Port = 587;
            //smtpClient.UseDefaultCredentials = false;
            //smtpClient.Credentials = new NetworkCredential("nicholasgurr502@gmail.com", "MuseCodingApplication");
            //smtpClient.EnableSsl = true;

            //try
            //{
            //    smtpClient.Send(mailMessage);
            //    MessageBox.Show("Email Sent Successfully.");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error: " + ex.Message);
            //}
        }
    }
}
