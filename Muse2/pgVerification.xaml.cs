using DataObjects;
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

namespace Muse2
{
    /// <summary>
    /// Interaction logic for pgVerification.xaml
    /// </summary>
    public partial class pgVerification : Page
    {
        string resetCode;
        string tempEmail;
        Regex numericalRegex = new Regex("[^0-9]+");
        Page pgSignUp = new pgSignUp();
        public pgVerification()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            stkVerificationCode.Visibility = Visibility.Hidden;
        }
        private void CodeCheck()
        {
            string userEnteredCode = txtCode1.Text + txtCode2.Text + txtCode3.Text + txtCode4.Text + txtCode5.Text + txtCode6.Text;

            if (resetCode.Equals(userEnteredCode))
            {
                // reset password code
            }
        }
        #region Async Send Email Verification
        private void SendEmail()
        {
            // generate the reset code
            Random rnd = new Random();
            resetCode = rnd.Next(111111, 999999).ToString();
            string result = Task.Run(() => requestAPI(resetCode, "Password Reset Code: ")).Result;
            MessageBox.Show("Email Sent Successfully!");
            txtCode1.Focus();
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
                stkEmailPass.Visibility = Visibility.Hidden;
                SendEmail();
                btnEnterCode.Content = "ENTER CODE";
            }
            if(btnEnterCode.Content.Equals("ENTER CODE"))
            {
                CodeCheck();
            }
        }
    }
}
