using System;
using System.Collections.Generic;
using System.Data;
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
using DataObjects;
using LogicLayer;
using DataAccessFakes;
using System.Security.Cryptography.X509Certificates;

namespace Muse2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UserManager _userManager = null;
        UserVM loggedInUser = null;

        public MainWindow()
        {
            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _userManager = new UserManager();

            string password = pwdPassword.Password;
            string email = txtEmail.Text;

            btnLogin.IsDefault = true;


            mnuAdmin.Visibility = Visibility.Hidden;
            mnuArtist.Visibility = Visibility.Hidden;
            lblSongTitle.Content = "";
            lblSongArtist.Content = "";

            imgPause.IsEnabled = false;
            imgPause.Visibility = Visibility.Hidden;
            grdLibrary.Visibility = Visibility.Collapsed;

            List<MyDataObject> list = new List<MyDataObject>();
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            list.Add(new MyDataObject() { ImageFilePath = new Uri("file:C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\static\\defaultAlbumImage.png") });
            grdLibrary.ItemsSource = list;
        }

        private void mnuExitApplcation_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public class MyDataObject
        {
            public Uri ImageFilePath { get; set; }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (btnLogin.Content.ToString() == "Log In")
            {
                var email = txtEmail.Text;
                var password = pwdPassword.Password;

                if (!email.IsValidEmail())
                {
                    MessageBox.Show("That is not a valid email address", "Invalid Email",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    txtEmail.SelectAll();
                    txtEmail.Focus();
                    return;
                }
                if (!password.IsValidPassword())
                {
                    MessageBox.Show("That is not a valid password", "Invalid Password",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    pwdPassword.SelectAll();
                    pwdPassword.Focus();
                    return;
                }
                // try to log in the user

                try
                {
                    loggedInUser = _userManager.LoginUser(email, password);

                    if (pwdPassword.Password.ToString() == "Password")
                    {
                        txtEmail.Text = "";
                        txtEmail.Visibility = Visibility.Hidden;
                        lblEmail.Visibility = Visibility.Hidden;

                        pwdPassword.Password = "";
                        pwdPassword.Visibility = Visibility.Hidden;
                        lblPassword.Visibility = Visibility.Hidden;

                        btnLogin.Content = "Log Out";
                        btnLogin.IsDefault = false;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    // you may never throw exceptions from the presentation layer
                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Login Failed",
                         MessageBoxButton.OK, MessageBoxImage.Error);
                    pwdPassword.SelectAll();
                    txtEmail.Clear();
                    txtEmail.Focus();
                    return;
                }
            }
            else // logout      
            {
                txtEmail.Visibility = Visibility.Visible;
                lblEmail.Visibility = Visibility.Visible;

                pwdPassword.Visibility = Visibility.Visible;
                lblPassword.Visibility = Visibility.Visible;

                btnLogin.Content = "Log In";
                btnLogin.IsDefault = true;
            }
        }
    }
}
