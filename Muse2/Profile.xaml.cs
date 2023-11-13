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

        public Profile(UserVM loggedInUser, SongManager _songManager)
        {
            _accountImage = loggedInUser.ImageFilePath;
            _firstName = loggedInUser.FirstName;
            _lastName = loggedInUser.LastName;
            _profileName = loggedInUser.ProfileName;
            _email = loggedInUser.Email;

            InitializeComponent();
        }
        // Menu Items
        private void mnuExitApplcation_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var AccountImage = new BitmapImage(new System.Uri(_accountImage));
                imgStatsAccont.Source = AccountImage;
                imgAccontImage.Source = AccountImage;
                imgFavoritesAccontImage.Source = AccountImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to find your profile photo. ", ex.Message);
            }
            txtFirstName.Text = _firstName;
            txtLastName.Text = _lastName;
            txtEmail.Text = _email;
            txtProfileName.Text = _profileName;

            txtFirstName.IsReadOnly = true;
            txtLastName.IsReadOnly = true;
            txtEmail.IsReadOnly = true;
            txtProfileName.IsReadOnly = true;
        }
        // Buttons
        private void btnLogo_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
        private void btnResetPassword_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnDeleteAccount_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            UserManager _userManager = new UserManager();

            if (btnEdit.Content.ToString() == "Edit")
            {
                btnEdit.Content = "Add Changes";
                txtFirstName.IsReadOnly = false;
                txtLastName.IsReadOnly = false;
            }
            else
            {
                var NewFirstName = txtFirstName.Text;
                var NewLastName = txtLastName.Text;
                //var NewAccountImage = txtFirstName.Text;
                try
                {
                    _userManager.UpdateFirstName(_email, NewFirstName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid first name." + " " + ex.Message);
                    txtFirstName.Text = _firstName;
                }
                try
                {
                    // _userManager.UpdateLastName(_email, NewLastName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid last name." + " " + ex.Message);
                    txtLastName.Text = _lastName;
                }
                try
                {
                    // _userManager.UpdateAccountImage(_email, NewAccountImage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid image." + " " + ex.Message);
                    txtFirstName.Text = _firstName;
                }
                btnEdit.Content = "Edit";
            }
        }
        private void btnFavoritesEdit_Click(object sender, RoutedEventArgs e)
        {
            if (btnFavoritesEdit.Content.ToString() == "Edit")
            {
                btnFavoritesEdit.Content = "Add Changes";
            }
            else
            {
                btnFavoritesEdit.Content = "Edit";
            }
        }
    }
}
