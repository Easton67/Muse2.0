using DataObjects;
using LogicLayer;
using Microsoft.VisualBasic.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

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
        private UserVM _loggedInUser = null;
        private UserManager _userManager = null;
        private UserVM loggedInUser = null;
        private SongManager _songManager = null;
        private PlaylistManager _playlistManager = null;
        private string userImg = "";
        private int songNumber = 0;
        private Random shuffledSongNumber = new Random();
        private int userNumber = 0;
        private string playlistImg = "";
        private List<Song> userSongs = null;
        private ContextMenu contextMenu;
        private Playlist selectedPlaylist;
        private string baseDirectory = AppContext.BaseDirectory;
        private bool isEnabledShuffle;

        public MainWindow(UserVM LoggedInUser)
        {
            InitializeComponent();

            loggedInUser = LoggedInUser;
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

                if (SongCurrentPosition > 0)
                {
                    minutesPassed++;

                    if (minutesPassed % 60 == 0)
                    {
                        try
                        {
                            UserVM updatedUM = _userManager.GetUserVMByEmail(loggedInUser.Email);
                            int newMinutesListened = updatedUM.MinutesListened + 1;
                            _userManager.UpdateMinutesListened(loggedInUser.UserID, newMinutesListened);
                            loggedInUser = updatedUM;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Minutes Listened could not be updated.",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }

                if (SongCurrentPosition == SongLengthInSeconds)
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

            // Hide what should not be seen before showing what should be.
            updateUIForLogout();
            updateUIForUserLogin();
        }
        #region UI Helpers
        private void updateUIForLogout()
        {
            // Turn off song if playing
            mediaPlayer.Pause();

            // Hide role specific  items
            //mnuAlbum.Visibility = Visibility.Collapsed;
            mnuFile.Visibility = Visibility.Collapsed;
            mnuSong.Visibility = Visibility.Collapsed;
            mnuAccount.Visibility = Visibility.Collapsed;
            mnuControls.Visibility = Visibility.Collapsed;
            mnuAdmin.Visibility = Visibility.Collapsed;
            mnuPlaylist.Visibility = Visibility.Collapsed;
            mnuResetPassword.Visibility = Visibility.Collapsed;
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
            btnProfileName.Content = "";
            btnProfileName.Visibility = Visibility.Hidden;

            imgAccount.Visibility = Visibility.Hidden;
            defaultimgAccount.Visibility = Visibility.Visible;

            // Hide Account
            btnProfileName.Content = "";

            // Reset the Library
            grdLibrary.Visibility = Visibility.Collapsed;
            grdLibrary.ItemsSource = null;
            lblDataGridHeader.Visibility = Visibility.Hidden;
            txtDataGridHeaderEdit.Visibility = Visibility.Hidden;
            lblDataGridSubHeader.Visibility = Visibility.Hidden;
            txtDataGridSubHeaderEdit.Visibility = Visibility.Hidden;
            lblDataGridHeader.Content = "Library";
            lblDataGridSubHeader.Content = "";
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not find your profile photo. Please try logging in again.",
                MessageBoxButton.OK, MessageBoxImage.Error);
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
                   $"Would you like to add a song to get started'?",
                   "Welcome to Muse!",
                   MessageBoxButton.YesNo,
                   MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        var AddSong = new AddSong(loggedInUser);
                        AddSong.ShowDialog();
                        updateUIForUserLogin();
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

                MenuItem writeReview = new MenuItem();
                writeReview.Header = "Write a review";
                writeReview.Click += mnuCreateReview_Click;
                contextMenu.Items.Add(writeReview);

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
            btnProfileName.Content = loggedInUser.ProfileName;
            btnProfileName.Visibility = Visibility.Visible;

            // set menus for specific roles
            //mnuAlbum.Visibility = Visibility.Visible;
            mnuPlaylist.Visibility = Visibility.Visible;
            mnuFile.Visibility = Visibility.Visible;
            mnuSong.Visibility = Visibility.Visible;
            mnuAccount.Visibility = Visibility.Visible;
            mnuControls.Visibility = Visibility.Visible;
            mnuViewProfile.Visibility = Visibility.Visible;
            mnuResetPassword.Visibility = Visibility.Visible;
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
        private void mnuUrlToMp3_Click(object sender, RoutedEventArgs e)
        {
            var UrlToMp3 = new UrlToMp3(loggedInUser);
            UrlToMp3.ShowDialog();
            songListRepopulation();
        }
        #endregion
        private void btnProfileName_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            timer.Stop();
            mediaPlayer.Pause();
            var profileWindow = new Profile(loggedInUser, _songManager);
            profileWindow.ShowDialog();
            if (grdLibrary.Visibility == Visibility.Visible)
            {
                UserVM updatedUser = _userManager.GetUserVMByEmail(loggedInUser.Email);
                try
                {
                    var AccountImage = new BitmapImage(new System.Uri(updatedUser.ImageFilePath));
                    defaultimgAccount.Visibility = Visibility.Hidden;
                    imgAccount.Visibility = Visibility.Visible;
                    imgAccount.Source = AccountImage;
                }
                catch (Exception)
                {
                    imgAccount.Source = null;
                    imgAccount.Visibility = Visibility.Hidden;
                    defaultimgAccount.Visibility = Visibility.Visible;
                    GetAccountAndRoles();
                    return;
                }
            }
            else if (grdUsers.Visibility == Visibility.Visible)
            {
                grdUsersRepopulation();
            }
            else
            {
                playlistListRepopulation();
            }
        }
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var SignIn = new SignIn();
            this.Close();
            SignIn.ShowDialog();
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
            Pause();
        }
        private void Pause()
        {
            btnPlay.Visibility = Visibility.Visible;
            btnPause.Visibility = Visibility.Hidden;
            timer.Stop();
            mediaPlayer.Pause();
        }
        private void Play()
        {
            btnPlay.Visibility = Visibility.Hidden;
            btnPause.Visibility = Visibility.Visible;
            mediaPlayer.Play();
            timer.Start();
        }
        private void Rewind()
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
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            Play();
        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            NextSongHelper();
        }
        private void btnRewind_Click(object sender, RoutedEventArgs e)
        {
            Rewind();
        }
        private void btnShuffle_Click(object sender, RoutedEventArgs e)
        {
            if (isEnabledShuffle == false)
            {
                isEnabledShuffle = true;
                NextSongHelper();
            }
            else
            {
                isEnabledShuffle = false;
                btnShuffle.BorderThickness = new Thickness(0);
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
                if (userSongs[songNumber].ImageFilePath == AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\")
                {
                    BitmapImage CoverArt = new BitmapImage(new System.Uri(AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\defaultAlbumImage.png"));
                    imgCoverArt.Source = CoverArt;
                    return;
                }
                if (userSongs[songNumber].ImageFilePath != null)
                {
                    BitmapImage CoverArt = new BitmapImage(new System.Uri(userSongs[songNumber].ImageFilePath));
                    imgCoverArt.Source = CoverArt;
                }
                else
                {
                    BitmapImage CoverArt = new BitmapImage(new System.Uri(AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\defaultAlbumImage.png"));
                    imgCoverArt.Source = CoverArt;
                }
            }
            catch (Exception ex)
            {
                BitmapImage CoverArt = new BitmapImage(new System.Uri(AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\defaultAlbumImage.png"));
                imgCoverArt.Source = CoverArt;
                NextSongHelper();
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
                if (imgPlaylistPicture.Visibility == Visibility.Visible)
                {
                    mediaPlayer.Open(new Uri((baseDirectory + "\\MuseConfig\\SongFiles\\" + userSongs[songNumber].Mp3FilePath)));
                }
                else
                {
                    mediaPlayer.Open(new Uri((userSongs[songNumber].Mp3FilePath)));
                }
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
            if (isEnabledShuffle == true)
            {
                int minShuffledIndex = 0;
                int maxShuffledIndex = userSongs.Count;

                int shuffledSongIndex = shuffledSongNumber.Next(minShuffledIndex, maxShuffledIndex);
                songNumber = shuffledSongIndex;
            }
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
                        if (userSongs.Count() == 0)
                        {
                            grdLibrary.ItemsSource = null;
                        }
                        else
                        {
                            // Reload your library or playlist
                            if (btnPlaylistImageEdit.Visibility == Visibility.Hidden)
                            {
                                songListRepopulation();
                            }
                            else
                            {
                                playlistSongsRepopulation();
                            }
                            // Simulate skipping to the next song, so it isn't showing up in the media player
                            if (grdLibrary.Items.Count != 0)
                            {
                                NextSongHelper();
                            }
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
        private void btnPlaylistImageEdit_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.Title = "Open File";
                openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;)|*.jpg;*.jpeg;*.png;|All Files (*.*)|*.*";

                bool? result = openFileDialog.ShowDialog();

                if (result == true)
                {
                    playlistImg = openFileDialog.FileName;
                    var PlaylistImage = new BitmapImage(new System.Uri(playlistImg));
                    imgPlaylistPicture.Source = PlaylistImage;


                    string destinationFolder = baseDirectory + "\\MuseConfig\\PlaylistImages";

                    if (!Directory.Exists(destinationFolder))
                    {
                        Directory.CreateDirectory(destinationFolder);
                    }

                    string newImageFilePath = System.IO.Path.Combine(destinationFolder, System.IO.Path.GetFileName(playlistImg));
                    File.Copy(playlistImg, newImageFilePath, true);

                    var songImage = new BitmapImage(new System.Uri(playlistImg));

                    imgPlaylistPicture.Source = songImage;

                    playlistImg = System.IO.Path.GetFileName(newImageFilePath);

                    UpdatePlaylistHelper();
                }
                else
                {
                    // user closes the file explorer before picking a photo
                    MessageBox.Show("Choose a photo to update your current account photo.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid image." + " " + ex.Message);
            }
        }
        private void grdPlaylists_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            txtDataGridHeaderEdit.Visibility = Visibility.Hidden;
            txtDataGridSubHeaderEdit.Visibility = Visibility.Hidden;
            if (grdPlaylists.SelectedItem != null)
            {
                if (grdPlaylists.SelectedItems.Count != 0)
                {
                    try
                    {
                        selectedPlaylist = (Playlist)grdPlaylists.SelectedItem;
                        int playlistID = ((Playlist)grdPlaylists.SelectedItem).PlaylistID;
                        int userID = loggedInUser.UserID;
                        string playlistName = ((Playlist)grdPlaylists.SelectedItem).Title;
                        string playlistDescription = ((Playlist)grdPlaylists.SelectedItem).Description;
                        userSongs = _songManager.SelectSongsByPlaylistID(userID, playlistID);
                        grdLibrary.ItemsSource = userSongs;

                        // Change the header and subheader of the playlist I'm currently on

                        btnPlaylistImageEdit.Visibility = Visibility.Visible;
                        imgPlaylistPicture.Visibility = Visibility.Visible;
                        try
                        {
                            var playlistImageFilePath = ((Playlist)grdPlaylists.SelectedItem).ImageFilePath;

                            if (playlistImageFilePath.IsDefaultImage())
                            {
                                playlistImageFilePath = baseDirectory + "\\MuseConfig\\PlaylistImages" + "defaultAlbumImage.png";
                            }

                            BitmapImage playlistImageBitmap = new BitmapImage(new System.Uri(playlistImageFilePath));
                            imgPlaylistPicture.Source = playlistImageBitmap;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Unable to find playlist image.",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        }
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

                    // handle if the last song of the playlist
                    playlistSongsRepopulation();

                    // if there is another song to skip to, run this
                    if (grdLibrary.Items.Count != 0)
                    {
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
        private void UpdatePlaylistHelper()
        {
            try
            {
                var oldPlaylist = new Playlist()
                {
                    PlaylistID = ((Playlist)grdPlaylists.SelectedItem).PlaylistID,
                    Title = ((Playlist)grdPlaylists.SelectedItem).Title,
                    ImageFilePath = System.IO.Path.GetFileName(((Playlist)grdPlaylists.SelectedItem).ImageFilePath),
                    Description = ((Playlist)grdPlaylists.SelectedItem).Description,
                };

                var newPlaylist = new Playlist()
                {
                    PlaylistID = ((Playlist)grdPlaylists.SelectedItem).PlaylistID,
                    Title = lblDataGridHeader.Content.ToString(),
                    ImageFilePath = playlistImg,
                    Description = lblDataGridSubHeader.Content.ToString(),
                };

                if (newPlaylist.ImageFilePath == "")
                {
                    newPlaylist.ImageFilePath = oldPlaylist.ImageFilePath;
                }

                _playlistManager.UpdatePlaylist(oldPlaylist, newPlaylist);

                // kick the user back to the library so they can regrab the selected song.
                userSongs = _songManager.SelectSongsByUserID(loggedInUser.UserID);
                lblDataGridHeader.Content = "Library";
                lblDataGridSubHeader.Content = "";
                grdLibrary.ItemsSource = userSongs;
                btnPlaylistImageEdit.Visibility = Visibility.Hidden;
                imgPlaylistPicture.Visibility = Visibility.Hidden;

                playlistListRepopulation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not update your playlist.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void txtDataGridSubHeaderEdit_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the key the user pressed was enter
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                lblDataGridSubHeader.Content = txtDataGridSubHeaderEdit.Text;
                txtDataGridSubHeaderEdit.Visibility = Visibility.Hidden;
                lblDataGridSubHeader.Visibility = Visibility.Visible;

                UpdatePlaylistHelper();
                playlistListRepopulation();
            }
        }
        private void txtDataGridHeaderEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                lblDataGridHeader.Content = txtDataGridHeaderEdit.Text;
                txtDataGridHeaderEdit.Visibility = Visibility.Hidden;
                lblDataGridHeader.Visibility = Visibility.Visible;

                UpdatePlaylistHelper();
                playlistListRepopulation();
            }
        }
        #endregion
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
            // set the edit button to be clickable
            btnUserEdit.IsEnabled = true;
            if (grdUsers.SelectedItems.Count != 0)
            {
                var User = grdUsers.SelectedItem as User;

                try
                {
                    ImageBrush imageBrush = (ImageBrush)btnUserProfileImage.Background;
                    BitmapImage profileImage = new BitmapImage(new Uri(User.ImageFilePath));
                    imageBrush.ImageSource = profileImage;

                    grdUsersRepopulation();

                    txtUserID.Text = User.UserID.ToString();
                    txtUserFirstName.Text = User.FirstName;
                    txtUserLastName.Text = User.LastName;
                    txtUserEmail.Text = User.Email;
                    txtUserProfileName.Text = User.ProfileName;
                    chkUserActive.IsChecked = User.Active;
                    txtMinutesListened.Text = User.MinutesListened.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, $"Unable to find the account image for \"{User.ProfileName}\". Please fix this.",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }
        private void btnUserEdit_Click(object sender, RoutedEventArgs e)
        {
            btnUserEdit.IsEnabled = false;
            btnUserSaveChanges.IsEnabled = true;
            btnUserProfileImage.IsEnabled = true;
            txtUserFirstName.IsEnabled = true;
            txtUserLastName.IsEnabled = true;
            chkUserActive.IsEnabled = true;
            txtMinutesListened.IsEnabled = true;
        }
        private void grdUsersRepopulation()
        {
            try
            {
                List<User> allUsers = _userManager.SelectAllUsers();
                lblDataGridHeader.Content = "All Users";
                grdUsers.Visibility = Visibility.Visible;
                grdUsers.ItemsSource = allUsers;

                txtUserFirstName.IsEnabled = false;
                txtUserLastName.IsEnabled = false;
                chkUserActive.IsEnabled = false;
                txtMinutesListened.IsEnabled = false;
                btnUserSaveChanges.IsEnabled = false;
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
            // re-enable the user edit button
            btnUserEdit.IsEnabled = true;
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

            string userEmail = txtUserEmail.Text;

            UserVM oldUser = _userManager.GetUserVMByEmail(userEmail);
            oldUser.ImageFilePath = System.IO.Path.GetFileName(oldUser.ImageFilePath);

            var newUser = new UserVM()
            {
                UserID = int.Parse(txtUserID.Text),
                ProfileName = txtUserProfileName.Text,
                Email = txtUserEmail.Text,
                FirstName = NewFirstName,
                LastName = NewLastName,
                ImageFilePath = System.IO.Path.GetFileName(userImg),
                Active = (bool)chkUserActive.IsChecked,
                MinutesListened = int.Parse(txtMinutesListened.Text),
                Roles = oldUser.Roles
            };

            if (userImg == "")
            {
                newUser.ImageFilePath = oldUser.ImageFilePath;
            }

            try
            {
                _userManager.UpdateUser(oldUser, newUser);
                MessageBox.Show("Account details have been updated", "Success!",
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
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.Title = "Open File";
                openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;)|*.jpg;*.jpeg;*.png;|All Files (*.*)|*.*";

                bool? result = openFileDialog.ShowDialog();

                if (result == true)
                {
                    userImg = openFileDialog.FileName;

                    string destinationFolder = baseDirectory + "\\MuseConfig\\ProfileImages";

                    if (!Directory.Exists(destinationFolder))
                    {
                        Directory.CreateDirectory(destinationFolder);
                    }

                    string newImageFilePath = System.IO.Path.Combine(destinationFolder, System.IO.Path.GetFileName(userImg));
                    File.Copy(userImg, newImageFilePath, true);

                    // Create the new imagebrush
                    ImageBrush imageBrush = (ImageBrush)btnUserProfileImage.Background;

                    // put the full file path instead of just the file name into the bitmap image.
                    var AccountImage = new BitmapImage(new System.Uri(newImageFilePath));
                    imageBrush.ImageSource = AccountImage;
                }
                else
                {
                    // user closes the file explorer before picking a photo
                    MessageBox.Show("Choose a photo to update your current account photo.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid image." + " " + ex.Message);
            }
        }
        #endregion
        private void lblDataGridHeader_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lblDataGridHeader.Content != "Library")
            {
                if (txtDataGridSubHeaderEdit.Visibility == Visibility.Hidden)
                {
                    lblDataGridHeader.Visibility = Visibility.Hidden;
                    txtDataGridHeaderEdit.Visibility = Visibility.Visible;
                    txtDataGridHeaderEdit.Text = "";
                    txtDataGridHeaderEdit.Focus();
                }
                else
                {
                    txtDataGridSubHeaderEdit.Visibility = Visibility.Hidden;
                    lblDataGridSubHeader.Visibility = Visibility.Visible;
                    txtDataGridHeaderEdit.Text = "";
                    txtDataGridHeaderEdit.Focus();
                }
            }
        }
        private void lblDataGridSubHeader_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (txtDataGridHeaderEdit.Visibility == Visibility.Hidden)
            {
                lblDataGridSubHeader.Visibility = Visibility.Hidden;
                txtDataGridSubHeaderEdit.Visibility = Visibility.Visible;
                txtDataGridSubHeaderEdit.Text = "";
                txtDataGridSubHeaderEdit.Focus();
            }
            else
            {
                txtDataGridHeaderEdit.Visibility = Visibility.Hidden;
                lblDataGridHeader.Visibility = Visibility.Visible;
                txtDataGridSubHeaderEdit.Text = "";
                txtDataGridSubHeaderEdit.Focus();
            }
        }
        private void mnuCreateReview_Click(object sender, RoutedEventArgs e)
        {
            var song = grdLibrary.SelectedItem as Song;

            if (grdLibrary.SelectedItem != null)
            {
                var AddReview = new AddReview(song, loggedInUser);
                AddReview.ShowDialog();
            }
            else
            {
                MessageBox.Show("Select a song song.");
            }
        }
        private void txtMinutesListened_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex numericRegex = new Regex("[^0-9]+");
            e.Handled = numericRegex.IsMatch(e.Text);
        }
        private void grdLibrary_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                var song = grdLibrary.SelectedItem as Song;

                if (grdLibrary.SelectedItem != null)
                {
                        MessageBoxResult result = MessageBox.Show(
                        $"Are you sure you want to delete {song.Title}?",
                        "Confirmation",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            _songManager.DeleteSong(song.SongID);
                            if (userSongs.Count() == 0)
                            {
                                grdLibrary.ItemsSource = null;
                                grdLibrary.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                // Reload your library or playlist
                                if (btnPlaylistImageEdit.Visibility == Visibility.Hidden)
                                {
                                    songListRepopulation();
                                }
                                else
                                {
                                    playlistSongsRepopulation();
                                }
                                // Simulate skipping to the next song, so it isn't showing up in the media player
                                if (grdLibrary.Items.Count != 0)
                                {
                                    NextSongHelper();
                                }
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
        }
        private void btnQueue_Click(object sender, RoutedEventArgs e)
        {

        }
        private void mnuPlaylistFolderToDb_Click(object sender, RoutedEventArgs e)
        {
            var p = new addPlaylistFolderFromFiles(loggedInUser);
            p.ShowDialog();
            updateUIForUserLogin();
        }
        private void mnuShuffleOn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void mnuShuffleOff_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}