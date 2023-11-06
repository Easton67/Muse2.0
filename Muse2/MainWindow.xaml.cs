using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using DataObjects;
using LogicLayer;
using Microsoft.Win32;

namespace Muse2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UserManager _userManager = null;
        UserVM loggedInUser = null;
        SongManager _songManager = null;
        public int songNumber = 0;

        private MediaPlayer mediaPlayer = new MediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

    private void timer_Tick(object sender, EventArgs e)
    {
        if (mediaPlayer.Source != null)
        {
            lblCurrentTime.Content = mediaPlayer.Position.ToString(@"mm\:ss");

            // Check for if Natural Duration has a valid time span to avoid 
            // the "Duration value of Automatic" error
            if (mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                lblSongLength.Content = mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
            }
            else
            {
                lblSongLength.Content = "00:00";
            }
        } 
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _userManager = new UserManager();
            _songManager = new SongManager();

            updateUIForLogout();
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
            btnRewind.Visibility = Visibility.Collapsed;
            btnPause.Visibility = Visibility.Collapsed;
            btnPlay.Visibility = Visibility.Collapsed;
            btnNext.Visibility = Visibility.Collapsed;
            barSongLength.Visibility = Visibility.Collapsed;

            // Default the login and hide it
            txtEmail.Text = "Drake@gmail.com";
            txtEmail.Visibility = Visibility.Visible;
            lblEmail.Visibility = Visibility.Visible;
            lblProfileName.Content = "";
            lblProfileName.Content = Visibility.Hidden;
            pwdPassword.Password = "password";
            pwdPassword.Visibility = Visibility.Visible;
            lblPassword.Visibility = Visibility.Visible;
            btnLogin.Content = "Log In";
            btnLogin.IsDefault = false;

            // Hide Account
            lblProfileName.Content = "";

            // Hide the grid
            grdLibrary.Visibility = Visibility.Collapsed;
        }
        private void updateUIForUserLogin()
        {
            // set the variables
            var ProfileName = loggedInUser.ProfileName;
            var AccountImage = new System.Uri(loggedInUser.ImageFilePath);

            List<Song> userSongs = _songManager.SelectSongsByProfileName(ProfileName);

            // set song controls
            lblSongTitle.Content = "";
            lblSongArtist.Content = "";
            imgCoverArt.Visibility = Visibility.Visible;
            btnRewind.Visibility = Visibility.Visible;
            btnPlay.Visibility = Visibility.Visible;
            btnNext.Visibility = Visibility.Visible;
            barSongLength.Visibility = Visibility.Visible;

            // load the first song in the list
            lblSongTitle.Content = userSongs[songNumber].Title;
            lblSongArtist.Content = userSongs[songNumber].Artist;
            BitmapImage CoverArt = new BitmapImage(new System.Uri(userSongs[songNumber].ImageFilePath));
            imgCoverArt.Source = CoverArt;
            mediaPlayer.Open(new Uri((userSongs[songNumber].Mp3FilePath)));

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

            // account image
            BitmapImage Account = new BitmapImage(AccountImage);
            imgAccount.Source = Account;
            
            // set menus for specific roles
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
            grdLibrary.ItemsSource = userSongs;
        }
        private void mnuExitApplcation_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

        // Song Controls
        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            btnPlay.Visibility = Visibility.Visible;
            btnPause.Visibility = Visibility.Hidden;

            mediaPlayer.Pause();
        }
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        { 
            btnPlay.Visibility = Visibility.Hidden;
            btnPause.Visibility = Visibility.Visible;

            mediaPlayer.Play();
        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            var ProfileName = loggedInUser.ProfileName;
            List<Song> userSongs = _songManager.SelectSongsByProfileName(ProfileName);

            if (songNumber < userSongs.Count - 1)
            {
                songNumber++;
                lblSongTitle.Content = userSongs[songNumber].Title;
                lblSongArtist.Content = userSongs[songNumber].Artist;
                BitmapImage CoverArt = new BitmapImage(new System.Uri(userSongs[songNumber].ImageFilePath));
                imgCoverArt.Source = CoverArt;
                mediaPlayer.Open(new Uri((userSongs[songNumber].Mp3FilePath)));
                
                // AutoPlay the next song if the pause button is visible
                if (btnPause.IsVisible)
                {
                    mediaPlayer.Play();
                }
            }
            else
            {
                songNumber = 0;
                lblSongTitle.Content = userSongs[songNumber].Title;
                lblSongArtist.Content = userSongs[songNumber].Artist;
                BitmapImage CoverArt = new BitmapImage(new System.Uri(userSongs[songNumber].ImageFilePath));
                imgCoverArt.Source = CoverArt;
                mediaPlayer.Open(new Uri((userSongs[songNumber].Mp3FilePath)));
            }
        }
    }
}
