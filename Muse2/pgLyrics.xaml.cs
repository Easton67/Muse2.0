using System.Windows;
using System.Windows.Controls;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Muse2
{
    /// <summary>
    /// Interaction logic for pgLyrics.xaml
    /// </summary>
    public partial class pgLyrics : Page
    {
        private const string GeniusApiBaseUrl = "https://api.genius.com";
        private const string AccessToken = "XWJIBe22GdHOtfYAhboNnxPX4ZnfjPjvjLNbMrZlOIM3tsJed6amxUkC3jrj3LRz";
        public string lyrics = "No Lyrics Provided";
        public string Lyrics { get; set; } = "No Lyrics Provided";
        public pgLyrics()
        {
            InitializeComponent();
        }

        private void txtLyrics_LostFocus(object sender, RoutedEventArgs e)
        {
            lyrics = txtLyrics.Text;
        }

        private void btnSearchForLyrics_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
