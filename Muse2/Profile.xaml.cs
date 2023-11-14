using DataObjects;
using LogicLayer;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
                txtProfileName.IsReadOnly = false;
                txtFirstName.IsEnabled = true;
                txtLastName.IsEnabled = true;
                txtProfileName.IsEnabled = true;
            }
            else
            {
                var NewFirstName = txtFirstName.Text;
                var NewLastName = txtLastName.Text;
                var NewProfileName = txtProfileName.Text;
                bool isBadUpdate = false;

                if (!NewFirstName.IsValidFirstName())
                {
                    MessageBox.Show("That is not a valid first name", "Invalid first name",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    txtFirstName.Focus();
                    return;
                }
                if (!NewLastName.IsValidLastName())
                {
                    MessageBox.Show("That is not a valid last name", "Invalid last name",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    txtFirstName.Focus();
                    return;
                }
                if (!NewProfileName.IsValidProfileName())
                {
                    MessageBox.Show("That is not a valid profile name", "Invalid profile name",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    txtFirstName.Focus();
                    return;
                }

                try
                {
                    _userManager.UpdateFirstName(_email, NewFirstName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid first name." + " " + ex.Message);
                    txtFirstName.Text = _firstName;
                    isBadUpdate = true;
                }
                try
                {
                    _userManager.UpdateLastName(_email, NewLastName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid last name." + " " + ex.Message);
                    txtLastName.Text = _lastName;
                    isBadUpdate = true;
                }
                try
                {
                     _userManager.UpdateProfileName(_email, NewProfileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid profile name." + " " + ex.Message);
                    txtFirstName.Text = _firstName;
                    isBadUpdate = true;
                }
                finally
                {
                    if (!isBadUpdate)
                    {
                        btnEdit.Content = "Edit";
                        MessageBox.Show("Your account details have been updated", "Success!",
                        MessageBoxButton.OK);
                    }
                }
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

        private void btnAccontImage_Click(object sender, RoutedEventArgs e)
        {
            UserManager _userManager = new UserManager();
            var NewAccountImage = "";

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.Title = "Open File";
                openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;)|*.jpg;*.jpeg;*.png;|All Files (*.*)|*.*";

                bool? result = openFileDialog.ShowDialog();

                if (result == true)
                {
                    _accountImage = openFileDialog.FileName;
                    var AccountImage = new BitmapImage(new System.Uri(_accountImage));
                    _userManager.UpdateAccountImage(_email, _accountImage);

                    imgAccontImage.Source = AccountImage;
                }
                else
                {
                    // user closes the file explorer before picking a photo
                    MessageBox.Show("Choose a photo to update your current account photo.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid image." + " " + ex.Message);
                txtFirstName.Text = _firstName;
            }
        }
    }
}
