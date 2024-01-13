using System.Windows;
using System.Windows.Controls;

namespace Muse2
{
    /// <summary>
    /// Interaction logic for pgLyrics.xaml
    /// </summary>
    public partial class pgLyrics : Page
    {
        public string lyrics;
        public pgLyrics()
        {
            InitializeComponent();
        }

        private void txtLyrics_LostFocus(object sender, RoutedEventArgs e)
        {
            lyrics = txtLyrics.Text;
        }
    }
}
