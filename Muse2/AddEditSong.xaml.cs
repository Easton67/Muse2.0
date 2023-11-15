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
            txtTitle.Text = song.Title;
            txtArtist.Text = song.Artist;
            txtAlbum.Text = song.Album;
            txtYear.Text = song.YearReleased.ToString();
        }
    }
}
