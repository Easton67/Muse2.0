using DataObjects;
using LogicLayer;
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

namespace Muse2
{
    /// <summary>
    /// Interaction logic for ViewSong.xaml
    /// </summary>
    public partial class ViewSong : Window
    {
        private Song song = null;
        private string imageFilePath = ""; 
        private string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public ViewSong(Song s)
        {
            song = s;

            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string defaultImagePath = baseDirectory + "\\MuseConfig\\AlbumArt\\defaultAlbumImage.png";
                BitmapImage albumArtwork = (imageFilePath != null || imageFilePath != "")
                            ? new BitmapImage(new System.Uri(song.ImageFilePath))
                            : new BitmapImage(new System.Uri(baseDirectory + "\\MuseConfig\\AlbumArt\\defaultAlbumImage.png"));
                imgCoverArt.Source = albumArtwork;

                // Get the average color
                Color averageColor = GetAverageColor(albumArtwork);
                this.Background = new SolidColorBrush(averageColor);
                Color textColor = IsColorTooDark(averageColor) ? Colors.White : Colors.Black;
                lblTitle.Foreground = new SolidColorBrush(textColor);
                lblArtist.Foreground = new SolidColorBrush(textColor);
                txtLyrics.Foreground = new SolidColorBrush(textColor);

                lblTitle.Content = song.Title;
                if (song.Explicit == true)
                {
                    imgExplicit.Visibility = Visibility.Visible;
                }
                else
                {
                    imgExplicit.Visibility = Visibility.Hidden;
                }
                lblArtist.Content = song.Artist;
                txtLyrics.Text = song.Lyrics;
                if (txtLyrics.Text == "")
                {
                    txtLyrics.Text = "No Lyrics";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not find this song. Please try again",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        static bool IsColorTooDark(Color color)
        {
            // brightness formula (0.299R + 0.587G + 0.114B)
            double brightness = (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;

            double brightnessThreshold = 0.5;

            return brightness < brightnessThreshold;
        }
        private static Color GetAverageColor(BitmapImage bitmapImage)
        {
            FormatConvertedBitmap convertedBitmap = new FormatConvertedBitmap(bitmapImage, PixelFormats.Pbgra32, null, 0);

            int width = convertedBitmap.PixelWidth;
            int height = convertedBitmap.PixelHeight;

            int stride = width * 4; // 4 bytes per pixel (BGRA)

            byte[] pixels = new byte[height * stride];
            convertedBitmap.CopyPixels(pixels, stride, 0);

            int totalRed = 0;
            int totalGreen = 0;
            int totalBlue = 0;

            for (int i = 0; i < pixels.Length; i += 4)
            {
                totalBlue += pixels[i];
                totalGreen += pixels[i + 1];
                totalRed += pixels[i + 2];
            }

            int pixelCount = width * height;
            byte averageBlue = (byte)(totalBlue / pixelCount);
            byte averageGreen = (byte)(totalGreen / pixelCount);
            byte averageRed = (byte)(totalRed / pixelCount);

            return Color.FromRgb(averageRed, averageGreen, averageBlue);
        }
        private void txtLyrics_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            // Adjust the vertical scroll position based on the mouse wheel delta
            if (e.Delta > 0)
            {
                textBox.ScrollToVerticalOffset(textBox.VerticalOffset - 8);
            }
            else
            {
                textBox.ScrollToVerticalOffset(textBox.VerticalOffset + 8);
            }

            // Mark the event as handled to prevent it from being handled by the default scroll behavior
            e.Handled = true;
        }
    }
}
