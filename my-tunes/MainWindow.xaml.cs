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

        public MainWindow()
        {
            InitializeComponent();
            LoadSongs();

            //this.playlistListBox.Items.Add("All Music");
            this.playlistListBox.Items.Clear();
            LoadPlaylists();
        }

        private void LoadPlaylists()
        {
            musicDataSet = new DataSet();
            musicDataSet.ReadXmlSchema("music.xsd");
            musicDataSet.ReadXml("music.xml");

            DataTable table = musicDataSet.Tables["playlist"];

            this.playlistListBox.ItemsSource = table.Rows;

        }

        private void LoadSongs()
        {
            musicDataSet = new DataSet();
            musicDataSet.ReadXmlSchema("music.xsd");
            musicDataSet.ReadXml("music.xml");

            DataTable table = musicDataSet.Tables["song"];

            songsDataGrid.ItemsSource = table.DefaultView;
        }
    }
}
