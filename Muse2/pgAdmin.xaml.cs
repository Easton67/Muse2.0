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
        private UserManager _userManager = new UserManager();
        private string userImg = "";
        private UserVM _loggedInUser;
        public User user = new User();
        private string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private List<User> allUsers = null;

        public pgAdmin(UserVM loggedInUser)
        {
            InitializeComponent();

            _loggedInUser = loggedInUser;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            grdUsersRepopulation();
        }
        private void grdUsersRepopulation()
        {
            try
            {
                allUsers = _userManager.SelectAllUsers();
                grdUsers.ItemsSource = allUsers;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Could not find users.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void grdUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdUsers.SelectedItems.Count != 0)
            {
                try
                {
                    var User = grdUsers.SelectedItem as User;

                    Window mainWindow = Window.GetWindow(this);

                    if (mainWindow != null)
                    {
                        if (mainWindow is MainWindow mainWin)
                        {
                            mainWin.HideLibrary();

                            Frame frmMain = mainWin.FindName("frmListOfPlaylists") as Frame;

                            if (frmMain != null && frmMain.NavigationService != null)
                            {
                                frmMain.NavigationService.Navigate(new pgUsers(User));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, $"Unable to find \"{user.ProfileName}\".",
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
    }
}
