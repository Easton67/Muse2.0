using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Muse2.Pages.Login;
using DataObjects;
using System.Windows.Input;

namespace Muse2.Windows.Login
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class SignIn : Window
    {
        Dictionary<string, Page> pages = new Dictionary<string, Page>();
        UserVM _loggedInUser = null;

        public SignIn()
        {
            InitializeComponent();
        }

        #region Minimize, Close, Drag
        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        private void MinimizeImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void btnClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion

        public SignIn(UserVM loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CloseWindow.win = this;
            pages.Add("frmSignIn", new pgSignIn());
            pages.Add("frmSignUp", new pgSignUp());
            pages.Add("frmVerification", new pgVerification());
            pages.Add("frmResetPassword", new pgResetPassword(_loggedInUser));

            frmMain.Navigate(pages["frmSignIn"]);
        }
        public void NavigateToSignUp()
        {
            frmMain.NavigationService.Navigate(new pgSignUp());
        }
        public void NavigateToResetPassword()
        {
            frmMain.NavigationService.Navigate(new pgResetPassword(_loggedInUser));
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (btnBack.Content.Equals("Sign Up"))
            {
                txtSubHeader.Text = "Sign up for Muse.";
                frmMain.Navigate(pages["frmSignUp"]);
                btnBack.Content = "Back";
            }
            else
            {
                btnBack.Content = "Sign Up";

                // check if it we are on main screen to turn forgot password back to forgot password

                frmMain.Navigate(pages["frmSignIn"]);
                if (txtSubHeader.Text.Equals("Control what you listen to."))
                {
                    btnForgotPassword.Content = "Forgot Password";
                }
            }
        }
        private void btnForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            if (btnForgotPassword.Content.Equals("Forgot Password"))
            {
                txtSubHeader.Text = "Send this code where?";
                frmMain.Navigate(pages["frmVerification"]);
                btnForgotPassword.Content = "Sign In";
            }
            else
            {
                btnForgotPassword.Content = "Forgot Password";
                frmMain.Navigate(pages["frmSignIn"]);
                btnBack.Content = "Sign Up";
            }
        }
    }
}
