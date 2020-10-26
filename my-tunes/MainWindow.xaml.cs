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
            LoadPlaylists();
        }

        private void LoadPlaylists()
        {
            musicDataSet = new DataSet();
            musicDataSet.ReadXmlSchema("music.xsd");
            musicDataSet.ReadXml("music.xml");

            DataTable table = musicDataSet.Tables["playlist"];

            for(int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                this.playlistListBox.Items.Add(row["name"].ToString());
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            musicDataSet = new DataSet();
            musicDataSet.ReadXmlSchema("music.xsd");
            musicDataSet.ReadXml("music.xml");

            DataTable table = musicDataSet.Tables["song"];
            DataRow row = table.NewRow();
            row["title"] = this.titleTxtBox.Text;
            row["artist"] = this.artistTxtBox.Text;
            row["album"] = this.albumTxtBox.Text;
            row["genre"] = this.genreTxtBox.Text;
            row["length"] = this.lengthTxtBox.Text;
            row["filename"] = this.filenameTxtBox.Text;
            table.Rows.Add(row);

            musicDataSet.WriteXml("music.xml");

        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            DataTable table = musicDataSet.Tables["song"];
            DataRow row = table.Rows.Find(this.idTxtBox.Text);
            if (row != null)
                table.Rows.Remove(row);

            musicDataSet.WriteXml("music.xml");
        }
    }
}
