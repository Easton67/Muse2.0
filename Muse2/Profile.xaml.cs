using DataObjects;
using LogicLayer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Label = System.Windows.Controls.Label;

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
        private int _userID;
        List<Song> userSongs = null;

        public Profile(UserVM loggedInUser, SongManager _songManager)
        {
            _accountImage = loggedInUser.ImageFilePath;
            _firstName = loggedInUser.FirstName;
            _lastName = loggedInUser.LastName;
            _profileName = loggedInUser.ProfileName;
            _email = loggedInUser.Email;
            _userID = loggedInUser.UserID;

            InitializeComponent();
        }
        // Menu Items
        private void mnuExitApplcation_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SongManager _songManager = new SongManager();

            try
            {
                userSongs = _songManager.SelectSongsByUserID(_userID);

                // Order the songs by plays in descending order to show the most played top 5.
                List<Song> topSongs = userSongs.OrderByDescending(song => song.Plays).Take(5).ToList();

                // Create labels for only the songs with the top 5 plays
                int i = 0;
                foreach (var song in topSongs)
                {
                    // set the top song as the image for the most played song
                    imgTopSongs.Source = new BitmapImage(new System.Uri(topSongs[0].ImageFilePath));
                    string labelText = $"{i + 1}. {song.Title}";
                    Label lblTopSongs = new Label { Content = labelText };
                    lblTopSongs.Margin = new Thickness(-428, 10, 0, 0);
                    lblTopSongs.Padding = new Thickness(10, 5, 10, 5);
                    lblTopSongs.FontSize = 20;
                    lblTopSongs.Width = 260;
                    lblTopSongs.FontWeight = FontWeights.Bold;
                    labelStackPanel.Children.Add(lblTopSongs);
                    i += 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Songs not found.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var AccountImage = new BitmapImage(new System.Uri(_accountImage));
                imgStatsAccount.Source = AccountImage;
                imgAccountImage.Source = AccountImage;
                imgFavoritesAccountImage.Source = AccountImage;
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
            Close();
        }
        private void btnResetPassword_Click(object sender, RoutedEventArgs e)
        {
            var resetPassword = new ResetPassword(_email);
            resetPassword.ShowDialog();
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
            }
            else
            {
                var NewFirstName = txtFirstName.Text;
                var NewLastName = txtLastName.Text;
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

                    imgAccountImage.Source = AccountImage;
                    btnEdit.Content = "Add Changes";
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
            }
        }
    }
}
