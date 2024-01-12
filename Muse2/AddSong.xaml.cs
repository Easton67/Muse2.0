using DataObjects;
using LogicLayer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Muse2
{
    /// <summary>
    /// Interaction logic for AddSong.xaml
    /// </summary>
    public partial class AddSong : Window
    {
        private string _mp3FileName;
        private string _songTitle;
        private string _artistName;
        private int _yearReleased;
        private bool _isExplicit;
        private int _plays;
        private UserVM _loggedInUser = null;
        private string _mp3File = "";
        // set this to the default when adding, and then change it if a picture is selected
        private string _imgFile = "defaultAlbumImage.png";
        private string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        Regex numericRegex = new Regex("[^0-9]+");

        Dictionary<string, Page> pages = new Dictionary<string, Page>();
        public AddSong(UserVM loggedInUser)
        {
            InitializeComponent();

            _loggedInUser = loggedInUser;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CloseWindow.win = this;
            pages.Add("frmSongInformation", new pgAddSong());
            pages.Add("frmArtwork", new pgArtwork());
            pages.Add("frmLyrics", new pgLyrics());

            frmMain.Navigate(pages["frmSongInformation"]);
        }
        private void btnSongInfomation_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(pages["frmSongInformation"]);
        }
        private void btnArtwork_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(pages["frmArtwork"]);
        }
        private void btnLyrics_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(pages["frmLyrics"]);
        }
        private void btnCreateSong_Click(object sender, RoutedEventArgs e)
        {
            if (!_mp3File.IsValidMP3())
            {
                MessageBox.Show("That is not a valid mp3 file Path", "Invalid song file path",
                MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            //int year= 0;
            //if(txtYear.Text != "")
            //{
            //    year = int.Parse(txtYear.Text);
            //}
            //else
            //{
            //    year = 0;
            //}
            //if (!year.IsValidYear())
            //{
            //    MessageBox.Show("That is not a valid year", "Invalid Year",
            //    MessageBoxButton.OK, MessageBoxImage.Error);
            //    CleanWindow();
            //    SongInformationHelper();
            //    return;
            //}
            //if (txtMp3FilePath.Text == "")
            //{
            //    MessageBox.Show("Click the Add MP3 button to add a song file.", "No MP3 file selected");
            //    return;
            //}
            //try
            //{
            //    var newSong = new Song()
            //    {
            //        Title = txtTitle.Text,
            //        ImageFilePath = _imgFile,
            //        Mp3FilePath = _mp3File,
            //        YearReleased = year,
            //        Lyrics = txtLyrics.Text,
            //        Explicit = (bool)chkExplicit.IsChecked,
            //        Plays = int.Parse(txtPlays.Text),
            //        UserID = _loggedInUser.UserID,
            //        Album = txtAlbum.Text,
            //        Artist = txtArtist.Text
            //    };
            //    var sm = new SongManager();
            //    bool result = sm.InsertSong(newSong);
            //    this.Close();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            //}
        }

        private void btnTestSong_Click(object sender, RoutedEventArgs e)
        {
            if (frmMain.Content is pgAddSong addSongPage)
            {
                _mp3FileName = addSongPage.mp3FileName;
                _songTitle = addSongPage.songTitle;
                _artistName = addSongPage.artistName;
                _yearReleased = addSongPage.yearReleased;
                _isExplicit = addSongPage.isExplicit;
                _plays = addSongPage.plays;
            }
            MessageBox.Show(_mp3FileName);
            MessageBox.Show(_songTitle);
            MessageBox.Show(_artistName);
            MessageBox.Show(_yearReleased.ToString());
            MessageBox.Show(_isExplicit.ToString());
            MessageBox.Show(_plays.ToString());
        }
    }
}
