using DataObjects;
using LogicLayer;
using NAudio.Lame;
using NAudio.Wave;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using YoutubeExplode;

namespace Muse2
{
    /// <summary>
    /// Interaction logic for UrlToMp3.xaml
    /// </summary>
    public partial class UrlToMp3 : Window
    {
        private UserVM _loggedInUser = null;
        private string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public UrlToMp3(UserVM loggedInUser)
        {
            InitializeComponent();

            _loggedInUser = loggedInUser;
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

        private async void btnCreateMp3FromURL_Click(object sender, RoutedEventArgs e)
        {
            string url = txtURL.Text;
            string title = baseDirectory + "\\MuseConfig\\SongFiles\\" + txtSongTitle.Text + ".mp3";
            string formattedTitle = title.ToLower();
            string finalTitle = Regex.Replace(formattedTitle, @"\s", string.Empty);
            string mp3Title = Regex.Replace(txtSongTitle.Text + ".mp3", @"\s", string.Empty).ToLower();

            try
            {
                await DownloadAndConvertAudioAsync(url, finalTitle);

                try
                {
                    SongManager sm = new SongManager();
                    var newSong = new Song()
                    {
                        Title = txtSongTitle.Text,
                        ImageFilePath = "defaultAlbumImage.png",
                        Mp3FilePath = mp3Title,
                        YearReleased = 2023,
                        Lyrics = "No Lyrics Provided",
                        Explicit = false,
                        Plays = 0,
                        UserID = _loggedInUser.UserID,
                        Album = "Unknown",
                        Artist = "Unknown"
                    };
                    sm.InsertSong(newSong);
                    MessageBox.Show("Song added to your library successfully.");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Song could not be added. Please try again.",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "URL didn't convert, please try again",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
