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
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        private string _accountImage;
        private string _firstName;
        private string _lastName;
        private string _profileName;
        private string _email;
        SongManager _songManager = null;

        public Profile(UserVM loggedInUser, SongManager _songManager)
        {
            _accountImage = loggedInUser.ImageFilePath;
            _firstName = loggedInUser.FirstName;
            _lastName = loggedInUser.LastName;
            _profileName = loggedInUser.ProfileName;
            _email = loggedInUser.Email;

            InitializeComponent();
        }

        private void mnuExitApplcation_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            imgStatsAccont.Source = new BitmapImage(new System.Uri(_accountImage));
            imgAccontImage.Source = new BitmapImage(new System.Uri(_accountImage));
            txtFirstName.Text = _firstName;
            txtLastName.Text = _lastName;
            txtEmail.Text = _email;
            txtProfileName.Text = _profileName;
        }

        private void btnLogo_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void btnResetPassword_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddChanges_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteAccount_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
