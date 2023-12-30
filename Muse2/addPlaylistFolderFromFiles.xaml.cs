using DataObjects;
using LogicLayer;
using Microsoft.VisualBasic.ApplicationServices;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Muse2
{
    /// <summary>
    /// Interaction logic for addPlaylistFolderFromFiles.xaml
    /// </summary>
    public partial class addPlaylistFolderFromFiles : Window
    { 
        private UserVM _loggedInUser = new UserVM();
        SongManager _songManager = new SongManager();
        UserVM loggedInUser = new UserVM();
        private string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string songName = "Unknown";
        string artist = "Unknown";
        string imageFilePath = "defaultAlbumImage.png";
        public addPlaylistFolderFromFiles(UserVM loggedInUser)
        {
            InitializeComponent();

            _loggedInUser = loggedInUser;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            txtFolderPath.Text = "C:\\Users\\67Eas\\Downloads\\playlist\\everything";

            if (txtFolderPath.Text.Equals(""))
            {
                System.Windows.MessageBox.Show("Enter a file path");
                btnAdd.Focus();
                return;
            }

            System.Windows.MessageBox.Show("Loading... Please wait.");
            string mp3FileFolder = txtFolderPath.Text;

            // Get all files in the MP3 folder
            string[] mp3Files = Directory.GetFiles(mp3FileFolder, "*.mp3");

            foreach (string mp3FilePath in mp3Files)
            {
                // Get file name without extension
                string mp3FileName = System.IO.Path.GetFileNameWithoutExtension(mp3FilePath);

                if(mp3FileName.Contains(" - "))
                {
                    string[] parts = mp3FileName.Split('-');

                    string artistPart = parts[0];
                    string songPart = parts[1];
                    artist = artistPart;
                    songName = songPart;
                }
                else
                {
                    artist = "Unknown";
                    songName = "Unknown";
                    imageFilePath = "defaultAlbumImage.png";
                }

                // Specify the path to the AlbumArt folder
                string albumArtFolderPath = baseDirectory + "MuseConfig\\AlbumArt\\";

                // Check if there is a matching PNG file in the AlbumArt folder
                string matchingPngFilePath = System.IO.Path.Combine(albumArtFolderPath, $"{mp3FileName}.png");

                if (File.Exists(matchingPngFilePath))
                {
                    imageFilePath = ($"{mp3FileName}.png");
                }

                try
                {
                    // Extract information from the MP3 file
                    var newSong = new Song()
                    {
                        Title = mp3FileName,
                        ImageFilePath = imageFilePath,
                        Mp3FilePath = mp3FileName + ".mp3",
                        YearReleased = 2023,
                        Lyrics = "",
                        Explicit = false,
                        Plays = 0,
                        UserID = _loggedInUser.UserID,
                        Album = "",
                        Artist = artist,
                    };

                    var sm = new SongManager();
                    bool result = sm.InsertSong(newSong);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message + "\n\n" + ex.InnerException?.Message);
                }
            }
            System.Windows.MessageBox.Show("Folder successfully uploaded!");
            this.Close();
        }
    }
}
