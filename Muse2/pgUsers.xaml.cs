using System;
using DataObjects;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LogicLayer;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.Win32;

namespace Muse2
{
    /// <summary>
    /// Interaction logic for pgUsers.xaml
    /// </summary>
    public partial class pgUsers : Page
    {
        public User _user = new User();
        private List<User> allUsers = null;
        private UserManager _userManager = new UserManager();
        private string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private string userImg = "";


        public pgUsers(User user)
        {
            InitializeComponent();

            _user = user;
        }

        private void grdUsersRepopulation()
        {
            try
            {
                allUsers = _userManager.SelectAllUsers();

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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(_user != null)
            {
                txtUserID.Text = _user.UserID.ToString();
                txtUserFirstName.Text = _user.FirstName;
                txtUserLastName.Text = _user.LastName;
                txtUserEmail.Text = _user.Email;
                txtUserProfileName.Text = _user.ProfileName;
                chkUserActive.IsChecked = _user.Active;
                txtMinutesListened.Text = _user.MinutesListened.ToString();
            }
            else
            {
                txtUserID.Text = "";
                txtUserFirstName.Text = "";
                txtUserLastName.Text = "";
                txtUserEmail.Text = "";
                txtUserProfileName.Text = "";
                chkUserActive.IsChecked = false;
                txtMinutesListened.Text = "";
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

        private void txtMinutesListened_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex numericRegex = new Regex("[^0-9]+");
            e.Handled = numericRegex.IsMatch(e.Text);
        }

        private void btnUserProfileImage_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.Title = "Open File";
                openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;)|*.jpg;*.jpeg;*.png;|All Files (*.*)|*.*";

                bool? result = openFileDialog.ShowDialog();

                if (result == true)
                {
                    userImg = openFileDialog.FileName;

                    string destinationFolder = baseDirectory + "\\MuseConfig\\ProfileImages";

                    if (!Directory.Exists(destinationFolder))
                    {
                        Directory.CreateDirectory(destinationFolder);
                    }

                    string newImageFilePath = System.IO.Path.Combine(destinationFolder, System.IO.Path.GetFileName(userImg));
                    File.Copy(userImg, newImageFilePath, true);

                    // Create the new imagebrush
                    ImageBrush imageBrush = (ImageBrush)btnUserProfileImage.Background;

                    // put the full file path instead of just the file name into the bitmap image.
                    var AccountImage = new BitmapImage(new System.Uri(newImageFilePath));
                    imageBrush.ImageSource = AccountImage;
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
