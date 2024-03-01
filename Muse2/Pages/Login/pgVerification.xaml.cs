using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Muse2.Pages.Login
{
    /// <summary>
    /// Interaction logic for pgVerification.xaml
    /// </summary>
    public partial class pgVerification : Page
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
        bool btnShowPasswordTopIsClicked;
        bool btnShowPasswordBottomIsClicked;
        bool emailSent;

        public pgVerification()
        {
            InitializeComponent();
        }
       
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            stkPassword.Visibility = Visibility.Hidden;
            stkConfirmPassword.Visibility = Visibility.Hidden;
            stkVerificationCode.Visibility = Visibility.Hidden;
            btnShowPasswordTop.Visibility = Visibility.Hidden;
            btnShowPasswordBottom.Visibility = Visibility.Hidden;
            txtShownPasswordTop.Visibility = Visibility.Hidden;
            txtShownPasswordBottom.Visibility = Visibility.Hidden;
            txtEmail.Text = "67Easton@gmail.com";
            txtEmail.Focus();
        }
        #region Validation
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
        #endregion
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
        #region Async Send Email Verification
        private void SendEmail()
        {
            // generate the reset code
            Random rnd = new Random();
            resetCode = rnd.Next(111111, 999999).ToString();
            try
            {
                string result = Task.Run(() => requestAPI(resetCode, "Password Reset Code: ")).Result;
                emailSent = true;
                stkVerificationCode.Visibility = Visibility.Visible;
                txtCode1.Focus();
            }
            catch (Exception)
            {
                MessageBox.Show("We are unable to make your request to our email server. Please try again later.", "Unable to send email");
                return;
            }
        }
        private async Task<string> requestAPI(string message, string subject)
        {
            WebClient wc = new WebClient();
            return await wc.DownloadStringTaskAsync($"http://127.0.0.1:8000/email/?recipient={tempEmail}&subject={subject}&body={message}");
        }
        #endregion
        #region Code Check
        private void txtCode1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = numericalRegex.IsMatch(e.Text);

            if (txtCode1.Text != "")
            {
                txtCode2.Focus();
            }
        }

        private void txtCode2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = numericalRegex.IsMatch(e.Text);

            if (txtCode2.Text != "")
            {
                txtCode3.Focus();
            }
        }

        private void txtCode3_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = numericalRegex.IsMatch(e.Text);

            if (txtCode3.Text != "")
            {
                txtCode4.Focus();
            }
        }

        private void txtCode4_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = numericalRegex.IsMatch(e.Text);

            if (txtCode4.Text != "")
            {
                txtCode5.Focus();
            }
        }

        private void txtCode5_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = numericalRegex.IsMatch(e.Text);

            if (txtCode5.Text != "")
            {
                txtCode6.Focus();
            }
        }

        private void txtCode6_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = numericalRegex.IsMatch(e.Text);

            if (txtCode6.Text != "")
            {
                btnEnterCode.Focus();
            }
        }

        #endregion
        private void btnEnterCode_Click(object sender, RoutedEventArgs e)
        {
            if(btnEnterCode.Content.Equals("SEND EMAIL"))
            {
                tempEmail = txtEmail.Text;

                if (!tempEmail.IsValidEmail())
                {
                    MessageBox.Show("That is not a valid email address", "Invalid Email",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    txtEmail.SelectAll();
                    txtEmail.Focus();
                    return;
                }
                stkForgetPasswordInputs.Visibility = Visibility.Hidden;
                SendEmail();
                if(emailSent == true)
                {
                    btnEnterCode.Content = "ENTER CODE";
                    return;
                }
            }
            if (btnEnterCode.Content.Equals("RESET PASSWORD"))
            {
                ResetPassword();
                return;
            }
            if (btnEnterCode.Content.Equals("ENTER CODE"))
            {
                try
                {
                    string userEnteredCode = txtCode1.Text + txtCode2.Text + txtCode3.Text + txtCode4.Text + txtCode5.Text + txtCode6.Text;

                    if (resetCode.Equals(userEnteredCode))
                    {
                        stkForgetPasswordInputs.Visibility = Visibility.Visible;
                        lblPassword.Content = "New Password";
                        stkPassword.Visibility = Visibility.Visible;
                        stkConfirmPassword.Visibility = Visibility.Visible;
                        stkVerificationCode.Visibility = Visibility.Hidden;
                        btnShowPasswordTop.Visibility = Visibility.Visible;
                        btnShowPasswordBottom.Visibility = Visibility.Visible;
                        btnEnterCode.Content = "RESET PASSWORD";
                        txtEmail.Focus();
                    }
                    else
                    {
                        SendEmail();
                        MessageBox.Show("A different code was sent to your email.","Incorrect code");
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
        #region Show Password Buttons
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
        #endregion
    }
}
