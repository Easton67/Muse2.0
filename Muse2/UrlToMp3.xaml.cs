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
        public UrlToMp3()
        {
            InitializeComponent();
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
            string title = "C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\net7.0-windows\\MuseConfig\\SongFiles\\" + txtSongTitle.Text + ".mp3";
            string formattedTitle = title.ToLower();
            string finalTitle = Regex.Replace(formattedTitle, @"\s", string.Empty);
            MessageBox.Show(finalTitle);

            try
            {
                await DownloadAndConvertAudioAsync(url, finalTitle);
                MessageBox.Show("Audio downloaded and converted successfully.");
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
