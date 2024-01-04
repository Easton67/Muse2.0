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
        string playlistFolderPath;
        // Variables to store information
        string songTitle = "Unknown";
        string imageFilePath = "defaultAlbumImage.png";
        string artist = "Unknown";
        string lyrics = "No lyrics provided.";
        string mp3FileName = "";
        private string songfilesLocation = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\SongFiles";
        private string imageFilesLocation = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt";


        int numberOfSongsAdded = 0;

        public addPlaylistFolderFromFiles(UserVM loggedInUser)
        {
            InitializeComponent();

            _loggedInUser = loggedInUser;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            playlistFolderPath = txtFolderPath.Text;

            if (txtFolderPath.Text.Equals(""))
            {
                System.Windows.MessageBox.Show("Enter a file path");
                btnAdd.Focus();
                return;
            }
            System.Windows.MessageBox.Show("Loading... Please wait.");

            string[] subfolders = Directory.GetDirectories(playlistFolderPath);
            int numberOfSongsAdded = 0;

            foreach (string subfolder in subfolders)
            {
                try
                {
                    string[] songFolder = Directory.GetFiles(subfolder);

                    string artist = "Unknown";
                    string songTitle = "Unknown";
                    string imageFilePath = "defaultAlbumImage.png";
                    string lyrics = "No Lyrics Provided.";

                    foreach (string file in songFolder)
                    {
                        string extension = System.IO.Path.GetExtension(file);

                        switch (extension.ToLower())
                        {
                            case ".mp3":

                                if (file.Contains(" - "))
                                {
                                    try
                                    {
                                        string newmp3FileLocation = System.IO.Path.Combine(songfilesLocation, System.IO.Path.GetFileName(file));
                                        File.Copy(file, newmp3FileLocation, true);
                                        var unsplitSongTitle = System.IO.Path.GetFileNameWithoutExtension(file);
                                        mp3FileName = System.IO.Path.GetFileName(file);

                                        string[] parts = unsplitSongTitle.Split('-');
                                        string artistPart = parts[0].Trim();
                                        string songPart = parts[1].Trim();
                                        artist = artistPart;
                                        songTitle = songPart;
                                    }                                    
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Error copying file '{file}': {ex.Message}");
                                    }
                                }
                                else
                                {
                                    string newmp3FileLocation = System.IO.Path.Combine(songfilesLocation, System.IO.Path.GetFileName(file));
                                    File.Copy(file, newmp3FileLocation, true);
                                    var unsplitSongTitle = System.IO.Path.GetFileNameWithoutExtension(file);
                                    mp3FileName = unsplitSongTitle + ".mp3";

                                    artist = "Unknown";
                                    songTitle = unsplitSongTitle;
                                }
                                break;
                            case ".png":

                                try
                                {
                                    string newPngFileLocation = System.IO.Path.Combine(imageFilesLocation, System.IO.Path.GetFileName(file));
                                    File.Copy(file, newPngFileLocation, true);
                                    imageFilePath = System.IO.Path.GetFileName(file);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error copying file '{file}': {ex.Message}");
                                }
                                break;
                            case ".txt":

                                lyrics = File.ReadAllText(file);
                                break;
                        }
                    }
                    try
                    {
                        var newSong = new Song()
                        {
                            Title = songTitle,
                            ImageFilePath = imageFilePath,
                            Mp3FilePath = mp3FileName,
                            YearReleased = 2023,
                            Lyrics = lyrics,
                            Explicit = false,
                            Plays = 0,
                            UserID = _loggedInUser.UserID,
                            Album = "",
                            Artist = artist,
                        };

                        var sm = new SongManager();
                        bool result = sm.InsertSong(newSong);
                        numberOfSongsAdded++;
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.Message + "\n\n" + ex.InnerException?.Message);
                    }
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Error");
                }
            }
            System.Windows.MessageBox.Show(numberOfSongsAdded.ToString() + " songs were added to your library.");
            this.Close();
        }
    }
}
