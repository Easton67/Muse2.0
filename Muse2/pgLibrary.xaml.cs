using LogicLayer;
using DataObjects;
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
    /// Interaction logic for pgLibrary.xaml
    /// </summary>
    public partial class pgLibrary : Page
    {
        private UserVM loggedInUser;
        private UserManager _userManager;
        private SongManager _songManager;
        private PlaylistManager _playlistManager;
        public int songNumber = 0;
        public Song song;
        private List<Song> userSongs = null;

        public pgLibrary(UserVM _loggedInUser)
        {
            InitializeComponent();

            loggedInUser = _loggedInUser;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _songManager = new SongManager();
            _playlistManager = new PlaylistManager();

            songListRepopulation();
        }

        private void grdLibrary_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdLibrary.SelectedItems.Count != 0)
            {
                if (grdLibrary.SelectedItem != null)
                {
                    int selectedRowIndex = grdLibrary.Items.IndexOf(grdLibrary.SelectedItem);
                    songNumber = selectedRowIndex;
                    song = grdLibrary.SelectedItem as Song;
                }

                //lblSongTitle.Content = Song.Title;
                //lblSongArtist.Content = Song.Artist;
                //if (Song.Explicit == true)
                //{
                //    imgExplicit.Visibility = Visibility.Visible;
                //}
                //else
                //{
                //    imgExplicit.Visibility = Visibility.Hidden;
                //}
                //CurrentSongHelper();
                //btnPlay.Visibility = Visibility.Hidden;
                //btnPause.Visibility = Visibility.Visible;
                //if (grdLibrary.SelectedItem != null)
                //{
                //    int selectedRowIndex = grdLibrary.Items.IndexOf(grdLibrary.SelectedItem);
                //    songNumber = selectedRowIndex;
                //}
                //mediaPlayer.Play();
                //timer.Start();
            }
            else
            {
                MessageBox.Show("Select a Song to listen to it.");
            }
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

        private void mnuAddSongToPlaylistFromDataGrid_Click(object sender, RoutedEventArgs e)
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
    }
}
