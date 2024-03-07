using LogicLayer;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Muse2.Pages.Library
{
    /// <summary>
    /// Interaction logic for pgLibrary.xaml
    /// </summary>
    public partial class pgLibrary : Page
    {
        private UserVM _loggedInUser;
        private UserManager _userManager;
        private SongManager _songManager;
        private PlaylistManager _playlistManager;
        public int songNumber = 0;
        public Song song;
        public List<Song> userSongs = null;
        private ContextMenu contextMenu;
        Window mainWindow = null;
        List<Song> filteredSongs = new List<Song>();
        private Song selectedSong = new Song();

        public pgLibrary(UserVM loggedInUser)
        {
            InitializeComponent();

            _loggedInUser = loggedInUser;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindow = Window.GetWindow(this);

            _songManager = new SongManager();
            _playlistManager = new PlaylistManager();

            songListRepopulation();
            playlistListRepopulation();

            song = userSongs[0];
        }
        private void AddSongToQueue()
        {
            // click the play button on the main window
            Button btnQueue = mainWindow.FindName("btnQueue") as Button;
            if (btnQueue != null)
            {
                btnQueue.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
        private void Next()
        {
            // click the play button on the main window
            Window mainWindow = Window.GetWindow(this);
            Button btnPlay = mainWindow.FindName("btnNext") as Button;
            if (btnPlay != null)
            {
                btnPlay.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
        private void playlistListRepopulation()
        {
            // set the playlists
            try
            {
                List<Playlist> playlists = _playlistManager.SelectPlaylistByUserID(_loggedInUser.UserID);

                contextMenu = new ContextMenu();

                List<string> playlistTitles = new List<string>();

                // Add playlist title to a new list of just the title
                foreach (Playlist playlist in playlists)
                {
                    string playlistTitle = playlist.Title;
                    playlistTitles.Add(playlistTitle);
                }

                MenuItem addSongToQueue = new MenuItem();
                addSongToQueue.Header = "Add Song To Queue";
                addSongToQueue.Click += mnuQueueNextSong_Click;
                contextMenu.Items.Add(addSongToQueue);

                MenuItem editSong = new MenuItem();
                editSong.Header = "Edit Song Details";
                editSong.Click += mnuEditSongFromDataGrid_Click;
                contextMenu.Items.Add(editSong);

                MenuItem writeReview = new MenuItem();
                writeReview.Header = "Write a review";
                writeReview.Click += mnuCreateReview_Click;
                contextMenu.Items.Add(writeReview);

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
        private void songListRepopulation()
        {
            try
            {
                userSongs = _songManager.SelectSongsByUserID(_loggedInUser.UserID);
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
        private void grdLibrary_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdLibrary.SelectedItems.Count != 0 && grdLibrary.SelectedItem != null)
            {
                int selectedRowIndex = grdLibrary.Items.IndexOf(grdLibrary.SelectedItem);
                songNumber = selectedRowIndex;

                if (grdLibrary.ItemsSource == filteredSongs)
                {
                    string selectedMp3FilePath = filteredSongs[selectedRowIndex].Mp3FilePath;
                    songNumber = userSongs.FindIndex(song => song.Mp3FilePath == selectedMp3FilePath);
                }
                song = grdLibrary.SelectedItem as Song;
                if (Application.Current.MainWindow is MainWindow s)
                {
                    s.Play(song, this);
                }
            }
        }
        #region Context Menu 
        private void mnuQueueNextSong_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void mnuEditSongFromDataGrid_Click(object sender, RoutedEventArgs e)
        {
            var song = grdLibrary.SelectedItem as Song;

            if (grdLibrary.SelectedItem != null)
            {
                var EditSong = new AddEditSongxaml(song, _loggedInUser);
                EditSong.ShowDialog();
                songListRepopulation();
            }
            else
            {
                MessageBox.Show("Select a Song to view it.");
            }
        }
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
                        List<Playlist> playlists = _playlistManager.SelectPlaylistByUserID(_loggedInUser.UserID);
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
        private void mnuCreateReview_Click(object sender, RoutedEventArgs e)
        {
            var song = grdLibrary.SelectedItem as Song;

            if (grdLibrary.SelectedItem != null)
            {
                var AddReview = new AddReview(song, _loggedInUser);
                AddReview.ShowDialog();
            }
            else
            {
                MessageBox.Show("Select a song.");
            }
        }
        private void DeleteOneOrMoreSongs(Song selectedSong)
        {
            var song = grdLibrary.SelectedItem as Song;

            if (grdLibrary.SelectedItem != null)
            {
                if (grdLibrary.SelectedItems.Count > 1)
                {
                    MessageBoxResult result = MessageBox.Show(
                         $"Are you sure you want to delete {grdLibrary.SelectedItems.Count} songs from your library?",
                         "Confirmation",
                         MessageBoxButton.YesNo,
                         MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        foreach (var selectedItem in grdLibrary.SelectedItems)
                        {
                            try
                            {
                                if (selectedItem is Song s)
                                {
                                    _songManager.DeleteSong(selectedSong.SongID);
                                    songNumber = grdLibrary.Items.IndexOf(grdLibrary.SelectedItem);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not delete this song. Please try again",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                        Window mainWindow = Window.GetWindow(this);

                        if (mainWindow != null)
                        {
                            if (mainWindow is MainWindow mainWin)
                            {
                                Frame frmMain = mainWin.FindName("frmMain") as Frame;

                                if (frmMain != null && frmMain.NavigationService != null)
                                {
                                    songNumber = 0;
                                    MessageBox.Show("Songs Successfully Deleted!");
                                    if (txtSearch.Text.Equals(""))
                                    {
                                        userSongs = _songManager.SelectSongsByUserID(_loggedInUser.UserID);
                                        List<Song> filteredSongs = new List<Song>();
                                        filteredSongs = userSongs.Where(x => x.Title.ToLower().Contains(txtSearch.Text.ToLower())).ToList();
                                        grdLibrary.ItemsSource = filteredSongs;
                                    }
                                    mainWin.CurrentSongHelper(selectedSong);
                                    mainWin.Pause();
                                }
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
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
                                if (txtSearch.Text != (""))
                                {
                                    userSongs = _songManager.SelectSongsByUserID(_loggedInUser.UserID);
                                    List<Song> filteredSongs = new List<Song>();
                                    filteredSongs = userSongs.Where(x => x.Title.ToLower().Contains(txtSearch.Text.ToLower()) 
                                                                      || x.Artist.ToLower().Contains(txtSearch.Text.ToLower())).ToList();
                                    grdLibrary.ItemsSource = filteredSongs;
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
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Select a Song to view it.");
            }
        }
        private void mnuDeleteSong_Click(object sender, RoutedEventArgs e)
        {
            DeleteOneOrMoreSongs(selectedSong);
        }
        #endregion
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
                                songListRepopulation();
                                if (grdLibrary.Items.Count != 0)
                                {
                                    Next();
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
        private void grdLibrary_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (grdLibrary.SelectedItems.Count > 1)
            {
                ContextMenu multipleItemsContextMenu = new ContextMenu();

                MenuItem addSongsToPlaylist = new MenuItem();
                addSongsToPlaylist.Header = "Add Songs To Playlist";
                multipleItemsContextMenu.Items.Add(addSongsToPlaylist);

                MenuItem removeSongsFromPlaylist = new MenuItem();
                removeSongsFromPlaylist.Header = "Remove Songs From Library";
                removeSongsFromPlaylist.Click += mnuDeleteSong_Click;
                multipleItemsContextMenu.Items.Add(removeSongsFromPlaylist);

                grdLibrary.ContextMenu = multipleItemsContextMenu;
            }
            else
            {
                grdLibrary.ContextMenu = contextMenu;
            }
        }
        #region Search Box 
        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text.Replace(" ", "").Equals("Search"))
            {
                txtSearch.Text = "";
            }
        }
        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text.Equals(""))
            {
                txtSearch.Text = "Search";
            }
        }
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearch.Text.Equals(""))
            {
                songListRepopulation();
            }
            if (txtSearch.Text != "Search")
            {
                userSongs = _songManager.SelectSongsByUserID(_loggedInUser.UserID);
                grdLibrary.ItemsSource = userSongs;

                filteredSongs = new List<Song>();
                filteredSongs = userSongs.Where(x => x.Title.ToLower().Contains(txtSearch.Text.ToLower()) || x.Artist.ToLower().Contains(txtSearch.Text.ToLower())).ToList();
                grdLibrary.ItemsSource = filteredSongs;
            }
        }
        #endregion
    }
}
