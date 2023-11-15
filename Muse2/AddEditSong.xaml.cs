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

namespace Muse2
{
    /// <summary>
    /// Interaction logic for AddEditSongxaml.xaml
    /// </summary>
    public partial class AddEditSongxaml : Window
    {
        private Song song = null;

        public AddEditSongxaml(Song s)
        {
            song = s;

            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CleanWindow();
            SongInformationHelper();
            // ArtworkHelper();
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

            txtTitle.Visibility = Visibility.Visible;
            txtArtist.Visibility = Visibility.Visible;
            txtAlbum.Visibility = Visibility.Visible;
            txtYear.Visibility = Visibility.Visible;

            txtTitle.Text = song.Title;
            txtArtist.Text = song.Artist;
            txtAlbum.Text = song.Album;
            txtYear.Text = song.YearReleased.ToString();
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
                var SongImage = new BitmapImage(new System.Uri(song.ImageFilePath));
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
            btnArtwork.Background = Brushes.Lavender;
            lblLyrics.Visibility = Visibility.Visible;
            txtLyrics.Visibility = Visibility.Visible;
            txtLyrics.Text = song.Lyrics;
        }
        // Tab specific buttons
        private void btnDeleteSong_Click(object sender, RoutedEventArgs e)
        {

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
                    song.ImageFilePath = openFileDialog.FileName;
                    var SongImage = new BitmapImage(new System.Uri(song.ImageFilePath));

                    imgSongImage.Source = SongImage;
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
    }
}
