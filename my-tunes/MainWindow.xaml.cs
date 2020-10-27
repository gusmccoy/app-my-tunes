using System;
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
            }
            else
            {
                LoadSongs(musicLib.SongsForPlaylist(selectedPlaylist));
            }
           
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            //Go and add a save later
            //make it to where you can't do it on all music
            var playlistToDelete = playlistListBox.SelectedItem as string;

            musicLib.DeletePlaylist(playlistToDelete);
            ReloadPlaylists();
        }

        private void rename_Click(object sender, RoutedEventArgs e)
        {
            // add confirmation box

            var playlistToRename = playlistListBox.SelectedItem as string;
            //musicLib.RenamePlaylist(playlistToRename, NEWNAME);
        }
    }
}
