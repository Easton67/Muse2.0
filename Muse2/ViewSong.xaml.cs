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

        public ViewSong(Song s)
        {
            song = s;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
            try
            {
                imgCoverArt.Source = new BitmapImage(new System.Uri(song.ImageFilePath));
            }
            catch
            {
                imgCoverArt.Source = null;
            }
        }
    }
}
