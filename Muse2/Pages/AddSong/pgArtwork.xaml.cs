using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Muse2.Pages.AddSong
{
    /// <summary>
    /// Interaction logic for pgArtwork.xaml
    /// </summary>
    public partial class pgArtwork : Page
    {
        public string imageFilePath = "defaultAlbumImage";
        private string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public pgArtwork()
        {
            InitializeComponent();
            TextBoxWithImage.PreviewKeyDown += TextBoxWithImage_PreviewKeyDown;
        }

        private void btnRemoveArtwork_Click(object sender, RoutedEventArgs e)
        {
            imgArtwork.Source = new BitmapImage(new System.Uri(baseDirectory + "\\MuseConfig\\AlbumArt\\defaultAlbumImage.png"));
            imageFilePath = "defaultAlbumImage";
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxWithImage.Focus();
        }

        private void TextBoxWithImage_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Set Handled to true to prevent the caret from being displayed
            e.Handled = true;

            // You can still process the key events if needed
            // For example, to capture and display the pressed key
            if (e.Key != Key.Enter)
            {
                char pressedKey = (char)KeyInterop.VirtualKeyFromKey(e.Key);
                TextBoxWithImage.Text += pressedKey;
            }

            // Check for Ctrl+V (paste) key combination
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.V)
            {
                // Handle paste event
                HandlePaste();
            }
        }
        private void HandlePaste()
        {
            // Check if clipboard contains image data
            if (Clipboard.ContainsImage())
            {
                // Grab image from clipboard
                BitmapSource imageSource = Clipboard.GetImage();

                imgArtwork.Source = imageSource;

                SaveImageToFile(imageSource);
            }
        }

        private void SaveImageToFile(BitmapSource imageSource)
        {
            string filePath = "";
            Random random = new Random();
            int randomNumber = random.Next(1, 10000001);
            filePath = baseDirectory + "\\MuseConfig\\AlbumArt\\pastedImage" + randomNumber + ".png";
            
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(imageSource));
                encoder.Save(stream);
            }
        }
    }
}
