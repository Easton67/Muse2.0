using DataObjects;
using LogicLayer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for AddPlaylist.xaml
    /// </summary>
    public partial class AddPlaylist : Window
    {
        private UserVM _loggedInUser = null;
        private string _imgFile = "";
        private string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public AddPlaylist(UserVM loggedInUser)
        {
            InitializeComponent();

            _loggedInUser = loggedInUser;
        }
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newPlaylist = new Playlist()
                {
                    Title = txtPlaylistTitle.Text,
                    ImageFilePath = System.IO.Path.GetFileName(_imgFile),
                    Description = txtPlaylistDescription.Text,
                    UserID = _loggedInUser.UserID
                };

                if(newPlaylist.ImageFilePath == "")
                {
                    newPlaylist.ImageFilePath = baseDirectory + "\\MuseConfig\\PlaylistImages\\defaultAlbumImage";
                }


                var pm = new PlaylistManager();
                bool result = pm.CreatePlaylist(newPlaylist);
                MessageBox.Show("Playlist Created!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }
        private void btnPlaylistCover_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.Title = "Open File";
                openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;)|*.jpg;*.jpeg;*.png;|All Files (*.*)|*.*";

                bool? result = openFileDialog.ShowDialog();

                if (result == true)
                {
                    _imgFile = openFileDialog.FileName;

                    string destinationFolder = baseDirectory + "\\MuseConfig\\PlaylistImages\\";

                    if (!Directory.Exists(destinationFolder))
                    {
                        Directory.CreateDirectory(destinationFolder);
                    }

                    string newImageFilePath = System.IO.Path.Combine(destinationFolder, System.IO.Path.GetFileName(_imgFile));
                    File.Copy(_imgFile, newImageFilePath, true);

                    var songImage = new BitmapImage(new System.Uri(_imgFile));

                    _imgFile = newImageFilePath;

                    imgPlaylistCover.Source = songImage;
                }
                else
                {
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
