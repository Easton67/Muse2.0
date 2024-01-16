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
namespace Muse2
{
    /// <summary>
    /// Interaction logic for pgPlaylistList.xaml
    /// </summary>
    public partial class pgPlaylistList : Page
    {
        private UserVM _loggedInUser;
        public  Playlist selectedPlaylist;
        private PlaylistManager _playlistManager;
        private SongManager _songManager;
        private ContextMenu contextMenu;
        private List<Song> userSongs = null;
        private List<Song> playlistSongs = null;
        private string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private int userNumber;
        Dictionary<string, Page> pages = new Dictionary<string, Page>();

        public pgPlaylistList(UserVM loggedInUser)
        {
            InitializeComponent();

            _loggedInUser = loggedInUser;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _songManager = new SongManager();
            _playlistManager = new PlaylistManager();

            playlistListRepopulation();
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
                newPlaylist.Click += mnuCreateNewPlaylist_Click;
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
                        // menuItem.Click += mnuAddSongToPlaylistFromDataGrid_Click;
                        addSong.Items.Add(menuItem);
                    }
                }

                // grdLibrary.ContextMenu = contextMenu;

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
        private void grdPlaylists_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdPlaylists.SelectedItem != null)
            {
                if (grdPlaylists.SelectedItems.Count != 0)
                {
                    try
                    {
                        selectedPlaylist = (Playlist)grdPlaylists.SelectedItem;
                        int userID = _loggedInUser.UserID;
                        int playlistID = selectedPlaylist.PlaylistID;
                        playlistSongs = _songManager.SelectSongsByPlaylistID(userID, playlistID);

                        Window mainWindow = Window.GetWindow(this);
                        if (mainWindow != null)
                        {
                            Frame frmMain = mainWindow.FindName("frmMain") as Frame;

                            if (frmMain != null && frmMain.NavigationService != null)
                            {
                                frmMain.NavigationService.Navigate(new pgPlaylist(_loggedInUser, playlistSongs));
                            }
                        }
                        string playlistName = ((Playlist)grdPlaylists.SelectedItem).Title;
                        string playlistDescription = ((Playlist)grdPlaylists.SelectedItem).Description;

                        // Change the header and subheader of the playlist I'm currently on

                        // btnPlaylistImageEdit.Visibility = Visibility.Visible;
                        // imgPlaylistPicture.Visibility = Visibility.Visible;
                        try
                        {
                            var playlistImageFilePath = ((Playlist)grdPlaylists.SelectedItem).ImageFilePath;

                            if (playlistImageFilePath.IsDefaultImage())
                            {
                                playlistImageFilePath = baseDirectory + "\\MuseConfig\\PlaylistImages" + "defaultAlbumImage.png";
                            }

                            BitmapImage playlistImageBitmap = new BitmapImage(new System.Uri(playlistImageFilePath));
                            // imgPlaylistPicture.Source = playlistImageBitmap;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Unable to find playlist image.",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        // lblDataGridHeader.Visibility = Visibility.Visible;
                        // lblDataGridHeader.Content = playlistName;
                        // lblDataGridSubHeader.Visibility = Visibility.Visible;
                        // lblDataGridSubHeader.Content = playlistDescription;
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
        private void mnuCreateNewPlaylist_Click(object sender, RoutedEventArgs e)
        {
            var addPlaylist = new AddPlaylist(_loggedInUser);
            addPlaylist.ShowDialog();
            try
            {
                grdPlaylists.ItemsSource = _playlistManager.SelectPlaylistByUserID(_loggedInUser.UserID);
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
        private void btnAllSongs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window mainWindow = Window.GetWindow(this);
                if (mainWindow != null)
                {
                    Frame frmMain = mainWindow.FindName("frmMain") as Frame;

                    if (frmMain != null && frmMain.NavigationService != null)
                    {
                        frmMain.NavigationService.Navigate(new pgLibrary(_loggedInUser));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Unable to find library. Please try again.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                // grdLibrary.ItemsSource = null;
            }
        }
    }
}
