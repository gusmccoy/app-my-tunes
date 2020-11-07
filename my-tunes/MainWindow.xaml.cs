using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
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
using System.Xml.Serialization;

namespace my_tunes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MusicLib musicLib;
        private Point startPoint;
        private string currentPlaylist = "All Music";

        public MainWindow()
        {
            InitializeComponent();
            musicLib = new MusicLib();

            LoadSongs(musicLib.Songs);
            LoadPlaylists();

            songsDataGrid.ContextMenu = new ContextMenu();
            MenuItem removeSong = new MenuItem();
            removeSong.Header = "Remove";
            removeSong.Click += remove_Click;
            songsDataGrid.ContextMenu.Items.Add(removeSong);
        }

        private void LoadPlaylists()
        {
            this.playlistListBox.Items.Clear();

            List<string> playlists = new List<string>();
            playlists.Add("All Music");
            playlists.AddRange(musicLib.Playlists);

            this.playlistListBox.ItemsSource = playlists;
        }

        private void ReloadPlaylists()
        {
            List<string> playlists = new List<string>();
            playlists.Add("All Music");
            playlists.AddRange(musicLib.Playlists);

            this.playlistListBox.ItemsSource = playlists;
        }

        private void LoadSongs(DataTable table)
        {
            songsDataGrid.ItemsSource = table.DefaultView;
        }

        private void playlistListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedPlaylist = playlistListBox.SelectedItem as string;

            if(selectedPlaylist == "All Music")
            {
                LoadSongs(musicLib.Songs);

                foreach (Control item in playlistContextMenu.Items)
                {
                    item.IsEnabled = false;
                }

                songsDataGrid.ContextMenu = new ContextMenu();
                MenuItem removeSong = new MenuItem();
                removeSong.Header = "Remove";
                removeSong.Click += remove_Click;
                songsDataGrid.ContextMenu.Items.Add(removeSong);
            }
            else
            {
                LoadSongs(musicLib.SongsForPlaylist(selectedPlaylist));

                foreach (Control item in playlistContextMenu.Items)
                {
                    item.IsEnabled = true;
                }

                songsDataGrid.ContextMenu = new ContextMenu();
                MenuItem removeSongFromPlaylist = new MenuItem();
                removeSongFromPlaylist.Header = "Remove Song From Playlist";
                removeSongFromPlaylist.Click += removeFromPlaylist_Click;
                songsDataGrid.ContextMenu.Items.Add(removeSongFromPlaylist);
            }
           currentPlaylist = selectedPlaylist;
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            var playlistToDelete = playlistListBox.SelectedItem as string;

            musicLib.DeletePlaylist(playlistToDelete);
            musicLib.Save();
            ReloadPlaylists();
        }

        private void rename_Click(object sender, RoutedEventArgs e)
        {
            RenameConfirmationWindow renameConfirmationWindow = new RenameConfirmationWindow();
            var playlistToRename = playlistListBox.SelectedItem as string;

            bool? dialogResult = renameConfirmationWindow.ShowDialog();

            switch (dialogResult)
            {
                case true:
                    if (renameConfirmationWindow.NewName.Trim() != "")
                    {
                        musicLib.RenamePlaylist(playlistToRename, renameConfirmationWindow.NewName);
                        musicLib.Save();
                        ReloadPlaylists();
                    }
                    else
                    {
                        MessageBox.Show("Please enter a new playlist name");
                    }

                    break;

                case false:
                    break;
            }
        }

        private async void openFileButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "All Supported Audio | *.mp3; *.m4a; *.wav; *.wma";
            bool? dialogResult = fileDialog.ShowDialog();

            switch (dialogResult)
            {
                case true:
                    string path = fileDialog.FileName;
                    await musicLib.AddSong(path);
                    musicLib.Save();
                    ReloadPlaylists();
                    break;

                case false:
                    break;
            }
        }

        private void addPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            NewPlaylistConfirmationWindow newPlaylistConfirmationWindow = new NewPlaylistConfirmationWindow();
            var playlistToRename = playlistListBox.SelectedItem as string;

            bool? dialogResult = newPlaylistConfirmationWindow.ShowDialog();

            switch (dialogResult)
            {
                case true:
                    if (newPlaylistConfirmationWindow.NewName.Trim() != "")
                    {
                        musicLib.AddPlaylist(newPlaylistConfirmationWindow.NewName);
                        musicLib.Save();
                        ReloadPlaylists();
                    }
                    else
                    {
                        MessageBox.Show("Please enter a playlist name");
                    }

                    break;

                case false:
                    break;
            }
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();

            aboutWindow.ShowDialog();
        }

        private void songsDataGrid_MouseMove(object sender, MouseEventArgs e)
        {
            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            // Start the drag-drop if mouse has moved far enough
            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Initiate dragging the text from the Row
                DataRowView song = songsDataGrid.SelectedItem as DataRowView;
                DragDrop.DoDragDrop(songsDataGrid, song.Row.ItemArray[0].ToString(), DragDropEffects.Copy);
            }

        }

        private void songsDataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Store the mouse position
            startPoint = e.GetPosition(null);
        }

        private void playlistListBox_Drop(object sender, DragEventArgs e)
        {
            Label playlist = sender as Label;

            if (playlist != null && e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string data = (string)e.Data.GetData(DataFormats.StringFormat);

                int songId = Int32.Parse(data);

                musicLib.AddSongToPlaylist(songId, playlist.Content.ToString());
                musicLib.Save();
            }

        }

        private void playlistListBox_DragOver(object sender, DragEventArgs e)
        {
            // By default, don't allow dropping
            e.Effects = DragDropEffects.None;

            // If the DataObject contains string data, extract it
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string dataString = (string)e.Data.GetData(DataFormats.StringFormat);
                int value;
                // If string can be converted into number, allow copy effect
                if (int.TryParse(dataString, out value))
                {
                    e.Effects = DragDropEffects.Copy;
                }
            }

        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            //axWindowsMediaPlayer1.URL = @"c:\mediafile.wmv";
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            //axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this song?", "Delete Song?", 
                                        MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                DataRowView song = songsDataGrid.SelectedItem as DataRowView;
                int songId = Int32.Parse(song.Row.ItemArray[0].ToString());
                musicLib.DeleteSong(songId);
                musicLib.Save();
                LoadSongs(musicLib.Songs);
            }

        }

        private void removeFromPlaylist_Click(object sender, RoutedEventArgs e)
        {
            DataRowView song = songsDataGrid.SelectedItem as DataRowView;
            var selectedPlaylist = playlistListBox.SelectedItem as string;
            int songId = Int32.Parse(song.Row.ItemArray[0].ToString());
            int position = Int32.Parse(song.Row.ItemArray[1].ToString());

            musicLib.RemoveSongFromPlaylist(position, songId, selectedPlaylist);
            musicLib.Save();
            LoadSongs(musicLib.SongsForPlaylist(selectedPlaylist));
        }

        // CREDIT TO: https://stackoverflow.com/questions/31809201/how-to-use-textbox-to-search-data-in-data-grid-view
        private void searchBarTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(musicLib != null)
            {
                DataTable table = new DataTable();
                if (currentPlaylist != "All Music")
                {
                    table = musicLib.SongsForPlaylist(currentPlaylist);
                }
                else
                {
                    table = musicLib.Songs;
                }
                LoadSongs(table);
                table.DefaultView.RowFilter = string.Format("title LIKE '%{0}%' OR artist LIKE '%{0}%' OR album LIKE '%{0}%'",
                    searchBarTextBox.Text);
            }
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // CREDIT TO: https://stackoverflow.com/questions/502199/how-to-open-a-web-page-from-my-application
            var processStartInfo = new ProcessStartInfo
            {
                FileName = e.Uri.AbsoluteUri,
                UseShellExecute = true
            };

            Process.Start(processStartInfo);
        }
    }
}
