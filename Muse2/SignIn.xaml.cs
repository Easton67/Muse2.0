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
        Dictionary<string, Page> pages = new Dictionary<string, Page>();


        public SignIn()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CloseWindow.win = this;
            pages.Add("frmSignIn", new pgSignIn());
            pages.Add("frmSignUp", new pgSignUp());
            pages.Add("frmVerification", new pgVerification());

            frmMain.Navigate(pages["frmSignIn"]);
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
                if (txtSubHeader.Text.Equals("Control what you listen to."))
                {
                    frmMain.Navigate(pages["frmSignIn"]);
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
            }
        }
    }
}
