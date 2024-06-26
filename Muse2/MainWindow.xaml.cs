﻿using DataObjects;
using LogicLayer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Muse2.Pages.Library;
using Muse2.Windows.Login;

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
        private string userImg = "";
        private List<Song> userSongs = null;
        private Stack<Song> queue = null;
        private ContextMenu contextMenu;
        private Playlist selectedPlaylist;
        private string baseDirectory = AppContext.BaseDirectory;
        private bool isEnabledShuffle;
        private Song selectedSong;
        Dictionary<string, Page> pages = new Dictionary<string, Page>();
        private double SongCurrentPosition;

        public MainWindow(UserVM LoggedInUser)
        {
            InitializeComponent();

            Application.Current.MainWindow = this;

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
                SongCurrentPosition = mediaPlayer.Position.TotalSeconds;
                double LengthToSendToQueue = 10.00;

                barSongLength.Maximum = SongLengthInSeconds;
                barSongLength.Minimum = 00.00;
                barSongLength.Value = SongCurrentPosition;

                sliderSongLength.Minimum = 00.00;
                sliderSongLength.Maximum = SongLengthInSeconds;
                sliderSongLength.Value = SongCurrentPosition;

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
                    queue.Push(selectedSong);
                    MessageBox.Show("Sent to queue");
                }

                if (SongCurrentPosition == SongLengthInSeconds)
                {
                    UpdateSongPlayCount();
                    songListRepopulation();
                    Next();
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
            pages.Add("frmUsers", new pgUsers(loggedInUser));

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
            sliderSongLength.Value = 00.00;
            lblDataGridHeader.Visibility = Visibility.Collapsed;
            lblDataGridSubHeader.Visibility = Visibility.Collapsed;
            txtDataGridHeaderEdit.Visibility = Visibility.Collapsed;
            txtDataGridSubHeaderEdit.Visibility = Visibility.Collapsed;
            btnPlaylistImageEdit.Visibility = Visibility.Collapsed;

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
                userSongs = _songManager.SelectSongsByUserID(loggedInUser.UserID);

                if (userSongs.Count() > 0)
                {
                    // load the first song in the list
                    selectedSong = userSongs[songNumber];
                    btnViewSong.Visibility = Visibility.Visible;
                    try
                    {
                        imgCoverArt.Source = new BitmapImage(new System.Uri(selectedSong.ImageFilePath));
                    }
                    catch (Exception)
                    {
                        imgCoverArt.Source = new BitmapImage(new System.Uri(baseDirectory + "\\MuseConfig\\AlbumArt\\defaultAlbumImage.png"));
                    }
                    mediaPlayer.Open(new Uri(selectedSong.Mp3FilePath));
                    imgExplicit.Visibility = (selectedSong.Explicit) ? Visibility.Visible : Visibility.Hidden;
                    lblSongTitle.Content = selectedSong.Title;
                    lblSongArtist.Content = selectedSong.Artist;

                    // set song controls
                    lblCurrentTime.Visibility = Visibility.Visible;
                    lblSongLength.Visibility = Visibility.Visible;
                    imgCoverArt.Visibility = Visibility.Visible;
                    btnRewind.Visibility = Visibility.Visible;
                    btnPlay.Visibility = Visibility.Visible;
                    btnNext.Visibility = Visibility.Visible;
                    barSongLength.Visibility = Visibility.Visible;
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
            Play(selectedSong, null);
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
            Next();
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
            songNumber = (songNumber <= 0) ? userSongs.Count - 1 : songNumber - 1;
            CurrentSongHelper(selectedSong);
            if (btnPause.IsVisible)
            {
                mediaPlayer.Play();
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
        }
        private void mnuExitApplcation_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer = null;
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
            SignIn signInWindow = new SignIn(loggedInUser);
            signInWindow.Show();
            signInWindow.NavigateToResetPassword();
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
            SignIn signInWindow = new SignIn(loggedInUser);
            signInWindow.Show();
            signInWindow.NavigateToSignUp();
        }
        private void mnuViewAllUsers_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(pages["frmAdmin"]);
            frmListOfPlaylists.Navigate(pages["frmUsers"]);
        }
        #endregion
        private void btnProfileName_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            timer.Stop();
            mediaPlayer.Pause();
            var profileWindow = new Profile(loggedInUser, _songManager);
            profileWindow.ShowDialog();
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
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #region Song Events
        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateCurrentTimeLabel();
        }
        private void UpdateCurrentTimeLabel()
        {
            TimeSpan currentPosition = mediaPlayer.Position;
            lblCurrentTime.Content = currentPosition.ToString(@"mm\:ss");
        }
        public Song GetCurrentSong(Song librarySong, Page callingPage)
        {
            if (callingPage is pgLibrary)
            {
                pgLibrary libraryPage = (pgLibrary)callingPage;
                librarySong = userSongs[libraryPage.songNumber];
            }
            return librarySong;
        }
        private void sliderSongLength_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SongCurrentPosition = e.NewValue;
            mediaPlayer.Position = TimeSpan.FromSeconds(SongCurrentPosition);
            sliderSongLength.Value = SongCurrentPosition;
            barSongLength.Value = SongCurrentPosition;
            timer.Interval = TimeSpan.FromSeconds(SongCurrentPosition);
            lblCurrentTime.Content = mediaPlayer.Position.ToString(@"mm\:ss");
            timer.Start();
        }
        private void btnViewSong_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var ViewSong = new ViewSong(selectedSong);
            ViewSong.ShowDialog();
        }
        private void btnQueue_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            Pause();
        }
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            Play(selectedSong, null);
        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Next();
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
                Next();
            }
        }
        #endregion
        # region Main Song Methods
        public void Pause()
        {
            btnPlay.Visibility = Visibility.Visible;
            btnPause.Visibility = Visibility.Hidden;
            timer.Stop();
            mediaPlayer.Pause();
        }
        public void Play(Song librarySong, Page callingPage)
        {
            selectedSong = GetCurrentSong(librarySong, callingPage);
            if (mediaPlayer.Position >= TimeSpan.FromSeconds(1))
            {
                TimeSpan currentPosition = mediaPlayer.Position;
                lblCurrentTime.Content = currentPosition.ToString(@"mm\:ss");
                mediaPlayer.Play();
                btnPlay.Visibility = Visibility.Hidden;
                btnPause.Visibility = Visibility.Visible;
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;
                timer.Start();
            }
            mediaPlayer.Open(new Uri(selectedSong.Mp3FilePath));
            CurrentSongHelper(selectedSong);
            mediaPlayer.Play();
            btnPlay.Visibility = Visibility.Hidden;
            btnPause.Visibility = Visibility.Visible;
            mediaPlayer.Play();
            timer.Start();
        }
        private void Next()
        {
            // long one-liner just to find the next song in the list without using a new variable
            selectedSong = userSongs[userSongs.IndexOf(selectedSong) + 1];
            barSongLength.Value = 00.00;
            sliderSongLength.Value = 00.00;
            CurrentSongHelper(selectedSong); 
            mediaPlayer.Open(new Uri(selectedSong.Mp3FilePath));
            if (btnPause.Visibility == Visibility.Visible)
            {
                mediaPlayer.Play();
                timer.Start();
            }
            else
            {
                mediaPlayer.Stop();
            }
        }
        private void Rewind()
        {
            if(mediaPlayer.Position >= TimeSpan.FromSeconds(1))
            {
                mediaPlayer.Stop();
                mediaPlayer.Play();
                return;
            }

            selectedSong = userSongs[userSongs.IndexOf(selectedSong) - 1];
            Play(selectedSong, null);
            barSongLength.Value = 00.00;
            sliderSongLength.Value = 00.00;
        }
        #endregion
        #region Song Control Helpers
        public void CurrentSongHelper(Song selectedSong)
        {

            lblSongTitle.Content = selectedSong.Title;
            lblSongArtist.Content = selectedSong.Artist;
            imgExplicit.Visibility = (selectedSong.Explicit) ? Visibility.Visible : Visibility.Hidden;
            try
            {
                imgCoverArt.Source = (selectedSong.ImageFilePath.Length > 1) ?
                    new BitmapImage(new System.Uri(selectedSong.ImageFilePath)) :
                    new BitmapImage(new System.Uri(AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\defaultAlbumImage.png"));
            }
            catch (Exception)
            {
                imgCoverArt.Source = new BitmapImage(new System.Uri(AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\defaultAlbumImage.png"));
            }

        }
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
        #endregion
        #region Playlist
        private void playlistListRepopulation()
        {
            // set the playlists
            //try
            //{
            //    List<Playlist> playlists = _playlistManager.SelectPlaylistsByUserID(loggedInUser.UserID);

            //    contextMenu = new ContextMenu();

            //    List<string> playlistTitles = new List<string>();

            //    // Add playlist title to a new list of just the title
            //    foreach (Playlist playlist in playlists)
            //    {
            //        string playlistTitle = playlist.Title;
            //        playlistTitles.Add(playlistTitle);
            //    }

            //    MenuItem editSong = new MenuItem();
            //    editSong.Header = "Edit Song Details";
            //    // editSong.Click += mnuAddSongFromDataGrid_Click;
            //    contextMenu.Items.Add(editSong);

            //    MenuItem writeReview = new MenuItem();
            //    writeReview.Header = "Write a review";
            //    // writeReview.Click += mnuCreateReview_Click;
            //    contextMenu.Items.Add(writeReview);

            //    MenuItem newPlaylist = new MenuItem();
            //    newPlaylist.Header = "New Playlist";
            //    // newPlaylist.Click += mnuCreateNewPlaylist_Click;
            //    contextMenu.Items.Add(newPlaylist);

            //    if (playlists.Count > 0)
            //    {
            //        MenuItem deleteSong = new MenuItem();
            //        deleteSong.Header = "Delete Song";
            //        // deleteSong.Click += mnuDeleteSong_Click;
            //        contextMenu.Items.Add(deleteSong);

            //        MenuItem addSong = new MenuItem();
            //        addSong.Header = "Add Song To Playlist:";
            //        contextMenu.Items.Add(addSong);

            //        // Add the list of playlist titles to the context menu
            //        foreach (string menuItemText in playlistTitles)
            //        {
            //            MenuItem menuItem = new MenuItem();
            //            menuItem.Header = menuItemText;
            //            menuItem.Click += mnuAddSongToPlaylistFromDataGrid_Click;
            //            addSong.Items.Add(menuItem);
            //        }
            //    }

            //    // grdLibrary.ContextMenu = contextMenu;

            //    if (playlists.Count > 0)
            //    {
            //        // grdPlaylists.Visibility = Visibility.Visible;
            //        // grdPlaylists.ItemsSource = playlists;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not find your playlists. Please try again.",
            //    MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
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
        private void UpdatePlaylistHelper()
        {
            //try
            //{
            //    var oldPlaylist = new Playlist()
            //    {
            //        PlaylistID = ((Playlist)grdPlaylist.SelectedItem).PlaylistID,
            //        Title = ((Playlist)grdPlaylist.SelectedItem).Title,
            //        ImageFilePath = System.IO.Path.GetFileName(((Playlist)grdPlaylist.SelectedItem).ImageFilePath),
            //        Description = ((Playlist)grdPlaylist.SelectedItem).Description,
            //    };

            //    var newPlaylist = new Playlist()
            //    {
            //        PlaylistID = ((Playlist)grdPlaylist.SelectedItem).PlaylistID,
            //        Title = lblDataGridHeader.Content.ToString(),
            //        ImageFilePath = playlistImg,
            //        Description = lblDataGridSubHeader.Content.ToString(),
            //    };

            //    if (newPlaylist.ImageFilePath == "")
            //    {
            //        newPlaylist.ImageFilePath = oldPlaylist.ImageFilePath;
            //    }

            //    _playlistManager.UpdatePlaylist(oldPlaylist, newPlaylist);

            //    // kick the user back to the library so they can regrab the selected song.
            //    userSongs = _songManager.SelectSongsByUserID(_loggedInUser.UserID);
            //    lblDataGridHeader.Content = "Library";
            //    lblDataGridSubHeader.Content = "";

            //    // grdLibrary.ItemsSource = userSongs;
            //    btnPlaylistImageEdit.Visibility = Visibility.Hidden;
            //    imgPlaylistPicture.Visibility = Visibility.Hidden;

            //    playlistListRepopulation();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not update your playlist.",
            //    MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
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
        private void lblDataGridHeader_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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
                    userImg = openFileDialog.FileName;

                    string destinationFolder = baseDirectory + "\\MuseConfig\\ProfileImages";

                    if (!Directory.Exists(destinationFolder))
                    {
                        Directory.CreateDirectory(destinationFolder);
                    }

                    string newImageFilePath = System.IO.Path.Combine(destinationFolder, System.IO.Path.GetFileName(userImg));
                    File.Copy(userImg, newImageFilePath, true);

                    // Create the new imagebrush
                    // ImageBrush imageBrush = (ImageBrush)btnUserProfileImage.Background;

                    // put the full file path instead of just the file name into the bitmap image.
                    var AccountImage = new BitmapImage(new System.Uri(newImageFilePath));
                    // imageBrush.ImageSource = AccountImage;
                }
                else
                {
                    // user closes the file explorer before picking a photo
                    System.Windows.MessageBox.Show("Choose a photo to update your current account photo.");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Invalid image." + " " + ex.Message);
            }
        }
        public void HideLibrary()
        {
            lblLibrary.Visibility = Visibility.Collapsed;
        }
        public void ShowLibrary()
        {
            lblLibrary.Visibility = Visibility.Visible;
        }
        public void ShowPlaylistItems()
        {
            lblDataGridHeader.Visibility = Visibility.Visible;
            lblDataGridSubHeader.Visibility = Visibility.Visible;
            btnPlaylistImageEdit.Visibility = Visibility.Visible;

            lblLibrary.Visibility = Visibility.Collapsed;
        }
        public void HidePlaylistItems()
        {
            lblLibrary.Visibility = Visibility.Visible;

            lblDataGridHeader.Visibility = Visibility.Collapsed;
            lblDataGridSubHeader.Visibility = Visibility.Collapsed;
            txtDataGridHeaderEdit.Visibility = Visibility.Collapsed;
            txtDataGridSubHeaderEdit.Visibility = Visibility.Collapsed;
            btnPlaylistImageEdit.Visibility = Visibility.Collapsed;
        }
        #endregion
    }
}