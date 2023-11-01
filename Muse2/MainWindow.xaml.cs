using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using DataObjects;
using LogicLayer;

namespace Muse2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UserManager _userManager = null;
        UserVM loggedInUser = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            // set variables
            string password = pwdPassword.Password;
            string email = txtEmail.Text;

            // Load images



            _userManager = new UserManager();


            // First time log in
            txtEmail.Focus();
            btnLogin.IsDefault = true;

            // Hide role specific buttons
            mnuAdmin.Visibility = Visibility.Hidden;
            mnuArtist.Visibility = Visibility.Hidden;

            // Hide all song controls
            lblSongTitle.Content = "";
            lblSongArtist.Content = "";
            imgCoverArt.Visibility = Visibility.Collapsed;
            imgPause.Visibility = Visibility.Collapsed;
            imgPlay.Visibility = Visibility.Collapsed;
            imgRewind.Visibility = Visibility.Collapsed;
            imgNext.Visibility = Visibility.Collapsed;
            barSongLength.Visibility = Visibility.Collapsed;

            // Default the login and hide it
            txtEmail.Text = "";
            txtEmail.Visibility = Visibility.Visible;
            lblEmail.Visibility = Visibility.Visible;
            lblProfileName.Content = "";
            lblProfileName.Content = Visibility.Hidden;
            pwdPassword.Password = "";
            pwdPassword.Visibility = Visibility.Visible;
            lblPassword.Visibility = Visibility.Visible;
            btnLogin.Content = "Log In";
            btnLogin.IsDefault = false;

            // Hide Account
            lblProfileName.Content = "";

            // Hide the grid
            grdLibrary.Visibility = Visibility.Collapsed;

            List<MyDataObject> list = new List<MyDataObject>();
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            grdLibrary.ItemsSource = list;
        }
        private void updateUIForLogout()
        {
            txtEmail.Focus();
            btnLogin.IsDefault = true;

            // Hide role specific buttons
            mnuAdmin.Visibility = Visibility.Hidden;
            mnuArtist.Visibility = Visibility.Hidden;

            // Hide all song controls
            lblSongTitle.Content = "";
            lblSongArtist.Content = "";
            imgCoverArt.Visibility = Visibility.Collapsed;
            imgPause.Visibility = Visibility.Collapsed;
            imgPlay.Visibility = Visibility.Collapsed;
            imgRewind.Visibility = Visibility.Collapsed;
            imgNext.Visibility = Visibility.Collapsed;
            barSongLength.Visibility = Visibility.Collapsed;

            // Default the login and hide it
            txtEmail.Text = "";
            txtEmail.Visibility = Visibility.Visible;
            lblEmail.Visibility = Visibility.Visible;
            lblProfileName.Content = "";
            lblProfileName.Content = Visibility.Hidden;
            pwdPassword.Password = "";
            pwdPassword.Visibility = Visibility.Visible;
            lblPassword.Visibility = Visibility.Visible;
            btnLogin.Content = "Log In";
            btnLogin.IsDefault = false;

            var AccountImage = new System.Uri("C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAccount.png");
            BitmapImage bitmapImage = new BitmapImage(AccountImage);
            imgAccount.Source = bitmapImage;

            // Hide Account
            lblProfileName.Content = "";

            // Hide the grid
            grdLibrary.Visibility = Visibility.Collapsed;
        }

        private void updateUIForUserLogin()
        {
            // set song controls
            lblSongTitle.Content = "";
            lblSongArtist.Content = "";
            imgCoverArt.Visibility = Visibility.Visible;
            imgPause.Visibility = Visibility.Visible;
            imgPlay.Visibility = Visibility.Visible;
            imgRewind.Visibility = Visibility.Visible;
            imgNext.Visibility = Visibility.Visible;
            barSongLength.Visibility = Visibility.Visible;

            // set account
            txtEmail.Text = "";
            txtEmail.Visibility = Visibility.Hidden;
            lblEmail.Visibility = Visibility.Collapsed;
            lblProfileName.Content = loggedInUser.ProfileName;
            pwdPassword.Password = "";
            pwdPassword.Visibility = Visibility.Hidden;
            lblPassword.Visibility = Visibility.Hidden;
            btnLogin.Content = "Log Out";
            btnLogin.IsDefault = false;

            var AccountImage = new System.Uri(loggedInUser.ImageFilePath);
            BitmapImage bitmapImage = new BitmapImage(AccountImage);
            imgAccount.Source = bitmapImage;




            foreach (var role in loggedInUser.Roles)
            {
                if (role.ToString() == "Admin")
                {
                    mnuAdmin.Visibility = Visibility.Visible;
                    break;
                }
                if (role.ToString() == "Artist")
                {
                    mnuArtist.Visibility = Visibility.Visible;
                    break;
                }
            }

            // set library
            grdLibrary.Visibility = Visibility.Visible;

        }

        private void mnuExitApplcation_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public class MyDataObject
        {
            public Uri ImageFilePath { get; set; }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (btnLogin.Content.ToString() == "Log In")
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

                // try to log in the user

                try
                {
                    loggedInUser = _userManager.LoginUser(email, password);
                    updateUIForUserLogin();
                }
                catch (Exception ex)
                {
                    // you may never throw exceptions from the presentation layer
                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Login Failed",
                         MessageBoxButton.OK, MessageBoxImage.Error);
                    pwdPassword.SelectAll();
                    txtEmail.Clear();
                    txtEmail.Focus();
                    return;
                }
            }
            else // logout      
            {
                updateUIForLogout();
            }
        }
    }
}
