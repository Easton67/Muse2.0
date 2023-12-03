using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Muse2
{
    /// <summary>
    /// Interaction logic for AddReview.xaml
    /// </summary>
    public partial class AddReview : Window
    {
        private Song _song = null;
        private UserVM _loggedInUser = null;
        private string _imgFile = "";

        public AddReview(Song song, UserVM loggedInUser)
        {
            InitializeComponent();

            _loggedInUser = loggedInUser;

            _song = song;
            _imgFile = _song.ImageFilePath;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            imgSongImage.Source = new BitmapImage(new System.Uri(_song.ImageFilePath));
            lblSongTitle.Content = _song.Title;
            lblArtist.Content = _song.Artist;
            lblAlbumTitle.Content = _song.Album;
            lblYearReleased.Content = _song.YearReleased.ToString();
            txtRating.Focus();
        }
        private void txtRating_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-5]");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void btnCreateReview_Click(object sender, RoutedEventArgs e)
        {
            ReviewManager _reviewManager = new ReviewManager();

            var review = new Review()
            {
                Rating = int.Parse(txtRating.Text),
                Message = txtReviewMessage.Text,
                UserID = _loggedInUser.UserID,
                SongID = _song.SongID
            };

            try
            {
                _reviewManager.CreateReview(review);
                MessageBox.Show("You can view your reviews inside of your profile", "Success!");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not create review. Please try again.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                txtRating.Text = "";
                txtReviewMessage.Text = "";
                txtRating.Focus();
            }
        }
    }
}
