using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Muse2
{
    /// <summary>
    /// Interaction logic for pgArtwork.xaml
    /// </summary>
    public partial class pgArtwork : Page
    {
        public string imageFilePath = "";
        private string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public pgArtwork()
        {
            InitializeComponent();
        }

        private void btnRemoveArtwork_Click(object sender, RoutedEventArgs e)
        {
            imgArtwork.Source = null;
        }

        private void btnAddArtwork_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.Title = "Open File";
                openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;)|*.jpg;*.jpeg;*.png;|All Files (*.*)|*.*";

                bool? result = openFileDialog.ShowDialog();

                if (result == true)
                {
                    imageFilePath = openFileDialog.FileName;

                    string destinationFolder = baseDirectory + "\\MuseConfig\\AlbumArt";

                    if (!Directory.Exists(destinationFolder))
                    {
                        Directory.CreateDirectory(destinationFolder);
                    }

                    // Copy the selected image file to AlbumArt
                    string newImageFilePath = System.IO.Path.Combine(destinationFolder, System.IO.Path.GetFileName(imageFilePath));
                    File.Copy(imageFilePath, newImageFilePath, true);

                    var songImage = new BitmapImage(new System.Uri(imageFilePath));

                    imgArtwork.Source = songImage;

                    // get just the file name since we handle the pathing down at the Data Access Layer
                    imageFilePath = System.IO.Path.GetFileName(newImageFilePath);
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
