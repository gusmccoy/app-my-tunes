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
        private DataSet musicDataSet;
        private MusicLib musicLib;

        public MainWindow()
        {
            InitializeComponent();
            musicLib = new MusicLib();

            LoadSongs();
            LoadPlaylists();
        }

        private void LoadPlaylists()
        {            
            this.playlistListBox.Items.Clear();
            this.playlistListBox.ItemsSource = musicLib.Playlists;
        }

        private void LoadSongs()
        {
            songsDataGrid.ItemsSource = musicLib.Songs.DefaultView;
        }
    }
}
