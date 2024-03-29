﻿using DataObjects;
using LogicLayer;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Muse2.Pages.Login
{
    /// <summary>
    /// Interaction logic for pgSignIn.xaml
    /// </summary>
    public partial class pgSignIn : Page
    {
        UserVM loggedInUser = null;
        bool btnShowPasswordTopIsClicked;

        public pgSignIn()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // easier to hide than to figure out the exact coordinates
            txtShownPasswordTop.Text = "";
            txtEmail.Text = "67Easton@gmail.com";
            pwdPassword.Password = "password";
            txtShownPasswordTop.Visibility = Visibility.Hidden;
        }
        private void btnShowPasswordTop_Click(object sender, RoutedEventArgs e)
        {
            txtShownPasswordTop.Text = pwdPassword.Password;
            btnShowPasswordTopIsClicked = !btnShowPasswordTopIsClicked;
            txtShownPasswordTop.Visibility = (btnShowPasswordTopIsClicked) ? Visibility.Visible
                                                                           : Visibility.Hidden;
        }
        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            var email = txtEmail.Text;
            var password = pwdPassword.Password;
            btnShowPasswordTop.Visibility = Visibility.Visible;

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

                if (Application.Current.MainWindow != null)
                {
                    Application.Current.MainWindow.Hide();
                }

                var home = new MainWindow(loggedInUser);
                home.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Login Failed",
                MessageBoxButton.OK, MessageBoxImage.Error);
                pwdPassword.SelectAll();
                pwdPassword.Clear();
                txtEmail.Focus();
                return;
            }
        }
    }
}
