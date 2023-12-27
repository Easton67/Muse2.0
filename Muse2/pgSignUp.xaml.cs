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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Muse2
{
    /// <summary>
    /// Interaction logic for pgSignUp.xaml
    /// </summary>
    public partial class pgSignUp : Page
    {
        bool btnShowPasswordBottomIsClicked;
        bool btnShowPasswordTopIsClicked;
        string password;
        string email;
        string profileName;
        public pgSignUp()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txtShownPasswordTop.Visibility = Visibility.Hidden;
            txtShownPasswordBottom.Visibility = Visibility.Hidden;
        }

        private void btnShowPasswordTop_Click(object sender, RoutedEventArgs e)
        {
            if (btnShowPasswordBottomIsClicked == false)
            {
                txtShownPasswordTop.Visibility = Visibility.Visible;
                txtShownPasswordTop.Text = pwdPassword.Password;
                btnShowPasswordBottomIsClicked = true;
                return;
            }
            else
            {
                pwdPassword.Visibility = Visibility.Visible;
                txtShownPasswordTop.Visibility = Visibility.Hidden;
                btnShowPasswordBottomIsClicked = false;
            }
        }

        private void btnShowPasswordBottom_Click(object sender, RoutedEventArgs e)
        {
            if (btnShowPasswordTopIsClicked == false)
            {
                txtShownPasswordBottom.Visibility = Visibility.Visible;
                txtShownPasswordBottom.Text = pwdConfirmPassword.Password;
                btnShowPasswordTopIsClicked = true;
            }
            else
            {
                pwdConfirmPassword.Visibility = Visibility.Visible;
                txtShownPasswordBottom.Visibility = Visibility.Hidden;
                btnShowPasswordTopIsClicked = false;
            }
        }
        #region Validation
        private void SetProfileName()
        {
            if (!profileName.IsValidProfileName())
            {
                MessageBox.Show("That is not a valid profile name", "Invalid Profile Name",
                MessageBoxButton.OK, MessageBoxImage.Error);
                txtProfileName.Clear();
                txtProfileName.Focus();
                return;
            }
            else
            {
                profileName = txtProfileName.Text;
            }
        }
        private void SetEmail()
        {
            if (!email.IsValidEmail())
            {
                MessageBox.Show("That is not a valid email address", "Invalid Email",
                MessageBoxButton.OK, MessageBoxImage.Error);
                txtEmail.SelectAll();
                txtEmail.Focus();
                return;
            }
            else
            {
                email = txtEmail.Text;
            }
        }
        private void SetPassword()
        {
            if (pwdPassword.Password == pwdConfirmPassword.Password && pwdPassword.Password.IsValidPassword())
            {
                password = pwdConfirmPassword.Password;
            }
            else
            {
                MessageBox.Show("That is not a valid password", "Invalid Password",
                MessageBoxButton.OK, MessageBoxImage.Error);
                pwdPassword.SelectAll();
                pwdPassword.Focus();
                return;
            }
        }
        #endregion
        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            // run check if not all fields are put in
            if(txtProfileName.Text == "" || txtEmail.Text == "" || pwdPassword.Password == "" || pwdConfirmPassword.Password == "")
            {
                MessageBox.Show("Please fill out all fields to continue.");
                return;
            }

            // seperated into different methods for validation checks
            SetPassword();
            SetEmail();
            SetProfileName();
            var newUser = new User()
            {
                ProfileName = profileName,
                Email = email,
                ImageFilePath = "C:\\Users\\67Eas\\Downloads\\eye.png"
            };
            try
            {
                UserManager um = new UserManager();
                um.InsertUser(newUser, password);
                MessageBox.Show("Account Created!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "User could not be created. Please try again.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
