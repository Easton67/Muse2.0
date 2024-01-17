using DataObjects;
using LogicLayer;
using Microsoft.VisualBasic.Logging;
using Microsoft.Win32;
using System;
using System.Collections;
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
        private DispatcherTimer timer = new DispatcherTimer();
        private int minutesPassed = 0;
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private UserManager _userManager;
        private UserVM loggedInUser;
        private SongManager _songManager;
        private PlaylistManager _playlistManager;
        private int songNumber = 0;
        private Random shuffledSongNumber = new Random();
        private int userNumber = 0;
        private string playlistImg = "";
        private List<Song> userSongs = null;
        private List<Song> queue = null;
        private ContextMenu contextMenu;
        private Playlist selectedPlaylist;
        private string baseDirectory = AppContext.BaseDirectory;
        private bool isEnabledShuffle;
        private Song selectedSong;
        Dictionary<string, Page> pages = new Dictionary<string, Page>();

        public MainWindow(UserVM LoggedInUser)
        {
            InitializeComponent();

            loggedInUser = LoggedInUser;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += SongTimer;
        }
        public void HideLibrary()
        {
            lblLibrary.Visibility = Visibility.Collapsed;
        }
        public void ShowLibrary()
        {
            lblLibrary.Visibility = Visibility.Visible;
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
                double LengthToSendToQueue = 10.00;

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

                if (SongCurrentPosition == LengthToSendToQueue)
                {
                    queue.Add(selectedSong);
                    MessageBox.Show("Sent to queue");
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
            pages.Add("frmLibrary", new pgLibrary(loggedInUser));
            pages.Add("frmAdmin", new pgAdmin(loggedInUser));
            pages.Add("frmListOfPlaylists", new pgPlaylistList(loggedInUser));

            frmMain.Navigate(pages["frmLibrary"]);
            frmListOfPlaylists.Navigate(pages["frmListOfPlaylists"]);

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
            //mediaPlayer.Pause();

            // Hide role specific  items
            //mnuAlbum.Visibility = Visibility.Collapsed;
            mnuFile.Visibility = Visibility.Collapsed;
            mnuSong.Visibility = Visibility.Collapsed;
            mnuAccount.Visibility = Visibility.Collapsed;
            mnuControls.Visibility = Visibility.Collapsed;
            mnuAdmin.Visibility = Visibility.Collapsed;
            mnuPlaylist.Visibility = Visibility.Collapsed;
            mnuResetPassword.Visibility = Visibility.Collapsed;

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
                    mediaPlayer.Open(new Uri(userSongs[songNumber].Mp3FilePath));
                    if (userSongs[0].Explicit == true)
                    {
                        imgExplicit.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        imgExplicit.Visibility = Visibility.Hidden;
                    }
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

                    playlistListRepopulation();
                }
                else
                {
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
                //grdLibrary.ItemsSource = userSongs;

                if (userSongs != null)
                {
                    // grdLibrary.Visibility = Visibility.Visible;
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
                // editSong.Click += mnuAddSongFromDataGrid_Click;
                contextMenu.Items.Add(editSong);

                MenuItem writeReview = new MenuItem();
                writeReview.Header = "Write a review";
                // writeReview.Click += mnuCreateReview_Click;
                contextMenu.Items.Add(writeReview);

                MenuItem newPlaylist = new MenuItem();
                newPlaylist.Header = "New Playlist";
                // newPlaylist.Click += mnuCreateNewPlaylist_Click;
                contextMenu.Items.Add(newPlaylist);

                if (playlists.Count > 0)
                {
                    MenuItem deleteSong = new MenuItem();
                    deleteSong.Header = "Delete Song";
                    // deleteSong.Click += mnuDeleteSong_Click;
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

                // grdLibrary.ContextMenu = contextMenu;

                if (playlists.Count > 0)
                {
                    // grdPlaylists.Visibility = Visibility.Visible;
                    // grdPlaylists.ItemsSource = playlists;
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
            //try
            //{
            //    int playlistID = ((Playlist)grdPlaylists.SelectedItem).PlaylistID;
            //    int userID = loggedInUser.UserID;
            //    string playlistName = ((Playlist)grdPlaylists.SelectedItem).Title;
            //    string playlistDescription = ((Playlist)grdPlaylists.SelectedItem).Description;
            //    userSongs = _songManager.SelectSongsByPlaylistID(userID, playlistID);
            //    // grdLibrary.ItemsSource = userSongs;

            //    // Change the header and subheader of the playlist I'm currently on

            //    btnPlaylistImageEdit.Visibility = Visibility.Visible;
            //    imgPlaylistPicture.Visibility = Visibility.Visible;
            //    var playlistImageFilePath = ((Playlist)grdPlaylists.SelectedItem).ImageFilePath;
            //    BitmapImage playlistImageBitmap = new BitmapImage(new System.Uri(playlistImageFilePath));
            //    imgPlaylistPicture.Source = playlistImageBitmap;
            //    lblDataGridHeader.Visibility = Visibility.Visible;
            //    lblDataGridHeader.Content = playlistName;
            //    lblDataGridSubHeader.Visibility = Visibility.Visible;
            //    lblDataGridSubHeader.Content = playlistDescription;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Unable to view playlist. Please try again.",
            //    MessageBoxButton.OK, MessageBoxImage.Error);
            //}
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
        private void mnuShuffleOn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void mnuShuffleOff_Click(object sender, RoutedEventArgs e)
        {

        }
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
            // grdUsers.Visibility = Visibility.Hidden;
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
            // grdLibrary.Visibility = Visibility.Hidden;
            // grdPlaylists.Visibility = Visibility.Hidden;
            // lblDataGridSubHeader.Visibility = Visibility.Hidden;
            // btnAllSongs.Visibility = Visibility.Hidden;

            // panelSelectedUser.Visibility = Visibility.Visible;

            // grdUsersRepopulation();
        }
        #endregion
        private void btnProfileName_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            timer.Stop();
            mediaPlayer.Pause();
            var profileWindow = new Profile(loggedInUser, _songManager);
            profileWindow.ShowDialog();
            //if (grdLibrary.Visibility == Visibility.Visible)
            //{
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
            //}
            //else if (grdUsers.Visibility == Visibility.Visible)
            //{
            //    grdUsersRepopulation();
            //}
            //else
            //{
            //    playlistListRepopulation();
            //}
        }
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #region Song Controls
        private void btnQueue_Click(object sender, RoutedEventArgs e)
        {

        }
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
            pgLibrary libraryPage = (pgLibrary)pages["frmLibrary"];
            selectedSong = libraryPage.song;
            songNumber = libraryPage.songNumber;

            // add song that is being played to the queue
            // past songs played

            CurrentSongHelper();

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
                if (userSongs[songNumber].ImageFilePath != null || userSongs[songNumber].ImageFilePath == "")
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
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Song cover unable to be found" +
                "Please make sure your image file exists",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void CurrentSongHelper()
        {
            try
            {
                GetSongCover();
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
                mediaPlayer.Open(new Uri(userSongs[songNumber].Mp3FilePath));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Song file could not be played. " +
                "Please make sure your song file exists",
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
        private void mnuRemoveSongFromPlaylist_Click(object sender, RoutedEventArgs e)
        {
            // var song = grdLibrary.SelectedItem as Song;

            //if (grdLibrary.SelectedItem != null)
            //{
            //    try
            //    {
            //        _playlistManager.RemoveSongFromPlaylist(song.SongID);

            //        // handle if the last song of the playlist
            //        playlistSongsRepopulation();

            //        // if there is another song to skip to, run this
            //        if (grdLibrary.Items.Count != 0)
            //        {
            //            NextSongHelper();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not remove this song. Please try again",
            //        MessageBoxButton.OK, MessageBoxImage.Error);
            //        return;
            //    }
            //}
        }
        #endregion
    }
}