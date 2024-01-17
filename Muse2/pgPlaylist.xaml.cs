using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
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
using Microsoft.Win32;

namespace Muse2
{
    /// <summary>
    /// Interaction logic for pgPlaylist.xaml
    /// </summary>
    public partial class pgPlaylist : Page
    {
        private UserVM _loggedInUser;
        private PlaylistManager _playlistManager;
        private SongManager _songManager;
        private ContextMenu contextMenu;
        public Song song;
        private List<Song> userSongs = null;
        public List<Song> _playlistSongs = null;
        private Playlist _selectedPlaylist = null;
        private string playlistImg = "";
        public int songNumber = 0;
        private string baseDirectory = AppContext.BaseDirectory;

        public pgPlaylist(UserVM loggedInUser, List<Song> playlistSongs, Playlist selectedPlaylist)
        {
            InitializeComponent();

            _loggedInUser = loggedInUser;
            _playlistSongs = playlistSongs;
            _selectedPlaylist = selectedPlaylist;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            grdPlaylist.ItemsSource = _playlistSongs;
            txtDataGridHeaderEdit.Visibility = Visibility.Hidden;
            txtDataGridSubHeaderEdit.Visibility = Visibility.Hidden;
            lblDataGridHeader.Content = _selectedPlaylist.Title;
            lblDataGridSubHeader.Content = _selectedPlaylist.Description;

            try
            {
                var PlaylistImage = new BitmapImage(new System.Uri(_selectedPlaylist.ImageFilePath));
                imgPlaylistPicture.Source = PlaylistImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not update your playlist image.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
                
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
        private void songListRepopulation()
        {
            try
            {
                userSongs = _songManager.SelectSongsByUserID(_loggedInUser.UserID);
                grdPlaylist.ItemsSource = userSongs;

                if (userSongs != null)
                {
                    grdPlaylist.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not find your library. Please try logging in again.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void Play()
        {
            // click the play button on the main window
            Window mainWindow = Window.GetWindow(this);
            Button btnPlay = mainWindow.FindName("btnPlay") as Button;
            if (btnPlay != null)
            {
                btnPlay.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
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
        private void grdPlaylist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdPlaylist.SelectedItems.Count != 0)
            {
                if (grdPlaylist.SelectedItem != null)
                {
                    int selectedRowIndex = grdPlaylist.Items.IndexOf(grdPlaylist.SelectedItem);
                    songNumber = selectedRowIndex;
                    song = grdPlaylist.SelectedItem as Song;
                }
                Play();
            }
            else
            {
                MessageBox.Show("Select a Song to listen to it.");
            }
        }
        private void grdPlaylist_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                var song = grdPlaylist.SelectedItem as Song;

                if (grdPlaylist.SelectedItem != null)
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
                                grdPlaylist.ItemsSource = null;
                                grdPlaylist.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                songListRepopulation();
                                if (grdPlaylist.Items.Count != 0)
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
        #region Context Menu 
        private void mnuEditSongFromDataGrid_Click(object sender, RoutedEventArgs e)
        {
            var song = grdPlaylist.SelectedItem as Song;

            if (grdPlaylist.SelectedItem != null)
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
            var song = grdPlaylist.SelectedItem as Song;

            if (grdPlaylist.SelectedItem != null)
            {
                var AddReview = new AddReview(song, _loggedInUser);
                AddReview.ShowDialog();
            }
            else
            {
                MessageBox.Show("Select a song.");
            }
        }

        private void mnuDeleteSong_Click(object sender, RoutedEventArgs e)
        {
            var song = grdPlaylist.SelectedItem as Song;

            if (grdPlaylist.SelectedItem != null)
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
                            grdPlaylist.ItemsSource = null;
                        }
                        else
                        {
                            songListRepopulation();
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
        private void UpdatePlaylistHelper()
        {
            try
            {
                var oldPlaylist = new Playlist()
                {
                    PlaylistID = ((Playlist)grdPlaylist.SelectedItem).PlaylistID,
                    Title = ((Playlist)grdPlaylist.SelectedItem).Title,
                    ImageFilePath = System.IO.Path.GetFileName(((Playlist)grdPlaylist.SelectedItem).ImageFilePath),
                    Description = ((Playlist)grdPlaylist.SelectedItem).Description,
                };

                var newPlaylist = new Playlist()
                {
                    PlaylistID = ((Playlist)grdPlaylist.SelectedItem).PlaylistID,
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
                userSongs = _songManager.SelectSongsByUserID(_loggedInUser.UserID);
                lblDataGridHeader.Content = "Library";
                lblDataGridSubHeader.Content = "";

                // grdLibrary.ItemsSource = userSongs;
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
    }
}
