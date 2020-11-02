﻿using System;
using System.Collections.Generic;
using System.Data;
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
using System.Xml.Serialization;

namespace my_tunes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MusicLib musicLib;

        public MainWindow()
        {
            InitializeComponent();
            musicLib = new MusicLib();

            LoadSongs(musicLib.Songs);
            LoadPlaylists();
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
            }
            else
            {
                LoadSongs(musicLib.SongsForPlaylist(selectedPlaylist));

                foreach (Control item in playlistContextMenu.Items)
                {
                    item.IsEnabled = true;
                }
            }
           
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            // Go and add a save later
            var playlistToDelete = playlistListBox.SelectedItem as string;

            musicLib.DeletePlaylist(playlistToDelete);
            ReloadPlaylists();
        }

        private void rename_Click(object sender, RoutedEventArgs e)
        {
            // Add a save later
            RenameConfirmationWindow renameConfirmationWindow = new RenameConfirmationWindow();
            var playlistToRename = playlistListBox.SelectedItem as string;

            bool? dialogResult = renameConfirmationWindow.ShowDialog();

            switch (dialogResult)
            {
                case true:
                    if (renameConfirmationWindow.NewName.Trim() != "")
                    {
                        musicLib.RenamePlaylist(playlistToRename, renameConfirmationWindow.NewName);
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

        private void openFileButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            // Add a save later
            NewPlaylistConfirmationWindow newPlaylistConfirmationWindow = new NewPlaylistConfirmationWindow();
            var playlistToRename = playlistListBox.SelectedItem as string;

            bool? dialogResult = newPlaylistConfirmationWindow.ShowDialog();

            switch (dialogResult)
            {
                case true:
                    if (newPlaylistConfirmationWindow.NewName.Trim() != "")
                    {
                        musicLib.AddPlaylist(newPlaylistConfirmationWindow.NewName);
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
    }
}
