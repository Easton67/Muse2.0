using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Muse2
{
    /// <summary>
    /// Interaction logic for pgAddSong.xaml
    /// </summary>
    public partial class pgAddSong : Page
    {
        private string _mp3File = "";
        private string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        Regex numericRegex = new Regex("[^0-9]+");

        public pgAddSong()
        {
            InitializeComponent();
        }

        private void btnAddMp3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

                openFileDialog.Title = "Open Mp3 File";
                openFileDialog.Filter = "MP3 Files (*.mp3)|*.mp3|All Files (*.*)|*.*";

                bool? result = openFileDialog.ShowDialog();

                if (result == true)
                {
                    _mp3File = openFileDialog.FileName;

                    string destinationFolder = baseDirectory + "\\MuseConfig\\SongFiles";

                    if (!Directory.Exists(destinationFolder))
                    {
                        Directory.CreateDirectory(destinationFolder);
                    }

                    // Copy the selected MP3 file to SongFiles
                    string newFilePath = System.IO.Path.Combine(destinationFolder, System.IO.Path.GetFileName(_mp3File));
                    File.Copy(_mp3File, newFilePath, true);

                    _mp3File = System.IO.Path.GetFileName(newFilePath);

                    txtMp3FilePath.Text = baseDirectory + "\\MuseConfig\\SongFiles\\" + _mp3File;
                }
                else
                {
                    System.Windows.MessageBox.Show("Choose an MP3 to add.");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message, "Unable to create your song. " +
                "Please make sure your file is correct.",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void txtPlays_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = numericRegex.IsMatch(e.Text);
        }

        private void txtYear_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = numericRegex.IsMatch(e.Text);
        }

        private void btnAddFolder_Click(object sender, RoutedEventArgs e)
        {
            string selectedFolder = OpenFolderDialog();

            if (!string.IsNullOrEmpty(selectedFolder))
            {
                Console.WriteLine($"Selected Folder: {selectedFolder}");
            }
            else
            {
                Console.WriteLine("No folder selected.");
            }
        }

        private void btnAddSongFromUrl_Click(object sender, RoutedEventArgs e)
        {
            txtMp3FilePath.IsReadOnly = false;
        }

        static string OpenFolderDialog()
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select your playlist folder";

                DialogResult result = folderDialog.ShowDialog();

                if (result == DialogResult.OK && CheckFolderContents(folderDialog.SelectedPath))
                {
                    return folderDialog.SelectedPath;
                }

                return null;
            }
        }

        static bool CheckFolderContents(string folderPath)
        {
            // Check if at least one TXT, MP3, and PNG file exists in subdirectories
            bool txtExists = Directory.EnumerateFiles(folderPath, "*.txt", SearchOption.AllDirectories).Any();
            bool mp3Exists = Directory.EnumerateFiles(folderPath, "*.mp3", SearchOption.AllDirectories).Any();
            bool pngExists = Directory.EnumerateFiles(folderPath, "*.png", SearchOption.AllDirectories).Any();

            return txtExists && mp3Exists && pngExists;
        }
    }
}
