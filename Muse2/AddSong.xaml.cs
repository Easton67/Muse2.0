using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using YoutubeExplode;
using Muse2.Pages.AddSong;

namespace Muse2
{
    /// <summary>
    /// Interaction logic for AddSong.xaml
    /// </summary>
    public partial class AddSong : Window
    {
        private UserVM _loggedInUser = null;
        private string _lyrics;
        private string _mp3FileName;
        private string _songTitle;
        private string _artistName;
        private int _yearReleased;
        private bool _isExplicit;
        private int _plays;
        private string _mp3File = "";
        private string _fullImageFilePath;
        // set this to the default when adding, and then change it if a picture is selected
        private string _imageFilePath = "defaultAlbumImage.png";
        private string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        Regex numericRegex = new Regex("[^0-9]+");

        Dictionary<string, Page> pages = new Dictionary<string, Page>();
        public AddSong(UserVM loggedInUser)
        {
            InitializeComponent();

            _loggedInUser = loggedInUser;
        }
        private void CleanWindow()
        {
            btnSongInfomation.Background = Brushes.White;
            btnArtwork.Background = Brushes.White;
            btnLyrics.Background = Brushes.White;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CloseWindow.win = this;
            pages.Add("frmSongInformation", new pgAddSong(_loggedInUser));
            pages.Add("frmArtwork", new pgArtwork());
            pages.Add("frmLyrics", new pgLyrics());

            frmMain.Navigate(pages["frmSongInformation"]);
        }
        private void btnSongInfomation_Click(object sender, RoutedEventArgs e)
        {
            CleanWindow();
            btnSongInfomation.Background = Brushes.Lavender;
            frmMain.Navigate(pages["frmSongInformation"]);
        }
        private void btnArtwork_Click(object sender, RoutedEventArgs e)
        {
            CleanWindow();
            btnArtwork.Background = Brushes.Lavender;
            frmMain.Navigate(pages["frmArtwork"]);
        }
        private void btnLyrics_Click(object sender, RoutedEventArgs e)
        {
            CleanWindow();
            btnLyrics.Background = Brushes.Lavender;
            frmMain.Navigate(pages["frmLyrics"]);
        }
        static async Task DownloadAndConvertAudioAsync(string videoUrl, string outputFilePath)
        {
            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(videoUrl);

            if (video != null)
            {
                var streamInfoSet = await youtube.Videos.Streams.GetManifestAsync(videoUrl);
                var audioStreamInfo = streamInfoSet.GetAudioOnlyStreams().FirstOrDefault();

                if (audioStreamInfo != null)
                {
                    var audioStream = await youtube.Videos.Streams.GetAsync(audioStreamInfo);

                    using (var audioFileStream = File.Create(outputFilePath))
                    {
                        await audioStream.CopyToAsync(audioFileStream);
                    }
                }
            }
        }
        private async void btnCreateSong_Click(object sender, RoutedEventArgs e)
        {
            // grab the public properties from each page in the frame

            pgAddSong addSongPage = (pgAddSong)pages["frmSongInformation"];
            _mp3FileName = addSongPage.mp3FileName;
            _songTitle = addSongPage.songTitle;
            _artistName = addSongPage.artistName;
            _yearReleased = addSongPage.yearReleased;
            _isExplicit = addSongPage.isExplicit;
            _plays = addSongPage.plays;

            if (_mp3FileName.StartsWith("https://www.youtube.com"))
            {
                try
                {
                    // extract the mp3 from the url provided
                    string url = _mp3FileName;
                    string mp3File = Regex.Replace(_songTitle + ".mp3", @"\s", string.Empty).ToLower();
                    string mp3FilePath = baseDirectory + "\\MuseConfig\\SongFiles\\" + mp3File;
                    _mp3FileName = mp3File;

                    await DownloadAndConvertAudioAsync(url, mp3FilePath);
                }
                catch (Exception)
                {
                    MessageBox.Show("That is not a valid youtube URL.", "Invalid url file path",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                if (!_mp3FileName.IsValidMP3())
                {
                    MessageBox.Show("That is not a valid mp3 file Path", "Invalid song file path",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            if (_imageFilePath.IsDefaultImage() || _imageFilePath == null)
            {
                _imageFilePath = baseDirectory + "\\MuseConfig\\AlbumArt\\defaultAlbumImage.png";
                return;
            }

            if (!_yearReleased.IsValidYear())
            {
                _yearReleased = 2024;
                return;
            }

            pgArtwork addArtwork = (pgArtwork)pages["frmArtwork"];
            _imageFilePath = addArtwork.imageFilePath;
            _fullImageFilePath = addArtwork.fullImageFilePath;

            pgLyrics addLyrics = (pgLyrics)pages["frmLyrics"];
            _lyrics = addLyrics.lyrics;

            try
            {
                var newSong = new Song()
                {
                    Title = _songTitle,
                    ImageFilePath = _imageFilePath,
                    Mp3FilePath = _mp3FileName,
                    YearReleased = _yearReleased,
                    Lyrics = _lyrics,
                    Explicit = _isExplicit,
                    Plays = _plays,
                    UserID = _loggedInUser.UserID,
                    Artist = _artistName,
                    Album = "",
                    Genre = "",
                    DateUploaded = null,
                    DateAdded = DateTime.Now.Date
                };

                newSong.Photo = File.ReadAllBytes(_fullImageFilePath);
                newSong.PhotoMimeType = GetMimeType(_fullImageFilePath);

                var sm = new SongManager();
                bool result = sm.InsertSong(newSong);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }
        private string GetMimeType(string filePath)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(filePath).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
            {
                mimeType = regKey.GetValue("Content Type").ToString();
            }
            return mimeType;
        }
    }
}
