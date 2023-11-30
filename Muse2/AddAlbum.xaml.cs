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

namespace Muse2
{
    /// <summary>
    /// Interaction logic for AddAlbum.xaml
    /// </summary>
    public partial class AddAlbum : Window
    {
        private UserVM _loggedInUser = null;
        private string _imgFile = "";

        public AddAlbum(UserVM loggedInUser)
        {
            InitializeComponent();

            _loggedInUser = loggedInUser;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnAlbumCover_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

    }
}
