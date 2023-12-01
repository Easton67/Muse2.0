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
using System.Windows.Input;
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
        private int minutesPassed = 0;
        private MediaPlayer mediaPlayer = new MediaPlayer();

        UserManager _userManager = null;
        UserVM loggedInUser = null;
        SongManager _songManager = null;
        PlaylistManager _playlistManager = null;
        private string userImg = "";
        private int songNumber = 0;
        private int userNumber = 0;
        List<Song> userSongs = null;
        private ContextMenu contextMenu;
        private string baseDirectory = AppContext.BaseDirectory;

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

                if (SongCurrentPosition == SongLengthInSeconds)
                {
                    UpdateSongPlayCount();
                    songListRepopulation();
                    NextSongHelper();
                }

                if (SongCurrentPosition > 0)
                {
                    minutesPassed++;

                    if(minutesPassed % 60 == 0)
                    {
                        try
                        {
                            UserVM updatedUM = _userManager.GetUserVMByEmail(loggedInUser.Email);
                            int newMinutesListened = updatedUM.MinutesListened + 1;
                            _userManager.UpdateMinutesListened(loggedInUser.UserID, newMinutesListened);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Minutes Listened could not be updated.",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
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
        #region UI Helpers
        private void updateUIForLogout()
        {
            txtEmail.Focus();
            btnLogin.IsDefault = true;

            // Turn off song if playing
            mediaPlayer.Pause();

            // Hide role specific  items
            mnuAlbum.Visibility = Visibility.Collapsed;
            mnuFile.Visibility = Visibility.Collapsed;
            mnuSong.Visibility = Visibility.Collapsed;
            mnuAccount.Visibility = Visibility.Collapsed;
            mnuControls.Visibility = Visibility.Collapsed;
            mnuAdmin.Visibility = Visibility.Collapsed;
            mnuPlaylist.Visibility = Visibility.Collapsed;
            grdUsers.Visibility = Visibility.Collapsed;
            panelSelectedUser.Visibility = Visibility.Hidden;

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
            lblDataGridHeader.Visibility = Visibility.Hidden;
            lblDataGridSubHeader.Visibility = Visibility.Hidden;
            btnAllSongs.Visibility = Visibility.Hidden;

            // Reset the Playlists
            grdPlaylists.ItemsSource = null;
            btnPlaylistImageEdit.Visibility = Visibility.Hidden;
            imgPlaylistPicture.Visibility = Visibility.Hidden;
            imgPlaylistPicture.Source = null;
            grdPlaylists.Visibility = Visibility.Hidden;
        }
        private void updateUIForUserLogin()
        {
            try
            {
                var AccountImage = new System.Uri(loggedInUser.ImageFilePath);
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
                    lblDataGridHeader.Visibility = Visibility.Visible;
                    btnAllSongs.Visibility = Visibility.Visible;

                    playlistListRepopulation();
                }
                else
                {
                    // Hide the library
                    grdLibrary.ItemsSource = null;
                    grdLibrary.Visibility = Visibility.Hidden;

                    // Prompt the user to add a song
                    MessageBoxResult result = MessageBox.Show(
                   $"Welcome to Muse! To get started, add one of your songs to your library'?",
                   "Confirmation",
                   MessageBoxButton.YesNo,
                   MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        var AddSong = new AddSong(loggedInUser);
                        AddSong.ShowDialog();
                        songListRepopulation();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not find your library. Please try logging in again.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            GetAccountAndRoles();
        }
        private void songListRepopulation()
        {
            try
            {
                userSongs = _songManager.SelectSongsByUserID(loggedInUser.UserID);
                grdLibrary.ItemsSource = userSongs;

                if (userSongs != null)
                {
                    grdLibrary.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not find your library. Please try logging in again.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void playlistListRepopulation()
        {
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
                editSong.Header = "Edit Song Details";
                editSong.Click += mnuAddSongFromDataGrid_Click;
                contextMenu.Items.Add(editSong);

                MenuItem newPlaylist = new MenuItem();
                newPlaylist.Header = "New Playlist";
                newPlaylist.Click += mnuCreateNewPlaylist_Click;
                contextMenu.Items.Add(newPlaylist);

                if (playlists.Count > 0)
                {
                    MenuItem deleteSong = new MenuItem();
                    deleteSong.Header = "Delete Song";
                    deleteSong.Click += mnuDeleteSong_Click;
                    contextMenu.Items.Add(deleteSong);

                    MenuItem addSong = new MenuItem();
                    addSong.Header = "Add Song To Playlist:";
                    contextMenu.Items.Add(addSong);

                    // Add the list of playlist titles to the context menu
                    foreach (string menuItemText in playlistTitles)
                    {
                        MenuItem menuItem = new MenuItem();
                        menuItem.Header = menuItemText;
                        menuItem.Click += mnuAddSongToPlaylistFromDataGrid_Click;
                        addSong.Items.Add(menuItem);
                    }
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
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not find your playlists. Please try again.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void playlistSongsRepopulation()
        {
            try
            {
                int playlistID = ((Playlist)grdPlaylists.SelectedItem).PlaylistID;
                int userID = loggedInUser.UserID;
                string playlistName = ((Playlist)grdPlaylists.SelectedItem).Title;
                string playlistDescription = ((Playlist)grdPlaylists.SelectedItem).Description;
                userSongs = _songManager.SelectSongsByPlaylistID(userID, playlistID);
                grdLibrary.ItemsSource = userSongs;

                // Change the header and subheader of the playlist I'm currently on

                btnPlaylistImageEdit.Visibility = Visibility.Visible;
                imgPlaylistPicture.Visibility = Visibility.Visible;
                var playlistImageFilePath = ((Playlist)grdPlaylists.SelectedItem).ImageFilePath;
                BitmapImage playlistImageBitmap = new BitmapImage(new System.Uri(playlistImageFilePath));
                imgPlaylistPicture.Source = playlistImageBitmap;
                lblDataGridHeader.Visibility = Visibility.Visible;
                lblDataGridHeader.Content = playlistName;
                lblDataGridSubHeader.Visibility = Visibility.Visible;
                lblDataGridSubHeader.Content = playlistDescription;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Unable to view playlist. Please try again.",
                MessageBoxButton.OK, MessageBoxImage.Error);
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
            mnuAlbum.Visibility = Visibility.Visible;
            mnuPlaylist.Visibility = Visibility.Visible;
            mnuFile.Visibility = Visibility.Visible;
            mnuSong.Visibility = Visibility.Visible;
            mnuAccount.Visibility = Visibility.Visible;
            mnuControls.Visibility = Visibility.Visible;
            mnuViewProfile.Visibility = Visibility.Visible;
            foreach (var role in loggedInUser.Roles)
            {
                if (role.ToString() == "Admin")
                {
                    mnuAdmin.Visibility = Visibility.Visible;
                    break;
                }
            }
        }
        #endregion
        #region Menu Items
        private void mnuNewPlaylist_Click(object sender, RoutedEventArgs e)
        {
            var addPlaylist = new AddPlaylist(loggedInUser);
            addPlaylist.ShowDialog();
            playlistListRepopulation();
        }
        private void mnuPlay_Click(object sender, RoutedEventArgs e)
        {
            btnPlay.Visibility = Visibility.Hidden;
            btnPause.Visibility = Visibility.Visible;
            mediaPlayer.Play();
            timer.Start();
        }
        private void mnuPause_Click(object sender, RoutedEventArgs e)
        {
            btnPlay.Visibility = Visibility.Visible;
            btnPause.Visibility = Visibility.Hidden;
            timer.Stop();
            mediaPlayer.Pause();
        }
        private void mnuNext_Click(object sender, RoutedEventArgs e)
        {
            NextSongHelper();
        }
        private void mnuRewind_Click(object sender, RoutedEventArgs e)
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
        private void mnuViewProfile_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            timer.Stop();
            mediaPlayer.Pause();
            var profileWindow = new Profile(loggedInUser, _songManager);
            profileWindow.ShowDialog();
            loggedInUser = _userManager.GetUserVMByEmail(loggedInUser.Email);
            updateUIForUserLogin();
            grdUsers.Visibility = Visibility.Hidden;
        }
        private void mnuExitApplcation_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void mnuAddSongToLibrary_Click(object sender, RoutedEventArgs e)
        {
            var AddSong = new AddSong(loggedInUser);
            AddSong.ShowDialog();
            songListRepopulation();
        }
        private void mnuResetPassword_Click(object sender, RoutedEventArgs e)
        {
            var resetPassword = new ResetPassword(loggedInUser.Email);
            resetPassword.ShowDialog();
        }
        private void mnuCreateNewAlbum_Click(object sender, RoutedEventArgs e)
        {
            var addAlbum = new AddAlbum(loggedInUser);
            addAlbum.ShowDialog();
            songListRepopulation();
        }
        private void mnuViewAllSongs_Click(object sender, RoutedEventArgs e)
        {
            updateUIForUserLogin();
        }
        private void mnuSignUp_Click(object sender, RoutedEventArgs e)
        {
            var SignUp = new SignUp();
            SignUp.ShowDialog();
        }
        private void mnuViewAllUsers_Click(object sender, RoutedEventArgs e)
        {
            // Hide everything on the page so users can be displayed
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
            grdLibrary.Visibility = Visibility.Hidden;
            grdPlaylists.Visibility = Visibility.Hidden;
            lblDataGridSubHeader.Visibility = Visibility.Hidden;
            btnAllSongs.Visibility = Visibility.Hidden;

            panelSelectedUser.Visibility = Visibility.Visible;

            grdUsersRepopulation();
        }
        #endregion
        private void btnProfileName_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            timer.Stop();
            mediaPlayer.Pause();
            var profileWindow = new Profile(loggedInUser, _songManager);
            profileWindow.ShowDialog();
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
        #region Song Controls
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
        #endregion
        #region Song Control Helpers
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
                    // \MuseConfig\AlbumArt\defaultAlbumImage.png"
                }
                else
                {
                    BitmapImage CoverArt = new BitmapImage(new System.Uri(AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\defaultAlbumImage.png"));
                    imgCoverArt.Source = CoverArt;
                }
            }
            catch (Exception ex)
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
            catch (Exception ex)
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
        #endregion
        #region Library features
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
            if (btnPlaylistImageEdit.Visibility == Visibility.Visible)
            {
                MenuItem removeSongFromPlaylist = new MenuItem();
                removeSongFromPlaylist.Header = "Remove Song From Playlist";
                removeSongFromPlaylist.Click += mnuRemoveSongFromPlaylist_Click;

                bool removeSongFromPlaylistMenuExists = false;
                foreach (var item in contextMenu.Items)
                {
                    if (item is MenuItem menuItem && menuItem.Header?.ToString() == "Remove Song From Playlist")
                    {
                        removeSongFromPlaylistMenuExists = true;
                        break;
                    }
                }

                // If the MenuItem doesn't exist, add it
                if (!removeSongFromPlaylistMenuExists)
                {
                    contextMenu.Items.Add(removeSongFromPlaylist);
                }

                removeSongFromPlaylist.Visibility = Visibility.Visible;
            }

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
                var AddEditSong = new AddEditSongxaml(song, loggedInUser);
                AddEditSong.ShowDialog();
                songListRepopulation();
            }
            else
            {
                MessageBox.Show("Select a Song to view it.");
            }
        }
        private void mnuEditSongFromDataGrid_Click(object sender, RoutedEventArgs e)
        {
            var song = grdLibrary.SelectedItem as Song;

            if (grdLibrary.SelectedItem != null)
            {
                var EditSong = new AddEditSongxaml(song, loggedInUser);
                EditSong.ShowDialog();
                songListRepopulation();
            }
            else
            {
                MessageBox.Show("Select a Song to view it.");
            }
        }
        private void mnuDeleteSong_Click(object sender, RoutedEventArgs e)
        {
            var song = grdLibrary.SelectedItem as Song;

            if (grdLibrary.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Are you sure you want to delete '{song.Title}'?",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _songManager.DeleteSong(song.SongID);
                        MessageBox.Show("Song successfully deleted");

                        // Reload your library or playlist
                        if (btnPlaylistImageEdit.Visibility == Visibility.Hidden)
                        {
                            songListRepopulation();
                            // Simulate skipping to the next song, so it isn't showing up in the media player
                            NextSongHelper();
                        }
                        else
                        {
                            playlistSongsRepopulation();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not delete this song. Please try again",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Select a Song to view it.");
            }
        }
        #endregion
        #region Playlist Manipulation
        private void mnuAddSongToPlaylistFromDataGrid_Click(object sender, RoutedEventArgs e)
        {
            // Get the index of the clicked menu item
            if (sender is MenuItem addToPlaylist)
            {
                if (addToPlaylist.Parent is ItemsControl playlistName)
                {
                    // Get the index from the sub item, not the parent item
                    int index = playlistName.ItemContainerGenerator.IndexFromContainer(addToPlaylist);

                    try
                    {
                        List<Playlist> playlists = _playlistManager.SelectPlaylistByUserID(loggedInUser.UserID);
                        int songID = userSongs[songNumber].SongID;
                        int playlistID = playlists[index].PlaylistID;

                        try
                        {
                            _playlistManager.InsertSongIntoPlaylist(songID, playlistID);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("You have already added this song to your playlist");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Song was not added. Please try again.",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private void grdPlaylists_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (grdPlaylists.SelectedItem != null)
            {
                if (grdPlaylists.SelectedItems.Count != 0)
                {
                    playlistSongsRepopulation();
                }
            }
            else
            {
                MessageBox.Show("Select a playlist to view all your songs you have added to it.");
            }
        }
        private void btnAllSongs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                userSongs = _songManager.SelectSongsByUserID(loggedInUser.UserID);
                lblDataGridHeader.Content = "Library";
                lblDataGridSubHeader.Content = "";
                grdLibrary.ItemsSource = userSongs;
                btnPlaylistImageEdit.Visibility = Visibility.Hidden;
                imgPlaylistPicture.Visibility = Visibility.Hidden;
                imgPlaylistPicture.Source = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Unable to find library. Please try again.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                grdLibrary.ItemsSource = null;
            }
        }
        private void mnuCreateNewPlaylist_Click(object sender, RoutedEventArgs e)
        {
            var addPlaylist = new AddPlaylist(loggedInUser);
            addPlaylist.ShowDialog();
            try
            {
                grdPlaylists.ItemsSource = _playlistManager.SelectPlaylistByUserID(loggedInUser.UserID);
                playlistListRepopulation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not find your playlists.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void mnuRemovePlaylist_Click(object sender, RoutedEventArgs e)
        {
            var playlist = grdPlaylists.SelectedItem as Playlist;

            if (grdPlaylists.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Are you sure you want to delete '{playlist.Title}'?",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _playlistManager.DeletePlaylist(playlist.PlaylistID);
                        MessageBox.Show("Playlist successfully deleted");
                        // Reload your library or playlist
                        if (btnPlaylistImageEdit.Visibility == Visibility.Hidden)
                        {
                            songListRepopulation();
                        }
                        else
                        {
                            playlistSongsRepopulation();
                        }
                        playlistListRepopulation();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not playlist this song. Please try again",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
        }
        private void mnuRemoveSongFromPlaylist_Click(object sender, RoutedEventArgs e)
        {
            var song = grdLibrary.SelectedItem as Song;

            if (grdLibrary.SelectedItem != null)
            {
                try
                {
                    _playlistManager.RemoveSongFromPlaylist(song.SongID);
                    // Reload your library or playlist
                    if (btnPlaylistImageEdit.Visibility == Visibility.Hidden)
                    {
                        songListRepopulation();
                        // Simulate skipping to the next song, so it isn't showing up in the media player
                        NextSongHelper();
                    }
                    else
                    {
                        playlistSongsRepopulation();
                        NextSongHelper();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not remove this song. Please try again",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }
        #endregion
        private void mnuViewAlbum_Click(object sender, RoutedEventArgs e)
        {

        }
        #region Admin Functionality
        private void grdUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdUsers.SelectedItem != null)
            {
                int selectedRowIndex = grdLibrary.Items.IndexOf(grdUsers.SelectedItem);

                userNumber = selectedRowIndex;
            }
        }
        private void grdUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdUsers.SelectedItems.Count != 0)
            {
                var User = grdUsers.SelectedItem as User;

                try
                {
                    ImageBrush imageBrush = (ImageBrush)btnUserProfileImage.Background;
                    BitmapImage profileImage = new BitmapImage(new Uri(User.ImageFilePath, UriKind.Relative));
                    imageBrush.ImageSource = profileImage;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, $"Unable to find the account image for \"{User.ProfileName}\". Please fix this.",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                txtUserID.Text = User.UserID.ToString();
                txtUserFirstName.Text = User.FirstName;
                txtUserLastName.Text = User.LastName;
                txtUserEmail.Text = User.Email;
                txtUserProfileName.Text = User.ProfileName;
            }
        }
        private void btnUserEdit_Click(object sender, RoutedEventArgs e)
        {
            btnUserProfileImage.IsEnabled = true;
            txtUserFirstName.IsEnabled = true;
            txtUserLastName.IsEnabled = true;
            txtUserProfileName.IsEnabled = true;
        }
        private void grdUsersRepopulation()
        {
            try
            {
                List<User> allUsers = _userManager.SelectAllUsers();
                lblDataGridHeader.Content = "All Users";
                grdUsers.Visibility = Visibility.Visible;
                grdUsers.ItemsSource = allUsers;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not found users.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void btnUserSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            string NewFirstName = txtUserFirstName.Text;
            string NewLastName = txtUserLastName.Text;

            if (!NewFirstName.IsValidFirstName())
            {
                MessageBox.Show("That is not a valid first name", "Invalid first name",
                MessageBoxButton.OK, MessageBoxImage.Error);
                txtUserFirstName.Focus();
                return;
            }
            if (!NewLastName.IsValidLastName())
            {
                MessageBox.Show("That is not a valid last name", "Invalid last name",
                MessageBoxButton.OK, MessageBoxImage.Error);
                txtUserFirstName.Focus();
                return;
            }

            UserManager _userManager = new UserManager();
            string userEmail = txtUserEmail.Text;
            UserVM oldUser = _userManager.GetUserVMByEmail(userEmail);

            var newUser = new UserVM()
            {
                UserID = int.Parse(txtUserID.Text),
                ProfileName = loggedInUser.ProfileName,
                Email = loggedInUser.Email,
                FirstName = NewFirstName,
                LastName = NewLastName,
                ImageFilePath = userImg,
                Active = (bool)chkUserActive.IsChecked,
                MinutesListened = 0,
                Roles = loggedInUser.Roles
            };

            try
            {
                _userManager.UpdateUser(oldUser, newUser);
                MessageBox.Show("Your account details have been updated", "Success!",
                MessageBoxButton.OK);
                grdUsersRepopulation();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to update your profile." + " " + ex.Message);
            }
        }
        private void btnUserProfileImage_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UserManager _userManager = new UserManager();

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.Title = "Open File";
                openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;)|*.jpg;*.jpeg;*.png;|All Files (*.*)|*.*";

                bool? result = openFileDialog.ShowDialog();

                if (result == true)
                {
                    // Create the new imagebrush
                    ImageBrush imageBrush = (ImageBrush)btnUserProfileImage.Background;
                    userImg = openFileDialog.FileName;
                    var AccountImage = new BitmapImage(new System.Uri(userImg));
                    imageBrush.ImageSource = AccountImage;
                }
                else
                {
                    // user closes the file explorer before picking a photo
                    MessageBox.Show("Choose a photo to update your current account photo.");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Invalid image." + " " + ex.Message);
            }
        }
        #endregion
        private void btnPlaylistImageEdit_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
