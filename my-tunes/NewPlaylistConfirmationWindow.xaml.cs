using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace my_tunes
{
    /// <summary>
    /// Interaction logic for newPlaylistConfirmation.xaml
    /// </summary>
    public partial class NewPlaylistConfirmationWindow : Window
    {
        private string newName;
        public string NewName
        {
            get
            {
                return newName;
            }
        }

        public NewPlaylistConfirmationWindow()
        {
            InitializeComponent();
            newName = "";
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void newNameTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            newName = newNameTextBox.Text;
        }
    }
}

