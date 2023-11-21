using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Resources;
using System.Security.Principal;
using System.Threading;
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
        private DispatcherTimer timer;
        UserManager _userManager = null;
        UserVM loggedInUser = null;
        SongManager _songManager = null;
        PlaylistManager _playlistManager = null;
        public int songNumber = 0;
        private MediaPlayer mediaPlayer = new MediaPlayer();
        List<Song> userSongs = null;
        private ContextMenu contextMenu;

        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += SongTimer;
        }
        private void SongTimer(object sender, EventArgs e)
        {
            if (mediaPlayer.Source != null)
            {
                lblCurrentTime.Content = mediaPlayer.Position.ToString(@"mm\:ss");
            }
            // Check for if Natural Duration has a valid time span to avoid 
            // the "Duration value of Automatic" error

            if (mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                lblSongLength.Content = mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");

                double SongLengthInSeconds = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                double SongCurrentPosition = mediaPlayer.Position.TotalSeconds;

                barSongLength.Maximum = SongLengthInSeconds;
                barSongLength.Minimum = 00.00;
                barSongLength.Value = SongCurrentPosition;

                if(SongCurrentPosition == SongLengthInSeconds)
                {
                    UpdateSongPlayCount();
                    songListRepopulation();
                    NextSongHelper();
                }
            }
            else
            {
                lblSongLength.Content = "00:00";
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _userManager = new UserManager();
            _songManager = new SongManager();
            _playlistManager = new PlaylistManager();
            updateUIForLogout();
        }
        // UI Helpers
        private void updateUIForLogout()
        {
            txtEmail.Focus();
            btnLogin.IsDefault = true;

            // Turn off song if playing
            mediaPlayer.Pause();

            // Hide role specific buttons and menu items
            mnuFile.Visibility = Visibility.Collapsed;
            mnuSong.Visibility = Visibility.Collapsed;
            mnuAccount.Visibility = Visibility.Collapsed;
            mnuFriends.Visibility = Visibility.Collapsed;
            mnuControls.Visibility = Visibility.Collapsed;
            mnuAdmin.Visibility = Visibility.Collapsed;
            mnuArtist.Visibility = Visibility.Collapsed;

            // Hide all song controls
            btnViewSong.Visibility = Visibility.Hidden;
            lblSongTitle.Content = "";
            lblSongArtist.Content = "";
            imgExplicit.Visibility = Visibility.Hidden;
            lblCurrentTime.Visibility = Visibility.Hidden;
            lblSongLength.Visibility = Visibility.Hidden;
            imgCoverArt.Visibility = Visibility.Collapsed;
            imgCoverArt.Source = null;
            btnRewind.Visibility = Visibility.Collapsed;
            btnPause.Visibility = Visibility.Collapsed;
            btnPlay.Visibility = Visibility.Collapsed;
            btnNext.Visibility = Visibility.Collapsed;
            barSongLength.Visibility = Visibility.Collapsed;

            // Default the login and hide it
            txtEmail.Text = "Liam@gmail.com";
            txtEmail.Visibility = Visibility.Visible;
            lblEmail.Visibility = Visibility.Visible;
            btnProfileName.Content = "";
            btnProfileName.Visibility = Visibility.Hidden;
            pwdPassword.Password = "password";
            pwdPassword.Visibility = Visibility.Visible;
            lblPassword.Visibility = Visibility.Visible;
            btnLogin.Content = "Log In";
            btnLogin.IsDefault = false;

            imgAccount.Visibility = Visibility.Hidden;
            defaultimgAccount.Visibility = Visibility.Visible;

            // Hide Account
            btnProfileName.Content = "";

            // Reset the Library
            grdLibrary.Visibility = Visibility.Collapsed;
            grdLibrary.ItemsSource = null;

            // Reset the Playlists
            grdPlaylists.ItemsSource = null;
            grdPlaylists.Visibility = Visibility.Collapsed;
        }
        private void songListRepopulation()
        {
            try
            {
                userSongs = _songManager.SelectSongsByUserID(loggedInUser.UserID);

                grdLibrary.ItemsSource = userSongs;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not find your library. Please try logging in again.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void GetAccountAndRoles()
        {
            // set account
            txtEmail.Text = "";
            txtEmail.Visibility = Visibility.Hidden;
            lblEmail.Visibility = Visibility.Collapsed;
            btnProfileName.Content = loggedInUser.ProfileName;
            btnProfileName.Visibility = Visibility.Visible;
            pwdPassword.Password = "";
            pwdPassword.Visibility = Visibility.Hidden;
            lblPassword.Visibility = Visibility.Hidden;
            btnLogin.Content = "Log Out";
            btnLogin.IsDefault = false;

            // set menus for specific roles
            mnuFile.Visibility = Visibility.Visible;
            mnuSong.Visibility = Visibility.Visible;
            mnuAccount.Visibility = Visibility.Visible;
            mnuFriends.Visibility = Visibility.Visible;
            mnuControls.Visibility = Visibility.Visible;
            mnuViewProfile.Visibility = Visibility.Visible;
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
        }
        private void updateUIForUserLogin()
        {
            try
            {
                var AccountImage = new System.Uri(loggedInUser.ImageFilePath);
                // account image
                defaultimgAccount.Visibility = Visibility.Hidden;
                BitmapImage Account = new BitmapImage(AccountImage);
                imgAccount.Visibility = Visibility.Visible;
                imgAccount.Source = Account;
            }
            catch (Exception)
            {
                imgAccount.Source = null;
                imgAccount.Visibility = Visibility.Hidden;
                defaultimgAccount.Visibility = Visibility.Visible;
                GetAccountAndRoles();
                return;
            }
            try
            {
                int userSongsCount = _songManager.SelectSongsByUserID(loggedInUser.UserID).Count();

                if (userSongsCount > 0)
                {
                    userSongs = _songManager.SelectSongsByUserID(loggedInUser.UserID);

                    // load the first song in the list
                    btnViewSong.Visibility = Visibility.Visible;
                    BitmapImage CoverArt = new BitmapImage(new System.Uri(userSongs[songNumber].ImageFilePath));
                    imgCoverArt.Source = CoverArt;
                    mediaPlayer.Open(new Uri((userSongs[songNumber].Mp3FilePath)));
                    if (userSongs[0].Explicit == true)
                    {
                        imgExplicit.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        imgExplicit.Visibility = Visibility.Hidden;
                    }
                    grdLibrary.ItemsSource = userSongs;
                    lblSongTitle.Content = userSongs[songNumber].Title;
                    lblSongArtist.Content = userSongs[songNumber].Artist;

                    // set song controls
                    lblCurrentTime.Visibility = Visibility.Visible;
                    lblSongLength.Visibility = Visibility.Visible;
                    imgCoverArt.Visibility = Visibility.Visible;
                    btnRewind.Visibility = Visibility.Visible;
                    btnPlay.Visibility = Visibility.Visible;
                    btnNext.Visibility = Visibility.Visible;
                    barSongLength.Visibility = Visibility.Visible;

                    // set library
                    grdLibrary.Visibility = Visibility.Visible;

                    // set the playlists
                    try
                    {
                        List<Playlist> playlists = _playlistManager.SelectPlaylistByUserID(loggedInUser.UserID);

                        contextMenu = new ContextMenu();

                        List<string> playlistTitles = new List<string>();

                        // Add playlist title to a new list of just the title
                        foreach (Playlist playlist in playlists)
                        {
                            string playlistTitle = playlist.Title;
                            playlistTitles.Add(playlistTitle);
                        }

                        MenuItem editSong = new MenuItem();
                        MenuItem addSong = new MenuItem();
                        editSong.Header = "Edit Song Details";
                        addSong.Header = "Add Song To Playlist:";
                        editSong.Click += mnuAddSongFromDataGrid_Click;
                        contextMenu.Items.Add(editSong); 
                        contextMenu.Items.Add(addSong);

                        // Add the list of playlist titles to the context menu
                        foreach (string menuItemText in playlistTitles)
                        {
                            MenuItem menuItem = new MenuItem();
                            menuItem.Header = menuItemText;
                            menuItem.Click += mnuAddSongToPlaylistFromDataGrid_Click;
                            contextMenu.Items.Add(menuItem);
                        }

                        grdLibrary.ContextMenu = contextMenu;

                        if (playlists.Count > 0)
                        {
                            grdPlaylists.Visibility = Visibility.Visible;
                            grdPlaylists.ItemsSource = playlists;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Login Failed",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    // Hide the library
                    grdLibrary.ItemsSource = null;
                    grdLibrary.Visibility = Visibility.Hidden;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not find your library. Please try logging in again.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            GetAccountAndRoles();
        }
        // Menu Items
        private void mnuViewProfile_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            timer.Stop();
            mediaPlayer.Pause();
            var profileWindow = new Profile(loggedInUser, _songManager);
            profileWindow.ShowDialog();
            loggedInUser = _userManager.GetUserVMByEmail(loggedInUser.Email);
            updateUIForUserLogin();
        }
        private void mnuExitApplcation_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void mnuResetPassword_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnProfileName_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            timer.Stop();
            mediaPlayer.Pause();
            var profileWindow = new Profile(loggedInUser, _songManager);
            profileWindow.ShowDialog();
            loggedInUser = _userManager.GetUserVMByEmail(loggedInUser.Email);
            updateUIForUserLogin();
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
                    pwdPassword.Clear();
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
        private void btnViewSong_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var song = userSongs[songNumber] as Song;

            var ViewSong = new ViewSong(song);
            ViewSong.ShowDialog();
        }
        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            btnPlay.Visibility = Visibility.Visible;
            btnPause.Visibility = Visibility.Hidden;
            timer.Stop();
            mediaPlayer.Pause();
        }
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        { 
            btnPlay.Visibility = Visibility.Hidden;
            btnPause.Visibility = Visibility.Visible;
            mediaPlayer.Play();
            timer.Start();
        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            NextSongHelper();
        }
        private void btnRewind_Click(object sender, RoutedEventArgs e)
        {
            // if the song is not at the start, rewind it
            if (mediaPlayer.Position.ToString(@"mm\:ss") != "00:00")
            {
                mediaPlayer.Stop();
                mediaPlayer.Play();
                return;
            }
            if (btnPause.IsVisible)
            {
                if (songNumber <= 0)
                {
                    songNumber = userSongs.Count - 1;
                    CurrentSongHelper();
                    mediaPlayer.Play();
                }
                else
                {
                    songNumber--;
                    CurrentSongHelper();
                    mediaPlayer.Play();
                }
            }
            else
            {
                if (songNumber <= 0)
                {
                    songNumber = userSongs.Count - 1;
                    CurrentSongHelper();
                }
                else
                {
                    songNumber--;
                    CurrentSongHelper();
                }
            }
        }
        // Song control helpers
        private void UpdateSongPlayCount()
        {
            var SongID = userSongs[songNumber].SongID;
            int NewPlays = userSongs[songNumber].Plays + 1;
            try
            {
                _songManager.UpdatePlaysBySongID(SongID, NewPlays);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Plays not updated",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void GetSongCover()
        {
            try
            {
                if (userSongs[songNumber].ImageFilePath != "")
                {
                    BitmapImage CoverArt = new BitmapImage(new System.Uri(userSongs[songNumber].ImageFilePath));
                    imgCoverArt.Source = CoverArt;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Song cover could not be found.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void CurrentSongHelper()
        {
            lblSongTitle.Content = userSongs[songNumber].Title;
            lblSongArtist.Content = userSongs[songNumber].Artist;
            if (userSongs[songNumber].Explicit == true)
            {
                imgExplicit.Visibility = Visibility.Visible;
            }
            else
            {
                imgExplicit.Visibility = Visibility.Hidden;
            }
            GetSongCover();
            try
            {
                mediaPlayer.Open(new Uri((userSongs[songNumber].Mp3FilePath)));
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Song file could not be found. " +
                "Please make sure your file is in the correct location.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void NextSongHelper()
        {
            if (songNumber < userSongs.Count - 1)
            {
                songNumber++;
                CurrentSongHelper();
                if (btnPause.IsVisible)
                {
                    mediaPlayer.Play();
                }
            }
            else
            {
                songNumber = 0;
                CurrentSongHelper();
                if (btnPause.IsVisible)
                {
                    mediaPlayer.Play();
                }
            }
        }
        private void grdLibrary_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (grdLibrary.SelectedItems.Count != 0)
            {
                var Song = grdLibrary.SelectedItem as Song;
                lblSongTitle.Content = Song.Title;
                lblSongArtist.Content = Song.Artist;
                if (Song.Explicit == true)
                {
                    imgExplicit.Visibility = Visibility.Visible;
                }
                else
                {
                    imgExplicit.Visibility = Visibility.Hidden;
                }
                GetSongCover();
                CurrentSongHelper();
                btnPlay.Visibility = Visibility.Hidden;
                btnPause.Visibility = Visibility.Visible;
                if (grdLibrary.SelectedItem != null)
                {
                    int selectedRowIndex = grdLibrary.Items.IndexOf(grdLibrary.SelectedItem);
                    songNumber = selectedRowIndex;
                }
                mediaPlayer.Play();
                timer.Start();
            }
            else
            {
                MessageBox.Show("Select a Song to listen to it.");
            }
        }
        private void grdLibrary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdLibrary.SelectedItem != null)
            {
                int selectedRowIndex = grdLibrary.Items.IndexOf(grdLibrary.SelectedItem);
                songNumber = selectedRowIndex;
            }
        }
        
        
        
        
        private void mnuAddSongFromDataGrid_Click(object sender, RoutedEventArgs e)
        {
            var song = grdLibrary.SelectedItem as Song;

            if (grdLibrary.SelectedItem != null)
            {
                var AddEditSong = new AddEditSongxaml(song);
                AddEditSong.ShowDialog();
                songListRepopulation();            
            }
            else
            {
                MessageBox.Show("Select a Song to view it.");
            }
        }



        private void mnuAddSongToLibrary_Click(object sender, RoutedEventArgs e)
        {
            var AddSong = new AddSong(loggedInUser);
            AddSong.ShowDialog();
            grdLibrary.ItemsSource = _songManager.SelectSongsByUserID(loggedInUser.UserID);
        }




        private void mnuHelp_Click(object sender, RoutedEventArgs e)
        {
            var SignUp = new SignUp();
            SignUp.ShowDialog();
        }
        private void mnuEditSongFromDataGrid_Click(object sender, RoutedEventArgs e)
        {
            var song = grdLibrary.SelectedItem as Song;

            if (grdLibrary.SelectedItem != null)
            {
                var AddEditSong = new AddEditSongxaml(song);
                AddEditSong.ShowDialog();
                songListRepopulation();
            }
            else
            {
                MessageBox.Show("Select a Song to view it.");
            }
        }

        private void mnuAddSongToPlaylistFromDataGrid_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
