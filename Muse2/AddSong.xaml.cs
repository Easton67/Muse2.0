using DataObjects;
using LogicLayer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
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
using System.Xml.Linq;

namespace Muse2
{
    /// <summary>
    /// Interaction logic for AddSong.xaml
    /// </summary>
    public partial class AddSong : Window
    {
        private UserVM _loggedInUser = null;
        private string _mp3File = "";
        private string _imgFile = "";
        private string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public AddSong(UserVM loggedInUser)
        {
            InitializeComponent();

            _loggedInUser = loggedInUser;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CleanWindow();
            SongInformationHelper();
        }
        private void CleanWindow()
        {
            btnSongInfomation.Background = Brushes.White;
            btnArtwork.Background = Brushes.White;
            btnLyrics.Background = Brushes.White;

            // Song Info Tab
            btnAddMp3.Visibility = Visibility.Hidden;
            lblMp3File.Visibility = Visibility.Hidden;
            lblTitle.Visibility = Visibility.Hidden;
            lblArtist.Visibility = Visibility.Hidden;
            lblAlbum.Visibility = Visibility.Hidden;
            lblYear.Visibility = Visibility.Hidden;
            txtMp3FilePath.Visibility = Visibility.Hidden;
            txtTitle.Visibility = Visibility.Hidden;
            txtArtist.Visibility = Visibility.Hidden;
            txtAlbum.Visibility = Visibility.Hidden;
            txtYear.Visibility = Visibility.Hidden;
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
        private void SongInformationHelper()
        {
            btnSongInfomation.Background = Brushes.Lavender;
            btnAddMp3.Visibility = Visibility.Visible;
            lblMp3File.Visibility = Visibility.Visible;
            lblTitle.Visibility = Visibility.Visible;
            lblArtist.Visibility = Visibility.Visible;
            lblAlbum.Visibility = Visibility.Visible;
            lblYear.Visibility = Visibility.Visible;
            lblExplicit.Visibility = Visibility.Visible;
            lblPlays.Visibility = Visibility.Visible;

            txtMp3FilePath.Visibility = Visibility.Visible;
            txtTitle.Visibility = Visibility.Visible;
            txtArtist.Visibility = Visibility.Visible;
            txtAlbum.Visibility = Visibility.Visible;
            txtYear.Visibility = Visibility.Visible;
            txtPlays.Visibility = Visibility.Visible;
            chkExplicit.Visibility = Visibility.Visible;
        }
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
        }
        private void btnLyrics_Click(object sender, RoutedEventArgs e)
        {
            CleanWindow();
            btnLyrics.Background = Brushes.Lavender;
            lblLyrics.Visibility = Visibility.Visible;
            txtLyrics.Visibility = Visibility.Visible;
        }
        private void btnCreateSong_Click(object sender, RoutedEventArgs e)
        {
            var newSong = new Song()
            {
                Title = txtTitle.Text,
                ImageFilePath = _imgFile,
                Mp3FilePath = _mp3File,
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
                var sm = new SongManager();
                bool result = sm.InsertSong(newSong);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }
        private void btnRemoveArtwork_Click(object sender, RoutedEventArgs e)
        {
            imgSongImage.Source = null;
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

                    // Copy the selected image file to AlbumArt
                    string newImageFilePath = System.IO.Path.Combine(destinationFolder, System.IO.Path.GetFileName(_imgFile));
                    File.Copy(_imgFile, newImageFilePath, true);

                    var songImage = new BitmapImage(new System.Uri(_imgFile));

                    imgSongImage.Source = songImage;

                    // get just the file name since we handle the pathing down at the Data Access Layer
                    _imgFile = System.IO.Path.GetFileName(newImageFilePath);
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
        private void txtYear_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void lblPlays_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void btnAddMp3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.Title = "Open Mp3 File";
                openFileDialog.Filter = "MP3 Files (*.mp3)|*.mp3|All Files (*.*)|*.*";

                bool? result = openFileDialog.ShowDialog();

                if (result == true)
                {
                    _mp3File = openFileDialog.FileName;

                    string destinationFolder = baseDirectory + "\\MuseConfig\\SongFiles";

                    if (!Directory.Exists(destinationFolder))
                    {
                        Directory.CreateDirectory(destinationFolder);
                    }

                    // Copy the selected MP3 file to SongFiles
                    string newFilePath = System.IO.Path.Combine(destinationFolder, System.IO.Path.GetFileName(_mp3File));
                    File.Copy(_mp3File, newFilePath, true);

                    _mp3File = System.IO.Path.GetFileName(newFilePath);

                    txtMp3FilePath.Text = baseDirectory + "\\MuseConfig\\SongFiles\\" + _mp3File;
                }
                else
                {
                    MessageBox.Show("Choose an MP3 to add.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Unable to create your song. " +
                "Please make sure your file is correct.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
