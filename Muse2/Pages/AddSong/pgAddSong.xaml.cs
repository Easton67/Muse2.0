using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace Muse2.Pages.AddSong
{
    /// <summary>
    /// Interaction logic for pgAddSong.xaml
    /// </summary>
    public partial class pgAddSong : Page
    {
        private UserVM _loggedInUser = null;
        private List<Song> userSongs = null;
        private List<DataObjects.Playlist> userPlaylists = null;
        private Playlist _playlist = null;
        private Song _song = null;
        public string mp3FileName;
        private string playlistFolderPath;
        public string songTitle;
        public string artistName;
        public string albumName;
        public int yearReleased;
        public bool isExplicit;
        public string genre;
        public int plays = 0;
        private string songfilesLocation = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\SongFiles";
        private string imageFilesLocation = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt";
        private string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        Regex numericRegex = new Regex("[^0-9]+");

        public pgAddSong(UserVM loggedInUser)
        {
            InitializeComponent();

            _loggedInUser = loggedInUser;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            lblPlays.Visibility = Visibility.Collapsed;
            txtPlays.Visibility = Visibility.Collapsed;
            btnConfirm.Visibility = Visibility.Hidden;
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
                    mp3FileName = openFileDialog.FileName;

                    string destinationFolder = baseDirectory + "\\MuseConfig\\SongFiles";

                    if (!Directory.Exists(destinationFolder))
                    {
                        Directory.CreateDirectory(destinationFolder);
                    }

                    // Copy the selected MP3 file to SongFiles
                    string newFilePath = System.IO.Path.Combine(destinationFolder, System.IO.Path.GetFileName(mp3FileName));
                    File.Copy(mp3FileName, newFilePath, true);

                    mp3FileName = System.IO.Path.GetFileName(newFilePath);

                    txtMp3FilePath.Text = mp3FileName;
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
            // Change which buttons are being shown
            btnConfirm.Visibility = Visibility.Visible;
            btnAddFolder.Visibility = Visibility.Hidden;

            // Hide all other controls
            lblTitle.Content = "Add to Playlist";

            lblYear.Visibility = Visibility.Hidden;
            txtYear.Visibility = Visibility.Hidden;

            lblAlbum.Visibility = Visibility.Hidden;
            txtAlbum.Visibility = Visibility.Hidden;

            lblArtist.Visibility = Visibility.Hidden;
            txtArtist.Visibility = Visibility.Hidden;

            lblGenre.Visibility = Visibility.Hidden;
            cboGenre.Visibility = Visibility.Hidden;

            lblExplicit.Visibility = Visibility.Hidden;
            chkExplicit.Visibility = Visibility.Hidden;

            lblPlays.Visibility = Visibility.Hidden;
            txtPlays.Visibility = Visibility.Hidden;

            btnConfirm.Visibility = Visibility.Visible;

            string selectedFolder = OpenFolderDialog();

            if (!string.IsNullOrEmpty(selectedFolder))
            {
                lblMp3File.Content = "Playlist Folder";
                txtMp3FilePath.Text = Path.GetFileName(selectedFolder);
            }
        }
        private void btnAddSongFromUrl_Click(object sender, RoutedEventArgs e)
        {
            txtMp3FilePath.IsReadOnly = false;
            txtMp3FilePath.IsEnabled = true;
            txtMp3FilePath.Focus();
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
        #region Create Variables
        private void txtTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            songTitle = txtTitle.Text;
        }

        private void txtArtist_LostFocus(object sender, RoutedEventArgs e)
        {
            artistName = txtArtist.Text;
        }

        private void txtAlbum_LostFocus(object sender, RoutedEventArgs e)
        {
            albumName = txtAlbum.Text;
        }

        private void txtYear_LostFocus(object sender, RoutedEventArgs e)
        {
            if(txtYear.Text != "")
            {
                yearReleased = int.Parse(txtYear.Text);
            }
        }

        private void chkExplicit_LostFocus(object sender, RoutedEventArgs e)
        {
            isExplicit = (bool)chkExplicit.IsChecked;
        }

        private void cboGenre_LostFocus(object sender, RoutedEventArgs e)
        {
            genre = cboGenre.Text;
        }

        private void txtPlays_LostFocus(object sender, RoutedEventArgs e)
        {
            plays = int.Parse(txtPlays.Text);
        }

        private void txtMp3FilePath_LostFocus(object sender, RoutedEventArgs e)
        {
            mp3FileName =  txtMp3FilePath.Text;
        }

        #endregion
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            btnConfirm.Visibility = Visibility.Hidden;
            btnAddFolder.Visibility = Visibility.Visible;
            playlistFolderPath = txtMp3FilePath.Text;

            if (txtMp3FilePath.Text.Equals(""))
            {
                System.Windows.MessageBox.Show("Please enter a valid file path", "Invalid Path");
                btnAddMp3.Focus();
                return;
            }
            System.Windows.MessageBox.Show("Loading... Please wait.");

            string[] subfolders = Directory.GetDirectories(playlistFolderPath);
            int numberOfSongsAdded = 0;

            // Create playlist if they choose to add it to one
            try
            {
                var newPlaylist = new DataObjects.Playlist()
                {
                    Title = txtTitle.Text,
                    ImageFilePath = "defaultAlbumImage.png",
                    Description = "",
                    UserID = _loggedInUser.UserID
                };
                // Make playlist and then find that playlist within all of them to get the playlistID

                var pm = new PlaylistManager();
                pm.CreatePlaylist(newPlaylist);
                userPlaylists = pm.SelectPlaylistByUserID(_loggedInUser.UserID);
                _playlist = userPlaylists.FirstOrDefault(p => p.Title == txtTitle.Text);
            }
            catch (Exception ex) 
            {
                System.Windows.MessageBox.Show(ex.Message, "Unable to add songs to this playlist.");
            }

            // loop through each folder, add the song to the library, and add it to the playlist if the user chooses

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
                                        System.Windows.MessageBox.Show($"Error copying file '{file}': {ex.Message}");
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
                                    System.Windows.MessageBox.Show($"Error copying file '{file}': {ex.Message}");
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
                        var pm = new PlaylistManager();
                        bool result = sm.InsertSong(newSong);
                        numberOfSongsAdded++;

                        userSongs = sm.SelectSongsByUserID(_loggedInUser.UserID);
                        _song = userSongs.FirstOrDefault(s => s.Title == newSong.Title);
                        pm.InsertSongIntoPlaylist(_song.SongID, _playlist.PlaylistID);
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
            if(txtTitle.Equals(""))
            {
                System.Windows.MessageBox.Show(numberOfSongsAdded.ToString() + " songs were added to your library.");
            }
            System.Windows.MessageBox.Show(numberOfSongsAdded.ToString() + " songs were added to your playlist " + txtTitle.Text + ".");
        }
    }
}
