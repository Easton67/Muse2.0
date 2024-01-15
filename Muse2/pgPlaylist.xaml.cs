using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
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

namespace Muse2
{
    /// <summary>
    /// Interaction logic for pgPlaylist.xaml
    /// </summary>
    public partial class pgPlaylist : Page
    {
        private UserVM _loggedInUser;
        private PlaylistManager _playlistManager;
        private ContextMenu contextMenu;
        public pgPlaylist(UserVM loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;
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

        private void grdPlaylist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void grdPlaylist_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void mnuEditSongFromDataGrid_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mnuAddSongToPlaylistFromDataGrid_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mnuCreateReview_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mnuDeleteSong_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
