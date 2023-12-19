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
    /// Interaction logic for ImageSavingPaste.xaml
    /// </summary>
    public partial class ImageSavingPaste : Window
    {
        public ImageSavingPaste()
        {
            InitializeComponent();
            TextBoxWithImage.PreviewKeyDown += TextBoxWithImage_PreviewKeyDown;
        }

        private void TextBoxWithImage_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
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
                // Retrieve image data from clipboard
                BitmapSource imageSource = Clipboard.GetImage();

                // Display the image in the Image control
                DisplayImage.Source = imageSource;

                // Optionally, save the image to a file
                SaveImageToFile(imageSource);
            }
        }

        private void SaveImageToFile(BitmapSource imageSource)
        {
            // Allow the user to choose a file location
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Image Files (*.png; *.jpg; *.jpeg; *.gif; *.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All Files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                // Save the image to the chosen file location
                string filePath = saveFileDialog.FileName;
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    BitmapEncoder encoder = new PngBitmapEncoder(); // Choose the appropriate encoder based on your requirements
                    encoder.Frames.Add(BitmapFrame.Create(imageSource));
                    encoder.Save(stream);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the image source from the DisplayImage control
            BitmapSource imageSource = DisplayImage.Source as BitmapSource;

            if (imageSource != null)
            {
                // Save the image to a file
                SaveImageToFile(imageSource);
            }
            else
            {
                MessageBox.Show("No image to save.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
