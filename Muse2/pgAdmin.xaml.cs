using DataObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using LogicLayer;

namespace Muse2
{
    /// <summary>
    /// Interaction logic for pgAdmin.xaml
    /// </summary>
    public partial class pgAdmin : Page
    {
        private UserManager _userManager;
        private string userImg = "";
        private UserVM _loggedInUser;
        private string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public pgAdmin(UserVM loggedInUser)
        {
            InitializeComponent();

            _loggedInUser = loggedInUser;
        }

        private void grdUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // set the edit button to be clickable
            btnUserEdit.IsEnabled = true;
            if (grdUsers.SelectedItems.Count != 0)
            {
                var User = grdUsers.SelectedItem as User;

                try
                {
                    ImageBrush imageBrush = (ImageBrush)btnUserProfileImage.Background;
                    BitmapImage profileImage = new BitmapImage(new Uri(User.ImageFilePath));
                    imageBrush.ImageSource = profileImage;

                    grdUsersRepopulation();

                    txtUserID.Text = User.UserID.ToString();
                    txtUserFirstName.Text = User.FirstName;
                    txtUserLastName.Text = User.LastName;
                    txtUserEmail.Text = User.Email;
                    txtUserProfileName.Text = User.ProfileName;
                    chkUserActive.IsChecked = User.Active;
                    txtMinutesListened.Text = User.MinutesListened.ToString();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, $"Unable to find the account image for \"{User.ProfileName}\". Please fix this.",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private void grdUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdUsers.SelectedItem != null)
            {
                //int selectedRowIndex = grdLibrary.Items.IndexOf(grdUsers.SelectedItem);

                //userNumber = selectedRowIndex;
            }
        }

        private void btnUserProfileImage_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //try
            //{
            //    OpenFileDialog openFileDialog = new OpenFileDialog();

            //    openFileDialog.Title = "Open File";
            //    openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;)|*.jpg;*.jpeg;*.png;|All Files (*.*)|*.*";

            //    bool? result = openFileDialog.ShowDialog();

            //    if (result == true)
            //    {
            //        userImg = openFileDialog.FileName;

            //        string destinationFolder = baseDirectory + "\\MuseConfig\\ProfileImages";

            //        if (!Directory.Exists(destinationFolder))
            //        {
            //            Directory.CreateDirectory(destinationFolder);
            //        }

            //        string newImageFilePath = System.IO.Path.Combine(destinationFolder, System.IO.Path.GetFileName(userImg));
            //        File.Copy(userImg, newImageFilePath, true);

            //        // Create the new imagebrush
            //        ImageBrush imageBrush = (ImageBrush)btnUserProfileImage.Background;

            //        // put the full file path instead of just the file name into the bitmap image.
            //        var AccountImage = new BitmapImage(new System.Uri(newImageFilePath));
            //        imageBrush.ImageSource = AccountImage;
            //    }
            //    else
            //    {
            //        // user closes the file explorer before picking a photo
            //        System.Windows.MessageBox.Show("Choose a photo to update your current account photo.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    System.Windows.MessageBox.Show("Invalid image." + " " + ex.Message);
            //}
        }

        private void btnUserSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            // re-enable the user edit button
            btnUserEdit.IsEnabled = true;
            string NewFirstName = txtUserFirstName.Text;
            string NewLastName = txtUserLastName.Text;

            if (!NewFirstName.IsValidFirstName())
            {
                System.Windows.MessageBox.Show("That is not a valid first name", "Invalid first name",
                MessageBoxButton.OK, MessageBoxImage.Error);
                txtUserFirstName.Focus();
                return;
            }
            if (!NewLastName.IsValidLastName())
            {
                System.Windows.MessageBox.Show("That is not a valid last name", "Invalid last name",
                MessageBoxButton.OK, MessageBoxImage.Error);
                txtUserFirstName.Focus();
                return;
            }

            string userEmail = txtUserEmail.Text;

            UserVM oldUser = _userManager.GetUserVMByEmail(userEmail);
            oldUser.ImageFilePath = System.IO.Path.GetFileName(oldUser.ImageFilePath);

            var newUser = new UserVM()
            {
                UserID = int.Parse(txtUserID.Text),
                ProfileName = txtUserProfileName.Text,
                Email = txtUserEmail.Text,
                FirstName = NewFirstName,
                LastName = NewLastName,
                ImageFilePath = System.IO.Path.GetFileName(userImg),
                Active = (bool)chkUserActive.IsChecked,
                MinutesListened = int.Parse(txtMinutesListened.Text),
                Roles = oldUser.Roles
            };

            if (userImg == "")
            {
                newUser.ImageFilePath = oldUser.ImageFilePath;
            }

            try
            {
                _userManager.UpdateUser(oldUser, newUser);
                System.Windows.MessageBox.Show("Account details have been updated", "Success!",
                MessageBoxButton.OK);
                grdUsersRepopulation();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Unable to update your profile." + " " + ex.Message);
            }
        }

        private void grdUsersRepopulation()
        {
            try
            {
                List<User> allUsers = _userManager.SelectAllUsers();
                // lblDataGridHeader.Content = "All Users";
                grdUsers.Visibility = Visibility.Visible;
                grdUsers.ItemsSource = allUsers;

                txtUserFirstName.IsEnabled = false;
                txtUserLastName.IsEnabled = false;
                chkUserActive.IsEnabled = false;
                txtMinutesListened.IsEnabled = false;
                btnUserSaveChanges.IsEnabled = false;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not found users.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnUserEdit_Click(object sender, RoutedEventArgs e)
        {
            btnUserEdit.IsEnabled = false;
            btnUserSaveChanges.IsEnabled = true;
            btnUserProfileImage.IsEnabled = true;
            txtUserFirstName.IsEnabled = true;
            txtUserLastName.IsEnabled = true;
            chkUserActive.IsEnabled = true;
            txtMinutesListened.IsEnabled = true;
        }

        private void txtMinutesListened_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex numericRegex = new Regex("[^0-9]+");
            e.Handled = numericRegex.IsMatch(e.Text);
        }
    }
}
