using DataObjects;
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
        private Playlist selectedPlaylist;
        private string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private PlaylistManager _playlistManager;
        private SongManager _songManager;
        private ContextMenu contextMenu;
        private List<Song> userSongs = null;
        private int userNumber;
        public pgPlaylistList(UserVM loggedInUser)
        {
            InitializeComponent();

            _loggedInUser = loggedInUser;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            playlistListRepopulation();
        }

        private void grdPlaylists_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // txtDataGridHeaderEdit.Visibility = Visibility.Hidden;
            // txtDataGridSubHeaderEdit.Visibility = Visibility.Hidden;
            if (grdPlaylists.SelectedItem != null)
            {
                if (grdPlaylists.SelectedItems.Count != 0)
                {
                    try
                    {
                        selectedPlaylist = (Playlist)grdPlaylists.SelectedItem;
                        int playlistID = ((Playlist)grdPlaylists.SelectedItem).PlaylistID;
                        int userID = _loggedInUser.UserID;
                        string playlistName = ((Playlist)grdPlaylists.SelectedItem).Title;
                        string playlistDescription = ((Playlist)grdPlaylists.SelectedItem).Description;
                        userSongs = _songManager.SelectSongsByPlaylistID(userID, playlistID);
                        // grdLibrary.ItemsSource = userSongs;

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
                        MessageBox.Show("Playlist successfully deleted");
                        // Reload your library or playlist
                        //if (btnPlaylistImageEdit.Visibility == Visibility.Hidden)
                        //{
                        //    songListRepopulation();
                        //}
                        //else
                        //{
                        //    playlistSongsRepopulation();
                        //}
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
    }
}
