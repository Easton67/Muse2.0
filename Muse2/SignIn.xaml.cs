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
        bool btnShowPasswordBottomIsClicked;
        bool btnShowPasswordTopIsClicked;
        Regex numericalRegex = new Regex("[^0-9]+");

        public SignIn()
        {
            InitializeComponent();
            frmSignIn.Navigate(new pgSignIn());
            frmSignUp.Navigate(new pgSignUp());
            frmVerification.Navigate(new pgVerification());
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            frmSignIn.Visibility = Visibility.Visible;
        }
        private void frmClear()
        {
            txtSubHeader.Text = "Control what you listen to.";
            frmSignIn.Visibility = Visibility.Collapsed;
            frmSignUp.Visibility = Visibility.Collapsed;
            frmVerification.Visibility = Visibility.Collapsed;
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (btnBack.Content.Equals("Sign Up"))
            {
                frmClear();
                txtSubHeader.Text = "Sign up for Muse.";
                frmSignUp.Visibility = Visibility.Visible;
                btnBack.Content = "Back";
            }
            else
            {
                frmClear();
                frmSignIn.Visibility = Visibility.Visible;
                btnBack.Content = "Sign Up";
                // check if it we are on main screen to turn forgot password back to forgot password
                if (txtSubHeader.Text.Equals("Control what you listen to."))
                {
                    frmClear();
                    frmSignIn.Visibility = Visibility.Visible;
                    btnForgotPassword.Content = "Forgot Password";
                }
            }
        }
        private void btnForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            if (btnForgotPassword.Content.Equals("Forgot Password"))
            {
                frmClear();
                txtSubHeader.Text = "Send this code where?";
                frmVerification.Visibility = Visibility.Visible;
                btnForgotPassword.Content = "Sign In";
            }
            else
            {
                frmClear();
                frmSignIn.Visibility = Visibility.Visible;
                btnForgotPassword.Content = "Forgot Password";
            }
        }
    }
}
