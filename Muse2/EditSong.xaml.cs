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
using System.Windows.Shapes;
using LogicLayer;
using System.Diagnostics;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.IO;

namespace Muse2
{
    /// <summary>
    /// Interaction logic for AddEditSongxaml.xaml
    /// </summary>
    public partial class AddEditSongxaml : Window
    {
        private Song _song = null; 
        private UserVM _loggedInUser = null;
        private string _imgFile = "";
        private string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public AddEditSongxaml(Song song, UserVM loggedInUser)
        {
            InitializeComponent();

            _song = song;
            _imgFile = _song.ImageFilePath;

            _loggedInUser = loggedInUser;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CleanWindow();
            SongInformationHelper();
        }
        // Reset Method
        private void CleanWindow()
        {
            btnSongInfomation.Background = Brushes.White;
            btnArtwork.Background = Brushes.White;
            btnLyrics.Background = Brushes.White;

            // Song Info Tab
            lblTitle.Visibility = Visibility.Hidden;
            lblArtist.Visibility = Visibility.Hidden;
            lblAlbum.Visibility = Visibility.Hidden;
            lblYear.Visibility = Visibility.Hidden;
            txtTitle.Visibility = Visibility.Hidden;
            txtArtist.Visibility = Visibility.Hidden;
            txtAlbum.Visibility = Visibility.Hidden;
            txtYear.Visibility = Visibility.Hidden;
            btnDeleteSong.Visibility = Visibility.Hidden;
            chkExplicit.Visibility = Visibility.Hidden;
            lblExplicit.Visibility = Visibility.Hidden;
            lblPlays.Visibility = Visibility.Hidden;
            txtPlays.Visibility = Visibility.Hidden;

            // Artwork Tab
            lblSongArt.Visibility = Visibility.Hidden;
            imgSongImage.Visibility = Visibility.Hidden;
            btnAddArtwork.Visibility = Visibility.Hidden;
            lblLyrics.Visibility = Visibility.Hidden;
            btnRemoveArtwork.Visibility = Visibility.Hidden;

            //Lyrics Tab
            lblLyrics.Visibility = Visibility.Hidden;
            txtLyrics.Visibility = Visibility.Hidden;
        }
        // Sets the window's initial state
        private void SongInformationHelper()
        {
            btnSongInfomation.Background = Brushes.Lavender;
            lblTitle.Visibility = Visibility.Visible;
            lblArtist.Visibility = Visibility.Visible;
            lblAlbum.Visibility = Visibility.Visible;
            lblYear.Visibility = Visibility.Visible;
            lblExplicit.Visibility = Visibility.Visible;
            lblPlays.Visibility = Visibility.Visible;

            txtTitle.Visibility = Visibility.Visible;
            txtArtist.Visibility = Visibility.Visible;
            txtAlbum.Visibility = Visibility.Visible;
            txtYear.Visibility = Visibility.Visible;
            txtPlays.Visibility = Visibility.Visible;
            chkExplicit.Visibility = Visibility.Visible;
            btnDeleteSong.Visibility = Visibility.Visible;

            // Set the current song's details
            txtTitle.Text = _song.Title;
            txtArtist.Text = _song.Artist;
            txtAlbum.Text = _song.Album;
            txtYear.Text = _song.YearReleased.ToString();
            chkExplicit.IsChecked = _song.Explicit;
            txtPlays.Text = _song.Plays.ToString();
            txtLyrics.Text = _song.Lyrics;

        }
        // Navigation Buttons
        private void btnSongInfomation_Click(object sender, RoutedEventArgs e)
        {
            CleanWindow();
            SongInformationHelper();
        }
        private void btnArtwork_Click(object sender, RoutedEventArgs e)
        {
            CleanWindow();
            btnArtwork.Background = Brushes.Lavender;

            lblSongArt.Visibility = Visibility.Visible;
            imgSongImage.Visibility = Visibility.Visible;
            btnAddArtwork.Visibility = Visibility.Visible;
            btnRemoveArtwork.Visibility = Visibility.Visible;

            try
            {
                var SongImage = new BitmapImage(new System.Uri(_song.ImageFilePath));
                imgSongImage.Source = SongImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to find your profile photo. ", ex.Message);
            }
        }
        private void btnLyrics_Click(object sender, RoutedEventArgs e)
        {
            CleanWindow();
            btnArtwork.Background = Brushes.White;
            btnLyrics.Background = Brushes.Lavender;
            lblLyrics.Visibility = Visibility.Visible;
            txtLyrics.Visibility = Visibility.Visible;
            txtLyrics.Text = _song.Lyrics;
        }
        // Tab specific buttons
        private void btnDeleteSong_Click(object sender, RoutedEventArgs e)
        {
            SongManager _songManager = new SongManager();
            try
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Are you sure you want to delete '{_song.Title}'?",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _songManager.DeleteSong(_song.SongID);
                        MessageBox.Show("Song successfully deleted");
                        Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not delete this song. Please try again",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to delete song." + " " + ex.Message);
            }
        }
        private void btnAddArtwork_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.Title = "Open File";
                openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;)|*.jpg;*.jpeg;*.png;|All Files (*.*)|*.*";

                bool? result = openFileDialog.ShowDialog();

                if (result == true)
                {
                    _imgFile = openFileDialog.FileName;

                    string destinationFolder = baseDirectory + "\\MuseConfig\\AlbumArt";

                    if (!Directory.Exists(destinationFolder))
                    {
                        Directory.CreateDirectory(destinationFolder);
                    }

                    string newImageFilePath = System.IO.Path.Combine(destinationFolder, System.IO.Path.GetFileName(_imgFile));
                    File.Copy(_imgFile, newImageFilePath, true);

                    var songImage = new BitmapImage(new System.Uri(_imgFile));

                    imgSongImage.Source = songImage;


                }
                else
                {
                    MessageBox.Show("Choose a photo to update your current account photo.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid image." + " " + ex.Message);
            }
        }
        private void btnAddChanges_Click(object sender, RoutedEventArgs e)
        {
            SongManager _songManager = new SongManager();

            var oldSong = this._song;

            var newSong = new Song()
            {
                SongID = oldSong.SongID,
                Title = txtTitle.Text,
                ImageFilePath = _imgFile,
                Mp3FilePath = oldSong.Mp3FilePath,
                YearReleased = int.Parse(txtYear.Text),
                Lyrics = txtLyrics.Text,
                Explicit = (bool)chkExplicit.IsChecked,
                Plays = int.Parse(txtPlays.Text),
                UserID = _loggedInUser.UserID,
                Album = txtAlbum.Text,
                Artist = txtArtist.Text
            };

            try
            {
                 _songManager.UpdateSong(oldSong, newSong);
                MessageBox.Show("Song Successfully updted!");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to update your song." + " " + ex.Message);
            }
        }
        private void txtYear_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-9]");
            e.Handled = !regex.IsMatch(e.Text);
        }
        private void lblPlays_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
