using DataObjects;
using LogicLayer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private UserVM _loggedInUser = null;
        private string _imgFile = "";
        List<Song> userSongs = null;
        List<Review> reviews = null;
        private ReviewManager _reviewManager = new ReviewManager(); 

        public Profile(UserVM loggedInUser, SongManager _songManager)
        {
            InitializeComponent();

            _loggedInUser = loggedInUser;
        }
        // Menu Items
        private void mnuResetPassword_Click(object sender, RoutedEventArgs e)
        {
            var resetPassword = new ResetPassword(_loggedInUser.Email);
            resetPassword.ShowDialog();
        }
        private void mnuExitApplcation_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SongManager _songManager = new SongManager();
            try
            {
                reviews = _reviewManager.SelectReviewsByUserID(_loggedInUser.UserID);
                icReviews.ItemsSource = reviews;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not find your reviews.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                userSongs = _songManager.SelectSongsByUserID(_loggedInUser.UserID);

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
                    lblTopSongs.Margin = new Thickness(0, 10, 0, 0);
                    lblTopSongs.Padding = new Thickness(10, 5, 10, 5);
                    lblTopSongs.FontSize = 20;
                    lblTopSongs.Width = 260;
                    lblTopSongs.FontWeight = FontWeights.Bold;
                    labelStackPanel.Children.Add(lblTopSongs);
                    i += 1;
                }

                try
                {
                    lblMinutesTotalListened.Content = _loggedInUser.MinutesListened;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Minutes Listened could not be found.",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
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
                var AccountImage = new BitmapImage(new System.Uri(_loggedInUser.ImageFilePath));
                imgStatsAccount.Source = AccountImage;
                imgAccountImage.Source = AccountImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to find your profile photo. ", ex.Message);
            }
            txtFirstName.Text = _loggedInUser.FirstName;
            txtLastName.Text = _loggedInUser.LastName;
            txtEmail.Text = _loggedInUser.Email;
            txtProfileName.Text = _loggedInUser.ProfileName;

            txtFirstName.IsReadOnly = true;
            txtLastName.IsReadOnly = true;
            txtEmail.IsReadOnly = true;
            txtProfileName.IsReadOnly = true;
        }
        #region Profile Buttons
        private void btnLogo_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void btnResetPassword_Click(object sender, RoutedEventArgs e)
        {
            var resetPassword = new ResetPassword(_loggedInUser.Email);
            resetPassword.ShowDialog();
        }
        private void btnDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Are you sure you want to deactivate your account?",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        UserManager _userManager = new UserManager();

                        bool newActive = false;

                        // Passsing in false as the new value for newActive
                        _userManager.UpdateActiveUser(_loggedInUser.UserID, _loggedInUser.Active, newActive);
                        MessageBox.Show("Your account has been deactivated. \n\n Please contact the admin to have your account reactivated. \n\n Goodbye, and thank you for using Muse :)");
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not deactivate your account. Please try again",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, $"Unable to deactivate your account. ",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (btnEdit.Content.ToString() == "Edit")
            {
                btnAccontImage.IsEnabled = true;
                btnEdit.Content = "Add Changes";
                txtFirstName.IsReadOnly = false;
                txtLastName.IsReadOnly = false;
                txtProfileName.IsReadOnly = false;
                txtFirstName.IsEnabled = true;
                txtLastName.IsEnabled = true;
            }
            else
            {
                string NewFirstName = txtFirstName.Text;
                string NewLastName = txtLastName.Text;

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

                UserManager _userManager = new UserManager();

                var oldUser = this._loggedInUser;

                var newUser = new UserVM()
                {
                    UserID = _loggedInUser.UserID,
                    ProfileName = _loggedInUser.ProfileName,
                    Email = _loggedInUser.Email,
                    FirstName = NewFirstName,
                    LastName = NewLastName,
                    ImageFilePath = _imgFile,
                    Active = true,
                    MinutesListened = 0,
                    Roles = _loggedInUser.Roles
                };

                try
                {
                    _userManager.UpdateUser(oldUser, newUser);
                    btnEdit.Content = "Edit";
                    MessageBox.Show("Your account details have been updated.\n\nYour account will be updated the next time you log in.", "Success!",
                    MessageBoxButton.OK);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to update your profile." + " " + ex.Message);
                    txtFirstName.Text = "";
                    txtLastName.Text = "";
                    txtFirstName.Focus();
                }
            }
        }
        #endregion
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
                    _imgFile = openFileDialog.FileName;
                    var AccountImage = new BitmapImage(new System.Uri(_imgFile));

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
